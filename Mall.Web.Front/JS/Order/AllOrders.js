
function SearchOrderByState(event) {
    var state = event.id;
    $.ajax({
        type: 'GET',
        url: 'SearchOrderByState',
        Data: { "orderState": state },
        Datatype: 'html',
        success: function (Data) {
            $("#order-list").html(Data);
        },
        error: function () {
            OpenTip("出错啦!", 2);
        }
    })
}

function SearchOrderId() {
    var orderId = $("#orderId").val();
    if (!/^\w{8}-(\w{4}-){3}\w{12}$/.test(orderId)) {
        Tip("请输入正确的订单号","orderId");
        return false;
    }
    $.ajax({
        type: 'GET',
        url: 'SearchOrderId',
        Data: { "orderId": orderId },
        Datatype: 'html',
        success: function (Data) {
            $("#order-list").html(Data);
        },
        error: function () {
            OpenTip("出错啦!", 2);
        }
    })
}