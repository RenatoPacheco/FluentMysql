jQuery(document).ready(function ($) {
    $page.iniciar();
});

var $page = new function () {
    this.filtro = new function () {
        this.selecionados = 0;
        this.evento = new function () {
            this.ChangeCheckbox = function (evento) {
                $page.filtro.selecionados += $(this).is(':checked') ? 1 : -1;
            };
        };
        this.iniciar = function () {
            $('#filtro *[action="selecionaTodos"]').bind({
                'click': function (evento) {
                    $('#filtro input:checkbox').prop('checked', true);
                    return false;
                }
            });
            $('#filtro *[action="desselecionaTodos"]').bind({
                'click': function (evento) {
                    $('#filtro input:checkbox').prop('checked', false);
                    return false;
                }
            });
            $('#filtro *[action="excluirSelecionados"]').bind({
                'click': function (evento) {
                    return confirm("Deseja realmente excluir os itens selecionados?");
                }
            });
            this.atualizar();
        };
        this.atualizar = function () {
            $('#filtro input:checkbox')
                .unbind({ 'change': $page.filtro.evento.ChangeCheckbox })
                .bind({ 'change': $page.filtro.evento.ChangeCheckbox });
        };
    };
    this.iniciar = function () {
        this.filtro.iniciar();
    };
};