var Toast = (function () {
    function Toast() {
    }
    Toast.prototype.all = function (urls) {
        var _this = this;
        return Promise.all(urls.map(function (url) {
            switch (url.split('.').pop().toLowerCase()) {
                case 'css':
                    return _this.css(url);
                case 'js':
                    return _this.js(url);
                default:
                    return Promise.reject(new Error("Unable to detect extension of '" + url + "'"));
            }
        }));
    };
    Toast.prototype.css = function (url) {
        var link = document.createElement('link');
        link.rel = 'stylesheet';
        link.href = url;
        document.querySelector('head').appendChild(link);
        return this.promise(link);
    };
    Toast.prototype.js = function (url) {
        var script = document.createElement('script');
        script.src = url;
        document.querySelector('head').appendChild(script);
        return this.promise(script);
    };
    Toast.prototype.promise = function (element) {
        return new Promise(function (resolve, reject) {
            element.addEventListener('load', function () {
                resolve(element);
            });
            element.addEventListener('error', function () {
                reject();
            });
        });
    };
    return Toast;
}());
export default new Toast();
