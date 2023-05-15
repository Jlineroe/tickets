PermisosActions('Profiles');
DataTables();
funFuncionamientoTreView();
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
(async function load() {
    const $BtnGuardar = document.getElementById('BtnGuardar');
    $BtnGuardar.addEventListener('click', async (event) => {
        $('.container-fluid .form-control').removeAttr('required');
        try {
            var $elementRequired = $('.required');
            var Valida = ValidarVacios($elementRequired);
            if (Valida == false) {

            } else if (Valida == "error") {
                event.preventDefault();
            } else {
                event.preventDefault();
                FuncBtnLoader($BtnGuardar, 1);
                
                var ListSitios = $('#DdlSitios').chosen().val();
                if (ListSitios.length == 0) {
                    toastr.error('Escoja como minimo un sitio para continuar.', 'Seleccione un sitio');
                    return;
                }
                var objectSitios = [];
                for (var i = 0; i < ListSitios.length; i++) {
                    objectSitios.push({
                        IdMasterSites: ListSitios[i]
                    });
                }
                var $TreeElement = $('#treeview input:checkbox:checked');
                var Menu = [];
                for (var i = 0; i < $TreeElement.length; i++) {
                    var value = $TreeElement[i].id.replace('ChkM_', '').replace('ChkA_', '');
                    Menu.push({
                        IdMasterMenu: value,
                        Level: ($TreeElement[i].id.includes('ChkM_') ? 1 :0)
                    });
                }
                var IdProfile = $('#HdIdProfile').val();
                var params = new Object();
                if (IdProfile.toString() != "")
                    params['IdMasterProfiles'] = IdProfile;
                params['NameProfile'] = $('#TxtNameProfile').val();
                params['DescriptionProfile'] = $('#TxtDescriptionProfile').val();
                var estado = $('#DdlEstado').val();
                params['State'] = (estado == 1 ? true : false);
                params['Sitios'] = objectSitios;
                params['Menu'] = Menu;
                var Result = await $.ajax({
                    type: "post",
                    url: "/Profiles/" + (IdProfile.toString() != "" ? "UpdateProfile" :"SaveProfile"),
                    data: params,
                });
                limpiarCampos();
                swal("Bien Hecho!", "Accion realizada correctamente.", "success");
                await ListadoPerfiles();
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
async function ListadoPerfiles() {
    var Result = await $.ajax({
        contentType: 'application/json; charset=utf-8',
        type: "post",
        url: '/Profiles/ListProfiles',
    });
    $("#divTable").html(Result);
    await DataTables();
}
function limpiarCampos() {
    $('#DdlSitios').val('').trigger("chosen:updated");
    $('#HdIdProfile').val('');
    $('#TxtNameProfile').val('');
    $('#TxtDescriptionProfile').val('');
    $('#TxtNameProfile').removeClass('is-invalid');
    $('#TxtNameProfile-error').html('');
    $('.chkModulos').prop("checked", false);
}
async function BuscarPerfil(IdProfile) {
    $('#myModal').modal('show');
    limpiarCampos();
    var params = new Object();
    params["IdProfile"] = IdProfile;
    var result = await $.ajax({
        type: "post",
        url: '/Profiles/ListProfilesJson',
        data: params
    });
    $('#HdIdProfile').val(result.IdMasterProfiles);
    $('#TxtNameProfile').val(result.NameProfile);
    $('#TxtDescriptionProfile').val(result.DescriptionProfile);
    var sitios = result.Sitios;
    $.each(sitios, function (key, value) {
        $('#DdlSitios option[value=' + value.IdMasterSites + ']').prop("selected", true);
    });
    for (var i = 0; i < result.Menu.length; i++) {
        if (result.Menu[i].Level == 0) {
            $("#ChkA_" + result.Menu[i].IdMasterMenu).prop("checked", (result.Menu[i].Permiso == 1 ? true : false));
        } else {
            $("#ChkM_" + result.Menu[i].IdMasterMenu).prop("checked", (result.Menu[i].Permiso == 1 ? true : false));
        }
    }
    $('#DdlSitios').trigger("chosen:updated");
    var estado = (result.State == true ? 1 : 0);
    $('#myModal #DdlEstado').val(estado).prop('disabled', result.State);
}
async function DeletePerfil(IdProfile) {
    swal({
        title: "¿Deseas continuar?",
        text: "¿Deseas eliminar este perfil?",
        type: "warning",
        showCancelButton: true,
        confirmButtonText: "Si, Eliminar!",
        closeOnConfirm: false,
        showLoaderOnConfirm: true
    }, async function () {
            var params = new Object();
            params["IdMasterProfiles"] = IdProfile;
            var result = await $.ajax({
                type: "post",
                url: '/Profiles/DeleteProfile',
                data: params
            });
            swal("Bien Hecho!", "Perfil Desactivado exitosamente.", "success");
            await ListadoPerfiles();
            DataTables();
    });
}
async function VerifyNameProfile(value) {
    if (value != "") {
        var params = new Object();
        params["NameProfile"] = value;
        var Result = await $.ajax({
            type: "post",
            url: '/Profiles/VerifyNameProfile',
            data: params
        });
        if (Result == true) {
            $('#TxtNameProfile').removeClass('is-invalid');
            $('#TxtNameProfile-error').html('');
        } else {
            $('#TxtNameProfile').addClass('is-invalid');
            $('#TxtNameProfile-error').html(Result);
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
        $('[data-toggle="popover"]').popover();
    }
}
function AppendRow(customerId, name, country) {
    var row = $("#TableGeneral tr:last-child");
    if ($("#TableGeneral tr:last-child span").eq(0).html() != "&nbsp;") {
        row = row.clone();
    }
    //Bind CustomerId.
    $(".CustomerId", row).html(customerId);

    //Bind Name.
    $(".Name", row).html(name);

    //Bind Country.
    $(".Country", row).html(country);

    row.find(".Edit").show();
    row.find(".Delete").show();

    $("#TableGeneral tbody").append(row);
};
function funFuncionamientoTreView() {

    $('.tree > ul').attr('role', 'tree').find('ul').attr('role', 'group');
    $('.tree').find('li:has(ul)').addClass('parent_li').attr('role', 'treeitem').find(' > span').find(' > i').attr('title', 'Collapse this branch').on('click', function (e) {

        var children = $(this).parent().parent('li.parent_li').find(' > ul > li');
        if (children.is(':visible')) {
            children.hide('fast');
            $(this).removeClass().addClass('fa fa-lg fa-plus-circle');
        } else {
            children.show('fast');
            $(this).removeClass().addClass('fa fa-lg fa-minus-circle');
        }
        e.stopPropagation();
    });


    $('.chkOpcion').change(checkboxChildChanged);

    function checkboxChildChanged() {

        //var $this = $(this).parent().parent().parent().parent().find(' > ul > li > label > span > label > input');
        var $this = $(this).parent().parent().parent().parent();
        var $chk = $this.find('span').find('label').find('input');
        $chk.prop({
            indeterminate: false,
            checked: "checked"
        });

    }

    $('.chkModulos').change(checkboxChanged);

    function checkboxChanged() {

        var $this = $(this),
            checked = $this.prop("checked"),
            container = $this.parent().parent().parent(),
            siblings = container.siblings();

        container.find('input[type="checkbox"]')
            .prop({
                indeterminate: false,
                checked: checked
            })
            .siblings('label')
            .removeClass('custom-checked custom-unchecked custom-indeterminate')
            .addClass(checked ? 'custom-checked' : 'custom-unchecked');

        checkSiblings(container, checked);
    }

    function checkSiblings($el, checked) {

        var parent = $el.parent().parent(),
            all = true,
            indeterminate = false;

        $el.siblings().each(function () {
            return all = ($(this).children('input[type="checkbox"]').prop("checked") === checked);
        });

        if (all && checked) {
            parent.children('input[type="checkbox"]')
                .prop({
                    indeterminate: false,
                    checked: checked
                })
                .siblings('label')
                .removeClass('custom-checked custom-unchecked custom-indeterminate')
                .addClass(checked ? 'custom-checked' : 'custom-unchecked');

            checkSiblings(parent, checked);
        }
        else if (all && !checked) {
            indeterminate = parent.find('input[type="checkbox"]:checked').length > 0;
            parent.children('input[type="checkbox"]')
                .prop("checked", checked)
                .prop("indeterminate", indeterminate)
                .siblings('label')
                .removeClass('custom-checked custom-unchecked custom-indeterminate')
                .addClass(indeterminate ? 'custom-indeterminate' : (checked ? 'custom-checked' : 'custom-unchecked'));

            checkSiblings(parent, checked);
        }
        else {
            $el.parents("li").children('input[type="checkbox"]')
                .prop({
                    indeterminate: true,
                    checked: false
                })
                .siblings('label')
                .removeClass('custom-checked custom-unchecked custom-indeterminate')
                .addClass('custom-indeterminate');
        }
    }
}
