$('.datepicker').datepicker({
    todayBtn: "linked",
    calendarWeeks: true,
    format: 'yyyy/mm/dd'
    //format: 'dd/mm/yyyy'
})
var control = 0;
async function inputdate() {
    $('input:text[id=field_729]').val();
    $('input:text[id=field_730]').val();
    $('input:text[id=field_750]').val();
    $('input:text[id=field_751]').val();
    
    if (document.getElementById("field_729").value != "" && document.getElementById("field_730").value != "" )
    {
        if (document.getElementById("field_729").value >= document.getElementById("field_730").value )
        {

            console.log(field_729.value);
            console.log("control:", control);
            control = control + 1;
            swal({
                title: 'Alerta automatica',
                text: 'Rango de fecha incorrecto',
            })
        }
        else if (document.getElementById("field_750").value != "" && document.getElementById("field_751").value != "") {
            if (document.getElementById("field_750").value >= document.getElementById("field_751").value) {

                console.log(field_729.value);
                console.log("control:", control);
                control = control + 1;
                swal({
                    title: 'Alerta automatica InPQR',
                    text: 'Rango de fecha incorrecto',
                })
            }
        }
    }
    else if (document.getElementById("field_750").value != "" && document.getElementById("field_751").value != "")
    {
        if (document.getElementById("field_750").value >= document.getElementById("field_751").value ) {

            console.log(field_729.value);
            console.log("control:", control);
            control = control + 1;
            swal({
                title: 'Alerta automatica InPQR',
                text: 'Rango de fecha incorrecto',
            })
        }
    } 
}
$('.chosen-select').chosen()
$('#TxtDescriptionClient').summernote("code", $('#TxtDescriptionClient').text())
$('.summernote-disabled').summernote('disable');
$('#TxtResolution').summernote("code", $('#TxtResolution').text());
//$('#DdlStatus').change();
$('#divLoaderMaster').hide();

$("#BtnPrepago").hide();
$("#BtnPospago").hide();
$("#BtnAscard").hide();
$("#BtnCuotasAscard").hide();
$("#BtnEliminacionCentrales").hide();
$("#divTableprepago").hide();
$("#divTableascard1").hide();
$("#divTablepospago").hide();
$("#divTableascard2").hide();
$("#divTableeliminar").hide();


$(document).ready(function () {

    var date = new Date();
    var mes = (date.getMonth() + 1);
    if ((date.getMonth() + 1) < 10) {
        mes = '0' + mes;
    }
    var fecha = date.getFullYear() + '/' + mes + '/' + date.getDate();
    var hora = date.getHours() + ':' + date.getMinutes() + ':' + date.getSeconds();
    var fechaYHora = fecha;// + ' ' + hora;
    //console.log(fechaYHora);

    BtnGuardar_S.disabled = true;

});

//validar colores postpago
let boton = document.getElementById("BtnGuardar_S")

let field_797 = document.getElementById("field_797")
let field_798 = document.getElementById("field_798")
let field_768 = document.getElementById("field_768")
let field_799 = document.getElementById("field_799")
let field_767 = document.getElementById("field_767")
let field_769 = document.getElementById("field_769")
let field_770 = document.getElementById("field_770")
let field_801 = document.getElementById("field_801")
let field_727 = document.getElementById("field_727")
let field_729 = document.getElementById("field_729")
let field_730 = document.getElementById("field_730")
let field_728 = document.getElementById("field_728")
let field_771 = document.getElementById("field_771")
let field_803 = document.getElementById("field_803")
let field_859 = document.getElementById("field_859")
let field_804 = document.getElementById("field_804")
let field_805 = document.getElementById("field_805")
let field_806 = document.getElementById("field_806")

//validar colores ascard
let field_731 = document.getElementById("field_731")
let field_732 = document.getElementById("field_732")
let field_820 = document.getElementById("field_820")
let field_736 = document.getElementById("field_736")
let field_737 = document.getElementById("field_737")
let field_739 = document.getElementById("field_739")
let field_819 = document.getElementById("field_819")

//validar colores cuotas ascard
let field_740 = document.getElementById("field_740")
let field_741 = document.getElementById("field_741")
let field_744 = document.getElementById("field_744")
let field_745 = document.getElementById("field_745")

//validar prepago
let field_830 = document.getElementById("field_830")
let field_832 = document.getElementById("field_832")
let field_833 = document.getElementById("field_833")
let field_834 = document.getElementById("field_834")
let field_835 = document.getElementById("field_835")
let field_836 = document.getElementById("field_836")
let field_837 = document.getElementById("field_837")
let field_838 = document.getElementById("field_838")
let field_839 = document.getElementById("field_839")

//validar eliminacion de centrales
let field_844 = document.getElementById("field_844")
let field_846 = document.getElementById("field_846")
let field_847 = document.getElementById("field_847")
let field_848 = document.getElementById("field_848")
let field_850 = document.getElementById("field_850")
let field_851 = document.getElementById("field_851")

boton.addEventListener("click", (event) => {
    event.preventDefault()
    if (field_797.value === "") {
        field_797.style.borderColor = "red"
        field_797.focus()
    }

    if (field_798.value === "") {
        field_798.style.borderColor = "red"
        field_798.focus()
    }

    if (field_768.value === "") {
        field_768.style.borderColor = "red"
        field_768.focus()
    }

    if (field_799.value === "") {
        field_798.style.borderColor = "red"
        field_799.focus()
    }

    if (field_767.value === "") {
        field_767.style.borderColor = "red"
        field_767.focus()
    }

    if (field_769.value === "") {
        field_769.style.borderColor = "red"
        field_769.focus()
    }

    if (field_770.value === "") {
        field_770.style.borderColor = "red"
        field_770.focus()
    }

    if (field_801.value === "") {
        field_801.style.borderColor = "red"
        field_801.focus()
    }

    if (field_727.value === "") {
        field_727.style.borderColor = "red"
        field_727.focus()
    }

    if (field_729.value === "") {
        field_729.style.borderColor = "red"
        field_729.focus()
    }

    if (field_730.value === "") {
        field_730.style.borderColor = "red"
        field_730.focus()
    }

    if (field_728.value === "") {
        field_728.style.borderColor = "red"
        field_728.focus()
    }

    if (field_771.value === "") {
        field_771.style.borderColor = "red"
        field_771.focus()
    }

    if (field_803.value === "") {
        field_803.style.borderColor = "red"
        field_803.focus()
    }

    if (field_859.value === "") {
        field_859.style.borderColor = "red"
        field_859.focus()
    }

    if (field_804.value === "") {
        field_804.style.borderColor = "red"
        field_804.focus()
    }

    if (field_805.value === "") {
        field_805.style.borderColor = "red"
        field_805.focus()
    }

    if (field_806.value === "") {
        field_806.style.borderColor = "red"
        field_806.focus()
    }


    //ascard
    if (field_731.value === "") {
        field_731.style.borderColor = "red"
        field_731.focus()
    }
    if (field_732.value === "") {
        field_732.style.borderColor = "red"
        field_732.focus()
    }
    if (field_813.value === "") {
        field_813.style.borderColor = "red"
        field_813.focus()
    }
    if (field_736.value === "") {
        field_736.style.borderColor = "red"
        field_736.focus()
    }
    if (field_737.value === "") {
        field_737.style.borderColor = "red"
        field_737.focus()
    }
    if (field_739.value === "") {
        field_739.style.borderColor = "red"
        field_739.focus()
    }
    if (field_819.value === "") {
        field_819.style.borderColor = "red"
        field_819.focus()
    }

    //cuotas ascard
    if (field_740.value === "") {
        field_740.style.borderColor = "red"
        field_740.focus()
    }
    if (field_741.value === "") {
        field_741.style.borderColor = "red"
        field_741.focus()
    }
    if (field_744.value === "") {
        field_744.style.borderColor = "red"
        field_744.focus()
    }
    if (field_745.value === "") {
        field_745.style.borderColor = "red"
        field_745.focus()
    }

    //prepago
    if (field_830.value === "") {
        field_830.style.borderColor = "red"
        field_830.focus()
    }
    if (field_832.value === "") {
        field_832.style.borderColor = "red"
        field_832.focus()
    }
    if (field_833.value === "") {
        field_833.style.borderColor = "red"
        field_833.focus()
    }
    if (field_834.value === "") {
        field_834.style.borderColor = "red"
        field_834.focus()
    }
    if (field_835.value === "") {
        field_835.style.borderColor = "red"
        field_835.focus()
    }
    if (field_836.value === "") {
        field_836.style.borderColor = "red"
        field_836.focus()
    }
    if (field_837.value === "") {
        field_837.style.borderColor = "red"
        field_837.focus()
    }
    if (field_830.value === "") {
        field_830.style.borderColor = "red"
        field_830.focus()
    }
    if (field_831.value === "") {
        field_831.style.borderColor = "red"
        field_831.focus()
    }

    //eliminacion de centrales
    if (field_844.value === "") {
        field_844.style.borderColor = "red"
        field_844.focus()
    }
    if (field_846.value === "") {
        field_846.style.borderColor = "red"
        field_846.focus()
    }
    if (field_847.value === "") {
        field_847.style.borderColor = "red"
        field_847.focus()
    }
    if (field_848.value === "") {
        field_848.style.borderColor = "red"
        field_848.focus()
    }
    if (field_850.value === "") {
        field_850.style.borderColor = "red"
        field_850.focus()
    }
    if (field_851.value === "") {
        field_851.style.borderColor = "red"
        field_851.focus()
    }

})


function textocajas() {
    field_797.addEventListener("keyup", () => {
        field_797.style.borderColor = "#d1d3e2"
    })
    field_798.addEventListener("keyup", () => {
        field_798.style.borderColor = "#d1d3e2"
    })
    field_768.addEventListener("keyup", () => {
        field_768.style.borderColor = "#d1d3e2"
    })
    field_799.addEventListener("keyup", () => {
        field_799.style.borderColor = "#d1d3e2"
    })
    field_767.addEventListener("keyup", () => {
        field_767.style.borderColor = "#d1d3e2"
    })
    field_769.addEventListener("keyup", () => {
        field_769.style.borderColor = "#d1d3e2"
    })
    field_770.addEventListener("keyup", () => {
        field_770.style.borderColor = "#d1d3e2"
    })
    field_801.addEventListener("keyup", () => {
        field_801.style.borderColor = "#d1d3e2"
    })
    field_727.addEventListener("keyup", () => {
        field_727.style.borderColor = "#d1d3e2"
    })
    field_729.addEventListener("keyup", () => {
        field_729.style.borderColor = "#d1d3e2"
    })
    field_730.addEventListener("keyup", () => {
        field_730.style.borderColor = "#d1d3e2"
    })
    field_728.addEventListener("keyup", () => {
        field_728.style.borderColor = "#d1d3e2"
    })
    field_771.addEventListener("keyup", () => {
        field_771.style.borderColor = "#d1d3e2"
    })
    field_803.addEventListener("keyup", () => {
        field_803.style.borderColor = "#d1d3e2"
    })
    field_859.addEventListener("keyup", () => {
        field_859.style.borderColor = "#d1d3e2"
    })
    field_804.addEventListener("keyup", () => {
        field_804.style.borderColor = "#d1d3e2"
    })
    field_805.addEventListener("keyup", () => {
        field_805.style.borderColor = "#d1d3e2"
    })
    field_806.addEventListener("keyup", () => {
        field_806.style.borderColor = "#d1d3e2"
    })

    //ascard
    //field_731.addEventListener("keyup", () => {
    //    field_731.style.borderColor = "#d1d3e2"
    //})
    //field_732.addEventListener("keyup", () => {
    //    field_732.style.borderColor = "#d1d3e2"
    //})
    //field_820.addEventListener("keyup", () => {
    //    field_820.style.borderColor = "#d1d3e2"
    //})
    //field_736.addEventListener("keyup", () => {
    //    field_736.style.borderColor = "#d1d3e2"
    //})
    //field_737.addEventListener("keyup", () => {
    //    field_737.style.borderColor = "#d1d3e2"
    //})
    //field_739.addEventListener("keyup", () => {
    //    field_739.style.borderColor = "#d1d3e2"
    //})
    //field_819.addEventListener("keyup", () => {
    //    field_819.style.borderColor = "#d1d3e2"
    //})

    //cuotas 
    //field_740.addEventListener("keyup", () => {
    //    field_740.style.borderColor = "#d1d3e2"
    //})
    //field_741.addEventListener("keyup", () => {
    //    field_741.style.borderColor = "#d1d3e2"
    //})
    //field_744.addEventListener("keyup", () => {
    //    field_744.style.borderColor = "#d1d3e2"
    //})
    //field_745.addEventListener("keyup", () => {
    //    field_745.style.borderColor = "#d1d3e2"
    //})

    //prepago 
    //field_830.addEventListener("keyup", () => {
    //    field_830.style.borderColor = "#d1d3e2"
    //})
    //field_832.addEventListener("keyup", () => {
    //    field_832.style.borderColor = "#d1d3e2"
    //})
    //field_833.addEventListener("keyup", () => {
    //    field_833.style.borderColor = "#d1d3e2"
    //})
    //field_834.addEventListener("keyup", () => {
    //    field_834.style.borderColor = "#d1d3e2"
    //})
    //field_834.addEventListener("keyup", () => {
    //    field_834.style.borderColor = "#d1d3e2"
    //})
    //field_836.addEventListener("keyup", () => {
    //    field_836.style.borderColor = "#d1d3e2"
    //})
    //field_837.addEventListener("keyup", () => {
    //    field_837.style.borderColor = "#d1d3e2"
    //})
    //field_838.addEventListener("keyup", () => {
    //    field_838.style.borderColor = "#d1d3e2"
    //})
    //field_839.addEventListener("keyup", () => {
    //    field_839.style.borderColor = "#d1d3e2"
    //})

    //eliminacion 
    //field_844.addEventListener("keyup", () => {
    //    field_844.style.borderColor = "#d1d3e2"
    //})
    //field_846.addEventListener("keyup", () => {
    //    field_846.style.borderColor = "#d1d3e2"
    //})
    //field_847.addEventListener("keyup", () => {
    //    field_847.style.borderColor = "#d1d3e2"
    //})
    //field_848.addEventListener("keyup", () => {
    //    field_848.style.borderColor = "#d1d3e2"
    //})
    //field_850.addEventListener("keyup", () => {
    //    field_850.style.borderColor = "#d1d3e2"
    //})
    //field_851.addEventListener("keyup", () => {
    //    field_851.style.borderColor = "#d1d3e2"
    //})
}

//// fin validar colores

const $BtnGuardar = document.getElementById('BtnGuardar_S');
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
    ocultar_prepago();
    //BtnGuardar_S.disabled = false;

    async function obtenerValueFields($ObjFields) {
        var Retorno = [];
        var valor = "";
        if ($ObjFields.dataset.idfieldstypes == 5) {
            valor = $('#' + $ObjFields.id).is(':checked').toString();
        } else if ($ObjFields.dataset.idfieldstypes == 6) {
            valor = $('#' + $ObjFields.id + ' option:selected').text();
            if (valor.includes("Seleccionar")) {
                valor = "";
            }
        } else {
            valor = $ObjFields.value;
        }
        Retorno.push({
            NameFieldsUDF: $ObjFields.dataset.namefieldsudf,
            Value: valor
        });
        return Retorno[0];
    }
    if ($BtnGuardar != undefined)
    debugger
    {
        $BtnGuardar.addEventListener('click', async (event) => {
            $('.content-wrapper .form-control').removeAttr('required');
            debugger
            try {
                FuncBtnLoader($BtnGuardar, 1);
                var $elementRequired = $('#formCreate .required');
                var $elementRequired2 = $('#formCreate .requiredForever');
                var ValidaOthers = true;
                var Valida = true;
                var status = ["1", "593", "594", "595", "230", "285", "421", "443", "513", "556"]; //ESTADOS NO OBLIGATORIOS PARA AIRE

                var idStatus = $('#DdlStatus').val();
                var index = $.inArray(idStatus, status)

                //validaciones de colores
                //postpago
                

                if (index == -1) {
                    Valida = await ValidarVacios($elementRequired);
                    ValidaOthers = await ValidarVacios($elementRequired2);
                }
                 var idAjusteMovil = $("#IdTemplate_SuicheMovil").val();
                if (idAjusteMovil === '28') {
                    var Custcode = $('#field_781').val().toUpperCase();
                    //console.log(mdm_pqr.length);
                    if (Custcode == '') {

                       
                    } else if (Custcode.length < 10){
                        swal({
                            title: '¡Alerta automatica!',
                            text: 'Campo Custcode debe tener minimo 10 caracteres para poder guardar',
                        })

                        event.preventDefault();
                        return false;
                    }

                    // var Min = $('#field_767').val().toUpperCase();
                    // //console.log(mdm_pqr.length);
                    //if (Min == '' ) {

                        
                    //} else if (Min.length > 6) {
                    //    swal({
                    //        title: '¡Alerta automatica!',
                    //        text: 'Campo Min debe tener minimo 10 caracteres para poder guardar',
                    //    })

                    //    event.preventDefault();
                    //    return false;
                    //}
                }
                 if (Valida == false) {
                    swal({
                        title: '¡Alerta automatica!',
                        text: 'Todos los campos son obligatorios',
                    })
                    event.preventDefault();
                    return false;

                } if (ValidaOthers == false) {
                    swal({
                        title: '¡Alerta automatica!',
                        text: 'Todos los campos son obligatorios',
                    })
                    textocajas()
                    event.preventDefault();
                    return false;
                //}else if ($('#field_797').val() != '' && $('#field_797').val().toUpperCase().length<7) {
                ////    //console.log(mdm_pqr.length);
                //        swal({
                //        title: '¡Alerta automatica!',
                //        text: 'Todos los campos son obligatorios para poder guardar',
                //    })

                //    event.preventDefault();
                //    return false;

                //} else if ($('#field_825').val().toUpperCase().length < 7) {
                //    //console.log(mdm_pqr.length);
                //    //swal({
                //    //    title: '¡Alerta automatica!',
                //    //    text: 'Campo Min debe tener minimo 7 caracteres para poder guardar',
                //    //})

                //    event.preventDefault();
                //    return false;

                } else if ($('#field_770').val() != '' && $('#field_770').val().toUpperCase().length < 3 ) {
                    //    //console.log(mdm_pqr.length);
                    swal({
                        title: '¡Alerta automatica!',
                        text: 'Todos los campos son obligatorios para poder guardar',
                    })

                    event.preventDefault();
                    $('#field_770').val("");
                    return false;

                }

                //else if ($('#field_149').val() != "4500" && $('#field_145').val() == "--Seleccionar--" || $('#field_149').val() != "4501" && $('#field_145').val() == "--Seleccionar--") { otiginal
                else if ($('#field_149').val() != "4500" && $('#field_150').val() == "--Seleccionar--" || $('#field_149').val() != "4501" && $('#field_150').val() == "--Seleccionar--") {
                    swal({ title: 'Debe seleccionar un ajuste!', type: 'error', timer: 5000 })
                    event.preventDefault();
                    return false;

                }else if ($('#field_150').val() === "212" && control == 0) {

                        swal({ title: 'Selecciono ajuste sin gestionarlo!', type: 'error',timer:5000 })
                        event.preventDefault();
                        return false; 
                } 


                if ($('#field_767').val() != '' && $('#field_767').val().toUpperCase().length < 10) {
                    //    //console.log(mdm_pqr.length);

                    swal({
                        title: '¡Alerta automatica!',
                        text: 'Todos los campos son obligatorios para poder guardar, MIN debe tener 10 digitos',
                    })

                    event.preventDefault();
                    return false;
                }

                if ($('#field_833').val() != '' && $('#field_833').val().toUpperCase().length < 10) {
                    //    //console.log(mdm_pqr.length);
                    swal({
                        title: '¡Alerta automatica!',
                        text: 'Todos los campos son obligatorios para poder guardar, MIN debe tener 10 digitos',
                    })

                    event.preventDefault();
                    return false;
                }

                    else {
                            event.preventDefault();
                        swal
                        (
                            {
                                title: "¿Estas seguro?",
                                text: "¿Deseas guardar los cambios?",
                                type: "warning",
                                showCancelButton: true,
                                confirmButtonText: "Si, continuar!",
                                closeOnConfirm: true,
                                showLoaderOnConfirm: true
                            }, async function ()
                                {
                                    try
                                    {
                                        debugger
                                            var $Fields = $('.fieldSave');
                                            var objFields = [];
                                            for (i = 0; i < $Fields.length; i++) {
                                                var Array = await obtenerValueFields($Fields[i]);
                                                objFields.push(Array);
                                            }
                                            var params = new Object();
                                            
                                            params["IdWorkOrder"] = $('#formCreate')[0].dataset.idworkorder;
                                            params["Status"] = {
                                                IdStatusDefinition: $('#DdlStatus').val()
                                            }
                                            params["SubStatus"] = {
                                                IdStatusDefinition: $('#DdlSubStatus').val()
                                            }
                                            params["Resolutions"] = $("#TxtResolution").summernote("code");
                                            params["ListFielsUDFSolution"] = objFields;
                                            params["WorkOrderEscalations"] = {
                                                GroupsScaled: {
                                                    IdMasterGroups: $('#DdlGroupScale').val()
                                                },
                                                TypeScaling: $('#DdlTypeScaled').val(),
                                                Comments: $('#TxtCommentsScaled').val()
                                            }

                                            if ($('#field_725').val() === "4192") {
                                                params["BPBServicio"] = {
                                                    Nombre: $("#DdlServicio").val()
                                                }
                                                params["CtaContable"] = {
                                                    Nombre: $("#DdlCTA_Contable").val()
                                                }

                                            }

                                            
                                            var Result = await $.ajax({
                                                type: "post",
                                                url: '/WorkOrderSolutions/SaveWorkOrderSolutions',
                                                contentType: "application/json; charset=utf-8",
                                                data: JSON.stringify(params)
                                            });
                                        if ($('#field_150').val() === "212")
                                        {

                                            event.preventDefault();
                                            swal
                                                (
                                                    {
                                                        title: "Gestion guardada exitosamente",
                                                        text: "¿Termino la Gestion de la PQR?",
                                                        type: "success",
                                                        showCancelButton: "No",
                                                        cancelButtonText: "No",
                                                        confirmButtonText: "Si",
                                                        closeOnConfirm: true,
                                                        showLoaderOnConfirm: true
                                                    },
                                                    async function () {
                                                        try {
                                                            swal("Bien Hecho!", "Acción correcta, Continue con la siguiente PQR.", "success");
                                                            setTimeout(function () {
                                                                window.location = "/ListWorkOrders";
                                                            }, 1000);

                                                        }
                                                        catch (ex) {
                                                            swal("Error save1 interno", "Comunicate con el area de IT para la verificacion de este error..", "error");
                                                        }
                                                        
                                                    }



                                            );

                                            //if ($('#field_150').val() === "212" && control == 1) {
                                            //swal("Bien Hecho!", "Acción correcta, Continue con el siguiente Ajuste", "success");
                                            //setTimeout(function () {
                                            //    location.reload();
                                            //}, 1000);

                                            //}

                                            //else if ($('#field_150').val() === "213" && control == 0) {
                                            //    swal("Bien Hecho!", "Acción correcta, Continue con la siguiente PQR.", "success");
                                            //    setTimeout(function () {
                                            //        window.location = "/ListWorkOrders";
                                            //    }, 1000);
                                            //}
                                        }
                                        else {
                                            swal("Bien Hecho!", "Acción realizado correctamente.", "success");
                                            setTimeout(function () {
                                                window.location = "/ListWorkOrders";
                                            }, 1000);
                                        }

                                    }
                                    catch (ex)
                                    {
                                            swal("Error save1 interno", "Comunicate con el area de IT para la verificacion de este error..", "error");
                                    }
                            }
                       );
                    
                    }
            } catch (ex) {

                swal("Error save2 interno", "Comunicate con el area de IT para la verificacion de este error..", "error");
                textocajas()
            } finally {
                FuncBtnLoader($BtnGuardar, 0); 
            }
        });
    }

    //ocultar campos de plantillas
    ocultar_eliminacioncentrales();
    ocultar_ascard();
    ocultar_cuotasascard();
    ocultar_postpago();
    ocultar_prepago();

    
    //console.log($("#field_149").val());

    cambiolista()
    $('#field_769').get(0).type = 'number';
    
    OcultarcamposplanillaMovil();
    var idAjusteMovil = $("#IdTemplate_SuicheMovil").val();
    if (idAjusteMovil === '28') {
        $("#PlanillAjusteMovil").show();
    } else {
        $("#PlanillAjusteMovil").hide(); 
    }
    
})();

async function cambiolista() {
   
    var sele = $("#field_149").val();
    //alert(sele);
    
    //var $ListDepend = $('.divFieldsDepend' + $select.dataset.idfieldsudf);

  /*  if ($ListDepend.length != 0) {*/
        var $option = '<option value="">--Seleccionar--</option>';
        /*if ($select.value != "") {*/
            var params = new Object();
            params['ParentIdDisposi'] = sele;
            var Result = await $.ajax({
                type: "post",
                url: '/CreateWorkOrder/GetListDispositions',
                data: params
            });
            $.each(Result, function (i, data) {
                $option += '<option value="' + data.IdFieldsDispositions + '">' + data.Dispositions + '</option>';
            });

   /* console.log($option);*/

    $('#field_150').html($option);
    
    //var $ListDepend = $('divFieldsDepend');
    //    //}
    //    ;
    //    for (i = 0; i < $ListDepend.length; i++) {
    //        $('#field_150').html($option);
    //    }
    //}
}

async function onChangeList($select) {
    /* alert($select.value);*/
    var cmb_AjustePospago = $("#field_804").val();
    if (cmb_AjustePospago != 0) {
        BtnGuardar_S.disabled = false;
    }
    BtnGuardar_S.disabled = true;
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
            $.each(Result, function (i, data) {
                $option += '<option value="' + data.IdFieldsDispositions + '">' + data.Dispositions + '</option>';
            });
        }
        for (i = 0; i < $ListDepend.length; i++) {
            $('#' + $ListDepend[i].id).html($option);
        }
    }

    //

    //planitlla pqr movil escrita
    var cmb_AjusteMovil = $("#field_150").val();

    
    
    var cmb_AjusteMovil2 = $("#field_149").val();// ocultar campo ajuste cuando en favorabilidad se escoge la opcion caso no favorable
    if (cmb_AjusteMovil2 == '4502') { //NO FAVORABLE
        
        $("#div_150").css({ 'display': 'none' });
        ocultar_ascard();
        ocultar_cuotasascard();
        ocultar_eliminacioncentrales();
        ocultar_postpago();
        ocultar_prepago();
        $("#field_150").removeClass('requiredForever');
        $("#field_727").removeClass('requiredForever');

        BtnGuardar_S.disabled = false; //boton guardar desactivado si caso no favorable
    } else {
        $("#div_150").css({ 'display': '' });
        BtnGuardar_S.disabled = true;
    }

    //else if (cmb_AjusteMovil != '4570' || cmb_AjusteMovil != '4565') {
    //    $("#btnEliminacionCentrales").hide(); // eliminacion de centrales
    //    BtnGuardar_S.disabled = false;
    //}

    
    var cmb_AjusteMovil1 = $("#field_150").val();
    var cmb_Ajuste = $("#field_150").val();
    var cmb_Favorabilidad = $("#field_149").val();
    
    if ($("#field_149").val() != 0 || $("#field_149").val() != 0) {
        BtnGuardar_S.disabled = false;
    }

    if (cmb_AjusteMovil1 == '4505' || cmb_AjusteMovil1 == '4510') {
        swal({
            title: '¡Alerta automatica!',
            text: 'Recuerda hacer actualizacion o modificacion de centrales',
        })
        BtnGuardar_S.disabled = false;
    }
    if (cmb_AjusteMovil1 == '4512' || cmb_AjusteMovil1 == '4507') {
        BtnGuardar_S.disabled = false;
    }

    if (cmb_AjusteMovil === '4504' || cmb_AjusteMovil === '4503' || cmb_AjusteMovil === '4509' || cmb_AjusteMovil === '4508') {
        $("#Acard1").show(); //boton ascard
        $("#BtBPB").show(); //boton postpago
        $("#btncuotaAcard").show(); // boton cuotas ascard
        $("#btnprepago").show(); //btn prepago
        $("#btnEliminacionCentrales").hide(); // eliminacion de centrales

        $("#BtnGuardar_S").attr('disabled', true);
        BtnGuardar_S.disabled = true;
        //alert('boton desactivar');
    } else {
        $("#Acard1").hide(); //boton ascard
        $("#BtBPB").hide(); //boton postpago
        $("#btncuotaAcard").hide(); // boton cuotas ascard
        $("#btnprepago").hide(); //btn prepago
        $("#btnEliminacionCentrales").hide(); // eliminacion de centrales

        BtnGuardar_S.disabled = false;
    }
    if ($("#field_804").val() != 0 || $("#field_806").val() != 0) {
        BtnGuardar_S.disabled = false;
    }

    if (cmb_AjusteMovil === '4506' || cmb_AjusteMovil === '4511') {
        //alert("entraste a eliminacion de centrales")
        $("#btnEliminacionCentrales").show(); // eliminacion de centrales
        BtnGuardar_S.disabled = true;
    }


    // llenar valor ajuste de ascard cuando se cambia la descripcion motivo de ajuste
    var cmb_motivoAjuste = $("#field_733").val();
    if (cmb_motivoAjuste === '4237') {
        $("#field_812").val('8');
    } else if (cmb_motivoAjuste === '4241') {
        $("#field_812").val('25');
    } else if (cmb_motivoAjuste === '4242') {
        $("#field_812").val('26');
    } else if (cmb_motivoAjuste === '4243') {
        $("#field_812").val('27');
    } else if (cmb_motivoAjuste === '4388') {
        $("#field_812").val('61');
    } else if (cmb_motivoAjuste === '4259') {
        $("#field_812").val('85');
    } else if (cmb_motivoAjuste === '4260') {
        $("#field_812").val('86');
    } else if (cmb_motivoAjuste === '4240') {
        $("#field_812").val('15');
    } else if (cmb_motivoAjuste === '4254') {
        $("#field_812").val('38');
    } else if (cmb_motivoAjuste === '4252') {
        $("#field_812").val('36');
    } else if (cmb_motivoAjuste === '4253') {
        $("#field_812").val('37');
    } else if (cmb_motivoAjuste === '4247') {
        $("#field_812").val('31');
    } else if (cmb_motivoAjuste === '4250') {
        $("#field_812").val('34');
    } else if (cmb_motivoAjuste === '4251') {
        $("#field_812").val('35');
    } else if (cmb_motivoAjuste === '4248') {
        $("#field_812").val('32');
    } else if (cmb_motivoAjuste === '4256') {
        $("#field_812").val('77');
    } else if (cmb_motivoAjuste === '4257') {
        $("#field_812").val('81');
    } else if (cmb_motivoAjuste === '4258') {
        $("#field_812").val('82');
    } else if (cmb_motivoAjuste === '4244') {
        $("#field_812").val('28');
    } else if (cmb_motivoAjuste === '4245') {
        $("#field_812").val('29');
    } else if (cmb_motivoAjuste === '4246') {
        $("#field_812").val('30');
    } else if (cmb_motivoAjuste === '4249') {
        $("#field_812").val('33');
    } else if (cmb_motivoAjuste === '4234') {
        $("#field_812").val('4');
    } else if (cmb_motivoAjuste === '4236') {
        $("#field_812").val('6');
    } else if (cmb_motivoAjuste === '4235') {
        $("#field_812").val('5');
    } else if (cmb_motivoAjuste === '4255') {
        $("#field_812").val('47');
    } else if (cmb_motivoAjuste === '4238') {
        $("#field_812").val('9');
    } else if (cmb_motivoAjuste === '4239') {
        $("#field_812").val('10');
    }
}

async function OcultarcamposplanillaMovil($select) {
    $("#lbl_780").hide();
    $("#field_780").hide();
    $("#field_780").removeClass('form-control Req-2 fieldSave requiredForever');
    $("#lbl_781").hide();
    $("#field_781").hide();
    $("#field_781").removeClass('form-control Req-2 fieldSave requiredForever');
    $("#lbl_782").hide();
    $("#field_782").hide();
    $("#field_782").removeClass('form-control Req-2 fieldSave requiredForever');
    $("#lbl_783").hide();
    $("#field_783").hide();
    $("#field_783").removeClass('form-control Req-2 fieldSave requiredForever');
    $("#lbl_784").hide();
    $("#field_784").hide();
    $("#field_784").removeClass('form-control Req-2 fieldSave requiredForever');
    $("#lbl_785").hide();
    $("#field_785").hide();
    $("#field_785").removeClass('form-control Req-2 fieldSave requiredForever');
    $("#lbl_786").hide();
    $("#field_786").hide();
    $("#field_786").removeClass('form-control Req-2 fieldSave requiredForever');
    $("#lbl_787").hide();
    $("#field_787").hide();
    $("#field_787").removeClass('form-control Req-2 fieldSave requiredForever');
    $("#lbl_789").hide();
    $("#field_789").hide();
    $("#field_789").removeClass('form-control Req-2 fieldSave requiredForever');


    
}
async function changeEstados(value)
{
    var $option = '<option value="">--Seleccionar--</option>';
    if (value != "")
    {
        var params = new Object();
        params["IdStatus"] = value;
        var result = await $.ajax({
            type: "post",
            url: '/WorkOrderSolutions/GetSubStatus',
            data: params
        });
        if (result.TypeAction.IdTypeActions == 3)/*ESCALAMIENTOS*/ {
            $('.divScaled').show();
            $('.divScaled .form-control').prop('disabled', false);
        } else {
            $('.divScaled').hide();
            $('.divScaled .form-control').prop('disabled', true).val('');
            $('.divScaled .form-control').change();
            if (result.TypeAction.IdTypeActions == 5) /*Con respuesta*/ {
                $('#divCommentsScaled').show();
                $('#TxtCommentsScaled').prop('disabled', false);
            }
        }
        $('.lblrequi-').text('');
        $('.required').removeClass('required');
        $('.lblReq-' + result.TypeAction.IdTypeRequired).text('*');
        $('.Req-' + result.TypeAction.IdTypeRequired).removeClass('required').addClass('required');
        var SubStatus = result.SubStatus;
        $.each(SubStatus, function (i, data) {
            $option += '<option value="' + data.IdStatusDefinition + '">' + data.NameStatus + '</option>';
        });
    }
    $('#DdlSubStatus').html($option);

    if ((value == '228') || (value == '229')) {
        $("#field_727").removeClass('requiredForever');
        $("#field_149").removeClass('requiredForever');
        $("#field_151").removeClass('requiredForever');
        BtnGuardar_S.disabled = true;
    } else {
        //$("#field_727").addClass('form-control Req-2 fieldSave requiredForever');
        //$("#field_149").addClass('form-control Req-2 fieldSave requiredForever');
        //$("#field_151").addClass('form-control Req-2 fieldSave requiredForever');
        BtnGuardar_S.disabled = false;
    }

}
async function changeTypesScaled(value)
{
    var $option = '<option value="">--Seleccionar--</option>';
    var mensaje = "";
    var btnDisabled = false;
    if (value != "") {
        var params = new Object();
        params['IdGroups'] = value;
        var Result = await $.ajax({
            type: "post",
            url: '/WorkOrderSolutions/GetTypesScaled',
            data: params
        });
        var NumUsers = Result.UsersXGroups.length;
        if (NumUsers == 0) {
            mensaje = "Este grupo no cuenta con usuarios disponibles para asignar.";
            btnDisabled = true;
        }
        $.each(Result.TypesScaled, function (i, data) {
            $option += '<option value="' + data + '">' + data + '</option>';;
        });
    }
    $('#BtnGuardar').prop('disabled', btnDisabled);
    $('#DdlGroupScale-error').text(mensaje);
    $('#DdlTypeScaled').html($option);
}
async function getListPlataformas($select) {
    let $option = '<option value="">--Seleccionar--</option>'
    if ($select.value != "") {
        let params = new Object()
        params['IdLocation'] = $select.value
        let Result = await $.ajax({
            type: "post",
            url: '/CreateWorkOrder/ListPlatform',
            data: params
        })
        for (const data of Result) {
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
async function changeAgregarAdjunto() {
    if ($BtnGuardar != undefined) {
        try {
            FuncBtnLoader($BtnGuardar, 1);
            var file = document.getElementById("FileAddAttachments").files[0];
            var formData = new FormData();
            formData.append("Adjunto", file);
            var ResultAdjunt = await $.ajax({
                type: "post",
                url: '/WorkOrderSolutions/AddAttachments',
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
}

async function deleteAdjunto(NameEncrypted) {
    try {
        var params = new Object();
        params["IdWorkOrder"] = $('#formCreate')[0].dataset.idworkorder;
        params['NameEncrypted'] = NameEncrypted;
        var Result = await $.ajax({
            type: "post",
            url: '/WorkOrderSolutions/DeleteAttachment',
            data: params
        });
        $('#' + NameEncrypted).remove();
        toastr.success('Adjunto eliminado correctamente.', 'Adjunto eliminado');
    } catch (ex) {
        swal("Error interno", "Comunicate con el area de IT para la verificacion de este error..", "error");
    }
}


// Negacion de linea 
// 08/2021

async function SaveNegacion($Btn) {
    try {
        ToolsFuncBtnLoader($Btn, 1);

        /*
        let $elementRequired = $('.required');
        let ValidaInput = ValidarVacios($elementRequired);
        if (ValidaInput == false) {
            toastr.error('Los campos no pueden estar vacios o con errores.', 'Campos obligatorios');
            return
        }
        */
        let params = new Object();


        if ($('#IdWorkOrderh2').val() === '') {
            swal("Error", "Ingrese el campo id del caso", "error");
            return;
        }

        if ($('#txtBase').val() === '') {
            swal("Error", "Ingrese el campo Base", "error");
            return;
        }

        if ($('#txtImagen').val() === '') {
            swal("Error", "Ingrese el campo Imagen", "error");
            return;
        }

        if ($('#txtMin').val() === '') {
            swal("Error", "Ingrese el campo MIN", "error");
            return;
        }


        if ($('#txtFechaActivacion').val() === '') {
            swal("Error", "Ingrese el campo   FechaActivacion", "error");
            return;
        }

        if ($('#txtCurcode').val() === '') {
            swal("Error", "Ingrese el campo  Curcode", "error");
            return;
        }

        if ($('#txtNombre').val() === '') {
            swal("Error", "Ingrese el campo Nombre", "error");
            return;
        }

        if ($('#txtApellido').val() === '') {
            swal("Error", "Ingrese el campo  Apellido", "error");
            return;
        }

        if ($('#txtCanal').val() === '') {
            swal("Error", "Ingrese el campo  Canal", "error");
            return;
        }

        if ($('#txtAscard').val() === '') {
            swal("Error", "Ingrese el campo Ascard", "error");
            return;
        }

        if ($('#TxtFechaReposicion').val() === '') {
            swal("Error", "Ingrese el campo  Fecha Reposicion", "error");
            return;
        }

        if ($('#txtContrato').val() === '') {
            swal("Error", "Ingrese el campo Contrato", "error");
            return;
        }

        if ($('#txtGrabacion').val() === '') {
            swal("Error", "Ingrese el campo  Grabacion", "error");
            return;
        }

        if ($('#TxtReasignacion').val() === '') {
            swal("Error", "Ingrese el campo  Reasignacion", "error");
            return;
        }

        if ($('#txtEstado').val() === '') {
            swal("Error", "Ingrese el campo  Estado", "error");
            return;
        }


        if ($('#txtLocalizado').val() === '') {
            swal("Error", "Ingrese el campo   Localizado", "error");
            return;
        }
        if ($('#txtObservaciones').val() === '') {
            swal("Error", "Ingrese el campo  Observaciones", "error");
            return;
        }

        if ($('#txtRangoProbable').val() === '') {
            swal("Error", "Ingrese el campo   RangoProbable", "error");
            return;
        }

        if ($('#txtDireccion').val() === '') {
            swal("Error", "Ingrese el campo Direccion", "error");
            return;
        }

        if ($('#txtCustomerId').val() === '') {
            swal("Error", "Ingrese el campo  CustomerId", "error");
            return;
        }

        if ($('#txtCiudad').val() === '') {
            swal("Error", "Ingrese el campo Ciudad", "error");
            return;
        }

        if ($('#txtDepartamento').val() === '') {
            swal("Error", "Ingrese el campo Departamento", "error");
            return;
        }

        if ($('#txtFechaRadicacion').val() === '') {
            swal("Error", "Ingrese el campo FechaRadicacion", "error");
            return;
        }

        if ($('#txtNotificacion').val() === '') {
            swal("Error", "Ingrese el campo Notificacion", "error");
            return;
        }

        if ($('#txtFechaDesactivacion').val() === '') {
            swal("Error", "Ingrese el campo FechaDesactivacion", "error");
            return;
        }

        if ($('#txtPqr').val() === '') {
            swal("Error", "Ingrese el campo Pqr", "error");
            return;
        }

        if ($('#txtCedula').val() === '') {
            swal("Error", "Ingrese el campo Cedula", "error");
            return;
        }

        if ($('#txtAreaRadicacion').val() === '') {
            swal("Error", "Ingrese el campo AreaRadicacion", "error");
            return;
        }

        if ($('#txtTipoReclamo').val() === '') {
            swal("Error", "Ingrese el campo TipoReclamo", "error");
            return;
        }

        params['IdWorkOrder'] = $('#IdWorkOrderh2').val();
        params['Base'] = $('#txtBase').val();
        params['Imagen'] = $('#txtImagen').val();
        params['MIN'] = $('#txtMin').val();


        params['FechaActivacion'] = $('#txtFechaActivacion').val();
        params['Curcode'] = $('#txtCurcode').val();
        params['Nombre'] = $('#txtNombre').val();
        params['Apellido'] = $('#txtApellido').val();
        params['Canal'] = $('#txtCanal').val();
        params['Ascard'] = $('#txtAscard').val();
        params['FechaReposicion'] = $('#TxtFechaReposicion').val();
        params['Contrato'] = $('#txtContrato').val();
        params['Grabacion'] = $('#txtGrabacion').val();
        params['Reasignacion'] = $('#TxtReasignacion').val();
        params['Estado'] = $('#txtEstado').val();
        params['Legalizado'] = $('#txtLegalizado').val();
        params['Observaciones'] = $('#txtObservaciones').val();
        params['RangoProbable'] = $('#txtRangoProbable').val();
        params['DireccionInformaCliente'] = $('#txtDireccion').val();
        params['CustomerID'] = $('#txtCustomerId').val();
        params['Ciudad'] = $('#txtCiudad').val();
        params['Departamento'] = $('#txtDepartamento').val();
        params['FechaRadicacion'] = $('#txtFechaRadicacion').val();
        params['Notificacion'] = $('#txtNotificacion').val(); 
        params['FechaDesactivacion'] = $('#txtFechaDesactivacion').val(); 
        params['PQR'] = $('#txtPqr').val(); 
        params['Cedula'] = $('#txtCedula').val(); 
        params['AreaRadica'] = $('#txtAreaRadica').val();
        params['TipoReclamo'] = $('#txtTipoReclamo').val();

        let Result = await $.ajax({
            type: "post",
            url: '/WorkOrderSolutions/GuardarNegacionLinea_Alistamiento',
            data: params,
        });
        swal("Bien Hecho!", "Acción realizada correctamente.", "success");
        //await ListarDatosNegacionLinea();
       // $('#myModalNegacion').modal('hide');
        location.reload(); 
    } catch (ex) {
        swal("Error Interno ", "Comunicate con el area de IT para la verificacion de este error..", "error");
    } finally {

       // $('#myModalNegacion').modal('hide');
        //ToolsFuncBtnLoader($Btn, 0);

    }
}

async function ListarDatosNegacionLinea() {
    let param = new Object();
    param['IdWorkOrder'] = $('#h_IdWorkOrder').val();

    let Result = await $.ajax({
        //contentType: 'application/json; charset=utf-8',
        type: "post",
        url: '/WorkOrderSolutions/NegacionLinea_Alistamiento',
        data: param,
    });
    $("#TableGeneral").html(Result);
    // await ToolsDataTables();
}

function GenerarExcelNegacionLineaPendiente(event) {

    var h_IdWorkOrder;
    h_IdWorkOrder = $('#h_IdWorkOrder').val();

    event.preventDefault();

    location.href = "/WorkOrderSolutions/NegacionLinea_AlistamientoExcelPendiente?IdWorkOrder=" + h_IdWorkOrder;
}

function GenerarExcelNegacionLinea(event) {
    var h_IdWorkOrder;
    h_IdWorkOrder = $('#h_IdWorkOrder').val();
    event.preventDefault();
    location.href = "/WorkOrderSolutions/NegacionLinea_Alistamiento_ExcelTodo?IdWorkOrder=" + h_IdWorkOrder;
}

  function DeletelineaNegacion2(Id) {
      try {
          swal({
              title: "¿Quieres continuar?",
              text: "¿Deseas eliminar este Ítem?",
              type: "warning",
              showCancelButton: true,
              confirmButtonText: "Si, Eliminar!",
              closeOnConfirm: true,
              showLoaderOnConfirm: true
          }, async function () {
              let params = new Object();
              params['Id'] = Id;
              let Result = await $.ajax({
                  type: "post",
                  url: '/WorkOrderSolutions/EliminarNegacionLinea_Alistamiento',
                  data: params
              });
                 // await ListarDatosNegacionLinea();
                  location.reload(); 
              toastr.success('<b> El Ítem </b> fue eliminado correctamente.', 'Eliminado');
          });
      } catch (ex) {
          swal("Error interno", "Comunicate con el area de IT para la verificacion de este error..", "error");
      }
 

}

async function CargarExcelNegacionLinea() {
    try {
        var h_IdWorkOrder;
        h_IdWorkOrder = $('#h_IdWorkOrder').val();


        var file = document.getElementById('ArchivoExcel').files;
        var formData = new FormData();
        formData.append("ArchivoExcel", file);
        formData.append("IdWorkOrder", h_IdWorkOrder);

        let Result = await $.ajax({
            type: "post",
            url: '/WorkOrderSolutions/CargarExcelNegacionLinea',
            processData: false,
            contentType: false,
            cache: false,
            dataType: 'json',
            data: formData
        });
 
        swal({ title: "Bien Hecho!", text: "Accion realizada correctamente.", type: "success" }, function () {
            location.reload();
        });
    } catch (ex) {
     
        swal("Error interno", "Comunicate con el area de IT para la verificacion de este error..", "error");
    }
    finally {
        //await AtachmentMiss();
    }
}

function checkNumber(e) {
    tecla = (document.all) ? e.keyCode : e.which;
    //Tecla de retroceso para borrar, siempre la permite
    if (tecla == 8) {
        return true;
    }
    // Patron de entrada, en este caso solo acepta numeros 
    patron = /^[0-9]+/;
    tecla_final = String.fromCharCode(tecla);
    return patron.test(tecla_final);
}
var flag = 0;
var control = 0;

async function showAdjustScreen1() { // postpago
    
    if (flag == 0) {
        swal("Activacion de Ajuste BPB!", "Se debe diligenciar el campo de Ajustes en SI para realizar mas de un ajuste", "success");
        BtnGuardar_S.disabled = false;

        swal({
            title: '¡Alerta automatica!',
            text: 'Todos los campos son obligatorios.',
        })

        $("#BtnPrepago").hide();
        $("#BtnPospago").show();
        $("#BtnAscard").hide();
        $("#BtnCuotasAscard").hide();
        $("#BtnEliminacionCentrales").hide();
        $("#divTableprepago").hide();
        $("#divTableascard1").hide();
        $("#divTablepospago").show();
        $("#divTableascard2").hide();
        $("#divTableeliminar").hide();

        // AGREGADOS NUEVOS
        $("#div_797").show(); // analista postpago
        $("#field_797").val().toUpperCase();
        $("#field_797").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_798").show(); // user red postpago
        $("#field_798").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_768").show(); // cun postpago
        $("#field_768").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_799").show(); //tipo reclamo postpago
        $("#field_799").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_767").show(); // min postpago
        $("#field_767").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_769").show(); // custcode postpago
        $("#field_769").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_770").show(); // customerid postpago
        $("#field_770").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_800").show(); //servicio 800 postpago
        $("#field_800").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_801").show(); //cta contable 801 postpago
        $("#field_801").addClass('form-control Req-2 fieldSave requiredForever');
        field_801.disabled = true;
        $("#div_802").show(); //iva 802 postpago
        $("#field_802").addClass('form-control Req-2 fieldSave requiredForever');
        field_802.disabled = true; // iva
        $("#div_727").show(); // valor postpago
        document.getElementById('field_727').type = 'number';
        $("#field_727").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_729").show(); // periodo a ajustar postpago
        $("#field_729").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_730").show(); // periodo ajustar hasta postpago
        $("#field_730").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_728").show(); // justificacion postpago
        $("#field_728").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_858").show(); //area que genero inconsistencia postpago
        $("#field_858").addClass('form-control Req-2 fieldSave requiredForever');
        if (document.getElementById('field_858').value == "") {
            $("#field_858").val('AIB PQR ESCRITA');
        }
        $("#div_803").show(); //gerencia 803 postpago
        $("#field_803").addClass('form-control Req-2 fieldSave requiredForever');
        if (document.getElementById('field_803').value == "") {
            $("#field_803").val('GERENCIA GESTION PQRs');
        }
        $("#div_859").show(); //user responsable postpago
        $("#field_859").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_804").show(); //centrales 804 postpago
        $("#field_804").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_805").show(); //aliado 805 postpago
        $("#field_805").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_806").show(); //causal 806 postpago
        $("#field_806").addClass('form-control Req-2 fieldSave requiredForever');
        $("#field_807").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_726").css({ 'display': 'none' }); // segmento
        $("#div_807").css({ 'display': '' }); //estado 807 postpago
        if ($("#field_807").val() != "APLICADO" && $("#field_807").val() != "RECHAZADO" && $("#field_807").val() != "Aplicado" && $("#field_807").val() != "Rechazado") {
            $("#field_807").val("Pte aplicar ajuste");
        }
        $("#field_807").attr('readonly', true);
        $("#div_810").css({ 'display': '' }); //fecha creacion 808
        $("#div_811").css({ 'display': '' }); //fecha ultima actualizacion 809 postpago
        $("#botonexport").show(); //boton exportar postpago
        var date = new Date();
        var fecha = date.getFullYear() + '/' + (date.getMonth() + 1) + '/' + date.getDate();
        var hora = date.getHours() + ':' + date.getMinutes() + ':' + date.getSeconds();
        var fechaYHora = fecha + ' ' + hora;
        $("#field_811").val(fechaYHora);
        $('#field_810').val(fechaYHora); //actualizacion anterior
        //$("#lbl_807").hide();
        //$("#field_807").hide();
        $("#field_807").attr('readonly', true);
        $("#lbl_810").hide();
        $("#field_810").hide();
        $("#lbl_811").hide();
        $("#field_811").hide();
        $("#div_810").hide();
        $("#div_811").hide();

        $("#field_828").val(fechaYHora); //actualizacion anterior validar campo de CUOTAS
        $("#field_829").val(fechaYHora); // ultima actualizacion validar campo de CUOTAS

        restaurar_colores_ascard();
        restaurar_colores_centrales();
        restaurar_colores_cuotas();
        restaurar_colores_prepago();


        flag = 1;
        control = control + 1;
    }
    else 
    {
        swal("Desactivacion de Ajuste BPB!", "Se debe diligenciar el campo de Ajuste en NO", "success");
        //$("#lbl_725").show();
        //$("#field_725").show();
        //$("#field_725").removeClass('form-control Req-2 fieldSave requiredForever');
        //$("#field_725").addClass('form-control Req-1 fieldSave');
        $("#botonexport").hide(); //boton exportar postpago

        ocultar_postpago();

        restaurar_colores_ascard();
        restaurar_colores_centrales();
        restaurar_colores_cuotas();
        restaurar_colores_prepago();

        flag = 0;
        control = control - 1;
    }
    
}//postpago         OK
async function showAdjustScreen2() { // ascard
    
    if (flag == 0) {
        swal("Activacion de Ajuste Ascard!", "Se debe diligenciar el campo de Ajustes en SI para realizar mas de un ajuste", "success");
        BtnGuardar_S.disabled = false;


        swal({
            title: '¡Alerta automatica!',
            text: 'Todos los campos son obligatorios.',
        })


        $("#BtnPrepago").hide();
        $("#BtnPospago").hide();
        $("#BtnAscard").show();
        $("#BtnCuotasAscard").hide();
        $("#BtnEliminacionCentrales").hide();
        $("#divTableprepago").hide();
        $("#divTableascard1").show();
        $("#divTablepospago").hide();
        $("#divTableascard2").hide();
        $("#divTableeliminar").hide();

        $("#field_727").removeClass(' requiredForever');
        $("#div_727").css({ 'display': 'none' }); // valor postpago
        //agregados
        $("#div_731").show();//numero de credito 731 ascard
        $("#field_731").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_732").show();//valor del ajuste 732 ascard
        $("#field_732").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_812").show();//motivo del ajuste 810 ascard
        $("#field_812").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_733").show();//descripcion motivo del ajuste 733 ascard
        $("#field_733").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_734").show();//area que solicita el ajuste 734 ascard
        $("#field_734").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_735").show();//comentari0 735 ascard
        $("#field_735").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_813").show();//nombre quien solicita 811 ascard
        $("#field_813").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_736").show();//documento del usuario 736 ascard
        $("#field_736").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_737").show();//reclamo del usuario 737 ascard
        $("#field_737").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_739").show();//custcode asociado al credito 739 ascard
        $("#field_739").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_819").show();//cun/nr 812 ascard
        $("#field_819").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_820").show();//tipo de reclamo ascard
        $("#field_820").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_821").show();//aliado 814 ascard
        $("#field_821").addClass('form-control Req-2 fieldSave requiredForever');
        $("#field_822").addClass('form-control Req-2 fieldSave requiredForever');
        if ($("#field_822").val() != "APLICADO" && $("#field_822").val() != "RECHAZADO" && $("#field_822").val() != "Aplicado" && $("#field_822").val() != "Rechazado") {
            $("#field_822").val("Pte aplicar ajuste");
        }
        $("#field_822").attr('readonly', true);

        var date = new Date();
        var fecha = date.getFullYear() + '/' + (date.getMonth() + 1) + '/' + date.getDate();
        var hora = date.getHours() + ':' + date.getMinutes() + ':' + date.getSeconds();
        var fechaYHora = fecha + ' ' + hora;
        $("#field_825").val(fechaYHora);

        $('#field_824').val(fechaYHora); // ultima actualizacion
         //actualizacion anterior
        //$("#div_815").hide();//estado 815 ascard
        $("#div_822").show();//estado
        $("#div_824").hide();//actualizacin anterior 816 ascard
        $("#div_825").hide();//ultima actualizacion 817 ascard

        $("#Exportarascard").show(); //boton exportar ascard

        restaurar_colores_centrales();
        restaurar_colores_cuotas();
        restaurar_colores_postpago();
        restaurar_colores_prepago();

        flag = 1;
        control = control + 1;
    }
    else {
        swal("Desactivacion de Ajuste Ascard!", "Se debe diligenciar el campo de Ajuste en NO", "success");
        //$("#lbl_725").show();
        //$("#field_725").show();
        //$("#field_725").removeClass('form-control Req-2 fieldSave requiredForever');
        //$("#field_725").addClass('form-control Req-1 fieldSave');

        ocultar_ascard();

        $("#Exportarascard").hide(); //boton exportar ascard

        restaurar_colores_centrales();
        restaurar_colores_cuotas();
        restaurar_colores_postpago();
        restaurar_colores_prepago();

        flag = 0;
        control = control - 1;
    }

}//ascard           OK
async function showAdjustScreen3() {// cuotas ascard    OK
    
    if (flag == 0)
    {
        swal("Activacion de Ajuste Ascard 2!", "Se debe diligenciar el campo de Ajustes en SI para realizar mas de un ajuste", "success");
        BtnGuardar_S.disabled = false;


        swal({
            title: '¡Alerta automatica!',
            text: 'Todos los campos son obligatorios.',
        })

        $("#BtnPrepago").hide();
        $("#BtnPospago").hide();
        $("#BtnAscard").hide();
        $("#BtnCuotasAscard").show();
        $("#BtnEliminacionCentrales").hide();
        $("#divTableprepago").hide();
        $("#divTableascard1").hide();
        $("#divTablepospago").hide();
        $("#divTableascard2").show();
        $("#divTableeliminar").hide();

        $("#field_727").removeClass(' requiredForever');
        $("#div_727").css({ 'display': 'none' }); // valor postpago
        //agregados
        $("#div_740").show();//numero de credito 740 cuotas ascard
        $("#field_740").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_741").show();//valor nueva cuota 741 cuotas ascard
        $("#field_741").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_742").show();//concepto 742 cuotas ascard cuotas ascard
        $("#field_742").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_743").show();//area que solicita ajuste 743 cuotas ascard
        $("#field_743").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_744").show();//reclamo del usuario 744 cuotas ascard
        $("#field_744").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_745").show();//cantidad de cuotas 745 cuotas ascard
        $("#field_745").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_826").show();//aliado 818 cuotas ascard
        $("#field_826").addClass('form-control Req-2 fieldSave requiredForever');
        $("#field_827").addClass('form-control Req-2 fieldSave requiredForever');
        if ($("#field_827").val() != "APLICADO" && $("#field_827").val() != "RECHAZADO" && $("#field_827").val() != "Aplicado" && $("#field_827").val() != "Rechazado") {
            $("#field_827").val("Pte aplicar ajuste");
        }
        $("#field_727").removeClass('requiredForever');
        $("#div_827").show();//estado 819 cuotas ascard
        $("#field_827").attr('readonly', true);
        var date = new Date();
        var fecha = date.getFullYear() + '/' + (date.getMonth() + 1) + '/' + date.getDate();
        var hora = date.getHours() + ':' + date.getMinutes() + ':' + date.getSeconds();
        var fechaYHora = fecha+ ' ' + hora;
        //console.log(fechaYHora);
        //console.log($("#field_820").val("2022-06-01 00:00:00.000"))
        //console.log($("#field_821").val("2022-06-01 00:00:00.000"))

        $("#field_828").addClass('form-control Req-2 fieldSave requiredForever');
        $("#field_828").val(fechaYHora); //actualizacion anterior
        $("#field_829").val(fechaYHora); // ultima actualizacion
        $("#div_828").hide();//actualizacion anteriro 820 cuotas ascard
        $("#div_829").hide();//ultima acxtualizacion 821 cuotas ascard
        $("#div_827").show();//actualizacion anteriro 820 cuotas ascard
        $("#Exportarcuotas").show(); //boton exportar cuotas

        restaurar_colores_ascard();
        restaurar_colores_centrales();
        restaurar_colores_postpago();
        restaurar_colores_prepago();

        flag = 1;
        control = control + 1;
    }
    else
    {
        swal("Desactivacion de Ajuste Ascard 2!", "Se debe diligenciar el campo de Ajuste en NO", "success");
        //$("#lbl_725").show();
        //$("#field_725").show();
        //$("#field_725").removeClass('form-control Req-2 fieldSave requiredForever');
        //$("#field_725").addClass('form-control Req-1 fieldSave');

        $("#Exportarcuotas").hide(); //boton exportar cuotas
        ocultar_cuotasascard();

        restaurar_colores_ascard();
        restaurar_colores_centrales();
        restaurar_colores_postpago();
        restaurar_colores_prepago();

        flag = 0;
        control = control - 1;

    }
}//cuotas ascard    OK
async function showAdjustScreen4() // prepago
{   
    
    if (flag == 0) {
        swal("Activacion de Ajuste Incidencias PQR!", "Se debe diligenciar el campo de Ajustes en SI para realizar mas de un ajuste", "success");
        BtnGuardar_S.disabled = false;

        swal({
            title: '¡Alerta automatica!',
            text: 'Todos los campos son obligatorios.',
        })

        $("#BtnPrepago").show();
        $("#BtnPospago").hide();
        $("#BtnAscard").hide();
        $("#BtnCuotasAscard").hide();
        $("#BtnEliminacionCentrales").hide();
        $("#divTableprepago").show();
        $("#divTableascard1").hide();
        $("#divTablepospago").hide();
        $("#divTableascard2").hide();
        $("#divTableeliminar").hide();

        $("#field_727").removeClass(' requiredForever');

        $("#div_727").css({ 'display': 'none' }); // valor prepago

        //AGREGADOS

        $("#div_830").show();//radicado prepago
        $("#field_830").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_831").show();//tipo de reclamo prepago
        $("#field_831").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_832").show();//nombre de titular prepago
        $("#field_832").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_833").show();//min prepago
        $("#field_833").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_834").show();//custcode prepago
        $("#field_834").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_835").show();//valor prepago
        $("#field_835").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_836").show();//concepto prepago
        $("#field_836").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_837").show();//analista prepago
        $("#field_837").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_838").show();//periodo ajustado desde prepago
        $("#field_838").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_839").show();//periodo ajustado hasta prepago
        $("#field_839").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_840").show();//aliado prepago
        $("#field_840").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_855").show();//ESTADO prepago
        $("#field_855").addClass('form-control Req-2 fieldSave requiredForever');
        if ($("#field_855").val() != "APLICADO" && $("#field_855").val() != "RECHAZADO" && $("#field_855").val() != "Aplicado" && $("#field_855").val() != "Rechazado") {
            $("#field_855").val("Pte aplicar ajuste");
        }
        $("#field_855").attr('readonly', true);

        var date = new Date();
        var fecha = date.getFullYear() + '/' + (date.getMonth() + 1) + '/' + date.getDate();
        var hora = date.getHours() + ':' + date.getMinutes() + ':' + date.getSeconds();
        var fechaYHora = fecha + ' ' + hora;
        $("#field_857").val(fechaYHora); // ultima actualizacion
        $("#field_856").val(fechaYHora); //actualizacion anterior
        
        $("#field_856").addClass('form-control Req-2 fieldSave requiredForever');
        $("#field_857").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_855").css({ 'display': '' });//estado prepago cubo
        $("#div_856").css({ 'display': 'none' });//actualizacion anterior prepago
        $("#div_857").css({ 'display': 'none' });//ultima actualizacion prepago

        $("#Exportarprepago").show(); //boton exportar prepago

        restaurar_colores_ascard();
        restaurar_colores_centrales();
        restaurar_colores_cuotas();
        restaurar_colores_postpago();

        flag = 1;
        control = control + 1;
    }
    else
    {
        swal("Desactivacion de Ajuste Incidencias PQR!", "Se debe diligenciar el campo de Ajuste en NO", "success");
        //$("#lbl_725").show();
        //$("#field_725").show();
        //$("#field_725").removeClass('form-control Req-2 fieldSave requiredForever');
        //$("#field_725").addClass('form-control Req-1 fieldSave');

        $("#Exportarprepago").hide(); //boton exportar prepago
 
        ocultar_prepago();
        
        restaurar_colores_ascard();
        restaurar_colores_centrales();
        restaurar_colores_cuotas();
        restaurar_colores_postpago();

        flag = 0;
        control = control - 1;
    }
}//prepago          OK
async function showAdjustScreen5() {

    if (flag == 0) {
        var field_782 = document.getElementById('field_782');
        //swal("Activacion de Ajuste BPB!", "Se debe diligenciar el campo de Ajustes en SI para realizar mas de un ajuste", "success");

        $("#BtnPrepago").hide();
        $("#BtnPospago").hide();
        $("#BtnAscard").hide();
        $("#BtnCuotasAscard").hide();
        $("#BtnEliminacionCentrales").hide();
        $("#divTableprepago").hide();
        $("#divTableascard1").hide();
        $("#divTablepospago").hide();
        $("#divTableascard2").hide();
        $("#divTableeliminar").hide();

        $("#lbl_780").show();
        $("#field_780").show();
        $("#field_780").addClass('form-control Req-2 fieldSave requiredForever');       
        $("#lbl_781").show();
        $("#field_781").show();
        $("#field_781").addClass('form-control Req-2 fieldSave requiredForever');
        $("#lbl_782").show();
        $("#field_782").show();
        $("#field_782").addClass('form-control Req-2 fieldSave requiredForever');
        $("#field_782").val('DIME');
        field_782.disabled = true;
        $("#lbl_783").show();
        $("#field_783").show();
        $("#field_783").addClass('form-control Req-2 fieldSave requiredForever');
        $("#lbl_784").show();
        $("#field_784").show();
        $("#field_784").addClass('form-control Req-2 fieldSave requiredForever');
        $("#lbl_785").show();
        $("#field_785").show();
        $("#field_785").addClass('form-control Req-2 fieldSave requiredForever');
        $("#lbl_786").show();
        $("#field_786").show();
        $("#field_786").addClass('form-control Req-2 fieldSave requiredForever');
        $("#lbl_787").show();
        $("#field_787").show();
        $("#field_787").addClass('form-control Req-2 fieldSave requiredForever');
        $("#lbl_789").show();
        $("#field_789").show();
        $("#field_789").addClass('form-control Req-2 fieldSave requiredForever');
       
        flag = 1;
        control = control + 1;
    }
    else {
      
        $("#lbl_780").hide();
        $("#field_780").hide();
        $("#field_780").removeClass('form-control Req-2 fieldSave requiredForever');
        $("#lbl_781").hide();
        $("#field_781").hide();
        $("#field_781").removeClass('form-control Req-2 fieldSave requiredForever');
        $("#lbl_782").hide();
        $("#field_782").hide();
        $("#field_782").removeClass('form-control Req-2 fieldSave requiredForever');        
        $("#lbl_783").hide();
        $("#field_783").hide();
        $("#field_783").removeClass('form-control Req-2 fieldSave requiredForever');        
        $("#lbl_784").hide();
        $("#field_784").hide();
        $("#field_784").removeClass('form-control Req-2 fieldSave requiredForever');        
        $("#lbl_785").hide();
        $("#field_785").hide();
        $("#field_785").removeClass('form-control Req-2 fieldSave requiredForever');        
        $("#lbl_786").hide();
        $("#field_786").hide();
        $("#field_786").removeClass('form-control Req-2 fieldSave requiredForever');        
        $("#lbl_787").hide();
        $("#field_787").hide();
        $("#field_787").removeClass('form-control Req-2 fieldSave requiredForever');        
        $("#lbl_789").hide();
        $("#field_789").hide();
        $("#field_789").removeClass('form-control Req-2 fieldSave requiredForever');        
        //$("#lbl_726").hide();
        //$("#field_726").hide();
        //$("#field_726").removeClass('form-control Req-2 fieldSave requiredForever');
        //$("#field_726").addClass('form-control Req-1 fieldSave');
        //$("#lbl_728").hide();
        //$("#field_728").hide();
        //$("#field_728").removeClass('form-control Req-2 fieldSave requiredForever');
        //$("#field_728").addClass('form-control Req-1 fieldSave');
        //$("#lbl_729").hide();
        //$("#729").hide();
        //$("#field_729").hide();
        //$("#field_729").removeClass('form-control Req-2 fieldSave requiredForever');
        //$("#field_729").addClass('form-control Req-1 fieldSave');
        //$("#lbl_730").hide();
        //$("#730").hide();
        //$("#field_730").hide();
        //$("#field_730").removeClass('form-control Req-2 fieldSave requiredForever');
        //$("#field_730").addClass('form-control Req-1 fieldSave');
        //$("#lbl_767").hide();
        //$("#field_767").hide();
        //$("#lbl_768").hide();
        //$("#field_768").hide();
        //$("#lbl_771").hide();
        //$("#field_771").hide();
        //$("#lbl_772").hide();
        //$("#field_772").hide();
        flag = 0;
        control = control - 1;
    }

}
async function showAdjustScreen6() { // eliminacion de centrales

    if (flag == 0) {
        swal("Activacion de Eliminacion de Centrales", "Se debe diligenciar el campo de Ajustes en SI para realizar mas de un ajuste", "success");
        BtnGuardar_S.disabled = false;

        swal({
            title: '¡Alerta automatica!',
            text: 'Todos los campos son obligatorios.',
        })

        $("#BtnPrepago").hide();
        $("#BtnPospago").hide();
        $("#BtnAscard").hide();
        $("#BtnCuotasAscard").hide();
        $("#BtnEliminacionCentrales").show();
        $("#divTableprepago").hide();
        $("#divTableascard1").hide();
        $("#divTablepospago").hide();
        $("#divTableascard2").hide();
        $("#divTableeliminar").show();

        $("#field_727").removeClass(' requiredForever');
        $("#div_727").css({ 'display': 'none' }); // valor postpago

        //AGREGADOS
        $("#div_844").show();//cun centrales
        $("#field_844").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_845").show();//tipo de documento centrales
        $("#field_845").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_846").show();//nombrer centrales
        $("#field_846").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_847").show();//cedula centrales
        $("#field_847").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_848").show();//custcode No credito centrales
        $("#field_848").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_849").show();//estado centrales
        $("#field_849").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_850").show();//motivo de la elminacion centrales
        $("#field_850").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_851").show();//analista centrales
        $("#field_851").addClass('form-control Req-2 fieldSave requiredForever');
        $("#field_852").addClass('form-control Req-2 fieldSave requiredForever');
        if ($("#field_852").val() != "ELIMINADO" && $("#field_852").val() != "Eliminado" && $("#field_852").val() != "RECHAZADO" && $("#field_852").val() != "Rechazado") {
            $("#field_852").val("Pendiente Eliminar");
        }
        $("#field_852").attr('Readonly', true);
        $("#field_854").addClass('form-control Req-2 fieldSave requiredForever');
        var date = new Date();
        var fecha = date.getFullYear() + '/' + (date.getMonth() + 1) + '/' + date.getDate();
        var hora = date.getHours() + ':' + date.getMinutes() + ':' + date.getSeconds();
        var fechaYHora = fecha + ' ' + hora;
        $("#field_853").val(fechaYHora); // ultima actualizacion
        $("#field_854").val(fechaYHora); //actualizacion anterior


        $("#div_852").css({ 'display': '' });//estado centrales cubo
        $("#div_853").hide;//actualizacion anterior centrales

        $("#div_854").css({ 'display': 'none' });//ultima actualizacion centrales

        $("#ExportarEliminarCentrales").show(); //boton exportar Eliminar Centrales

        restaurar_colores_ascard();
        restaurar_colores_cuotas();
        restaurar_colores_postpago();
        restaurar_colores_prepago();

        flag = 1;
        control = control + 1;
    }
    else {
        swal("Desactivacion de Eliminacion de Centrales", "Se debe diligenciar el campo de Ajuste en NO", "success");
        //$("#lbl_725").show();
        //$("#field_725").show();
        //$("#field_725").removeClass('form-control Req-2 fieldSave requiredForever');
        //$("#field_725").addClass('form-control Req-1 fieldSave');

        ocultar_eliminacioncentrales();
        $("#ExportarEliminarCentrales").hide(); //boton exportar Eliminar Centrales

        restaurar_colores_ascard();
        restaurar_colores_cuotas();
        restaurar_colores_postpago();
        restaurar_colores_prepago();

        flag = 0;
        control = control - 1;
    }
}//eliminacion de centrales OK
//excel planilla ajuste movil
function GenerarExcel(event) {
    event.preventDefault();

    var fechainicio = $('#txtFechaInicio').val();
    var fechafin = $('#txtFechaFinal').val();
    var parametersString = "";

    if (fechainicio != "") {
        if (parametersString.includes('?')) {
            parametersString += '&FechaInicio=' + fechainicio;
        } else {
            parametersString += '?FechaInicio=' + fechainicio;
        }
    }

    if (fechafin != "") {
        if (parametersString.includes('?')) {
            parametersString += '&FechaFinal=' + fechafin;
        } else {
            parametersString += '?FechaFinal=' + fechafin;
        }
    }

    location.href = "/WorkOrderSolutions/PlanillaAjustesMovil_Excel" + parametersString;
}
async function getDispositionsByServicio(){
    var $option = '<option value="">--Seleccionar--</option>';
    if (true) {        
        var params = new Object();
        params['servicio'] = $("#TxtSearchServicio").val();
        var Result = await $.ajax({
            type: "get",
            url: '/WorkOrderSolutions/GetDispositionsByServicio/',
            data: params
        });
        $.each(Result, function (i, data) {
            $option += '<option value="' + data.ID + '">' + data.Nombre + '</option>';
        });
    }
    $("#DdlServicio").html($option);
    return false;
}
async function getCtaContable(){
    var $option = '<option value="">--Seleccionar--</option>';
    if (true) {
        var params = new Object();
        params['ID'] = $("#DdlServicio").val();
        var Result = await $.ajax({
            type: "get",
            url: '/WorkOrderSolutions/GetCtaContable/',
            data: params
        });
        $.each(Result, function (i, data) {
            $option += '<option value="' + data.ID + '">' + data.Nombre + '</option>';
        });
    }
    complete: $("#DdlCTA_Contable").html($option);
    return false;
}
async function BtnGuardar_2($Btn)
{
    try
    {
        //ToolsFuncBtnLoader($Btn, 1);

        let params = new Object();


        if ($('#IdWorkOrderh2').val() === '') {
            swal("Error", "Ingrese el campo id del caso", "error");
            return;
        }

        params['IdWorkOrder'] = $('#IdWorkOrderh2').val();
        params["Status"] = {
            IdStatusDefinition: $('#DdlStatus').val()
        }
        params["SubStatus"] = {
            IdStatusDefinition: $('#DdlSubStatus').val()
        }
        params["Resolutions"] = $("#TxtResolution").summernote("code");
        params["ListFielsUDFSolution"] = objFields;
        params["WorkOrderEscalations"] = {
            GroupsScaled: {
                IdMasterGroups: $('#DdlGroupScale').val()
            },
            TypeScaling: $('#DdlTypeScaled').val(),
            Comments: $('#TxtCommentsScaled').val()

        }

        var Result = await $.ajax({
            type: "post",
            url: '/WorkOrderSolutions/SaveWorkOrderSolutions_2',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(params)
        });
        swal("Bien Hecho!", "Acción realizada correctamente.", "success");
        window.location = "/ListWorkOrders";
    } catch (ex) {
        swal("Error interno", "Comunicate con el area de IT para la verificacion de este error..", "error");
    } finally {

    }
}
async function BtnGuardar_Ng() {
    swal("Bien Hecho!", "Acción realizada correctamente.", "success");
    window.location = "/ListWorkOrders";
}
/// 
///
///
//CODIGO NUEVO//

//ocultar botones de exportar
$("#botonexport").hide(); //boton exportar postpago
$("#Exportarascard").hide(); //boton exportar ascard
$("#Exportarprepago").hide(); //boton exportar prepago
$("#Exportarcuotas").hide(); //boton exportar cuotas
$("#ExportarEliminarCentrales").hide(); //boton exportar Eliminar Centrales

// OCULTAR CAMPOS
$("#div_725").css({ 'display': 'none' }); //LISTAS RELACIONADAS
$("#div_319").css({ 'display': 'none' }); //PRETENCIONES
$("#div_151").css({ 'display': 'none' }); //CENTRALES
$("#div_778").css({ 'display': 'none' }); //responsabilidad de la inconsistencia
$("#div_795").css({ 'display': 'none' }); //prueba
$("#div_796").css({ 'display': 'none' }); //proceso

// ocultar botones

$("#Acard1").hide(); //boton ascard
$("#BtBPB").hide(); //boton postpago
$("#btncuotaAcard").hide(); // boton cuotas ascard
$("#btnprepago").hide(); //btn prepago
$("#btnEliminacionCentrales").hide(); // eliminacion de centrales

//ocultar inPQR
$("#div_746").css({ 'display': 'none' }); //746
$("#div_774").css({ 'display': 'none' }); //774
$("#div_747").css({ 'display': 'none' }); //747
$("#div_748").css({ 'display': 'none' }); //748
$("#div_749").css({ 'display': 'none' }); //749
$("#div_750").css({ 'display': 'none' }); //750
$("#div_751").css({ 'display': 'none' }); //751
$("#div_752").css({ 'display': 'none' }); //752
$("#div_753").css({ 'display': 'none' }); //753
$("#div_754").css({ 'display': 'none' }); //754
$("#div_755").css({ 'display': 'none' }); //755
$("#div_756").css({ 'display': 'none' }); //756
$("#div_757").css({ 'display': 'none' }); //757

//validaciones de colores
//field_797.onblur = function () {
//let swError = false;
//    swError = swError | validarInput("field_797", "text");
//if (swError) { return false; }
//};


/// buscar cuenta servicio e iva
//field_801.onblur = function () { // al perder el foco el campop cta contable ejecuta la funcion id consulta
//    //console.log(field_799.value);
//    Idconsulta(field_801.value); 
//};

async function changeCuenta($Select) {

    var cmb_cuenta = $("#field_800").val();
    if (cmb_cuenta != null) {
        //alert(field_800.value);
        var _cuenta = field_800.value
        //_cuenta = _cuenta.substring(0,_cuenta.length - 13)
        consultar_cuenta(_cuenta);
    }

}

var IDENT = null;
function Idconsulta() {
    IDENT = document.getElementById("field_800").value;
    //console.log(IDENT);
    consultar_cuenta(IDENT);
}

function consultar_cuenta(servicio) {
    //console.log("El id a editar es " + Id);
    //alert("El id a editar es " + Id);

    $.post('/WorkOrderSolutions/WorkOrder_GetContable/', { 'servicio': servicio })
        .done((objplantilla) => {
            console.log(' ' + JSON.stringify(objplantilla));
            //alert(' ' + JSON.stringify(objplantilla));

            asignarCamposCuenta(objplantilla);

        })
        .fail((error) => {
            alert("ocurrio un error al momento de asignar los valores de la plantilla de mensaje :" + error.message);
        });

}

function asignarCamposCuenta(objplantilla) {

    $('#field_801').val(objplantilla.cod_cuenta); //cta contable
    $('#field_800').val(objplantilla.servicio); // servicio
    $('#field_802').val(objplantilla.iva); // iva

    var elements = document.querySelectorAll("input[type='text']");
    for (var i = 0; i < elements.length; i++) {
        elements[i].addEventListener("focus", function () {
            inputfocused = this;
        });
    }
}

//gerencia
//field_858.onblur = function () { // al perder el foco el campo area que genero ajuste
//    console.log(field_858.value);
//    IdconsultaGerencia(field_803.value);
//};


async function changeArea($Select) {

    var cmb_area = $("#field_858").val();
    if (cmb_area != null) {
        IdconsultaGerencia(field_858.value);
    }

}

var IDE = null;
function IdconsultaGerencia() {
    IDE = document.getElementById("field_858").value;
    console.log("este es el id" + IDE);
    consultar_gerencia(IDE);

}

function consultar_gerencia(area_genero_ajuste) {
    //console.log("El id a editar es " + Id);
    //alert("El id a editar es " + Id);

    $.post('/WorkOrderSolutions/WorkOrder_GetGerencia/', { 'area_genero_ajuste': area_genero_ajuste })
        .done((objplantilla) => {
            console.log(' ' + JSON.stringify(objplantilla));
            //alert(' ' + JSON.stringify(objplantilla));

            asignarCamposGerencia(objplantilla);

        })
        .fail((error) => {
            alert("ocurrio un error al momento de asignar los valores de la plantilla de mensaje :" + error.message);
        });

}

function asignarCamposGerencia(objplantilla) {

    $('#field_858').val(objplantilla.area_genero_ajuste); //area que genero ajuste
    $('#field_803').val(objplantilla.gerencia); // gerencia

    var elements = document.querySelectorAll("input[type='text']");
    for (var i = 0; i < elements.length; i++) {
        elements[i].addEventListener("focus", function () {
            inputfocused = this;
        });
    }
}


function ocultar_postpago() {
    // AGREGADOS NUEVOS

    //$("#field_150").removeClass('form-control Req-2 fieldSave requiredForever');//ajuste
    $("#field_797").removeClass('requiredForever');
    $("#div_797").css({ 'display': 'none' }); // analista postpago
    $("#field_798").removeClass('requiredForever');
    $("#div_798").css({ 'display': 'none' }); // user red postpago
    $("#field_768").removeClass('requiredForever');
    $("#div_768").css({ 'display': 'none' }); // cun postpago
    $("#field_799").removeClass('requiredForever');
    $("#div_799").css({ 'display': 'none' }); //tipo reclamo postpago
    $("#field_767").removeClass('requiredForever');
    $("#div_767").css({ 'display': 'none' }); // min postpago
    $("#field_769").removeClass('requiredForever');
    $("#div_769").css({ 'display': 'none' }); // custcode postpago
    $("#field_770").removeClass('requiredForever');
    $("#div_770").css({ 'display': 'none' }); // customerid postpago
    $("#field_800").removeClass('requiredForever');
    $("#div_800").css({ 'display': 'none' }); //servicio 800 postpago
    $("#field_801").removeClass('requiredForever');
    $("#div_801").css({ 'display': 'none' }); //cta contable 801 postpago
    $("#field_802").removeClass('requiredForever');
    $("#div_802").css({ 'display': 'none' }); //iva 802 postpago
    $("#field_727").removeClass('requiredForever');
    $("#div_727").css({ 'display': 'none' }); // valor postpago
    $("#field_729").removeClass('requiredForever');
    $("#div_729").css({ 'display': 'none' }); // periodo a ajustar postpago
    $("#field_730").removeClass('requiredForever');
    $("#div_730").css({ 'display': 'none' }); // periodo ajustar hasta postpago
    $("#field_728").removeClass('requiredForever');
    $("#div_728").css({ 'display': 'none' }); // justificacion postpago
    $("#field_771").removeClass('requiredForever');
    $("#div_771").css({ 'display': 'none' }); //arwa que genero inconsistencia postpago
    $("#field_803").removeClass('requiredForever');
    $("#div_803").css({ 'display': 'none' }); //gerencia 803 postpago
    $("#field_859").removeClass('requiredForever');
    $("#div_859").css({ 'display': 'none' }); //user responsable postpago
    $("#div_859").hide();
    $("#field_804").removeClass('requiredForever');
    $("#div_804").css({ 'display': 'none' }); //centrales 804 postpago
    $("#field_805").removeClass('requiredForever');
    $("#div_805").css({ 'display': 'none' }); //aliado 805 postpago
    $("#field_806").removeClass('requiredForever');
    $("#div_806").css({ 'display': 'none' }); //causal 806 postpago
    $("#div_807").css({ 'display': 'none' }); //estado 807 postpago
    $("#field_807").removeClass('requiredForever');
    $("#div_810").css({ 'display': 'none' }); //fecha creacion 808 postpago
    $("#field_810").removeClass('requiredForever');
    $("#div_811").css({ 'display': 'none' }); //fecha ultima actualizacion 809 postpago
    $("#field_811").removeClass('requiredForever');
    $("#div_726").css({ 'display': 'none' }); // segmento postpago postpago
    $("#field_726").removeClass('requiredForever');
    $("#div_858").css({ 'display': 'none' }); // segmento postpago postpago
    $("#field_858").removeClass('requiredForever');
    $("#div_772").css({ 'display': 'none' }); // usuario ajuste postpago
    $("#field_772").removeClass('requiredForever');

    $("#field_727").addClass('form-control Req-2 fieldSave requiredForever'); //valida valor pospago


    $("#BtnPrepago").hide();
    $("#BtnPospago").hide();
    $("#BtnAscard").hide();
    $("#BtnCuotasAscard").hide();
    $("#BtnEliminacionCentrales").hide();
    $("#divTableprepago").hide();
    $("#divTableascard1").hide();
    $("#divTablepospago").hide();
    $("#divTableascard2").hide();
    $("#divTableeliminar").hide();

    BtnGuardar_S.disabled = true;

}
function ocultar_prepago() {

    //AGREGADOS
    $("#field_830").removeClass('requiredForever');
    $("#div_830").css({ 'display': 'none' });//radicado prepago
    $("#field_831").removeClass('requiredForever');
    $("#div_831").css({ 'display': 'none' });//tipo de reclamo prepago
    $("#field_832").removeClass('requiredForever');
    $("#div_832").css({ 'display': 'none' });//nombre de titular prepago
    $("#field_833").removeClass('requiredForever');
    $("#div_833").css({ 'display': 'none' });//min prepago
    $("#field_834").removeClass('requiredForever');
    $("#div_834").css({ 'display': 'none' });//custcode prepago
    $("#field_835").removeClass('requiredForever');
    $("#div_835").css({ 'display': 'none' });//valor prepago
    $("#field_836").removeClass('requiredForever');
    $("#div_836").css({ 'display': 'none' });//concepto prepago
    $("#field_837").removeClass('requiredForever');
    $("#div_837").css({ 'display': 'none' });//analista prepago
    $("#field_838").removeClass('requiredForever');
    $("#div_838").css({ 'display': 'none' });//periodo ajustado desde prepago
    $("#field_839").removeClass('requiredForever');
    $("#div_839").css({ 'display': 'none' });//periodo ajustado hasta prepago
    $("#field_840").removeClass('requiredForever');
    $("#div_840").css({ 'display': 'none' });//aliado prepago
    $("#field_833").removeClass('requiredForever');
    $("#div_833").css({ 'display': 'none' });//estado prepago
    $("#field_834").removeClass('requiredForever');
    $("#div_834").css({ 'display': 'none' });//actualizacion anterior prepago
    $("#field_835").removeClass('requiredForever');
    $("#div_835").css({ 'display': 'none' });//ultima actualizacion prepago
    $("#field_855").removeClass('requiredForever');
    $("#div_855").css({ 'display': 'none' });//ultima actualizacion prepago
    $("#field_856").removeClass('requiredForever');
    $("#div_856").css({ 'display': 'none' });//ultima actualizacion prepago
    $("#field_857").removeClass('requiredForever');
    $("#div_857").css({ 'display': 'none' });//ultima actualizacion prepago

    $("#field_727").addClass('form-control Req-2 fieldSave requiredForever'); //valida valor pospago

    $("#BtnPrepago").hide();
    $("#BtnPospago").hide();
    $("#BtnAscard").hide();
    $("#BtnCuotasAscard").hide();
    $("#BtnEliminacionCentrales").hide();
    $("#divTableprepago").hide();
    $("#divTableascard1").hide();
    $("#divTablepospago").hide();
    $("#divTableascard2").hide();
    $("#divTableeliminar").hide();

    BtnGuardar_S.disabled = true;
}
function ocultar_ascard() {
    //agregados
    $("#field_731").removeClass('requiredForever');
    $("#div_731").css({ 'display': 'none' });//numero de credito 731 ascard
    $("#field_732").removeClass('requiredForever');
    $("#div_732").css({ 'display': 'none' });//valor del ajuste 732 ascard
    $("#field_812").removeClass('requiredForever');
    $("#div_812").css({ 'display': 'none' });//motivo del ajuste 810 ascard
    $("#field_733").removeClass('requiredForever');
    $("#div_733").css({ 'display': 'none' });//descripcion motivo del ajuste 733 ascard
    $("#field_734").removeClass('requiredForever');
    $("#div_734").css({ 'display': 'none' });//area que solicita el ajuste 734 ascard
    $("#field_735").removeClass('requiredForever');
    $("#div_735").css({ 'display': 'none' });//comentari0 735 ascard
    $("#field_813").removeClass('requiredForever');
    $("#div_813").css({ 'display': 'none' });//nombre quien solicita 811 ascard
    $("#field_736").removeClass('requiredForever');
    $("#div_736").css({ 'display': 'none' });//documento del usuario 736 ascard
    $("#field_737").removeClass('requiredForever');
    $("#div_737").css({ 'display': 'none' });//reclamo del usuario 737 ascard
    $("#field_739").removeClass('requiredForever');
    $("#div_739").css({ 'display': 'none' });//custcode asociado al credito ascard
    $("#field_819").removeClass('requiredForever');
    $("#div_819").css({ 'display': 'none' });//cun/nr 812 ascard
    $("#field_820").removeClass('requiredForever');
    $("#div_820").css({ 'display': 'none' });//tipo de reclamo 813 ascard
    $("#field_821").removeClass('requiredForever');
    $("#div_821").css({ 'display': 'none' });//aliado 814 ascard
    $("#field_822").removeClass('requiredForever');
    $("#div_822").css({ 'display': 'none' });//estado 815 ascard
    $("#field_824").removeClass('requiredForever');
    $("#div_824").css({ 'display': 'none' });//actualizacin anterior 816 ascard
    $("#field_825").removeClass('requiredForever');
    $("#div_825").css({ 'display': 'none' });//ultima actualizacion 817 ascard

    $("#field_727").addClass('form-control Req-2 fieldSave requiredForever'); //valida valor pospago

    $("#BtnPrepago").hide();
    $("#BtnPospago").hide();
    $("#BtnAscard").hide();
    $("#BtnCuotasAscard").hide();
    $("#BtnEliminacionCentrales").hide();
    $("#divTableprepago").hide();
    $("#divTableascard1").hide();
    $("#divTablepospago").hide();
    $("#divTableascard2").hide();
    $("#divTableeliminar").hide();

    BtnGuardar_S.disabled = true;

}
function ocultar_cuotasascard() {
    //agregados
    $("#field_740").removeClass('requiredForever');
    $("#div_740").css({ 'display': 'none' });//numero de credito 740 cuotas ascard
    $("#field_741").removeClass('requiredForever');
    $("#div_741").css({ 'display': 'none' });//valor nueva cuota 741 cuotas ascard
    $("#field_742").removeClass('requiredForever');
    $("#div_742").css({ 'display': 'none' });//concepto 742 cuotas ascard
    $("#field_743").removeClass('requiredForever');
    $("#div_743").css({ 'display': 'none' });//area que solicita ajuste 743 cuotas ascard
    $("#field_744").removeClass('requiredForever');
    $("#div_744").css({ 'display': 'none' });//reclamo del usuario 744 cuotas ascard
    $("#field_745").removeClass('requiredForever');
    $("#div_745").css({ 'display': 'none' });//cantidad de cuotas 745 cuotas ascard
    $("#field_826").removeClass('requiredForever');
    $("#div_826").css({ 'display': 'none' });//aliado 818 cuotas ascard
    $("#field_827").removeClass('requiredForever');
    $("#div_827").css({ 'display': 'none' });//estado 819 cuotas ascard
    $("#field_828").removeClass('requiredForever');
    $("#div_828").css({ 'display': 'none' });//actualizacion anteriro 820 cuotas ascard
    $("#field_829").removeClass('requiredForever');
    $("#div_829").css({ 'display': 'none' });//ultima acxtualizacion 821 cuotas ascard

    $("#field_727").addClass('form-control Req-2 fieldSave requiredForever'); //valida valor pospago

    $("#BtnPrepago").hide();
    $("#BtnPospago").hide();
    $("#BtnAscard").hide();
    $("#BtnCuotasAscard").hide();
    $("#BtnEliminacionCentrales").hide();
    $("#divTableprepago").hide();
    $("#divTableascard1").hide();
    $("#divTablepospago").hide();
    $("#divTableascard2").hide();
    $("#divTableeliminar").hide();

    BtnGuardar_S.disabled = true;

}
function ocultar_eliminacioncentrales() {
    //AGREGADOS
    $("#field_844").removeClass('requiredForever');
    $("#div_844").css({ 'display': 'none' });//cun  centrales
    $("#field_845").removeClass('requiredForever');
    $("#div_845").css({ 'display': 'none' });//tipo de documento centrales
    $("#field_846").removeClass('requiredForever');
    $("#div_846").css({ 'display': 'none' });//nombrer centrales
    $("#field_847").removeClass('requiredForever');
    $("#div_847").css({ 'display': 'none' });//cedula centrales
    $("#field_848").removeClass('requiredForever');
    $("#div_848").css({ 'display': 'none' });//custcode No credito centrales
    $("#field_849").removeClass('requiredForever');
    $("#div_849").css({ 'display': 'none' });//estado centrales
    $("#field_850").removeClass('requiredForever');
    $("#div_850").css({ 'display': 'none' });//motivo de la elminacion centrales
    $("#field_851").removeClass('requiredForever');
    $("#div_851").css({ 'display': 'none' });//analista centrales
    $("#field_852").removeClass('requiredForever');
    $("#div_852").css({ 'display': 'none' });//estado centrales cubo
    $("#field_853").removeClass('requiredForever');
    $("#div_853").css({ 'display': 'none' });//actualizacion anterior centrales
    $("#field_854").removeClass('requiredForever');
    $("#div_854").css({ 'display': 'none' });//ultima actualizacion centrales
    $("#field_844").removeClass('requiredForever');
    $("#div_844").css({ 'display': 'none' });//cun  centrales
    $("#field_845").removeClass('requiredForever');
    $("#div_845").css({ 'display': 'none' });//tipo de documento centrales
    $("#field_846").removeClass('requiredForever');
    $("#div_846").css({ 'display': 'none' });//nombrer centrales
    $("#field_847").removeClass('requiredForever');
    $("#div_847").css({ 'display': 'none' });//cedula centrales
    $("#field_848").removeClass('requiredForever');
    $("#div_848").css({ 'display': 'none' });//custcode No credito centrales
    $("#field_849").removeClass('requiredForever');
    $("#div_849").css({ 'display': 'none' });//estado centrales
    $("#field_850").removeClass('requiredForever');
    $("#div_850").css({ 'display': 'none' });//motivo de la elminacion centrales
    $("#field_851").removeClass('requiredForever');
    $("#div_851").css({ 'display': 'none' });//analista centrales
    $("#field_852").removeClass('requiredForever');
    $("#div_852").css({ 'display': 'none' });//estado centrales cubo
    $("#field_853").removeClass('requiredForever');
    $("#div_853").css({ 'display': 'none' });//actualizacion anterior centrales
    $("#field_854").removeClass('requiredForever');
    $("#div_854").css({ 'display': 'none' });//ultima actualizacion centrales

    $("#field_727").addClass('form-control Req-2 fieldSave requiredForever'); //valida valor pospago

    $("#BtnPrepago").hide();
    $("#BtnPospago").hide();
    $("#BtnAscard").hide();
    $("#BtnCuotasAscard").hide();
    $("#BtnEliminacionCentrales").hide();
    $("#divTableprepago").hide();
    $("#divTableascard1").hide();
    $("#divTablepospago").hide();
    $("#divTableascard2").hide();
    $("#divTableeliminar").hide();

    BtnGuardar_S.disabled = true;
}

// validaciones cun 16 digitos numericos

//function secureCUSTCODE(el) {
//    const regex = /^[0-9.]+$/
//    if (!regex.test(el.value)) {
//        el.value = el.value.substring(0, el.value.length -1)
//    }
//}

$("#field_769").on('keydown keypress', function (e) { //cuscode postpago


    cadena = document.getElementById("field_769").value
    console.log(cadena);
    console.log(e.keyCode);

    if (e.keyCode != 8) {
        if (e.keyCode != 46) {
            if (e.keyCode != 37) {
                if (e.keyCode != 39) {
                    if (e.keyCode != 9) {
                        if (cadena.length > 19) {
                            return false;
                        }
                    }
                }
            }
        }
    }

    //var el = $("#field_769").val();
    //if (e.key.length === 1) {
    //    if ($(this).val().length < 20 && parseFloat(e.key)) {
    //        $(this).val($(this).val() + e.key);
    //    }
    //    return false;
    //}
})

//$("#field_769").on('keydown keypress', function (e) { //cuscode postpago
//    var el = $("#field_769").val();
//    if (e.key.length === 1) {
//        if ($(this).val().length < 20 && !isNaN(parseFloat(e.key))) {
//            $(this).val($(this).val() + e.key);
//        }
//        return false;
//    }
//})


$("#field_745").on('keydown keypress', function (e) { //cuscode 
    var el = $("#field_745").val();
    if (e.key.length === 1) {
        if ($(this).val().length < 2 && !isNaN(parseFloat(e.key))) {
            $(this).val($(this).val() + e.key);
        }
        return false;
    }
})

$("#field_747").on('keydown keypress', function (e) { //cuscode
    var el = $("#field_747").val();
    if (e.key.length === 1) {
        if ($(this).val().length < 20 && !isNaN(parseFloat(e.key))) {
            $(this).val($(this).val() + e.key);
        }
        return false;
    }
})

$("#field_753").on('keydown keypress', function (e) { //cuscode
    var el = $("#field_753").val();
    if (e.key.length === 1) {
        if ($(this).val().length < 20 && !isNaN(parseFloat(e.key))) {
            $(this).val($(this).val() + e.key);
        }
        return false;
    }
})

$("#field_768").on('keydown keypress', function (e){ //cun postpago
    if (e.key.length === 1) {
        if ($(this).val().length < 16 && !isNaN(parseFloat(e.key))) {
            $(this).val($(this).val() + e.key);
        }
        return false;
    }
})

$("#field_767").on('keydown keypress', function (e) { // min postpago
    if (e.key.length === 1) {
        if ($(this).val().length < 10 && !isNaN(parseFloat(e.key))) {
            $(this).val($(this).val() + e.key);
        }
        return false;
    }
})

$("#field_833").on('keydown keypress', function (e) { // min prepago
    if (e.key.length === 1) {
        if ($(this).val().length < 10 && !isNaN(parseFloat(e.key))) {
            $(this).val($(this).val() + e.key);
        }
        return false;
    }
})

$("#field_770").on('keydown keypress', function (e) { // customerid postpago
    if (e.key.length === 1) {
        if ($(this).val().length < 10 && !isNaN(parseFloat(e.key))) {
            $(this).val($(this).val() + e.key);
        }
        return false;
    }
})

$("#field_728").on('keydown keypress', function (e) { // justificacion postpago
    if (e.key.length === 1) {
        if ($(this).val().length < 250) {
            $(this).val($(this).val() + e.key);
        }
        return false;
    }
})

$("#field_731").on('keydown keypress', function (e) { // numero de credito ascard
    if (e.key.length === 1) {
        if ($(this).val().length < 16 && !isNaN(parseFloat(e.key))) {
            $(this).val($(this).val() + e.key);
        }
        return false;
    }
})

$("#field_736").on('keydown keypress', function (e) { // numero de credito ascard
    if (e.key.length === 1) {
        if ($(this).val().length < 26 && !isNaN(parseFloat(e.key))) {
            $(this).val($(this).val() + e.key);
        }
        return false;
    }
})

$("#field_737").on('keydown keypress', function (e) { // Reclamo del usuario ascard
    if (e.key.length === 1) {
        if ($(this).val().length < 250) {
            $(this).val($(this).val() + e.key);
        }
        return false;
    }
})

$("#field_744").on('keydown keypress', function (e) { // Reclamo del usuario cuotas ascard
    if (e.key.length === 1) {
        if ($(this).val().length < 250 ) {
            $(this).val($(this).val() + e.key);
        }
        return false;
    }
})

// formato a numero
$("#field_740").on('keydown keypress', function (e) { // nUMERO DE CREDITO CUOTAS ASCARD
    if (e.key.length === 1) {
        if ($(this).val().length < 16 && !isNaN(parseFloat(e.key))) {
            $(this).val($(this).val() + e.key);
        }
        return false;
    }
})

$("#field_847").on('keydown keypress', function (e) { // nUMERO DE CEDULA ELIMINACION DE CENTRALES
    if (e.key.length === 1) {
        if ($(this).val().length < 16 && !isNaN(parseFloat(e.key))) {
            $(this).val($(this).val() + e.key);
        }
        return false;
    }
})



//formato numerico con decimales
///// convertir string a moneda
//field_727.onblur = function () { //valor de postpago

//    return parseFloat($('#field_727').val());

//    //const money = document.getElementById("field_727").value;
//    //const currency = function (number) {
//    //    return new Intl.NumberFormat({ style: 'currency', minimumFractionDigits: 2 }).format(number);
//    //};
//    //var resultado = 0;
//    //$('#field_727').val(currency(money));
//}

$('#field_727').addClass('form-control Req-2 fieldSave requiredForever');

async function showAdjustScreen7() {
    location.href = "/WorkOrderSolutions/ExportarPostpago/"

} ////exportar postpago

async function botonexport() { // eliminacion de centrales

    //console.log("El id a editar es " + Id);
    //alert("El id a editar es " + Id);
    //alert("Archivo exportado a la carpeta Temp");
    $.post('/WorkOrderSolutions/ExportarPostpago/');
    alert("Archivo exportado a la carpeta Temp");
}

async function exportarpre() { // eliminacion de centrales

    //console.log("El id a editar es " + Id);
    //alert("El id a editar es " + Id);
    //alert("Archivo exportado a la carpeta Temp");
    location.href = "/WorkOrderSolutions/ExportarPrepago/"
    //$.post('/WorkOrderSolutions/ExportarPrepago/');
    //alert("Archivo exportado a la carpeta Temp");
}

async function exportarascard() { // eliminacion de centrales

    //console.log("El id a editar es " + Id);
    //alert("El id a editar es " + Id);
    //alert("Exportar");
    //$.post('/WorkOrderSolutions/ExportarAscard/');
    //alert("Archivo exportado a la carpeta Temp");

    location.href = "/WorkOrderSolutions/ExportarAscard/"
}

async function exportarcuotas() { // eliminacion de centrales

    //console.log("El id a editar es " + Id);
    //alert("El id a editar es " + Id);
    //alert("Exportar");
    //$.post('/WorkOrderSolutions/ExportarCuotasAscard/');
    //alert("Archivo exportado a la carpeta Temp");

    location.href = "/WorkOrderSolutions/ExportarCuotasAscard/"
}

async function exportarEliminarCentrales() { // eliminacion de centrales

    //console.log("El id a editar es " + Id);
    //alert("El id a editar es " + Id);
    //alert("Archivo exportado a la carpeta Temp");
    location.href = "/WorkOrderSolutions/ExportarEliminarCentrales/"
    //$.post('/WorkOrderSolutions/ExportarPrepago/');
    //alert("Archivo exportado a la carpeta Temp");
}

function CargarDataMasiva() {

        if ($("#ExcelData").val() == '') {
        swal({
            title: "PRECAUCIÓN",
            text: "Seleccione el archivo a cargar.",
            type: "error",
            timeout: 3000
        });
        return false;
    }

    var f = $("#ExcelData").val();
    var ext = f.split('.');
        //ahora obtenemos el ultimo valor despues el punto
        //obtenemos el length por si el archivo lleva nombre con mas de 2 puntos
        ext = ext[ext.length - 1];
        if (ext != 'xlsx' && ext != 'xls') {
                    swal({
                                    title: "¡ PRECAUCIÓN !",
                                    text: "Formato de archivo no válido.",
                                                type: "error",
                                                            timeout: 3000
                    });
                    return false;
        }

    swal({
        title: "Estas Seguro?",
        text: "A continuación se cargaran los casos en PQR",
        type: "warning",
        showCancelButton: true,
        confirmButtonClass: "btn-success",
        confirmButtonText: "SI",
        closeOnConfirm: true,
        closeOnCancel: false
    },

        function (isConfirm) {
            if (isConfirm) {
                CargueData();
            } else {
                swal("Cancelado", "Cancelado", "error");
            };
        })
}


async function CargueData() {
    var formData = new FormData();
    var file = document.getElementById("ExcelData").files[0];
    formData.delete("MyFile", file);
    formData.append("MyFile", file);
    $("#cargando").show();
    $("#ExcelData").val("");

    await $.ajax({
        type: "POST",
        url: '/PortalZec/CargarDataMasiva',
        data: formData,
        dataType: 'json',
        contentType: false,
        processData: false,
        cache: false,
        success: function (response) {
            if (response.success == true) {
                swal({
                    title: "¡ BIEN HECHO !",
                    text: "Data cargada correctamente",
                    type: "success",
                    timeout: 3000
                });
                $("#cargando").hide();
                setTimeout(function () { window.location = "/PortalZec"; }, 2000);
            } else {
                swal({
                    title: "¡ PRECAUCIÓN !",
                    content: response.Message,
                    type: "error",
                    timeout: 3000
                });
                $("#cargando").hide();
                return false;
            }
        }
    })
}

async function validacuscode(el) {
    const regex = /^[0-9.]+$/
    if (!regex.test(el.value)) {
        el.value = el.value.substring(0, el.value.length -1)
    }
}



// CUN POSTPAGO
$("#field_768").attr('maxlength', '16');
$("#field_768").on('keypress', function (e)
{
    return aplicarFormato(this, e, "CUN");
})

$("#field_768").on('blur', function ()
{
    return validarFormato(this, "CUN");

})

// CUSCODE POSTPAGO
$("#field_769").attr('maxlength', '20');
$("#field_769").on('keypress', function (e) {
    return aplicarFormato(this, e, "CUSTCODE");
})

$("#field_769").on('blur', function () {
    return validarFormato(this, "CUSTCODE");
})

// VALOR POSTPAGO
$("#field_727").on('keypress', function (e) {
    return aplicarFormato(this, e, "DECIMAL");
})

$("#field_727").on('blur', function () {
    return validarFormato(this, "DECIMAL");
})

// VALOR AJUSTE ASCARD1
$("#field_732").on('keypress', function (e) {
    return aplicarFormato(this, e, "DECIMAL");
})

$("#field_732").on('blur', function () {
    return validarFormato(this, "DECIMAL");
})

// CUSCODE ASCARD1
$("#field_739").attr('maxlength', '20');
$("#field_739").on('keypress', function (e) {
    return aplicarFormato(this, e, "CUSTCODE");
})

$("#field_739").on('blur', function () {
    return validarFormato(this, "CUSTCODE");
})

// CUN ASCARD
$("#field_819").attr('maxlength', '16');
$("#field_819").on('keypress', function (e) {
    return aplicarFormato(this, e, "CUN");
})

$("#field_819").on('blur', function () {
    return validarFormato(this, "CUN");
})

// VALOR AJUSTE ASCARD2
$("#field_741").on('keypress', function (e) {
    return aplicarFormato(this, e, "DECIMAL");
})

$("#field_741").on('blur', function () {
    return validarFormato(this, "DECIMAL");
})

// VALOR AJUSTE prepago
$("#field_835").on('keypress', function (e) {
    return aplicarFormato(this, e, "DECIMAL");
})

$("#field_835").on('blur', function () {
    return validarFormato(this, "DECIMAL");
})

// cun centrales
$("#field_844").attr('maxlength', '16');
$("#field_844").on('keypress', function (e) {
    return aplicarFormato(this, e, "CUN");
})

$("#field_844").on('blur', function () {
    return validarFormato(this, "CUN");
})


// CUSCODE ELIMINACION CENTRALES
$("#field_848").attr('maxlength', '20');
$("#field_848").on('keypress', function (e) {
    return aplicarFormato(this, e, "CUSTCODE");
})

$("#field_848").on('blur', function () {
    return validarFormato(this, "CUSTCODE");
})

// CUSCODE prepago
$("#field_834").attr('maxlength', '20');
$("#field_834").on('keypress', function (e) {
    return aplicarFormato(this, e, "CUSTCODE");
})

$("#field_834").on('blur', function () {
    return validarFormato(this, "CUSTCODE");
})

//// CUSTOMERID POSTPAGO
$("#field_770").attr('maxlength', '10');
$("#field_770").on('keypress', function (e) {
    return aplicarFormato(this, e, "CUSTOMERID");
})

$("#field_770").on('blur', function () {
    if ($('#field_770').val().toUpperCase().length < 3)
        alert("Digite un CUSTOMER ID válido. Verifique.")
        //$('#field_770').val("")
    //return validarFormato(this, "CUSTOMERID");
})


    
async function restaurar_colores_postpago() {

    //restaurar colores
    field_797.style.borderColor = "#d1d3e2";
    field_798.style.borderColor = "#d1d3e2";
    field_768.style.borderColor = "#d1d3e2";
    field_799.style.borderColor = "#d1d3e2";
    field_767.style.borderColor = "#d1d3e2";
    field_769.style.borderColor = "#d1d3e2";
    field_770.style.borderColor = "#d1d3e2";
    field_801.style.borderColor = "#d1d3e2";
    field_727.style.borderColor = "#d1d3e2";
    field_729.style.borderColor = "#d1d3e2";
    field_730.style.borderColor = "#d1d3e2";
    field_728.style.borderColor = "#d1d3e2";
    field_771.style.borderColor = "#d1d3e2";
    field_803.style.borderColor = "#d1d3e2";
    field_859.style.borderColor = "#d1d3e2";
    field_804.style.borderColor = "#d1d3e2";
    field_805.style.borderColor = "#d1d3e2";
    field_806.style.borderColor = "#d1d3e2";

}

async function restaurar_colores_prepago() {

    ////restaurar colores
    field_830.style.borderColor = "#d1d3e2";
    field_832.style.borderColor = "#d1d3e2";
    field_833.style.borderColor = "#d1d3e2";
    field_834.style.borderColor = "#d1d3e2";
    field_835.style.borderColor = "#d1d3e2";
    field_836.style.borderColor = "#d1d3e2";
    field_837.style.borderColor = "#d1d3e2";
    field_838.style.borderColor = "#d1d3e2";
    field_839.style.borderColor = "#d1d3e2";

}

async function restaurar_colores_ascard() {

    ////restaurar colores
    field_731.style.borderColor = "#d1d3e2";
    field_732.style.borderColor = "#d1d3e2";
    field_820.style.borderColor = "#d1d3e2";
    field_736.style.borderColor = "#d1d3e2";
    field_737.style.borderColor = "#d1d3e2";
    field_739.style.borderColor = "#d1d3e2";
    field_819.style.borderColor = "#d1d3e2";
}

async function restaurar_colores_cuotas() {

    ////restaurar colores
    field_740.style.borderColor = "#d1d3e2";
    field_741.style.borderColor = "#d1d3e2";
    field_744.style.borderColor = "#d1d3e2";
    field_745.style.borderColor = "#d1d3e2";

}

async function restaurar_colores_centrales() {

    ////restaurar colores
    field_844.style.borderColor = "#d1d3e2";
    field_846.style.borderColor = "#d1d3e2";
    field_847.style.borderColor = "#d1d3e2";
    field_848.style.borderColor = "#d1d3e2";
    field_850.style.borderColor = "#d1d3e2";
    field_851.style.borderColor = "#d1d3e2";

}

