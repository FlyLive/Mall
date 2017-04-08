document.onkeydown = function (event) {
    var e = event || window.event || arguments.callee.caller.arguments[0];
    if (e && e.keyCode == 13) { // enter 键
        log();
    }
};

function log() {
    var account = $("#account").val();
    var password = $("#password").val();
    if (account == "" || /\s+/g.test(account)) {
        Tip("账户不能为空","account");
        return false;
    }
    if (password == "" || /\s+/g.test(password)) {
        Tip("密码不能为空","password");
        return false;
    }
    document.forms["login"].submit();
}