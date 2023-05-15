$('.datepicker').datepicker({
    todayBtn: "linked",
    calendarWeeks: true,
    format: 'yyyy/mm/dd'
    //format: 'dd/mm/yyyy'
})
var control = 0;
async function inputdate() {
    $('input:text[id=field_880]').val();
    $('input:text[id=field_879]').val();
    $('input:text[id=field_750]').val();
    $('input:text[id=field_751]').val();
    
    if (document.getElementById("field_880").value != "" && document.getElementById("field_879").value != "" )
    {
        if (document.getElementById("field_880").value >= document.getElementById("field_879").value )
        {

            console.log(field_880.value);
            console.log("control:", control);
            control = control + 1;
            swal({
                title: 'Alerta automatica',
                text: 'Rango de fecha incorrecto',
            })
        }
        //else if (document.getElementById("field_750").value != "" && document.getElementById("field_751").value != "") {
        //    if (document.getElementById("field_750").value >= document.getElementById("field_751").value) {

        //        console.log(field_880.value);
        //        console.log("control:", control);
        //        control = control + 1;
        //        swal({
        //            title: 'Alerta automatica InPQR',
        //            text: 'Rango de fecha incorrecto',
        //        })
        //    }
        //}
    }
    //else if (document.getElementById("field_750").value != "" && document.getElementById("field_751").value != "")
    //{
    //    if (document.getElementById("field_750").value >= document.getElementById("field_751").value ) {

    //        console.log(field_880.value);
    //        console.log("control:", control);
    //        control = control + 1;
    //        swal({
    //            title: 'Alerta automatica InPQR',
    //            text: 'Rango de fecha incorrecto',
    //        })
    //    }
    //} 
}
$('.chosen-select').chosen()
$('#TxtDescriptionClient').summernote("code", $('#TxtDescriptionClient').text())
$('.summernote-disabled').summernote('disable');
$('#TxtResolution').summernote("code", $('#TxtResolution').text());
//$('#DdlStatus').change();
$('#divLoaderMaster').hide();

ocultar_postpago1();
ocultar_prepago1();
ocultar_ascard1();
ocultar_cuotasascard1();
ocultar_eliminacioncentrales1();

$("#BtnPrepago").hide();
$("#BtnPospago").hide();
$("#BtnAscard").hide();
$("#BtnCuotasAscard").hide();
$("#BtnEliminacionCentrales").hide();
$("#divTableprepago13").hide();
$("#divTableascard13").hide();
$("#divTablepospago13").hide();
$("#divTableascard213").hide();
$("#divTableeliminar13").hide();

//validar colores postpago
let boton = document.getElementById("BtnGuardar_S")

let field_872 = document.getElementById("field_872")
let field_860 = document.getElementById("field_860")
let field_877 = document.getElementById("field_877")
let field_871 = document.getElementById("field_871")
let field_878 = document.getElementById("field_878")
let field_876 = document.getElementById("field_876")
let field_875 = document.getElementById("field_875")
let field_869 = document.getElementById("field_869")
let field_882 = document.getElementById("field_882")
let field_880 = document.getElementById("field_880")
let field_879 = document.getElementById("field_879")
let field_881 = document.getElementById("field_881")
let field_874 = document.getElementById("field_874")
let field_867 = document.getElementById("field_867")
let field_866 = document.getElementById("field_866")
let field_865 = document.getElementById("field_865")
let field_864 = document.getElementById("field_864")

//validar colores ascard
let field_883 = document.getElementById("field_883")
let field_884 = document.getElementById("field_884")
let field_894 = document.getElementById("field_894")
let field_888 = document.getElementById("field_888")
let field_889 = document.getElementById("field_889")
let field_890 = document.getElementById("field_890")
let field_893 = document.getElementById("field_893")

//validar colores cuotas ascard
let field_898 = document.getElementById("field_898")
let field_899 = document.getElementById("field_899")
let field_902 = document.getElementById("field_902")
let field_903 = document.getElementById("field_903")

//validar prepago
let field_907 = document.getElementById("field_907")
let field_909 = document.getElementById("field_909")
let field_910 = document.getElementById("field_910")
let field_911 = document.getElementById("field_911")
let field_912 = document.getElementById("field_912")
let field_913 = document.getElementById("field_913")
let field_914 = document.getElementById("field_914")
let field_915 = document.getElementById("field_915")
let field_916 = document.getElementById("field_916")

//validar eliminacion de centrales
let field_921 = document.getElementById("field_921")
let field_923 = document.getElementById("field_923")
let field_924 = document.getElementById("field_924")
let field_925 = document.getElementById("field_925")
let field_927 = document.getElementById("field_927")
let field_928 = document.getElementById("field_928")

boton.addEventListener("click", (event) => {
    event.preventDefault()
    if (field_872.value === "") {
        field_872.style.borderColor = "red"
        field_872.focus()
    }

    if (field_860.value === "") {
        field_860.style.borderColor = "red"
        field_860.focus()
    }

    if (field_877.value === "") {
        field_877.style.borderColor = "red"
        field_877.focus()
    }

    if (field_871.value === "") {
        field_871.style.borderColor = "red"
        field_871.focus()
    }

    if (field_878.value === "") {
        field_878.style.borderColor = "red"
        field_878.focus()
    }

    if (field_876.value === "") {
        field_876.style.borderColor = "red"
        field_876.focus()
    }

    if (field_875.value === "") {
        field_875.style.borderColor = "red"
        field_875.focus()
    }

    if (field_869.value === "") {
        field_869.style.borderColor = "red"
        field_869.focus()
    }

    if (field_882.value === "") {
        field_882.style.borderColor = "red"
        field_882.focus()
    }

    if (field_881.value === "") {
        field_881.style.borderColor = "red"
        field_881.focus()
    }

    if (field_874.value === "") {
        field_874.style.borderColor = "red"
        field_874.focus()
    }

    if (field_867.value === "") {
        field_867.style.borderColor = "red"
        field_867.focus()
    }

    if (field_866.value === "") {
        field_866.style.borderColor = "red"
        field_866.focus()
    }

    if (field_865.value === "") {
        field_865.style.borderColor = "red"
        field_865.focus()
    }

    if (field_864.value === "") {
        field_864.style.borderColor = "red"
        field_864.focus()
    }
    //ascard
    if (field_883.value === "") {
        field_883.style.borderColor = "red"
        field_883.focus()
    }
    if (field_884.value === "") {
        field_884.style.borderColor = "red"
        field_884.focus()
    }
    if (field_892.value === "") {
        field_892.style.borderColor = "red"
        field_892.focus()
    }
    if (field_888.value === "") {
        field_888.style.borderColor = "red"
        field_888.focus()
    }
    if (field_889.value === "") {
        field_889.style.borderColor = "red"
        field_889.focus()
    }
    if (field_890.value === "") {
        field_890.style.borderColor = "red"
        field_890.focus()
    }
    if (field_893.value === "") {
        field_893.style.borderColor = "red"
        field_893.focus()
    }

    //cuotas ascard
    if (field_898.value === "") {
        field_898.style.borderColor = "red"
        field_898.focus()
    }
    if (field_899.value === "") {
        field_899.style.borderColor = "red"
        field_899.focus()
    }
    if (field_902.value === "") {
        field_902.style.borderColor = "red"
        field_902.focus()
    }
    if (field_903.value === "") {
        field_903.style.borderColor = "red"
        field_903.focus()
    }

    //prepago
    if (field_907.value === "") {
        field_907.style.borderColor = "red"
        field_907.focus()
    }
    if (field_909.value === "") {
        field_909.style.borderColor = "red"
        field_909.focus()
    }
    if (field_910.value === "") {
        field_910.style.borderColor = "red"
        field_910.focus()
    }
    if (field_911.value === "") {
        field_911.style.borderColor = "red"
        field_911.focus()
    }
    if (field_912.value === "") {
        field_912.style.borderColor = "red"
        field_912.focus()
    }
    if (field_913.value === "") {
        field_913.style.borderColor = "red"
        field_913.focus()
    }
    if (field_914.value === "") {
        field_914.style.borderColor = "red"
        field_914.focus()
    }

    //eliminacion de centrales
    if (field_921.value === "") {
        field_921.style.borderColor = "red"
        field_921.focus()
    }
    if (field_923.value === "") {
        field_923.style.borderColor = "red"
        field_923.focus()
    }
    if (field_924.value === "") {
        field_924.style.borderColor = "red"
        field_924.focus()
    }
    if (field_925.value === "") {
        field_925.style.borderColor = "red"
        field_925.focus()
    }
    if (field_927.value === "") {
        field_927.style.borderColor = "red"
        field_927.focus()
    }
    if (field_928.value === "") {
        field_928.style.borderColor = "red"
        field_928.focus()
    }

})


function textocajas() {
    field_872.addEventListener("keyup", () => {
        field_872.style.borderColor = "#d1d3e2"
    })
    field_860.addEventListener("keyup", () => {
        field_860.style.borderColor = "#d1d3e2"
    })
    field_877.addEventListener("keyup", () => {
        field_877.style.borderColor = "#d1d3e2"
    })
    field_871.addEventListener("keyup", () => {
        field_871.style.borderColor = "#d1d3e2"
    })
    field_878.addEventListener("keyup", () => {
        field_878.style.borderColor = "#d1d3e2"
    })
    field_876.addEventListener("keyup", () => {
        field_876.style.borderColor = "#d1d3e2"
    })
    field_875.addEventListener("keyup", () => {
        field_875.style.borderColor = "#d1d3e2"
    })
    field_869.addEventListener("keyup", () => {
        field_869.style.borderColor = "#d1d3e2"
    })
    field_882.addEventListener("keyup", () => {
        field_882.style.borderColor = "#d1d3e2"
    })
    field_880.addEventListener("keyup", () => {
        field_880.style.borderColor = "#d1d3e2"
    })
    field_879.addEventListener("keyup", () => {
        field_879.style.borderColor = "#d1d3e2"
    })
    field_881.addEventListener("keyup", () => {
        field_881.style.borderColor = "#d1d3e2"
    })
    field_874.addEventListener("keyup", () => {
        field_874.style.borderColor = "#d1d3e2"
    })
    field_867.addEventListener("keyup", () => {
        field_867.style.borderColor = "#d1d3e2"
    })
    field_866.addEventListener("keyup", () => {
        field_866.style.borderColor = "#d1d3e2"
    })
    field_865.addEventListener("keyup", () => {
        field_865.style.borderColor = "#d1d3e2"
    })
    field_864.addEventListener("keyup", () => {
        field_864.style.borderColor = "#d1d3e2"
    })

    //ascard
    field_883.addEventListener("keyup", () => {
        field_883.style.borderColor = "#d1d3e2"
    })
    field_884.addEventListener("keyup", () => {
        field_884.style.borderColor = "#d1d3e2"
    })
    field_894.addEventListener("keyup", () => {
        field_894.style.borderColor = "#d1d3e2"
    })
    field_888.addEventListener("keyup", () => {
        field_888.style.borderColor = "#d1d3e2"
    })
    field_889.addEventListener("keyup", () => {
        field_889.style.borderColor = "#d1d3e2"
    })
    field_890.addEventListener("keyup", () => {
        field_890.style.borderColor = "#d1d3e2"
    })
    field_893.addEventListener("keyup", () => {
        field_893.style.borderColor = "#d1d3e2"
    })

    //cuotas 
    field_898.addEventListener("keyup", () => {
        field_898.style.borderColor = "#d1d3e2"
    })
    field_899.addEventListener("keyup", () => {
        field_899.style.borderColor = "#d1d3e2"
    })
    field_902.addEventListener("keyup", () => {
        field_902.style.borderColor = "#d1d3e2"
    })
    field_903.addEventListener("keyup", () => {
        field_903.style.borderColor = "#d1d3e2"
    })

    //prepago 
    field_907.addEventListener("keyup", () => {
        field_907.style.borderColor = "#d1d3e2"
    })
    field_909.addEventListener("keyup", () => {
        field_909.style.borderColor = "#d1d3e2"
    })
    field_910.addEventListener("keyup", () => {
        field_910.style.borderColor = "#d1d3e2"
    })
    field_911.addEventListener("keyup", () => {
        field_911.style.borderColor = "#d1d3e2"
    })
    field_913.addEventListener("keyup", () => {
        field_913.style.borderColor = "#d1d3e2"
    })
    field_914.addEventListener("keyup", () => {
        field_914.style.borderColor = "#d1d3e2"
    })
    field_915.addEventListener("keyup", () => {
        field_915.style.borderColor = "#d1d3e2"
    })
    field_916.addEventListener("keyup", () => {
        field_916.style.borderColor = "#d1d3e2"
    })

    //eliminacion 
    field_921.addEventListener("keyup", () => {
        field_921.style.borderColor = "#d1d3e2"
    })
    field_923.addEventListener("keyup", () => {
        field_923.style.borderColor = "#d1d3e2"
    })
    field_924.addEventListener("keyup", () => {
        field_924.style.borderColor = "#d1d3e2"
    })
    field_925.addEventListener("keyup", () => {
        field_925.style.borderColor = "#d1d3e2"
    })
    field_927.addEventListener("keyup", () => {
        field_927.style.borderColor = "#d1d3e2"
    })
    field_928.addEventListener("keyup", () => {
        field_928.style.borderColor = "#d1d3e2"
    })
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
    ocultar_prepago1();

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
    {
        debugger
        $BtnGuardar.addEventListener('click', async (event) => {
            $('.content-wrapper .form-control').removeAttr('required');
            try {
                debugger
                FuncBtnLoader($BtnGuardar, 1);
                var $elementRequired = $('#formCreate .required');
                var $elementRequired2 = $('#formCreate .requiredForever');
                var ValidaOthers = true;
                var Valida = true;
                var status = ["1", "593", "594", "595","294","332","422" ]; //ESTADOS NO OBLIGATORIOS PARA AIRE 294 332 422
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

                     var Min = $('#field_878').val().toUpperCase();
                     //console.log(mdm_pqr.length);
                    if (Min == '' ) {

                        
                    } else if (Min.length > 6) {
                        swal({
                            title: '¡Alerta automatica!',
                            text: 'Campo Min debe tener minimo 10 caracteres para poder guardar',
                        })

                        event.preventDefault();
                        return false;
                    }
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
                }else if ($('#field_872').val() != '' && $('#field_878').val().toUpperCase().length<7) {
                //    //console.log(mdm_pqr.length);
                        swal({
                        title: '¡Alerta automatica!',
                        text: 'Todos los campos son obligatorios para poder guardar',
                    })

                    event.preventDefault();
                    return false;

                } else if ($('#field_872').val() != '' && $('#field_878').val().toUpperCase().length < 7 ) {
                    //    //console.log(mdm_pqr.length);
                    swal({
                        title: '¡Alerta automatica!',
                        text: 'Todos los campos son obligatorios para poder guardar',
                    })

                    event.preventDefault();
                    return false;

                }

                else if ($('#field_149').val() != "4500" && $('#field_145').val() == "--Seleccionar--" || $('#field_149').val() != "4501" && $('#field_145').val() == "--Seleccionar--") {
                    swal({ title: 'Debe seleccionar un ajuste!', type: 'error', timer: 5000 })
                    event.preventDefault();
                    return false;

                }else if ($('#field_150').val() === "212" && control == 0) {

                        swal({ title: 'Selecciono ajuste sin gestionarlo!', type: 'error',timer:5000 })
                        event.preventDefault();
                        return false; 
                } 


                if ($('#field_878').val() != '' && $('#field_878').val().toUpperCase().length < 10) {
                    //    //console.log(mdm_pqr.length);

                    swal({
                        title: '¡Alerta automatica!',
                        text: 'Todos los campos son obligatorios para poder guardar, MIN debe tener 10 digitos',
                    })

                    event.preventDefault();
                    return false;
                }

                if ($('#field_910').val() != '' && $('#field_910').val().toUpperCase().length < 10) {
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
    ocultar_eliminacioncentrales1();
    ocultar_ascard1();
    ocultar_cuotasascard1();
    ocultar_postpago1();
    ocultar_prepago1();

    
    //console.log($("#field_149").val());

    cambiolista()
    //$('#field_876').get(0).type = 'number';
    
    OcultarcamposplanillaMovil();
    var idAjusteMovil = $("#IdTemplate_SuicheMovil").val();
    if (idAjusteMovil === '28') {
        $("#PlanillAjusteMovil").show();
    } else {
        $("#PlanillAjusteMovil").hide(); 
    }
    
})();

async function cambiolista() {
   
    var sele = $("#field_156").val();
        var $option = '<option value="">--Seleccionar--</option>';
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
    $('#field_977').html($option);


}


async function onChangeList($select) {
   /* alert($select.value);*/
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
    var cmb_AjusteMovil = $("#field_977").val();

    if (cmb_AjusteMovil === '4784' || cmb_AjusteMovil === '4783' || cmb_AjusteMovil === '4789' || cmb_AjusteMovil === '4788') {
        $("#Acard1").show(); //boton ascard
        $("#BtBPB").show(); //boton postpago
        $("#btncuotaAcard").show(); // boton cuotas ascard
        $("#btnprepago").show(); //btn prepago
        
        
        $("#BtnGuardar_S").attr('disabled', true);

        $("#field_882").addClass('form-control Req-2 fieldSave requiredForever');
        $("#field_259").addClass('form-control Req-2 fieldSave requiredForever');
    } else {
        $("#Acard1").hide(); //boton ascard
        $("#BtBPB").hide(); //boton postpago
        $("#btncuotaAcard").hide(); // boton cuotas ascard
        $("#btnprepago").hide(); //btn prepago
        $("#btnEliminacionCentrales").hide(); // eliminacion de centrales
    }
    if (cmb_AjusteMovil === '4786' || cmb_AjusteMovil === '4791') {
        $("#btnEliminacionCentrales").show(); // eliminacion de centrales
        BtnGuardar_S.disabled = true;
    } else {
        $("#btnEliminacionCentrales").hide(); // eliminacion de centrales
    }

    if (cmb_AjusteMovil === '4576' ) {
        
        ocultar_ascard1();
        ocultar_cuotasascard1();
        ocultar_eliminacioncentrales1();
        ocultar_postpago1();
        ocultar_prepago1();

        $("#field_882").removeClass('form-control Req-2 fieldSave requiredForever');
        $("#field_259").removeClass('form-control Req-2 fieldSave requiredForever');
        //$("#btnEliminacionCentrales").show(); // eliminacion de centrales
        $("#BtnGuardar_S").attr('disabled', true);
    }

    if (($('#field_156').val() == '4574' || $('#field_156').val() == '4575') && ($('#field_977').val() == '4789' || $('#field_977').val() == '4788' || $('#field_977').val() == '4791' || $('#field_977').val() == '4783') || ($('#field_156').val() == '4784') && ($('#field_977').val() == '4786')) {
        //    //console.log(mdm_pqr.length);

        //$("#field_882").removeClass('Req-2 fieldSave requiredForever');
        //$("#field_259").removeClass('Req-2 fieldSave requiredForever');
        $("#BtnGuardar_S").attr('disabled', true);

    } else {//if (($('#field_156').val() == '4574' || $('#field_156').val() == '4575') && ($('#field_977').val() == '4785' || $('#field_977').val() == '4787' || ($('#field_977').val() == '4790' || $('#field_977').val() == '4792'))) {
        //$("#field_259").addClass('form-control Req-2 fieldSave requiredForever');
        //$("#field_882").addClass('form-control Req-2 fieldSave requiredForever');
        $("#BtnGuardar_S").attr('disabled', false);

    }
       

    //else if (cmb_AjusteMovil != '4570' || cmb_AjusteMovil != '4565') {
    //    $("#btnEliminacionCentrales").hide(); // eliminacion de centrales
    //    $("#BtnGuardar_S").attr('disabled', false);
    //}

    var cmb_AjusteMovil = $("#field_149").val();// ocultar campo ajuste cuando en favorabilidad se escoge la opcion caso no favorable
    var cmb_AjusteMovil1 = $("#field_150").val();
    if (cmb_AjusteMovil === '4502') { //NO FAVORABLE
        $("#field_150").removeClass('requiredForever');
        $("#div_150").css({ 'display': 'none' });
        ocultar_ascard1();
        ocultar_cuotasascard1();
        ocultar_eliminacioncentrales1();
        ocultar_postpago1();
        ocultar_prepago1();
        $("#BtnGuardar_S").attr('disabled', false);
    } else {
        $("#div_150").css({ 'display': '' });
    }

    if (cmb_AjusteMovil1 == '4505' || cmb_AjusteMovil1 == '4510') {
        swal({
            title: '¡Alerta automatica!',
            text: 'Recuerda hacer actualizacion o modificacion de centrales',
        })
    } else {

    }

    if ((cmb_AjusteMovil == '4500' || cmb_AjusteMovil == '4501') && (cmb_AjusteMovil1 === '4505' || cmb_AjusteMovil1 === '4507' || cmb_AjusteMovil1 === '4510' || cmb_AjusteMovil1 === '4512')) {
        
        ocultar_ascard1();
        ocultar_cuotasascard1();
        ocultar_eliminacioncentrales1();
        ocultar_postpago1();
        ocultar_prepago1();
        $("#BtnGuardar_S").attr('disabled', false);
    } 
    if (cmb_AjusteMovil === '4500' || cmb_AjusteMovil === '4501') {
        $("#field_149").removeClass('form-control Req-2 fieldSave requiredForever');
        $("#field_149").addClass('form-control Req-1 fieldSave');
        $("#field_150").removeClass('form-control Req-2 fieldSave requiredForever');
        $("#field_150").addClass('form-control Req-1 fieldSave');
        ocultar_ascard1();
        ocultar_cuotasascard1();
        ocultar_eliminacioncentrales1();
        ocultar_postpago1();
        ocultar_prepago1();
        $("#BtnGuardar_S").attr('disabled', true);
    }

    if (($("#field_156").val() == '4574' || $("#field_156").val() == '4575') && ($("#field_977").val() == '4785' || $("#field_977").val() == '4787' || $("#field_977").val() == '4790' || $("#field_977").val() == '4792')) {
        $("#BtnGuardar_S").attr('disabled', false);
        $("#field_259").removeClass('requiredForever');
        $("#field_882").removeClass('requiredForever');
    } else if ($("#field_156").val() == '4576' && $("#field_977").val() == '4793') {
        $("#field_259").removeClass('requiredForever');
        $("#field_882").removeClass('requiredForever');
    } else {
        $("#field_259").addClass('requiredForever');
        $("#field_882").addClass('requiredForever');
    }

    if (($("#field_156").val() == '4574' || $("#field_156").val() == '4575') && ($("#field_977").val() == '4789' || $("#field_977").val() == '4788' || $("#field_977").val() == '4791' || $("#field_977").val() == '4784' || $("#field_977").val() == '4786' || $("#field_977").val() == '4783')) {
        $("#BtnGuardar_S").attr('disabled', true);
    }
    
    //var cmb_AjustePospago = $("#field_866").val();
    //if (cmb_AjustePospago != 0) {
    //    $("#BtnGuardar_S").attr('disabled', false);
    //}
    



    // llenar valor ajuste de ascard cuando se cambia la descripcion motivo de ajuste
    var cmb_motivoAjuste = $("#field_885").val();
    if (cmb_motivoAjuste === '4652') {
        $("#field_891").val('8');
    } else if (cmb_motivoAjuste === '4653') {
        $("#field_891").val('25');
    } else if (cmb_motivoAjuste === '4654') {
        $("#field_891").val('26');
    } else if (cmb_motivoAjuste === '4655') {
        $("#field_891").val('27');
    } else if (cmb_motivoAjuste === '4673') {
        $("#field_891").val('61');
    } else if (cmb_motivoAjuste === '4650') {
        $("#field_891").val('85');
    } else if (cmb_motivoAjuste === '4651') {
        $("#field_891").val('86');
    } else if (cmb_motivoAjuste === '4675') {
        $("#field_891").val('15');
    } else if (cmb_motivoAjuste === '4600') {
        $("#field_891").val('38');
    } else if (cmb_motivoAjuste === '4603') {
        $("#field_891").val('36');
    } else if (cmb_motivoAjuste === '4601') {
        $("#field_891").val('37');
    } else if (cmb_motivoAjuste === '4657') {
        $("#field_891").val('31');
    } else if (cmb_motivoAjuste === '4604') {
        $("#field_891").val('34');
    } else if (cmb_motivoAjuste === '4602') {
        $("#field_891").val('35');
    } else if (cmb_motivoAjuste === '4658') {
        $("#field_891").val('32');
    } else if (cmb_motivoAjuste === '4659') {
        $("#field_891").val('77');
    } else if (cmb_motivoAjuste === '4660') {
        $("#field_891").val('81');
    } else if (cmb_motivoAjuste === '4661') {
        $("#field_891").val('82');
    } else if (cmb_motivoAjuste === '4662') {
        $("#field_891").val('28');
    } else if (cmb_motivoAjuste === '4663') {
        $("#field_891").val('29');
    } else if (cmb_motivoAjuste === '4664') {
        $("#field_891").val('30');
    } else if (cmb_motivoAjuste === '4665') {
        $("#field_891").val('33');
    } else if (cmb_motivoAjuste === '4666') {
        $("#field_891").val('4');
    } else if (cmb_motivoAjuste === '4667') {
        $("#field_891").val('6');
    } else if (cmb_motivoAjuste === '4668') {
        $("#field_891").val('5');
    } else if (cmb_motivoAjuste === '4669') {
        $("#field_891").val('47');
    } else if (cmb_motivoAjuste === '4670') {
        $("#field_891").val('9');
    } else if (cmb_motivoAjuste === '4671') {
        $("#field_891").val('10');
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

    if ((value == '292') || (value == '293')) {
        //$("#field_882").revomeClass('requiredForever');
        //$("#field_259").removeClass('requiredForever');
        //$("#field_156").removeClass('required');
        BtnGuardar_S.disabled = true;
    } else {
        //$("#field_882").addClass('form-control Req-2 fieldSave requiredForever');
        //$("#field_259").addClass('form-control Req-2 fieldSave requiredForever');
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

async function showAdjustScreen11() { // postpago
    if (flag == 0) {
        swal("Activacion de Ajuste BPB!", "Se debe diligenciar el campo de Ajustes en SI para realizar mas de un ajuste", "success");
        BtnGuardar_S.disabled = false;

        swal({
            title: '¡Alerta automatica!',
            text: 'Todos los campos son obligatorios.',
        })

        // AGREGADOS NUEVOS
        $("#div_872").show(); // analista postpago
        $("#field_872").val().toUpperCase();
        $("#field_872").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_860").show(); // user red postpago
        $("#field_860").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_877").show(); // cun postpago
        $("#field_877").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_871").show(); //tipo reclamo postpago
        $("#field_871").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_878").show(); // min postpago
        $("#field_878").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_876").show(); // custcode postpago
        $("#field_876").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_875").show(); // customerid postpago
        $("#field_875").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_870").show(); //servicio 800 postpago
        $("#field_870").addClass('form-control Req-2 fieldSave requiredForever');
        //field_870.disabled=true; //servicio 800 postpago
        $("#div_869").show(); //cta contable 801 postpago
        $("#field_869").addClass('form-control Req-2 fieldSave requiredForever');
        field_869.disabled = true;
        $("#div_868").show(); //iva 802 postpago
        $("#field_868").addClass('form-control Req-2 fieldSave requiredForever');
        field_868.disabled = true; // iva
        $("#div_882").show(); // valor postpago
        document.getElementById('field_882').type = 'number';
        $("#field_882").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_880").show(); // periodo a ajustar postpago
        $("#field_880").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_879").show(); // periodo ajustar hasta postpago
        $("#field_879").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_881").show(); // justificacion postpago
        $("#field_881").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_873").show(); // usuario ajuste postpago
        $("#field_873").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_874").show(); // AREA GENERO AJUSTE postpago
        $("#field_874").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_861").show(); //area que genero inconsistencia postpago
        $("#field_861").addClass('form-control Req-2 fieldSave requiredForever');
        if (document.getElementById('field_861').value == "") {
            $("#field_861").val('AIB PQR ESCRITA');
        }
        $("#div_867").show(); //gerencia 803 postpago
        $("#field_867").addClass('form-control Req-2 fieldSave requiredForever');
        if (document.getElementById('field_867').value == "") {
            $("#field_867").val('GERENCIA GESTION PQRs');
        }
        $("#div_866").show(); //centrales 804 postpago
        $("#field_866").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_865").show(); //aliado 805 postpago
        $("#field_865").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_864").show(); //causal 806 postpago
        $("#field_864").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_726").css({ 'display': 'none' }); // segmento
        $("#div_863").css({ 'display': '' }); //estado 807 postpago
        if ($("#field_863").val() != "APLICADO" && $("#field_863").val() != "RECHAZADO" && $("#field_863").val() != "Aplicado" && $("#field_863").val() != "Rechazado") {
            $("#field_863").val("Pte aplicar ajuste");
        }
        $("#field_863").attr('readonly', true);
        $("#div_810").css({ 'display': '' }); //fecha creacion 808
        $("#div_862").css({ 'display': '' }); //fecha ultima actualizacion 809 postpago
        $("#botonexport").show(); //boton exportar postpago
        var date = new Date();
        var fecha = date.getFullYear() + '/' + (date.getMonth() + 1) + '/' + date.getDate();
        var hora = date.getHours() + ':' + date.getMinutes() + ':' + date.getSeconds();
        var fechaYHora = fecha + ' ' + hora;
        $("#field_862").val(fechaYHora);
        $('#field_810').val(fechaYHora); //actualizacion anterior
        $("#field_863").attr('readonly', true);
        $("#lbl_810").hide();
        $("#field_810").hide();
        $("#lbl_862").hide();
        $("#field_862").hide();
        $("#div_810").hide();
        $("#div_862").css({ 'display': 'none' });


        restaurar_colores_ascard1();
        restaurar_colores_centrales1();
        restaurar_colores_cuotas1();
        restaurar_colores_prepago1();

        $("#BtnPrepago").hide();
        $("#BtnPospago").show();
        $("#BtnAscard").hide();
        $("#BtnCuotasAscard").hide();
        $("#BtnEliminacionCentrales").hide();
        $("#divTableprepago13").hide();
        $("#divTableascard13").hide();
        $("#divTablepospago13").show();
        $("#divTableascard213").hide();
        $("#divTableeliminar13").hide();

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

        ocultar_postpago1();

        restaurar_colores_ascard1();
        restaurar_colores_centrales1();
        restaurar_colores_cuotas1();
        restaurar_colores_prepago1();

        flag = 0;
        control = control - 1;
    }
    
}//postpago         OK
async function showAdjustScreen21() { // ascard
    
    if (flag == 0) {
        swal("Activacion de Ajuste Ascard!", "Se debe diligenciar el campo de Ajustes en SI para realizar mas de un ajuste", "success");
        BtnGuardar_S.disabled = false;


        swal({
            title: '¡Alerta automatica!',
            text: 'Todos los campos son obligatorios.',
        })


        $("#field_882").removeClass(' requiredForever');
        $("#div_882").css({ 'display': 'none' }); // valor postpago
        //agregados
        $("#div_883").show();//numero de credito 731 ascard
        $("#field_883").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_884").show();//valor del ajuste 732 ascard
        $("#field_884").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_891").show();//motivo del ajuste 810 ascard
        $("#field_891").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_885").show();//descripcion motivo del ajuste 733 ascard
        $("#field_885").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_886").show();//area que solicita el ajuste 734 ascard
        $("#field_886").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_887").show();//comentari0 735 ascard
        $("#field_887").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_892").show();//nombre quien solicita 811 ascard
        $("#field_892").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_888").show();//documento del usuario 736 ascard
        $("#field_888").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_889").show();//reclamo del usuario 737 ascard
        $("#field_889").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_890").show();//custcode asociado al credito 739 ascard
        $("#field_890").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_893").show();//cun/nr 812 ascard
        $("#field_893").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_894").show();//tipo de reclamo 813 ascard
        $("#field_894").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_895").show();//aliado 814 ascard
        $("#field_895").addClass('form-control Req-2 fieldSave requiredForever');
        if ($("#field_896").val() != "APLICADO" && $("#field_896").val() != "RECHAZADO" && $("#field_896").val() != "Aplicado" && $("#field_896").val() != "Rechazado") {
            $("#field_896").val("Pte aplicar ajuste");
        }
        $("#field_896").attr('readonly', true);

        var date = new Date();
        var fecha = date.getFullYear() + '/' + (date.getMonth() + 1) + '/' + date.getDate();
        var hora = date.getHours() + ':' + date.getMinutes() + ':' + date.getSeconds();
        var fechaYHora = fecha + ' ' + hora;
        $("#field_897").val(fechaYHora);

        $("#field_882").removeClass('requiredForever');


         //actualizacion anterior
        //$("#div_815").hide();//estado 815 ascard
        $("#div_896").show();//estado
        $("#div_897").hide();//ultima actualizacion 817 ascard

        $("#Exportarascard").show(); //boton exportar ascard

        restaurar_colores_centrales1();
        restaurar_colores_cuotas1();
        restaurar_colores_postpago1();
        restaurar_colores_prepago1();

        $("#BtnPrepago").hide();
        $("#BtnPospago").hide();
        $("#BtnAscard").show();
        $("#BtnCuotasAscard").hide();
        $("#BtnEliminacionCentrales").hide();
        $("#divTableprepago13").hide();
        $("#divTableascard13").show();
        $("#divTablepospago13").hide();
        $("#divTableascard213").hide();
        $("#divTableeliminar13").hide();

        flag = 1;
        control = control + 1;
    }
    else {
        swal("Desactivacion de Ajuste Ascard!", "Se debe diligenciar el campo de Ajuste en NO", "success");
        //$("#lbl_725").show();
        //$("#field_725").show();
        //$("#field_725").removeClass('form-control Req-2 fieldSave requiredForever');
        //$("#field_725").addClass('form-control Req-1 fieldSave');

        ocultar_ascard1();

        $("#Exportarascard").hide(); //boton exportar ascard

        restaurar_colores_centrales1();
        restaurar_colores_cuotas1();
        restaurar_colores_postpago1();
        restaurar_colores_prepago1();

        flag = 0;
        control = control - 1;
    }

}//ascard           OK
async function showAdjustScreen31() {// cuotas ascard    OK
    
    if (flag == 0)
    {
        swal("Activacion de Ajuste Ascard 2!", "Se debe diligenciar el campo de Ajustes en SI para realizar mas de un ajuste", "success");
        BtnGuardar_S.disabled = false;


        swal({
            title: '¡Alerta automatica!',
            text: 'Todos los campos son obligatorios.',
        })


        $("#field_882").removeClass(' requiredForever');
        $("#div_882").css({ 'display': 'none' }); // valor postpago
        //agregados
        $("#div_898").show();//numero de credito 740 cuotas ascard
        $("#field_898").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_899").show();//valor nueva cuota 741 cuotas ascard
        $("#field_899").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_900").show();//concepto 742 cuotas ascard cuotas ascard
        $("#field_900").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_901").show();//area que solicita ajuste 743 cuotas ascard
        $("#field_901").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_902").show();//reclamo del usuario 744 cuotas ascard
        $("#field_902").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_903").show();//cantidad de cuotas 745 cuotas ascard
        $("#field_903").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_904").show();//aliado 818 cuotas ascard
        $("#field_904").addClass('form-control Req-2 fieldSave requiredForever');
        if ($("#field_905").val() != "APLICADO" && $("#field_905").val() != "RECHAZADO" && $("#field_905").val() != "Aplicado" && $("#field_905").val() != "Rechazado") {
            $("#field_905").val("Pte aplicar ajuste");
        }
        $("#field_882").removeClass('form-control Req-2 fieldSave requiredForever');
        $("#div_905").show();//estado 819 cuotas ascard
        $("#field_905").attr('readonly', true);
        var date = new Date();
        var fecha = date.getFullYear() + '/' + (date.getMonth() + 1) + '/' + date.getDate();
        var hora = date.getHours() + ':' + date.getMinutes() + ':' + date.getSeconds();
        var fechaYHora = fecha+ ' ' + hora;
        //console.log(fechaYHora);
        //console.log($("#field_894").val("2022-06-01 00:00:00.000"))
        //console.log($("#field_895").val("2022-06-01 00:00:00.000"))

        $("#field_882").removeClass('requiredForever');
        $("#field_906").val(fechaYHora); // ultima actualizacion
        $("#Exportarcuotas").show(); //boton exportar cuotas

        restaurar_colores_ascard1();
        restaurar_colores_centrales1();
        restaurar_colores_postpago1();
        restaurar_colores_prepago1();

        $("#BtnPrepago").hide();
        $("#BtnPospago").hide();
        $("#BtnAscard").hide();
        $("#BtnCuotasAscard").show();
        $("#BtnEliminacionCentrales").hide();
        $("#divTableprepago13").hide();
        $("#divTableascard13").hide();
        $("#divTablepospago13").hide();
        $("#divTableascard213").show();
        $("#divTableeliminar13").hide();

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
        ocultar_cuotasascard1();

        restaurar_colores_ascard1();
        restaurar_colores_centrales1();
        restaurar_colores_postpago1();
        restaurar_colores_prepago1();

        flag = 0;
        control = control - 1;

    }
}//cuotas ascard    OK
async function showAdjustScreen41() // prepago
{   
    
    if (flag == 0) {
        swal("Activacion de Ajuste Incidencias PQR!", "Se debe diligenciar el campo de Ajustes en SI para realizar mas de un ajuste", "success");
        BtnGuardar_S.disabled = false;

        swal({
            title: '¡Alerta automatica!',
            text: 'Todos los campos son obligatorios.',
        })

        $("#field_882").removeClass(' requiredForever');

        $("#div_882").css({ 'display': 'none' }); // valor prepago

        //AGREGADOS

        $("#div_907").show();//radicado prepago
        $("#field_907").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_908").show();//tipo de reclamo prepago
        $("#field_908").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_909").show();//nombre de titular prepago
        $("#field_909").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_910").show();//min prepago
        $("#field_910").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_911").show();//custcode prepago
        $("#field_911").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_912").show();//valor prepago
        $("#field_912").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_913").show();//concepto prepago
        $("#field_913").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_914").show();//analista prepago
        $("#field_914").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_915").show();//periodo ajustado desde prepago
        $("#field_915").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_916").show();//periodo ajustado hasta prepago
        $("#field_916").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_917").show();//aliado prepago
        $("#field_917").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_918").show();//ESTADO prepago
        $("#field_918").addClass('form-control Req-2 fieldSave requiredForever');
        if ($("#field_918").val() != "APLICADO" && $("#field_918").val() != "RECHAZADO" && $("#field_918").val() != "Aplicado" && $("#field_918").val() != "Rechazado") {
            $("#field_918").val("Pte aplicar ajuste");
        }
        $("#field_918").attr('readonly', true);

        var date = new Date();
        var fecha = date.getFullYear() + '/' + (date.getMonth() + 1) + '/' + date.getDate();
        var hora = date.getHours() + ':' + date.getMinutes() + ':' + date.getSeconds();
        var fechaYHora = fecha + ' ' + hora;
        $("#field_919").val(fechaYHora); //actualizacion anterior
        
        $("#field_919").addClass('form-control Req-2 fieldSave requiredForever');
        $("#div_929").css({ 'display': 'none' });//estado prepago cubo
        $("#div_919").css({ 'display': 'none' });//actualizacion anterior prepago


        $("#field_882").removeClass('requiredForever');
        $("#Exportarprepago").show(); //boton exportar prepago

        restaurar_colores_ascard1();
        restaurar_colores_centrales1();
        restaurar_colores_cuotas1();
        restaurar_colores_postpago1();

        $("#BtnPrepago").show();
        $("#BtnPospago").hide();
        $("#BtnAscard").hide();
        $("#BtnCuotasAscard").hide();
        $("#BtnEliminacionCentrales").hide();
        $("#divTableprepago13").show();
        $("#divTableascard13").hide();
        $("#divTablepospago13").hide();
        $("#divTableascard213").hide();
        $("#divTableeliminar13").hide();

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
 
        ocultar_prepago1();
        
        restaurar_colores_ascard1();
        restaurar_colores_centrales1();
        restaurar_colores_cuotas1();
        restaurar_colores_postpago1();

        flag = 0;
        control = control - 1;
    }
}//prepago          OK
async function showAdjustScreen51() {

    if (flag == 0) {
        var field_782 = document.getElementById('field_782');
        //swal("Activacion de Ajuste BPB!", "Se debe diligenciar el campo de Ajustes en SI para realizar mas de un ajuste", "success");
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

        $("#field_882").removeClass('requiredForever');

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
        flag = 0;
        control = control - 1;
    }

}
async function showAdjustScreen61() { // eliminacion de centrales

    if (flag == 0) {
        swal("Activacion de Eliminacion de Centrales", "Se debe diligenciar el campo de Ajustes en SI para realizar mas de un ajuste", "success");
        BtnGuardar_S.disabled = false;

        swal({
            title: '¡Alerta automatica!',
            text: 'Todos los campos son obligatorios.',
        })

        $("#field_882").removeClass('requiredForever');
        $("#div_882").css({ 'display': 'none' }); // valor postpago

        //AGREGADOS
        $("#div_921").show();//cun centrales
        $("#field_921").addClass('requiredForever');
        $("#div_922").show();//tipo de documento centrales
        $("#field_922").addClass('requiredForever');
        $("#div_923").show();//nombrer centrales
        $("#field_923").addClass('requiredForever');
        $("#div_924").show();//cedula centrales
        $("#field_924").addClass('requiredForever');
        $("#div_925").show();//custcode No credito centrales
        $("#field_925").addClass('requiredForever');
        $("#div_926").show();//estado centrales
        $("#field_926").addClass('requiredForever');
        $("#div_927").show();//motivo de la elminacion centrales
        $("#field_927").addClass('requiredForever');
        $("#div_928").show();//analista centrales
        $("#field_928").addClass('requiredForever');
        if ($("#field_929").val() != "ELIMINADO" && $("#field_929").val() != "Eliminado" && $("#field_929").val() != "RECHAZADO" && $("#field_929").val() != "Rechazado") {
            $("#field_929").val("Pendiente Eliminar");
        }
        $("#field_929").attr('Readonly', true);

        var date = new Date();
        var fecha = date.getFullYear() + '/' + (date.getMonth() + 1) + '/' + date.getDate();
        var hora = date.getHours() + ':' + date.getMinutes() + ':' + date.getSeconds();
        var fechaYHora = fecha + ' ' + hora;
        $("#field_930").val(fechaYHora); //actualizacion anterior


        $("#div_929").css({ 'display': '' });//estado centrales cubo
        $("#div_930").hide;//ultima actualizacion centrales

        $("#ExportarEliminarCentrales").show(); //boton exportar Eliminar Centrales


        $("#field_882").removeClass('requiredForever');

        restaurar_colores_ascard1();
        restaurar_colores_cuotas1();
        restaurar_colores_postpago1();
        restaurar_colores_prepago1();

        $("#BtnPrepago").hide();
        $("#BtnPospago").hide();
        $("#BtnAscard").hide();
        $("#BtnCuotasAscard").hide();
        $("#BtnEliminacionCentrales").show();
        $("#divTableprepago13").hide();
        $("#divTableascard13").hide();
        $("#divTablepospago13").hide();
        $("#divTableascard213").hide();
        $("#divTableeliminar13").show();


        flag = 1;
        control = control + 1;
    }
    else {
        swal("Desactivacion de Eliminacion de Centrales", "Se debe diligenciar el campo de Ajuste en NO", "success");

        ocultar_eliminacioncentrales1();
        $("#ExportarEliminarCentrales").hide(); //boton exportar Eliminar Centrales

        restaurar_colores_ascard1();
        restaurar_colores_cuotas1();
        restaurar_colores_postpago1();
        restaurar_colores_prepago1();

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

/// buscar cuenta servicio e iva
//field_869.onblur = function () { // al perder el foco el campop cta contable ejecuta la funcion id consulta
//    //console.log(field_871.value);
//    Idconsulta(field_869.value); 
//};


async function changeCuenta($Select) {

 
    var cmb_cuenta = $("#field_870").val();
    if (cmb_cuenta != null) {
        //alert(field_800.value);
        var _cuenta = field_870.value
        //_cuenta = _cuenta.substring(0, _cuenta.length - 13)
        consultar_cuenta(_cuenta);
    }

}

var IDENT = null;
//function Idconsulta() {
//    IDENT = document.getElementById("field_869").value;
//    //console.log(IDENT);
//    consultar_cuenta(IDENT);
//}

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

    $('#field_869').val(objplantilla.cod_cuenta); //cta contable
    $('#field_870').val(objplantilla.servicio); // servicio
    $('#field_868').val(objplantilla.iva); // iva

    var elements = document.querySelectorAll("input[type='text']");
    for (var i = 0; i < elements.length; i++) {
        elements[i].addEventListener("focus", function () {
            inputfocused = this;
        });
    }
}

//gerencia
//field_874.onblur = function () { // al perder el foco el campo area que genero ajuste
//field_874.onblur = function () { // al perder el foco el campo area que genero ajuste
//    console.log(UDF_VARCHAR419.value);
//    console.log(field_874.value);
//    IdconsultaGerencia(field_874.value);
//};
async function changeArea($Select) {

    var cmb_area = $("#field_874").val();
    if (cmb_area != null) {
        IdconsultaGerencia(field_874.value);
    }
   
}


var IDE = null;
function IdconsultaGerencia() {
    IDE = document.getElementById("field_874").value;
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

    $('#field_874').val(objplantilla.area_genero_ajuste); //area que genero ajuste
    $('#field_867').val(objplantilla.gerencia); // gerencia

    var elements = document.querySelectorAll("input[type='text']");
    for (var i = 0; i < elements.length; i++) {
        elements[i].addEventListener("focus", function () {
            inputfocused = this;
        });
    }
}


function ocultar_postpago1() {
    // AGREGADOS NUEVOS
    //$("#field_150").removeClass('form-control Req-2 fieldSave requiredForever');//ajuste
    $("#field_872").removeClass(' requiredForever');
    $("#div_872").css({ 'display': 'none' }); // analista postpago
    $("#field_860").removeClass(' requiredForever');
    $("#div_860").css({ 'display': 'none' }); // user red postpago
    $("#field_877").removeClass(' requiredForever');
    $("#div_877").css({ 'display': 'none' }); // cun postpago
    $("#field_871").removeClass(' requiredForever');
    $("#div_871").css({ 'display': 'none' }); //tipo reclamo postpago
    $("#field_878").removeClass(' requiredForever');
    $("#div_878").css({ 'display': 'none' }); // min postpago
    $("#field_876").removeClass(' requiredForever');
    $("#div_876").css({ 'display': 'none' }); // custcode postpago
    $("#field_875").removeClass(' requiredForever');
    $("#div_875").css({ 'display': 'none' }); // customerid postpago
    $("#field_870").removeClass(' requiredForever');
    $("#div_870").css({ 'display': 'none' }); //servicio 800 postpago
    $("#field_869").removeClass(' requiredForever');
    $("#div_869").css({ 'display': 'none' }); //cta contable 801 postpago
    $("#field_868").removeClass(' requiredForever');
    $("#div_868").css({ 'display': 'none' }); //iva 802 postpago
    $("#field_882").removeClass(' requiredForever');
    $("#div_882").css({ 'display': 'none' }); // valor postpago
    $("#field_880").removeClass(' requiredForever');
    $("#div_880").css({ 'display': 'none' }); // periodo a ajustar postpago
    $("#field_879").removeClass(' requiredForever');
    $("#div_879").css({ 'display': 'none' }); // periodo ajustar hasta postpago
    $("#field_881").removeClass(' requiredForever');
    $("#div_881").css({ 'display': 'none' }); // justificacion postpago
    $("#field_874").removeClass(' requiredForever');
    $("#div_874").css({ 'display': 'none' }); //arwa que genero inconsistencia postpago
    $("#field_867").removeClass(' requiredForever');
    $("#div_867").css({ 'display': 'none' }); //gerencia 803 postpago
    $("#field_866").removeClass(' requiredForever');
    $("#div_866").css({ 'display': 'none' }); //centrales 804 postpago
    $("#field_865").removeClass(' requiredForever');
    $("#div_865").css({ 'display': 'none' }); //aliado 805 postpago
    $("#field_864").removeClass(' requiredForever');
    $("#div_864").css({ 'display': 'none' }); //causal 806 postpago
    $("#div_863").css({ 'display': 'none' }); //estado 807 postpago
    $("#field_863").removeClass(' requiredForever');
    $("#div_810").css({ 'display': 'none' }); //fecha creacion 808 postpago
    $("#field_810").removeClass(' requiredForever');
    $("#div_862").css({ 'display': 'none' }); //fecha ultima actualizacion 809 postpago
    $("#field_862").removeClass(' requiredForever');
    $("#div_726").css({ 'display': 'none' }); // segmento postpago postpago
    $("#field_726").removeClass(' requiredForever');
    $("#div_861").css({ 'display': 'none' }); // area que genero inconsistencia postpago postpago
    $("#field_861").removeClass(' requiredForever');
    $("#div_873").css({ 'display': 'none' }); // usuario ajuste postpago
    $("#field_873").removeClass(' requiredForever');
    $("#field_882").addClass('form-control Req-2 fieldSave requiredForever');
    $("#field_259").addClass('form-control Req-2 fieldSave requiredForever');
    document.getElementById('field_882').value == "";
    BtnGuardar_S.disabled = true;


    $("#BtnPrepago").hide();
    $("#BtnPospago").hide();
    $("#BtnAscard").hide();
    $("#BtnCuotasAscard").hide();
    $("#BtnEliminacionCentrales").hide();
    $("#divTableprepago13").hide();
    $("#divTableascard13").hide();
    $("#divTablepospago13").hide();
    $("#divTableascard213").hide();
    $("#divTableeliminar13").hide();

}
function ocultar_prepago1() {

    //AGREGADOS
    $("#field_907").removeClass('requiredForever');
    $("#div_907").css({ 'display': 'none' });//radicado prepago
    $("#field_908").removeClass('requiredForever');
    $("#div_908").css({ 'display': 'none' });//tipo de reclamo prepago
    $("#field_909").removeClass('requiredForever');
    $("#div_909").css({ 'display': 'none' });//nombre de titular prepago
    $("#field_910").removeClass('requiredForever');
    $("#div_910").css({ 'display': 'none' });//min prepago
    $("#field_911").removeClass('requiredForever');
    $("#div_911").css({ 'display': 'none' });//custcode prepago
    $("#field_912").removeClass('requiredForever');
    $("#div_912").css({ 'display': 'none' });//valor prepago
    $("#field_913").removeClass('requiredForever');
    $("#div_913").css({ 'display': 'none' });//concepto prepago
    $("#field_914").removeClass('requiredForever');
    $("#div_914").css({ 'display': 'none' });//analista prepago
    $("#field_915").removeClass('requiredForever');
    $("#div_915").css({ 'display': 'none' });//periodo ajustado desde prepago
    $("#field_916").removeClass('requiredForever');
    $("#div_916").css({ 'display': 'none' });//periodo ajustado hasta prepago
    $("#field_917").removeClass('requiredForever');
    $("#div_917").css({ 'display': 'none' });//aliado prepago
    $("#field_918").removeClass('requiredForever');
    $("#div_918").css({ 'display': 'none' });//aliado prepago
    $("#field_919").removeClass('requiredForever');
    $("#div_919").css({ 'display': 'none' });//aliado prepago
    $("#field_882").addClass('form-control Req-2 fieldSave requiredForever');
    $("#field_259").addClass('form-control Req-2 fieldSave requiredForever');
    document.getElementById('field_882').value == "";
    BtnGuardar_S.disabled = true;

    $("#BtnPrepago").hide();
    $("#BtnPospago").hide();
    $("#BtnAscard").hide();
    $("#BtnCuotasAscard").hide();
    $("#BtnEliminacionCentrales").hide();
    $("#divTableprepago13").hide();
    $("#divTableascard13").hide();
    $("#divTablepospago13").hide();
    $("#divTableascard213").hide();
    $("#divTableeliminar13").hide();

}
function ocultar_ascard1() {
    //agregados
    $("#field_883").removeClass(' requiredForever');
    $("#div_883").css({ 'display': 'none' });//numero de credito 731 ascard
    $("#field_884").removeClass(' requiredForever');
    $("#div_884").css({ 'display': 'none' });//valor del ajuste 732 ascard
    $("#field_891").removeClass(' requiredForever');
    $("#div_891").css({ 'display': 'none' });//motivo del ajuste 810 ascard
    $("#field_885").removeClass(' requiredForever');
    $("#div_885").css({ 'display': 'none' });//descripcion motivo del ajuste 733 ascard
    $("#field_886").removeClass(' requiredForever');
    $("#div_886").css({ 'display': 'none' });//area que solicita el ajuste 734 ascard
    $("#field_887").removeClass(' requiredForever');
    $("#div_887").css({ 'display': 'none' });//comentari0 735 ascard
    $("#field_892").removeClass(' requiredForever');
    $("#div_892").css({ 'display': 'none' });//nombre quien solicita 811 ascard
    $("#field_888").removeClass(' requiredForever');
    $("#div_888").css({ 'display': 'none' });//documento del usuario 736 ascard
    $("#field_889").removeClass(' requiredForever');
    $("#div_889").css({ 'display': 'none' });//reclamo del usuario 737 ascard
    $("#field_890").removeClass(' requiredForever');
    $("#div_890").css({ 'display': 'none' });//custcode asociado al credito ascard
    $("#field_893").removeClass(' requiredForever');
    $("#div_893").css({ 'display': 'none' });//cun/nr 812 ascard
    $("#field_894").removeClass(' requiredForever');
    $("#div_894").css({ 'display': 'none' });//tipo de reclamo 813 ascard
    $("#field_895").removeClass(' requiredForever');
    $("#div_895").css({ 'display': 'none' });//aliado 814 ascard
    $("#field_896").removeClass(' requiredForever');
    $("#div_896").css({ 'display': 'none' });//estado 815 ascard
    $("#field_897").removeClass(' requiredForever');
    $("#div_897").css({ 'display': 'none' });//ultima actualizacion 817 ascard
    $("#field_882").addClass('form-control Req-2 fieldSave requiredForever');
    $("#field_259").addClass('form-control Req-2 fieldSave requiredForever');
    document.getElementById('field_882').value == "";
    BtnGuardar_S.disabled = true;

    $("#BtnPrepago").hide();
    $("#BtnPospago").hide();
    $("#BtnAscard").hide();
    $("#BtnCuotasAscard").hide();
    $("#BtnEliminacionCentrales").hide();
    $("#divTableprepago13").hide();
    $("#divTableascard13").hide();
    $("#divTablepospago13").hide();
    $("#divTableascard213").hide();
    $("#divTableeliminar13").hide();

}
function ocultar_cuotasascard1() {
    //agregados
    $("#field_898").removeClass(' requiredForever');
    $("#div_898").css({ 'display': 'none' });//numero de credito 740 cuotas ascard
    $("#field_899").removeClass(' requiredForever');
    $("#div_899").css({ 'display': 'none' });//valor nueva cuota 741 cuotas ascard
    $("#field_900").removeClass(' requiredForever');
    $("#div_900").css({ 'display': 'none' });//concepto 742 cuotas ascard
    $("#field_901").removeClass(' requiredForever');
    $("#div_901").css({ 'display': 'none' });//area que solicita ajuste 743 cuotas ascard
    $("#field_902").removeClass(' requiredForever');
    $("#div_902").css({ 'display': 'none' });//reclamo del usuario 744 cuotas ascard
    $("#field_903").removeClass(' requiredForever');
    $("#div_903").css({ 'display': 'none' });//cantidad de cuotas 745 cuotas ascard
    $("#field_904").removeClass(' requiredForever');
    $("#div_904").css({ 'display': 'none' });//aliado 818 cuotas ascard
    $("#field_905").removeClass(' requiredForever');
    $("#div_905").css({ 'display': 'none' });//estado 819 cuotas ascard
    $("#field_906").removeClass(' requiredForever');
    $("#div_906").css({ 'display': 'none' });//ultima acxtualizacion 821 cuotas ascard
    $("#field_882").addClass('form-control Req-2 fieldSave requiredForever');
    $("#field_259").addClass('form-control Req-2 fieldSave requiredForever');
    document.getElementById('field_882').value == "";
    BtnGuardar_S.disabled = true;

    $("#BtnPrepago").hide();
    $("#BtnPospago").hide();
    $("#BtnAscard").hide();
    $("#BtnCuotasAscard").hide();
    $("#BtnEliminacionCentrales").hide();
    $("#divTableprepago13").hide();
    $("#divTableascard13").hide();
    $("#divTablepospago13").hide();
    $("#divTableascard213").hide();
    $("#divTableeliminar13").hide();

}
function ocultar_eliminacioncentrales1() {
    //AGREGADOS
    $("#field_921").removeClass(' requiredForever');
    $("#div_921").css({ 'display': 'none' });//cun  centrales
    $("#field_922").removeClass(' requiredForever');
    $("#div_922").css({ 'display': 'none' });//tipo de documento centrales
    $("#field_923").removeClass(' requiredForever');
    $("#div_923").css({ 'display': 'none' });//nombrer centrales
    $("#field_924").removeClass(' requiredForever');
    $("#div_924").css({ 'display': 'none' });//cedula centrales
    $("#field_925").removeClass(' requiredForever');
    $("#div_925").css({ 'display': 'none' });//custcode No credito centrales
    $("#field_926").removeClass(' requiredForever');
    $("#div_926").css({ 'display': 'none' });//estado centrales
    $("#field_927").removeClass(' requiredForever');
    $("#div_927").css({ 'display': 'none' });//motivo de la elminacion centrales
    $("#field_928").removeClass(' requiredForever');
    $("#div_928").css({ 'display': 'none' });//analista centrales
    $("#field_929").removeClass(' requiredForever');
    $("#div_929").css({ 'display': 'none' });//estado centrales cubo
    $("#field_930").removeClass(' requiredForever');
    $("#div_930").css({ 'display': 'none' });//ultima actualizacion centrales
    $("#field_921").removeClass(' requiredForever');
    $("#div_921").css({ 'display': 'none' });//cun  centrales
    $("#field_922").removeClass(' requiredForever');
    $("#div_922").css({ 'display': 'none' });//tipo de documento centrales
    $("#field_923").removeClass(' requiredForever');
    $("#div_923").css({ 'display': 'none' });//nombrer centrales
    $("#field_924").removeClass(' requiredForever');
    $("#div_924").css({ 'display': 'none' });//cedula centrales
    $("#field_925").removeClass(' requiredForever');
    $("#div_925").css({ 'display': 'none' });//custcode No credito centrales
    $("#field_926").removeClass(' requiredForever');
    $("#div_926").css({ 'display': 'none' });//estado centrales
    $("#field_927").removeClass(' requiredForever');
    $("#div_927").css({ 'display': 'none' });//motivo de la elminacion centrales
    $("#field_928").removeClass(' requiredForever');
    $("#div_928").css({ 'display': 'none' });//analista centrales
    $("#field_929").removeClass(' requiredForever');
    $("#div_929").css({ 'display': 'none' });//estado centrales cubo
    $("#field_930").removeClass(' requiredForever');
    $("#div_930").css({ 'display': 'none' });//ultima actualizacion centrales
    $("#field_882").addClass('form-control Req-2 fieldSave requiredForever');
    $("#field_259").addClass('form-control Req-2 fieldSave requiredForever');
    document.getElementById('field_882').value == "";
    BtnGuardar_S.disabled = true;

    $("#BtnPrepago").hide();
    $("#BtnPospago").hide();
    $("#BtnAscard").hide();
    $("#BtnCuotasAscard").hide();
    $("#BtnEliminacionCentrales").hide();
    $("#divTableprepago13").hide();
    $("#divTableascard13").hide();
    $("#divTablepospago13").hide();
    $("#divTableascard213").hide();
    $("#divTableeliminar13").hide();

}

// validaciones cun 16 digitos numericos


$("#field_876").on('keydown keypress', function (e) { //cuscode postpago


    cadena = document.getElementById("field_876").value
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
})

$("#field_903").on('keydown keypress', function (e) { //cuscode 
    var el = $("#field_903").val();
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

$("#field_877").on('keydown keypress', function (e){ //cun postpago
    if (e.key.length === 1) {
        if ($(this).val().length < 16 && !isNaN(parseFloat(e.key))) {
            $(this).val($(this).val() + e.key);
        }
        return false;
    }
})

$("#field_878").on('keydown keypress', function (e) { // min postpago
    if (e.key.length === 1) {
        if ($(this).val().length < 10 && !isNaN(parseFloat(e.key))) {
            $(this).val($(this).val() + e.key);
        }
        return false;
    }
})

$("#field_910").on('keydown keypress', function (e) { // min prepago
    if (e.key.length === 1) {
        if ($(this).val().length < 10 && !isNaN(parseFloat(e.key))) {
            $(this).val($(this).val() + e.key);
        }
        return false;
    }
})

$("#field_875").on('keydown keypress', function (e) { // customerid postpago
    if (e.key.length === 1) {
        if ($(this).val().length < 10 && !isNaN(parseFloat(e.key))) {
            $(this).val($(this).val() + e.key);
        }
        return false;
    }
})

$("#field_881").on('keydown keypress', function (e) { // justificacion postpago
    if (e.key.length === 1) {
        if ($(this).val().length < 250) {
            $(this).val($(this).val() + e.key);
        }
        return false;
    }
})

$("#field_883").on('keydown keypress', function (e) { // numero de credito ascard
    if (e.key.length === 1) {
        if ($(this).val().length < 16 && !isNaN(parseFloat(e.key))) {
            $(this).val($(this).val() + e.key);
        }
        return false;
    }
})

$("#field_888").on('keydown keypress', function (e) { // numero de credito ascard
    if (e.key.length === 1) {
        if ($(this).val().length < 26 && !isNaN(parseFloat(e.key))) {
            $(this).val($(this).val() + e.key);
        }
        return false;
    }
})

$("#field_889").on('keydown keypress', function (e) { // Reclamo del usuario ascard
    if (e.key.length === 1) {
        if ($(this).val().length < 250) {
            $(this).val($(this).val() + e.key);
        }
        return false;
    }
})

$("#field_902").on('keydown keypress', function (e) { // Reclamo del usuario cuotas ascard
    if (e.key.length === 1) {
        if ($(this).val().length < 250 ) {
            $(this).val($(this).val() + e.key);
        }
        return false;
    }
})

// formato a numero
$("#field_898").on('keydown keypress', function (e) { // nUMERO DE CREDITO CUOTAS ASCARD
    if (e.key.length === 1) {
        if ($(this).val().length < 16 && !isNaN(parseFloat(e.key))) {
            $(this).val($(this).val() + e.key);
        }
        return false;
    }
})

$("#field_924").on('keydown keypress', function (e) { // nUMERO DE CEDULA ELIMINACION DE CENTRALES
    if (e.key.length === 1) {
        if ($(this).val().length < 16 && !isNaN(parseFloat(e.key))) {
            $(this).val($(this).val() + e.key);
        }
        return false;
    }
})



//formato numerico con decimales
///// convertir string a moneda
//field_882.onblur = function () { //valor de postpago

//    return parseFloat($('#field_882').val());

//    //const money = document.getElementById("field_882").value;
//    //const currency = function (number) {
//    //    return new Intl.NumberFormat({ style: 'currency', minimumFractionDigits: 2 }).format(number);
//    //};
//    //var resultado = 0;
//    //$('#field_882').val(currency(money));
//}

$('#field_882').addClass('form-control Req-2 fieldSave requiredForever');

async function showAdjustScreen71() {
    location.href = "/WorkOrderSolutions/ExportarPostpago13/"

}

async function botonexport1() { // eliminacion de centrales

    //console.log("El id a editar es " + Id);
    //alert("El id a editar es " + Id);
    //alert("Archivo exportado a la carpeta Temp");
    $.post('/WorkOrderSolutions/ExportarPostpago13/');
    alert("Archivo exportado a la carpeta Temp");
}

async function exportarpre1() { // eliminacion de centrales

    //console.log("El id a editar es " + Id);
    //alert("El id a editar es " + Id);
    //alert("Archivo exportado a la carpeta Temp");
    location.href = "/WorkOrderSolutions/ExportarPrepago13/"
    //$.post('/WorkOrderSolutions/ExportarPrepago/');
    //alert("Archivo exportado a la carpeta Temp");
}

async function exportarascard1() { // eliminacion de centrales

    //console.log("El id a editar es " + Id);
    //alert("El id a editar es " + Id);
    //alert("Exportar");
    //$.post('/WorkOrderSolutions/ExportarAscard/');
    //alert("Archivo exportado a la carpeta Temp");

    location.href = "/WorkOrderSolutions/ExportarAscard13/"
}

async function exportarcuotas1() { // eliminacion de centrales

    //console.log("El id a editar es " + Id);
    //alert("El id a editar es " + Id);
    //alert("Exportar");
    //$.post('/WorkOrderSolutions/ExportarCuotasAscard/');
    //alert("Archivo exportado a la carpeta Temp");

    location.href = "/WorkOrderSolutions/ExportarCuotasAscard13/"
}

async function exportarEliminarCentrales1() { // eliminacion de centrales

    //console.log("El id a editar es " + Id);
    //alert("El id a editar es " + Id);
    //alert("Archivo exportado a la carpeta Temp");
    location.href = "/WorkOrderSolutions/ExportarEliminarCentrales13/"
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
});

// CUN POSTPAGO
$("#field_877").attr('maxlength', '16');
$("#field_877").on('keypress', function (e)
{
    return aplicarFormato(this, e, "CUN");
})

$("#field_877").on('blur', function ()
{
    return validarFormato(this, "CUN");

})

// CUSCODE POSTPAGO
$("#field_876").attr('maxlength', '20');
$("#field_876").on('keypress', function (e) {
    return aplicarFormato(this, e, "CUSTCODE");
})

$("#field_876").on('blur', function () {
    return validarFormato(this, "CUSTCODE");
})

// VALOR POSTPAGO
$("#field_882").on('keypress', function (e) {
    return aplicarFormato(this, e, "DECIMAL");
})

$("#field_882").on('blur', function () {
    return validarFormato(this, "DECIMAL");
})

// VALOR AJUSTE ASCARD1
$("#field_884").on('keypress', function (e) {
    return aplicarFormato(this, e, "DECIMAL");
})

$("#field_884").on('blur', function () {
    return validarFormato(this, "DECIMAL");
})

// CUSCODE ASCARD1
$("#field_890").attr('maxlength', '20');
$("#field_890").on('keypress', function (e) {
    return aplicarFormato(this, e, "CUSTCODE");
})

$("#field_890").on('blur', function () {
    return validarFormato(this, "CUSTCODE");
})

// CUN ASCARD
$("#field_893").attr('maxlength', '16');
$("#field_893").on('keypress', function (e) {
    return aplicarFormato(this, e, "CUN");
})

$("#field_893").on('blur', function () {
    return validarFormato(this, "CUN");
})

// VALOR AJUSTE ASCARD2
$("#field_899").on('keypress', function (e) {
    return aplicarFormato(this, e, "DECIMAL");
})

$("#field_899").on('blur', function () {
    return validarFormato(this, "DECIMAL");
})

// VALOR AJUSTE prepago
$("#field_912").on('keypress', function (e) {
    return aplicarFormato(this, e, "DECIMAL");
})

$("#field_912").on('blur', function () {
    return validarFormato(this, "DECIMAL");
})

// cun centrales
$("#field_921").attr('maxlength', '16');
$("#field_921").on('keypress', function (e) {
    return aplicarFormato(this, e, "CUN");
})

$("#field_921").on('blur', function () {
    return validarFormato(this, "CUN");
})


// CUSCODE ELIMINACION CENTRALES
$("#field_925").attr('maxlength', '20');
$("#field_925").on('keypress', function (e) {
    return aplicarFormato(this, e, "CUSTCODE");
})

$("#field_925").on('blur', function () {
    return validarFormato(this, "CUSTCODE");
})

// CUSCODE prepago
$("#field_911").attr('maxlength', '20');
$("#field_911").on('keypress', function (e) {
    return aplicarFormato(this, e, "CUSTCODE");
})

$("#field_911").on('blur', function () {
    return validarFormato(this, "CUSTCODE");
})

//// CUSTOMERID POSTPAGO
$("#field_875").attr('maxlength', '10');
$("#field_875").on('keypress', function (e) {
    return aplicarFormato(this, e, "CUSTOMERID");
})

$("#field_875").on('blur', function () {
    if ($('#field_875').val().toUpperCase().length < 3)
        alert("Digite un CUSTOMER ID válido. Verifique.")
    //return validarFormato(this, "CUSTOMERID");
})


    
async function restaurar_colores_postpago1() {

    //restaurar colores
    field_872.style.borderColor = "#d1d3e2";
    field_860.style.borderColor = "#d1d3e2";
    field_877.style.borderColor = "#d1d3e2";
    field_871.style.borderColor = "#d1d3e2";
    field_878.style.borderColor = "#d1d3e2";
    field_876.style.borderColor = "#d1d3e2";
    field_875.style.borderColor = "#d1d3e2";
    field_869.style.borderColor = "#d1d3e2";
    field_882.style.borderColor = "#d1d3e2";
    field_880.style.borderColor = "#d1d3e2";
    field_879.style.borderColor = "#d1d3e2";
    field_881.style.borderColor = "#d1d3e2";
    field_874.style.borderColor = "#d1d3e2";
    field_867.style.borderColor = "#d1d3e2";
    field_866.style.borderColor = "#d1d3e2";
    field_865.style.borderColor = "#d1d3e2";
    field_864.style.borderColor = "#d1d3e2";

}

async function restaurar_colores_prepago1() {

    //restaurar colores
    field_907.style.borderColor = "#d1d3e2";
    field_909.style.borderColor = "#d1d3e2";
    field_910.style.borderColor = "#d1d3e2";
    field_911.style.borderColor = "#d1d3e2";
    field_912.style.borderColor = "#d1d3e2";
    field_913.style.borderColor = "#d1d3e2";
    field_914.style.borderColor = "#d1d3e2";
    field_915.style.borderColor = "#d1d3e2";
    field_916.style.borderColor = "#d1d3e2";

}

async function restaurar_colores_ascard1() {

    //restaurar colores
    field_883.style.borderColor = "#d1d3e2";
    field_884.style.borderColor = "#d1d3e2";
    field_894.style.borderColor = "#d1d3e2";
    field_888.style.borderColor = "#d1d3e2";
    field_889.style.borderColor = "#d1d3e2";
    field_892.style.borderColor = "#d1d3e2";
    field_890.style.borderColor = "#d1d3e2";
    field_893.style.borderColor = "#d1d3e2";
}

async function restaurar_colores_cuotas1() {

    //restaurar colores
    field_898.style.borderColor = "#d1d3e2";
    field_899.style.borderColor = "#d1d3e2";
    field_902.style.borderColor = "#d1d3e2";
    field_903.style.borderColor = "#d1d3e2";

}

async function restaurar_colores_centrales1() {

    //restaurar colores
    field_921.style.borderColor = "#d1d3e2";
    field_923.style.borderColor = "#d1d3e2";
    field_924.style.borderColor = "#d1d3e2";
    field_925.style.borderColor = "#d1d3e2";
    field_927.style.borderColor = "#d1d3e2";
    field_928.style.borderColor = "#d1d3e2";

}