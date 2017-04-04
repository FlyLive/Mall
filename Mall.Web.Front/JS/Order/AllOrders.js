
function SearchOrderByState(event) {
    var state = event.id;
    $.ajax({
        type: 'GET',
        url: 'SearchOrderByState',
        data: { "orderState": state },
        datatype: 'html',
        success: function (Data) {
            $("#order-list").html(Data);
        },
        error: function () {
            OpenTip("出错啦!", 2);
        }
    });
}

function SearchOrderId() {
    var orderId = $("#orderId").val();
    if (!/^[0-9a-f]{8}(-[0-9a-f]{4}){3}-[0-9a-f]{12}$/.test(orderId)) {
        Tip("请输入正确的订单号","orderId");
        return false;
    }
    $.ajax({
        type: 'GET',
        url: 'SearchOrderId',
        data: { "orderId": orderId },
        datatype: 'html',
        success: function (Data) {
            $("#order-list").html(Data);
        },
        error: function () {
            OpenTip("出错啦!", 2);
        }
    });
}

function ApplyRefund(orderId) {
    $.ajax({
        type: 'Post',
        url: '/Custom/ApplyRefund',
        data: { "orderId": orderId },
        datatype: 'html',
        success: function (result) {
            if (result == "True") {
                OpenTipSuccess("申请成功，等待处理!", 1);
                location.reload();
            }
            else {
                OpenTip("申请失败!", 1);
            }
        },
        error: function () {
            OpenTip("出错啦!", 2);
        }
    })
}

function ApplyReturn(orderId) {
    $.ajax({
        type: 'Post',
        url: '/Custom/ApplReturn',
        data: { "orderId": orderId },
        success: function (result) {
            if (result == "True") {
                OpenTipSuccess("申请成功，等待处理!", 1);
                location.reload();
            }
            else {
                OpenTip("申请失败!", 1);
            }
        },
        error: function () {
            OpenTip("出错啦!", 2);
        }
    })
}