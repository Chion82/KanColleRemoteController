var fs = require('fs');
var process = require('child_process');
var auth = require('../auth/auth.js');
var path = require('path');
var os = require('os');

exports.parse_action = function(req,res,next) {
	if (!auth.auto_authenticate(req,res,next))
		return;
	if (req.query.action=='restart')
		action_restart(req,res,next);
	else if (req.query.action=='stop')
		action_stop(req,res,next);
	else if (req.query.action=='getstatus')
		action_get_status(req,res,next);
	else if (req.query.action=='capture')
		action_capture(req,res,next);
	else
		res.json({"status":"error","message":"Action not specified"});
}

var action_restart = function(req,res,next) {
	fs.writeFileSync('isrunning','1');
	res.json({"status":"OK"});
	
	exePath = get_controller_path();
	process.exec('taskkill /f /im KanColleRemoteController.exe');
	
	setTimeout(function() {
		process.exec(exePath + " quitall", function(err, stdout, stderr){
			setTimeout(function() {
				process.exec(exePath + " runall", {cwd: get_root_directory() }, function(err, stdout, stderr){

				});
			}, 5000);
		});
	}, 2000);

}

var action_stop = function(req,res,next) {
	fs.writeFileSync('isrunning','0');
	res.json({"status":"OK"});
	exePath = get_controller_path();
	
	process.exec('taskkill /f /im KanColleRemoteController.exe');

	setTimeout(function() {
		process.exec(exePath + " quitall", function(err, stdout, stderr){

		});
	}, 2000);
}

var action_get_status = function(req,res,next) {
	content = {};
	content.status = fs.readFileSync('isrunning','utf8')=="1"?"running":"stopped";
	if (content.status=="running") {
		content.uptime = new Date().getTime()/1000 - parseInt(fs.statSync('isrunning').mtime.getTime()/1000);
	} else {
		content.uptime = 0;
	}
	cpuLoad = process.execSync('wmic cpu get loadpercentage',{encoding:'utf8'});
	cpuLoad = (cpuLoad.split('\n'))[1];
	content.cpuLoad = parseInt(cpuLoad.substr(0,3));
	content.totalmem = os.totalmem();
	content.freemem = os.freemem();
	res.json(content);
}

var action_capture = function(req,res,next) {
	root = get_root_directory();
	filename = (parseInt(Math.random() * (9999999999-1000000000) + 1000000000)) + '.bmp';
	process.exec(get_controller_path() + " capture", {cwd: get_root_directory() }, function(err, stdout, stderr){
		try {
			//fs.createReadStream(root + '\\capture.bmp').pipe(fs.createWriteStream(root + '\\WebServer\\public\\capture\\' + filename));
			deleteFilesInFolder(root + '\\WebServer\\public\\capture\\');
			fs.createReadStream(root + '\\capture.bmp').pipe(fs.createWriteStream(root + '\\WebServer\\public\\capture\\' + filename));
			setTimeout(function(){
				res.json({"status":"OK","url":"capture/" + filename });
			}, 500);
		} catch (err) {
			console.log(err);
			res.json({"status":"error"});
		}
	});
}

function get_controller_path() {
	pathArray = __dirname.split('\\');
	pathArray.splice(-1,1);
	pathArray.splice(-1,1);
	rootDirectory = pathArray.join('\\');
	return path.join(rootDirectory,"KanColleRemoteController.exe");
}

function get_root_directory() {
	pathArray = __dirname.split('\\');
	pathArray.splice(-1,1);
	pathArray.splice(-1,1);
	return pathArray.join('\\');
}

var deleteFilesInFolder = function(path) {
  if( fs.existsSync(path) ) {
    fs.readdirSync(path).forEach(function(file,index){
      var curPath = path + "/" + file;
      fs.unlinkSync(curPath);
    });
  }
};
