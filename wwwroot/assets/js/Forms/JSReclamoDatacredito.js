
//PermisosActions('Templates');
DataTables();
$('#divLoaderMaster').hide();
FilterCase();


$(document).ready(function () {
    
    $("#subestado").hide();

})

async function cambio() {
    $("#subestado").hide();
    var cmb_est = document.getElementById("est").value;
    if (cmb_est == "Eliminación" || cmb_est == "Ratificar" || cmb_est == "") {
        $("#subestado").hide();
    } else {
        $("#subestado").show();
    }

    //$(`#${m}`).show();
    //$("#field_235").removeClass('required Req-3 fieldSave requiredForever');
}

async function FilterCase() {
    
    var Result = await $.ajax({
        //contentType: 'application/json; charset=utf-8',
        type: "post",
        url: '/ReclamoDatacredito/ListReclamos',
    });
    $("#divTable").html(Result);
    await DataTables();
}

async function DataTables() {
    try {
        var result = await GenerarDTFiltros('divTable');
    } catch (ex) {
        //alert(ex);
    } finally {
        //$.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
    }
}


async function GuardarEstado($Btn) {
    if ((document.getElementById("est").value == '' || document.getElementById("est").value == null)) {
        swal("Datos incompletos", "Para actualizar el registro debe seleccionar una opcion en estado", "error");
    } else if ((document.getElementById("est").value == 'Cifin' || document.getElementById("est").value == 'Cuenta Mora' || document.getElementById("est").value == 'Prescripción') && (document.getElementById("subest").value == '' || document.getElementById("subest").value == null)) {
        swal("Datos incompletos", "Para actualizar el registro debe seleccionar una opcion en subestado", "error");
    } else {
        
        swal({
            title: "¿Deseas actualizar este estado?",
            text: "¿Estas seguro?",
            type: "warning",
            showCancelButton: true,
            confirmButtonText: "Si, actualizar!",
            closeOnConfirm: false,
            showLoaderOnConfirm: true
        }, async function () {
            var params = new Object();

            let paramString = location.search.substring(1).split('?');
            params['No_Reclamo'] = paramString;
            params['Estad'] = document.getElementById("est").value;
            params['SubEstad'] = document.getElementById("subest").value;
            var Result = await $.ajax({
                type: "post",
                url: '/ReclamoDatacredito/UpdateEstado',
                data: params
            });
            AddListWorkOrder();
            swal("Bien Hecho!", "Accion realizada correctamente!", "success");
            //setInterval("location.reload()", 2000);
            if (document.getElementById("subest").value == null || document.getElementById("subest").value == "") {
            setTimeout(function () {
                window.location = "../";
            }, 2000);
            }
            
        });
            
    }
}


async function AddListWorkOrder() {
   
            var params = new Object();

            let paramString = location.search.substring(1).split('?');
            params['No_Reclamo'] = paramString;
            var Result = await $.ajax({
                type: "post",
                url: '/ReclamoDatacredito/AddListWorkOrder',
                data: params
            });

            swal("Bien Hecho!", "Registro pasado a ListWorkOrder", "success");
            //setInterval("location.reload()", 2000);

            setTimeout(function () {
                window.location.href = "/ReclamoDatacredito";
            }, 2000);

}