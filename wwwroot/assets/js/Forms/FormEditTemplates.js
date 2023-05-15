//Elimina de la cache el listado de campos
localStorage.removeItem("ListFields");
$('.datepicker').datepicker({
    todayBtn: "linked",
    calendarWeeks: true,
    format: 'yyyy/mm/dd'
});
//Cambiar los campos de posicion
$("#sortable").sortable();
$("#sortableSolutions").sortable();
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
async function SetGuardarNuevoCampo($Btn)
{
    try
    {
        ToolsFuncBtnLoader($Btn, 1)
        var $elementRequired = $('#myModal .required')
        var Valida = NewValidarVacios($elementRequired)
        var longitud = $('#TxtLongitudCampo').val()
        if (Valida == false)
        {
            toastr.error('Los campos no pueden estar vacios o con errores.', 'Campos obligatorios')
            return
        } else if (parseInt(longitud) > 5000) {
            toastr.error('Longitud no puede ser mayor a 5000.', 'Longitud invalida')
            return
        } else {
            var IdFieldsUDF = $('#HdIdInput').val()
            await AccionesGuardarModal(IdFieldsUDF)
            $('#myModal').modal('hide')
            $('.datepicker').datepicker({
                format: 'yyyy/mm/dd'
            })
        }
    } catch (ex) {
        swal("Error interno", "Comunicate con el area de IT para la verificacion de este error..", "error");
    } finally {
        setTimeout(function () {
            ToolsFuncBtnLoader($Btn, 0);
        }, 500);
    }
}

async function AccionesGuardarModal(IdFieldsUDF) {
    if (IdFieldsUDF != "") {
        var params = new Object();
        params['IdFieldsUDF'] = IdFieldsUDF;
        params['NameField'] = $('#TxtNombreCampo').val();
        params['FieldType'] = {
            IdFieldsTypesUDF: $('#DdlTipoCampo').val()
        };
        params['TypeRequired'] = {
            IdTypeRequired: $('#DdlTipoObligatorio').val()
        };
        params['ParentDispositions'] = {
            IdFieldsUDF: $('#DdlListDependent').val(),
            Parent_IdDispositions: $('#DdlItemDependent').val()
        };
        var $optionItems = $(".newOption");
        var Dispositi = [];
        for (var i = 0; i < $optionItems.length; i++) {
            Dispositi.push({
                IdFieldsUDF: IdFieldsUDF,
                Dispositions: $optionItems[i].value,
                Parent_IdDispositions: $('#DdlItemDependent').val()
            });
        }
        params['Dispositions'] = Dispositi;
        var Result = await $.ajax({
            type: "post",
            url: '/Templates/UpdateFieldsUDF',
            data: params
        });
        $('#lbl_' + IdFieldsUDF).text(params.NameField);
        swal("Bien Hecho!", "Accion realizada correctamente.", "success");
    }
    else {
        debugger
        var params = new Object();
        params['Template'] = {
            IdTemplates: $('#HdIdPlantilla').val()
        };
        params['FieldType'] = {
            IdFieldsTypesUDF: $('#DdlTipoCampo').val()
        };
        params['TypeRequired'] = {
            IdTypeRequired: $('#DdlTipoObligatorio').val()
        };
        params['Longitud'] = $('#TxtLongitudCampo').val();
        params['NameField'] = $('#TxtNombreCampo').val();
        params['ParentDispositions'] = {
            IdFieldsUDF: $('#DdlListDependent').val(),
            Parent_IdDispositions: $('#DdlItemDependent').val()
        };
        var $optionItems = $(".newOption");
        var Dispositi = [];
        for (var i = 0; i < $optionItems.length; i++) {
            Dispositi.push({
                IdFieldsUDF: IdFieldsUDF,
                Dispositions: $optionItems[i].value,
                Parent_IdDispositions: $('#DdlItemDependent').val()
            });
        }
        params['Dispositions'] = Dispositi;
        params['SolutionField'] = $('#HdSolutionField').val();
        params['Position'] = 0;
        if (params.Template.IdTemplates != "0") {
            swal({
                title: "¿Deseas agregar este campo a la plantilla?",
                text: "Esta accion se realizara inmediatamente.",
                type: "warning",
                showCancelButton: true,
                confirmButtonText: "Si, continuar!",
                closeOnConfirm: true,
                showLoaderOnConfirm: true
            },
                async function ()
                {
                    var Result = await $.ajax({
                        type: "post",
                        url: '/Templates/SaveFieldsUDF',
                        data: params
                    });
                    toastr.success('Creado correctamente.', 'Campo Creado');
                    if (params.SolutionField == "false") {
                        $('#sortable').append(Result);
                    } else {
                        $('#sortableSolutions').append(Result);
                    }
            });
        }
        else
        {
            var cacheListFields = JSON.parse(localStorage.getItem("ListFields"));
            if (cacheListFields == null) {
                cacheListFields = [];
            }
            //params['IdDivHTML'] = cacheListFields.length;
            params['IdFieldsUDF'] = cacheListFields.length;
            cacheListFields.push(params);
            localStorage.setItem("ListFields", JSON.stringify(cacheListFields));
            let $divInput = await $.ajax({
                type: "post",
                url: '/Templates/ViewTypeFieldLocal',
                data: params
            });
            if (params.SolutionField == "false") {
                $('#sortable').append($divInput);
            } else {
                $('#sortableSolutions').append($divInput);
            }
            toastr.success('Nuevo campo agregado, recuerde guardar los cambios.', 'Bien hecho');
            //await AddHTMLInputTemplate(params);
        }
    }
}
(async function load() {
    //const $BtnGuardarCampo = document.getElementById('BtnGuardar');
    //$BtnGuardarCampo.addEventListener('click', async (event) => {
    //    $('.content-wrapper .form-control').removeAttr('required');
    //    try {
    //        var $elementRequired = $('#myModal .required');
    //        var Valida = ValidarVacios($elementRequired);
    //        var longitud = $('#TxtLongitudCampo').val();
            
    //        if (Valida == false) {

    //        } else if (parseInt(longitud) > 5000) {
    //            event.preventDefault();
    //            toastr.error('Longitud no puede ser mayor a 5000.', 'Longitud invalida');
    //        } else {
    //            event.preventDefault();
    //            FuncBtnLoader($BtnGuardarCampo, 1);
    //            debugger
    //            var IdFieldsUDF = $('#HdIdInput').val();
    //            await AccionesGuardarModal(IdFieldsUDF);
    //            $('#myModal').modal('hide');
    //            $('.datepicker').datepicker({
    //                format: 'yyyy/mm/dd'
    //            });
    //        }
    //    } catch (ex) {
    //        swal("Error interno", "Comunicate con el area de IT para la verificacion de este error..", "error");
    //    } finally {
    //        setTimeout(function () {
    //            FuncBtnLoader($BtnGuardarCampo, 0);
    //        }, 500);
    //    }
    //});
    const $BtnGuardarTemplate = document.getElementById('BtnGuardarTemplate');
    if ($BtnGuardarTemplate != undefined) {
        $BtnGuardarTemplate.addEventListener('click', async (event) => {
            $('.content-wrapper .form-control').removeAttr('required');
            try {
                var $elementRequired = $('#formTemplate .required');
                var Valida = ValidarVacios($elementRequired);
                if (Valida == false) {

                } else if (Valida == "error") {
                    event.preventDefault();
                } else {
                    event.preventDefault();
                    FuncBtnLoader($BtnGuardarTemplate, 1);
                    var params = new Object();
                    params['IdTemplates'] = $('#HdIdPlantilla').val();
                    params['NameTemplate'] = $('#TxtNameTemplate').val();
                    params['DescriptionTemplate'] = $('#TxtDescriptionTemplate').val();
                    params['Sitio'] = {
                        IdMasterSites: $('#DdlSitio').val()
                    };
                    var cacheListFields = JSON.parse(localStorage.getItem("ListFields"));
                    if (cacheListFields == null) {
                        cacheListFields = [];
                    }
                    var orden = $('.numDivFields');
                    var ListFieldsUDF = [];
                    for (var i = 0; i < cacheListFields.length; i++) {
                        if (cacheListFields[i] != null) {
                            //var IdHTML = orden[i].id.replace("divHTMLInput", "");
                            var IdHTML = orden[i].dataset.idfieldsudf
                            if (IdHTML == cacheListFields[i].IdFieldsUDF) {
                                cacheListFields[i].Position =i;
                            }
                            ListFieldsUDF.push(cacheListFields[i]);
                        }
                    }
                    
                    params['ListFieldsUDF'] = ListFieldsUDF;
                    var Result = await $.ajax({
                        type: "post",
                        url: '/Templates/SaveTemplate',
                        data: params
                    });
                    swal("Bien Hecho!", "Accion realizada correctamente.", "success");
                    setTimeout(function () {
                        window.location = "/Templates";
                    }, 1000);
                }
            } catch (ex) {
                swal("Error interno", "Comunicate con el area de IT para la verificacion de este error..", "error");
            } finally {
                setTimeout(function () {
                    FuncBtnLoader($BtnGuardarTemplate, 0);
                }, 500);
            }
        });
    }
    const $BtnUpdateTemplate = document.getElementById('BtnUpdateTemplate');
    if ($BtnUpdateTemplate != undefined) {
        $BtnUpdateTemplate.addEventListener('click', async (event) => {
            $('.content-wrapper .form-control').removeAttr('required');
            try {
                var $elementRequired = $('#formTemplate .required');
                var Valida = ValidarVacios($elementRequired);
                if (Valida == false) {

                } else if (Valida == "error") {
                    event.preventDefault();
                } else {
                    event.preventDefault();
                    FuncBtnLoader($BtnUpdateTemplate, 1);
                    var params = new Object();
                    params['IdTemplates'] = $('#HdIdPlantilla').val();
                    params['NameTemplate'] = $('#TxtNameTemplate').val();
                    params['DescriptionTemplate'] = $('#TxtDescriptionTemplate').val();
                    params['Sitio'] = {
                        IdMasterSites: $('#DdlSitio').val()
                    };
                    var orden = $('.numDivFields');
                    var ListFieldsUDF = [];
                    for (var i = 0; i < orden.length; i++) {
                        var IdFields = orden[i].id.replace("divNewInput_", "");
                        ListFieldsUDF.push({
                            IdFieldsUDF: IdFields,
                            Position: i
                        });
                    }
                    params['ListFieldsUDF'] = ListFieldsUDF;
                    var Result = await $.ajax({
                        type: "post",
                        url: '/Templates/UpdateTemplate',
                        data: params
                    });
                    swal("Bien Hecho!", "Accion realizada correctamente.", "success");
                    setTimeout(function () {
                        window.location = "/Templates";
                    }, 1000);
                }
            } catch (ex) {
                swal("Error interno", "Comunicate con el area de IT para la verificacion de este error..", "error");
            } finally {
                setTimeout(function () {
                    FuncBtnLoader($BtnUpdateTemplate, 0);
                }, 500);
            }
        });
    }
})();
//async function AddHTMLInputTemplate(params) {
//    var $labelField = '<strong>' + params.NameField +'</strong>';
//    var $inputField = "";
//    var $elementsFinal = "";
//    var TipoCampo = params.FieldType.IdFieldsTypesUDF;
//    if (TipoCampo == 1) //Fecha
//    {
//        $inputField = '<input type="text" class="form-control form-control-sm datepicker" placeholder="Fecha"/>';
//        $elementsFinal = $labelField + $inputField;
//    } else if (TipoCampo == 2) //Numero
//    {
//        $inputField = '<input type="number" placeholder="Numero" class="form-control form-control-sm" onkeypress="return solonumeros(event);" />';
//        $elementsFinal = $labelField + $inputField;
//    } else if (TipoCampo == 3) //Linea de texto simple
//    {
//        $inputField = '<input type="text" placeholder="Texto" class="form-control form-control-sm" maxlength="' + params.Longitud+'" />';
//        $elementsFinal = $labelField + $inputField;
//    } else if (TipoCampo == 5) //Caja de verificacion
//    {
//        var count = $('.custom-control-input').length;
//        var $elementInput = '<input type="checkbox" class="custom-control-input" id="check' + count+'" />';
//        $inputField = `<div class="text-center">
//                          <div class="custom-control custom-checkbox">
//                            ${$elementInput}
//                            <label class="custom-control-label" for="check${count}">${params.NameField}</label>
//                          </div>
//                        </div>`;
//        $elementsFinal = $inputField;
//    } else if (TipoCampo == 6) //Lista desplegable
//    {
//        var dispositions = params.Dispositions;
//        var $options = '<option value="">--Seleccionar--</option>';
//        $.each(dispositions, function (key, value) {
//            $options += '<option>' + value.Dispositions + '</option>';
//        });
//        $inputField = `<select class="form-control form-control-sm">
//                ${$options}
//            </select>`;
//        $elementsFinal = $labelField + $inputField;
//    }
//    var $divInput = `<div class="col-md-3 numDivFields" id="divHTMLInput${params.IdDivHTML}">
//                        <div class="form-group">
//                            <div class="card">
//                                <div class="card-body rounded-sm shadow" style="padding: 10px;">
//                                    <div class="text-center">
//                                        ${$elementsFinal}
//                                    </div>
//                                </div>
//                                <div class="card-footer" style="padding-top: 7px;padding-bottom: 7px;">
//                                    <div class="text-center divBtns">
//                                        <button type="button" style="padding-top: 0px;padding-bottom: 0px;" class="btn btn-link" onclick="DeleteInputHTML(${params.IdDivHTML});">Desactivar</button>
//                                    </div>
//                                </div>
//                            </div>
//                        </div>
//                    </div>`;
//    if (params.SolutionField == "false") {
//        $('#sortable').append($divInput);
//    } else {
//        $('#sortableSolutions').append($divInput);
//    }
//}
//Listas desplegables
async function AddOptionItemListas() {
    var ListDependent = $('#DdlListDependent').val();
    var ItemDependent = $('#DdlItemDependent').val();
    var ItemListas = $('#TxtItemLista').val();
    var valiError = $('#TxtItemLista')[0].classList.contains('is-invalid');
    if (ItemListas == "") {
        toastr.error('Escriba un item para continuar.', 'No puede ser vacío');
        $('#TxtItemLista').focus();
    } else if (ListDependent != "" & ItemDependent == "") {
        toastr.error('Seleccione el item del que dependera, para continuar.', 'No puede ser vacío');
        $('#DdlItemDependent').focus();
    } else if (valiError == true) {
        toastr.error('Verifique los errores para continuar.', 'Verificar');
        $('#TxtItemLista').focus();
    } else {
        $('#DdlItemLista').append('<option class="newOption" value="' + ItemListas + '">' + ItemListas + '</option>');
        $('#TxtItemLista').val('');
        toastr.info('<b>' + ItemListas + '</b> fue agregado correctamente no olvide guardar los cambios.', 'Bien hecho');
    }
}
async function DeleteItemLista() {
    try {
        var TextItemLista = $('#DdlItemLista option:selected').html();
        var ItemLista = $('#DdlItemLista').val();
        var $element = $("#DdlItemLista option[value='" + ItemLista + "']");
        if (ItemLista == "") {
            toastr.error('Seleccione un item para continuar.', 'Seleccione');
        } else if ($element[0].classList.contains('newOption')) {
            $("#DdlItemLista option[value='" + ItemLista + "']").remove();
            $('#DdlItemLista').val('');
            toastr.info('<b>' + ItemLista + '</b> fue eliminado correctamente.', 'Eliminado');
        } else {
            swal({
                title: "¿Quieres continuar?",
                text: "¿Deseas eliminar el item " + TextItemLista + "?",
                type: "warning",
                showCancelButton: true,
                confirmButtonText: "Si, Eliminar!",
                closeOnConfirm: true,
                showLoaderOnConfirm: true
            }, async function () {
                var params = new Object();
                params['IdDispositions'] = ItemLista;
                var Result = await $.ajax({
                    type: "post",
                    url: '/Templates/DeleteItemDispositions',
                    data: params
                });
                $("#DdlItemLista option[value='" + ItemLista + "']").remove();
                $('#DdlItemLista').val('');
                toastr.success('<b>' + TextItemLista + '</b> fue eliminado correctamente.', 'Eliminado');
            });
        }
    } catch (ex) {
        swal("Error interno", "Comunicate con el area de IT para la verificacion de este error..", "error");
    }
}
//End Listas desplegables
async function VerifyNameTemplate(value) {
    if (value != "") {
        var params = new Object();
        params["NameTemplate"] = value;
        var Result = await $.ajax({
            type: "post",
            url: '/Templates/VerifyNameTemplate',
            data: params
        });
        if (Result == true) {
            $('#TxtNameTemplate').removeClass('is-invalid');
            $('#TxtNameTemplate-error').html('');
        } else {
            $('#TxtNameTemplate').addClass('is-invalid');
            $('#TxtNameTemplate-error').html(Result);
        }
    }
}
async function DeleteInputHTML(IdHTML) {
    try {
        swal({
            title: "¿Deseas eliminar para siempre este campo?",
            text: "¿Quieres continuar?",
            type: "warning",
            showCancelButton: true,
            confirmButtonText: "Si, continuar!",
            closeOnConfirm: true,
            showLoaderOnConfirm: true
        }, async function () {
            var cacheListFields = JSON.parse(localStorage.getItem("ListFields"));
            delete cacheListFields[IdHTML];
            localStorage.setItem("ListFields", JSON.stringify(cacheListFields));
            $('#divHTMLInput-' + IdHTML).remove();
            toastr.info('Eliminado correctamente.', 'Eliminado');
        });
    } catch (ex) {
        swal("Error interno", "Comunicate con el area de IT para la verificacion de este error..", "error");
    }
}

async function DisabledFieldUDF(IdFields) {
    swal({
        title: "¿Deseas desactivar este campo?",
        text: "¿Quieres continuar?",
        type: "warning",
        showCancelButton: true,
        confirmButtonText: "Si, continuar!",
        closeOnConfirm: true,
        showLoaderOnConfirm: true
    }, async function () {
        var params = new Object();
            params['IdFields'] = IdFields;
        var Result = await $.ajax({
            type: "post",
            url: '/Templates/DisabledFields',
            data: params
        });
            toastr.info('Campo desactivado correctamente.', 'Bien hecho');
        $('#divNewInput_' + IdFields + ' .BtnDisabled').remove();
        $('#divNewInput_' + IdFields + ' .divBtns')
            .append('<button type="button" class="btn btn-link BtnEnabled" onclick="EnabledFieldUDF(' + IdFields + ');" style="padding-top: 0px;padding-bottom: 0px;">Activar</button>');
            $('#divNewInput_' + IdFields + ' .card').addClass('border-danger');
            $('#divNewInput_' + IdFields + ' .card-footer').addClass('border-danger');
    });
}
async function EnabledFieldUDF(IdFields) {
    swal({
        title: "¿Deseas activar este campo?",
        text: "¿Quieres continuar?",
        type: "warning",
        showCancelButton: true,
        confirmButtonText: "Si, continuar!",
        closeOnConfirm: true,
        showLoaderOnConfirm: true
    }, async function () {
        var params = new Object();
            params['IdFields'] = IdFields;
        var Result = await $.ajax({
            type: "post",
            url: '/Templates/EnabledFields',
            data: params
        });
            toastr.success('Campo activado correctamente.', 'Bien hecho');
            $('#divNewInput_' + IdFields + ' .BtnEnabled').remove();
            $('#divNewInput_' + IdFields + ' .divBtns').append('<button type="button" class="btn btn-link BtnDisabled" onclick="DisabledFieldUDF(' + IdFields+');" style="padding-top: 0px;padding-bottom: 0px;">Desactivar</button>');
            $('#divNewInput_' + IdFields + ' .card').removeClass('border-danger');
            $('#divNewInput_' + IdFields + ' .card-footer').removeClass('border-danger');
    });
}
async function changeTipoCampo(Id) {
    $('.divFields').hide();
    $('.divFields .form-control').prop('disabled', true).val('');
    if (Id != "") {
        $('.divFieldsTypes' + Id + ' .form-control').prop('disabled', false);
        $('.divFieldsTypes' + Id).show();
    }
    var IdTemplate = $('#HdIdPlantilla').val();
    if (IdTemplate != "0") {
        await GetFieldsTypeList();
        $('#msjListas').text('');
    } else {
        $('#msjListas').html('Para poder crear listas dependientes, primero debes guardar esta plantilla.');
        $('#DdlListDependent').prop('disabled', true).html('<option value="">--Ninguna--</option>');
        $('#DdlItemDependent').prop('disabled', true);
    }
}
async function SearchInforInput(SolutionField) {
    try {
        await limpiarCampos();
        $('#myModal').modal('show');
        $('#HdSolutionField').val(SolutionField);
        if (SolutionField == true) {
            $('.FieldsTypes-8').hide() //Ocultar campo puesto
            $('.TemplateSolutions').show();
        } else {
            $('.FieldsTypes-8').show() //Ocultar campo puesto
            $('.TS-true').hide();
        }
    } catch (ex) {
        swal("Error interno", "Comunicate con el area de IT para la verificacion de este error..", "error");
    }
}
async function ModalEditAjaxInput(IdFieldsUDF) {
    try {
        $('#myModal').modal('show');
        var params = new Object();
        params["Fields"] = {
            IdFieldsUDF: IdFieldsUDF
        };
        result = await $.ajax({
            type: "post",
            url: '/Templates/FieldsUDFJson',
            data: params
        });
        $('#HdIdInput').val(result.IdFieldsUDF);
        $('#DdlTipoCampo').val(result.FieldType.IdFieldsTypesUDF).change();
        $('#DdlTipoObligatorio').val(result.TypeRequired.IdTypeRequired);
        $('#TxtNombreCampo').val(result.NameField);
        if (result.Longitud != 0) {
            $('#TxtLongitudCampo').val(result.Longitud);
        }
        $('#HdSolutionField').val(result.SolutionField);
        if (result.SolutionField == true) {
            $('.TemplateSolutions').show();
        } else {
            $('.TS-true').hide();
        }
        if (result.FieldType.IdFieldsTypesUDF == 6) {
            await GetFieldsTypeList();
        }
        var ListDependet = result.ParentDispositions.IdFieldsUDF;
        if (ListDependet != 0) {
            $('#DdlListDependent').val(ListDependet);
            await changeListDependent(ListDependet);
            var Parent_IdDispo = result.ParentDispositions.Parent_IdDispositions;
            $('#DdlItemDependent').val(Parent_IdDispo);
            await changeListItemDependent(Parent_IdDispo);
        } else {
            var ItemLista = result.Dispositions;
            var $option = '<option value="">--Seleccionar--</option>';
            $.each(ItemLista, function (key, value) {
                $option += '<option value="' +  value.IdFieldsDispositions + '">' + value.Dispositions + '</option>';
            });
            $('#DdlItemLista').html($option);
            $('.noEdit').prop('disabled', true);
        }
    } catch (ex) {
        swal("Error interno", "Comunicate con el area de IT para la verificacion de este error..", "error");
    }
}

async function GetFieldsTypeList() {
    var params = new Object();
    params["Template"] = {
        IdTemplates: $('#HdIdPlantilla').val()
    };
    //Listas deplegables
    params["FieldType"] = {
        IdFieldsTypesUDF: 6
    };
    params["SolutionField"] = $('#HdSolutionField').val();
    var Result = await $.ajax({
        type: "post",
        url: '/Templates/FieldsTypeList',
        data: params
    });
    var $option = '<option value="">--Ninguna--</option>';
    
    var IdFieldsUDF = $('#HdIdInput').val();
    $.each(Result, function (key, value) {
        if (value.IdFieldsUDF != IdFieldsUDF) {
            $option += '<option value="' + value.IdFieldsUDF + '">' + value.NameField + '</option>';
        }
    });
    $('#DdlListDependent').html($option);
    $('#DdlItemDependent').prop('disabled', true);
}
async function changeListDependent(value) {
    var $option = '<option value="">--Seleccionar--</option>';
    if (value != "") {
        var params = new Object();
        params["IdFieldsUDF"] = value;
        var Result = await $.ajax({
            type: "post",
            url: '/Templates/ListDispositions',
            data: params
        });
        $.each(Result, function (key, value) {
            $option += '<option value="' + value.IdFieldsDispositions + '">' + value.Dispositions + '</option>';
        });
    } 
    $('#DdlItemDependent').html($option).change();
    if (value != "") {
        $('#DdlItemDependent').prop('disabled', false);
    } else {
        $('#DdlItemDependent').prop('disabled', true);
    }
}
async function changeListItemDependent(value) {
    var $option = '<option value="">--Seleccionar--</option>';
    if (value != "") {
        var params = new Object();
        params["IdFieldsUDF"] = $('#HdIdInput').val();
        params["Parent_IdDispositions"] = value;
        var Result = await $.ajax({
            type: "post",
            url: '/Templates/ListDispositions',
            data: params
        });
        $.each(Result, function (key, value) {
            $option += '<option value="' + value.IdFieldsDispositions + '">' + value.Dispositions + '</option>';
        });
    }
    $('#DdlItemLista').html($option);
}
function limpiarCampos() {
    $('#HdIdInput').val('');
    $('#myModal .form-control').val('');
    $('#TxtNameCategory').removeClass('is-invalid');
    $('#myModal .text-danger').html('');
    $('#DdlItemLista').html('<option value="">--Seleccionar--</option>');
    $('.divFields').hide();
    $('.noEdit').prop('disabled', false);
    $('.divFields .form-control').prop('disabled', true);
    $('#TxtItemLista').removeClass('is-invalid');
    $('#TxtItemLista-error').html('');
}
async function ValidaItemListas(value) {
    if (value != "") {
        var Validar = $('#DdlItemLista option[value="' + value + '"]').val();
        if (Validar != undefined) {
            $('#TxtItemLista').addClass('is-invalid');
            $('#TxtItemLista-error').html(value + " ya se encuentra agregado.");
        } else {
            $('#TxtItemLista').removeClass('is-invalid');
            $('#TxtItemLista-error').html('');
        }
    } else {
        $('#TxtItemLista').removeClass('is-invalid');
        $('#TxtItemLista-error').html('');
    }
}