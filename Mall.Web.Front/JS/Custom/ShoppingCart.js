
function SearchGoods() {
    var goodsName = $("#search-cart").val();

    if (goodsName == "" || /\s+/g.test(goodsName)) {
        Tip("商品名不能为空", "search-cart");
        return false;
    }

    $.ajax({
        type: 'GET',
        url: '/Custom/SearchGoodFromCart',
        data: { "searchName": goodsName },
        datatype: "html",
        success: function (html) {
            $("#shoppingCart-detail-goods").html(html);
        },
        error: function () {
            OpenTip("出错啦!", 2);
        },
    });
}

function BuyGood(event) {
    var goodsId = $(event).parent().parent().parent().attr("id");
    alert(goodsId);
    document.location.href = "/Order/ConfirmOrder/?goodsId=" + goodsId;
}

function DeletGoods(event) {
    var goodsId = $(event).parent().parent().parent().attr("id");
    alert(goodsId);
    $.ajax({
        type: 'POST',
        url: '/Custom/DeletGoodsFromCart',
        data: { "goodsId": goodsId},
        datatype: Boolean,
        success: function (result) {
            if (result == "True") {
                location.reload();
            }
        },
        error: function () {
            OpenTip("出错啦!", 2);
        },
    });
}

function BuyBySelected(event) {
    
}

function ReduceCount(event) {
    var count = $(event).parent().find("#count").val();
    var goodsId = $(event).parent().parent().parent().attr("id");
    if (count > 1) {
        $.ajax({
            type: 'GET',
            url: '/Custom/ModifyShoppingCart',
            data: { "goodsId": goodsId, "count": count },
            datatype: Boolean,
            success: function (result) {
                if (result == "True") {
                    count--;
                    $(event).parent().find("#count").val(count);
                }
            },
            error: function () {
                OpenTip("出错啦!", 2);
            },
        });
        return false;
    }
    OpenTip("数量不能少于一哦！", 1);
}

function IncreaseCount(event) {
    var count = $(event).parent().find("#count").val();
    var goodsId = $(event).parent().parent().val();
    count++;
    $.ajax({
        type: 'GET',
        url: '/Custom/ModifyShoppingCart',
        data: { "goodsId": goodsId, "count": count },
        datatype: Boolean,
        success: function (result) {
            if (result == "True") {
                $(event).parent().find("#count").val(count);
            }
        },
        error: function () {
            OpenTip("出错啦!", 2);
        },
    });
}