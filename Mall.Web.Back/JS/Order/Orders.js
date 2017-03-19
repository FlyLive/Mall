$("#manageConfirm").click(function () {
    if (ManageConfirm() == "True") {
        $('#manageConfirmModal').modal('hide');
        $("#managePassword").val("");

        var orderId = $("#orderId").val();
        //layer.msg('接受订单？', {
        //    time: 20000, //20s后自动关闭
        //    btn: ['确认发货', '再等一下'],
        //    btn1: function () {
        //        $.ajax({
        //            type: "POST",
        //            url: "/Order/AcceptOrder",
        //            data: { "orderId": orderId },
        //            async: false,
        //            success: function (result) {
        //                if (result == "True") {
        //                    OpenTipSuccess("已接受订单，等待发货!", 2);
        //                    location.reload();
        //                }
        //                else {
        //                    OpenTip("请求未能提交!", 1);
        //                }
        //            },
        //            error: function () {
        //                OpenTip("出错啦!", 2);
        //                return false;
        //            }
        //        });
        //    }, btn2: function () {
        //        layer.msg("已取消");
        //    }
        //});
        layer.msg('确认发货？', {
            time: 20000, //20s后自动关闭
            btn: ['确认发货', '再等一下'],
            btn1: function () {
                $.ajax({
                    type: "POST",
                    url: "/Order/DeliveryOrder",
                    data: { "orderId": orderId },
                    async: false,
                    success: function (result) {
                        if (result == "True") {
                            OpenTipSuccess("已发货!", 2);
                            location.reload();
                        }
                        else {
                            OpenTip("请求未能提交!", 1);
                        }
                    },
                    error: function () {
                        OpenTip("出错啦!", 2);
                        return false;
                    }
                });

            }, btn2: function () {
                layer.msg("已取消");
            }
        });
        //layer.msg('确认发货？', {
        //    time: 20000, //20s后自动关闭
        //    btn: ['确认发货', '再等一下'],
        //    btn1: function () {
        //        $.ajax({
        //            type: "POST",
        //            url: "/Order/DeliveryOrder",
        //            data: { "orderId": orderId },
        //            async: false,
        //            success: function (result) {
        //                if (result == "True") {
        //                    OpenTipSuccess("已发货!", 2);
        //                    location.reload();
        //                }
        //                else {
        //                    OpenTip("请求未能提交!", 1);
        //                }
        //            },
        //            error: function () {
        //                OpenTip("出错啦!", 2);
        //                return false;
        //            }
        //        });
        //    }, btn2: function () {
        //        layer.msg("已取消");
        //    }
        //});
    }
})

function AcceptOrder(id) {
    $(function () {
        $('#manageConfirmModal').modal({
            keyboard: true
        })
    });
    $("#orderId").val(id);
}

function ConfirmDelivery(id) {
    $(function () {
        $('#manageConfirmModal').modal({
            keyboard: true
        })
    });
    $("#orderId").val(id);
}
function ModifyRemark(id) {
    $(function () {
        $('#modifyRemarkModal').modal({
            keyboard: true
        })
    });
    $("#orderId").val(id);
}
function SubmitRemark() {
    var text = $("#modify_remark").val();
    var orderId = $("#orderId").val();
    $.ajax({
        type: 'POST',
        url: '/Order/ModifyRemark',
        data: { "orderId": orderId, "mark": text },
        success: function () {
            if (result == "True") {
                OpenTipSuccess("已修改!", 2);
                location.reload();
            }
            else {
                OpenTip("请求未能提交!", 1);
            }
        },
        error: function () {
            OpenTip("出错啦!", 2);
        }
    })
}