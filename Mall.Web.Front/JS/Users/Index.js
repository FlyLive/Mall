function log() {
    var account = $("#account").val();
    var password = $("#password").val();
    if (account == "" || account.substring(0, account.length) == 0) {
        layer.msg("账户不能为空");
        return false;
    }
    if (password == "" || password.substring(0, password.length) == 0) {
        layer.msg("密码不能为空");
        return false;
    }
    document.forms["login"].submit();
}