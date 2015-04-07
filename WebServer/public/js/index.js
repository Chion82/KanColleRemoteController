var app = angular.module('index', []);
app.controller('indexController',function($scope){
	$(".loginBtn").on('click',function(){
		var btn = $(this).button('loading');
		passencoded = $.md5($.md5($scope.pass));
		$.get('api/auth?pass=' + passencoded, function(response, status){
			if (response.status=='error') {
				$("#loginFailedModal").modal();
			} else if (response.status=="OK") {
				window.location.href="dashboard.html";
				$.cookie("pass",passencoded);
			}
			btn.button('reset');
		});
	});
});

