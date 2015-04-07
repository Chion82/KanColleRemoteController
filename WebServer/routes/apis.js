var express = require('express');
var router = express.Router();

var kcv_api = require('../controls/kcv_apis.js');
var operations = require('../controls/operations.js');
var config = require('../controls/config.js');
var auth = require('../auth/auth.js');

router.get('/gamedata/fleets', kcv_api.get_fleets_info);

router.get('/gamedata/admiral',kcv_api.get_admiral_info);

router.get('/gamedata/materials', kcv_api.get_materials_info);

router.get('/gamedata/repairingdocks', kcv_api.get_repairing_docks_info);

router.get('/gamedata/buildingdocks', kcv_api.get_building_docks_info);

router.get('/controller', operations.parse_action);

router.get('/config', config.get_conf_info);
router.post('/config', config.update_conf_files);

router.get('/auth', function(req,res,next) {
	if (auth.authenticate_password(req.query.pass)) {
		res.json({"status":"OK","message":"Password correct."});
	} else {
		res.json({"status":"error","message":"Password incorrect."});
	}
});

module.exports = router;
