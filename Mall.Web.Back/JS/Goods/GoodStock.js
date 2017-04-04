function ModifyStock(goodsId) {
    layer.prompt({
        title: '输入新入库的数量，并确认(格式:+12)',
        formType: 0
    }, function (value, index) {
        if (value == "" || /\s+/g.test(value) || /^\d+$/.test(value)) {
            OpenTip("库存格式错误,请重试", 3);
            return false;
        }
        layer.close(index);
        $.ajax({
            type: 'Post',
            url: '/Goods/ModifyGoodsStock',
            data: { "goodsId": goodsId ,"count":value},
            success: function (result) {
                if (result == "True") {
                    OpenTipSuccess("修改成功!", 2);
                    location.reload();
                }
            },
            error: function () {
                OpenTip("出错啦!");
            }
        });
    });
}