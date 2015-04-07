var PASS = 'admin';

var md5 = require('MD5');

exports.authentication_failed_str = '{"status":"error", "message":"Password incorrect."}';

var authenticate_password = function(password) {
	if (password == md5(md5(PASS))) {
		return true;
	} else {
		return false;
	}
}
exports.authenticate_password = authenticate_password;

exports.auto_authenticate = function(req,res,next) {
	if (!authenticate_password(req.query.pass)) {
		res.json({"status":"error", "message":"Password incorrect."});
		return false;
	} else {
		return true;
	}
}
