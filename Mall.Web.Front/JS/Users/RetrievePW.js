function ClientConfirm() {
    var account = $("#account").val();
    var email = $("#email").val();

    if (account == null || /\s+/g.test(account)) {
        Tips("账户不能为空!", "account");
        return false;
    }
    if (!/^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/.test(email)) {
        Tip("邮箱格式不正确!", "email");
        return false;
    }

    $.ajax({
        type: "Get",
        url: "ClientConfirm",
        Data: { account, email},
        Datatype: Boolean,
        success: function (Data) {
            if (Data == "True") {
                layer.confirm("验证码已发送,请及时查看并进行重置密码",{
                    btn: ["好的"],
                });
                $('#VerifyCodeModal').modal({
                    keyboard: true
                })
                return false;
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

function VerifyCode() {
    var verifyCode = $("#verifyCode").val();
    if (!/^\w{8}-(\w{4}-){3}\w{12}$/.test(verifyCode)) {
        Tip("请输入有效的32位验证码", "verifyCode");
        return false;
    }
    $.ajax({
        type: "Get",
        url: "VerifyCodeConfirm",
        Data: { "verifyCode": verifyCode },
        Datatype: Boolean,
        success: function (Data) {
            if (Data == "True") {
                $('#VerifyCodeModal').modal('hide');
                $(function () {
                    $('#ReSetPWModal').modal({
                        keyboard: true
                    })
                });
                layer.msg("验证码正确,请及时重置密码", { time: 2000 ,icon:1});
                return true;
            }
            else {
                layer.open({
                    title: "错误提示",
                    content: "验证码错误,请重试!",
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
    if (first == null || /\s+/g.test(first)) {
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