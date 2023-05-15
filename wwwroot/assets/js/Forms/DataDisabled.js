DataTables();
async function DataTables() {
    try {
        var result = await GenerarDTFiltros('divTable');
    } catch (ex) {
        alert(ex);
    } finally {
        $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
    }
}

$('#divLoaderMaster').hide()
async function DisabledDataImport(IdDataImported, NameData) {
    swal({
        title: "¿Estas seguro?",
        text: " ¿ Deseas desactivar la data " + NameData+" ?, Esta accion no se puede revertir.",
        type: "warning",
        showCancelButton: true,
        confirmButtonText: "Si, Desactivar!",
        closeOnConfirm: false,
        showLoaderOnConfirm: true
    }, async function () {
        swal({
            title: "Motivo!",
            text: "Digite el motivo de la desactivacion",
            type: "input",
            showCancelButton: true,
            closeOnConfirm: false,
            inputPlaceholder: "Motivo desactivacion"
        }, async function (inputValue) {
            if (inputValue === false) return false;
            if (inputValue === "") {
                swal.showInputError("Este campo es obligatorio !");
                return false
            }
            if (inputValue.length > 1000) {
                swal.showInputError("El motivo no puede contener mas de 1000 caracteres!");
                return false
            }
            var params = new Object();
                params["IdDataImported"] = IdDataImported;
                params["DesactivationReason"] = inputValue;
            var result = await $.ajax({
                type: "post",
                url: '/DataDisabled/DesactivarDatas',
                data: params
            });
            swal("Bien Hecho!", "Data desactivada correctamente.", "success");
            setTimeout(function () {
                location.reload();
            }, 1000);
        });
    });
}