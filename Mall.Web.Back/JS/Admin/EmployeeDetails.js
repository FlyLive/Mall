function DeletAdminLogItem() {
    layer.confirm('确认删除该操作记录条目？', {
        btn: ['确认', '保留'],
        anim: 2,
        skin: 'layui-layer-lan',
    }, function () {
        layer.msg('删除成功');
    }, function () {
    });
}