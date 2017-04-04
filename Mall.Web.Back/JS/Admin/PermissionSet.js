
//初始化ZTree
function InitZTreeMenu(menu, tag) {
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

    var allRoles = new Array();

    for (var i = 0; i < menu.length; i++) {
        allRoles.push({
            id: menu[i].Id,
            pId: menu[i].ParentId,
            name: menu[i].Name,
            checked: menu[i].Has,
            open: true,
            nocheck: menu[i].IsDefault
        });
    }

    $.fn.zTree.init($("#" + tag), setting, allRoles);
}

//获取目标ID 下 ZTree的选中Id
function GetZTreeCheckedId(tag) {
    var treeObj = $.fn.zTree.getZTreeObj(tag);
    var mNodes = treeObj.getCheckedNodes(true);

    var menuIds = new Array();
    for (var i = 0; i < mNodes.length; i++)
        menuIds.push(mNodes[i].id);
    return menuIds;
}

function GetPermissionMenus() {
    var id = $("#currentUserId").val();
    $.ajax({
        type: "Get",
        url: "/Admin/GetPermissions",
        data: { "userId": id },
        datatype: "json",
        success: function (menus) {
            InitZTreeMenu(menus, "employeePermissions");
            $(function () {
                $('#modifyPermissionsModal').modal({
                    keyboard: true
                })
            });
        },
        error: function () {
            OpenTip("出错啦!", 1);
        }
    });
}

function ModifyPermission() {
    var userId = $("#currentUserId").val();
    var menuIds = GetZTreeCheckedId("employeePermissions");

    $.ajax({
        type: 'POST',
        url: '/Admin/ModifyEmployeePermissions',
        data: { "userId": userId ,"menuIds": menuIds},
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

function SearchEmployee() {
    var search = $("#search-input").val();
    $.ajax({
        type: 'Get',
        url: '/Admin/SearchEmployee',
        data: { "search": search },
        datatype: 'html',
        success: function (html) {
            $("#admin-list").html(html);
        },
        error: function () {
            OpenTip("出错啦!", 1);
        }
    });
}