$(document).ready(function () {

    FechaActual();
    LLenarComboDependientes(1005, 'cbotipologia');
    LLenarComboDependientes(1007, 'cboSeguimiento');

    /*Restringo Fecha de anteriores a la de hoy */
    var today = new Date().toISOString().split('T')[0];
    document.getElementsByName("txtFechaProximaAccion")[0].setAttribute('min', today);

    $("#FechaBackOffice").hide();
    $("#DetalleGestion").hide();
    $("#Canal").hide();

});


const $BtnGuardar = document.getElementById('BtnGuardar');
async function FuncBtnLoader($BtnGuardar, Ocultar) {
    if (Ocultar == 1) {
        $BtnGuardar.style.display = 'none';
        $('.BtnLoading').show();
    } else {
        $BtnGuardar.style.display = '';
        $('.BtnLoading').hide();
    }
}

var ArrayDiasHabiles;
var DiasHabiles = "";

function NoSR() {
    let NoSRPre = $("#txtNoSRPre").val();
    $("#txtNoSRPre").val(NoSRPre.split("-")[0]);
    $("#txtNoSR").val(NoSRPre.split("-")[1]);
}

async function validarNoSR(event) {
    let NoSRPre = $("#txtNoSRPre").val();
    let NoSR = $("#txtNoSR").val();

    let NoSRCompleto = NoSRPre + '-' + NoSR;

    let keyCode = event.keyCode || event.which;
    if (keyCode == 9) {
        if (NoSRCompleto.length < 14 || NoSRCompleto.length > 15) {
            swal("Error", "El numero de caracteres no puede ser menor a 14 ni mayor a 15", "error")
            $("#txtNoSRPre").val('');
            $("#txtNoSR").val('');
        } else {
            try {
                let params = new Object()
                params['NoSR'] = NoSRCompleto
                let Result = await $.ajax({
                    type: "post",
                    url: '/GestionBOE/GetGestionBOE',
                    data: params
                })

                if (Result.length == 0) {
                    limpiarCampos();
                    return
                } else {
                    Result = Result[0]

                    //if (Result.Segmento == 4897) {ValidarCombo(4897);}

                    
                    if (Result.Segmento == 4897) { ValidarCombo(4897); }


                    $("#cboSegmento").val(Result.Segmento);
                    $("#cboEstado").val(Result.Estado);
                    $("#txtFechaCreacion").val(FormatoFecha(Result.FechaCreacion));
                    $("#txtFechaEnvioCampo").val(FormatoFecha(Result.FechaEnvioACampo));
                    $("#txtFechaProximaAccion").val(FormatoFecha(Result.FechaProximaAccion));
                    $("#txtFechaBackOffice").val(FormatoFecha(Result.FechaBackOffice));
                    $("#txtFechaActualizacionEstado").val(FormatoFecha(Result.FechaActualizacionEstado));
                    $("#cboDetalleGestion").val(Result.DetalleGestion);
                    $("#cboCanal").val(Result.Canal);
                    $("#txtObservaciones").val(Result.Observaciones);

                    setTimeout(function () {
                        $("#cboSeguimiento").val(Result.SeguimientoPendiente).trigger('chosen:updated');
                        $("#cbotipologia").val(Result.Tipologia).trigger('chosen:updated');
                    }, 1000);

                    //Establecer disabled a campos que no pueden editar
                    $("#cboSegmento").prop("disabled", true);
                    $("#cbotipologia").prop("disabled", true).trigger("chosen:updated");
                    $("#txtFechaCreacion").prop("disabled", true);
                    $("#txtFechaEnvioCampo").prop("disabled", true);
                    $("#cboDetalleGestion").prop("disabled", true);
                    $("#cboCanal").prop("disabled", true);
                    $("#txtObservaciones").prop("disabled", true);
                    $("#txtFechaBackOffice").prop("disabled", true);
                    $("#txtNoSRPre").prop("disabled", true);
                    $("#txtNoSR").prop("disabled", true);
                }
                


            } catch (ex) {
                swal("Error interno", "Comunicate con el area de IT para la verificacion de este error..", "error")
            }
        }
    }
}

//SOLO NUMEROS
function SoloNumeros() {
    if ((event.keyCode < 48) || (event.keyCode > 57))
        event.returnValue = false;
}

function FechaActual() {
    var today = new Date();
    var dd = String(today.getDate()).padStart(2, '0');
    var mm = String(today.getMonth() + 1).padStart(2, '0'); //January is 0!
    var yyyy = today.getFullYear();

    today = yyyy + '-' + mm + '-' + dd;
    $("#txtFechaActualizacionEstado").val(today);
}

function FormatoFecha(fecha) {
    var date = new Date(parseInt(fecha.substr(6)));
    return date.toISOString().substr(0, 10);
}

function limpiarCampos() {
    $("#cboSegmento").val('');
    $("#cbotipologia").val('').trigger('chosen:updated');;
    $("#cboEstado").val('');
    $("#cboSeguimiento").val('').trigger('chosen:updated');;
    $("#txtFechaCreacion").val('');
    $("#txtFechaEnvioCampo").val('');
    $("#txtFechaProximaAccion").val('');
    $("#txtFechaBackOffice").val('');
    $("#cboDetalleGestion").val('');
    $("#cboCanal").val('');
    $("#txtObservaciones").val('');
}

function ValidarCombo(IdParent) {
    //4897 - Bono social
    if (IdParent != 4897) {
        LLenarComboDependientes(1005, 'cbotipologia');
        LLenarComboDependientes(1007, 'cboSeguimiento');
        $("#FechaEnvioCampo").show();
        $("#FechaBackOffice").hide();
        $("#DetalleGestion").hide();
        $("#Canal").hide();
    } else {
        LlenarCombosBonoSocial(IdParent);
        $("#FechaEnvioCampo").hide();
        $("#FechaBackOffice").show();
        $("#DetalleGestion").show();
        $("#Canal").show();
    }
}

async function LLenarComboDependientes(IdFieldsUDF, Input) {
    try {
        let $option = "<option value=''>--Selecccionar--</option>"
        if (IdFieldsUDF != "") {
            let params = new Object()
            params['IdFieldsUDF'] = IdFieldsUDF
            let Result = await $.ajax({
                type: "post",
                url: '/Templates/ListDispositions',
                data: params
            })

            let ArrayResult = Result.filter(option => option.Parent_IdDispositions == 0);
            for (let item of ArrayResult) {

                $option += "<option value=" + item.IdFieldsDispositions + ">" + item.Dispositions + "</option>"
            }
        }
        $("#" + Input).html($option).trigger('chosen:updated');
    } catch (ex) {
        swal("Error interno", "Comunicate con el area de IT para la verificacion de este error..", "error")
    }
}

async function LlenarCombosBonoSocial(IdParent) {
    try {
        let $optionTipologia = "<option value=''>--Selecccionar--</option>"
        let $optionSeguimiento = "<option value=''>--Selecccionar--</option>"

        if (IdParent != "") {
            let params = new Object()
            params['Parent_IdDispositions'] = IdParent
            let Result = await $.ajax({
                type: "post",
                url: '/Templates/ListDispositions',
                data: params
            })

            let Tipologia = Result.filter(({ IdFieldsUDF }) => IdFieldsUDF === 1005);
            let Seguimiento = Result.filter(({ IdFieldsUDF }) => IdFieldsUDF === 1007);

            for (let item of Tipologia) {
                $optionTipologia += "<option value=" + item.IdFieldsDispositions + ">" + item.Dispositions + "</option>"
            }
            for (let item of Seguimiento) {
                $optionSeguimiento += "<option value=" + item.IdFieldsDispositions + ">" + item.Dispositions + "</option>"
            }
        }

        $("#" + 'cbotipologia').html($optionTipologia).trigger('chosen:updated');
        $("#" + 'cboSeguimiento').html($optionSeguimiento).trigger('chosen:updated');

    } catch (ex) {
        swal("Error interno", "Comunicate con el area de IT para la verificacion de este error..", "error")
    }
}

async function BS_SeguimientoPendiente() {

    let Seguimiento = $('select[name=cboSeguimiento] option').filter(':selected').val()
    $("#txtFechaProximaAccion").val('');

    switch (Seguimiento) {
        case '4983'://Bs. pdt Minetad
            DiasHabiles = 3;
            break;
        case '4984'://Pdt. volcaje
            DiasHabiles = 4
            break;
        case '4985'://Pdt. volcaje ATR Distribuidora
            DiasHabiles = 6
            break;
        case '4986'://Pdt. carga gestión de accesos
            DiasHabiles = 5
            break;
        case '4987'://Pdt. Coordinación en linea
            DiasHabiles = 1
            break;
    }

    if (DiasHabiles != 0 || DiasHabiles != "") {

        let params = new Object()
        params['DiasHabiles'] = DiasHabiles
        let Result = await $.ajax({
            type: "post",
            url: '/Templates/DiasHabiles',
            data: params
        })

        ArrayDiasHabiles = Result
    }
}

function ProximaAccion() {

    let Segmento = $("#cboSegmento").val();

    if (Segmento != 4897) {
        ArrayDiasHabiles = "";
    } else {

        let FechaProximaAccion = $("#txtFechaProximaAccion").val();
        let UltimaFechaHabil = ArrayDiasHabiles[ArrayDiasHabiles.length - 1].Fecha;

        //Validamos si la fecha se encuentra en el Array, de lo contrario es un dia no habil
        let incluyeFecha = ArrayDiasHabiles.find(diaHabil => diaHabil.Fecha == FechaProximaAccion);

        if (FechaProximaAccion > UltimaFechaHabil) {
            swal("Error", "El dia escogido no puede ser mayor a" + " " + DiasHabiles + " " + "dias habiles", "error");
            $("#txtFechaProximaAccion").val('');
        } else if (incluyeFecha == undefined) {
            swal("Error", "El dia escogido es un dia no habil..", "error")
            $("#txtFechaProximaAccion").val('');
        }
    }

    
}

async function guardarGestionOBE() {

    let Segmento = $('#cboSegmento').val();
    camposObligatorios(Segmento);

    var $elementRequired = $('#formCreate .required');
    var Valida = true;
    Valida = await ValidarVacios($elementRequired);

    if (Valida == false) {
        swal("Alerta automatica", "Todos los campos son obligatorios.", "error");
        event.preventDefault();
        return false;
    } else {
        
        let NoSRPre = $("#txtNoSRPre").val();
        let NoSR = $("#txtNoSR").val();

        let NoSRCompleto = NoSRPre + '-' + NoSR;

        if (NoSRCompleto.length < 14 || NoSRCompleto.length > 15) {
            swal("Error", "El numero de caracteres no puede ser menor a 14 ni mayor a 15", "error")
            $("#txtNoSRPre").val('');
            $("#txtNoSR").val('');
            return;
        }

        let Segmento = $("#cboSegmento").val();
        let Tipologia = $("#cbotipologia").val();
        let Estado = $("#cboEstado").val();
        let Seguimiento = $("#cboSeguimiento").val();
        let FechaCreacion = $("#txtFechaCreacion").val();
        let FechaEnvioCampo = $("#txtFechaEnvioCampo").val();
        let FechaActualizacionEstado = $("#txtFechaActualizacionEstado").val();
        let FechaProximaAccion = $("#txtFechaProximaAccion").val();
        let FechaBackOffice = $("#txtFechaBackOffice").val();
        let DetalleGestion = $("#cboDetalleGestion").val();
        let Canal = $("#cboCanal").val();
        let Observaciones = $("#txtObservaciones").val();


        FuncBtnLoader($BtnGuardar, 1)

        var params = new Object();
        params['NoSR'] = NoSRCompleto;
        params['Segmento'] = Segmento;
        params['Tipologia'] = Tipologia;
        params['Estado'] = Estado;
        params['FechaActualizacionEstado'] = FechaActualizacionEstado;
        params['SeguimientoPendiente'] = Seguimiento;
        params['FechaCreacion'] = FechaCreacion;
        params['FechaEnvioACampo'] = FechaEnvioCampo;
        params['FechaProximaAccion'] = FechaProximaAccion;
        params['FechaBackOffice'] = FechaBackOffice;
        params['DetalleGestion'] = DetalleGestion;
        params['Canal'] = Canal;
        params['Observaciones'] = Observaciones;

        try {
            var Result = await $.ajax({
                type: "post",
                url: '/GestionBOE/SaveGestionBOE',
                data: params
            });
            swal("Bien Hecho!", "Accion realizada correctamente.", "success");
            setTimeout(function () {
                location.reload();
            }, 2000);

        } catch (ex) {
            swal("Error Interno", "Comunicate con el area de IT para la verificacion de este error..", "error");
        }

        
    }
}

function camposObligatorios(Segmento) {
    //4897 -> BONO SOCIAL
    //4896 -> CALLERS

    if (Segmento == 4897) {
        $("#txtFechaEnvioCampo").removeClass("required");
    }
    else if (Segmento == 4896) {
        $("#cboSegmento").removeClass("required");
        $("#cboSeguimiento").removeClass("required");
        $("#txtFechaCreacion").removeClass("required");
        $("#txtFechaEnvioCampo").removeClass("required");
        $("#txtFechaProximaAccion").removeClass("required");
        $("#cboDetalleGestion").removeClass("required");
        $("#cboCanal").removeClass("required");
        $("#txtObservaciones").removeClass("required");
        $("#txtFechaBackOffice").removeClass("required");
    } else {
        $("#cboCanal").removeClass("required");
        $("#cboDetalleGestion").removeClass("required");
        $("#txtFechaBackOffice").removeClass("required");
    }
}