var repairingDocksApp = angular.module('repairingDocks',[]);
repairingDocksApp.controller('repairingDocksController',function($scope,$http){
	$scope.docks = [{'state':'Loading...'}];
	$scope.update = function() {
		$http.get('api/gamedata/repairingdocks?pass=' + pass).success(function(response){
			$scope.docks = [];
			for (dockIndex in response.data) {
				view = response.data[dockIndex]
				if (view!=null) {
					view.remainingString = secondsTimeSpanToHMS(view.remaining);
					if (view.ship=="") {
						view.ship = {};
						view.ship.info = {};
						view.ship.HP = {};
						view.ship.info.name = "-";
						view.ship.info.shipType = "-";
						view.ship.HP.current = "-";
						view.ship.HP.max = "-";
						view.remainingString = "-";
					}
					$scope.docks.push(view);
				}
			}
		});
	}
});

angular.bootstrap($('#repairingdocks'),['repairingDocks']);

