$('.datepicker').datepicker({
    todayBtn: "linked",
    calendarWeeks: true,
    format: 'yyyy/mm/dd'
})
var control = 0;
async function inputdate() {
    $('input:text[id=field_729]').val();
    $('input:text[id=field_730]').val();
    $('input:text[id=field_750]').val();
    $('input:text[id=field_751]').val();

    if (document.getElementById("field_729").value != "" && document.getElementById("field_730").value != "") {
        if (document.getElementById("field_729").value >= document.getElementById("field_730").value) {

            console.log(field_729.value);
            console.log("control:", control);
            control = control + 1;
            swal({
                title: 'Alerta automatica',
                text: 'Rango de fecha incorrecto',
            })
        }
        else if (document.getElementById("field_750").value != "" && document.getElementById("field_751").value != "") {
            if (document.getElementById("field_750").value >= document.getElementById("field_751").value) {

                console.log(field_729.value);
                console.log("control:", control);
                control = control + 1;
                swal({
                    title: 'Alerta automatica InPQR',
                    text: 'Rango de fecha incorrecto',
                })
            }
        }
    }
    else if (document.getElementById("field_750").value != "" && document.getElementById("field_751").value != "") {
        if (document.getElementById("field_750").value >= document.getElementById("field_751").value) {

            console.log(field_729.value);
            console.log("control:", control);
            control = control + 1;
            swal({
                title: 'Alerta automatica InPQR',
                text: 'Rango de fecha incorrecto',
            })
        }
    }
}
$('.chosen-select').chosen()
$('#TxtDescriptionClient').summernote("code", $('#TxtDescriptionClient').text())
$('.summernote-disabled').summernote('disable');
$('#TxtResolution').summernote("code", $('#TxtResolution').text());
//$('#DdlStatus').change();
$('#divLoaderMaster').hide();

if ($('field_10992').val != null) {
    $('#combosubestado').hide();
} else {
    $('#combosubestado').show();
}

if ($('#cmbsubstate').val() == 28 || $('#cmbsubstate').val() == 11 || $('#cmbsubstate').val() == 29 || $('#cmbsubstate').val() == 35 || $('#cmbsubstate').val() == 55 || $('#cmbsubstate').val() == 56) {
    $('#combosubestado').show();
}

const $BtnGuardar = document.getElementById('BtnGuardar_S');
function FuncBtnLoader($BtnGuardar, Ocultar) {
    if (Ocultar == 1) {
        $BtnGuardar.style.display = 'none';
        $('.BtnLoading').show();
    } else {
        $BtnGuardar.style.display = '';
        $('.BtnLoading').hide();
    }
}
(async function load() {
    $("#div_235").hide(); // oculta el select ajuste
    $("#div_997").hide(); // oculta PQR
    $("#div_998").hide(); // oculta PQR AJU AIB
    $("#div_999").hide(); // oculta observacion

    async function obtenerValueFields($ObjFields) {
        var Retorno = [];
        var valor = "";
        if ($ObjFields.dataset.idfieldstypes == 5) {
            valor = $('#' + $ObjFields.id).is(':checked').toString();
        } else if ($ObjFields.dataset.idfieldstypes == 6) {
            valor = $('#' + $ObjFields.id + ' option:selected').text();
            if (valor.includes("Seleccionar")) {
                valor = "";
            }
        } else {
            valor = $ObjFields.value;
        }
        Retorno.push({
            NameFieldsUDF: $ObjFields.dataset.namefieldsudf,
            Value: valor
        });
        return Retorno[0];
    }
    if ($BtnGuardar != undefined) {
        $BtnGuardar.addEventListener('click', async (event) => {
            $('.content-wrapper .form-control').removeAttr('required');

            try {
                FuncBtnLoader($BtnGuardar, 1);
                var $elementRequired = $('#formCreate .required');
                var $elementRequired2 = $('#formCreate .requiredForever');
                var ValidaOthers = true;
                var Valida = true;
                var status = ["1", "593", "594", "595"]; //ESTADOS NO OBLIGATORIOS PARA AIRE
                var idStatus = $('#DdlStatus').val();
                var index = $.inArray(idStatus, status)
                if (index == -1) {
                    Valida = await ValidarVacios($elementRequired);
                    ValidaOthers = await ValidarVacios($elementRequired2);
                }
                var idAjusteMovil = $("#IdTemplate_SuicheMovil").val();
                if (idAjusteMovil === '28') {
                    var Custcode = $('#field_781').val() ? $('#field_781').val().toUpperCase() : "";
                    //console.log(mdm_pqr.length);
                    if (Custcode == '') {


                    } else if (Custcode.length < 10) {
                        swal({
                            title: '¡Alerta automatica!',
                            text: 'Campo Custcode debe tener minimo 10 caracteres para poder guardar',
                        })

                        event.preventDefault();
                        return false;
                    }

                    var Min = $('#field_786').val() ? $('#field_786').val().toUpperCase() : "";
                    //console.log(mdm_pqr.length);
                    if (Min == '') {


                    } else if (Min.length < 10) {
                        swal({
                            title: '¡Alerta automatica!',
                            text: 'Campo Min debe tener minimo 10 caracteres para poder guardar',
                        })

                        event.preventDefault();
                        return false;
                    }
                }

                if (ValidaOthers == false) {

                    swal({
                        title: '¡Alerta automatica!',
                        text: 'Todos los campos son obligatorios',
                    })

                    event.preventDefault();
                    return false;
                } else if (Valida == false) {
                    swal({
                        title: '¡Alerta automatica!',
                        text: 'Todos los campos son obligatorios',
                    })
                    event.preventDefault();
                    return false;

                }
                else if ($('#field_150').val() === "212" && control == 0) {

                    swal({ title: 'Selecciono ajuste sin gestionarlo!', type: 'error', timer: 5000 })
                    event.preventDefault();
                    return false;
                }
                else {


                    event.preventDefault();
                    swal
                        (
                            {
                                title: "¿Estas seguro?",
                                text: "¿Deseas guardar los cambios?",
                                type: "warning",
                                showCancelButton: true,
                                confirmButtonText: "Si, continuar!",
                                closeOnConfirm: true,
                                showLoaderOnConfirm: true
                            }, async function () {
                            try {
                                var $Fields = $('.fieldSave');
                                var objFields = [];
                                for (i = 0; i < $Fields.length; i++) {
                                    var Array = await obtenerValueFields($Fields[i]);
                                    objFields.push(Array);
                                }
                                var params = new Object();
                                debugger
                                params["IdWorkOrder"] = $('#formCreate')[0].dataset.idworkorder;
                                params["Status"] = {
                                    IdStatusDefinition: $('#DdlStatus').val()
                                }
                                params["SubStatus"] = {
                                    IdStatusDefinition: $('#DdlSubStatus').val()
                                }
                                params["Resolutions"] = $("#TxtResolution").summernote("code");
                                params["ListFielsUDFSolution"] = objFields;
                                params["WorkOrderEscalations"] = {
                                    GroupsScaled: {
                                        IdMasterGroups: $('#DdlGroupScale').val()
                                    },
                                    TypeScaling: $('#DdlTypeScaled').val(),
                                    Comments: $('#TxtCommentsScaled').val()
                                }

                                if ($('#field_725').val() === "4192") {
                                    params["BPBServicio"] = {
                                        Nombre: $("#DdlServicio").val()
                                    }
                                    params["CtaContable"] = {
                                        Nombre: $("#DdlCTA_Contable").val()
                                    }

                                }

                                debugger
                                var Result = await $.ajax({
                                    type: "post",
                                    url: '/WorkOrderSolutions/SaveWorkOrderSolutions',
                                    contentType: "application/json; charset=utf-8",
                                    data: JSON.stringify(params)
                                });
                                if ($('#field_150').val() === "212") {

                                    event.preventDefault();
                                    swal
                                        (
                                            {
                                                title: "Gestion guardada exitosamente",
                                                text: "¿Termino la Gestion de la PQR?",
                                                type: "success",
                                                showCancelButton: "No",
                                                cancelButtonText: "No",
                                                confirmButtonText: "Si",
                                                closeOnConfirm: true,
                                                showLoaderOnConfirm: true
                                            },
                                            async function () {
                                                try {
                                                    swal("Bien Hecho!", "Acción correcta, Continue con la siguiente PQR.", "success");
                                                    setTimeout(function () {
                                                        window.location = "/ListWorkOrders";
                                                    }, 1000);

                                                }
                                                catch (ex) {
                                                    swal("Error save1 interno", "Comunicate con el area de IT para la verificacion de este error..", "error");
                                                }


                                            }



                                        );

                                    //if ($('#field_150').val() === "212" && control == 1) {
                                    //swal("Bien Hecho!", "Acción correcta, Continue con el siguiente Ajuste", "success");
                                    //setTimeout(function () {
                                    //    location.reload();
                                    //}, 1000);

                                    //}

                                    //else if ($('#field_150').val() === "213" && control == 0) {
                                    //    swal("Bien Hecho!", "Acción correcta, Continue con la siguiente PQR.", "success");
                                    //    setTimeout(function () {
                                    //        window.location = "/ListWorkOrders";
                                    //    }, 1000);
                                    //}
                                }
                                else {
                                    swal("Bien Hecho!", "Acción realizado correctamente.", "success");
                                    setTimeout(function () {
                                        window.location = "/ListWorkOrders";
                                    }, 1000);
                                }

                            }
                            catch (ex) {
                                swal("Error save1 interno", "Comunicate con el area de IT para la verificacion de este error..", "error");
                            }
                        }
                        );

                }
            } catch (ex) {

                swal("Error save2 interno", "Comunicate con el area de IT para la verificacion de este error..", "error");
            } finally {
                FuncBtnLoader($BtnGuardar, 0);
            }
        });
    }
    OcultarcamposplanillaMovil();
    var idAjusteMovil = $("#IdTemplate_SuicheMovil").val();
    if (idAjusteMovil === '28') {
        $("#PlanillAjusteMovil").show();
    } else {
        $("#PlanillAjusteMovil").hide();
    }
})();
async function onChangeList($select) {
    var $ListDepend = $('.divFieldsDepend' + $select.dataset.idfieldsudf);
    if ($ListDepend.length != 0) {
        var $option = '<option value="">--Seleccionar--</option>';
        if ($select.value != "") {
            var params = new Object();
            params['ParentIdDisposi'] = $select.value;
            var Result = await $.ajax({
                type: "post",
                url: '/CreateWorkOrder/GetListDispositions',
                data: params
            });
            $.each(Result, function (i, data) {
                $option += '<option value="' + data.IdFieldsDispositions + '">' + data.Dispositions + '</option>';
            });
        }
        for (i = 0; i < $ListDepend.length; i++) {
            $('#' + $ListDepend[i].id).html($option);
        }
    }

    var cmb_favorabilidad = $("#field_146").val();
    var cmb_ajuste = $("#field_235").val();
    if (cmb_favorabilidad != '203' && cmb_favorabilidad != '205') {
        $("#div_235").hide();
        $("#field_235").removeClass('required Req-3 fieldSave requiredForever');
        $("#div_997").hide();
        $("#div_998").hide();
        $("#div_999").hide();
    } else {
        $("#div_235").show();
        $("#field_235").addClass('required Req-3 fieldSave requiredForever');
        if (cmb_ajuste == '4885' || cmb_ajuste == '4886') {
            $("#div_997").show();
            $("#div_998").show();
            $("#div_999").show();
            $("#field_997").addClass('form-control required Req-2 fieldSave requiredForever');
            $("#field_998").addClass('form-control required Req-2 fieldSave requiredForever');
            $("#field_999").addClass('form-control required Req-2 fieldSave requiredForever');

        } else {
            $("#div_997").hide();
            $("#div_998").hide();
            $("#div_999").hide();
            $("#field_997").removeClass('form-control required Req-2 fieldSave requiredForever');
            $("#field_998").removeClass('form-control required Req-2 fieldSave requiredForever');
            $("#field_999").removeClass('form-control required Req-2 fieldSave requiredForever');
        }

    }



}

async function OcultarcamposplanillaMovil($select) {
    $("#lbl_780").hide();
    $("#field_780").hide();
    $("#field_780").removeClass('form-control Req-2 fieldSave requiredForever');
    $("#lbl_781").hide();
    $("#field_781").hide();
    $("#field_781").removeClass('form-control Req-2 fieldSave requiredForever');
    $("#lbl_782").hide();
    $("#field_782").hide();
    $("#field_782").removeClass('form-control Req-2 fieldSave requiredForever');
    $("#lbl_783").hide();
    $("#field_783").hide();
    $("#field_783").removeClass('form-control Req-2 fieldSave requiredForever');
    $("#lbl_784").hide();
    $("#field_784").hide();
    $("#field_784").removeClass('form-control Req-2 fieldSave requiredForever');
    $("#lbl_785").hide();
    $("#field_785").hide();
    $("#field_785").removeClass('form-control Req-2 fieldSave requiredForever');
    $("#lbl_786").hide();
    $("#field_786").hide();
    $("#field_786").removeClass('form-control Req-2 fieldSave requiredForever');
    $("#lbl_787").hide();
    $("#field_787").hide();
    $("#field_787").removeClass('form-control Req-2 fieldSave requiredForever');
}
async function changeEstados(value) {
    var $option = '<option value="">--Seleccionar--</option>';
    if (value != "") {
        var params = new Object();
        params["IdStatus"] = value;
        var result = await $.ajax({
            type: "post",
            url: '/WorkOrderSolutions/GetSubStatus',
            data: params
        });
        if (result.TypeAction.IdTypeActions == 3)/*ESCALAMIENTOS*/ {
            $('.divScaled').show();
            $('.divScaled .form-control').prop('disabled', false);
        } else {
            $('.divScaled').hide();
            $('.divScaled .form-control').prop('disabled', true).val('');
            $('.divScaled .form-control').change();
            if (result.TypeAction.IdTypeActions == 5) /*Con respuesta*/ {
                $('#divCommentsScaled').show();
                $('#TxtCommentsScaled').prop('disabled', false);
            }
        }
        $('.lblrequi-').text('');
        $('.required').removeClass('required');
        $('.lblReq-' + result.TypeAction.IdTypeRequired).text('*');
        $('.Req-' + result.TypeAction.IdTypeRequired).removeClass('required').addClass('required');
        var SubStatus = result.SubStatus;
        $.each(SubStatus, function (i, data) {
            $option += '<option value="' + data.IdStatusDefinition + '">' + data.NameStatus + '</option>';
        });
    }
    $('#DdlSubStatus').html($option);
}
async function changeTypesScaled(value) {
    var $option = '<option value="">--Seleccionar--</option>';
    var mensaje = "";
    var btnDisabled = false;
    if (value != "") {
        var params = new Object();
        params['IdGroups'] = value;
        var Result = await $.ajax({
            type: "post",
            url: '/WorkOrderSolutions/GetTypesScaled',
            data: params
        });
        var NumUsers = Result.UsersXGroups.length;
        if (NumUsers == 0) {
            mensaje = "Este grupo no cuenta con usuarios disponibles para asignar.";
            btnDisabled = true;
        }
        $.each(Result.TypesScaled, function (i, data) {
            $option += '<option value="' + data + '">' + data + '</option>';;
        });
    }
    $('#BtnGuardar').prop('disabled', btnDisabled);
    $('#DdlGroupScale-error').text(mensaje);
    $('#DdlTypeScaled').html($option);
}
async function getListPlataformas($select) {
    let $option = '<option value="">--Seleccionar--</option>'
    if ($select.value != "") {
        let params = new Object()
        params['IdLocation'] = $select.value
        let Result = await $.ajax({
            type: "post",
            url: '/CreateWorkOrder/ListPlatform',
            data: params
        })
        for (const data of Result) {
            $option += `<option value="${data.IdPlatform}">${data.Description}</option>`
        }
    }
    $('#DdlPlataforma-' + $select.dataset.idfieldsudf).html($option)
    await getListPuestos($('#DdlPlataforma-' + $select.dataset.idfieldsudf)[0])
}
async function getListPuestos($select) {
    let $option = '<option value="">Buscar por Numero,IP,Extension o Nombre</option>'
    if ($select.value != "") {
        let params = new Object()
        params['IdPlatform'] = $select.value
        let Result = await $.ajax({
            type: "post",
            url: '/CreateWorkOrder/ListBooth',
            data: params
        })
        for (const data of Result) {
            $option += `<option value="${data.IdBooth}">${data.BoothNumber} - ${data.IP} - ${data.BoothName} - ${data.Extension}</option>`
        }
    }
    $('#field_' + $select.dataset.idfieldsudf).html($option).trigger('chosen:updated')
}
async function changeAgregarAdjunto() {
    if ($BtnGuardar != undefined) {
        try {
            FuncBtnLoader($BtnGuardar, 1);
            var file = document.getElementById("FileAddAttachments").files[0];
            var formData = new FormData();
            formData.append("Adjunto", file);
            var ResultAdjunt = await $.ajax({
                type: "post",
                url: '/WorkOrderSolutions/AddAttachments',
                processData: false,
                contentType: false,
                cache: false,
                dataType: 'json',
                data: formData
            });
            if (ResultAdjunt.msjError != null) {
                toastr.error(ResultAdjunt.msjError, 'Error');
            } else {
                var params = new Object();
                ResultAdjunt.IdWorkOrder = $('#formCreate')[0].dataset.idworkorder;
                params['InforAdjunto'] = ResultAdjunt;
                var ReturnView = await $.ajax({
                    type: "post",
                    url: '/WorkOrderSolutions/DivAttachment',
                    data: params
                });
                $('#FileAddAttachments').val('');
                $('#divAdjuntos').append(ReturnView);
                toastr.info('Adjunto agregado correctamente.', 'Adjunto agregado');
            }
        } catch (ex) {
            swal("Error interno", "Comunicate con el area de IT para la verificacion de este error..", "error");
        } finally {
            FuncBtnLoader($BtnGuardar, 0);
        }
    }
}

async function deleteAdjunto(NameEncrypted) {
    try {
        var params = new Object();
        params["IdWorkOrder"] = $('#formCreate')[0].dataset.idworkorder;
        params['NameEncrypted'] = NameEncrypted;
        var Result = await $.ajax({
            type: "post",
            url: '/WorkOrderSolutions/DeleteAttachment',
            data: params
        });
        $('#' + NameEncrypted).remove();
        toastr.success('Adjunto eliminado correctamente.', 'Adjunto eliminado');
    } catch (ex) {
        swal("Error interno", "Comunicate con el area de IT para la verificacion de este error..", "error");
    }
}


// Negacion de linea 
// 08/2021

async function SaveNegacion($Btn) {
    try {
        ToolsFuncBtnLoader($Btn, 1);

        /*
        let $elementRequired = $('.required');
        let ValidaInput = ValidarVacios($elementRequired);
        if (ValidaInput == false) {
            toastr.error('Los campos no pueden estar vacios o con errores.', 'Campos obligatorios');
            return
        }
        */
        let params = new Object();


        if ($('#IdWorkOrderh2').val() === '') {
            swal("Error", "Ingrese el campo id del caso", "error");
            return;
        }

        if ($('#txtBase').val() === '') {
            swal("Error", "Ingrese el campo Base", "error");
            return;
        }

        if ($('#txtImagen').val() === '') {
            swal("Error", "Ingrese el campo Imagen", "error");
            return;
        }

        if ($('#txtMin').val() === '') {
            swal("Error", "Ingrese el campo MIN", "error");
            return;
        }


        if ($('#txtFechaActivacion').val() === '') {
            swal("Error", "Ingrese el campo   FechaActivacion", "error");
            return;
        }

        if ($('#txtCurcode').val() === '') {
            swal("Error", "Ingrese el campo  Curcode", "error");
            return;
        }

        if ($('#txtNombre').val() === '') {
            swal("Error", "Ingrese el campo Nombre", "error");
            return;
        }

        if ($('#txtApellido').val() === '') {
            swal("Error", "Ingrese el campo  Apellido", "error");
            return;
        }

        if ($('#txtCanal').val() === '') {
            swal("Error", "Ingrese el campo  Canal", "error");
            return;
        }

        if ($('#txtAscard').val() === '') {
            swal("Error", "Ingrese el campo Ascard", "error");
            return;
        }

        if ($('#TxtFechaReposicion').val() === '') {
            swal("Error", "Ingrese el campo  Fecha Reposicion", "error");
            return;
        }

        if ($('#txtContrato').val() === '') {
            swal("Error", "Ingrese el campo Contrato", "error");
            return;
        }

        if ($('#txtGrabacion').val() === '') {
            swal("Error", "Ingrese el campo  Grabacion", "error");
            return;
        }

        if ($('#TxtReasignacion').val() === '') {
            swal("Error", "Ingrese el campo  Reasignacion", "error");
            return;
        }

        if ($('#txtEstado').val() === '') {
            swal("Error", "Ingrese el campo  Estado", "error");
            return;
        }


        if ($('#txtLocalizado').val() === '') {
            swal("Error", "Ingrese el campo   Localizado", "error");
            return;
        }
        if ($('#txtObservaciones').val() === '') {
            swal("Error", "Ingrese el campo  Observaciones", "error");
            return;
        }

        if ($('#txtRangoProbable').val() === '') {
            swal("Error", "Ingrese el campo   RangoProbable", "error");
            return;
        }

        if ($('#txtDireccion').val() === '') {
            swal("Error", "Ingrese el campo Direccion", "error");
            return;
        }

        if ($('#txtCustomerId').val() === '') {
            swal("Error", "Ingrese el campo  CustomerId", "error");
            return;
        }

        if ($('#txtCiudad').val() === '') {
            swal("Error", "Ingrese el campo Ciudad", "error");
            return;
        }

        if ($('#txtDepartamento').val() === '') {
            swal("Error", "Ingrese el campo Departamento", "error");
            return;
        }

        if ($('#txtFechaRadicacion').val() === '') {
            swal("Error", "Ingrese el campo FechaRadicacion", "error");
            return;
        }

        if ($('#txtNotificacion').val() === '') {
            swal("Error", "Ingrese el campo Notificacion", "error");
            return;
        }

        if ($('#txtFechaDesactivacion').val() === '') {
            swal("Error", "Ingrese el campo FechaDesactivacion", "error");
            return;
        }

        if ($('#txtPqr').val() === '') {
            swal("Error", "Ingrese el campo Pqr", "error");
            return;
        }

        if ($('#txtCedula').val() === '') {
            swal("Error", "Ingrese el campo Cedula", "error");
            return;
        }

        if ($('#txtAreaRadicacion').val() === '') {
            swal("Error", "Ingrese el campo AreaRadicacion", "error");
            return;
        }

        if ($('#txtTipoReclamo').val() === '') {
            swal("Error", "Ingrese el campo TipoReclamo", "error");
            return;
        }

        params['IdWorkOrder'] = $('#IdWorkOrderh2').val();
        params['Base'] = $('#txtBase').val();
        params['Imagen'] = $('#txtImagen').val();
        params['MIN'] = $('#txtMin').val();


        params['FechaActivacion'] = $('#txtFechaActivacion').val();
        params['Curcode'] = $('#txtCurcode').val();
        params['Nombre'] = $('#txtNombre').val();
        params['Apellido'] = $('#txtApellido').val();
        params['Canal'] = $('#txtCanal').val();
        params['Ascard'] = $('#txtAscard').val();
        params['FechaReposicion'] = $('#TxtFechaReposicion').val();
        params['Contrato'] = $('#txtContrato').val();
        params['Grabacion'] = $('#txtGrabacion').val();
        params['Reasignacion'] = $('#TxtReasignacion').val();
        params['Estado'] = $('#txtEstado').val();
        params['Legalizado'] = $('#txtLegalizado').val();
        params['Observaciones'] = $('#txtObservaciones').val();
        params['RangoProbable'] = $('#txtRangoProbable').val();
        params['DireccionInformaCliente'] = $('#txtDireccion').val();
        params['CustomerID'] = $('#txtCustomerId').val();
        params['Ciudad'] = $('#txtCiudad').val();
        params['Departamento'] = $('#txtDepartamento').val();
        params['FechaRadicacion'] = $('#txtFechaRadicacion').val();
        params['Notificacion'] = $('#txtNotificacion').val();
        params['FechaDesactivacion'] = $('#txtFechaDesactivacion').val();
        params['PQR'] = $('#txtPqr').val();
        params['Cedula'] = $('#txtCedula').val();
        params['AreaRadica'] = $('#txtAreaRadica').val();
        params['TipoReclamo'] = $('#txtTipoReclamo').val();

        let Result = await $.ajax({
            type: "post",
            url: '/WorkOrderSolutions/GuardarNegacionLinea_Alistamiento',
            data: params,
        });
        swal("Bien Hecho!", "Acción realizada correctamente.", "success");
        //await ListarDatosNegacionLinea();
        // $('#myModalNegacion').modal('hide');
        location.reload();
    } catch (ex) {
        swal("Error Interno ", "Comunicate con el area de IT para la verificacion de este error..", "error");
    } finally {

        // $('#myModalNegacion').modal('hide');
        //ToolsFuncBtnLoader($Btn, 0);

    }
}

async function ListarDatosNegacionLinea() {
    let param = new Object();
    param['IdWorkOrder'] = $('#h_IdWorkOrder').val();

    let Result = await $.ajax({
        //contentType: 'application/json; charset=utf-8',
        type: "post",
        url: '/WorkOrderSolutions/NegacionLinea_Alistamiento',
        data: param,
    });
    $("#TableGeneral").html(Result);
    // await ToolsDataTables();
}





function GenerarExcelNegacionLineaPendiente(event) {

    var h_IdWorkOrder;
    h_IdWorkOrder = $('#h_IdWorkOrder').val();

    event.preventDefault();

    location.href = "/WorkOrderSolutions/NegacionLinea_AlistamientoExcelPendiente?IdWorkOrder=" + h_IdWorkOrder;
}


function GenerarExcelNegacionLinea(event) {
    var h_IdWorkOrder;
    h_IdWorkOrder = $('#h_IdWorkOrder').val();
    event.preventDefault();
    location.href = "/WorkOrderSolutions/NegacionLinea_Alistamiento_ExcelTodo?IdWorkOrder=" + h_IdWorkOrder;
}

function DeletelineaNegacion2(Id) {
    try {
        swal({
            title: "¿Quieres continuar?",
            text: "¿Deseas eliminar este Ítem?",
            type: "warning",
            showCancelButton: true,
            confirmButtonText: "Si, Eliminar!",
            closeOnConfirm: true,
            showLoaderOnConfirm: true
        }, async function () {
            let params = new Object();
            params['Id'] = Id;
            let Result = await $.ajax({
                type: "post",
                url: '/WorkOrderSolutions/EliminarNegacionLinea_Alistamiento',
                data: params
            });
            // await ListarDatosNegacionLinea();
            location.reload();
            toastr.success('<b> El Ítem </b> fue eliminado correctamente.', 'Eliminado');
        });
    } catch (ex) {
        swal("Error interno", "Comunicate con el area de IT para la verificacion de este error..", "error");
    }


}



async function CargarExcelNegacionLinea() {
    try {
        var h_IdWorkOrder;
        h_IdWorkOrder = $('#h_IdWorkOrder').val();


        var file = document.getElementById('ArchivoExcel').files;
        var formData = new FormData();
        formData.append("ArchivoExcel", file);
        formData.append("IdWorkOrder", h_IdWorkOrder);

        let Result = await $.ajax({
            type: "post",
            url: '/WorkOrderSolutions/CargarExcelNegacionLinea',
            processData: false,
            contentType: false,
            cache: false,
            dataType: 'json',
            data: formData
        });

        swal({ title: "Bien Hecho!", text: "Accion realizada correctamente.", type: "success" }, function () {
            location.reload();
        });
    } catch (ex) {

        swal("Error interno", "Comunicate con el area de IT para la verificacion de este error..", "error");
    }
    finally {
        //await AtachmentMiss();
    }
}



function checkNumber(e) {
    tecla = (document.all) ? e.keyCode : e.which;
    //Tecla de retroceso para borrar, siempre la permite
    if (tecla == 8) {
        return true;
    }
    // Patron de entrada, en este caso solo acepta numeros 
    patron = /^[0-9]+/;
    tecla_final = String.fromCharCode(tecla);
    return patron.test(tecla_final);
}
var flag = 0;
var control = 0;

async function showAdjustScreen1() {

    if (flag == 0) {

        swal("Activacion de Ajuste BPB!", "Se debe diligenciar el campo de Ajustes en SI para realizar mas de un ajuste", "success");
        $("#lbl_725").show();
        $("#field_725").show();
        $("#field_725").addClass('form-control Req-2 fieldSave requiredForever');
        $("#divCta_Contable").show();
        $("#DdlServicio").addClass('form-control Req-2 requiredForever');
        $("#DdlCTA_Contable").addClass('form-control Req-2 requiredForever');
        $("#lbl_727").show();
        $("#field_727").show();
        $("#field_727").addClass('form-control Req-2 fieldSave requiredForever');
        $("#lbl_726").show();
        $("#field_726").show();
        $("#field_726").addClass('form-control Req-2 fieldSave requiredForever');
        $("#lbl_728").show();
        $("#field_728").show();
        $("#field_728").addClass('form-control Req-2 fieldSave requiredForever');
        $("#lbl_729").show();
        $("#729").show();
        $("#field_729").show();
        $("#field_729").removeClass('form-control Req-2 fieldSave datepicker requiredForever');
        $("#field_729").addClass('form-control Req-1 fieldSave datepicker requiredForever');
        $("#lbl_730").show();
        $("#730").show();
        $("#field_730").show();
        $("#field_730").removeClass('form-control Req-2 fieldSave datepicker requiredForever');
        $("#field_730").addClass('form-control Req-1 fieldSave datepicker requiredForever');
        $("#lbl_767").show();
        $("#field_767").show();
        $("#lbl_768").show();
        $("#field_768").show();
        $("#lbl_769").show();
        $("#field_769").show();
        $("#lbl_770").show();
        $("#field_770").show();
        $("#lbl_771").show();
        $("#field_771").show();
        $("#lbl_772").show();
        $("#field_772").show();
        flag = 1;
        control = control + 1;
    }
    else {
        swal("Desactivacion de Ajuste BPB!", "Se debe diligenciar el campo de Ajuste en NO", "success");
        $("#lbl_725").show();
        $("#field_725").show();
        $("#field_725").removeClass('form-control Req-2 fieldSave requiredForever');
        $("#field_725").addClass('form-control Req-1 fieldSave');
        $("#divCta_Contable").hide();
        $("#DdlServicio").removeClass('form-control Req-2 requiredForever');
        $("#DdlServicio").addClass('form-control');
        $("#DdlCTA_Contable").removeClass('form-control Req-2 requiredForever');
        $("#DdlCTA_Contable").addClass('form-control');
        $("#lbl_727").hide();
        $("#field_727").hide();
        $("#field_727").removeClass('form-control Req-2 fieldSave requiredForever');
        $("#field_727").addClass('form-control Req-1 fieldSave');
        $("#lbl_726").hide();
        $("#field_726").hide();
        $("#field_726").removeClass('form-control Req-2 fieldSave requiredForever');
        $("#field_726").addClass('form-control Req-1 fieldSave');
        $("#lbl_728").hide();
        $("#field_728").hide();
        $("#field_728").removeClass('form-control Req-2 fieldSave requiredForever');
        $("#field_728").addClass('form-control Req-1 fieldSave');
        $("#lbl_729").hide();
        $("#729").hide();
        $("#field_729").hide();
        $("#field_729").removeClass('form-control Req-2 fieldSave requiredForever');
        $("#field_729").addClass('form-control Req-1 fieldSave');
        $("#lbl_730").hide();
        $("#730").hide();
        $("#field_730").hide();
        $("#field_730").removeClass('form-control Req-2 fieldSave requiredForever');
        $("#field_730").addClass('form-control Req-1 fieldSave');
        $("#lbl_767").hide();
        $("#field_767").hide();
        $("#lbl_768").hide();
        $("#field_768").hide();
        $("#lbl_769").hide();
        $("#field_769").hide();
        $("#lbl_770").hide();
        $("#field_770").hide();
        $("#lbl_771").hide();
        $("#field_771").hide();
        $("#lbl_772").hide();
        $("#field_772").hide();
        flag = 0;
        control = control - 1;
    }

}
async function showAdjustScreen2() {

    if (flag == 0) {
        swal("Activacion de Ajuste Ascard!", "Se debe diligenciar el campo de Ajustes en SI para realizar mas de un ajuste", "success");
        $("#lbl_725").show();
        $("#field_725").show();
        $("#field_725").addClass('form-control Req-2 fieldSave requiredForever');
        $("#lbl_731").show();
        $("#field_731").show();
        $("#field_731").addClass('form-control Req-2 fieldSave requiredForever');
        $("#lbl_732").show();
        $("#field_732").show();
        $("#field_732").addClass('form-control Req-2 fieldSave requiredForever');
        $("#lbl_733").show();
        $("#field_733").show();
        $("#field_733").addClass('form-control Req-2 fieldSave requiredForever');
        $("#lbl_734").show();
        $("#field_734").show();
        $("#field_734").addClass('form-control Req-2 fieldSave requiredForever');
        $("#lbl_735").show();
        $("#field_735").show();
        $("#field_735").addClass('form-control Req-2 fieldSave requiredForever');
        $("#lbl_736").show();
        $("#field_736").show();
        $("#field_736").addClass('form-control Req-2 fieldSave requiredForever');
        $("#lbl_737").show();
        $("#field_737").show();
        $("#field_737").addClass('form-control Req-2 fieldSave requiredForever');
        $("#lbl_738").show();
        $("#field_738").show();
        $("#field_738").addClass('form-control Req-2 fieldSave requiredForever');
        flag = 1;
        control = control + 1;
    }
    else {
        swal("Desactivacion de Ajuste Ascard!", "Se debe diligenciar el campo de Ajuste en NO", "success");
        $("#lbl_725").show();
        $("#field_725").show();
        $("#field_725").removeClass('form-control Req-2 fieldSave requiredForever');
        $("#field_725").addClass('form-control Req-1 fieldSave');
        $("#lbl_731").hide();
        $("#field_731").hide();
        $("#field_731").removeClass('form-control Req-2 fieldSave requiredForever');
        $("#field_731").addClass('form-control Req-1 fieldSave');
        $("#lbl_732").hide();
        $("#field_732").hide();
        $("#field_732").removeClass('form-control Req-2 fieldSave requiredForever');
        $("#field_732").addClass('form-control Req-1 fieldSave');
        $("#lbl_733").hide();
        $("#field_733").hide();
        $("#field_733").removeClass('form-control Req-2 fieldSave requiredForever');
        $("#field_733").addClass('form-control Req-1 fieldSave');
        $("#lbl_734").hide();
        $("#field_734").hide();
        $("#field_734").removeClass('form-control Req-2 fieldSave requiredForever');
        $("#field_734").addClass('form-control Req-1 fieldSave');
        $("#lbl_735").hide();
        $("#field_735").hide();
        $("#field_735").removeClass('form-control Req-2 fieldSave requiredForever');
        $("#field_735").addClass('form-control Req-1 fieldSave');
        $("#lbl_736").hide();
        $("#field_736").hide();
        $("#field_736").removeClass('form-control Req-2 fieldSave requiredForever');
        $("#field_736").addClass('form-control Req-1 fieldSave');
        $("#lbl_737").hide();
        $("#field_737").hide();
        $("#field_737").removeClass('form-control Req-2 fieldSave requiredForever');
        $("#field_737").addClass('form-control Req-1 fieldSave');
        $("#lbl_739").hide();
        $("#field_739").hide();
        $("#field_739").removeClass('form-control Req-2 fieldSave requiredForever');
        $("#field_739").addClass('form-control Req-1 fieldSave');
        flag = 0;
        control = control - 1;
    }

}
async function showAdjustScreen3() {

    if (flag == 0) {
        swal("Activacion de Ajuste Ascard 2!", "Se debe diligenciar el campo de Ajustes en SI para realizar mas de un ajuste", "success");
        $("#lbl_725").show();
        $("#field_725").show();
        $("#field_725").addClass('form-control Req-2 fieldSave requiredForever');
        $("#lbl_740").show();
        $("#field_740").show();
        $("#field_740").addClass('form-control Req-2 fieldSave requiredForever');
        $("#lbl_741").show();
        $("#field_741").show();
        $("#field_741").addClass('form-control Req-2 fieldSave requiredForever');
        $("#lbl_742").show();
        $("#field_742").show();
        $("#field_742").addClass('form-control Req-2 fieldSave requiredForever');
        $("#lbl_743").show();
        $("#field_743").show();
        $("#field_743").addClass('form-control Req-2 fieldSave requiredForever');
        $("#lbl_744").show();
        $("#field_744").show();
        $("#field_744").addClass('form-control Req-2 fieldSave requiredForever');
        $("#lbl_745").show();
        $("#field_745").show();
        $("#field_745").addClass('form-control Req-2 fieldSave requiredForever');
        flag = 1;
        control = control + 1;
    }
    else {
        swal("Desactivacion de Ajuste Ascard 2!", "Se debe diligenciar el campo de Ajuste en NO", "success");
        $("#lbl_725").show();
        $("#field_725").show();
        $("#field_725").removeClass('form-control Req-2 fieldSave requiredForever');
        $("#field_725").addClass('form-control Req-1 fieldSave');
        $("#lbl_740").hide();
        $("#field_740").hide();
        $("#field_740").removeClass('form-control Req-2 fieldSave requiredForever');
        $("#field_740").addClass('form-control Req-1 fieldSave');
        $("#lbl_741").hide();
        $("#field_741").hide();
        $("#field_741").removeClass('form-control Req-2 fieldSave requiredForever');
        $("#field_741").addClass('form-control Req-1 fieldSave');
        $("#lbl_742").hide();
        $("#field_742").hide();
        $("#field_742").removeClass('form-control Req-2 fieldSave requiredForever');
        $("#field_742").addClass('form-control Req-1 fieldSave');
        $("#lbl_743").hide();
        $("#field_743").hide();
        $("#field_743").removeClass('form-control Req-2 fieldSave requiredForever');
        $("#field_743").addClass('form-control Req-1 fieldSave');
        $("#lbl_744").hide();
        $("#field_744").hide();
        $("#field_744").removeClass('form-control Req-2 fieldSave requiredForever');
        $("#field_744").addClass('form-control Req-1 fieldSave');
        $("#lbl_745").hide();
        $("#field_745").hide();
        $("#field_745").removeClass('form-control Req-2 fieldSave requiredForever');
        $("#field_745").addClass('form-control Req-1 fieldSave');
        flag = 0;
        control = control - 1;

    }
}
async function showAdjustScreen4() {

    if (flag == 0) {
        swal("Activacion de Ajuste Incidencias PQR!", "Se debe diligenciar el campo de Ajustes en SI para realizar mas de un ajuste", "success");
        $("#lbl_725").show();
        $("#field_725").show();
        $("#field_725").addClass('form-control Req-2 fieldSave requiredForever');
        $("#lbl_746").show();
        $("#field_746").show();
        $("#field_746").addClass('form-control Req-2 fieldSave requiredForever');
        $("#lbl_747").show();
        $("#field_747").show();
        $("#field_747").addClass('form-control Req-2 fieldSave requiredForever');
        $("#lbl_748").show();
        $("#field_748").show();
        $("#field_748").addClass('form-control Req-2 fieldSave requiredForever');
        $("#lbl_749").show();
        $("#field_749").show();
        $("#field_749").addClass('form-control Req-2 fieldSave requiredForever');
        $("#lbl_750").show();
        $("#750").show();
        $("#field_750").show();
        $("#field_750").addClass('form-control Req-2 fieldSave requiredForever');
        $("#lbl_751").show();
        $("#751").show();
        $("#field_751").show();
        $("#field_751").addClass('form-control Req-2 fieldSave requiredForever');
        $("#lbl_752").show();
        $("#field_752").show();
        $("#field_752").addClass('form-control Req-2 fieldSave requiredForever');
        $("#lbl_753").show();
        $("#field_753").show();
        $("#field_753").addClass('form-control Req-2 fieldSave requiredForever');
        $("#lbl_754").show();
        $("#field_754").show();
        $("#field_754").addClass('form-control Req-2 fieldSave requiredForever');
        $("#lbl_755").show();
        $("#755").show();
        $("#field_755").show();
        $("#field_755").addClass('form-control Req-2 fieldSave requiredForever');
        $("#lbl_756").show();
        $("#field_756").show();
        $("#field_756").addClass('form-control Req-2 fieldSave requiredForever');
        $("#lbl_757").show();
        $("#field_757").show();
        $("#field_757").addClass('form-control Req-2 fieldSave requiredForever');
        $("#lbl_774").show();
        $("#field_774").show();
        flag = 1;
        control = control + 1;
    }
    else {
        swal("Desactivacion de Ajuste Incidencias PQR!", "Se debe diligenciar el campo de Ajuste en NO", "success");
        $("#lbl_725").show();
        $("#field_725").show();
        $("#field_725").removeClass('form-control Req-2 fieldSave requiredForever');
        $("#field_725").addClass('form-control Req-1 fieldSave');
        $("#lbl_746").hide();
        $("#field_746").hide();
        $("#field_746").removeClass('form-control Req-2 fieldSave requiredForever');
        $("#field_746").addClass('form-control Req-1 fieldSave');
        $("#lbl_747").hide();
        $("#field_747").hide();
        $("#field_747").removeClass('form-control Req-2 fieldSave requiredForever');
        $("#field_747").addClass('form-control Req-1 fieldSave');
        $("#lbl_748").hide();
        $("#field_748").hide();
        $("#field_748").removeClass('form-control Req-2 fieldSave requiredForever');
        $("#field_748").addClass('form-control Req-1 fieldSave');
        $("#lbl_749").hide();
        $("#field_749").hide();
        $("#field_749").removeClass('form-control Req-2 fieldSave requiredForever');
        $("#field_749").addClass('form-control Req-1 fieldSave');
        $("#lbl_750").hide();
        $("#750").hide();
        $("#field_750").hide();
        $("#field_750").removeClass('form-control Req-2 fieldSave requiredForever');
        $("#field_750").addClass('form-control Req-1 fieldSave');
        $("#lbl_751").hide();
        $("#751").hide();
        $("#field_751").hide();
        $("#field_751").removeClass('form-control Req-2 fieldSave requiredForever');
        $("#field_751").addClass('form-control Req-1 fieldSave');
        $("#lbl_752").hide();
        $("#field_752").hide();
        $("#field_752").removeClass('form-control Req-2 fieldSave requiredForever');
        $("#field_752").addClass('form-control Req-1 fieldSave');
        $("#lbl_753").hide();
        $("#field_753").hide();
        $("#field_753").removeClass('form-control Req-2 fieldSave requiredForever');
        $("#field_753").addClass('form-control Req-1 fieldSave');
        $("#lbl_754").hide();
        $("#field_754").hide();
        $("#field_754").removeClass('form-control Req-2 fieldSave requiredForever');
        $("#field_754").addClass('form-control Req-1 fieldSave');
        $("#lbl_755").hide();
        $("#755").hide();
        $("#field_755").hide();
        $("#field_755").removeClass('form-control Req-2 fieldSave requiredForever');
        $("#field_755").addClass('form-control Req-1 fieldSave');
        $("#lbl_756").hide();
        $("#field_756").hide();
        $("#field_756").removeClass('form-control Req-2 fieldSave requiredForever');
        $("#field_756").addClass('form-control Req-1 fieldSave');
        $("#lbl_757").hide();
        $("#field_757").hide();
        $("#field_757").removeClass('form-control Req-2 fieldSave requiredForever');
        $("#field_757").addClass('form-control Req-1 fieldSave');
        $("#lbl_774").hide();
        $("#field_774").hide();
        flag = 0;
        control = control - 1;
    }
}

async function showAdjustScreen5() {

    if (flag == 0) {

        //swal("Activacion de Ajuste BPB!", "Se debe diligenciar el campo de Ajustes en SI para realizar mas de un ajuste", "success");
        $("#lbl_780").show();
        $("#field_780").show();
        $("#field_780").addClass('form-control Req-2 fieldSave requiredForever');
        $("#lbl_781").show();
        $("#field_781").show();
        $("#field_781").addClass('form-control Req-2 fieldSave requiredForever');
        $("#lbl_782").show();
        $("#field_782").show();
        $("#field_782").addClass('form-control Req-2 fieldSave requiredForever');
        $("#field_782").val('Dime');
        $("#lbl_783").show();
        $("#field_783").show();
        $("#field_783").addClass('form-control Req-2 fieldSave requiredForever');
        $("#lbl_784").show();
        $("#field_784").show();
        $("#field_784").addClass('form-control Req-2 fieldSave requiredForever');
        $("#lbl_785").show();
        $("#field_785").show();
        $("#field_785").addClass('form-control Req-2 fieldSave requiredForever');
        $("#lbl_786").show();
        $("#field_786").show();
        $("#field_786").addClass('form-control Req-2 fieldSave requiredForever');
        $("#lbl_787").show();
        $("#field_787").show();
        $("#field_787").addClass('form-control Req-2 fieldSave requiredForever');

        flag = 1;
        control = control + 1;
    }
    else {

        $("#lbl_780").hide();
        $("#field_780").hide();
        $("#field_780").removeClass('form-control Req-2 fieldSave requiredForever');
        $("#lbl_781").hide();
        $("#field_781").hide();
        $("#field_781").removeClass('form-control Req-2 fieldSave requiredForever');
        $("#lbl_782").hide();
        $("#field_782").hide();
        $("#field_782").removeClass('form-control Req-2 fieldSave requiredForever');
        $("#lbl_783").hide();
        $("#field_783").hide();
        $("#field_783").removeClass('form-control Req-2 fieldSave requiredForever');
        $("#lbl_784").hide();
        $("#field_784").hide();
        $("#field_784").removeClass('form-control Req-2 fieldSave requiredForever');
        $("#lbl_785").hide();
        $("#field_785").hide();
        $("#field_785").removeClass('form-control Req-2 fieldSave requiredForever');
        $("#lbl_786").hide();
        $("#field_786").hide();
        $("#field_786").removeClass('form-control Req-2 fieldSave requiredForever');
        $("#lbl_787").hide();
        $("#field_787").hide();
        $("#field_787").removeClass('form-control Req-2 fieldSave requiredForever');
        //$("#lbl_726").hide();
        //$("#field_726").hide();
        //$("#field_726").removeClass('form-control Req-2 fieldSave requiredForever');
        //$("#field_726").addClass('form-control Req-1 fieldSave');
        //$("#lbl_728").hide();
        //$("#field_728").hide();
        //$("#field_728").removeClass('form-control Req-2 fieldSave requiredForever');
        //$("#field_728").addClass('form-control Req-1 fieldSave');
        //$("#lbl_729").hide();
        //$("#729").hide();
        //$("#field_729").hide();
        //$("#field_729").removeClass('form-control Req-2 fieldSave requiredForever');
        //$("#field_729").addClass('form-control Req-1 fieldSave');
        //$("#lbl_730").hide();
        //$("#730").hide();
        //$("#field_730").hide();
        //$("#field_730").removeClass('form-control Req-2 fieldSave requiredForever');
        //$("#field_730").addClass('form-control Req-1 fieldSave');
        //$("#lbl_767").hide();
        //$("#field_767").hide();
        //$("#lbl_768").hide();
        //$("#field_768").hide();
        //$("#lbl_769").hide();
        //$("#field_769").hide();
        //$("#lbl_770").hide();
        //$("#field_770").hide();
        //$("#lbl_771").hide();
        //$("#field_771").hide();
        //$("#lbl_772").hide();
        //$("#field_772").hide();
        flag = 0;
        control = control - 1;
    }

}

//excel planilla ajuste movil
function GenerarExcel(event) {
    event.preventDefault();

    var fechainicio = $('#txtFechaInicio').val();
    var fechafin = $('#txtFechaFinal').val();
    var parametersString = "";

    if (fechainicio != "") {
        if (parametersString.includes('?')) {
            parametersString += '&FechaInicio=' + fechainicio;
        } else {
            parametersString += '?FechaInicio=' + fechainicio;
        }
    }

    if (fechafin != "") {
        if (parametersString.includes('?')) {
            parametersString += '&FechaFinal=' + fechafin;
        } else {
            parametersString += '?FechaFinal=' + fechafin;
        }
    }

    location.href = "/WorkOrderSolutions/PlanillaAjustesMovil_Excel" + parametersString;
}

async function getDispositionsByServicio() {
    var $option = '<option value="">--Seleccionar--</option>';
    if (true) {
        var params = new Object();
        params['servicio'] = $("#TxtSearchServicio").val();
        var Result = await $.ajax({
            type: "get",
            url: '/WorkOrderSolutions/GetDispositionsByServicio/',
            data: params
        });
        $.each(Result, function (i, data) {
            $option += '<option value="' + data.ID + '">' + data.Nombre + '</option>';
        });
    }
    $("#DdlServicio").html($option);
    return false;
}
async function getCtaContable() {
    var $option = '<option value="">--Seleccionar--</option>';
    if (true) {
        var params = new Object();
        params['ID'] = $("#DdlServicio").val();
        var Result = await $.ajax({
            type: "get",
            url: '/WorkOrderSolutions/GetCtaContable/',
            data: params
        });
        $.each(Result, function (i, data) {
            $option += '<option value="' + data.ID + '">' + data.Nombre + '</option>';
        });
    }
    complete: $("#DdlCTA_Contable").html($option);
    return false;
}
async function BtnGuardar_2($Btn) {
    try {
        //ToolsFuncBtnLoader($Btn, 1);

        let params = new Object();


        if ($('#IdWorkOrderh2').val() === '') {
            swal("Error", "Ingrese el campo id del caso", "error");
            return;
        }

        params['IdWorkOrder'] = $('#IdWorkOrderh2').val();
        params["Status"] = {
            IdStatusDefinition: $('#DdlStatus').val()
        }
        params["SubStatus"] = {
            IdStatusDefinition: $('#DdlSubStatus').val()
        }
        params["Resolutions"] = $("#TxtResolution").summernote("code");
        params["ListFielsUDFSolution"] = objFields;
        params["WorkOrderEscalations"] = {
            GroupsScaled: {
                IdMasterGroups: $('#DdlGroupScale').val()
            },
            TypeScaling: $('#DdlTypeScaled').val(),
            Comments: $('#TxtCommentsScaled').val()
        }

        var Result = await $.ajax({
            type: "post",
            url: '/WorkOrderSolutions/SaveWorkOrderSolutions_2',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(params)
        });
        swal("Bien Hecho!", "Acción realizada correctamente.", "success");
        window.location = "/ListWorkOrders";
    } catch (ex) {
        swal("Error interno", "Comunicate con el area de IT para la verificacion de este error..", "error");
    } finally {

    }
}
async function BtnGuardar_Ng() {
    swal("Bien Hecho!", "Acción realizada correctamente.", "success");
    window.location = "/ListWorkOrders";
}

//codigo nuevo
// formato a numero
$("#field_997").on('keydown keypress', function (e) { // NUMERICO EN PLANTILLA FIJA AL CAMPO PQR
    if (e.key.length === 1) {
        if ($(this).val().length < 9 && !isNaN(parseFloat(e.key))) {
            $(this).val($(this).val() + e.key);

        }
        return false;
    }
})

$("#field_998").on('keydown keypress', function (e) { // NUMERICO EN PLANTILLA FIJA AL CAMPO PQR AJU AIB
    if (e.key.length === 1) {
        if ($(this).val().length < 9 && !isNaN(parseFloat(e.key))) {
            $(this).val($(this).val() + e.key);
        }
        return false;
    }
})

//validacion de caracteres en PQR y PQR AJU
//field_997.onblur = function () { // al perder el foco el campo area que genero ajuste
//    if ($(this).val().length < 9) {
//        $(this).val("");
//    }
//};

//field_998.onblur = function () { // al perder el foco el campo area que genero ajuste
//    if ($(this).val().length < 9) {
//        $(this).val("");
//    }
//};