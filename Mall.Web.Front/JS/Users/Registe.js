function Registe() {
    var account = $("#name").val();

    var fPword = $("#fPWord").val();
    var sPWord = $("#sPWord").val();

    var email = $("#email").val();

    var agree = $("#agree").is(":checked");

    //账户确认
    if (account == "" || /\s+/g.test(account)) {
        Tips('账户不能为空,请重试!', "name");
        return false;
    }
    else if (ReName(account)) {
        Tips('该账户已经被人抢先一步使用了哦,换一个试试！', "name");
        return false;
    }

    //密码确认
    if (ConfirmPassword(fPword, sPWord)) {
        if (fPword.length < 6 || fPword.length > 12) {
            Tips('密码长度为6-12位,请重试!', "fPWord");
            return false;
        }
    }
    else {
        return false;
    }

    //邮箱确认
    if (!/^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/.test(email)) {
        Tips('邮箱格式错误,请重试!', "email");
        return false;
    }

    if (!agree) {
        layer.open({
            title: '提示',
            content: '必须同意本网站使用规则！',
        });
        return false;
    }

    //提交表单
    document.forms["registe"].submit();
}
function ReName(account) {
    return false;
}
function Tips(text,id) {
    layer.tips(text, "#" + id, {
        tip: [2, "#2277ff"],
        time: 1500,
    });
}
function ConfirmPassword(first, second) {
    if (first == "" || /\s+/g.test(first)) {
        Tips('密码不能为空,请重试!', "fPWord");
        return false;
    }
    else if (first != second) {
        Tips('两次密码不一致,请重试!', "sPWord");
        return false;
    }
    return true;
}