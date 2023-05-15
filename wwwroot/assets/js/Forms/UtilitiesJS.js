
async function ToolsFuncBtnLoader($Btn, Ocultar) {
    if (Ocultar == 1) {
        $Btn.style.display = 'none'
        $('#' + $Btn.id + '-load').show()
    } else {
        $Btn.style.display = ''
        $('#' + $Btn.id + '-load').hide()
    }
}
function NewValidarVacios($elementRequired) {
    var swi = true
    for (var i = 0; i < $elementRequired.length; ++i) {
        if (($elementRequired[i].value == "" & $elementRequired[i].disabled == false)) {
            $elementRequired[i].classList.add('inputRequired-error')
            swi = false
        } else if ($elementRequired[i].classList.contains('has-error')) {
            $elementRequired[i].classList.add('inputRequired-error')
            $elementRequired[i].focus()
            swi = false
        } else {
            $elementRequired[i].classList.remove('inputRequired-error')
        }
    }
    return swi;
}
function GenerarDTFiltros(IdDiv) {
    try {
        var IdTable = $(`#${IdDiv} table`).attr("id");
        $(`#${IdTable} thead tr`).clone(true).appendTo(`#${IdTable} thead`);
        $(`#${IdTable} thead tr:eq(0) th`).each(function (i) {
            var title = $(`#${IdTable} thead th`).eq($(this).index()).text();
            var $input = `<input type="text"  class="form-control form-control-sm" placeholder="${title}" data-index="${i}" />`;
            $(this).html((i > 0 ? $input : ""));
            //$(this).html($input);
        });
        // DataTable
        var table = $(`#${IdTable}`).DataTable({
            oLanguage: {
                "sProcessing": "Procesando...",
                "sLengthMenu": "Mostrar _MENU_ registros",
                "sZeroRecords": "No se encontraron resultados",
                "sEmptyTable": "Ningún dato disponible en esta tabla",
                "sInfo": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
                "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
                "sInfoFiltered": "(filtrado de un total de _MAX_ registros)",
                "sInfoPostFix": "",
                "sSearch": "Ingrese Fecha:",
                "sUrl": "",
                "sInfoThousands": ",",
                "sLoadingRecords": "Cargando...",
                "oPaginate": {
                    "sFirst": "Primero",
                    "sLast": "Último",
                    "sNext": "Siguiente",
                    "sPrevious": "Anterior"
                },
                "oAria": {
                    "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
                    "sSortDescending": ": Activar para ordenar la columna de manera descendente"
                }
            },
            paging: true,
            searching: true,
            //scrollY: "400px",
            //responsive: true,
            //dom: 'Bfrtip',//'<"html5buttons"B>lTfgitp',
            //stateSave: true,
            scrollX: true
        });
        // Filter event handler
        $(table.table().container()).on('keyup', 'thead tr:eq(0) input', function () {
            table
                .column($(this).data('index'))
                .search(this.value)
                .draw();
        });
    } catch (ex) {
        //console.log(JSON.stringify(ex));
        //swal("Error JavaScript", "GenerarDT():" + ex, "error");
    }
}
function GenerarDTMultiSelect(IdDiv) {
    var IdTable = $('#' + IdDiv + ' table').attr("id");
    $('#' + IdTable + ' thead tr').clone(true).appendTo('#' + IdTable + ' thead');
    $('#' + IdTable + ' thead tr:eq(0) th').each(function (i) {
        var title = $('#' + IdTable + ' thead th').eq($(this).index()).text();
        var $input = `<input type="text" class="form-control form-control-sm input-sm" placeholder="${title}" data-index="${i}" />`;
        $(this).html($input);
    });
    // DataTable
    var table = $('#' + IdTable).DataTable({
        oLanguage: {
            "sProcessing": "Procesando...",
            "sLengthMenu": "Mostrar _MENU_ registros",
            "sZeroRecords": "No se encontraron resultados",
            "sEmptyTable": "Ningún dato disponible en esta tabla",
            "sInfo": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
            "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
            "sInfoFiltered": "(filtrado de un total de _MAX_ registros)",
            "sInfoPostFix": "",
            "sSearch": "Buscar:",
            "sUrl": "",
            "sInfoThousands": ",",
            "sLoadingRecords": "Cargando...",
            "oPaginate": {
                "sFirst": "Primero",
                "sLast": "Último",
                "sNext": "Siguiente",
                "sPrevious": "Anterior"
            },
            "oAria": {
                "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
                "sSortDescending": ": Activar para ordenar la columna de manera descendente"
            }
        },
        paging: false,
        keys: true,
        scrollX: true,
        scrollY: "400px",
        searching: true,
        select: {
            style: 'multi'
        }
    });
    // Filter event handler
    $(table.table().container()).on('keyup', 'thead tr:eq(0) input', function () {
        table
            .column($(this).data('index'))
            .search(this.value)
            .draw();
    });
}
function GenerarDTNotPagin(IdDiv) {
    try {
        var IdTable = $(`#${IdDiv} table`).attr("id");
        // DataTable
        var table = $(`#${IdTable}`).DataTable({
            searching: false,
            "paging": false,
            "ordering": false,
            "info": false
        });
    } catch (ex) {
        swal("Error JavaScript", "GenerarDTNotPagin():" + ex, "error");
    }
}
function ValidarVacios($elementRequired) {
    $elementRequired.attr('required', 'required');
    var swi = true;
    for (var i = 0; i < $elementRequired.length; ++i) {
        if (($elementRequired[i].value == "" & $elementRequired[i].disabled == false)) {
            swi = false;
            break;
        } else if ($elementRequired[i].classList.contains('is-invalid')) {
            swi = "error";
            break;
        }
    }
    return swi;
}
async function PermisosActions(Controller) {
    try {
        let Result = await $.getJSON('/' + Controller + '/PermisosActions');
        $.each(Result, function (i, data) {
            $('#' + data.Controller).remove();
            $('.' + data.Controller).remove();
        });
    } catch (ex) {
        alert("Error en UtilitiesJS.PermisosActions")
    } 
}
//SIN ESPACIOS
function sinespacios(e) {

    key = e.keycode || e.which;
    teclado = String.fromCharCode(key).toLowerCase();
    letras = "@.-/+_abcdefghijklmnñopqrstuvwxyz0123456789";
    especiales = "8-37-38-46-164";//array
    teclado_especial = false;

    for (var i in especiales) {
        if (key == especiales[i]) {
            teclado_especial = true; break;
        }
    }
    if (letras.indexOf(teclado) == -1 && !teclado_especial) {
        return false;
    }
}
//SIN ESPACIOS REGISTRO
function sinespaciosregis(e) {

    key = e.keycode || e.which;
    teclado = String.fromCharCode(key).toLowerCase();
    letras = "_abcdefghijklmnñopqrstuvwxyz0123456789";
    especiales = "8-37-38-46-164";//array
    teclado_especial = false;

    for (var i in especiales) {
        if (key == especiales[i]) {
            teclado_especial = true; break;
        }
    }
    if (letras.indexOf(teclado) == -1 && !teclado_especial) {
        return false;
    }
}
//SOLO NUMEROS
function solonumeros(e) {
    key = e.keycode || e.which;
    teclado = String.fromCharCode(key);
    numeros = "0123456789";
    especiales = "8-37-38-46";//array
    teclado_especial = false;

    for (var i in especiales) {
        if (key == especiales[i]) {
            teclado_especial = true;
        }
    }
    if (numeros.indexOf(teclado) == -1 && !teclado_especial) {
        return false;
    }
}
//SOLO LETRAS
function sololetras(e) {

    key = e.keycode || e.which;
    teclado = String.fromCharCode(key).toLowerCase();
    letras = " abcdefghijklmnñopqrstuvwxyz";
    especiales = "8-37-38-46-164";//array
    teclado_especial = false;

    for (var i in especiales) {
        if (key == especiales[i]) {
            teclado_especial = true; break;
        }
    }
    if (letras.indexOf(teclado) == -1 && !teclado_especial) {
        return false;
    }
}

async function ToolsDataTables(divTable) {
    try {
        let idDiv = divTable ? divTable : "divTable";

        var result = await GenerarDTFiltros(idDiv)

    } catch (ex) {
        console.log(ex)
    } finally {
        $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
        $('[data-toggle="popover"]').popover();
    }
}

function aplicarFormato(obj, e, type) {



    let cadena = obj.value;

    let swOK = false;



    if (type == "CUN") {

        // Permito Enter

        if (e.keyCode == 13) { swOK = true; }



        // Dígitos del 0 al 9

        if ((e.keyCode < 48 || e.keyCode > 57) && !swOK) {

            return false;

        }

    }

    if (type == "CUSTOMERID") {

        // Permito Enter

        if (e.keyCode == 13) { swOK = true; }



        // Dígitos del 0 al 9

        if ((e.keyCode < 48 || e.keyCode > 57) && !swOK) {

            return false;

        }

    }



    if (type == "CUSTCODE") {

        // Permito Enter

        if (e.keyCode == 13) { swOK = true; }



        // Valido el punto

        if (e.keyCode == 46) {

            if (cadena.includes(".") || cadena.length == 0) {

                return false;

            }

            if (cadena.length != 1) {

                return false;

            }

            swOK = true;

        }



        if ((e.keyCode < 48 || e.keyCode > 57) && !swOK) {

            return false;

        }

    }



    if (type == "DECIMAL") {

        // Permito Enter

        if (e.keyCode == 13) { swOK = true; }



        // Valido el punto

        if (e.keyCode == 46) {

            if (cadena.includes(".") || cadena.length == 0) {

                return false;

            }

            swOK = true;

        }



        if ((e.keyCode < 48 || e.keyCode > 57) && !swOK) {

            return false;

        }

    }

}

function validarFormato(obj, type) {



    let cadena = obj.value;



    if (obj.value.length == 0) { return false; }



    if (type == "CUN") {

        if (obj.value.length < 11) {

            alert("Digite un CUN válido. Verifique.");

            obj.value = "";

            return false;



        }

    }



    if (type == "CUSTCODE") {

        if (cadena.indexOf(".") != 1 || obj.value.length < 10) {

            alert("Digite un CUSTCODE válido. Verifique.");

            obj.value = "";

            return false;

        }

    }



    if (type == "DECIMAL") {

        cadena = cadena.replace(",", "").replace(",", "");



        if (isNaN(cadena)) {

            alert("Digite un número decimal válido. Verifique.");

            obj.value = "";

            return false;

        }



        if (cadena.includes(".")) {

            if (cadena.split(".")[1].length > 2) {

                let parteEntera = cadena.split(".")[0];

                let parteDecimal = cadena.split(".")[1].substring(0, 2);

                cadena = parteEntera + "." + parteDecimal;

            }

        }

        obj.value = cadena;

    }



}

function validarInput(elementId, elementType) {
    var swError = false;
    if (elementType == "text") {



        var element = $("#" + elementId);
        if (element.val() == "") {
            element.css({ "border": "solid 1px red" });
            element.change();
            swError = true;
        } else {
            element.css({ "border": "solid 1px #CCC" });
            element.change();
        }



    }
    else if (elementType == 'select') {



        var element = $('#' + elementId);
        if (document.getElementById(elementId).selectedIndex == 0) {
            element.css({ 'border': 'solid 1px red' });
            swError = true;
        } else {
            element.css({ 'border': 'solid 1px #CCC' });
        }



    }
    else if (elementType == 'text2') {



        var element = $('#' + elementId);
        if (document.getElementById(elementId).slot == '') {
            element.css({ 'border': 'solid 1px red' });
            swError = true;
        } else {
            element.css({ 'border': 'solid 1px #CCC' });
        }



    }



    return swError;
}