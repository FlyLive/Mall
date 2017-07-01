function Search() {
    var search = $("#search-input").val();

    $.ajax({
        type: 'Get',
        url: '/Goods/Search',
        data: { "search": search },
        datatype: 'html',
        success: function (result) {
            $("#goods-menu").html(result);
            var goodsId = $(".goods-menu .selected").get(0).id;
            GetGoodsInfo(goodsId);
            GetGoodsImg(goodsId);
        },
        error: function () {
            OpenTip("出错啦!");
        }
    });
}

function InitGood(event) {
    $(".goods-menu-item").removeClass("selected");
    $(event).addClass("selected");
    var goodsId = $(event).attr("id");
    GetGoodsInfo(goodsId);
    GetGoodsImg(goodsId);
}

function DeletGoodsImages() {
    var imgs = $(".img-list .selected");
    var imgIds = new Array();
    for (var i = 0; i < imgs.length; i++) {
        imgIds.push(imgs.get(i).id);
    }
    $.ajax({
        type: "POST",
        url: '/Goods/DeletGoodsImgs',
        data: { "imageIds": imgIds },
        success: function () {
            OpenTipSuccess("修改成功");
            $(".img-list .selected").remove();
        },
        error: function () {
            OpenTip("出错啦!");
        }
    });
}

function GetGoodsInfo(goodsId) {
    $.ajax({
        type: "GET",
        url: '/Goods/GoodsInfoDetails',
        data: { "goodsId": goodsId },
        datatype: 'html',
        success: function (html) {
            $("#goods-edit-info").html(html);
        },
        error: function () {
            OpenTip("出错啦!");
        }
    });
}

function GetGoodsImg(goodsId) {
    $.ajax({
        type: "GET",
        url: '/Goods/GoodsImgList',
        data: { "goodsId": goodsId },
        datatype: 'html',
        success: function (html) {
            $("#img").html(html);
            $('#ssi-upload').ssi_uploader({
                url: "/Goods/CreateGoodsImg",
                data: { "goodsId": goodsId },
                maxFileSize: 5,
                allowed: ['jpg', 'gif', 'txt', 'png', 'pdf'],
            });
        },
        error: function () {
            OpenTip("出错啦!");
        }
    });
}

function ModifyGoods() {
    var id = $(".goods-menu .selected").first().attr("id");
    var name = $("#name").val();
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
        type: "POST",
        url: "GoodsEdit",
        data: { "goodsId": id, "name": name, "price": price, "detail": detail, "freight": freight, "author": author, "press": press, "publicationDate": publicationDate },
        success: function (result) {
            if (result == "True") {
                OpenTipSuccess("修改成功");
            }
        },
        error: function () {
            OpenTip("出错啦!", 1);
        }
    })
}