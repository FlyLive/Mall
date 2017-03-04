function ModifyAddress(val) {
    alert(val);
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

    if(name == "" || /\s+/g.test(name)){
        Tips("联系人不能为空", "modify_name");
        return false;
    }
    if (!(/^1(3|4|5|7|8)\d{9}$/.test(phone)) && !(/^0[\d]{2,3}-[\d]{7,8}$/.test(phone))) {
        Tips("联系方式格式不正确,请重试！", "modify_phone");
        return false;
    }
    if (address == "" || /\s+/g.test(address)) {
        Tips("收货地址不能为空", "modify_address");
        return false;
    }
    if((zip != "" || /\s+/g.test(zip)) && !/^[A-Za-zd]+([-_.][A-Za-zd]+)*@([A-Za-zd]+[-.])+[A-Za-zd]{2,5}/.test(zip)){
        Tips("邮箱格式不正确或不用填写","modify_zip");
        return false;
    }
    document.forms["modifyAddress"].submit();
}

function Delet(val) {
    alert(val);
    layer.confirm('确认删除该地址？', {
        btn: ['删除', '保留'], //按钮
        skin: 'layui-layer-molv',
        anim:4,
    }, function () {
        location.reload();
    }, function () {
    });
}