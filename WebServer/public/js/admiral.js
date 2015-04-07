var admiralApp = angular.module('admiral',[]);
admiralApp.controller('admiralController',function($scope,$http){
	$scope.admiral = {};
	$scope.admiral.nickname="Loading...";
	$scope.admiral.level="Loading...";
	$scope.admiral.rank="Loading...";
	$scope.admiral.experienceForNextLevel="Loading...";
	$scope.update = function() {
		$http.get('api/gamedata/admiral?pass=' + pass).success(function(response){
			if (response.status != "OK")
				return;
			$scope.admiral = response.data;
			$scope.admiral.sortieWinningRate = parseInt(response.data.sortieWinningRate * 100) + "%";
		});
	}
});

angular.bootstrap($("#admiral"),['admiral']);
