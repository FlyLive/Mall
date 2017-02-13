
function log() {
    var account = $("#account").val();
    var password = $("#password").val();

    if (account == null || account.substring(0, account.length) == 0) {
        layer.tips("账户不能为空,请重试!", "#account", {
            tips: [2, "#2277ff"],
            time: 1500,
        });
        return false;
    }
    if (password == null || password.substring(0, password.length) == 0) {
        layer.tips("密码不能为空,请重试!", "#password", {
            tips: [2, "#2277ff"],
            time: 1500,
        });
        return false;
    }

    document.forms["login"].submit();
}