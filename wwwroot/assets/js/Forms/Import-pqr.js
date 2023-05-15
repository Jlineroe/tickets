$('#divLoaderMaster').hide();
$('.chosen-select').chosen();
async function ValidarlblErrors($labelsError) {
    var validaColumnas = true;
    for (var i = 0; i < $labelsError.length; ++i) {
        if ($labelsError[i].innerText != "") {
            validaColumnas = false;
            break;
        }
    }
    return validaColumnas;
}
async function FuncBtnLoader($BtnGuardar, Ocultar) {
    if (Ocultar == 1) {
        $BtnGuardar.style.display = 'none';
        $('.BtnLoading').show();
    } else {
        $BtnGuardar.style.display = '';
        $('.BtnLoading').hide();
    }
}
const $BtnSiguiente1 = document.getElementById('BtnSiguiente1');
const $BtnSiguiente2 = document.getElementById('BtnSiguiente2');
(async function load() {
    $BtnSiguiente1.addEventListener('click', async (event) => {
        $('.content-wrapper .form-control').removeAttr('required');
        try {
            var $elementRequired = $('#formCargarData .required');
            var Valida = ValidarVacios($elementRequired);
            if (Valida == false) {

            } else if (Valida == "is-invalid") {
                event.preventDefault();
            } else {
                event.preventDefault();
                FuncBtnLoader($BtnSiguiente1, 1);
                var params = new Object();
                params['IdTemplate'] = $('#DdlTemplate').val();
                var Result = await $.ajax({
                    type: "post",
                    url: '/ImportarPQR/TypesFields',
                    data: params
                });
                $('#divFieldsTemplate').html(Result);
                $('.chosen-select').chosen();
                $('#fileID').prop('disabled', true);
                $('#DdlTemplate').prop('disabled', true);
                $('#BtnSiguiente1').prop('disabled', true);
                $('#Btns1').hide();
                $('.divCampos').show();
            }
        } catch (ex) {
            swal("Error interno", "Comunicate con el area de IT para la verificacion de este error..", "error");
        } finally {
            setTimeout(function () {
                FuncBtnLoader($BtnSiguiente1, 0);
            }, 500);
        }
    });
    $BtnSiguiente2.addEventListener('click', async (event) => {
        $('.content-wrapper .form-control').removeAttr('required');
        try {
            var $elementRequired = $('#formFields .required');
            var Valida = ValidarVacios($elementRequired);
            
                event.preventDefault();
                FuncBtnLoader($BtnSiguiente2, 1);
                var $required = $('#formFields .lblrequired');
                debugger
                await ValidarObligatoriosVacios($required);
                var $labelsError = $('#formFields .lblErrores');
                var validaColumnas = await ValidarlblErrors($labelsError);
                if (validaColumnas == false) {
                    toastr.error("Tienes errores pendientes por corregir, favor verificar!", "Errores en columnas");
                } else {
                    $('#DdlHojaExcel').prop('disabled', true);
                    $('#Btns2').hide();
                    $('.divGroups').show();
                }
            
        } catch (ex) {
            swal("Error interno", "Comunicate con el area de IT para la verificacion de este error..", "error");
        } finally {
            FuncBtnLoader($BtnSiguiente2, 0);
        }
    });
    async function ValidarObligatoriosVacios($elementRequired) {
        for (var i = 0; i < $elementRequired.length; ++i) {
            var idSelect = $elementRequired[i].dataset.idfieldsudf;
            var valueSelect = $('#DdlFielsUDF_' + idSelect).val();
            var lblerror = $('#DdlFielsUDF_' + idSelect + '-error').text();
            if (valueSelect == "") {
                $('#DdlFielsUDF_' + idSelect + '-error').html('Este campo es obligatorio.');
            } else if (lblerror != "") {

            }else {
                $('#DdlFielsUDF_' + idSelect + '-error').html('');
            }
        }
    }
    const $BtnFinalizar = document.getElementById('BtnFinalizar');
    $BtnFinalizar.addEventListener('click', async (event) => {
        $('.content-wrapper .form-control').removeAttr('required');
        try {
            var $elementRequired = $('#formFinalizar .required');
            var Valida = ValidarVacios($elementRequired);
            if (Valida == false) {

            } else if (Valida == "error") {
                event.preventDefault();
            } else {
                event.preventDefault();
                var Grupo = $('#DdlGroups option:selected').text();
                var Usuarios = $("#DdlUsuarios").chosen().val();
                var msj = "a los usuarios seleccionados";
                if (Usuarios.length == 0)
                    msj = "a todos los usuarios del grupo " + Grupo;
                
                var Algoritmo = $('#DdlAlgoritmo option:selected').text();
                swal({
                    title: "¿Estas seguro?",
                    text: " ¿Deseas cargar y asignar estos registros " + msj+" con el algoritmo " + Algoritmo + "?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonText: "Si, Cargar!",
                    closeOnConfirm: true,
                    showLoaderOnConfirm: true
                }, async function () {
                    try {
                        FuncBtnLoader($BtnFinalizar, 1);
                        var $Ddl = $('.colummnExcel');
                        var ColumnsSQL = [];
                        var ColumnsExcel = [];
                        var arrayUsuarios = [];
                        for (var i = 0; i < Usuarios.length; i++) {
                            arrayUsuarios.push({
                                IdMasterUsers: Usuarios[i]
                            });
                        }
                        $.each($Ddl, function (i, data) {
                            ColumnsSQL.push(data.dataset.namefieldsudf);
                            ColumnsExcel.push(data.value);
                        });
                        var params = new Object();
                        params['Plantilla'] = {
                            IdTemplates: $('#DdlTemplate').val()
                        }
                        params['Algorithms'] = {
                            IdAlgorithmsAssignment: $('#DdlAlgoritmo').val()
                        }
                        params['Grupo'] = {
                            IdMasterGroups: $('#DdlGroups').val()
                        }
                        params['ListUsers'] = arrayUsuarios;
                        params['ColumnsSQL'] = ColumnsSQL;
                        params['ColumnsExcel'] = ColumnsExcel;
                        var Result = await $.ajax({
                            type: "post",
                            url: '/ImportarPQR/FinalizarImportacion',
                            data: params,
                        });
                        if (Result.returnError != null) {
                            $('#Importar-error').html(Result.returnError);
                        } else {
                            swal("Bien Hecho!", "La carga fue realizada correctamente, favor verificar.", "success");
                            setTimeout(function () {
                                window.location = "/Home";
                            }, 1000);
                        }
                    } catch (ex) {
                        swal("Error interno", "Comunicate con el area de IT para la verificacion de este error.", "error");
                    } finally {
                        FuncBtnLoader($BtnFinalizar, 0);
                    }
                });
            }
        } catch (ex) {
            swal("Error interno", "Comunicate con el area de IT para la verificacion de este error.", "error");
        }
    });
})();

async function changeSitios(value) {
    var $optionTempl = '<option value="">--Seleccionar--</option>';
    var $optionGroup = '<option value="">--Seleccionar--</option>';
    if (value != "") {
        let params = new Object();
        params['Sitio'] = { IdMasterSites: value };
        //templates
        let ListTemplates = await $.ajax({
            type: "post",
            url: '/ImportarPQR/GetListTemplates',
            data: params
        });
        $.each(ListTemplates, function (i, data) {
            $optionTempl += '<option value="' + data.IdTemplates + '">' + data.NameTemplate + '</option>';
        });
        //Categories
        let ListGroups = await $.ajax({
            type: "post",
            url: '/ImportarPQR/GetListGroups',
            data: params
        });
        $.each(ListGroups, function (i, data) {
            $optionGroup += '<option value="' + data.IdMasterGroups + '">' + data.NameGroup + '</option>';
        });
    }
    $('#DdlTemplate').html($optionTempl);
    $('#DdlGroups').html($optionGroup);
}

async function onChangeGroups_ListUsers(value) {
    var $option = "";
    if (value != "") {
        var params = new Object();
        params["IdGroups"] = value;
        var Result = await $.ajax({
            type: "post",
            url: '/ImportarPQR/ListUsersXGroup',
            data: params
        });
        if (Result.length == 0) {
            $('#DdlGroups').addClass('is-invalid');
            $('#Importar-error').html('El grupo seleccionado no cuenta con usuarios con permisos de asignarle tickets, verifique los permisos de estos usuarios.');
        } else {
            $('#DdlGroups').removeClass('is-invalid');
            $('#Importar-error').html('');
            $.each(Result, function (key, value) {
                $option += `<option value="${value.IdMasterUsers}">${value.Nombres.trim()} ${value.PrimerApellido.trim()} ${value.SegundoApellido.trim()}</option>`;
            });
        }
    }
    $('#DdlUsuarios').html($option).trigger("chosen:updated");
}

async function changeFile() {
    FuncBtnLoader($BtnSiguiente1, 1);
    var file = document.getElementById("fileID").files[0];
    var formData = new FormData();
    formData.append("fileID", file);
    var Result = await $.ajax({
        type: "post",
        url: '/ImportarPQR/AdjuntarExcel',
        processData: false,
        contentType: false,
        cache: false,
        dataType: 'json',
        data: formData
    });
    if (Result.returnError != null) {
        $('#fileID-error').html(Result.returnError);
    } else {
        $('#lbl-fileID').text(Result.NameData);
        $('#fileID-error').html('');
        var $option = '<option value="">--Seleccionar--</option>';
        var ListHojas = Result.NameHojas;
        $.each(ListHojas, function (i, data) {
            if (data.includes('_') == false) $option += `<option value="${data}">${data}</option>`;
        });
        $('#DdlHojaExcel').html($option);
        FuncBtnLoader($BtnSiguiente1, 0);
    }
}

async function OnChangeHojaExcel(value) {
    if (value != "") {
        try {
            await FuncBtnLoader($BtnSiguiente2, 1);
            var params = new Object();
            params["HojaExcel"] = value;
            var Result = await $.ajax({
                type: "post",
                url: '/ImportarPQR/ColumnasExcel',
                data: params
            });
            if (Result.returnError != null) {
                $('#DdlHojaExcel-error').html(Result.returnError);
                $('#DdlHojaExcel').removeClass('is-invalid').addClass('is-invalid');
            } else {
                $('#DdlHojaExcel-error').html('');
                var ListColumn = Result.Columnas;
                var $option = '<option value="">--Seleccionar--</option>';
                $.each(ListColumn, function (i, data) {
                    $option += `<option value="${data}">${data}</option>`;
                });
                $('.colummnExcel').html($option);
                $('#divNRegisTotal').html(Result.NumRecords);
                await autoLlenadoSelect();
            }
        } catch (ex) {
            swal("Error interno", "Comunicate con el area de IT.", "error");
        } finally {
            setTimeout(async function () {
                await FuncBtnLoader($BtnSiguiente2, 0);
            }, 500);
            swal("Importacion Exitosa!!", "Estados actualizados", "success");
        }
    } else {
        var $option = '<option value="">--Seleccionar--</option>';
        $('.colummnExcel').html($option);
        swal("Importacion Exitosa!!", "Estados actualizados", "success");
    }
    $('.chosen-select').trigger("chosen:updated");
}

async function autoLlenadoSelect() {
    var $labelColumns = $('.labelsColumns');
    $.each($labelColumns, async function (i, data) {
        var IdDdl = data.dataset.idfields;
        var nameColumn = data.innerHTML.trim();
        $(`#DdlFielsUDF_${IdDdl} option[value='${nameColumn}']`).attr("selected", true);
        var $Column = document.getElementById('DdlFielsUDF_' + IdDdl);
        await ValidarDataColumnExcel($Column);
    });
}

async function ValidarDataColumnExcel($Column) {
    $('#' + $Column.id + '-error').html('Validando por favor espere...');
    if ($Column.value != "") {
        var ColumnsSQL = [$Column.dataset.namefieldsudf];
        var ColumnsExcel = [$Column.value];
        var params = new Object();
        params['ColumnsSQL'] = ColumnsSQL;
        params['ColumnsExcel'] = ColumnsExcel;
        params['FieldUDF'] = {
            IdFieldsUDF: $Column.dataset.idfieldsudf
        };
        var Result = await $.ajax({
            type: "post",
            url: '/ImportarPQR/ValidarColumnExcel',
            data: params,
        });
        if (Result.returnError != null) {
            $('#' + $Column.id + '-error').html(Result.returnError);
        } else {
            $('#' + $Column.id + '-error').html('');
        }
    } else {
        $('#' + $Column.id + '-error').html('');
    }
}

async function onChangeValDataColumn($Column) {
    FuncBtnLoader($BtnSiguiente2, 1);
    await ValidarDataColumnExcel($Column);
    FuncBtnLoader($BtnSiguiente2, 0);
}