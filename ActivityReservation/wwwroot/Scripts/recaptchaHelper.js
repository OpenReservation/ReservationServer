var recaptchaHelper = {
    _recaptchaStyles: '<style>.recaptcha{margin: 0.4em auto;}</style>',
    _googleRecaptchaUrl: 'https://recaptcha.google.com/recaptcha/api.js',
    _geetestRecaptchaUrl: 'https://static.geetest.com/static/tools/gt.js',
    _loadScript: function (url, onSuccess, onError) {
        var script = document.createElement('script');
        script.type = 'text/javascript';
        script.async = 'async';
        script.src = url;
        document.body.appendChild(script);
        if (script.readyState) {   //IE
            script.onreadystatechange = function () {
                if (script.readyState === 'complete' || script.readyState === 'loaded') {
                    onSuccess();
                } else {
                    onError();
                }
            }
        } else {    //非IE
            script.onload = onSuccess;
            script.onerror = onError;
        }
    },
    _onGoogleRecaptchaLoadSuccess: function () {
        recaptchaHelper.recaptchaType = 'Google';
        if (location.href.indexOf('weihanli.xyz') < 1) {
            $(".btnSubmit").eq(0)
                .before('<div class="g-recaptcha recaptcha" data-sitekey="6Lc-vz0UAAAAAG38zeZKM7_pQL5k4Z7FpnrtU_3A"></div>');
        } else {
            $(".btnSubmit").eq(0)
                .before('<div class="g-recaptcha recaptcha" data-sitekey="6Lc56zkUAAAAAOGHzUyKh5pCW2c7bPlNmra0R3NW"></div>');
        }
    },
    _onGoogleRecaptchaLoadError: function () {
        $("script").last().remove();
        // GoogleRecaptcha 加载失败自动使用极验验证码
        recaptchaHelper._loadScript(recaptchaHelper._geetestRecaptchaUrl, recaptchaHelper._onGeetestRecaptchaLoadSuccess, recaptchaHelper._onGeetestRecaptchaLoadError);
    },
    _onGeetestRecaptchaLoadSuccess: function () {
        recaptchaHelper.recaptchaType = 'Geetest';
        $(".btnSubmit").eq(0).before('<div id="geetestCaptcha" class="recaptcha"></div>');
        //
        $.ajax({
            // 获取id，challenge，success（是否启用failback）
            url: "/Home/GetGeetestValidCode",
            type: "get",
            dataType: "json",
            success: function (data) {
                // 使用initGeetest接口
                // 参数1：配置参数，与创建Geetest实例时接受的参数一致
                // 参数2：回调，回调的第一个参数验证码对象，之后可以使用它做appendTo之类的事件
                initGeetest({
                    gt: data.gt,
                    challenge: data.challenge,
                    //product: "float", // 产品形式，值可以是 float 或者 popup。默认值 popup
                    offline: !data.success
                },
                    function (captchaObj) {
                        // 将验证码加到id为captcha的元素里
                        $("#geetestCaptcha").html(''); //清空元素
                        captchaObj.appendTo("#geetestCaptcha");
                        // success
                        captchaObj.onSuccess(function () {
                            var result = captchaObj.getValidate();
                            var geetest = {
                                challenge: result.geetest_challenge,
                                validate: result.geetest_validate,
                                seccode: result.geetest_seccode
                            };
                            recaptchaHelper._geetestRecaptchaResponse = JSON.stringify(geetest);
                        });
                    });
            }
        });
    },
    _onGeetestRecaptchaLoadError: function () {
        $("script").last().remove();
        recaptchaHelper.recaptchaType = 'None';
    },
    _geetestRecaptchaResponse: '',

    init: function () {
        document.write(recaptchaHelper._recaptchaStyles);
        // loadGoogleRecaptcha firstly
        recaptchaHelper._loadScript(recaptchaHelper._googleRecaptchaUrl, recaptchaHelper._onGoogleRecaptchaLoadSuccess, recaptchaHelper._onGoogleRecaptchaLoadError);
        // load geetest only
        // recaptchaHelper._loadScript(recaptchaHelper._geetestRecaptchaUrl, recaptchaHelper._onGeetestRecaptchaLoadSuccess, recaptchaHelper._onGeetestRecaptchaLoadError);
    },
    recaptchaType: 'Google',
    getRecaptchaResponse: function () {
        if (recaptchaHelper.recaptchaType === 'None')
            return 'recaptchaType none';
        if (recaptchaHelper.recaptchaType === 'Geetest')
            return recaptchaHelper._geetestRecaptchaResponse;
        if (recaptchaHelper.recaptchaType === 'Google')
            return $("#g-recaptcha-response").val();
    }
};
recaptchaHelper.init();