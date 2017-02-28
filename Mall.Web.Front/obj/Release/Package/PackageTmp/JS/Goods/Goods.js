function active(event) {
    var element = $(".goods-detail-select > ul > li");
    element.removeClass("selected");
    
    event.classList.add("selected");

    var contents = $(".goods-detail > div");
    contents.removeClass("active");

    var content = document.getElementById(event.id + "-detail");
    content.classList.add("active");
}