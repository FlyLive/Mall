
function GetRoleMenus() {
    var id = $("#currentUserId").val();
    $.ajax({
        type: "Get",
        url: "/Admin/GetRoles",
        data: { "userId": id },
        datatype: "json",
        success: function (roleMenus) {
            if (roleMenus.length == 0) {
                $("#employeeRoles").parent().parent().parent().html("<h4 style='margin-left:20px'>暂时还未设置角色，请添加角色后重试</h4>")
                return false;
            }
            $(function () {
                $('#modifyEmployeeRolesModal').modal({
                    keyboard: true
                })
            });
            InitZTreeMenu(roleMenus, "employeeRoles");
        },
        error: function () {
            OpenTip("出错啦!", 1);
        }
    });
}

function ModifyEmployeeRoles() {
    var userId = $("#currentUserId").val();
    var roleIds = GetZTreeCheckedId("employeeRoles");

    $.ajax({
        type: 'POST',
        url: '/Admin/ModifyEmployeeRoles',
        data: { "userId": userId, "roleIds": roleIds },
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

function SubmitCreate() {
    $('#createRoleModal').modal('hide');

    var name = $("#role-name").val();
    var details = $("#role-detail").val();

    if (name == "" || /\s+/g.test(name)) {
        Tip("角色名不能为空", "name");
        return false;
    }
    if (details == "" || /\s+/g.test(details)) {
        Tip("角色详情不能为空", "name");
        return false;
    }

    var menuIds = GetZTreeCheckedId("createRole");
    if (menuIds.length == 0) {
        OpenTip("请至少选择一个权限", 2);
        return false;
    }

    $.ajax({
        type: "Post",
        url: "/Admin/CreateRole",
        data: { "menuIds": menuIds, "roleName": name, "roleDetails": details },
        success: function (result) {
            if (result == "True") {
                OpenTipSuccess("创建成功", 3);
                location.reload();
            }
        },
        error: function () {
            OpenTip("出错啦!", 1);
        }
    })
}

function CreateRole() {
    $.ajax({
        type: "Get",
        url: "/Admin/GetAllPermissions",
        datatype: "json",
        success: function (roleMenus) {
            InitZTreeMenu(roleMenus, "createRole");
            $(function () {
                $('#createRoleModal').modal({
                    keyboard: true
                })
            });
        },
        error: function () {
            OpenTip("出错啦!", 1);
        }
    });
}

function ModifyRolePermissions() {
    var name = $("#modify-name").val();
    var details = $("#modify-details").val();
    var id = $("#modify-role-id").val();

    var menuIds = GetZTreeCheckedId("rolePermissions");

    if (menuIds.length == 0) {
        OpenTip("角色权限不能为空", 2);
        return false;
    }

    $('#modifyRolePermissionsModal').modal('hide');

    $.ajax({
        type: "Get",
        url: "/Admin/ModifyRolePermissions",
        data: { "roleId": id, "menuIds": menuIds, "modifyName": name, "modifyDetails": details },
        success: function (result) {
            if (result == "True") {
                OpenTipSuccess("修改成功", 3);
            }
        },
        error: function () {
            OpenTip("出错啦!", 1);
        }
    });
}
$(document).ready(function () {
    $(".role-manage .permission-content .permission-item").bind("contextmenu", function (ev) {
        var id = this.id;
        var oEvent = event;
        var oDiv = document.getElementById('click-menu');

        $("#modify-role-id").val(id);

        oDiv.style.display = 'block';

        oDiv.style.left = oEvent.clientX + 'px';
        oDiv.style.top = oEvent.clientY + 'px';

        return false;
    });
})

function DeletRole() {
    var oDiv = document.getElementById('click-menu');
    oDiv.style.display = 'none';

    var roleId = $("#modify-role-id").val();
    if (roleId == "" || /\s+/g.test(roleId)) {
        return false;
    }
    $.ajax({
        type: "Post",
        url: "/Admin/DeletRoleByRoleId",
        data: { "roleId": roleId },
        success: function (result) {
            if (result == "True") {
                OpenTipSuccess("删除成功!", 1);
                $(".role-manage .permission-content #" + roleId).remove();
            }
            else {
                OpenTip("删除失败,未知错误", 3);
            }
        },
        error: function () {
            OpenTip("出错啦!", 1);
        }
    });

}

function ModifyRole() {
    var oDiv = document.getElementById('click-menu');
    oDiv.style.display = 'none';

    var roleId = $("#modify-role-id").val();
    if (roleId == "" || /\s+/g.test(roleId)) {
        return false;
    }
    $("#modify-role-id").val(roleId);
    $.ajax({
        type: "Get",
        url: "/Admin/GetRolePermissionsMenu",
        data: { "roleId": roleId },
        datatype: "json",
        success: function (roleMenus) {
            InitZTreeMenu(roleMenus, "rolePermissions");
            $(function () {
                $('#modifyRolePermissionsModal').modal({
                    keyboard: true
                })
            });
        },
        error: function () {
            OpenTip("出错啦!", 1);
        }
    });
}
document.onclick = function () {
    var clickDiv = document.getElementById('click-menu');
    clickDiv.style.display = 'none';
};