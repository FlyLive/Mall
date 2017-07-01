
function OnShelves(goodsId) {
    $.ajax({
        type: 'Post',
        url: '/Goods/OnTheShelves',
        data: { "goodsId": goodsId },
        success: function (result) {
            if (result == "True") {
                OpenTipSuccess("上架成功!", 3);
                $(".good-content ul li").remove("#" + goodsId);
            }
            else {
                OpenTip("上架失败,可能原因是库存不足!");
            }
        },
        error: function () {
            OpenTip("出错啦!");
        }
    });
}
$(".good-wrapper > div").css({ "width": "950px", "margin": "0 auto" });