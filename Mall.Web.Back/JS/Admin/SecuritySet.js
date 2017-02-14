function ConfirmLP() {
    var logPassword = $("#log_password").val();
    if (logPassword == "" || /\s+/g.test(logPassword)) {
        Tip("密码不能为空，请重试!", "log_password");
        return false;
    }
    else {
        $.ajax({
            type: 'Get',
            url: 'ConfirmLP',
            data: { 'log_password': logPassword },
            datatype: Boolean,
            success: function (result) {
                if (result == 'True') {
                    $(function () {
                        $('#modifyLPModal').modal({
                            keyboard: true
                        })
                    });
                }
                else {
                    OpenTip("登录密码错误，请重试!");
                }
            },
            error: function () {
                OpenTip("出错啦!");
            }
        });
    }
}

function ConfirmMP() {
    var managePassword = $("#manage_password").val();
    if (managePassword == "" || /\s+/g.test(managePassword)) {
        Tip("密码不能为空，请重试!","manage_password");
        return false;
    }
    else {
        $.ajax({
            type: "Get",
            url: "ConfirmMP",
            data: { "manage_password": managePassword },
            datatype: Boolean,
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

function OpenTip(content) {
    layer.open({
        title: '提示',
        content: content,
        icon: 5,
        skin: 'layui-layer-lan',
        closeBtn: 0,
        anim: 3,
    });
}

function Tip(content, tag) {
    layer.tips(content, "#" + tag, {
        tips: [2, "#2277ff"],
        time: 1500,
    });
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

//修改支付密码
function ModifyPP() {
    var firstPP = $("#modify_manage_password").val();
    var secondePP = $("#re_manage_password").val();

    if (Confirm(firstPP, secondePP)) {
        document.forms["modifyMP"].submit();
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