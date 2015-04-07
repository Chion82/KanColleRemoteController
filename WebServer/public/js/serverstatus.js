var serverStatusApp = angular.module('serverStatus',[]);
serverStatusApp.controller('serverStatusController', function($scope, $http){
	$scope.runningStatus = "Loading...";
	$scope.serverDomain = window.location.host;
	$scope.uptime = "Loading...";
	$scope.cpuLoad = "Loading...";
	$scope.mem = "Loading...";
	$scope.update = function() {
		$http.get('api/controller?pass=' + pass + '&action=getstatus')
			.success(function(response){
				$scope.runningStatus = response.status;
				$scope.uptime = secondsTimeSpanToHMS(response.uptime);
				$scope.cpuLoad = response.cpuLoad + "%";
				$scope.mem = parseInt((response.totalmem - response.freemem)/1024/1024) + 'MB used, ' + parseInt(response.freemem/1024/1024) + 'MB free / ' + parseInt(response.totalmem/1024/1024) + 'MB total'
			});
	}
});

//angular.bootstrap($("#serverStatusPanel"),['serverStatus']);
