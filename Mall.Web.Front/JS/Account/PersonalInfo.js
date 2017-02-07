
function ModifyPhoto() {
    $(function () {
        $('#modifyPhotoModal').modal({
            keyboard: true
        })
    });
}

function Modify() {
    var nick = $("#nick").val();
    var name = $("#name").val();
    var phone = $("#phone").val();
    var email = $("#email").val();

    if (nick == null || nick.substring(0, nick.length) == 0) {
        Tips("昵称不能为空!", "nick")
        return false;
    }
    if (name == null || name.substring(0, name.length) == 0) {
        Tips("真实姓名不能为空!", "name")
        return false;
    }
    if (!(/^1(3|4|5|7|8)\d{9}$/.test(phone)) && !(/^0[\d]{2,3}-[\d]{7,8}$/.test(phone))) {
        Tips("电话号码格式错误,请重试!", "phone")
        return false;
    }
    if (!/^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/.test(email)) {
        Tips('邮箱格式错误,请重试!', "email");
        return false;
    }

    document.forms["modifyInfo"].submit();
}
function Tips(content,name){
    layer.tips(content,"#" + name,{
        tips:[2,"#2277ff"],
        time:1500,
    });
}