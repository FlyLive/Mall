
$(window).load(function(){
    $.ajax({
        type: 'Get',
        url: '/Custom/Wallet',
        success: function (result) {
            $(".personal-center .wallet h3").text("￥" + result);
        },
        error: function () {
        }
    });
});