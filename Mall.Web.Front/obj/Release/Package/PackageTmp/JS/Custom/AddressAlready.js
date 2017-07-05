function ModifyAddress(val) {
    $(function () {
        $('#modifyAddressModal').modal({
            keyboard: true
        })
    });
    $("#modify_id").val(val);
}

function SubmitModify() {
    var name = $("#modify_name").val();
    var phone = $("#modify_phone").val();
    var address = $("#modify_address").val();
    var zip = $("#modify_zip").val();

    if ((name == "" || /\s+/g.test(name))
        && (phone == "" || /\s+/g.test(phone))
        && (address == "" || /\s+/g.test(address))
        && (zip == "" || /\s+/g.test(zip))) {
        OpenTip("不能全部为空,请在您想修改的部分填写", 1);
        return false;
    }
    if (phone != "" && !/\s+/g.test(phone)) {
        if (!(/^1(3|4|5|7|8)\d{9}$/.test(phone)) && !(/^0[\d]{2,3}-[\d]{7,8}$/.test(phone))) {
            Tip("联系方式格式不正确,请重试！", "modify_phone");
            return false;
        }
    }
    document.forms["modifyAddress"].submit();
}

function SetDefault(addressId) {
    $.ajax({
        type: 'POST',
        url: '/Custom/SetDefaultAddress',
        data: { "addressId": addressId },
        datatype: Boolean,
        success: function (result) {
            if (result == "True") {
                location.reload();
            }
        },
        error: function () {
            OpenTip("出错啦!", 1);
        }
    })
}

function Delet(addressId) {
    layer.confirm('确认删除该地址？', {
        btn: ['删除', '保留'], //按钮
        skin: 'layui-layer-molv',
        anim:4,
    }, function () {
        $.ajax({
            type: 'POST',
            url: '/Custom/DeletAddress',
            data: { "addressId": addressId },
            datatype: Boolean,
            success: function (result) {
                if(result == "True")
                    location.reload();
            },
            error: function () {
                OpenTip("出错啦!", 1);
            }
        })
    }, function () {
    });
}