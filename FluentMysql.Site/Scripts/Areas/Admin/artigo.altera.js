jQuery(document).ready(function ($) {
    $('#Hashtag').tagsinput({
        confirmKeys: [13, 188, 32]
    });

    $('textarea.basic').each(function (index) {
    }).ckeditor(function () {
        // Instance loaded callback.
    }, {
        //contentsCss: ['/Content/css/normalize.css', '/Content/css/template-editor.min.css', '/Content/css/template-ckeditor.css'],
        //on: { change: EventoChange, contentDom: EventoChange, paste: EventoPaste },
        pasteFromWordPromptCleanup: true,
        pasteFromWordRemoveFontStyles: true,
        pasteFromWordNumberedHeadingToList: true,
        pasteFromWordRemoveStyles: true,
        ignoreEmptyParagraph: true,
        removeFormatAttributes: true,
        toolbar: [
            ['Source', '-', 'Bold', 'Italic', 'Underline', 'Subscript', 'Superscript', '-', 'RemoveFormat'], ['NumberedList', 'BulletedList', '-', 'Indent', 'Outdent', 'Blockquote', 'JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock', '-'], ['Image']
        ],
        filebrowserBrowseUrl: 'browser/browse.php',
        filebrowserUploadUrl: '/Admin/Artigo/Upload',
        filebrowserImageWindowWidth: '640',
        filebrowserImageWindowHeight: '480'
    });

    $('*[action="excluir"]').bind({
        'click': function (evento) {
            return confirm("Deseja realmente excluir este registro?");
        }
    });
});