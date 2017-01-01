function StartTime() {
    var newDate = new Date();
    var div = document.getElementById("time");
    var h = newDate.getHours();
    var m = newDate.getMinutes();
    var s = newDate.getSeconds();
    m = Single(m);
    s = Single(s);
    div.innerHTML = h + ":" + m + ":" + s;
    t = setTimeout('StartTime()', 500);
}
function Single(obj) {
    if (obj < 10)
        return "0" + obj;
    return obj;
}