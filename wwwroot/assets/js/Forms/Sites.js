
PermisosActions('Sites');
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
    var $BtnGuardar = document.getElementById('BtnGuardar');
    $BtnGuardar.addEventListener('click', async (event) => {
        $('.container-fluid .form-control').removeAttr('required');
        try {
            var $elementRequired = $('#FormSite .required');
            var Valida = ValidarVacios($elementRequired);
            debugger
            if (Valida == false) {

            } else if (Valida == "error") {
                event.preventDefault();
            } else {
                event.preventDefault();
                FuncBtnLoader($BtnGuardar, 1);
                debugger
                var params = new Object();
                params['NameSite'] = $('#TxtNameSite').val();
                params["RequiredSubStatus"] = $('#chkSubStatus').prop('checked')
                params['DescriptionSite'] = $('#TxtDescripcionSite').val();
                var Result = await $.ajax({
                    type: "post",
                    url: '/Sites/GuardarSitio',
                    data: params,
                });
                swal("Bien Hecho!", "Accion realizada correctamente!", "success");
                await ListadoSitios();
                $('.required').val('');
            }
        } catch (ex) {
            swal("Error interno", "Comunicate con el area de IT para la verificacion de este error..", "error");
        } finally {
            setTimeout(function () {
                FuncBtnLoader($BtnGuardar, 0);
            }, 500);
        }
    });
    var $BtnUpdate = document.getElementById('BtnUpdate');
    $BtnUpdate.addEventListener('click', async (event) => {
        $('.container-fluid .form-control').removeAttr('required');
        try {
            var $elementRequired = $('#myModal .required');
            var Valida = ValidarVacios($elementRequired);
            if (Valida == false) {

            } else {
                event.preventDefault();
                debugger
                FuncBtnLoader($BtnUpdate, 1);
                var params = new Object();
                params['IdMasterSites'] = $('#myModal #HdIdSite').val();
                params['NameSite'] = $('#TxtNameSiteModal').val();
                params["RequiredSubStatus"] = $('#chkSubStatusModal').prop('checked')
                params['DescriptionSite'] = $('#TxtDescripcionSiteModal').val();
                var estado = $('#DdlEstado').val();
                params['State'] = (estado == 1 ? true : false);
                var Result = await $.ajax({
                    type: "post",
                    url: '/Sites/UpdateSitios',
                    data: params,
                });
                swal("Bien Hecho!", "Accion realizada correctamente!", "success");
                await ListadoSitios();
                limpiarCampos();
                $('#myModal').modal('hide');
            }
        } catch (ex) {
            swal("Error interno", "Comunicate con el area de IT para la verificacion de este error..", "error");
        } finally {
            setTimeout(function () {
                FuncBtnLoader($BtnUpdate, 0);
            }, 500);
        }
    });
})();
async function MostrarInforSitio(IdSitio) {
    $('#myModal').modal('show');
    var params = new Object();
    params["IdSite"] = IdSitio;
    var result = await $.ajax({
        type: "post",
        url: '/Sites/SitiosJson',
        data: params
    });
    limpiarCampos();
    $('#myModal #HdIdSite').val(result.IdMasterSites);
    $('#myModal #TxtNameSiteModal').val(result.NameSite);
    $('#myModal #chkSubStatusModal').prop('checked',result.RequiredSubStatus);
    $('#myModal #TxtDescripcionSiteModal').val(result.DescriptionSite);
    var estado = (result.State == true ? 1 : 0);
    $('#myModal #DdlEstado').val(estado).prop('disabled', result.State);
}

function limpiarCampos() {
    $('#myModal #HdIdSite').val('');
    $('#myModal #TxtNameSite').val('');
    $('#myModal #TxtDescripcionSite').val('');
    $('#myModal #DdlEstado').val('');
}
async function VerifyNameSite(value) {
    if (value != "") {
        var params = new Object();
        params["NameSitio"] = value;
        var Result = await $.ajax({
            type: "post",
            url: '/Sites/VerifyNameSitio',
            data: params
        });
        if (Result == true) {
            $('#TxtNameSite').removeClass('is-invalid');
            $('#TxtNameSite-error').html('');
        } else {
            $('#TxtNameSite').addClass('is-invalid');
            $('#TxtNameSite-error').html(Result);
        }
        
    }
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
async function ListadoSitios() {
    var Result = await $.ajax({
        contentType: 'application/json; charset=utf-8',
        type: "post",
        url: '/Sites/ListSitios',
    });
    $("#divTable").html(Result);
    await DataTables();
}
async function DeleteSitio(IdSitio) {
    swal({
        title: "¿Deseas desactivar este sitio?",
        text: "Nadien podra volver a acceder a la informacion de este sitio",
        type: "info",
        showCancelButton: true,
        confirmButtonText: "Si, Desactivar!",
        closeOnConfirm: false,
        showLoaderOnConfirm: true
    }, async function () {
        var params = new Object();
            params['IdSitio'] = IdSitio;
        var Result = await $.ajax({
            type: "post",
            url: '/Sites/DesactivarSitios',
            data: params
        });
        swal("Bien Hecho!", "Sitio desactivado exitosamente.", "success");
        await ListadoSitios();
    });
}