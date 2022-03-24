(function () {
    window.addEventListener("load", function () {
        setTimeout(function () {
            var link = document.getElementByClassName('link');
            link[0].href = "/Home/Index";
        });
    });
})();