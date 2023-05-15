//PermisosActions('ListWorkOrders');
//$('.chosen-select').chosen();
//$('.super-chosen').chosen();
//$('.input-daterange').datepicker({
//    todayBtn: "linked",
//    calendarWeeks: true,
//    keyboardNavigation: false,
//    forceParse: false,
//    autoclose: true,
//    format: 'yyyy/mm/dd'
//});
$('#divLoaderMaster').hide();
async function FuncBtnLoader($BtnGuardar, Ocultar) {
    if (Ocultar == 1) {
        $BtnGuardar.style.display = 'none';
        $('.BtnLoading').show();
    } else {
        $BtnGuardar.style.display = '';
        $('.BtnLoading').hide();
    }
}

async function ViewCasosPrepago($Btn) {

    try {
        var ESTADOArray = "";
        //if ($('#txtIdSolutions').val() === '') {
        //    swal("Error", "Solutions es obligatoria", "error");
        //    return;
        //}
        //if ($('#txtCuscode').val() === '') {
        if ($('#field_834').val() === '') {
            swal("Error", "Cuscode es obligatoria", "error");
            return;
        }

        if (new Date($('#txtFechaInicio').val()) > $('#txtFechaFinal').val()) {
            swal("Error", "La fecha de inicio no puede ser mayor a la fecha final", "error");
            return;
        }
        FuncBtnLoader($Btn, 1)
        //$('#divLoaderMaster').show();
        let params = new Object();
        params["Idsolutions"] = $('#txtIdSolutions').val();
        params["Cuscode"] = $('#field_834').val();
        params["FechaInicio"] = $('#txtFechaInicio').val();
        params["FechaFinal"] = $('#txtFechaFinal').val();

        let Result = await $.ajax({
            type: "post",
            url: '/CasosPqrPlntMovilEscrita/ListCasePrepago',
            data: params,
        });
        $("#divTableprepago").html(Result);
        ToolsDataTables();
        FuncBtnLoader($Btn, 0)
    } catch (ex) {
        swal("Error interno", "Comunicate con el area de IT para la verificacion de este error..", "error");
    }
}

async function ViewCasosPrepago13($Btn) {
    debugger
    try {
        var ESTADOArray = "";
        //if ($('#txtIdSolutions').val() === '') {
        //    swal("Error", "Solutions es obligatoria", "error");
        //    return;
        //}
        if ($('#field_911').val() === '') {
            swal("Error", "Cuscode es obligatoria", "error");
            return;
        }

        if (new Date($('#txtFechaInicio').val()) > $('#txtFechaFinal').val()) {
            swal("Error", "La fecha de inicio no puede ser mayor a la fecha final", "error");
            return;
        }
        FuncBtnLoader($Btn, 1)
        //$('#divLoaderMaster').show();
        let params = new Object();
        params["Idsolutions"] = $('#txtIdSolutions').val();
        params["Cuscode"] = $('#field_911').val();
        params["FechaInicio"] = $('#txtFechaInicio').val();
        params["FechaFinal"] = $('#txtFechaFinal').val();

        let Result = await $.ajax({
            type: "post",
            url: '/CasosPqrPlntMovilEscrita/ListCasePrepago13',
            data: params,
        });
        $("#divTableprepago13").html(Result);
        ToolsDataTables();
        FuncBtnLoader($Btn, 0)
    } catch (ex) {
        swal("Error interno", "Comunicate con el area de IT para la verificacion de este error..", "error");
    }
}


async function ViewCasosPospago($Btn) {

    try {
        var ESTADOArray = "";
        if ($('#txtFechaInicio').val() === '') {
            swal("Error", "Fecha inicio es obligatoria", "error");
            return;
        }

        if ($('#txtFechaFinal').val() === '') {
            swal("Error", "Fecha final es obligatoria", "error");
            return;
        }
        //if ($('#txtIdSolutions').val() === '') {
        //    swal("Error", "Solutions es obligatoria", "error");
        //    return;
        //}
        if ($('#field_769').val() === '') {
            swal("Error", "Cuscode es obligatoria", "error");
            return;
        }

        if (new Date($('#txtFechaInicio').val()) > $('#txtFechaFinal').val()) {
            swal("Error", "La fecha de inicio no puede ser mayor a la fecha final", "error");
            return;
        }
        FuncBtnLoader($Btn, 1)
        //$('#divLoaderMaster').show();
        let params = new Object();
        params["Idsolutions"] = $('#txtIdSolutions').val();
        params["Cuscode"] = $('#field_769').val();
        params["FechaInicio"] = $('#txtFechaInicio').val();
        params["FechaFinal"] = $('#txtFechaFinal').val();

        let Result = await $.ajax({
            type: "post",
            url: '/CasosPqrPlntMovilEscrita/ListCasePospago',
            data: params,
        });
        $("#divTablepospago").html(Result);
        ToolsDataTables();
        FuncBtnLoader($Btn, 0)
    } catch (ex) {
        swal("Error interno", "Comunicate con el area de IT para la verificacion de este error..", "error");
    }
}

async function ViewCasosPospago13($Btn) {

    try {
        var ESTADOArray = "";
        if ($('#txtFechaInicio').val() === '') {
            swal("Error", "Fecha inicio es obligatoria", "error");
            return;
        }

        if ($('#txtFechaFinal').val() === '') {
            swal("Error", "Fecha final es obligatoria", "error");
            return;
        }
        //if ($('#txtIdSolutions').val() === '') {
        //    swal("Error", "Solutions es obligatoria", "error");
        //    return;
        //}
        if ($('#field_876').val() === '') {
            swal("Error", "Cuscode es obligatoria", "error");
            return;
        }

        if (new Date($('#txtFechaInicio').val()) > $('#txtFechaFinal').val()) {
            swal("Error", "La fecha de inicio no puede ser mayor a la fecha final", "error");
            return;
        }
        FuncBtnLoader($Btn, 1)
        //$('#divLoaderMaster').show();
        let params = new Object();
        params["Idsolutions"] = $('#txtIdSolutions').val();
        params["Cuscode"] = $('#field_876').val();
        params["FechaInicio"] = $('#txtFechaInicio').val();
        params["FechaFinal"] = $('#txtFechaFinal').val();

        let Result = await $.ajax({
            type: "post",
            url: '/CasosPqrPlntMovilEscrita/ListCasePospago13',
            data: params,
        });
        $("#divTablepospago13").html(Result);
        ToolsDataTables();
        FuncBtnLoader($Btn, 0)
    } catch (ex) {
        swal("Error interno", "Comunicate con el area de IT para la verificacion de este error..", "error");
    }
}

async function ViewCasosAscard($Btn) {

    try {
        var ESTADOArray = "";
        if ($('#txtFechaInicio').val() === '') {
            swal("Error", "Fecha inicio es obligatoria", "error");
            return;
        }

        if ($('#txtFechaFinal').val() === '') {
            swal("Error", "Fecha final es obligatoria", "error");
            return;
        }
        //if ($('#txtIdSolutions').val() === '') {
        //    swal("Error", "Solutions es obligatoria", "error");
        //    return;
        //}
        if ($('#field_739').val() === '') {
            swal("Error", "Cuscode es obligatoria", "error");
            return;
        }

        if (new Date($('#txtFechaInicio').val()) > $('#txtFechaFinal').val()) {
            swal("Error", "La fecha de inicio no puede ser mayor a la fecha final", "error");
            return;
        }
        FuncBtnLoader($Btn, 1)
        //$('#divLoaderMaster').show();
        let params = new Object();
        params["Idsolutions"] = $('#txtIdSolutions').val();
        params["Cuscode"] = $('#field_739').val();
        params["FechaInicio"] = $('#txtFechaInicio').val();
        params["FechaFinal"] = $('#txtFechaFinal').val();

        let Result = await $.ajax({
            type: "post",
            url: '/CasosPqrPlntMovilEscrita/ListCaseAscard',
            data: params,
        });
        $("#divTableascard1").html(Result);
        ToolsDataTables();
        FuncBtnLoader($Btn, 0)
    } catch (ex) {
        swal("Error interno", "Comunicate con el area de IT para la verificacion de este error..", "error");
    }
}

async function ViewCasosAscard13($Btn) {

    try {
        var ESTADOArray = "";
        if ($('#txtFechaInicio').val() === '') {
            swal("Error", "Fecha inicio es obligatoria", "error");
            return;
        }

        if ($('#txtFechaFinal').val() === '') {
            swal("Error", "Fecha final es obligatoria", "error");
            return;
        }
        //if ($('#txtIdSolutions').val() === '') {
        //    swal("Error", "Solutions es obligatoria", "error");
        //    return;
        //}
        if ($('#field_890').val() === '') {
            swal("Error", "Cuscode es obligatoria", "error");
            return;
        }

        if (new Date($('#txtFechaInicio').val()) > $('#txtFechaFinal').val()) {
            swal("Error", "La fecha de inicio no puede ser mayor a la fecha final", "error");
            return;
        }
        FuncBtnLoader($Btn, 1)
        //$('#divLoaderMaster').show();
        let params = new Object();
        params["Idsolutions"] = $('#txtIdSolutions').val();
        params["Cuscode"] = $('#field_890').val();
        params["FechaInicio"] = $('#txtFechaInicio').val();
        params["FechaFinal"] = $('#txtFechaFinal').val();

        let Result = await $.ajax({
            type: "post",
            url: '/CasosPqrPlntMovilEscrita/ListCaseAscard13',
            data: params,
        });
        $("#divTableascard13").html(Result);
        ToolsDataTables();
        FuncBtnLoader($Btn, 0)
    } catch (ex) {
        swal("Error interno", "Comunicate con el area de IT para la verificacion de este error..", "error");
    }
}

async function ViewCasosCuotasAscard($Btn) {

    try {
        var ESTADOArray = "";
        if ($('#txtFechaInicio').val() === '') {
            swal("Error", "Fecha inicio es obligatoria", "error");
            return;
        }

        if ($('#txtFechaFinal').val() === '') {
            swal("Error", "Fecha final es obligatoria", "error");
            return;
        }
        if ($('#field_740').val() === '') {
            swal("Error", "Numero de credito es obligatorio", "error");
            return;
        }
        //if ($('#txtCuscode').val() === '') {
        //    swal("Error", "Cuscode es obligatoria", "error");
        //    return;
        //}

        if (new Date($('#txtFechaInicio').val()) > $('#txtFechaFinal').val()) {
            swal("Error", "La fecha de inicio no puede ser mayor a la fecha final", "error");
            return;
        }
        FuncBtnLoader($Btn, 1)
        //$('#divLoaderMaster').show();
        let params = new Object();
        params["Idsolutions"] = $('#txtIdSolutions').val();
        params["Cuscode"] = $('#field_740').val();
        params["FechaInicio"] = $('#txtFechaInicio').val();
        params["FechaFinal"] = $('#txtFechaFinal').val();

        let Result = await $.ajax({
            type: "post",
            url: '/CasosPqrPlntMovilEscrita/ListCaseCuotasAscard',
            data: params,
        });
        $("#divTableascard2").html(Result);
        ToolsDataTables();
        FuncBtnLoader($Btn, 0)
    } catch (ex) {
        swal("Error interno", "Comunicate con el area de IT para la verificacion de este error..", "error");
    }
}

async function ViewCasosCuotasAscard13($Btn) {

    try {
        var ESTADOArray = "";
        if ($('#txtFechaInicio').val() === '') {
            swal("Error", "Fecha inicio es obligatoria", "error");
            return;
        }

        if ($('#txtFechaFinal').val() === '') {
            swal("Error", "Fecha final es obligatoria", "error");
            return;
        }
        //if ($('#txtIdSolutions').val() === '') {
        //    swal("Error", "Solutions es obligatoria", "error");
        //    return;
        //}
        if ($('#field_898').val() === '') {
            swal("Error", "Cuscode es obligatoria", "error");
            return;
        }

        if (new Date($('#txtFechaInicio').val()) > $('#txtFechaFinal').val()) {
            swal("Error", "La fecha de inicio no puede ser mayor a la fecha final", "error");
            return;
        }
        FuncBtnLoader($Btn, 1)
        //$('#divLoaderMaster').show();
        let params = new Object();
        params["Idsolutions"] = $('#txtIdSolutions').val();
        params["Cuscode"] = $('#field_898').val();
        params["FechaInicio"] = $('#txtFechaInicio').val();
        params["FechaFinal"] = $('#txtFechaFinal').val();

        let Result = await $.ajax({
            type: "post",
            url: '/CasosPqrPlntMovilEscrita/ListCaseCuotasAscard13',
            data: params,
        });
        $("#divTableascard213").html(Result);
        ToolsDataTables();
        FuncBtnLoader($Btn, 0)
    } catch (ex) {
        swal("Error interno", "Comunicate con el area de IT para la verificacion de este error..", "error");
    }
}

async function ViewCasosEliminacionCentrales($Btn) {

    try {
        var ESTADOArray = "";
        if ($('#txtFechaInicio').val() === '') {
            swal("Error", "Fecha inicio es obligatoria", "error");
            return;
        }

        if ($('#txtFechaFinal').val() === '') {
            swal("Error", "Fecha final es obligatoria", "error");
            return;
        }
        //if ($('#txtIdSolutions').val() === '') {
        //    swal("Error", "Solutions es obligatoria", "error");
        //    return;
        //}
        if ($('#field_848').val() === '') {
            swal("Error", "Cuscode es obligatoria", "error");
            return;
        }

        if (new Date($('#txtFechaInicio').val()) > $('#txtFechaFinal').val()) {
            swal("Error", "La fecha de inicio no puede ser mayor a la fecha final", "error");
            return;
        }
        FuncBtnLoader($Btn, 1)
        //$('#divLoaderMaster').show();
        let params = new Object();
        params["Idsolutions"] = $('#txtIdSolutions').val();
        params["Cuscode"] = $('#field_848').val();
        params["FechaInicio"] = $('#txtFechaInicio').val();
        params["FechaFinal"] = $('#txtFechaFinal').val();

        let Result = await $.ajax({
            type: "post",
            url: '/CasosPqrPlntMovilEscrita/ListCaseEliminacionCentrales',
            data: params,
        });
        $("#divTableeliminar").html(Result);
        ToolsDataTables();
        FuncBtnLoader($Btn, 0)
    } catch (ex) {
        swal("Error interno", "Comunicate con el area de IT para la verificacion de este error..", "error");
    }
}

async function ViewCasosEliminacionCentrales13($Btn) {

    try {
        var ESTADOArray = "";
        if ($('#txtFechaInicio').val() === '') {
            swal("Error", "Fecha inicio es obligatoria", "error");
            return;
        }

        if ($('#txtFechaFinal').val() === '') {
            swal("Error", "Fecha final es obligatoria", "error");
            return;
        }
        //if ($('#txtIdSolutions').val() === '') {
        //    swal("Error", "Solutions es obligatoria", "error");
        //    return;
        //}
        if ($('#field_925').val() === '') {
            swal("Error", "Cuscode es obligatoria", "error");
            return;
        }

        if (new Date($('#txtFechaInicio').val()) > $('#txtFechaFinal').val()) {
            swal("Error", "La fecha de inicio no puede ser mayor a la fecha final", "error");
            return;
        }
        FuncBtnLoader($Btn, 1)
        //$('#divLoaderMaster').show();
        let params = new Object();
        params["Idsolutions"] = $('#txtIdSolutions').val();
        params["Cuscode"] = $('#field_925').val();
        params["FechaInicio"] = $('#txtFechaInicio').val();
        params["FechaFinal"] = $('#txtFechaFinal').val();

        let Result = await $.ajax({
            type: "post",
            url: '/CasosPqrPlntMovilEscrita/ListCaseEliminacionCentrales13',
            data: params,
        });
        $("#divTableeliminar13").html(Result);
        ToolsDataTables();
        FuncBtnLoader($Btn, 0)
    } catch (ex) {
        swal("Error interno", "Comunicate con el area de IT para la verificacion de este error..", "error");
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
                    url: '/ImportWorkOrder/TypesFields',
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
            if (Valida == false) {

            } else if (Valida == "is-invalid") {
                event.preventDefault();
            } else {
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

            } else {
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
                    text: " ¿Deseas cargar y asignar estos registros " + msj + " con el algoritmo " + Algoritmo + "?",
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

