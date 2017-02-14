
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
        layer.tips('账户不能为空,请重试!', "#account", {
            tips: [2, "#2277ff"],
            time: 1500,
        });
        return false;
    }
        
    if (logPassword == null || logPassword.substring(0, logPassword.length) == 0) {
        layer.tips('密码不能为空!', "#logPassword", {
            tips: [2, "#2277ff"],
            time: 1500,
        });
        return false;
    }
    else if (logPassword.length < 6 || logPassword.length > 12) {
        layer.tips('密码长度为6-12位,请重试!', "#logPassword", {
            tips: [2, "#2277ff"],
            time: 1500,
        });
        return false;
    }

    if(email == null || email.substring(0, email.length) == 0){
        layer.tips('邮箱不能为空!', "#email", {
            tips: [2, "#2277ff"],
            time: 1500,
        });
        return false;
    }
    
    //重名
    $.ajax({
        type: "Get",
        url: "ReAccount",
        data: { "account": account},
        datatype: Boolean,
        success: function (result) {
            if (result == "True") {
                layer.tips('账户不可用!', "#account", {
                    tips: [2, "#2277ff"],
                    time: 1500,
                });
            }
            else {
                $("#createEmployee").submit();
                    layer.open({
                        title:"提示",
                        content:'创建成功,可以上班了!', 
                        skin:"layui-layer-molv",
                        time: 2000,
                        anim:4,
                    });
            }
        },
        error: function () {
            layer.open({
                title: "错误提示",
                content: "出错啦!",
                icon: 5
            });
        }
    })
}