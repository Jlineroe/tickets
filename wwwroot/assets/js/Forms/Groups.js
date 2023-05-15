PermisosActions('Groups');
$('.chosen-select').chosen();
$('.super-chosen').chosen();
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
            var $elementRequired = $('.required');
            var Valida = ValidarVacios($elementRequired);
            if (Valida == false) {

            } else if (Valida == "error") {
                event.preventDefault();
            } else {
                event.preventDefault();
                FuncBtnLoader($BtnGuardar, 1);
                var GruposAEscalar = $("#DdlGruposAEscalar").chosen().val();
                var arrayGrupos = [];
                for (var i = 0; i < GruposAEscalar.length; i++) {
                    arrayGrupos.push({
                        IdMasterGroups: GruposAEscalar[i]
                    });
                }
                var arrayLOB = { Id: $('#DdlLOB').val() };
                var arraySites = { IdMasterSites: $('#DdlSitio').val() };
                var $optionTiposEscalamientos = $(".newOption");
                var escalamientos = [];
                for (var i = 0; i < $optionTiposEscalamientos.length; i++) {
                    escalamientos.push($optionTiposEscalamientos[i].value);
                }
                var params = new Object();
                params['IdMasterGroups'] = $('#HdIdGrupo').val();
                params['NameGroup'] = $('#TxtNameGroup').val();
                params['LOB'] = arrayLOB;
                params['Sitio'] = arraySites;
                params['TypesScaled'] = escalamientos;
                params['ReturnUser'] = $('#ChkDevolver').prop('checked');
                params['DescriptionGroup'] = $('#TxtDescriptionGroup').val();
                var estado = $('#DdlEstado').val();
                params['State'] = (estado == 1 ? true : false);
                if (GruposAEscalar.length > 0)
                    params['GruposAEscalar'] = arrayGrupos;
                var Result = await $.ajax({
                    type: "post",
                    url: '/Groups/SaveGroup',
                    data: params,
                });
                swal("Bien Hecho!", "Accion realizada correctamente.", "success");
                await ListadoGroups();
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
async function ListadoGroups() {
    var Result = await $.ajax({
        contentType: 'application/json; charset=utf-8',
        type: "post",
        url: '/Groups/ListGroups',
    });
    $("#divTable").html(Result);
    await DataTables();
}
async function VerifyNameGroup(value) {
    if (value != "") {
        var params = new Object();
        params["NameGroup"] = value;
        var Result = await $.ajax({
            type: "post",
            url: '/Groups/VerifyNameGroup',
            data: params
        });
        if (Result == true) {
            $('#TxtNameGroup').removeClass('is-invalid');
            $('#TxtNameGroup-error').html('');
        } else {
            $('#TxtNameGroup').addClass('is-invalid');
            $('#TxtNameGroup-error').html(Result);
        }
    }
}
/*TIPOS DE ESCALAMIENTOS*/
async function VerifyNameTipoEscalamiento(value) {
    value = value.trim();
    if (value != "") { 
        var Validar = $('#DdlTipoEscalamiento option[value="' + value + '"]').val();
        if (Validar != undefined) {
            toastr.error(value + ' ya se encuentra agregado', 'No puede ser vacío');
            return false;
        } else {
            var IdGroups = $('#HdIdGrupo').val();
            if (IdGroups !="") {
                var params = new Object();
                params['IdGroups'] = IdGroups;
                params["TipoEscalamiento"] = value;
                var Result = await $.ajax({
                    type: "post",
                    url: '/Groups/VerifyNameTipoEscalamiento',
                    data: params
                });
            }
        }
    }
    return Result;
}
async function AddOptionTipoEscalamiento() {
    var TipoEscalamiento = $('#TxtTipoEscalamiento').val();
    if (TipoEscalamiento == "") {
        toastr.error('Escriba una tipo escalamiento para continuar.', 'No puede ser vacío');
        return;
    }
    var veriError = await VerifyNameTipoEscalamiento(TipoEscalamiento);
    if (veriError == false) {
        return;
    }
    var valiError = $('#TxtTipoEscalamiento')[0].classList.contains('error');
    if (valiError == true) {
        toastr.error('Verifique los errores para continuar.', 'Verificar');
        $('#TxtTipoEscalamiento').focus();
    } else {
        $('#DdlTipoEscalamiento').append('<option class="newOption" value="' + TipoEscalamiento + '">' + TipoEscalamiento + '</option>');
        $('#TxtTipoEscalamiento').val('');
        toastr.success('<b>' + TipoEscalamiento + '</b> fue agregado correctamente no olvide guardar los cambios.', 'Bien hecho');
    }
}
async function DeleteTipoEscalamiento() {
    try {
        var TextTipoEscalamiento = $('#DdlTipoEscalamiento option:selected').html();
        var TipoEscalamiento = $('#DdlTipoEscalamiento').val();
        var $element = $("#DdlTipoEscalamiento option[value='" + TipoEscalamiento + "']");
        if (TipoEscalamiento == "") {
            toastr.error('Seleccione un tipo de escalamiento para continuar.', 'Seleccione');
        } else if ($element[0].classList.contains('newOption')) {
            $("#DdlTipoEscalamiento option[value='" + TipoEscalamiento + "']").remove();
            $('#DdlTipoEscalamiento').val('');
            toastr.success('<b>' + TipoEscalamiento + '</b> fue eliminado correctamente.', 'Eliminado');
        } else {
            swal({
                title: "¿Quieres continuar?",
                text: "¿Deseas eliminar el tipo de escalamiento " + TextTipoEscalamiento + "?",
                type: "warning",
                showCancelButton: true,
                confirmButtonText: "Si, Eliminar!",
                closeOnConfirm: true,
                showLoaderOnConfirm: true
            }, async function () {
                debugger
                var params = new Object();
                params['IdGroups'] = $('#HdIdGrupo').val();
                params["TipoEscalamiento"] = TipoEscalamiento;
                var Result = await $.ajax({
                    type: "post",
                    url: '/Groups/DeleteTipoEscala',
                    data: params
                });
                $("#DdlTipoEscalamiento option[value='" + TipoEscalamiento + "']").remove();
                $('#DdlTipoEscalamiento').val('');
                toastr.success('<b>' + TextTipoEscalamiento + '</b> fue eliminado correctamente.', 'Eliminado');
            });
        }
    } catch (ex) {
        swal("Error interno", "Comunicate con el area de IT para la verificacion de este error..", "error");
    }
}
/*END TIPOS DE ESCALAMIENTOS*/
async function DeleteGroup(IdGroup) {
    swal({
        title: "¿Quieres continuar?",
        text: "¿Deseas eliminar este Grupo?",
        type: "info",
        showCancelButton: true,
        confirmButtonText: "Si, Eliminar!",
        closeOnConfirm: false,
        showLoaderOnConfirm: true
    }, async function () {
        var params = new Object();
        params["IdGroup"] = IdGroup;
        var result = await $.ajax({
            type: "post",
            url: '/Groups/DeleteGroups',
            data: params
        });
        swal("Bien Hecho!", "Grupo Desactivado exitosamente.", "success");
        await ListadoGroups();
    });
}
async function SearchInforGroup(IdGroup) {
    try {
        await limpiarCampos();
        $("#DdlGruposAEscalar option[value=" + IdGroup + "]").hide();
        $('#myModal').modal('show');
        var params = new Object();
        params["IdGroup"] = IdGroup;
        var result = await $.ajax({
            type: "post",
            url: '/Groups/GroupsJson',
            data: params
        });
        $('#HdIdGrupo').val(result.IdMasterGroups);
        $('#TxtNameGroup').val(result.NameGroup);
        $('#DdlLOB').val(result.LOB.Id);
        $('#DdlSitio').val(result.Sitio.IdMasterSites);
        $('#ChkDevolver').prop('checked', result.ReturnUser);
        $('#TxtDescriptionGroup').val(result.DescriptionGroup);
        var estado = (result.State == true ? 1 : 0);
        $('#DdlEstado').val(estado).prop('disabled', result.State);
        var GruposAEscalar = result.GruposAEscalar;
        $.each(GruposAEscalar, function (key, value) {
            $('#DdlGruposAEscalar option[value=' + value.IdMasterGroups + ']').prop("selected", true);
        });
        var TiposEscalamientos = result.TypesScaled;
        var $optionTE = '<option value="">--Seleccionar--</option>';
        $.each(TiposEscalamientos, function (key, value) {
            $optionTE += '<option value="' + value + '">' + value+'</option>';
        });
        $('#DdlTipoEscalamiento').html($optionTE);
        $('#DdlGruposAEscalar').trigger("chosen:updated");
    } catch (ex) {
        swal("Error interno", "Comunicate con el area de IT para la verificacion de este error..", "error");
    }
}
function limpiarCampos() {
    $('#HdIdGrupo').val('');
    $('#TxtNameGroup').val('');
    $('#TxtNameGroup').removeClass('is-invalid');
    $('#TxtNameGroup-error').html('');
    $('#DdlLOB').val('');
    $('#DdlSitio').val('');
    $('#TxtDescriptionGroup').val('');
    $('#DdlEstado').val(1).prop('disabled', true);
    $("#DdlGruposAEscalar option").show().prop("selected", false);
    $('#TxtTipoEscalamiento').val('');
    $('#DdlTipoEscalamiento').html('<option value="">--Seleccionar--</option>');
    $('#DdlGruposAEscalar').trigger("chosen:updated");
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
