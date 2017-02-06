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
function ChangeLP() {
    var firstLP = $("#log_password").val();
    var secondeLP = $("#re_log_password").val();

    if (ConfirmPassword(firstLP,secondeLP)) {
        document.forms["changeLP"].submit();
    }
    return false;
}

function ChangePP() {
    var firstPP = $("#pay_password").val();
    var secondePP = $("#re_pay_password").val();

    if (Confirm(firstPP, secondePP)) {
        document.forms["changePP"].submit();
    }
    return false;
}

function ConfirmPassword(first, second) {
    if (first == "" || first.substring(0, first.length) == 0) {
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