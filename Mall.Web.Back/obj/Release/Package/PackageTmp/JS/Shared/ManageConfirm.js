function ManageConfirm(){
    var password = $("#managePassword").val();

    var result = $.ajax({
        type: 'GET',
        url: '/Admin/ConfirmMP',
        async: false,
        data: { "manage_password": password },
        success: function (result) {
            if (result == "True") {
                return true;
            }
            OpenTip("密码错误!",3);
            return false;
        },
        error: function () {
            OpenTip("出错啦!", 2);
            return false;
        }
    });
    return result.responseText;
}