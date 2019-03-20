
$(document).on('change', 'input[type=file]', function (e) {
    var TmpPath = URL.createObjectURL(e.target.files[0]);
    $('img#files').attr('src', TmpPath);
    $('input[name=URL]').val(TmpPath);
    handleFileSelect(e);
});



function handleFileSelect(e) {
    var files = e.target.files;
    var filesArr = Array.prototype.slice.call(files);

    filesArr.forEach(function (f, item) {
        if (f.type.match("image.*")) {
            var reader = new FileReader();
            reader.readAsDataURL(f);
            $("#NombreArchivo").empty();
            $("#NombreArchivo").attr("title", f.name);
            $("#NombreArchivo").append("<span class='glyphicon glyphicon-file kv-caption-icon' style='display:inline-block'></span>");
        }
        else {
            alert(f.name + ' Formato no permitido.');
            return;
        }
    });
}
