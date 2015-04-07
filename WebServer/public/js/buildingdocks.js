var buildingDocksApp = angular.module('buildingDocks',[]);
buildingDocksApp.controller('buildingDocksController',function($scope,$http){
	$scope.docks = [{'state':'Loading...'}];
	$scope.update = function() {
		$http.get('api/gamedata/buildingdocks?pass=' + pass).success(function(response){
			$scope.docks = [];
			for (dockIndex in response.data) {
				view = response.data[dockIndex]
				if (view!=null) {
					view.remainingString = secondsTimeSpanToHMS(view.remaining);
					if (view.ship=="") {
						view.ship = {};
						view.ship.HP = "-";
						view.ship.name = "-";
						view.ship.shipType = "-";
						view.ship.HP = "-";
						view.ship.speed = "-";
						view.remainingString = "-";
					}
					$scope.docks.push(view);
				}
			}
		});
	}
});

angular.bootstrap($('#buildingdocks'),['buildingDocks']);



