$('.chosen-select').chosen();
$('.summernote').summernote();
$('#divLoaderMaster').hide();

const $BtnGuardar = document.getElementById('BtnGuardar');
async function obtenerValueFields($ObjFields) {
    var Retorno = []
    var valor = ""
    if ($ObjFields.dataset.idfieldstypes == 5)
    {
        valor = $('#' + $ObjFields.id).is(':checked').toString()
    }
    else if ($ObjFields.dataset.idfieldstypes == 6)
    {
        valor = $('#' + $ObjFields.id + ' option:selected').text()
        if (valor.includes("Seleccionar")) {
            valor = ""
        }
    }
    else if ($ObjFields.dataset.idfieldstypes == 8)
    {
        valor = $('#' + $ObjFields.id).chosen().val()
    }
    else
    {
        valor = $ObjFields.value
    }
    Retorno.push({
        NameFieldsUDF: $ObjFields.dataset.namefieldsudf,
        Value: valor
    })
    return Retorno[0]
}
async function SetGuardarTickets($Btn) {
    try
    {
        ToolsFuncBtnLoader($Btn, 1)
        let IdPlantilla = $('#DdlPlantilla').val()
        if (IdPlantilla == "")
        {
            $('#DdlPlantilla').focus()
            toastr.error("Seleccione la plantilla para continuar", "La plantilla es obligatoria")
            return
        }
        let IdCategory = $('#DdlCategory').val()
        let IdSubCategory = $('#DdlSubCategory').val()
        if (IdCategory == "") {
            $('#DdlCategory').focus()
            toastr.error("Seleccione la categoria para continuar", "La categoria es obligatoria")
            return
        }
        if (IdSubCategory == "") {
            $('#DdlSubCategory').focus()
            toastr.error("Seleccione la Sub categoria para continuar", "La sub categoria es obligatoria")
            return
        }
        var $elementRequired = $('#formCreate .required')
        var Valida = NewValidarVacios($elementRequired)
        if (Valida == false)
        {
            toastr.error('Los campos no pueden estar vacios o con errores.', 'Campos obligatorios')
            return
        } 
        swal({
            title: "¿Estas seguro?",
            text: "¿Deseas guardar y crear este caso?",
            type: "warning",
            showCancelButton: true,
            confirmButtonText: "Si, continuar!",
            closeOnConfirm: true,
            showLoaderOnConfirm: true
        }, async function ()
            {
                var $Fields = $('.fieldSave');
                var objFields = []
            for (i = 0; i < $Fields.length; i++) {
                var Array = await obtenerValueFields($Fields[i]);
                objFields.push(Array);
            }
            var params = new Object();
            params["Template"] = {
                IdTemplates: $('#DdlPlantilla').val()
            }
            params["Title"] = $('#TxtTitle').val();
            params["Description"] = $("#TxtResolutions").summernote("code");
            params["Category"] = {
                IdCategory: $('#DdlCategory').val(),
                NameCategory: $('#DdlCategory option:selected').text(),
            }
            params["SubCategory"] = {
                IdCategory: $('#DdlSubCategory').val(),
                NameCategory: $('#DdlSubCategory option:selected').text()
            }
            params["ListFieldsUDF"] = objFields;

            var Result = await $.ajax({
                type: "post",
                url: '/CreateWorkOrder/SaveWorkOrder',
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(params)
            });
            swal("Bien Hecho!", "Accion realizada correctamente.", "success");
            setTimeout(function () {
                window.location = "/Home";
            }, 1000);
        });
    } catch (ex) {
        swal("Error interno", "Comunicate con el area de IT para la verificacion de este error..", "error");
    } finally {
        ToolsFuncBtnLoader($Btn, 0)
    }
}
async function changeSitios(value) {
    var $optionTempl = '<option value="">--Seleccionar--</option>';
    var $optionCate = '<option value="">--Seleccionar--</option>';
    if (value != "") {
        let params = new Object();
        params['Sitio'] = { IdMasterSites: value };
        //templates
        let ListTemplates = await $.ajax({
            type: "post",
            url: '/CreateWorkOrder/GetListTemplates',
            data: params
        })
        for (let data of ListTemplates) {
            $optionTempl += '<option value="' + data.IdTemplates + '">' + data.NameTemplate + '</option>';
        }
        //$.each(ListTemplates, function (i, data) {
        //    $optionTempl += '<option value="' + data.IdTemplates + '">' + data.NameTemplate + '</option>';
        //});
        //Categories
        let ListCategory = await $.ajax({
            type: "post",
            url: '/CreateWorkOrder/GetListCategory',
            data: params
        })
        for (let data of ListCategory) {
            $optionCate += '<option value="' + data.IdCategory + '" data-idtemplates="' + data.Template.IdTemplates+'">' + data.NameCategory + '</option>';
        }
        //$.each(ListCategory, function (i, data) {
        //    $optionCate += '<option value="' + data.IdCategory + '">' + data.NameCategory + '</option>';
        //});
    }
    $('#DdlPlantilla').html($optionTempl).trigger('chosen:updated')
    $('#DdlCategory').html($optionCate).trigger('chosen:updated')
}
async function changeTemplates(value) {
    try {
        if (value != "") {
            FuncBtnLoader($BtnGuardar, 1);
            $('#DdlPlantilla').prop('disabled', true);
            $('.divFields').show();
            $('#divFields').hide();
            var params = new Object();
            params['Fields'] = {
                Template: {
                    IdTemplates: value
                }
            }
            var Result = await $.ajax({
                type: "post",
                url: '/CreateWorkOrder/TypeField',
                data: params
            });
            if (Result == "") {
                Result = "<h2 class='font-weight-bold text-danger'>Esta plantilla no cuenta con ningun campo agregado, no puede continuar.</h2>";
            } else {
                FuncBtnLoader($BtnGuardar, 0);
            }
            $('#divFields').html(Result)
            $('#DdlPlantilla').prop('disabled', false);
            $('#divLoadinFields').hide()
            $('#divFields').show()
        } else {
            $('.divFields').hide()
            $('#divFields').html('')
        }
    } catch (ex) {
        swal("Error interno", "Comunicate con el area de IT para la verificacion de este error..", "error");
    } finally {
        $('.chosen-select').chosen()
        $('.datepicker').datepicker({
            todayBtn: "linked",
            calendarWeeks: true,
            format: 'yyyy/mm/dd'
        })
    }
}
async function changeCategory(value) {
    var $option = '<option value="">--Seleccionar--</option>';
    var Grupo = "",mensaje="";
    if (value != "") {
        var IdTemplates = $('#DdlCategory option[value="' + value + '"]')[0].dataset.idtemplates;
        if (IdTemplates != "0") {
            $("#DdlPlantilla").val(IdTemplates).trigger('chosen:updated')
            await changeTemplates(IdTemplates)
        } 
        var params = new Object();
        params['IdCategory'] = value;
        var Result = await $.ajax({
            type: "post",
            url: '/CreateWorkOrder/GetSubCategory',
            data: params
        });
        Grupo = Result.Grupo.NameGroup;
        for (let data of Result.SubCategory) {
            $option += '<option value="' + data.IdCategory + '">' + data.NameCategory + '</option>'
        }
        var NumUsers = Result.Grupo.UsersXGroups.length;
        if (NumUsers == 0) {
            mensaje = "Este grupo no cuenta con usuarios disponibles para asignar.";
            $('#BtnGuardar').prop('disabled', true)
        } else {
            $('#BtnGuardar').prop('disabled', false)
        }
    }
    $('#lblGrupos-error').text(mensaje);
    $('#TxtGrupo').val(Grupo);
    $('.DdlSubCategory').html($option).trigger('chosen:updated')
}
async function onChangeList($select) {
    var $ListDepend = $('.divFieldsDepend' + $select.dataset.idfieldsudf);
    if ($ListDepend.length != 0) {
        var $option = '<option value="">--Seleccionar--</option>';
        if ($select.value != "") {
            var params = new Object();
            params['ParentIdDisposi'] = $select.value;
            var Result = await $.ajax({
                type: "post",
                url: '/CreateWorkOrder/GetListDispositions',
                data: params
            });
            for (const data of Result) {
            //$.each(Result, function (i, data) {
                $option += '<option value="' + data.IdFieldsDispositions + '">' + data.Dispositions + '</option>';
            }
        }
        for (i = 0; i < $ListDepend.length; i++) {
            $('#' + $ListDepend[i].id).html($option);
        }
    }
}
async function getListPlataformas($select) {
    let $option = '<option value="">--Seleccionar--</option>'
    if ($select.value != "")
    {
        let params = new Object()
        params['IdLocation'] = $select.value
        let Result = await $.ajax({
            type: "post",
            url: '/CreateWorkOrder/ListPlatform',
            data: params
        })
        for (const data of Result)
        {
            $option += `<option value="${data.IdPlatform}">${data.Description}</option>`
        }
    }
    $('#DdlPlataforma-' + $select.dataset.idfieldsudf).html($option)
    await getListPuestos($('#DdlPlataforma-' + $select.dataset.idfieldsudf)[0])
}
async function getListPuestos($select) {
    let $option = '<option value="">Buscar por Numero,IP,Extension o Nombre</option>'
    if ($select.value != "") {
        let params = new Object()
        params['IdPlatform'] = $select.value
        let Result = await $.ajax({
            type: "post",
            url: '/CreateWorkOrder/ListBooth',
            data: params
        })
        for (const data of Result) {
            $option += `<option value="${data.IdBooth}">${data.BoothNumber} - ${data.IP} - ${data.BoothName} - ${data.Extension}</option>`
        }
    }
    $('#field_' + $select.dataset.idfieldsudf).html($option).trigger('chosen:updated')
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

async function changeAgregarAdjunto() {
    try {
        FuncBtnLoader($BtnGuardar, 1);
        var file = document.getElementById("FileAddAttachments").files[0];
        var formData = new FormData();
        formData.append("Adjunto", file);
        var ResultAdjunt = await $.ajax({
            type: "post",
            url: '/CreateWorkOrder/AddAttachments',
            processData: false,
            contentType: false,
            cache: false,
            dataType: 'json',
            data: formData
        });
        if (ResultAdjunt.msjError != null) {
            toastr.error(ResultAdjunt.msjError, 'Error');
        } else {
            var params = new Object();
            ResultAdjunt.IdWorkOrder = $('#formCreate')[0].dataset.idworkorder;
            params['InforAdjunto'] = ResultAdjunt;
            var ReturnView = await $.ajax({
                type: "post",
                url: '/WorkOrderSolutions/DivAttachment',
                data: params
            });
            $('#FileAddAttachments').val('');
            $('#divAdjuntos').append(ReturnView);
            toastr.info('Adjunto agregado correctamente.', 'Adjunto agregado');
        }
    } catch (ex) {
        swal("Error interno", "Comunicate con el area de IT para la verificacion de este error..", "error");
    } finally {
        FuncBtnLoader($BtnGuardar, 0);
    }
}

async function deleteAdjunto(NameEncrypted) {
    try {
        var params = new Object();
        params["IdWorkOrder"] = $('#formCreate')[0].dataset.idworkorder;
        params['NameEncrypted'] = NameEncrypted;
        var Result = await $.ajax({
            type: "post",
            url: '/CreateWorkOrder/DeleteAttachment',
            data: params
        });
        $('#' + NameEncrypted).remove();
        toastr.success('Adjunto eliminado correctamente.', 'Adjunto eliminado');
    } catch (ex) {
        swal("Error interno", "Comunicate con el area de IT para la verificacion de este error..", "error");
    }

}