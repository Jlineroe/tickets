PermisosActions('Users');
DataTables();
$('.chosen-select').chosen();
$('.super-chosen').chosen();
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
async function BtnGuardar() {
    const $BtnGuardar = document.getElementById('BtnNewGuardar');
    try {
        
        event.preventDefault();
        FuncBtnLoader($BtnGuardar, 1);
        //USUARIOS
        var IdUsers = $('#HdIdUsuario').val();
        var Usuarios = $("#DdlUsuarios").chosen().val();
        if (IdUsers == "" & Usuarios.length == 0) {
            toastr.error('Seleccione como minimo un usuario.', 'Seleccione un usuario');
            return;
        }
        var arrayUsuarios = [];
        for (var i = 0; i < Usuarios.length; i++) {
            arrayUsuarios.push({
                IdMasterUsers: Usuarios[i]
            });
        }

        //PERFILES
        var ListPerfiles = $("#DdlPerfiles").chosen().val();
        if (ListPerfiles.length == 0) {
            toastr.error('Seleccione como minimo un perfil.', 'Seleccione un perfil');
            return;
        }
        var arrayPerfiles = [];
        for (var i = 0; i < ListPerfiles.length; i++) {
            arrayPerfiles.push({
                IdMasterProfiles: ListPerfiles[i]
            });
        }
        //GRUPOS
        var ListGrupos = $("#DdlGrupos").chosen().val();
        if (ListGrupos.length == 0) {
            toastr.error('Seleccione como minimo un grupo.', 'Seleccione un grupo');
            return;
        }
        var arrayGrupos = [];
        for (var i = 0; i < ListGrupos.length; i++) {
            arrayGrupos.push({
                IdMasterGroups: ListGrupos[i]
            });
        }
        //INFOR PERFILES Y GRUPOS
        var estado = $('#DdlEstado').val();
        var arrayUpdUsers = {
            IdMasterUsers: IdUsers,
            State: (estado == 1 ? true : false),
            Grupos: arrayGrupos,
            Perfiles: arrayPerfiles
        }
        var params = new Object();
        params['Usuarios'] = arrayUsuarios;
        params['UpdUsers'] = arrayUpdUsers;
        var Result = await $.ajax({
            type: "post",
            url: '/Users/SaveUsers',
            data: params,
        });
        //await ListadoUsers();
        swal("Bien Hecho!", "Accion realizada correctamente.", "success");
        $('#myModal').modal('hide');
        setTimeout(function () {
            location.reload();
        }, 1000);
    } catch (ex) {
        swal("Error interno", "Comunicate con el area de IT para la verificacion de este error..", "error");
    } finally {
        setTimeout(function () {
            FuncBtnLoader($BtnGuardar, 0);
        }, 500);
    }
}
async function SearchInforUser(IdUser) {
    limpiarCampos();
    $('#divUsers').hide();
    $('#divDetalle').show();
    $('#myModal').modal('show');
    var params = new Object();
    params["IdUsers"] = IdUser;
    var result = await $.ajax({
        type: "post",
        url: '/Users/ListUsersJson',
        data: params
    });
    $('#HdIdUsuario').val(result.IdMasterUsers);
    var Perfiles = result.Perfiles;
    $.each(Perfiles, function (key, value) {
        $('#DdlPerfiles option[value=' + value.IdMasterProfiles + ']').prop("selected", true);
    });
    var Grupos = result.Grupos;
    $.each(Grupos, function (key, value) {
        $('#DdlGrupos option[value=' + value.IdMasterGroups + ']').prop("selected", true);
    });
    var estado = (result.State == true ? 1 : 0);
    $('#DdlEstado').val(estado).prop('disabled', result.State);
    //----------------------------------------------
    $('#TxtIdentificacion').val(result.Identificacion);
    $('#TxtNombre').val(result.Nombres + ' ' + result.PrimerApellido + ' ' + result.SegundoApellido);
    $('#TxtCentroCosto').val(result.CentroCosto);
    $('#TxtWinser').val(result.Winuser);
    $('#TxtEmail').val(result.EmailCorporativo);
    $('.chosen-select').trigger("chosen:updated");
}
function limpiarCampos() {
    $('#divUsers').show();
    $('#divDetalle').hide();
    $('#HdIdUsuario').val('');
    $("#DdlPerfiles option").prop("selected", false);
    $("#DdlGrupos option").prop("selected", false);
    $('#DdlEstado').val(1).prop('disabled', true);
    $('.chosen-select').trigger("chosen:updated");
}
async function DataTables() {
    try {
        var result = await GenerarDTFiltros('divTable');
    } catch (ex) {
        alert(ex);
    } finally {
        $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
        $('[data-toggle="popover"]').popover();
    }
}
//async function ListadoUsers() {
//    var Result = await $.ajax({
//        contentType: 'application/json; charset=utf-8',
//        type: "post",
//        url: '/Users/ListUsers'
//    });
//    $("#divTable").html(Result);
//    await DataTables();
//}
async function DeleteUser(IdUsuario) {
    swal({
        title: "¿Deseas continuar ?",
        text: "¿Deseas eliminar este usuario?",
        type: "warning",
        showCancelButton: true,
        confirmButtonText: "Si, Eliminar!",
        closeOnConfirm: false,
        showLoaderOnConfirm: true
    }, async function () {
        var params = new Object();
        params["IdMasterUser"] = IdUsuario;
        var result = await $.ajax({
            type: "post",
            url: '/Users/DeleteUsuarios',
            data: params
        });
        swal("Bien Hecho!", "Usuario Desactivado exitosamente.", "success");
        setTimeout(function () {
            location.reload();
        }, 1000);
    });
}