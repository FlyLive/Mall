function ConfirmLP() {
    var logPassword = $("#log_password").val();
    $("#log_password").val("");
    if (logPassword == "" || /\s+/g.test(logPassword)) {
        Tip("密码不能为空，请重试!", "log_password");
        return false;
    }
    else {
        $.ajax({
            type: 'Get',
            url: '/Admin/ConfirmLP',
            data: { 'log_password': logPassword },
            success: function (result) {
                if (result == 'True') {
                    $(function () {
                        $('#modifyLPModal').modal({
                            keyboard: true
                        })
                    });
                }
                else {
                    OpenTip("登录密码错误，请重试!",3);
                }
            },
            error: function () {
                OpenTip("出错啦!",2);
            }
        });
    }
}

function ConfirmMP() {
    var managePassword = $("#manage_password").val();
    $("#manage_password").val("");
    if (managePassword == "" || /\s+/g.test(managePassword)) {
        Tip("密码不能为空，请重试!","manage_password");
        return false;
    }
    else {
        $.ajax({
            type: "Get",
            url: "/Admin/ConfirmMP",
            data: { "manage_password": managePassword },
            success: function (result) {
                if (result == "True") {
                    $(function () {
                        $('#modifyMPModal').modal({
                            keyboard: true
                        })
                    });
                }
                else {
                    OpenTip("管理密码错误，请重试!");
                }
            },
            error: function () {
                OpenTip("出错啦!");
            }
        });
    }
}

//修改登录密码
function ModifyLP() {
    var firstLP = $("#modify_log_password").val();
    var secondeLP = $("#re_log_password").val();

    if (ConfirmPassword(firstLP, secondeLP)) {
        document.forms["modifyLP"].submit();
    }
    return false;
}

//修改管理密码
function ModifyMP() {
    var firstPP = $("#modify_manage_password").val();
    var secondePP = $("#re_manage_password").val();

    if (Confirm(firstPP, secondePP)) {
        $.ajax({
            type: "Get",
            url: "/Admin/ModifyMP",
            data: { "manage_password": managePassword },
            success: function (result) {
                if (result == "True") {
                    $("#modify_manage_password").val("");
                    $("#re_manage_password").val("");
                    $('#modifyMPModal').modal('hide');
                }
                else {
                    OpenTip("修改失败!");
                }
            },
            error: function () {
                OpenTip("出错啦!");
            }
        });
    }
    return false;
}

//验证密码
function ConfirmPassword(first, second) {
    if (first == "" || /\s+/g.test(first)) {
        OpenTip("密码不能为空，请重试!");
        return false;
    }
    else if (first != second) {
        OpenTip("两次密码输入不一致，请重试!");
        return false;
    }
    else if (first.length < 6 || first.length > 12) {
        OpenTip("密码长度为6-12位,请重试!");
        return false;
    }
    return true;
}