(function (document) {

    var menu = document.getElementById('menu'),
        menuLink = document.getElementById('menu-link');

    menuLink.onclick = function (e) {
        e.preventDefault();

        var activeClass = 'is-active';
        toggleClass(menu, activeClass);
        toggleClass(menuLink, activeClass);
    };

    function toggleClass(element, className) {
        var classes = element.className.split(/\s+/),
            length = classes.length,
            i = 0;

        for (; i < length; i++) {
            if (classes[i] === className) {
                classes.splice(i, 1);
                break;
            }
        }

        if (length === classes.length) {
            classes.push(className);
        }

        element.className = classes.join(' ');
    }

}(this.document));
