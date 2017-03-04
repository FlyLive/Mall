
$(".address-item").click(function () {
    $(".address-item").removeClass("selected");
    $(this).addClass("selected");
});

function ConfirmOrder() {
    var addressId = $(".confirm-order-address-infor .selected").attr("id");

    var goods = $("li[name='goods']");
    var goodsIds = new Array();

    var countsValue = new Array();

    var remarksValue = new Array();
    for (var i = 0; i < goods.length; i++) {
        //获取商品Id
        goodsIds.push(goods[i].id);
        //获取备注
        remarksValue.push($("#" + goodsIds[i] + " .custom-remark").val());
        //获取数量
        countsValue.push($("#" + goodsIds[i] + " .count").val());
    }
    for (var i = 0; i < goodsIds.length; i++) {
        alert(goodsIds[i]);
        alert(remarksValue[i]);
        alert(countsValue[i]);
    }
    $.ajax({
        type: 'POST',
        url: '/Order/CreateOrder',
        data: {"goodsId":goodsIds, "count":countsValue,"deliveryAddressId":addressId,"clientRemark":remarksValue},
        datatype: Boolean,
        async:false,
        success: function (result) {
            if (result == "True") {

            }
        },
        error: function () {
            OpenTip("出错啦!", 2);
        }
    });
}


function ReduceCount(event) {
    var count = $(event).parent().find(".count").val();
    if (count > 1) {
        count--;
        $(event).parent().find(".count").val(count);
    }
    else {
        OpenTip("数量不能少于一哦！", 1);
    }
}

function IncreaseCount(event) {
    var count = $(event).parent().find(".count").val();
    count++;
    $(event).parent().find(".count").val(count);
}