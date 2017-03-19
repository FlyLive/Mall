function ConfirmReceipt(orderId) {
    var isCorrect = ConfirmPP();
    if(isCorrect != "True"){
        return false;
    }
    $.ajax({
        type: 'POST',
        url: '/Custom/ReceiptOrder',
        data: { "orderId": orderId },
        success: function (result) {
            if (result == "True") {
                OpenTipSuccess("处理成功", 2);
                location.href = "/Order/OrderDetails/?orderId=" + orderId;
            }
            else {
                OpenTip("请求未提交！",1);
            }
        },
        error: function () {
            OpenTip("出错啦！",1);
        }
    });
}