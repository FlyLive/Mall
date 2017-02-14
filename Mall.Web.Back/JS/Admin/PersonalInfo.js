function ModifyPhoto() {
    $(function () {
        $('#modifyPhotoModal').modal({
            keyboard: true
        })
    });
}
function Modify() {
    var name = $("#realName").val();
    var phone = $("#phone").val();
    var email = $("#email").val();

    if (name == "" || /\s+/g.test(name)) {
        Tips("真实姓名不能为空!", "realName")
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
function Tips(content, name) {
    layer.tips(content, "#" + name, {
        tips: [2, "#2277ff"],
        time: 1500,
    });
}
$(window).load(function () {
    var options =
    {
        thumbBox: '.thumbBox',
        spinner: '.spinner',
        imgSrc: '../Pictures/Users/Avatar/avatar.png'
    }
    var cropper = $('.imageBox').cropbox(options);
    $('#upload-file').on('change', function () {
        var reader = new FileReader();
        reader.onload = function (e) {
            options.imgSrc = e.target.result;
            cropper = $('.imageBox').cropbox(options);
        }
        reader.readAsDataURL(this.files[0]);
        this.files = null;
    })
    $('#btnCrop').on('click', function () {
        var img = cropper.getDataURL();
        $('.cropped').html('');
        $('.cropped').append('<img src="' + img + '" align="absmiddle" style="width:64px;margin-top:4px;border-radius:64px;box-shadow:0px 0px 12px #7E7E7E;" ><p>64px*64px</p>');
        $('.cropped').append('<img src="' + img + '" align="absmiddle" style="width:128px;margin-top:4px;border-radius:128px;box-shadow:0px 0px 12px #7E7E7E;"><p>128px*128px</p>');
        $('.cropped').append('<img src="' + img + '" align="absmiddle" style="width:180px;margin-top:4px;border-radius:180px;box-shadow:0px 0px 12px #7E7E7E;"><p>180px*180px</p>');
    })
    $('#btnZoomIn').on('click', function () {
        cropper.zoomIn();
    })
    $('#btnZoomOut').on('click', function () {
        cropper.zoomOut();
    })
    $("#save").on('click', function () {
        var imgBase = cropper.getDataURL();
        $.ajax({
            type: "post",
            url: "ModifyPhoto",
            data: { "imgBase": imgBase },
            datatype: Text,
            success: function (src) {
                if (src != null) {
                    layer.msg("修改成功!");
                    location.reload();
                }
            },
            error: function () {
                layer.open({
                    title: "错误提示",
                    content: "出错啦!",
                    icon: 5,
                });
                return false;
            }
        })
    })
});