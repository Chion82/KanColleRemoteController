$("#restartBtn").on('click',function(){
	btn = $(this).button('loading');
	$.get('api/controller?action=restart&pass='+pass, function(data,status){
		if (data.status=='OK') {
			$('#restartCompleteAlert').show();
		}
		btn.button('reset');
		updateAllData();
	});
});

$("#stopBtn").on('click',function(){
	btn = $(this).button('loading');
	$.get('api/controller?action=stop&pass='+pass, function(data,status){
		if (data.status=='OK') {
			$('#stopCompleteAlert').show();
		}
		btn.button('reset');
		updateAllData();
	});
});
