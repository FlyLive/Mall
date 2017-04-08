document.onkeydown = function (event) {
    var e = event || window.event || arguments.callee.caller.arguments[0];
    if (e && e.keyCode == 13) { // enter 键
        Registe();
    }
};

function Registe() {
    var account = $("#name").val();

    var fPword = $("#fPWord").val();
    var sPWord = $("#sPWord").val();

    var email = $("#email").val();

    var agree = $("#agree").is(":checked");

    //账户确认
    if (account == "" || /\s+/g.test(account) || account.length > 10) {
        Tip('账户格式错误(1位以上,10位以下,不含空格),请重试!', "name");
        return false;
    }
    else if (ReName(account)) {
        Tip('该账户已经被人抢先一步使用了哦,换一个试试！', "name");
        return false;
    }

    //密码确认
    if (ConfirmPassword(fPword, sPWord)) {
        if (fPword.length < 6 || fPword.length > 12) {
            Tip('密码长度为6-12位,请重试!', "fPWord");
            return false;
        }
    }
    else {
        return false;
    }

    //邮箱确认
    if (!/^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/.test(email)) {
        Tip('邮箱格式错误,请重试!', "email");
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
    var result = $.ajax({
        type: 'Get',
        url: '/Users/ReName',
        async:false,
        data: { account },
        beforeSend: function () {
            layer.load(1, {
                shade: [0.1, '#fff'] //0.1透明度的白色背景
            });
        },
        success: function (result) {
            layer.closeAll("loading");
            return result;
        },
        error: function () {
            layer.closeAll("loading");
            OpenTip("出错啦!", 1);
        }
    });
    return result.responseText == "True" ? true : false;
}

function ConfirmPassword(first, second) {
    if (first == "" || /\s+/g.test(first)) {
        Tip('密码不能为空,请重试!', "fPWord");
        return false;
    }
    else if (first != second) {
        Tip('两次密码不一致,请重试!', "sPWord");
        return false;
    }
    return true;
}