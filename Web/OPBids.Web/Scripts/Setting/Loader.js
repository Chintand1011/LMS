$body = $("body");

$(document).on({
	ajaxStart: function () {
		$body.addClass("loading");


	},
	ajaxstop: function () {
		$body.removeClass("loading");
	}
});

//alert('Hello Workd');