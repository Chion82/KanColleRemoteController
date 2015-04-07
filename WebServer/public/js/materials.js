var materialsApp = angular.module('materials',[]);
materialsApp.controller('materialsController',function($scope,$http){
	$scope.fuel = "Loading...";
	$scope.ammunition = "Loading...";
	$scope.steel = "Loading...";
	$scope.bauxite = "Loading...";
	$scope.developmentMaterials = "Loading...";
	$scope.instantRepairMaterials = "Loading...";
	$scope.instantBuildMaterials = "Loading...";
	$scope.update = function() {
		$http.get('api/gamedata/materials?pass=' + pass).success(function(response){
			if (response.status!="OK")
				return;
			$scope.fuel = response.data.fuel;
			$scope.ammunition = response.data.ammunition;
			$scope.steel = response.data.steel;
			$scope.bauxite = response.data.bauxite;
			$scope.developmentMaterials = response.data.developmentMaterials;
			$scope.instantRepairMaterials = response.data.instantRepairMaterials;
			$scope.instantBuildMaterials = response.data.instantBuildMaterials;
		});
	};
});

angular.bootstrap($("#materials"),['materials']);
