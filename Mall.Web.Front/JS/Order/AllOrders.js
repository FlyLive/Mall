
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

function PayOrder(orderId) {
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

function ConfirmReceipt(orderId) {
    $.ajax({
        type: 'POST',
        url: '/Custom/ConfirmReceipt',
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