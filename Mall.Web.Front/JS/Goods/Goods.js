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
                layer.msg("添加成功，去购物车中查看哦！");
            }
            else {
                layer.msg("亲,请登录！", {
                    time: 2000, //20s后自动关闭
                    btn: ['登录', '注册'],
                    btn1: function () {
                        location.href = "/Users/Index";
                    },
                    btn2: function () {
                        location.href = "/Users/Registe";
                    },
                });
            }
        },
        error: function () {
            OpenTip("出错啦!", 2);
        }
    })
}

function ReduceCount() {
    var count = $("#count").val();
    if (count > 1) {
        count--;
        $("#count").val(count);
        return true;
    }
    Tip("数量不能少于一哦！", "count");
}

function IncreaseCount() {
    var count = $("#count").val();
    count++;
    $("#count").val(count);
}