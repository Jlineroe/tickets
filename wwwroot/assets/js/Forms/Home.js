$(document).ready(function () {
    Dashboard(0);
});

$('.datepicker').datepicker({
    todayBtn: "linked",
    calendarWeeks: true,
    format: 'yyyy/mm/dd'
})

$('#divLoaderMaster').hide();
function FuncBtnLoader($BtnGuardar, Ocultar) {
    if (Ocultar == 1) {
        $BtnGuardar.style.display = 'none';
        $('.BtnLoading').show();
    } else {
        $BtnGuardar.style.display = '';
        $('.BtnLoading').hide();
    }

    var firstTabEl = document.querySelector('#myTab li:last-child button')
    var firstTab = new bootstrap.Tab(firstTabEl)

    firstTab.show()
}

function irTableroTiempos() {
    window.location.href = "/TimeGroup"
}

function irColocacionPrestamo() {
    window.location.href = "/ColocacionPrestamo/ListColocacionPrestamosllamadas"
}

function irTableroTiempos_2() {
    window.location.href = "/TimeGroup_2"
}


//(async function load() {
//    const $BtnGuardar = document.getElementById('BtnGuardar');
//    $BtnGuardar.addEventListener('click', async (event) => {
//        $('.content-wrapper .form-control').removeAttr('required');
//        try {
//            var $elementRequired = $('.required');
//            var Valida = ValidarVacios($elementRequired);
//            if (Valida == false) {

//            } else if (Valida == "error") {
//                event.preventDefault();
//            } else {
//                event.preventDefault();
//                FuncBtnLoader($BtnGuardar, 1);
//                var params = new Object();
//                params['IdCategory'] = $('#HdIdCategory').val();
//                params['NameCategory'] = $('#TxtNameCategory').val();
//                params['Grupo'] = {
//                    IdMasterGroups: $('#DdlGroups').val()
//                };
//                params['Sitio'] = {
//                    IdMasterSites: $('#DdlSitio').val()
//                };
//                params['DescriptionCategory'] = $('#TxtDescripcionCategory').val();
//                var estado = $('#DdlEstado').val();
//                params['State'] = (estado == 1 ? true : false);
//                var $optionSubCategorias = $(".newOption");
//                var SubCategorias = [];
//                for (var i = 0; i < $optionSubCategorias.length; i++) {
//                    SubCategorias.push({
//                        NameCategory: $optionSubCategorias[i].value,
//                        SLA_HOUR: $optionSubCategorias[i].dataset.sla
//                    });
//                }
//                params['SubCategory'] = SubCategorias;
//                var Result = await $.ajax({
//                    type: "post",
//                    url: '/Categories/SaveCategories',
//                    data: params,
//                });
//                swal("Bien Hecho!", "Accion realizada correctamente.", "success");
//                await ListadoCategories();
//                $('#myModal').modal('hide');
//            }
//        } catch (ex) {
//            swal("Error interno", "Comunicate con el area de IT para la verificacion de este error..", "error");
//        } finally {
//            setTimeout(function () {
//                FuncBtnLoader($BtnGuardar, 0);
//            }, 500);
//        }
//    });
//})();

(async function load() {
    const $BtnGuardar = document.getElementById('BtnGuardar');
    $BtnGuardar.addEventListener('click', async (event) => {
        event.preventDefault();
        try {
            var params = new Object();
            params['IdMasterGroups'] = $('#DdlGroup').val();
            params['Meta'] = $('#txtMeta').val();
            params['Fecha'] = $('#field_Fech_Meta').val();

            console.log(params);
            var Result = await $.ajax({
                type: "post",
                url: '/Home/SaveMeta',
                data: params,
            });
            swal("Bien Hecho!", "Accion realizada correctamente.", "success");
            $('#myModal').modal('hide');
        } catch (ex) {
            swal("Error interno", "Comunicate con el area de IT para la verificacion de este error..", "error");
            console.log(ex);
        } finally {
            setTimeout(function () {
                FuncBtnLoader($BtnGuardar, 0);
            }, 500);
        }
    });
})();


async function Dashboard(IdGroup) {

    var params = new Object();
    params["IdGroup"] = IdGroup;

    let Result = await $.ajax({
        type: "post",
        url: '/Home/Historico',
        data: params,
        beforeSend: function () {
            $("#Historico").html("<div class='text-center'><div class='spinner-border text-secondary' style='width: 3rem; height: 3rem;' role='status'><span class='sr-only'> Cargando...</span></div></div>");
        },
    });
    $("#Historico").html(Result);

}


////async function changeGrupo(value) {
////    var $optionTempl = '<option value="">--Seleccionar--</option>';
////    var $optionCate = '<option value="">--Seleccionar--</option>';
////    var Meta = "";
////    var fechaMeta = $('#field_Fech_Meta').val();
////    if (value != "") {
////        //let params = new Object();
////        //params['IdMasterGroups'] = { IdMasterGroups: value, fecha: fechaMeta };

////        var params = new Object();
////        params["IdMasterGroups"] = value;
////        params['fecha'] = fechaMeta;

////        console.log(params)

////        //templates
////        //let Result = await $.ajax({
////        //    type: "post",
////        //    url: '/Home/GetGrupos',
////        //    data: params
////        //});
////        //Meta = Result.metas.Meta;
////    }
////    $('#txtMeta').val(Meta);
////    $('#lblGrupos-error').text(mensaje);
////}