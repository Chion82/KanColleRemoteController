var expeditionSettingsApp = angular.module('expeditionSettings',[]);
expeditionSettingsApp.controller('expeditionSettingsController',function($scope,$http){
	$scope.confData = 'Loading...';
	$scope.update = function() {
		$http.get('api/config?action=get&conffile=expedition&pass='+pass).success(function(response){
			if (response.data.search('2') != -1) {
				$("#expedition_fleet2").prop('checked', true);
			} 
			if (response.data.search('3') != -1) {
				$("#expedition_fleet3").prop('checked', true);
			}
			if (response.data.search('4') != -1) {
				$("#expedition_fleet4").prop('checked', true);
			}
		});
		$http.get('api/config?action=get&conffile=scriptconf&pass='+pass).success(function(response){
			$scope.confData = response.data;
		});
	};
	$scope.saveConfigFile = function() {
		btn = $("#save_expedition_config_button").button('loading');
		$http.post('api/config?action=update&conffile=scriptconf&pass='+pass,
		{data:$scope.confData}
		).success(function(response){
			if (response.status=="OK") {
				$("#config_file_update_alert").show();
			} else {
				alert('Update Failed.');
			}
			btn.button('reset');
		});
	};
});

angular.bootstrap($("#expeditionsettings"),['expeditionSettings']);

$("#save_expedition_enabled_button").on('click',function(){
	var btn = $(this).button('loading');
	confStr = '';
	if ($("#expedition_fleet2").prop('checked') == true)
		confStr += '2';
	if ($("#expedition_fleet3").prop('checked') == true)
		confStr += '3';
	if ($("#expedition_fleet4").prop('checked') == true)
		confStr += '4';
	$.post('api/config?action=update&conffile=expedition&pass='+pass,
	{
		data : confStr
	},
	function(data, status){
		if (data.status=="OK") {
			$("#expedition_update_alert").show();
		} else {
			alert('Update Failed.');
		}
		btn.button('reset');
	});
});

$("#save_expedition_config_button").on('click',function(){

});
