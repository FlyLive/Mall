
function SearchGoods() {
    var goodsName = $("#search-cart").val();

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

function GetCheckedNodeId() {
    var selected = document.getElementsByClassName("item-check");
    var goodsIds = new Array();

    for (var i = 0; i < selected.length; i++) {
        if (selected[i].checked)
            goodsIds.push(selected[i].parentElement.parentElement.parentElement.id);
    }
    return goodsIds;
}

function GetChecked() {
    var goodsIds = GetCheckedNodeId();

    $("#select-count").text(goodsIds.length);

    if (goodsIds.length == 0) {
        $("#select-money").text("￥0.00");
        return false;
    }
    var money = GetMoney(goodsIds);
    $("#select-money").text("￥" + money);
}

function SelectAll() {
    var selectAll = document.getElementById("select-all");
    if (selectAll.checked) {
        $("input:checkbox[class='item-check']").each(function() {
            this.checked = true;
        });
    }
    else {
        $("input:checkbox[class='item-check']").each(function () {
            this.checked = false;
        });
    }
    GetChecked();
}

function BuyGood(event) {
    $(event).parent().parent().parent().find(".item-check").attr("checked", true);

    document.forms["confirmOrder"].submit();
}

function DeletGoods(event) {
    var goodsId = $(event).parent().parent().parent().attr("id");
    alert(goodsId);
    $.ajax({
        type: 'POST',
        url: '/Custom/DeletGoodsFromCart',
        data: { "goodsId": goodsId },
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

function BuyBySelected() {
    var goodsIds = GetCheckedNodeId();
    if (goodsIds.length == 0) {
        OpenTip("所选商品为空", 3);
        return false;
    }
    document.forms["confirmOrder"].submit();
}

function ReduceCount(event) {
    var count = $(event).parent().find("#count").val();
    var goodsId = $(event).parent().parent().parent().attr("id");
    if (count > 1) {
        count--;
        var result = ModifyCartNumber(goodsId, count);
        if (result) {
            var totla = GetMoney(goodsId);
            $(event).parent().find("#count").val(count);
            $(event).parent().parent().parent().find("#totla").text("￥" + totla);
            GetChecked();
            return true;
        }
    }
    OpenTip("数量不能少于一哦！", 1);
}

function GetMoney(goodsId) {
    var result = $.ajax({
        type: 'Post',
        url: '/Custom/GetSelectedMoney',
        async:false,
        data: { "goodIds": goodsId },
        success: function (result) {
            return result;
        },
        error: function () {
            OpenTip("出错啦!", 2);
        },
    });
    return result.responseText;
}

function IncreaseCount(event) {
    var count = $(event).parent().find("#count").val();
    var goodsId = $(event).parent().parent().parent().attr("id");
    count++;
    var result = ModifyCartNumber(goodsId, count);
    if (result) {
        var totla = GetMoney(goodsId);
        $(event).parent().find("#count").val(count);
        $(event).parent().parent().parent().find("#totla").text("￥" + totla);
        GetChecked();
    }
}

function ModifyCartNumber(goodsId, count) {
    var result = $.ajax({
        type: 'GET',
        url: '/Custom/ModifyShoppingCart',
        data: { "goodsId": goodsId, "count": count },
        datatype: Boolean,
        success: function (result) {
            if (result == "True") {
                return true;
            }
            return false;
        },
        error: function () {
            OpenTip("出错啦!", 2);
            return false;
        },
    });

    return result;
}