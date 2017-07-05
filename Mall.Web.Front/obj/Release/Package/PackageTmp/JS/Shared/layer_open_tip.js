
function OpenTip(content,anim) {
    layer.open({
        title: '提示',
        content: content,
        icon: 5,
        skin: 'layui-layer-lan',
        closeBtn: 0,
        anim: anim,
    });
}

function Tip(content, tag) {
    layer.tips(content, "#" + tag, {
        tips: [2, "#2277ff"],
        time: 1500,
    });
}

function OpenTipSuccess(content, anim) {
    layer.open({
        title: "提示",
        content: content,
        skin: "layui-layer-molv",
        time: 2000,
        anim: anim,
    });
}