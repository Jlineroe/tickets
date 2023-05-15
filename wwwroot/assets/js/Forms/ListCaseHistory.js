//PermisosActions('ListWorkOrders');
//$('.chosen-select').chosen();
//$('.super-chosen').chosen();
//$('.input-daterange').datepicker({
//    todayBtn: "linked",
//    calendarWeeks: true,
//    keyboardNavigation: false,
//    forceParse: false,
//    autoclose: true,
//    format: 'yyyy/mm/dd'
//});
$('#divLoaderMaster').hide();
async function FuncBtnLoader($BtnGuardar, Ocultar) {
    if (Ocultar == 1) {
        $BtnGuardar.style.display = 'none';
        $('.BtnLoading').show();
    } else {
        $BtnGuardar.style.display = '';
        $('.BtnLoading').hide();
    }
}

async function ViewCaseHistory($Btn) {
    
    try {
        var ESTADOArray = "";
        if ($('#txtFechaInicio').val() === '') {
            swal("Error", "Fecha inicio es obligatoria", "error");
            return;
        }

        if ($('#txtFechaFinal').val() === '') {
            swal("Error", "Fecha final es obligatoria", "error");
            return;
        }

        if (new Date($('#txtFechaInicio').val()) > $('#txtFechaFinal').val()) {
            swal("Error", "La fecha de inicio no puede ser mayor a la fecha final", "error");
            return;
        }
        FuncBtnLoader($Btn, 1)
        //$('#divLoaderMaster').show();
        let params = new Object();
        params['ESTADO'] = $('#SelEstado option:selected').val();
        params["FechaInicio"] = $('#txtFechaInicio').val();
        params["FechaFinal"] = $('#txtFechaFinal').val();

        let Result = await $.ajax({
            type: "post",
            url: '/CaseHistory/ListCaseHistory',
            data: params,
        });
        $("#divTable").html(Result);
        ToolsDataTables();
        FuncBtnLoader($Btn, 0)
    } catch (ex) {
        swal("Error interno", "Comunicate con el area de IT para la verificacion de este error..", "error");
    }
}




