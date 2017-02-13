function ClientConfirm() {
    var account = $("#account").val();
    var email = $("#email").val();

    if (account == null || account.substring(0, account.length) == 0) {
        layer.tips("账户不能为空!", "#account", {
            tip: [2, "#2277ff"],
            time: 1500,
        });
        return false;
    }
    if (!/^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/.test(email)) {
        layer.tips("邮箱格式不正确!", "#email", {
            tip: [2, "#2277ff"],
            time: 1500,
        });
        return false;
    }

    $.ajax({
        type: "Get",
        url: "ClientConfirm",
        data: { account, email},
        datatype: Boolean,
        success: function (data) {
            if (data == "True") {
                $(function () {
                    $('#ReSetPWModal').modal({
                        keyboard: true
                    })
                });
            }
            else {
                layer.open({
                    title: "错误提示",
                    content: "账户不存在或邮箱错误,请重试!",
                });
                return false;
            }
        },
        error: function () {
            layer.open({
                title: "错误提示",
                content: "出错啦!",
                icon: 5,
            });
            return false;
        }
    })
}

function ReSetPW() {
    var fPassword = $("#f_password").val();
    var rePassword = $("#re_password").val();

    if (ConfirmPassword(fPassword, rePassword)) {
        if (fPassword.length < 6 || fPassword.length > 12) {
            layer.tips('密码长度为6-12位,请重试!', "#f_password", {
                tip: [2, "#2277ff"],
                time: 1500,
            });
            return false;
        }
        document.forms["RetrievePW"].submit();
    }
    return false;
}

//验证密码
function ConfirmPassword(first, second) {
    if (first == null || first.substring(0, first.length) == 0) {
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