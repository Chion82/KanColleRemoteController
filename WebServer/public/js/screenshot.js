$("#captureBtn").on('click',function(){
	btn = $(this).button('loading');
	$.get('api/controller?action=capture&pass='+pass,function(data,status){
		if (data.status=="OK") {
			$("#screenshotImage").attr('src',data.url);
		}
		btn.button('reset');
	});
});
