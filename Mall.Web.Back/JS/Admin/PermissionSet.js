function SetPermissionTree() {
    
    var p;
    $.ajax({
        type: 'get',
        url: 'GetPermissions',
        Datatype: Object,
        success: function (permissions) {
            //alert(permissions.length);
            p = permissions;
        },
        error: function () {

        }
    });
    var setting = {
        check: {
            enable: true,
            chkboxType: { "Y": "ps", "N": "ps" }
        },
        Data: {
            simpleData: {
                enable: true
            }
        }
    };

    var zNodes = [
        { id: 1, pId: 0, name: "随意勾选 1", open: true },
        { id: 11, pId: 1, name: "随意勾选 1-1", open: true },
        { id: 111, pId: 11, name: "随意勾选 1-1-1" },
        { id: 112, pId: 11, name: "随意勾选 1-1-2" },
        { id: 12, pId: 1, name: "随意勾选 1-2", open: true },
        { id: 121, pId: 12, name: "随意勾选 1-2-1" },
        { id: 122, pId: 12, name: "随意勾选 1-2-2" },
        { id: 2, pId: 0, name: "随意勾选 2", checked: true, open: true },
        { id: 21, pId: 2, name: "随意勾选 2-1" },
        { id: 22, pId: 2, name: "随意勾选 2-2", open: true },
        { id: 221, pId: 22, name: "随意勾选 2-2-1", checked: true },
        { id: 222, pId: 22, name: "随意勾选 2-2-2" },
        { id: 23, pId: 2, name: "随意勾选 2-3" }
    ];

    $(document).ready(function () {
        $.fn.zTree.init($("#treeDemo"), setting, zNodes);
    });
}

function ModifyPermission() {
    var treeObj = $.fn.zTree.getZTreeObj("treeDemo");
    var sNodes = treeObj.getCheckedNodes(true);
    var id = $("#employeeId").val();
    //alert(sNodes.length);
    alert(id);
    if (sNodes.length > 0) {

        for (var i = 0; i < sNodes.length; i++) {
            //alert(sNodes[i].id);
        }
    }
    else {
        alert("error");
    }
}

function Refresh(event) {
    var id = event.id;
    $.ajax({
        type: 'Get',
        url: 'GetEmployeeInfo',
        Data: { "employeeId": id },
        Datatype: 'html',
        success: function (html) {
            $("#admin-detail").html(html);
        },
        error: function () {
        }
    })
}