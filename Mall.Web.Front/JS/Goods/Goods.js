function active(event) {
    var element = $(".goods-detail-select > ul > li");
    element.removeClass("selected");
    
    event.classList.add("selected");

    var contents = $(".goods-detail > div");
    contents.removeClass("active");

    var content = document.getElementById(event.id + "-detail");
    content.classList.add("active");
}

function AddGoodsToCart() {
    var goodsId = $("#goodsId").val();
    var count = $("#count").val();

    $.ajax({
        type: 'POST',
        url: '/Custom/CreateShoppingCart',
        data: { "goodsId": goodsId, "count": count },
        success: function (result) {
            if (result == "True") {
                layer.meg("添加成功，去购物车中查看哦！");
            }
            else {
                layer.meg("添加失败！");
            }
        },
        error: function () {
            OpenTip("出错啦!", 2);
        }
    })
}