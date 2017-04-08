function Init(orderId) {
    $(function () {
        $('#manageConfirmModal').modal({
            keyboard: true
        })
    });
    $("#orderId").val(orderId);
}

//接受订单
function AcceptOrder(id) {
    Init(id);
    $("#manageConfirm").click(function () {
        if (ManageConfirm() == "True") {
            $('#manageConfirmModal').modal('hide');
            $("#managePassword").val("");

            var orderId = $("#orderId").val();
            layer.msg('接受订单？', {
                time: 20000, //20s后自动关闭
                btn: ['确认', '再等一下'],
                btn1: function () {
                    $.ajax({
                        type: "POST",
                        url: "/Order/AcceptOrder",
                        data: { "orderId": orderId },
                        async: false,
                        success: function (result) {
                            if (result == "True") {
                                OpenTipSuccess("已接受订单，等待发货!", 2);
                                $("#" + orderId).remove();
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
            $("#manageConfirm").unbind();
        }
    });
}

//同意发货
function ConfirmDelivery(id) {
    Init(id);
    $("#manageConfirm").click(function () {
        if (ManageConfirm() == "True") {
            $('#manageConfirmModal').modal('hide');
            $("#managePassword").val("");

            var orderId = $("#orderId").val();
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
                                $("#" + orderId).remove();
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
            $("#manageConfirm").unbind();
        }
    });
}

//同意退款
function AcceptRefund(id) {
    Init(id);
    $("#manageConfirm").click(function () {
        if (ManageConfirm() == "True") {
            $('#manageConfirmModal').modal('hide');
            $("#managePassword").val("");

            layer.msg('同意退款？', {
                time: 20000, //20s后自动关闭
                btn: ['确认', '再等一下'],
                btn1: function () {
                    $.ajax({
                        type: "POST",
                        url: "/Order/AcceptRefund",
                        data: { "orderId": id },
                        async: false,
                        success: function (result) {
                            if (result == "True") {
                                OpenTipSuccess("已同意退款!", 2);
                                $("#" + id).remove();
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
            $("#manageConfirm").unbind();
        }
    });
}

//确认退货成功
function ConfirmReturn(id) {
    Init(id);
    $("#manageConfirm").click(function () {
        if (ManageConfirm() == "True") {
            $('#manageConfirmModal').modal('hide');
            $("#managePassword").val("");

            layer.msg('同意退货？', {
                time: 20000, //20s后自动关闭
                btn: ['同意退货', '再等一下'],
                btn1: function () {
                    $.ajax({
                        type: "POST",
                        url: "/Order/ConfirmReturn",
                        data: { "orderId": id },
                        async: false,
                        success: function (result) {
                            if (result == "True") {
                                OpenTipSuccess("已成功退货!", 2);
                                $("#" + id).remove();
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
            $("#manageConfirm").unbind();
        }
    });
}

//同意退货
function AcceptReturn(id) {
    Init(id);
    $("#manageConfirm").click(function () {
        if (ManageConfirm() == "True") {
            $('#manageConfirmModal').modal('hide');
            $("#managePassword").val("");

            layer.msg('同意退货？', {
                time: 20000, //20s后自动关闭
                btn: ['同意退货', '再等一下'],
                btn1: function () {
                    $.ajax({
                        type: "POST",
                        url: "/Order/AcceptReturn",
                        data: { "orderId": id },
                        async: false,
                        success: function (result) {
                            if (result == "True") {
                                OpenTipSuccess("已同意退货!", 2);
                                $("#" + id).remove();
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
            $("#manageConfirm").unbind();
        }
    });
}

//拒绝退货
function RefuseReturn(id) {
    Init(id);
    $("#manageConfirm").click(function () {
        if (ManageConfirm() == "True") {
            $('#manageConfirmModal').modal('hide');
            $("#managePassword").val("");

            layer.msg('同意退货？', {
                time: 20000, //20s后自动关闭
                btn: ['拒绝退货', '再等一下'],
                btn1: function () {
                    $.ajax({
                        type: "POST",
                        url: "/Order/RefuseReturn",
                        data: { "orderId": id },
                        async: false,
                        success: function (result) {
                            if (result == "True") {
                                OpenTipSuccess("已同意退货!", 2);
                                $("#" + id).remove();
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
            $("#manageConfirm").unbind();
        }
    });
}

//修改评价
function ModifyRemark(id) {
    $(function () {
        $('#modifyRemarkModal').modal({
            keyboard: true
        })
    });
    $("#orderId").val(id);
}

//提交修改备注
function SubmitRemark() {
    $('#modifyRemarkModal').modal('hide');
    var text = $("#modify_remark").val();
    var orderId = $("#orderId").val();
    Init(orderId);
    $("#manageConfirm").click(function () {
        if (ManageConfirm() == "True") {
            $('#manageConfirmModal').modal('hide');
            $("#managePassword").val("");

            layer.msg('确认修改？', {
                time: 20000, //20s后自动关闭
                btn: ['确认', '再等一下'],
                btn1: function () {
                    $.ajax({
                        type: 'POST',
                        url: '/Order/ModifyRemark',
                        data: { "orderId": orderId, "mark": text },
                        success: function (result) {
                            if (result == "True") {
                                OpenTipSuccess("已修改!", 2);
                                $("#" + id + "#order-remark").text(text);
                            }
                            else {
                                OpenTip("请求未能提交!", 1);
                            }
                        },
                        error: function () {
                            OpenTip("出错啦!", 2);
                        }
                    });
                }, btn2: function () {
                    layer.msg("已取消");
                }
            });
            $("#manageConfirm").unbind();
        }
    });
}