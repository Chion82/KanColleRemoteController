var pass;

pass = $.cookie("pass");
if (!pass || pass=="") {
	alert("Please login first.");
	window.location.href="index.html";
}

$("#logoutBtn").on('click', function(){
	$.removeCookie('pass');
	window.location.href="index.html";
});

$("#refreshBtn").on('click', function(){
	updateAllData();
});

function updateAllData() {
	$serverStatusScope = angular.element($("#serverStatusPanel")).scope();
	$serverStatusScope.update();

	$fleetsScope = angular.element($("#fleets")).scope();
	$fleetsScope.update();

	$admiralScope = angular.element($("#admiral")).scope();
	$admiralScope.update();

	$materialsScope = angular.element($("#materials")).scope();
	$materialsScope.update();

	$repairingDocksScope = angular.element($("#repairingdocks")).scope();
	$repairingDocksScope.update();

	$buildingDocksScope = angular.element($("#buildingdocks")).scope();
	$buildingDocksScope.update();

	$expeditionSettingsScope = angular.element($("#expeditionsettings")).scope();
	$expeditionSettingsScope.update();

	$sortieSettingsScope = angular.element($("#sortiesettings")).scope();
	$sortieSettingsScope.update();
}

$(document).ready(function(){
	updateAllData();
});
