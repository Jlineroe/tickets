$('.input-daterange').datepicker({
    todayBtn: "linked",
    calendarWeeks: true,
    keyboardNavigation: false,
    forceParse: false,
    autoclose: true,
    format: 'yyyy/mm/dd',
    firstDay: 1
});
//$('.chosen-select').chosen();
$('.super-chosen').chosen();
$('#divLoaderMaster').hide();
function FuncBtnLoader($Btn, Ocultar) {
    if (Ocultar == 1) {
        $Btn.style.display = 'none';
        $('.BtnLoading').show();
    } else {
        $Btn.style.display = '';
        $('.BtnLoading').hide();
    }
}