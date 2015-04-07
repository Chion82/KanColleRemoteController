var fleetsApp = angular.module('fleets',[]);
fleetsApp.controller('fleetsController', function($scope, $http){
	$scope.fleets = [{fleetName:"Loading..."}];
	$scope.lastUpdateTime = 0;
	$scope.update = function() {
		$http.get('api/gamedata/fleets?pass=' + pass).success(function(response){
			if (response.status!="OK")
				return;
			$scope.fleets = [];
			for (fleet in response.data) {
				if (response.data[fleet] != null) {
					fleetData = response.data[fleet];
					view = response.data[fleet];
					if (fleet == 0) {
						view.active = "active";
						view.inActive = "in active";
					} else {
						view.active = "";
						view.inActive = "";
					}
					if (fleetData.isInSortie == true) {
						view.statusString = "In Sortie";
					} else if (fleetData.state.condition.isRejuvenating == true) {
						view.statusString = "Rejuvenating. Time remaining: " + secondsTimeSpanToHMS(fleetData.state.condition.remaining);
					} else if (fleetData.state.situation.search('Repairing') != -1) {
						view.statusString = "Repairing";
					} else {
						view.statusString = "Ready";
					}
					if (fleetData.expedition.isInExecution == true) {
						view.statusString = "Expedition in execution";
						view.expeditionString = fleetData.expedition.mission.missionId + " "
						+ fleetData.expedition.mission.title + " Time Remaining: " 
						+ secondsTimeSpanToHMS(fleetData.expedition.remaining);
					} else {
						view.expeditionString = "None";
					}
					view.lastUpdateString = new Date(response.lastUpdateTime*1000).toLocaleString();
					for (shipIndex in view.ships) {
						switch (view.ships[shipIndex].conditionType) {
						case "Brilliant":
							view.ships[shipIndex].conditionColor = "#00FF00";
							break;
						case "Normal":
							view.ships[shipIndex].conditionColor = "#0000FF";
							break;
						case "Tired":
							view.ships[shipIndex].conditionColor = "#FFFF00";
							break;
						case "OrangeTired":
							view.ships[shipIndex].conditionColor = "#FF6600";
							break;
						case "RedTired":
							view.ships[shipIndex].conditionColor = "#FF0000";
							break;
						}
					}
					$scope.fleets.push(view);
				}
			}
			$scope.lastUpdateTime = response.lastUpdateTime;
		});
	};
});

angular.bootstrap($("#fleets"),['fleets']);
