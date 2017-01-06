var global = document.getElementById("Global-mic");
var shake = document.getElementById("shake-mic");
var system = document.getElementById("system-mic");

var chatServer = new ChatServer('123456');
chatServer.connect()
    .success(function () {
        setChatHeadName();
    })
    .error(function () {
    })
var content = new Vue({
    el: '#content',
    data: {
        items: [],
    }
})

var onLine = new Vue({
    el: '#onLine',
    data: {
        items: []
    }
})

new Vue({
    el: '.chat-input',
    data: {
        newItem: ''
    },
    methods: {
        addItem: function () {
            var context = $(".input-text").val();
            chatServer.send(context);
            this.newItem = '';
        },
        clearItems: function () {//清空聊天记录
            for (var item in content.$data.items)
                content.$data.items.splice(item);
        },
    }
})
function setChatHeadName() {
    var chatNameP = document.getElementById("chat_name_p");
    chatNameP.innerText = chatServer.name;
    var renameButton = document.getElementById("chat_name_button");
    var chatNameWidth = chatNameP.clientWidth + renameButton.clientWidth + 1;

    $("#chat_name").css({ width: chatNameWidth });
}

function sendImg(index) {
    chatServer.send("Img" + index);
}
function setName() {
    var newName = $("#newNick").val();
    chatServer.rename(newName);
    $("#newNick").html('');
    setChatHeadName();
}
chatServer.on({
    receive: function (data) {
        var data_content = data.content;
        var length = chatServer.name.length;
        var index = data_content.indexOf('@');
        var end = data_content.length;
        var restContent = data_content.substring(0, index) + data_content.substring(index + length + 1, end);
        var isToMe = data_content.substring(index, index + length + 1) == '@' + chatServer.name;
        var imgContent = data.content.substring(0, 4);
        var isImg = (imgContent == 'Img1' || imgContent == 'Img2' || imgContent == 'Img3' || imgContent == 'Img4');

        if (isImg) {
            if (data.nick == chatServer.name) {
                content.$data.items.push({ classes: "myself message", display: 'inline', content_Img: "/Pictures/Chat/Img/" + imgContent + ".gif" })
            }
            else {
                content.$data.items.push({ classes: "message", nick: data.nick + ":",display: 'inline' , content_Img: "/Pictures/Chat/Img/" + imgContent + ".gif" })
                shake.play();
            }
        }
        else {
            if (data.nick == chatServer.name) {
                content.$data.items.push({ classes: "myself message", content: data.content})
            }
            else {
                if (isToMe) {
                    content.$data.items.push({ classes: "message", nick: data.nick + ":", toMe: "@" + chatServer.name, content: restContent })
                    system.play();
                }
                else{
                    content.$data.items.push({ classes: "message", nick: data.nick + ":", content: data.content, })
                    shake.play();
                }
            }

        };
        $(".chat-content #content").scrollTop($(".chat-content #content")[0].scrollHeight);
    },
    newbie: function (data) {
        for (var item in onLine.$data.items)
            onLine.$data.items.splice(item);

        content.$data.items.push({ classes: "sign", nick: data.nick + "加入了聊天" });

        var list = data.list;
        for (var i = 0; i < list.length; i++) {
            if (list[i] == chatServer.name) {
                onLine.$data.items.push({ img_src: "电脑(Myself).png", nick: list[i] });
            }
            else {
                onLine.$data.items.push({ img_src: "电脑(Others).png", nick: list[i] });
            }
        }
        global.play();
        $(".chat-content #content").scrollTop($(".chat-content #content")[0].scrollHeight);
    },
    leave: function (data) {
        for (var item in onLine.$data.items)
            onLine.$data.items.splice(item);

        content.$data.items.push({ classes: "sign", nick: data.nick + "退出了聊天" });

        var list = data.list;
        for (var i = 0; i < list.length; i++) {
            if (list[i] == chatServer.name) {
                onLine.$data.items.push({ img_src: "电脑(Myself).png", nick: list[i] });
            }
            else {
                onLine.$data.items.push({ img_src: "电脑(Others).png", nick: list[i] });
            }
        }
        $(".chat-content #content").scrollTop($(".chat-content #content")[0].scrollHeight);
    }
})