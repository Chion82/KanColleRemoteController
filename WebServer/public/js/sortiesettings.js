var sortieSettingsApp = angular.module('sortieSettings',[]);
sortieSettingsApp.controller('sortieSettingsController',function($scope,$http){
	$scope.confData = 'Loading...';
	$scope.enabled = false;
	$scope.update = function() {
		$http.get('api/config?action=get&conffile=sortie&pass='+pass).success(function(response){
			if (response.data == '1') {
				$scope.enabled = true;
			} else {
				$scope.enabled = false;
			}
		});
		$http.get('api/config?action=get&conffile=scriptsortieconf&pass='+pass).success(function(response){
			$scope.confData = response.data;
		});
	};
	$scope.saveSortieEnabled = function() {
		btn = $("#save_sortie_enabled_button").button('loading');
		$http.post('api/config?action=update&conffile=sortie&pass='+pass,
			{data : $scope.enabled?'1':'0'}
		).success(function(response){
			if (response.status=="OK") {
				$("#sortie_update_alert").show();
			} else {
				alert('Update Failed.');
			}
			btn.button('reset');
		});
	};
	$scope.saveConfigFile = function() {
		btn = $("#save_sortie_config_button").button('loading');
		$http.post('api/config?action=update&conffile=scriptsortieconf&pass='+pass,
		{data:$scope.confData}
		).success(function(response){
			if (response.status=="OK") {
				$("#sortie_config_file_update_alert").show();
			} else {
				alert('Update Failed.');
			}
			btn.button('reset');
		});
	};
});

angular.bootstrap($("#sortiesettings"),['sortieSettings']);
