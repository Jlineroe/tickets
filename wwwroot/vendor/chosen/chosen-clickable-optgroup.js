$(function () {
    $('[title="clickable_optgroup"]').addClass('chosen-container-optgroup-clickable');
});
$(document).on('click', '[title="clickable_optgroup"] .group-result', function () {
    var unselected = $(this).nextUntil('.group-result').not('.result-selected');
    if (unselected.length) {
        unselected.trigger('mouseup');
    } else {
        $(this).nextUntil('.group-result').each(function () {
            $('a.search-choice-close[data-option-array-index="' + $(this).data('option-array-index') + '"]').trigger('click');
        });
    }
});