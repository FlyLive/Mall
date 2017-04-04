
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

    if (name == "" || /\s+/g.test(name)) {
        Tip("角色名不能为空", "name");
        return false;
    }

    var menuIds = GetZTreeCheckedId("createRole");

    $.ajax({
        type: "Post",
        url: "/Admin/CreateRole",
        data: { "menuIds": menuIds, "roleName": name },
        success: function (result) {
            if (result == "Treu") {
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

function ModifyRole(event) {
    var roleId = $(event).attr("id");
    alert(roleId);
    if (roleId == undefined) {
        return false;
    }
    $("#modify-role-id").val(roleId);
    $.ajax({
        type: "Get",
        url: "/Admin/GetRolePermissionsMenu",
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

function ModifyRolePermissions() {
    var name = $("#modifyName").val();
    var id = $("#modify-role-id").val();

    if (name == "" || /\s+/g.test(name)) {
        Tip("角色名不能为空", "name");
        return false;
    }

    var menuIds = GetZTreeCheckedId("rolePermissions");

    $('#modifyRolePermissionsModal').modal('hide');

    $.ajax({
        type: "Get",
        url: "/Admin/ModifyRolePermissions",
        data: { "roleId": id, "menuIds": menuIds, "name": name },
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

$(".role-manage .permission-content .permission-item").click(ModifyRole(this));