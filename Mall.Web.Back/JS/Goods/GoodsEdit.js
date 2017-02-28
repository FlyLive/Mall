function Search() {
    var search = $("#search-input").val();

    $.ajax({
        type: 'Get',
        url: 'Search',
        Data: search,
        Datatype: 'html',
        success: function (result) {
            $("#goods-menu").html(result);
        },
        error: function () {

        }
    });
}

function Modify(event) {
    var id = $(this).find("#id").val();
    alert(id);
    return false;
    $.ajax({
        type: 'Get',
        url: 'Search',
        Data: search,
        Datatype: 'html',
        success: function (result) {
            $("#goods-edit").html(result);
        },
        error: function () {

        }
    });
}

function ModifyGoods() {
    var name = $("#name").val();
    var price = $("#price").val();
    var detail = $("#detail").val();
    var freight = $("#freight").val();

    if (name == "" || /\s+/g.test(name)) {
        Tip("商品名不能为空", "name");
        return false;
    }

    if (price == "" || /\s+/g.test(price) || /^\d+(\.\d+)?$/.test(price)) {
        Tip("价格格式错误,请重试", "price");
        return false;
    }

    if (detail == "" || /\s+/g.test(detail)) {
        Tip("商品详情不能为空", "detail");
        return false;
    }

    if (!(freight == "" || /\s+/g.test(freight))) {
        if (/^\d+(\.\d+)?$/.test(freight)) {
            Tip("运费格式错误,请重试", "freight");
            return false;
        }
    }
    document.forms[""].submit();
}