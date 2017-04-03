
$(".address-item").click(function () {
    var addressId = $(this).attr("id");
    var result = $.ajax({
        type: 'get',
        url: '/Custom/GetSelectedAddress',
        async: false,
        data: { "addressId": addressId },
        datatype: 'html',
        success: function (html) {
        },
        error: function () {
            OpenTip("出错啦!");
            return false;
        }
    });
    $(".address-item").removeClass("selected");
    $(this).addClass("selected");
    $("#confirm-address").html(result.responseText);
});

//购物车
function ConfirmOrderFromCart() {
    var addressId = $(".confirm-order-address-infor .selected").attr("id");

    var goods = $("li[name='goods']");
    var goodsIds = new Array();

    var remarksValue = new Array();
    for (var i = 0; i < goods.length; i++) {
        //获取商品Id
        goodsIds.push(goods[i].id);
        //获取备注
        remarksValue.push($(".goods-list #" + goodsIds[i] + " .custom-remark").val());
    }
    $.ajax({
        type: 'POST',
        url: '/Order/CreateOrderFromCart',
        data: { "goodsId": goodsIds, "deliveryAddressId": addressId, "customRemark": remarksValue },
        async: false,
        dataType: "json",
        success: function (orderId) {
            if (orderId != null) {
                ConfirmPPOfPay(orderId);
            }
        },
        error: function () {
            OpenTip("出错啦!", 2);
        }
    });
}

//立即购买
function ConfirmOrder() {
    var addressId = $(".confirm-order-address-infor .selected").attr("id");
    var goodsId = $(".goods-item").attr("id");
    var count = $("#count").val();
    var remark = $("#remark").val();

    $.ajax({
        type: 'POST',
        url: '/Order/CreateOrder',
        data: { "goodsId": goodsId, "deliveryAddressId": addressId, "customRemark": remark, "count": count },
        async: false,
        dataType: "json",
        success: function (orderId) {
            if (orderId != null) {
                ConfirmPPOfPay(orderId);
            }
        },
        error: function () {
            OpenTip("出错啦!", 2);
        }
    });
}

function ConfirmPPOfPay(orderId) {
    var newOrderId = orderId;
    layer.prompt({
        title: '输入支付密码，并确认',
        formType: 1,
        closeBtn: 2,
        cancel: function () {
            window.location.href = "/Order/AllOrders";
        },
        btn2: function () {
            window.location.href = "/Order/AllOrders";
        }
    }, function (pass, index) {
        layer.close(index);
        $.ajax({
            type: 'GET',
            url: '/Custom/ConfirmPP',
            data: { "pay_password": pass },
            success: function (result) {
                if (result == "True") {
                    PayNow(newOrderId);
                }
                else {
                    layer.confirm('密码错误！', {
                        btn: ['重试', '取消'] //按钮
                    }, function (index) {
                        layer.close(index);
                        ConfirmPPOfPay(newOrderId);
                    }, function () {
                        window.location.href = "/Order/AllOrders";
                    });
                }
            },
            error: function () {
                OpenTip("出错啦!", 3);
            }
        });
    });
}

function PayNow(orderId) {
    $.ajax({
        type: 'POST',
        url: '/Order/PayOrder',
        data: { "orderId": orderId },
        success: function (payResult) {
            if (payResult == "True") {
                window.location.href = "/Order/AllOrders";
            }
        },
        error: function () {
            OpenTip("出错啦!", 3);
        }
    });
}