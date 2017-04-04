
$(document).ready(function () {
    $.ajax({
        type: 'Get',
        url: '/Admin/GetAllMenus',
        datatype: 'json',
        success: function (menus) {
            InitZTreeMenu(menus,"permissionsTree");
        },
        error: function () {
            OpenTip("读取权限列表失败!", 1);
        }
    })
});

function CreateEmployee() {
    var account = $("#account").val();
    var logPassword = $("#logPassword").val();
    var email = $("#email").val();

    var ids = document.getElementsByName("permissionIds");
    for (var i = 0; i < ids.length; i++) {
        if (ids[i].checked) {
            permissionIds.push(ids[i].val());
        }
    }

    if (account == null || account.substring(0, account.length) == 0) {
        Tips('账户不能为空,请重试!', "account");
        return false;
    }

    if (logPassword == null || logPassword.substring(0, logPassword.length) == 0) {
        Tips('密码不能为空!', "logPassword");
        return false;
    }
    else if (logPassword.length < 6 || logPassword.length > 12) {
        Tips('密码长度为6-12位,请重试!', "logPassword");
        return false;
    }

    if (email == null || email.substring(0, email.length) == 0) {
        Tips('邮箱不能为空!', "email");
        return false;
    }

    //重名
    var result = ReCount(account);
    if (result == "True") {
        Tip("账户不可用!", "account");
        return false;
    }
    else {
        SubmitCreate();
    }
}

function ReCount(account) {
    var result = $.ajax({
        type: "Get",
        url: "ReAccount",
        data: { "account": account },
        async:false,
        success: function () {
        },
        error: function () {
            OpenTip("出错啦!", 1);
        }
    });
    return result.responseText;
}

function SubmitCreate() {
    var account = $("#account").val();
    var logPassword = $("#logPassword").val();
    var phoneNumber = $("#phoneNumber").val();
    var birthday = $("#birthday").val();
    var gender = $("input[name='gender']:checked").val() == 1;
    var nick = $("#nick").val();
    var managePassword = $("#managePassword").val();
    var email = $("#email").val();
    var permissionIds = GetZTreeCheckedId("permissionsTree");

    $.ajax({
        type: 'POST',
        url: '/Admin/CreateEmployee',
        data: { "account":account, "logPassword":logPassword,
            "phoneNumber":phoneNumber,"birthday":birthday,"gender":gender,"nick":nick,
             "managePassword":managePassword, "email":email, "permissionIds":permissionIds },
        success: function (result) {
            if (result == "True") {
                OpenTipSuccess('创建成功,可以上班了!',4);
            }
            else {
                OpenTip("创建失败,不可预知的错误发生了!",1);
            }
        },
        error: function () {
            OpenTip("出错啦!",1);
        }
    })
}