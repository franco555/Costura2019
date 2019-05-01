
jQuery(document).on('click', '.mega-dropdown', function (e) {
    e.stopPropagation();
});


$(document).ready(function () {

    var boolens = false;
    
    if ($("#datepicker").length > 0) { //if ( $("#undiv")[0] )
        loadFormatDatePickers();
    }

    //$('#datepicker').datepicker();

    //Menu slider
    $(".Ec-btn-Menu").click(function () { $(".Ec-ContentMenu").slideToggle("slow"); return false; });
    $(document).on("click", function (e) {
        e.stopPropagation();
        var container = $(".Ec-ContentMenu");
        if (!container.is(e.target) && container.has(e.target).length === 0) {
            if ($('.Ec-ContentMenu').css('display') === 'none') {
                var exc = "exc";
            } else {
                $(".Ec-ContentMenu").slideUp("slow");
            }
        }
    });

    //Cargar la ventana Modal
    $(document).on("click", "a.dialog-window", null, function (e) {
        e.preventDefault();

        var url = $(this).attr('href');
        var attr_option = $(this).attr("data-option");
        var title = $(this).attr("data-title");
        var classed = $(this).attr("id");

        $("#ModalTitle").removeClass();
        $("#ModalTitle").addClass("modal-header");

        WindowsModals(title, url, attr_option);
        
    });

    //Cargar Provincia
    $(document).on('change', 'select[name="CountryId"]', function (event) {
        if ($("Select[name='CountryId']").length > 0) {
            ReloadSelect("ProvinceId");
            ReloadSelect("CityId");
            $.ajax({
                type: 'POST',
                url: '/Generic/GetProvinces',
                dataType: 'json',
                data: { Id: $("Select[name='CountryId']").val() },
                success: function (data) {
                    $.each(data, function (i, data) {
                        $("Select[name='ProvinceId']").append('<option value="' + data.ProvinceId + '">' + data.Name + '</option>');
                    });
                },
                error: function (ex) {
                    MsgError("Error Change Select", 'Error al cargar Provincia.', ex, "no");
                    var s = JSON.stringify(ex);
                    console.log(ex);
                    console.log("ayuda: " + s);
                }
            });
        }
        return false;
    });

    //Cargar Ciudad
    $(document).on('change', "Select[name='ProvinceId']", function (event) {
        if ($("Select[name='CityId']")[0]) {
            ReloadSelect("CityId");
            $.ajax({
                type: 'POST',
                url: '/Generic/GetCities',
                dataType: 'json',
                data: { IdC: $("Select[name='CountryId']").val(), IdP: $("Select[name='ProvinceId']").val() },
                success: function (data) {
                    $.each(data, function (i, data) {
                        $("Select[name='CityId']").append('<option value="' + data.CityId + '">' + data.Name + '</option>');
                    });
                },
                error: function (ex) {
                    MsgError("Error En Select Provincia", 'Error al cargar Ciudad.', ex, "no");
                    console.log(ex);
                }
            });
        }
        return false;
    });

    //Cargar Categoria de documento
    $(document).on('change', "Select[name='TypeDocumentId']", function (event) {
        if ($("Select[name='CategoryDocumentId']")[0]) {
            ReloadSelect("CategoryDocumentId");
            $.ajax({
                type: 'POST',
                url: '/Generic/GetCategoryDocuments',
                dataType: 'json',
                data: { Id: $("Select[name='TypeDocumentId']").val() },
                success: function (data) {
                    $.each(data, function (i, data) {
                        $("Select[name='CategoryDocumentId']").append('<option value="' + data.CategoryDocumentId + '">' + data.Name + '</option>');
                    });
                },
                error: function (ex) {
                    MsgError("Error En Select CategoryDocument", 'Error al cargar Categoria Documento.', ex, "no");
                    console.log(ex);
                }
            });
        }
        return false;
    });


    //Cargar Clasificacion de produto
    $(document).on('change', "Select[name='ProductId']", function (event) {
        if ($("#Formulario")[0]) {
            ReloadSelect("ClassificationId");

            $.ajax({
                type: 'POST',
                url: '/Generic/GetClassifications',
                dataType: 'json',
                data: { Idp: $("Select[name='ProductId']").val(), Idc: $("input[name='CompanyId']").val() },
                success: function (data) {
                    $.each(data, function (i, data) {
                        $("Select[name='ClassificationId']").append('<option value="' + data.ClassificationId + '">' + data.Name + '</option>');
                    });

                },
                error: function (ex) {
                    MsgError("Error En Select Classification", 'Error al cargar Clasificacion de Producto.', ex, "no");
                    console.log(ex);
                }
            });
        }
        return false;
    });

    //Cargar Unidad
    $(document).on('change', "Select[name='ClassificationId']", function (event) {
        if ($("#Formulario")[0]) {
            ReloadSelect("MeasuredUnitId");

            $.ajax({
                type: 'POST',
                url: '/Generic/GetMeasuredUnits',
                dataType: 'json',
                data: {
                    IdPr: $("Select[name='ProductId']").val(),
                    IdCl: $("Select[name='ClassificationId']").val(),
                    IdCo: $("input[name='CompanyId']").val()
                },
                success: function (data) {
                    $.each(data, function (i, data) {
                        $("Select[name='MeasuredUnitId']").append('<option value="' + data.MeasuredUnitId + '">' + data.Name + '</option>');
                    });
                },
                error: function (ex) {
                    MsgError("Error En Select Classification", 'Error al cargar Unidad de Producto.', ex, "no");
                    console.log(ex);
                }
            });
        }
        return false;
    });

    //Cargar Price
    $(document).on('change', "Select[name='MeasuredUnitId']", function (event) {
        if ($("#Formulario")[0]) {
            $("input[name='Price']").val("");
            $.ajax({
                type: 'POST',
                url: '/Generic/GetPrice',
                dataType: 'json',
                data: {
                    IdPr: $("Select[name='ProductId']").val(),
                    IdMe: $("Select[name='MeasuredUnitId']").val(),
                    IdCl: $("Select[name='ClassificationId']").val(),
                    IdCo: $("input[name='CompanyId']").val()
                },
                success: function (data) {
                    $("input[name='Price']").val(data);
                },
                error: function (ex) {
                    MsgError("Error En Select MeasuredUnit", 'Error al cargar Precio de Producto.', ex, "no");
                    console.log(ex);
                }
            });
        }
        return false;
    });

    //Eliminar Registro
    $(document).on('click', 'a.Delete-reg', function (event) {
        var id = $(this).attr("Data-id");
        var text = $.trim($(".data-" + id).text());
        var controller = "Delete" + $(this).attr("data-controller");

        DeleteConfirm(text, id, controller);
    });

    //Boton Guardar de formulario
    $(document).on('click', 'a#SaveAs', function (event) {
        var frm = $(this).attr("data-frm");
       // Validate_Frm(frm);
    });

    //Guardar y agreagar en kardex
    $(document).on('click', 'a#SaveDocumentEntry', function (event) {
        var id = $(this).attr("Data-id");

        $.ajax({
            type: 'POST',
            url: '/Generic/AddKardex',
            dataType: 'json',
            data: { Id: id },
            success: function (data) {
                var msg = "Factura Guardado!";
                if (data === "OK") {
                    MsgSuccess("Guardado!.", msg, "", "yes");
                } else {
                    MsgError("No Guardado!.", "Los Productos no se guardaron.", data, "no");
                }
            },
            error: function (ex) {
                MsgError("Error!.", msg, ex, "no");
            }
        });

        return false;
    });

    //Guardar y agreagar en kardex
    $(document).on('click', 'a#DocumentEntryDetailCancel', function (event) {
        var id = $(this).attr("Data-id");

        $.ajax({
            type: 'POST',
            url: '/Generic/DocumetEntryDetailCancel',
            dataType: 'json',
            data: { Id: id },
            success: function (data) {
                var msg = "Factura Calcelda!";
                if (data === "OK") {
                    MsgSuccess("Cancelado!.", msg, "", "yes");
                } else {
                    MsgError("No Cancelado!.", "Los Productos no se Cancelaron.", data, "no");
                }
            },
            error: function (ex) {
                MsgError("Error!.", msg, ex, "no");
            }
        });

        return false;
    });

    //Cerrar Session
    $(document).on('click', '.btn-Salir-more', function (event) {document.getElementById('logoutForm').submit();});

    //Activar boton submit de User
    $(document).on('change', 'select[name="rolId"]', function (event) {if ($("Select[name='rolId']").val() > 0) {$("#btn-submitUser").show();$("#btn-NotSubmitUser").hide();} else {$("#btn-submitUser").hide();$("#btn-NotSubmitUser").show();}return false;}); //if ($(this).attr("name")) { }
});


function Validate_Frm(frm) {
    var check = false;
    var itemsHidden = 0;
    var itemsOk = 0;

    var forms = document.forms;
    var count = 0;

    for (var i = 0; i < forms.length; i++) { if (forms[i].name === frm) { count = forms[i].elements.length; break; } }

    $("." + frm).find(':input').each(function () {
        var element = this;
        var name = element.name;
        var type = element.type;
        var value = element.value;
        var selectedIndex = element.selectedIndex;

        if (type !== "hidden" && ignoreFiels(name)) {
            itemsOk += 1;
            switch (type) {
                case "text": check = validateText(name, value); break;
                case "number": check = validateNumber(name, value); break;
                //case "tel": check = validateTel(name, value); break;
                case "email": check = validateEmail(name, value); break;
                //case "checkbox": check = false; break;
                //case "radio": check = (radioChecked) ? validateChecked(name, checked) : false; break;
                case "select-one": check = validateSelect(name, selectedIndex); break;
                default: check = false; break;
            }
        } else { itemsHidden += 1; check = true; }

        if (!check) {
            return false;
        }
    });
    //alert(itemsHidden + "-" + itemsOk);
    if (count - itemsHidden === itemsOk && check === true) {

        $("." + frm).submit();
        $("." + frm)[0].reset();
    }
}

//validar datos vacios
function validateDate(value) { return value === "" ? false : true; }

//validar select
function validateSelect(name, i) {
    if (i <= 0) {
        messageForm(name, "s", "");
        return false;
    }
    return true;
}

// validar text
function validateText(name, value) {
    var check;
    if (validateDate(value)) {
        check = true;
    } else {
        messageForm(name, "i", "");
        check = false;
    }
    return check;
}
//validar si es un numero
function validateNumber(name, value) {
    var check;
    if (validateDate(value)) {
        var msg = "Ingrese sólo números.!";
        if (isNaN(value) === false) {
            check = false;
        } else {
            check = true;
            messageForm(name, "i", msg);
        }
    } else {
        messageForm(name, "i", "");
        check = true;
    }
    return check;
}
//validar telefono
function validateTel(name, value) {
    var check;
    check = validateNumber(name, value)? true : false;
    return check;
}
//validaar email
function validateEmail(name, value) {
    var check;
    var msg = "";
    if (validateDate(value)) {
        msg = "Ingrese un correo electrónico válido.!";
        var exp = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
        if (exp.test(value)) {
            check = true;
        } else {
            messageForm(name, "i", msg);
            check = false;
        }
    } else {
        msg = "Ingrese un E-Mail.!";
        messageForm(name, "i", msg);
        check = false;
    }
    return check;
}


function messageForm(name, item, ex) {

    var inp = "";
    var nameText = $("label[for=" + name + "]").text();

    if (item === "i") {
        inp = "input[name=" + name + "]";
    } else if (item === "s") {
        inp = "select[name=" + name + "]";
    }


    $(inp).focus();
    MsgError("Formulario!", "El Campo <strong>[ " + nameText + " ]</strong> Está Vacío!", ex, "no");
}

function ignoreFiels(name) {
    var ch = false;
    switch (name) {
        case "Name": ch = true; break;
        case "Email": ch = true; break;
        case "CountryId": ch = true; break;
        case "ProvinceId": ch = true; break;
        case "CityId": ch = true; break;
        case "IvaConditionId": ch = true; break;
        case "Date": ch = true; break;
        default: ch = false; break;
    }
    return ch;
}

function loadFormatDatePickers() {
    $('#datepicker').datetimepicker({format: 'YYYY/MM/DD'});
    $('#datepicker2').datetimepicker({ format: 'YYYY/MM/DD' });
}

function loadSelect() {

}

function ReloadSelect(id) {
    $("Select#" + id).empty();
    $("Select#" + id).append('<option value=0>[Seleccionar...]</option>');
}

function MsgError(title, msg, ex, act) {
    $.confirm({
        title: title,
        content:"<b>"+ msg + " </b> <hr> " + ex,
        type: 'red',
        typeAnimated: true,
        buttons: {
            tryAgain: {
                text: 'Aceptar',
                btnClass: 'btn-red',
                action: function () {
                    if (act === "yes") {
                        location.reload();
                    }
                }
            }
        }
    });
}

function MsgSuccess(title, msg, ex, act) {
    $.confirm({
        title: title,
        content: "<b>"+ msg + " </b> <hr> " + ex,
        type: 'green',
        typeAnimated: true,
        buttons: {
            tryAgain: {
                text: 'Aceptar',
                btnClass: 'btn-green',
                action: function () {
                    if (act === "yes") {
                        location.reload();
                    }
                }
            }
        }
    });
}

function DeleteConfirm(msg, id, controller) {
    $.confirm({
        title: 'Eliminar!',
        content: 'Seguro que desea eliminar! <br> <strong>' + msg + '</strong >',
        typeAnimated: true,
        type: 'red',

        buttons: {
            confirm: {
                btnClass: 'btn-blue',
                text: 'Eliminar',
                keys: ['enter'],
                action: function () {
                    if (id > 0) {
                        DeleteEject(id, msg, controller);
                    } else {
                        MsgError("Error!.", "No hay Id de: ", msg, "no");
                    }
                }
            },
            cancel: {
                btnClass: 'btn-red',
                text: 'Cancelar',
                keys: ['esc'],
                action: function () {

                }
            }
        }
    });
}

function DeleteEject(id, msg, controller) {
    $.ajax({
        type: 'POST',
        url: '/Generic/' + controller,
        dataType: 'json',
        data: { Id: id },
        success: function (data) {
            if (data === "OK") {
                MsgSuccess("Eliminado!.", msg, "", "yes");
            } else {
                MsgError("No eliminado!.", msg, data, "no");
            }
        },
        error: function (ex) {
            MsgError("Error!.", msg, ex, "no");
        }
    });

    return false;
}

function WindowsModals(title, url, class_attr) {
    $('#WindowModal .modal-title').html(title);

    if (class_attr === "Create") {
        $("#ModalTitle").addClass("titleModalCreate");
    } else if (class_attr === "Edit") {
        $("#ModalTitle").addClass("titleModalEdit");
    } else {
        $("#ModalTitle").addClass("titleModalDetail");
    }
    

    if (url.indexOf('#') === 0) {
        $('#WindowModal').modal('show');
    }
    else {
        $.get(url, function (data) {
            $('#WindowModal .te').html(data);
            $('#WindowModal').modal();
            if ($("#datepicker").length > 0) { loadFormatDatePickers(); }

            if ($("form.form").length > 0) {  
                //$("input[name=" + PushCursor +"]").bind('focus').focus();
                $('input:text:visible:first').focus();
            }
        });

    }
    
}

function PushCursor() {
    var nameInput = "No Existe";
    $(".form").find(':input').each(function () { if (this.type !== "hidden") { nameInput = this.name; return false; } });
    return nameInput;
}
