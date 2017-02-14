function log() {
    var account = $("#account").val();
    var password = $("#password").val();
    if (account == "" || /\s+/g.test(account)) {
        layer.msg("账户不能为空");
        return false;
    }
    if (password == "" || /\s+/g.test(password)) {
        layer.msg("密码不能为空");
        return false;
    }
    document.forms["login"].submit();
}