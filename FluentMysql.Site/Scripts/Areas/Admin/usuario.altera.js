jQuery(document).ready(function ($) {
    $page.iniciar();
});

var $page = new function () {
    this.iniciar = function () {
        $('*[action="excluir"]').bind({
            'click': function (evento) {
                return confirm("Deseja realmente excluir este registro?");
            }
        });
    };
};