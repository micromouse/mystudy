/// <reference path="oidc-client.js" />
$(document).ready(function () {
    var config = {
        authority: 'http://192.168.1.20:5000',
        client_id: 'javascript',
        popup_redirect_uri: 'http://localhost:5003/popupcallback.html',
        redirect_uri: 'http://localhost:5003/login-callback.html',
        silent_redirect_uri: 'http://localhost:5003/silent.html',
        response_type: 'id_token token',
        scope: 'openid profile lysalesplatform',
        post_logout_redirect_uri: 'http://localhost:5003',
        automaticSilentRenew: true
    };
    var mgr = new Oidc.UserManager(config);

    //显示信息
    var ShowLog = function (content) {
        $('#results').prop('innerHTML', content);
    };

    //登录
    $('#login').click(function () {
        mgr.signinRedirect();
    });

    //弹出窗口登录
    $('#popuplogin').click(function () {
        mgr.signinPopup();
    });

    //调用webapi
    $('#api').click(function () {
        mgr.getUser().then(function (user) {
            if (user) {
                var url = 'http://localhost:5001/api/identity';
                var xhr = new XMLHttpRequest();

                xhr.open('get', url);
                xhr.onload = function () {
                    if (xhr.status == 401) {
                        ShowLog('认证失败');
                    } else if (xhr.status == 403) {
                        ShowLog('403错误');
                    } else {
                        ShowLog(xhr.status + '<br/>' + decodeURI(xhr.responseText) + '<br/>' + user.access_token);
                    }
                };

                xhr.setRequestHeader('If-Modified-Since', '0');
                xhr.setRequestHeader('Authorization', 'Bearer ' + user.access_token);
                xhr.send();
            } else {
                alert('没有登录');
            }
        });
    });

    //调用wcf服务
    $('#wcfservice').click(function () {
        mgr.getUser().then(function (user) {
            if (user) {
                var url = 'http://192.168.1.106:9070/MaintainSystemService/Test';
                var xhr = new XMLHttpRequest();

                xhr.open('post', url);
                xhr.onload = function () {
                    ShowLog(xhr.status + xhr.responseText);
                };
                xhr.setRequestHeader('Authorization', 'Bearer ' + user.access_token);
                xhr.send(JSON.stringify({ 'Id': 'ccea97ef-1e80-4ac4-b03d-7116bed91707' }));
            } else {
                mgr.signinRedirect();
            }
        });
    });

    //登出
    $('#logout').click(function () {
        mgr.signoutRedirect();
    });

    //显示登录状态
    mgr.getUser().then(function (user) {
        ShowLog(user ? 'User logged in' : 'User not logged in');
    });

});