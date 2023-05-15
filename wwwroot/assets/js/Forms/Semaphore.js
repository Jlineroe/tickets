PermisosActions('Semaphore')
DataTables()
$('#divLoaderMaster').hide()
async function DataTables() {
    try {
        var result = await GenerarDTFiltros('divTable')
    } catch (ex) {
        alert(ex)
    } finally {
        $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust()
    }
}
(async function load() {
    const $BtnGuardar = document.getElementById('BtnGuardar')
    $BtnGuardar.addEventListener('click', async (event) => {
        $('.content-wrapper .form-control').removeAttr('required')
        try {
            var $elementRequired = $('.required')
            var Valida = ValidarVacios($elementRequired)
            debugger
            if (Valida == false) {

            } else if (Valida == "error") {
                event.preventDefault()
            } else {
                event.preventDefault()
                FuncBtnLoader($BtnGuardar, 1)
                var params = new Object()
                params['IdSemaphore'] = $('#HdIdSemaphore').val()
                params['SubCategory'] = {
                    IdCategory: $('#HdIdSubCategory').val()
                }
                if (params.SubCategory.IdCategory == "0" | params.SubCategory.IdCategory == "") {
                    params.SubCategory.IdCategory = null
                    params['Sitio'] = {
                        IdMasterSites: $('#DdlSitio').val()
                    }
                    params['SLA_HOUR'] = $('#TxtHorasSLA').val()
                }
                params['GreenTo'] = $('#TxtVerdeHasta').val()
                params['OrangeTo'] = $('#TxtNaranjaHasta').val()
                var Result = await $.ajax({
                    type: "post",
                    url: '/Semaphore/SaveSemaphore',
                    data: params,
                })
                swal("Bien Hecho!", "Accion realizada correctamente.", "success")
                await ListadoSemaforos()
                $('#myModal').modal('hide')
            }
        } catch (ex) {
            swal("Error interno", "Comunicate con el area de IT para la verificacion de este error..", "error")
        } finally {
            setTimeout(function () {
                FuncBtnLoader($BtnGuardar, 0)
            }, 500)
        }
    })
})()
async function VerifySite(value) {
    $('#DdlSitio').removeClass('is-invalid');
    $('#DdlSitio-error').html('');
    if (value != "") {
        var params = new Object();
        params['Sitio'] = {
            IdMasterSites: $('#DdlSitio').val()
        }
        var result = await $.ajax({
            type: "post",
            url: '/Semaphore/VerifySiteSemaphore',
            data: params
        })
        if (result == true) {
            $('#DdlSitio').addClass('is-invalid');
            $('#DdlSitio-error').html('Ya se ha creado un semaforo para este sitio, favor verificar.');
        }
    }
}
function FuncBtnLoader($BtnGuardar, Ocultar) {
    if (Ocultar == 1) {
        $BtnGuardar.style.display = 'none'
        $('.BtnLoading').show()
    } else {
        $BtnGuardar.style.display = ''
        $('.BtnLoading').hide()
    }
}
function limpiarCampos() {
    $('#HdIdSemaphore').val('')
    $('#HdIdSubCategory').val('')
    $('.form-control').val('')
    $('#DdlSitio').prop('disabled', false)
    $('#TxtHorasSLA').prop('disabled', false)

    $('#DdlSitio').removeClass('is-invalid');
    $('#DdlSitio-error').html('');
}
async function CrearSemaforo() {
    await limpiarCampos()
    $('#DdlSitio').prop('disabled', false)
    $('#TxtHorasSLA').prop('disabled', false)
    $('#formSemaforo .divCategories').hide()
    $('#myModal').modal('show')
}
async function SearchInforSemaphore(IdSemaphore,IdSubCategory) {
    try {
        await limpiarCampos()
        $('#myModal').modal('show')
        var params = new Object()
        if (IdSemaphore != 0) {
            params["IdSemaphore"] = IdSemaphore
            $('#formSemaforo .divCategories').hide()
        } else {
            $('#formSemaforo .divCategories').show()
            params["IdSubCategory"] = IdSubCategory
        }
        var result = await $.ajax({
            type: "post",
            url: '/Semaphore/ListSemaphoreJson',
            data: params
        })
        if (result.length > 0) {
            result = result[0]
            $('#HdIdSemaphore').val(result.IdSemaphore)
            let swi = true
            if (result.SubCategory.IdCategory == 0) {
                swi = false
                $('#formSemaforo .divCategories').hide()
            } else {
                $('#formSemaforo .divCategories').show()
            }
            if (result.GreenTo != 0) {
                $('#TxtNaranjaDesde').val(result.GreenTo + 1)
                $('#TxtRojoMayorA').val(result.OrangeTo)
            } else {
                result.GreenTo = "";
                result.OrangeTo = "";
            }
            $('#DdlSitio').val(result.Sitio.IdMasterSites).prop('disabled', swi)
            $('#HdIdSubCategory').val(result.SubCategory.IdCategory)
            $('#TxtCategory').val(result.Category.NameCategory)
            $('#TxtSubCategory').val(result.SubCategory.NameCategory)

            $('#TxtHorasSLA').val(result.SLA_HOUR).prop('disabled', swi)
            $('#TxtVerdeHasta').val(result.GreenTo)
            $('#TxtNaranjaHasta').val(result.OrangeTo)
        }
    } catch (ex) {
        swal("Error interno", "Comunicate con el area de IT para la verificacion de este error..", "error")
    } finally {
    }
}
async function changeInputSLASemaforo($input) {
    const SLA = $('#TxtHorasSLA').val()
    if ($input.id == "TxtHorasSLA")
    {
        $input.classList.remove('is-invalid')
        $('#' + $input.id + '-error').text('')
        $('#TxtVerdeHasta').val("")
        $('#TxtNaranjaDesde').val("")
        $('#TxtNaranjaHasta').val("")
        $('#TxtRojoMayorA').val("")
        if ($input.value != "") {
            if (parseInt($input.value) < 2) {
                $('#' + $input.id + '-error').text('Las horas de SLA deben ser mayor a 1')
                $input.classList.add('is-invalid')
            }
            const TerceraSLA = Math.round(SLA / 3)
            $('#TxtVerdeHasta').val(TerceraSLA)
            $('#TxtNaranjaDesde').val(TerceraSLA + 1)
            $('#TxtNaranjaHasta').val(TerceraSLA + TerceraSLA)
            $('#TxtRojoMayorA').val(TerceraSLA + TerceraSLA)
        } 
    }
    else if ($input.id == "TxtVerdeHasta")
    {
        $input.classList.remove('is-invalid')
        $('#' + $input.id + '-error').text("")
        $('#TxtNaranjaDesde').val("")
        $('#TxtNaranjaHasta').val("")
        $('#TxtRojoMayorA').val("")
        if ($input.value != "") {
            if (parseInt($input.value) >= SLA) {
                $input.classList.add('is-invalid')
                $('#' + $input.id + '-error').text("No puede ser mayor o igual a las Horas SLA")
            }
            $('#TxtNaranjaDesde').val(parseInt($input.value) + 1)
        }
    }
    else if ($input.id == "TxtNaranjaHasta")
    {
        $input.classList.remove('is-invalid')
        $('#' + $input.id + '-error').text("")
        $('#TxtRojoMayorA').val("")
        if ($input.value != "") {
            if (parseInt($input.value) > SLA) {
                $input.classList.add('is-invalid')
                $('#' + $input.id + '-error').text("No puede ser mayor a las Horas SLA")
            }
            $('#TxtRojoMayorA').val(parseInt($input.value))
        }
    }
}
async function ListadoSemaforos() {
    var Result = await $.ajax({
        contentType: 'application/json; charset=utf-8',
        type: "post",
        url: '/Semaphore/ListSemaphore',
    })
    $("#divTable").html(Result)
    await DataTables()
}
async function ActivarDesactivarSemaphore(IdSemaphore, Activar) {
    let params = new Object()
    params["IdSemaphore"] = IdSemaphore
    params["Activar"] = Activar
    let result = await $.ajax({
        type: "post",
        url: '/Semaphore/DisabledEnabledSemaphore',
        data: params
    })
    swal("Bien Hecho!", "Accion realizada correctamente.", "success")
    await ListadoSemaforos()
}