$('.datepicker').datepicker({
    todayBtn: "linked",
    calendarWeeks: true,
    format: 'yyyy/mm/dd'
})
$('.chosen-select').chosen()
$('#TxtDescriptionClient').summernote("code", $('#TxtDescriptionClient').text())
$('.summernote-disabled').summernote('disable');
$('#TxtResolution').summernote("code", $('#TxtResolution').text());
$('#DdlStatus').change();
$('#divLoaderMaster').hide();
const $BtnGuardar = document.getElementById('BtnGuardar');
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
                debugger
                var $elementRequired = $('#formCreate .required');
                var $elementRequired2 = $('#formCreate .requiredForever');
                var Valida = await ValidarVacios($elementRequired);
                var ValidaOthers = await ValidarVacios($elementRequired2);
                if (ValidaOthers == false) {

                } else if (Valida == false) {

                } else {
                    event.preventDefault();
                    swal({
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
                            params["txtPQR"] = $('#txtPQR').val();

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
                            debugger
                            var Result = await $.ajax({
                                type: "post",
                                url: '/WorkOrderSolutions/SaveWorkOrderSolutions',
                                contentType: "application/json; charset=utf-8",
                                data: JSON.stringify(params)
                            });
                            swal("Bien Hecho!", "Accion realizada correctamente.", "success");
                            setTimeout(function () {
                                window.location = "/ListWorkOrders";
                            }, 1000);
                        } catch (ex) {
                            swal("Error interno", "Comunicate con el area de IT para la verificacion de este error..", "error");
                        }
                    });
                }
            } catch (ex) {
                swal("Error interno", "Comunicate con el area de IT para la verificacion de este error..", "error");
            } finally {
                FuncBtnLoader($BtnGuardar, 0);
            }
        });
    }
})();