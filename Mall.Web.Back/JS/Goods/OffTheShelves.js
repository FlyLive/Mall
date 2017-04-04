
function OffShelves(goodsId) {
    $.ajax({
        type: 'Post',
        url: '/Goods/OffTheShelves',
        data: { "goodsId": goodsId },
        success: function (result) {
            if (result == "True") {
                OpenTipSuccess("下架成功!", 3);
                $(".good-content ul li").remove("#" + goodsId);
            }
        },
        error: function () {
            OpenTip("出错啦!");
        }
    });
}