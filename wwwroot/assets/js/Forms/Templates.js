PermisosActions('Templates');
DataTables();
$('#divLoaderMaster').hide();
async function ListadoPlantillas() {
    var Result = await $.ajax({
        contentType: 'application/json; charset=utf-8',
        type: "post",
        url: '/Templates/ListTemplates',
    });
    $("#divTable").html(Result);
    await DataTables();
}
async function DeleteTemplate(IdTemplate) {
    swal({
        title: "¿Deseas desactivar esta plantilla?",
        text: "¿Quieres continuar?",
        type: "warning",
        showCancelButton: true,
        confirmButtonText: "Si, Desactivar!",
        closeOnConfirm: false,
        showLoaderOnConfirm: true
    }, async function () {
        var params = new Object();
        params['IdTemplate'] = IdTemplate;
        var Result = await $.ajax({
            type: "post",
            url: '/Templates/DeleteTemplate',
            data: params
        });
        swal("Bien Hecho!", "Plantilla desactivada exitosamente.", "success");
        await ListadoPlantillas();
    });
}
async function ActivarTemplate(IdTemplate) {
    swal({
        title: "¿Deseas activar esta plantilla?",
        text: "¿Quieres continuar?",
        type: "warning",
        showCancelButton: true,
        confirmButtonText: "Si, Activar!",
        closeOnConfirm: false,
        showLoaderOnConfirm: true
    }, async function () {
        var params = new Object();
        params['IdTemplate'] = IdTemplate;
        var Result = await $.ajax({
            type: "post",
            url: '/Templates/ActivateTemplate',
            data: params
        });
        swal("Bien Hecho!", "Plantilla activada exitosamente.", "success");
        await ListadoPlantillas();
    });
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