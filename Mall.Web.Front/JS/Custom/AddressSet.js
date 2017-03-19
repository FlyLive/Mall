
function CreateAddress() {
    if (IsAddressFull()) {
        OpenTip("您不能创建更多收货地址!(上限为4)", 2);
        return false;
    }
    var contact = $("#name").val();
    var phone = $("#phone").val();
    var address = $("#address").val();

    if (contact == null || contact.substring(0, contact.length) == 0) {
        Tip("联系人不能为空!", "name");
        return false;
    }
    else if (!(/^1(3|4|5|7|8)\d{9}$/.test(phone)) && !(/^0[\d]{2,3}-[\d]{7,8}$/.test(phone))) {
        Tip("电话号码格式不正确,请重试!", "phone");
        return false;
    }
    else if (address == null || address.substring(0, address.length) == 0) {
        Tip("详细地址不能为空!", "address");
        return false;
    }
    document.forms["create"].submit();
}

function IsAddressFull() {
    var result = $.ajax({
        type: 'GET',
        url: '/Custom/IsAddressFull',
        datatype: Boolean,
        success: function (result) {
            if (result == "True") {
                return true;
            }
            else {
                return false;
            }
        },
        error: function () {
            OpenTip("出错啦!", 2);
        }
    });
    return result;
}