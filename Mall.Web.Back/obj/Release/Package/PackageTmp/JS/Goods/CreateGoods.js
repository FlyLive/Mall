
function CreateGoodsImg() {
    var name = $("#name").val();
    var count = $("#count").val();
    var price = $("#price").val();
    var detail = $("#detail").val();
    var freight = $("#freight").val();

    var author = $("#author").val();
    var press = $("#press").val();
    var publicationDate = $("#publicationDate").val();

    if (name == "" || /\s+/g.test(name)) {
        Tip("商品名不能为空", "name");
        return false;
    }

    if (count == "" || /\s+/g.test(count) || /^\d+$/.test(count)) {
        Tip("库存格式错误,请重试", "count");
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

    $.ajax({
        type: 'POST',
        url: '/Goods/CreateGoods',
        data: { "name": name, "count": count, "price": price, "detail": detail, "freight": freight, "author": author, "press": press, "publicationDate": publicationDate },
        datatype: 'json',
        success: function (goodsId) {
            $(function () {
                $('#createGoodsImgModal').modal({
                    keyboard: true
                });
            });
            $('#ssi-upload').ssi_uploader({
                url: "/Goods/CreateGoodsImg",
                data: { "goodsId": goodsId },
                maxFileSize: 5,
                allowed: ['jpg', 'gif', 'txt', 'png', 'pdf']
            });
        },
        error: function () {
            OpenTip("出错啦!", 1);
        }
    })
}