document.onkeydown = function (event) {
    var e = event || window.event || arguments.callee.caller.arguments[0];
    if (e && e.keyCode == 13) { // enter 键
        log();
    }
};

function log() {
    var account = $("#account").val();
    var password = $("#password").val();

    if (account == null || account.substring(0, account.length) == 0) {
        Tip("账户不能为空,请重试!", "account");
        return false;
    }
    if (password == null || password.substring(0, password.length) == 0) {
        Tip("密码不能为空,请重试!", "password");
        return false;
    }

    document.forms["login"].submit();
}