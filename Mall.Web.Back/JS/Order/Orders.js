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
                    url: "",
                    Data: { "orderId": orderId },
                    async: false,
                    success: function (result) {
                        if (result == "True") {

                        }
                        else {

                        }
                    },
                    error: function () {
                        OpenTip("出错啦!", 2);
                        return false;
                    }

                });
                layer.msg("已发货", { icon: 1 });

            }, btn2: function () {
                layer.msg("已取消");
            }
        });
    }
})

function ConfirmDelivery(id) {
    alert(id);
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
    alert(orderId);
    //$.ajax({
    //    type: 'post',
    //    url: 'ModifyRemark',
    //    Data: { orderId,text },
    //    Datatype: text,
    //    success: function () {

    //    },
    //    error: function () {

    //    }
    //})
}