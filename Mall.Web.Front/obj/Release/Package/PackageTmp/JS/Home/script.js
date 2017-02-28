$(function () {
    if (/msie (6.0|7.0|8.0)/i.test(navigator.userAgent)) {
        document.write('您的浏览器版本过低，此页面无法正常显示，请先升级浏览器！');
    }
});

$(function () {
    // 动画
    $('.animation .select').on({
        'mouseenter': function () {
            $(this).addClass('active');
        },
        'mouseleave': function () {
            $(this).removeClass('active');
        }
    });

    var animationDOM = $('#animation')[0].outerHTML;
    $('#animation').terseBanner({
        arrow: true,
        animation: 'fade'
    });
});