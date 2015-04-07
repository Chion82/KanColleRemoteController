var auth = require('../auth/auth.js');
var jsonFile = require('../models/json_file_def.js');
var fs = require('fs');

exports.get_fleets_info = function(req,res,next) {
	send_JSON_file_info(req,res,next,jsonFile.fleets_info);
}

exports.get_admiral_info = function(req,res,next) {
	send_JSON_file_info(req,res,next,jsonFile.admiral_info);
}

exports.get_materials_info = function(req,res,next) {
	send_JSON_file_info(req,res,next,jsonFile.materials_info);
}

exports.get_repairing_docks_info = function(req,res,next) {
	send_JSON_file_info(req,res,next,jsonFile.repairing_docks_info);
}

exports.get_building_docks_info = function(req,res,next) {
	send_JSON_file_info(req,res,next,jsonFile.building_docks_info);
}

function send_JSON_file_info(req,res,next,filePath) {
	if (!auth.auto_authenticate(req,res,next))
		return;
	try {
		var content = {};
		content.data = JSON.parse(fs.readFileSync(filePath, 'utf8'));
		content.status="OK";
		content.lastUpdateTime = parseInt(fs.statSync(filePath).mtime.getTime()/1000);
		res.json(content);
	} catch (err) {
		res.json({"status":"error", "message":"File open failed."});
	}
}

