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
    Data: {
        items: [],
    }
})

var onLine = new Vue({
    el: '#onLine',
    Data: {
        items: []
    }
})

new Vue({
    el: '.chat-input',
    Data: {
        newItem: ''
    },
    methods: {
        addItem: function () {
            var context = $(".input-text").val();
            chatServer.send(context);
            this.newItem = '';
        },
        clearItems: function () {//清空聊天记录
            for (var item in content.$Data.items)
                content.$Data.items.splice(item);
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
    receive: function (Data) {
        var Data_content = Data.content;
        var length = chatServer.name.length;
        var index = Data_content.indexOf('@');
        var end = Data_content.length;
        var restContent = Data_content.substring(0, index) + Data_content.substring(index + length + 1, end);
        var isToMe = Data_content.substring(index, index + length + 1) == '@' + chatServer.name;
        var imgContent = Data.content.substring(0, 4);
        var isImg = (imgContent == 'Img1' || imgContent == 'Img2' || imgContent == 'Img3' || imgContent == 'Img4');

        if (isImg) {
            if (Data.nick == chatServer.name) {
                content.$Data.items.push({ classes: "myself message", display: 'inline', content_Img: "/Pictures/Chat/Img/" + imgContent + ".gif" })
            }
            else {
                content.$Data.items.push({ classes: "message", nick: Data.nick + ":",display: 'inline' , content_Img: "/Pictures/Chat/Img/" + imgContent + ".gif" })
                shake.play();
            }
        }
        else {
            if (Data.nick == chatServer.name) {
                content.$Data.items.push({ classes: "myself message", content: Data.content})
            }
            else {
                if (isToMe) {
                    content.$Data.items.push({ classes: "message", nick: Data.nick + ":", toMe: "@" + chatServer.name, content: restContent })
                    system.play();
                }
                else{
                    content.$Data.items.push({ classes: "message", nick: Data.nick + ":", content: Data.content, })
                    shake.play();
                }
            }

        };
        $(".chat-content #content").scrollTop($(".chat-content #content")[0].scrollHeight);
    },
    newbie: function (Data) {
        for (var item in onLine.$Data.items)
            onLine.$Data.items.splice(item);

        content.$Data.items.push({ classes: "sign", nick: Data.nick + "加入了聊天" });

        var list = Data.list;
        for (var i = 0; i < list.length; i++) {
            if (list[i] == chatServer.name) {
                onLine.$Data.items.push({ img_src: "电脑(Myself).png", nick: list[i] });
            }
            else {
                onLine.$Data.items.push({ img_src: "电脑(Others).png", nick: list[i] });
            }
        }
        global.play();
        $(".chat-content #content").scrollTop($(".chat-content #content")[0].scrollHeight);
    },
    leave: function (Data) {
        for (var item in onLine.$Data.items)
            onLine.$Data.items.splice(item);

        content.$Data.items.push({ classes: "sign", nick: Data.nick + "退出了聊天" });

        var list = Data.list;
        for (var i = 0; i < list.length; i++) {
            if (list[i] == chatServer.name) {
                onLine.$Data.items.push({ img_src: "电脑(Myself).png", nick: list[i] });
            }
            else {
                onLine.$Data.items.push({ img_src: "电脑(Others).png", nick: list[i] });
            }
        }
        $(".chat-content #content").scrollTop($(".chat-content #content")[0].scrollHeight);
    }
})