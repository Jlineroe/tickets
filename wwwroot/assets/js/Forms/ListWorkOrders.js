PermisosActions('ListWorkOrders');
$('.chosen-select').chosen();
$('.super-chosen').chosen();
$('.input-daterange').datepicker({
    todayBtn: "linked",
    calendarWeeks: true,
    keyboardNavigation: false,
    forceParse: false,
    autoclose: true,
    format: 'yyyy/mm/dd'
});

async function FuncBtnLoader($BtnGuardar, Ocultar) {
    if (Ocultar == 1) {
        $BtnGuardar.style.display = 'none';
        $('.BtnLoading').show();
    } else {
        $BtnGuardar.style.display = '';
        $('.BtnLoading').hide();
    }
}
(async function load() {
    const $BtnReassign = document.getElementById('BtnReassign');
    $BtnReassign.addEventListener('click', async (event) => {
        $('.content-wrapper .form-control').removeAttr('required');
        try {
            var $elementRequired = $('#formReasignar .required');
            var Valida = ValidarVacios($elementRequired);
            let NumTicketsSelect = $('#divNumSelect').text();
            if (Valida == false) {

            } else if (Valida == "error") {
                event.preventDefault();
            } else if (parseInt(NumTicketsSelect) == 0) {
                event.preventDefault();
                toastr.error("Debe seleccionar como minimo un ticket para continuar!", "Seleccionar tickets");
            } else {
                event.preventDefault();
                var Grupo = $('#DdlGroups option:selected').text();
                var Usuarios = $("#DdlUsuarios").chosen().val();
                var msj = "a los usuarios seleccionados";
                if (Usuarios.length == 0) {
                    msj = "a todos los usuarios del grupo " + Grupo;
                }
                var Algoritmo = $('#DdlAlgoritmo option:selected').text();
                swal({
                    title: "¿Estas seguro?",
                    text: " ¿Deseas reasignar " + NumTicketsSelect+" registros " + msj + " con el algoritmo " + Algoritmo + "?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonText: "Si, Cargar!",
                    closeOnConfirm: true,
                    showLoaderOnConfirm: true
                }, async function () {
                    try {
                        FuncBtnLoader($BtnReassign, 1)
                        let ListIdWorkOrder = []
                        let Listcheck = $('#TableGeneral input:checkbox:checked')
                        for (var i = 0; i < Listcheck.length; i++) {
                            ListIdWorkOrder.push(Listcheck[i].dataset.idworkorder);
                        }
                        var arrayUsuarios = [];
                        for (var i = 0; i < Usuarios.length; i++) {
                            arrayUsuarios.push({
                                IdMasterUsers: Usuarios[i]
                            });
                        }
                        var params = new Object();
                        params['Algorithms'] = {
                            IdAlgorithmsAssignment: $('#DdlAlgoritmo').val()
                        }
                        params['Grupo'] = {
                            IdMasterGroups: $('#DdlGroups').val()
                        }
                        params['ListUsers'] = arrayUsuarios
                        params['ListIdWorkOrder'] = ListIdWorkOrder
                        var Result = await $.ajax({
                            type: "post",
                            url: '/ListWorkOrders/WorkOrderReassign',
                            contentType: "application/json; charset=utf-8",
                            data: JSON.stringify(params)
                        })
                        swal("Bien Hecho!", "La reasignacion fue realizada correctamente, favor verificar.", "success");
                        setTimeout(function () {
                            location.reload();
                        }, 1000);
                    } catch (ex) {
                        swal("Error interno", "Comunicate con el area de IT para la verificacion de este error.", "error");
                    } finally {
                        FuncBtnLoader($BtnReassign, 0);
                    }
                });
            }
        } catch (ex) {
            swal("Error interno", "Comunicate con el area de IT para la verificacion de este error.", "error");
        }
    });
})();
$('#divLoaderMaster').hide();
function ClickAllCheck($check) {
    $(".ChkWorkOrder").prop("checked", $check.checked)
    let Listcheck = $('#TableGeneral input:checkbox:checked')
    if ($check.checked == true & Listcheck.length > 0) {
        $('#divNumSelect').text(Listcheck.length - 1)
        $('#divCardReassign').show()
    } else {
        $('#divCardReassign').hide()
    }
}
function ClickCheckUni() {
    let Listcheck = $('#TableGeneral input:checkbox:checked')
    if (Listcheck.length > 0) $('#divCardReassign').show()
    else $('#divCardReassign').hide()
    $('#divNumSelect').text(Listcheck.length)
}
function FuncBtnLoader($BtnGuardar, Ocultar) {
    if (Ocultar == 1) {
        $BtnGuardar.style.display = 'none';
        $('.BtnLoading').show();
    } else {
        $BtnGuardar.style.display = '';
        $('.BtnLoading').hide();
    }
}
function onClickFiltros() {
    var NameBtn = $('#NameBtnFiltros').text();
    if (NameBtn.includes('Mostrar')) {
        $('#divListWorkOrders').removeClass('col-md-12').addClass('col-md-9');
        $('#divMasterFilters').show();
        $('#NameBtnFiltros').text('Ocultar Filtros');
    } else {
        $('#divListWorkOrders').removeClass('col-md-9').addClass('col-md-12');
        $('#divMasterFilters').hide();
        $('#NameBtnFiltros').text('Mostrar Filtros');
    }
}
function cleanFilters() {
    window.location = "/ListWorkOrders";
}
async function onChangeGroups_ListUsers(value) {
    var $option = "";
    if (value != "") {
        var params = new Object();
        params["IdGroups"] = value;
        var Result = await $.ajax({
            type: "post",
            url: '/ListWorkOrders/ListUsersXGroup',
            data: params
        });
        if (Result.length == 0) {
            $('#DdlGroups').addClass('is-invalid');
            $('#DdlGroups-error').html('El grupo seleccionado no cuenta con usuarios con permisos de asignarle tickets, verifique los permisos de estos usuarios.');
        } else {
            $('#DdlGroups').removeClass('is-invalid');
            $('#DdlGroups-error').html('');
            $.each(Result, function (key, value) {
                $option += `<option value="${value.IdMasterUsers}">${value.Nombres.trim()} ${value.PrimerApellido.trim()} ${value.SegundoApellido.trim()}</option>`;
            });
        }
    }
    $('#DdlUsuarios').html($option).trigger("chosen:updated");
}

async function changeMostrarTop(Top) {
    await GetListOrderXPagination()
}
async function clickBtnPaginator(pag, top) {
    $('#hddPagina').val(pag)
    await GetListOrderXPagination()
}
async function clickOrderBy(value) {
    $('#hddOrderBy').val(value)
    $('#DivOrderarPor .OrderBy').removeClass('active')
    $('#DivOrderarPor #divOrder' + value).addClass('active')
    $('#BtnOrderBy').text($('#DivOrderarPor #divOrder' + value).text())
    await GetListOrderXPagination()
}
async function clickOrderType(value) {
    $('#hddTypeOrder').val(value)
    $('#DivOrderarPor .TypeOrder').removeClass('active')
    $('#DivOrderarPor #divTypeOrder' + value).addClass('active')
    await GetListOrderXPagination()
}
async function GetListOrderXPagination() {
    var IdWorkOrder = $('#TxtIdWorkOrder').val()
    var Title = $('#TxtTitle').val()
    var Description = $('#TxtDescription').val()
    var PQR = $('#TxtPQRBog').val();
    var Cuenta = $('#TxtCuentaBog').val();
    var DdlTemplates = $('#DdlTemplates').val();
    var DdlEstados = $('#DdlEstado').chosen().val();
    var DdlGruposAsignados = $('#DdlGruposAsignados').chosen().val();
    var DdlUserCrea = $('#DdlUserCrea').chosen().val();
    var DdlUserAssign = $('#DdlUserAssign').chosen().val();
    var DdlUserScaled = $('#DdlUserScaled').chosen().val();
    var X_COORDINATE = $('#TxtX_COORDINATE').val();
    var Numero = $('#TxtNumero_MIN').val();

    $('#divLoaderMaster').show();
    var params = new Object()
    params["OrderBy"] = $('#hddOrderBy').val()
    params["OrderType"] = $('#hddTypeOrder').val()
    params["pag"] = $('#hddPagina').val()
    params["top"] = $('#DdlTopData').val()
    if (IdWorkOrder != "") params["ListIdWorkOrder"] = IdWorkOrder.split(',')
    params["Title"] = Title
    params["Description"] = Description
    params["IdTemplate"] = DdlTemplates
    params["IdStatus"] = DdlEstados
    params["IdGroupsAssign"] = DdlGruposAsignados
    params["IdUsersCrea"] = DdlUserCrea
    params["IdUsersAssign"] = DdlUserAssign
    params["IdUsersScaled"] = DdlUserScaled
    params["PQR"] = PQR
    params["Cuenta"] = Cuenta
    params["X_COORDINATE"] = X_COORDINATE
    params["Numero"] = Numero
    var TableData = await $.ajax({
        type: "post",
        url: '/ListWorkOrders/ListOrders',
        data: params
    })
    $('#divTable').html(TableData)
    var Pagination = await $.ajax({
        type: "post",
        url: '/ListWorkOrders/Pagination',
        data: params
    })
    $('.divPagination').html(Pagination)
    ClickCheckUni()
    $('#divLoaderMaster').hide();
}

async function clickOrderStatus(IdWorkOrder, IdStatusDefinition, IdMasterGroups) {

    var perfil = $('#txtPerfil').val();

    //Perfil Usuario Agente
    if (perfil == 2)  {

        var params1 = new Object()
        params1["IdWorkOrder"] = IdWorkOrder

        var Result = await $.ajax({
            type: "post",
            url: '/TimeGroup/SelWorkOrderAssigned',
            data: params1
        })

        if (Result.length == 0) {
            Result = Result[0]

            var params2 = new Object()

            params2["IdWorkOrder"] = IdWorkOrder
            params2["IdStatusDefinition"] = IdStatusDefinition
            params2["IdMasterGroups"] = IdMasterGroups

            var Result = await $.ajax({
                type: "post",
                url: '/TimeGroup/SaveWorkOrderAssigned',
                data: params2
            })

            window.location.href = "/WorkOrderSolutions?IdWorkOrder=" + IdWorkOrder

        } else {

            Result = Result[0]
            swal("Error interno", "No puede abrir esta PQR hasta no cerrar la PQR con numero: " + Result.IdWorkOrder, "error");
        }
     
    } else {
        window.location.href = "/WorkOrderSolutions?IdWorkOrder=" + IdWorkOrder
    }

    
}