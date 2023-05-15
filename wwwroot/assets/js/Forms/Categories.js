PermisosActions('Categories');
DataTables();
$('#divLoaderMaster').hide();
$('.chosen-select').chosen()
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
                var params = new Object();
                params['IdCategory'] = $('#HdIdCategory').val();
                params['NameCategory'] = $('#TxtNameCategory').val();
                params['Grupo'] = {
                    IdMasterGroups: $('#DdlGroups').val()
                };
                params['Sitio'] = {
                    IdMasterSites: $('#DdlSitio').val()
                };
                params['Template'] = {
                    IdTemplates: $('#DdlPlantilla').val()
                }
                params['DescriptionCategory'] = $('#TxtDescripcionCategory').val();
                var estado = $('#DdlEstado').val();
                params['State'] = (estado == 1 ? true : false);
                var $optionSubCategorias = $(".newOption");
                var SubCategorias = [];
                for (var i = 0; i < $optionSubCategorias.length; i++) {
                    SubCategorias.push({
                        NameCategory: $optionSubCategorias[i].value,
                        SLA_HOUR: $optionSubCategorias[i].dataset.sla
                    });
                }
                params['SubCategory'] = SubCategorias;
                var Result = await $.ajax({
                    type: "post",
                    url: '/Categories/SaveCategories',
                    data: params,
                });
                swal("Bien Hecho!", "Accion realizada correctamente.", "success");
                await ListadoCategories();
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
function limpiarCampos() {
    $('#HdIdCategory').val('');
    $('.form-control').val('');
    $('#TxtNameCategory').removeClass('is-invalid');
    $('#TxtNameCategory-error').html('');
    $('#DdlSubCategoria').html('<option value="">--Seleccionar--</option>');
    $('#DdlEstado').val(1).prop('disabled', true);
    $('.HorasSLA').hide();
}
async function VerifyNameCategory(value) {
    if (value != "") {
        var Validar = $('#DdlSubCategoria option[value="' + value + '"]').val();
        if (Validar != undefined) {
            $('#TxtNameCategory').addClass('is-invalid');
            $('#TxtNameCategory-error').html(value + " ya se encuentra agregado a las sub categorias de este grupo.");
        } else {
            var params = new Object();
            params["NameCategory"] = value;
            var Result = await $.ajax({
                type: "post",
                url: '/Categories/VerifyNameCategory',
                data: params
            });
            if (Result == true) {
                $('#TxtNameCategory').removeClass('is-invalid');
                $('#TxtNameCategory-error').html('');
            } else {
                $('#TxtNameCategory').addClass('is-invalid');
                $('#TxtNameCategory-error').html(Result);
            }
        }
    }
}
//async function VerifyNameSubCategoria(value) {
//    if (value != "") {
//        debugger
//        var Validar = $('#DdlSubCategoria option:selected');
//        if (Validar != undefined) {
//            $('#TxtSubCategoria').addClass('is-invalid');
//            $('#TxtSubCategoria-error').html(value + " ya se encuentra agregado a este grupo.");
//        } 
//    }
//}
function AddOptionSubCategoria() {
    var valiError = $('#TxtSubCategoria')[0].classList.contains('is-invalid');
    var SubCategoria = $('#TxtSubCategoria').val();
    var HorasSLA = $('#TxtHorasSLA').val();
    if (SubCategoria == "") {
        toastr.error('Escriba una sub categoria para continuar.', 'No puede ser vacío');
        $('#TxtSubCategoria').focus();
    } else if (HorasSLA == "") {
        toastr.error('Digite las horas de SLA de esta sub categoria.', 'No puede ser vacío');
        $('#TxtHorasSLA').focus();
    } else if (valiError == true) {
        toastr.error('Verifique los errores para continuar.', 'Verificar');
        $('#TxtSubCategoria').focus();
    } else {
        $('#DdlSubCategoria').append(`<option class="newOption" data-sla="${HorasSLA}" value="${SubCategoria}">
            ${SubCategoria} [${HorasSLA}]</option>`);
        $('#TxtSubCategoria').val('');
        $('#TxtHorasSLA').val('');
        toastr.success(`<b>${SubCategoria}</b> fue agregado correctamente no olvide guardar los cambios.`, 'Bien hecho');
    }
}
function changeSubCategory() {
    var SubCategory = $('#DdlSubCategoria').val();
    if (SubCategory != "") {
        $('.HorasSLA').show();
        var SLA = $('#DdlSubCategoria option[value="' + SubCategory + '"]')[0].dataset.sla;
        $('#TxtEditHorasSLA').val(SLA);
    } else {
        $('.HorasSLA').hide();
    }
}
async function ListTemplate(IdSites) {
    try {
        let $optionTempl = '<option value="">--Seleccionar--</option>'
        if (IdSites != "") {
            let params = new Object();
            params['Sitio'] = { IdMasterSites: IdSites };
            //Templates
            let ListTemplates = await $.ajax({
                type: "post",
                url: '/CreateWorkOrder/GetListTemplates',
                data: params
            })
            for (let data of ListTemplates) {
                $optionTempl += '<option value="' + data.IdTemplates + '">' + data.NameTemplate + '</option>';
            }
        }
        $('#DdlPlantilla').html($optionTempl).trigger('chosen:updated')
    } catch (ex) {
        swal("Error interno", "Comunicate con el area de IT para la verificacion de este error..", "error");
    }
}
async function SearchInforCategory(IdCategory) {
    try {
        await limpiarCampos();
        $('#myModal').modal('show');
        var params = new Object();
        params["IdCategory"] = IdCategory;
        var result = await $.ajax({
            type: "post",
            url: '/Categories/CategoriesJson',
            data: params
        });
        $('#HdIdCategory').val(result.IdCategory);
        $('#TxtNameCategory').val(result.NameCategory);
        $('#TxtDescripcionCategory').val(result.DescriptionCategory);
        $('#DdlGroups').val(result.Grupo.IdMasterGroups);
        $('#DdlSitio').val(result.Sitio.IdMasterSites);
        await ListTemplate(result.Sitio.IdMasterSites)
        var estado = (result.State == true ? 1 : 0);
        $('#DdlEstado').val(estado).prop('disabled', result.State);
        var SubCategorias = result.SubCategory;
        var $option = '<option value="">--Seleccionar--</option>';
        $.each(SubCategorias, function (key, value) {
            $option += `<option data-sla="${value.SLA_HOUR}" value="${value.IdCategory}">
            ${value.NameCategory} [${value.SLA_HOUR}]</option>`; 
        });
        $('#DdlSubCategoria').html($option);
    } catch (ex) {
        swal("Error interno", "Comunicate con el area de IT para la verificacion de este error..", "error");
    }
}
async function DeleteCategory(IdCategory) {
    swal({
        title: "¿Quieres continuar?",
        text: "¿Deseas eliminar esta categoria?",
        type: "warning",
        showCancelButton: true,
        confirmButtonText: "Si, Eliminar!",
        closeOnConfirm: false,
        showLoaderOnConfirm: true
    }, async function () {
        var params = new Object();
        params['IdCategory'] = IdCategory;
        var Result = await $.ajax({
            type: "post",
            url: '/Categories/DeleteCategory',
            data: params
        });
        swal("Bien Hecho!", "Categoria Desactivada exitosamente.", "success");
        await ListadoCategories();
    });
}
async function DeleteSubCategoria() {
    try {
        var TextSubCategoria = $('#DdlSubCategoria option:selected').html();
        var SubCategoria = $('#DdlSubCategoria').val();
        var $element = $("#DdlSubCategoria option[value='" + SubCategoria + "']");
        if (SubCategoria == "") {
            toastr.error('Seleccione una sub categoria para continuar.', 'Seleccione');
        } else if ($element[0].classList.contains('newOption')) {
            $("#DdlSubCategoria option[value='" + SubCategoria + "']").remove();
            $('#DdlSubCategoria').val('').change();
            $('#TxtEditHorasSLA').val('');
            toastr.success('<b>' + SubCategoria + '</b> fue eliminado correctamente.', 'Eliminado');
        } else {
            swal({
                title: "¿Quieres continuar?",
                text: "¿Deseas eliminar esta sub categoria?",
                type: "warning",
                showCancelButton: true,
                confirmButtonText: "Si, Eliminar!",
                closeOnConfirm: true,
                showLoaderOnConfirm: true
            }, async function () {
                var params = new Object();
                params['IdCategory'] = SubCategoria;
                var Result = await $.ajax({
                    type: "post",
                    url: '/Categories/DeleteCategory',
                    data: params
                });
                $("#DdlSubCategoria option[value='" + SubCategoria + "']").remove();
                $('#DdlSubCategoria').val('').change();
                $('#TxtEditHorasSLA').val('');
                    toastr.success('<b>' + TextSubCategoria + '</b> fue eliminado correctamente.', 'Eliminado');
            });
        }
    } catch (ex) {
        swal("Error interno", "Comunicate con el area de IT para la verificacion de este error..", "error");
    }
}
async function UpdateSLASubCategoria() {
    var SLA = $('#TxtEditHorasSLA').val();
    var SubCategory = $('#DdlSubCategoria').val();
    if (SLA == "") {
        toastr.error('Digite las horas del SLA para continuar.', 'SLA obligatorio');
    } else {
        var $element = $("#DdlSubCategoria option[value='" + SubCategory + "']");
        var SLAAnterior = "["+$element[0].dataset.sla+"]";
        if ($element[0].classList.contains('newOption')) {
            $element[0].dataset.sla = SLA;
            $element.html(SLA + ' - ' + SubCategory);
            $('#DdlSubCategoria').val('').change();
            toastr.success("SLA actualizado correctamente, recuerde guardar lo cambios.", 'Bien hecho');
        } else {
            swal({
                title: "¿Quieres continuar?",
                text: "¿Deseas actualizar el SLA de esta sub categoria?",
                type: "warning",
                showCancelButton: true,
                confirmButtonText: "Si, continuar!",
                closeOnConfirm: true,
                showLoaderOnConfirm: true
            }, async function () {
                var params = new Object();
                params['IdCategory'] = SubCategory;
                params['SLA_HOUR'] = SLA;
                var Result = await $.ajax({
                    type: "post",
                    url: '/Categories/UpdateSLASubCategory',
                    data: params,
                });
                $element[0].dataset.sla = SLA;
                var textSubCategoria = $('#DdlSubCategoria option:selected').text();
                    $element.html(`${textSubCategoria.replace(SLAAnterior, "").trim()} [${SLA}]`);
                $('#DdlSubCategoria').val('').change();
                toastr.success("SLA actualizado correctamente.", 'Bien hecho');
            });
        }
    }
}
async function ListadoCategories() {
    var Result = await $.ajax({
        contentType: 'application/json; charset=utf-8',
        type: "post",
        url: '/Categories/ListCategories',
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