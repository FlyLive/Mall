function ConfirmLP() {
    var logPassword = $("#input_log_password").val();
    if (logPassword == "" || /\s+/g.test(logPassword)) {
        Tip("密码不能为空，请重试!", "input_log_password");
        return false;
    }
    else {
        $.ajax({
            type: 'Get',
            url: '/Custom/ConfirmLP',
            data: { "log_password": logPassword },
            datatype: Boolean,
            success: function (result) {
                if (result == 'True') {
                    $(function () {
                        $('#modifyLPModal').modal({
                            keyboard: true
                        })
                    });
                }
                else {
                    OpenTip("登录密码错误，请重试!", 3);
                }
            },
            error: function () {
                OpenTip("出错啦!", 2);
            }
        });
    }
}

function ConfirmPP() {
    var payPassword = $("#input_pay_password").val();
    if (payPassword == "" || /\s+/g.test(payPassword)) {
        Tip("密码不能为空，请重试!", "input_pay_password");
        return false;
    }
    else {
        $.ajax({
            type: "Get",
            url: "/Custom/ConfirmMP",
            data: { "pay_password": payPassword },
            datatype: Boolean,
            success: function (result) {
                if (result == "True") {
                    $(function () {
                        $('#modifyPPModal').modal({
                            keyboard: true
                        })
                    });
                }
                else {
                    OpenTip("支付密码错误，请重试!");
                }
            },
            error: function () {
                OpenTip("出错啦!");
            }
        });
    }
}

//修改登录密码
function ChangeLP() {
    var firstLP = $("#log_password").val();
    var secondeLP = $("#re_log_password").val();

    if (ConfirmPassword(firstLP, secondeLP)) {
        if (firstLP.length < 6 || firstLP.length > 12) {
            layer.tips('密码长度为6-12位,请重试!', "#log_password", {
                tip: [2, "#2277ff"],
                time: 1500,
            });
            return false;
        }
        document.forms["modifyLP"].submit();
    }
    return false;
}

//修改支付密码
function ChangePP() {
    var oldPP = $("#old_pay_password").val();
    var firstPP = $("#pay_password").val();
    var secondePP = $("#re_pay_password").val();

    if (Confirm(oldPP,firstPP, secondePP)) {
        $.ajax({
            type: "Get",
            url: "/Custom/ModifyPP",
            data: { "pay_password": firstPP },
            datatype: Boolean,
            success: function (result) {
                if (result == "True") {
                    $(function () {
                        $('#modifyPPModal').modal('hide');
                    });
                }
                else {
                    OpenTip("修改失败!");
                }
            },
            error: function () {
                OpenTip("出错啦!");
            }
        });
    }
    return false;
}

//验证密码
function ConfirmPassword(first, second) {
    if (first == "" || /\s+/g.test(first)) {
        layer.open({
            title: '错误提示',
            content: '密码不能为空，请重试!',
        });
        return false;
    }
    else if (first != second) {
        layer.open({
            title: '错误提示',
            content: '两次密码输入不一致，请重试!',
        });
        return false;
    }
    return true;
}