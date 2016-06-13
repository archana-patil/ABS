
$(document).ready(function () {

    $("#FormId, #SSN").keydown(function (e) {
        // Allow: backspace, delete, tab, escape, enter and .
        if ($.inArray(e.keyCode, [46, 8, 9, 27, 13]) !== -1 ||
            // Allow: Ctrl+A, Command+A
                (e.keyCode == 65 && (e.ctrlKey === true || e.metaKey === true)) ||
            // Allow: home, end, left, right, down, up
                (e.keyCode >= 35 && e.keyCode <= 40)) {
            // let it happen, don't do anything
            return;
        }
        // Ensure that it is a number and stop the keypress
        if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
            e.preventDefault();
        }
    });

    $(".datetimepicker").datetimepicker({
        format: 'MM/DD/YYYY',
        maxDate: new Date(),
        minDate: '01/01/1900',
        showClear: true,
    });

    $("#SSN").mask("999-99-9999", { autoclear: false });
    
});

function responsive_jqgrid(jqgrid) {
    jqgrid.find('.ui-jqgrid').addClass('clear-margin col-xs-12').css('width', '');
    jqgrid.find('.ui-jqgrid-view').addClass('clear-margin col-xs-12').css('width', '');
    jqgrid.find('.ui-jqgrid-view > div').eq(1).addClass('clear-margin col-xs-12').css('width', '').css('min-height', '0');
    jqgrid.find('.ui-jqgrid-view > div').eq(2).addClass('clear-margin col-xs-12').css('width', '').css('min-height', '0');
    jqgrid.find('.ui-jqgrid-sdiv').addClass('clear-margin col-xs-12').css('width', '');
    jqgrid.find('.ui-jqgrid-pager').addClass('clear-margin col-xs-12').css('width', '');
}

