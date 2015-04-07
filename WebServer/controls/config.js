var auth = require('../auth/auth.js');
var fs = require('fs');
var confFile = require('../models/conf_file_def.js');
var iconv = require('iconv-lite');

exports.update_conf_files = function(req,res,next) {
	if (!auth.auto_authenticate(req,res,next))
		return;
	if (req.query.action!='update') {
		res.json({"status":"error","message":"Action not specified"});
		return;
	}
	content = {};
	data = req.body.data;
	switch(req.query.conffile) {
		case 'expedition':
			fs.writeFileSync(confFile.expedition_conf,data,'utf8');
			break;
		case 'sortie':
			fs.writeFileSync(confFile.sortie_conf,data,'utf8');
			break;
		case 'scriptconf':
			data = iconv.encode(data, 'GBK');
			fs.writeFileSync(confFile.script_conf,data);
			break;
		case 'scriptsortieconf':
			data = iconv.encode(data, 'GBK');
			fs.writeFileSync(confFile.script_sortie_conf,data);
			break;
		default:
			res.json({"status":"error","message":"Config file not found."});
			return;
	}
	content.status = 'OK';
	res.json(content);
}

exports.get_conf_info = function(req,res,next) {
	if (!auth.auto_authenticate(req,res,next))
		return;
	if (req.query.action!='get') {
		res.json({"status":"error","message":"Action not specified"});
		return;
	}
	content = {};
	data = "";
	switch(req.query.conffile) {
		case 'expedition':
			data = fs.readFileSync(confFile.expedition_conf,'utf8');
			break;
		case 'sortie':
			data = fs.readFileSync(confFile.sortie_conf,'utf8');
			break;
		case 'scriptconf':
			data = fs.readFileSync(confFile.script_conf);
			data = iconv.decode(data, 'GBK'); 
			break;
		case 'scriptsortieconf':
			data = fs.readFileSync(confFile.script_sortie_conf);
			data = iconv.decode(data, 'GBK'); 
			break;
		default:
			res.json({"status":"error","message":"Config file not found."});
			return;
	}
	content.data = data;
	content.status = 'OK';
	res.json(content);
}
