function SetPermissionTree() {
    var id = $("#currentUserId").val();
    $.ajax({
        type: "post",
        url: "/Admin/GetPermissions",
        data: { "userId": id },
        datatype: "json",
        success: function (menus) {
            var setting = {
                check: {
                    enable: true,
                    chkboxType: { "Y": "ps", "N": "ps" },
                },
                data: {
                    simpleData: {
                        enable: true
                    }
                },
            };

            var zNodes = new Array();

            for (var i = 0; i < menus.length; i++) {
                zNodes.push({
                    id: menus[i].Id,
                    pId: menus[i].ParentId,
                    name: menus[i].Name,
                    checked: menus[i].Has,
                    open: true,
                    nocheck: menus[i].IsDefault
                });
            }

            $.fn.zTree.init($("#employeePermissions"), setting, zNodes);
        },
        error: function () {
            OpenTip("出错啦!", 1);
        }
    });
}

function GetPermissionMenus() {
    $(function () {
        $('#modifyPermissionsModal').modal({
            keyboard: true
        })
    });
    SetPermissionTree();
}

function ModifyPermission() {
    var treeObj = $.fn.zTree.getZTreeObj("employeePermissions");
    var mNodes = treeObj.getCheckedNodes(true);

    var id = $("#currentUserId").val();
    var menuIds = new Array();
    for (var i = 0; i < mNodes.length; i++)
        menuIds.push(mNodes[i].id);

    $.ajax({
        type: 'POST',
        url: '/Admin/ModifyEmployeePermissions',
        data: { "userId": id ,"menuIds": menuIds},
        success: function (result) {
            if (result == "True") {
                OpenTipSuccess("修改成功,等待跳转!", 2);
                location.reload();
            }
            else {
                OpenTip("修改失败!", 1);
            }
        },
        error: function () {
            OpenTip("出错啦!", 1);
        }
    });
}

function Refresh(event) {
    var id = event.id;
    $.ajax({
        type: 'Get',
        url: '/Admin/GetEmployeeInfo',
        data: { "userId": id },
        datatype: 'html',
        success: function (html) {
            $("#admin-detail").html(html);
        },
        error: function () {
            OpenTip("出错啦!", 1);
        }
    });
}