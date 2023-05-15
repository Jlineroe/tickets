PermisosActions('Status');
DataTables();
$('#divLoaderMaster').hide();
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
    const $BtnGuardar = document.getElementById('BtnGuardar');
    $BtnGuardar.addEventListener('click', async (event) => {
        $('.content-wrapper .form-control').removeAttr('required');
        try {
            //event.preventDefault();
            var $elementRequired = $('.required');
            //var nameEstado = document.getElementById('TxtNameStatus').value;
            //var Sitio = document.getElementById('DdlSitio').value;
              

            var Valida = ValidarVacios($elementRequired);
            
            if (Valida == false) {

            } else if (Valida == "error") {
                event.preventDefault();
            } else {
                event.preventDefault();
                FuncBtnLoader($BtnGuardar, 1);
                var params = new Object();
                params['IdStatusDefinition'] = $('#HdIdStatus').val();
                params['NameStatus'] = $('#TxtNameStatus').val();
                params['TypeAction'] = {
                    IdTypeActions: $('#DdlTipoEstado').val()
                };
                params['Sitio'] = {
                    IdMasterSites: $('#DdlSitio').val()
                };
                params['DescriptionStatus'] = $('#TxtDescripcionStatus').val();
                var estado = $('#DdlEstado').val();
                params['State'] = (estado == 1 ? true : false);
                var $optionSubStatus = $(".newOption");
                var SubStatus = [];
                for (var i = 0; i < $optionSubStatus.length; i++) {
                    SubStatus.push({
                        NameStatus: $optionSubStatus[i].value
                    });
                }
                params['SubStatus'] = SubStatus;
                var Result = await $.ajax({
                    type: "post",
                    url: '/Status/SaveStatus',
                    data: params,                                       
                })

                if (Result != "") {
                    swal("Error", "Ya existe un estado con este nombre y este sitio, verifique", "error");
                    return;
                }

                swal("Bien Hecho!", "Accion realizada correctamente.", "success");
                await ListadoStatus();
                $('#myModal').modal('hide');
            }
        } catch (ex) {
            swal("Error interno", "Comunicate con el area de IT para la verificacion de este error..", "error");
        } finally {
            setTimeout(function () {
                FuncBtnLoader($BtnGuardar, 0);
            }, 500);
        }
    });
})();

async function ValidarEstado(pNameEstado, pSitio) {
    try { 
        var params = new Object();
        params["NameStatus"] = pNameEstado;
        params["pIdSitio"] = pSitio;  
        var Result = await $.ajax({
            type: "post",
            url: '/Status/VerifyNameStatus',
            contentType: 'application/json; charset=utf-8',
            data: params
        });
        if (Result == true) {
            $('#TxtNameStatus').removeClass('is-invalid');
            $('#TxtNameStatus-error').html('');
        } else {           
            $('#TxtNameStatus').addClass('is-invalid');
            $('#TxtNameStatus-error').html(Result);
        }  
    } catch (ex) {
        swal("Error interno", "Comunicate con el area de IT para la verificacion de este error..", "error");
    }
}

async function VerifyNameStatus(value) {
    value = value.trim();
    if (value != "") {
        var Validar = $('#DdlSubStatus option[value="' + value + '"]').val();
        if (Validar != undefined) {
            $('#TxtNameStatus').addClass('is-invalid');
            $('#TxtNameStatus-error').html(value + " ya se encuentra agregado a los sub estados de este estado.");
        } /*else {
            var params = new Object();
            params["NameStatus"] = value;
            var Result = await $.ajax({
                type: "post",
                url: '/Status/VerifyNameStatus',
                data: params
            });
            if (Result == true) {
                $('#TxtNameStatus').removeClass('is-invalid');
                $('#TxtNameStatus-error').html('');
            } else {
                $('#TxtNameStatus').addClass('is-invalid');
                $('#TxtNameStatus-error').html(Result);
            }
        }*/
    }
}
async function VerifyNameSubStatus(value) {
    value = value.trim();
    if (value != "") {
        var Validar = $('#DdlSubStatus option[value="' + value + '"]').val();
        var NameStatus = $('#TxtNameStatus').val();
        if (Validar != undefined) {
            $('#TxtSubStatus').addClass('is-invalid');
            $('#TxtSubStatus-error').html(value + " ya se encuentra agregado a este estado.");
        } else if (NameStatus == value) {
            $('#TxtSubStatus').addClass('is-invalid');
            $('#TxtSubStatus-error').html("No puede tener el mismo nombre del estado actual.");
        } else {
            var params = new Object();
            params["NameStatus"] = value;
            var Result = await $.ajax({
                type: "post",
                url: '/Status/VerifyNameStatus',
                data: params
            });
            if (Result == true) {
                $('#TxtSubStatus').removeClass('is-invalid');
                $('#TxtSubStatus-error').html('');
            } else {
                $('#TxtSubStatus').addClass('is-invalid');
                $('#TxtSubStatus-error').html(value + " ya se encuentra agregado como un estado o sub estado.");
            }
        }
    }
}
async function AddOptionSubStatus() {
    var SubStatus = $('#TxtSubStatus').val();
    await VerifyNameSubStatus(SubStatus);
    var valiError = $('#TxtSubStatus')[0].classList.contains('is-invalid');
    if (SubStatus == "") {
        toastr.error('Escriba una sub estado para continuar.', 'No puede ser vacío');
    } else if (valiError == true) {
        toastr.error('Verifique los errores para continuar.', 'Verificar');
        $('#TxtSubStatus').focus();
    } else {
        $('#DdlSubStatus').append('<option class="newOption" value="' + SubStatus + '">' + SubStatus + '</option>');
        $('#TxtSubStatus').val('');
        toastr.success('<b>' + SubStatus + '</b> fue agregado correctamente no olvide guardar los cambios.', 'Bien hecho');
    }
}
async function DeleteSubStatus() {
    try {
        var TextSubStatus = $('#DdlSubStatus option:selected').html();
        var SubStatus = $('#DdlSubStatus').val();
        var $element = $("#DdlSubStatus option[value='" + SubStatus + "']");

        if (SubStatus == "") {
            toastr.error('Seleccione un sub estado para continuar.', 'Seleccione');
        } else if ($element[0].classList.contains('newOption')) {
            $("#DdlSubStatus option[value='" + SubStatus + "']").remove();
            $('#DdlSubStatus').val('');
            toastr.success('<b>' + SubStatus + '</b> fue eliminado correctamente.', 'Eliminado');
        } else {
            swal({
                title: "¿Quieres continuar?",
                text: "¿Deseas eliminar el sub estado " + TextSubStatus + "?",
                type: "warning",
                showCancelButton: true,
                confirmButtonText: "Si, Eliminar!",
                closeOnConfirm: true,
                showLoaderOnConfirm: true
            }, async function () {
                var params = new Object();
                params['IdStatus'] = SubStatus;
                var Result = await $.ajax({
                    type: "post",
                    url: '/Status/DeleteStatus',
                    data: params
                });
                $("#DdlSubStatus option[value='" + SubStatus + "']").remove();
                $('#DdlSubStatus').val('');
                toastr.success('<b>' + TextSubStatus + '</b> fue eliminado correctamente.', 'Eliminado');
            });
        }
    } catch (ex) {
        swal("Error interno", "Comunicate con el area de IT para la verificacion de este error..", "error");
    }
}
async function SearchInforStatus(IdStatus) {
    await limpiarCampos();
    $('#myModal').modal('show');
    var params = new Object();
    params["IdStatus"] = IdStatus;
    var result = await $.ajax({
        type: "post",
        url: '/Status/SearchInforStatus',
        data: params
    });
    $('#HdIdStatus').val(result.IdStatusDefinition);
    $('#DdlTipoEstado').val(result.TypeAction.IdTypeActions);
    $('#DdlSitio').val(result.Sitio.IdMasterSites);
    $('#TxtNameStatus').val(result.NameStatus);
    $('#TxtDescripcionStatus').val(result.DescriptionStatus);
    var estado = (result.State == true ? 1 : 0);
    $('#DdlEstado').val(estado).prop('disabled', result.State);
    var SubStatus = result.SubStatus;
    var $option = '<option value="">--Seleccionar--</option>';
    $.each(SubStatus, function (key, value) {
        $option += '<option value="' + value.IdStatusDefinition + '">' + value.NameStatus + '</option>';
    });
    $('#DdlSubStatus').html($option);
}
async function DeleteStatus(IdStatus) {
    swal({
        title: "¿Deseas desactivar este estado?",
        text: "¿Estas seguro?",
        type: "warning",
        showCancelButton: true,
        confirmButtonText: "Si, desactivar!",
        closeOnConfirm: false,
        showLoaderOnConfirm: true
    }, async function () {
        var params = new Object();
        params['IdStatus'] = IdStatus;
        var Result = await $.ajax({
            type: "post",
            url: '/Status/DeleteStatus',
            data: params
        });
        swal("Bien Hecho!", "Estado Desactivado exitosamente.", "success");
        await ListadoStatus();
    });
}
async function ListadoStatus() {
    var Result = await $.ajax({
        contentType: 'application/json; charset=utf-8',
        type: "post",
        url: '/Status/ListStatus',
    });
    $("#divTable").html(Result);
    await DataTables();
}
async function DataTables() {
    try {
        var result = await GenerarDTFiltros('divTable');
    } catch (ex) {
        alert(ex);
    } finally {
        $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
    }
}
function limpiarCampos() {
    $('#HdIdStatus').val('');
    $('.form-control').val('');
    $('#TxtNameStatus').removeClass('is-invalid');
    $('#TxtNameStatus-error').html('');
    $('#TxtSubStatus').removeClass('is-invalid');
    $('#TxtSubStatus-error').html('');
    $('#DdlSubStatus').html('<option value="">--Seleccionar--</option>');
    $('#DdlEstado').val(1).prop('disabled', true);
}