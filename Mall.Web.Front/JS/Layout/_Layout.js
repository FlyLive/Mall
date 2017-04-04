$(window).scroll(function () {
        var search = document.getElementById("search-flip");
    if ($(window).scrollTop() >= 72) {
        search.style.display = "block";
    }
    else {
        search.style.display = "none";
    }
});

