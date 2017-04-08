$(window).scroll(function () {
        var search = document.getElementById("search-flip");
    if ($(window).scrollTop() >= 72) {
        search.style.display = "block";
    }
    else {
        search.style.display = "none";
    }
});

$(document).load(function () {
    $.ajax({
        type: 'Get',
        url: '/Custom/GetCartNumber',
        success: function (result) {
            $(".message-menu .label-info").text(result);
            $(".personal-center .cart h3").text(result);
        },
        error: function () {
        }
    })

    $.ajax({
        type: 'Get',
        url: '/Order/GetUnfinishedNumber',
        success: function (result) {
            $(".message-menu .label-default").text(result);
            $(".personal-center .unfinish h3").text(result);
        },
        error: function () {
        }
    })
})