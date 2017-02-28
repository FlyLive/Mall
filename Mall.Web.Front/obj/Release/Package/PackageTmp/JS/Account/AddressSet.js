
function CreateAddress() {
    var contact = $("#name").val();
    var phone = $("#phone").val();
    var address = $("#address").val();

    if (contact == null || contact.substring(0, contact.length) == 0) {
        Tips("联系人不能为空!", "name");
        return false;
    }
    else if (!(/^1(3|4|5|7|8)\d{9}$/.test(phone)) && !(/^0[\d]{2,3}-[\d]{7,8}$/.test(phone))) {
        Tips("电话号码格式不正确,请重试!", "phone");
        return false;
    }
    else if (address == null || address.substring(0, address.length) == 0) {
        Tips("详细地址不能为空!", "address");
        return false;
    }
    document.forms["create"].submit();
}
function Tips(text, id) {
    layer.tips(text, "#" + id, {
        tips: [2, "#2277ff"],
        time: 1500,
    });
}