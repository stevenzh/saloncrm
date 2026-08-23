// 服务器地址
var serverUrl = "http://localhost:45743";
//serverUrl = "http://cn.mdss.hk";

// 当前平台
var platform = "browser";
//platform = "cordova";
// 当前用户存储localStorage的KEY
var localAccount = "local_account";
// 日志文件
var logOb;

var userObject = {
    userid: "",
    username: "",
    password: "",
    time: "",
    uuid: "",     // 终端设备ID
    hostcode: "",
    host: "",
    branch: "1",
    branchName: "美容院",
    type: "",     // 1美容师 2管理账户（前台） 3顾问
    version: "1000",
    percentageLock: "",
    majorPercentage: "0.5",           // 顾问占比
    majorBeauticianPercentage: "0.5", // 美容师占比
    beauticianPercentage: ""          // 辅助美容师占比
}

var memberObject = {
    MemberID: "",
    Name: "",
    CardNo: "",
    MobileNumber: "",
    Address: "",
    Passwd: "",
    JoinDate: "",
    Sex: "",
    Vocation: "",
    Source: "",
    Level: "",
    Birthday: "",
    JoinBranch: "",
    Amt: 0,
    Points: 0,
    Remark: "",
    SalesmanId: ""
}

var deviceReadyDeferred = $.Deferred();
var jqmReadyDeferred = $.Deferred();

document.addEventListener("deviceready", onDeviceReady, false);
function onDeviceReady() {
    console.log("run onDeviceReady");
    //serverUrl = "http://localhost:45743";
    //serverUrl = "http://cn.mdss.hk";
    platform = "cordova";
    userObject.uuid = device.uuid;

    //window.resolveLocalFileSystemURL(cordova.file.dataDirectory, function (dir) {
    //    console.log("got main dir", dir);
    //    dir.getFile("salonlog.txt", { create: true }, function (file) {
    //        console.log("got the file", file);
    //        logOb = file;
    //        writeLog("App started");
    //    });
    //});

    deviceReadyDeferred.resolve();
}

$(document).bind("mobileinit", function () {
    console.log("config mobile init");
    $.support.cors = true;
    $.mobile.allowCrossDomainPages = true;   //设置是否允许跨域。
    $.mobile.phonegapNavigationEnabled = true;

    $.mobile.ajaxEnabled = false;   // 控制是否禁止默认的Ajax链接点击和表单提交，并停止hash的监听，然后以常规的HTTP方式进行。
    $.mobile.pushStateEnabled = false;  // 在支持的浏览器中开启history.replaceState这个增强特性，把哈希值（hash-based）的Ajax请求转化为完整的文档路径。jQuery Mobile建议在关闭Ajax导航和大量使用外部链接的情况下关闭这个特性。
    $.mobile.linkBindingEnabled = true; // jQuery Mobile会自动绑定锚标记到文档中，设置该选项为false将阻止所有的锚点击处理，例如取消激活按钮状态。一般来说只有在把锚标记处理交给另一个处理库时才设置该属性为false。
    $.mobile.hashListeningEnabled = true;  // 设置是否监听和处理 location.hash 的改变
    $.mobile.touchOverflowEnabled = false; // 设置是否使用设备的原生区域滚动特性，除了 iOS5 之外大部分的设备到目前还不支持原生的区域滚动特性
    $.mobile.defaultPageTransition = 'slide'; // 设置使用 Ajax 方式跳转的页面的默认过场动画
    $.mobile.defaultDialogTransition = 'pop'; // 设置使用 Ajax 方式的对话框的默认过场动画
    $.mobile.transitionFallbacks.slide = 'none';
    $.mobile.transitionFallbacks.pop = 'none';
    $.mobile.buttonMarkup.hoverDelay = 200; // 该属性设置触摸触摸某一个 jQuery Mobile 按钮后添加 hover 和 down 的 class 的延时。该数值越小，延时越小，触摸越灵敏，但同时很有可能错误的触发页面滚动条滚动。因此建议数值不要太小。
    $.mobile.loadingMessage = '页面载入中';  // 设置当页面显示加载提示时，加载提示文字的内容。
    $.mobile.pageLoadErrorMessage = '页面载入失败';  // 设置当 Ajax 加载页面错误时显示的提示信息。
    $.mobile.page.prototype.options.domCache = true; // DOM caching

    // $.mobile.defaultHomeScroll = 0; // changepage的時候會抖一下

    jqmReadyDeferred.resolve();
});

Date.prototype.DateAdd = function (strInterval, Number) {
    var dtTmp = this;
    switch (strInterval) {
        case 's': return new Date(Date.parse(dtTmp) + (1000 * Number));
        case 'n': return new Date(Date.parse(dtTmp) + (60000 * Number));
        case 'h': return new Date(Date.parse(dtTmp) + (3600000 * Number));
        case 'd': return new Date(Date.parse(dtTmp) + (86400000 * Number));
        case 'w': return new Date(Date.parse(dtTmp) + ((86400000 * 7) * Number));
        case 'q': return new Date(dtTmp.getFullYear(), (dtTmp.getMonth()) + Number * 3, dtTmp.getDate(), dtTmp.getHours(), dtTmp.getMinutes(), dtTmp.getSeconds());
        case 'm': return new Date(dtTmp.getFullYear(), (dtTmp.getMonth()) + Number, dtTmp.getDate(), dtTmp.getHours(), dtTmp.getMinutes(), dtTmp.getSeconds());
        case 'y': return new Date((dtTmp.getFullYear() + Number), dtTmp.getMonth(), dtTmp.getDate(), dtTmp.getHours(), dtTmp.getMinutes(), dtTmp.getSeconds());
    }
}

function commonInit() {
    var keyname = window.localStorage.getItem(localAccount);
    if (keyname === null) {
        //$.mobile.changePage("sign-in.html");
        window.location.href = 'sign-in.html';
        return;
    }
    userObject = $.parseJSON(keyname);

    var dt = new Date(userObject.time);
    dt = dt.DateAdd('d', 10);

    //登陆超过10天重新登陆
    if (dt < new Date()) {
        $.mobile.loading('show', { text: '加载中...', textVisible: true });

        if (platform === "cordova") {
            var param = {
                host: userObject.hostcode,
                username: userObject.username,
                password: userObject.password,
                uuid: device.uuid,
                model: device.model
            };

            $.ajax({
                url: serverUrl + '/Account/AppLogin',
                type: "POST",
                timeout: 10000,
                data: param,
                success: function (result) {
                    if (result.code === 1) {
                        userObject.time = new Date().getTime(); // 更新时间
                        userObject.branch = result.OrganId;
                        userObject.branchName = result.BranchName;
                        userObject.hostcode = result.HostCode;
                        userObject.host = result.HostId;
                        userObject.userid = result.Id;
                        userObject.uuid = device.uuid;
                        userObject.type = result.Type;

                        userObject.majorPercentage = result.MajorPercentage;
                        userObject.majorBeauticianPercentage = result.MajorBeauticianPercentage;
                        userObject.beauticianPercentage = result.BeauticianPercentage;
                        userObject.percentageLock = result.PercentageLock;

                        window.localStorage.setItem(localAccount, JSON.stringify(userObject));
                        $.mobile.loading('hide');

                    } else {
                        // 重新登陆
                        window.location.href = 'sign-in.html';
                    }
                    // This callback function will trigger on successful action
                },
                error: function (request, error) {
                    // This callback function will trigger on unsuccessful action
                    showNetErrMsg();
                }
            });
        } else {
            var param1 = {
                host: userObject.hostcode,
                username: userObject.username,
                password: userObject.password
            };
            $.ajax({
                url: serverUrl + '/Account/AjaxLogin',
                type: "POST",
                data: param1,
                success: function (result) {
                    console.log("url return:" + result.code);
                    if (result.code === '1') {
                        alert("code is char");
                    }
                    if (result.code === 1) {
                        userObject.time = new Date().getTime();
                        userObject.host = result.HostId;
                        userObject.hostcode = result.HostCode;
                        userObject.branch = result.OrganId;
                        userObject.branchName = result.BranchName;
                        userObject.userid = result.Id;

                        userObject.majorPercentage = result.MajorPercentage;
                        userObject.majorBeauticianPercentage = result.MajorBeauticianPercentage;
                        userObject.beauticianPercentage = result.BeauticianPercentage;
                        userObject.percentageLock = result.PercentageLock;

                        window.localStorage.setItem(localAccount, JSON.stringify(userObject));
                        window.location.href = 'index.html';
                    } else {
                        window.location.href = 'sign-in.html';
                        // alert(result.message);
                    }
                    // This callback function will trigger on successful action
                },
                error: function (request, error) {
                    // This callback function will trigger on unsuccessful action
                    showNetErrMsg();
                }
            });
        }
    }
}

function commonList(ui, type) {
    var $ul = $(ui);
    var html = "";
    $.ajax({
        type: "POST",
        dataType: "json",
        data: { "hostId": userObject.host, "type": type },
        url: serverUrl + "/Dictionary/AppList",
        success: function (data) {
            $(data).each(function (i, item) {
                html += "<option value=\"" + item.code + "\">" + item.name + "</option>";
            });
            $ul.html(html);

            //if ($(ui + '-button').hasClass('ui-btn')) {
            $ul.selectmenu("refresh");
            //}
        },
        error: function (request, error) {
            // This callback function will trigger on unsuccessful action
            showNetErrMsg();
        }
    });
}

function getDate(time) {
    if (typeof (time) !== "undefined" && time !== "" && time !== null) {
        var date = new Date(parseInt(time.replace(/\D/igm, "")));
        return date.getFullYear() + "-" + eval(date.getMonth() + 1) + "-" + date.getDate();
    }
    return "";
}

function getDateTime(time) {
    if (typeof (time) !== "undefined" && time !== "" && time !== null) {
        var date = new Date(parseInt(time.replace(/\D/igm, "")));
        return date.getFullYear() + "-" + eval(date.getMonth() + 1) + "-" + date.getDate() + " " + date.getHours() + ":" + date.getMinutes();
    }
    return "";
}

function getLocaleDate(mydate) {
    if (typeof (mydate) !== "undefined" && mydate !== "") {
        return mydate.getFullYear() + "年" + eval(mydate.getMonth() + 1) + "月" + mydate.getDate() + "日";
    }
    return "";
}

function fail(e) {
    console.log("FileSystem Error");
    console.dir(e);
}

function writeLog(str) {
    if (!logOb) return;
    var log = str + " [" + (new Date()) + "]\n";
    console.log("going to log " + log);

    logOb.createWriter(function (fileWriter) {

        fileWriter.onwriteend = function () {
            console.log("Successful file write...");
            readFile(logOb);
        };

        fileWriter.onerror = function (e) {
            console.log("Failed file write: " + e.toString());
        };

        //fileWriter.seek(fileWriter.length);
        var blob = new Blob([log], { type: 'text/plain' });
        fileWriter.write(blob);
        console.log("ok, in theory i worked");
    }, fail);
}

function onErrorReadFile(e) {
    console.log("FileSystem Error");
    console.dir(e);
}

function readFile(fileEntry) {

    fileEntry.file(function (file) {
        var reader = new FileReader();

        reader.onloadend = function () {
            console.log("Successful file read: " + this.result);
            console.log("file URL: " + fileEntry.toURL());
            console.log("file full path: " + fileEntry.fullPath);
            // displayFileData(fileEntry.fullPath + ": " + this.result);
        };

        reader.readAsText(file);

    }, onErrorReadFile);
}

function showNetErrMsg() {
    $.mobile.toast({
        message: "网络连接故障，请重新再试!",
        position: "bottom"
    });
}

var getUrlParameter = function getUrlParameter(sParam) {
    var sPageURL = decodeURIComponent(window.location.search.substring(1)),
        sURLVariables = sPageURL.split('&'),
        sParameterName,
        i;

    for (i = 0; i < sURLVariables.length; i++) {
        sParameterName = sURLVariables[i].split('=');

        if (sParameterName[0] === sParam) {
            return sParameterName[1] === undefined ? true : sParameterName[1];
        }
    }
};
