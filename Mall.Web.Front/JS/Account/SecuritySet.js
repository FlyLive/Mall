function ConfirmLP() {
    var oldLogPassword = $("#old_log_password").val();
    var input = $("#input_log_password").val();
    if (oldLogPassword == input) {
        $(function () {
            $('#ChangeLPModal').modal({
                keyboard: true
            })
        });
    }
    else {
        layer.open({
            title: '错误提示',
            content: '登录密码错误，请重试',
            icon: 5,
        });
    }
}

function ConfirmPP() {
    var oldPayPassword = $("#old_pay_password").val();
    var input = $("#input_pay_password").val();
    if (input == oldPayPassword) {
        $(function () {
            $('#ChangePPModal').modal({
                keyboard: true
            })
        });
    }
    else {
        layer.open({
            title: '错误提示',
            content: '支付密码错误，请重试',
            icon: 5,
        });
    }
}
//document.forms["表单name"].submit()//普通按钮提交表单
//修改登录密码
function ChangeLP() {
    var oldLP = $("#old_log_password").val();
    var firstLP = $("#log_password").val();
    var secondeLP = $("#re_log_password").val();

    if (ConfirmPassword(oldLP, firstLP, secondeLP)) {
        if (firstLP.length < 6 || firstLP.length > 12) {
            layer.tips('密码长度为6-12位,请重试!', "#log_password", {
                tip: [2, "#2277ff"],
                time: 1500,
            });
            return false;
        }
        document.forms["changeLP"].submit();
    }
    return false;
}

//修改支付密码
function ChangePP() {
    var oldPP = $("#old_pay_password").val();
    var firstPP = $("#pay_password").val();
    var secondePP = $("#re_pay_password").val();

    if (Confirm(oldPP,firstPP, secondePP)) {
        document.forms["changePP"].submit();
    }
    return false;
}

//验证密码
function ConfirmPassword(old,first, second) {
    if (first == "" || /\s+/g.test(first)) {
        layer.open({
            title: '错误提示',
            content: '密码不能为空，请重试!',
        });
        return false;
    }
    else if (old == first) {
        layer.open({
            title: '错误提示',
            content: '新密码不能与原密码相同,请重试!',
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