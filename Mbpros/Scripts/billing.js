var $ = jQuery.noConflict();

$(document).ready(function () {

    $(window).keydown(function (event) {
        if ((event.keyCode == 13 || event.keyCode == 169) && event.target.type != "textarea" && event.target.type != "submit" && event.target.type != "reset") {
            event.preventDefault();
            return false;
        }
    });

    var len = $("#txtAdditionalComments").val().length;
    $(".lmtchr").val(len);

    $("div[id^='divCheque']").hide();
    $("div[id^='divCopay']").hide();

    isNonLoggedIn();

    $("#BillingLog input[id*='txtCopayPaid']").keyup(function (e) {
        if (this.value.length == 1 && (e.which == 48 || e.which == 96)) {
            $("#BillingLog input[id*='txtCopayPaid']").val("");
        }
    });

    $("#BillingLog input[id*='txtBPatientName']").blur(function () {
        seqno = this.id.substring(15);
       // alert(seqno);
        if (this.value.length > 0) {
            $("#starDivDt" + seqno).addClass("required");
            $("#starDivProcCode" + seqno).addClass("required");
            //$("#starDivDXCode" + seqno).addClass("required");      
        }
        else {
            $("#starDivDt" + seqno).removeClass("required");
            $("#starDivProcCode" + seqno).removeClass("required");
           // $("#starDivDXCode" + seqno).removeClass("required");
        }
    });


    for (var seqno = 1; seqno <= 15; seqno++) {
        var copaytype = $("#BillingLog input[id='hdnCopayType" + seqno + "']");
        if (copaytype.val() == 'cash') {
            $("#cash" + seqno).attr("checked", "checked");
            $("#divCopay" + seqno).show();
        }
        else if (copaytype.val() == 'cheque') {
            $("#cheque" + seqno).attr("checked", "checked");
            $("#divCheque" + seqno).show();
            $("#divCopay" + seqno).show();
        }
        else if (copaytype.val() == 'creditcard') {
            $("#creditcard" + seqno).attr("checked", "checked");
            $("#divCopay" + seqno).show();
        }

        if ($("#txtBPatientName" + seqno).val() != "") {
            $("#starDivDt" + seqno).addClass("required");
            $("#starDivProcCode" + seqno).addClass("required");
            //$("#starDivDXCode" + seqno).addClass("required");
        }
    }

    //Text value should be numeric
    $("#BillingLog input[id*='txtChequeNo']").keydown(function (e) {
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

    //Text value should be numeric
    $("#BillingLog input[id*='txtCopayPaid'] ").keydown(function (e) {        
        // Allow: backspace, delete, tab, escape, enter and .
        if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||
            // Allow: Ctrl+A, Command+A
                (e.keyCode == 65 && (e.ctrlKey === true || e.metaKey === true)) ||
            // Allow: home, end, left, right, down, up
                (e.keyCode >= 35 && e.keyCode <= 40)) {
            // let it happen, don't do anything

            if ($.inArray(e.keyCode, [110, 190]) !== -1 && $(this).val().indexOf('.') >= 0)
                e.preventDefault();
            return;
        }

        // Ensure that it is a number and stop the keypress
        if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
            e.preventDefault();
        }
    });

    $(".form-group .input-validation-error").blur(function (e) {
        var nm = $(this).attr("name");
        if ($(this).val() == "") {
            $("span[data-valmsg-for='" + nm + "']").show();
            $(this).removeClass("input-validation-errorfree");
        }
        else {
            $("span[data-valmsg-for='" + nm + "']").hide();
            $(this).addClass("input-validation-errorfree");
        }
    });

    $(".hiddendiv").blur(function (e) {
        var id = $(this).attr('id');
        if ($(this).val() == "") {
            $("span[id='" + id + "']").show();
            $(this).removeClass("input-validation-errorfree");
        }
        else {
            $("span[id='" + id + "']").hide();
            $(this).addClass("input-validation-errorfree");
        }
    });
    $(".reqvalidation").blur(function (e) {
        var id = $(this).attr('id');
        if ($(this).val() == "") {
            $("span[id='" + id + "']").show();
            $(this).removeClass("input-validation-errorfree");
        }
        else {
            $("span[id='" + id + "']").hide();
            $(this).addClass("input-validation-errorfree");
        }
    });


    $("#btnBillingSave").click(function () {
        isNonLoggedIn();
        jQuery("span.reqvalidation").remove();
        jQuery(".reqvalidation").removeClass("reqvalidation");
        var flag = true;
        
        //validation for first input
        if ($("#cash1").is(":checked") || $("#creditcard1").is(":checked")) {
            var txtCopayPaid = $("#BillingLog input[id='txtCopayPaid1']");
            if ($.trim(txtCopayPaid.val()) == "") {
                txtCopayPaid.addClass("reqvalidation");
                jQuery("<span>").addClass("reqvalidation").html("").insertAfter(txtCopayPaid);
                txtCopayPaid.parent().closest('div').append(jQuery("<span id='txtCopayPaid1'>").addClass("reqvalidation").html("Please enter the copay amount"));
                flag = false;
            }
        }
        if ($("#cheque1").is(":checked")) {
            var txtCopayPaid = $("#BillingLog input[id='txtCopayPaid1']");
            if ($.trim(txtCopayPaid.val()) == "") {
                txtCopayPaid.addClass("reqvalidation");
                jQuery("<span'>").addClass("reqvalidation").html("").insertAfter(txtCopayPaid);
                txtCopayPaid.parent().closest('div').append(jQuery("<span id='txtCopayPaid1'>").addClass("reqvalidation").html("Please enter the copay amount"));
                flag = false;
            }
            var txtChequeNo = $("#BillingLog input[id='txtChequeNo1']");
            if ($.trim(txtChequeNo.val()) == "") {
                txtChequeNo.addClass("reqvalidation");
                jQuery("<span id='txtChequeNo1'>").addClass("reqvalidation").html("").insertAfter(txtChequeNo);
                txtChequeNo.parent().closest('div').append(jQuery("<span id='txtChequeNo1'>").addClass("reqvalidation").html("Please enter the check no."));
                flag = false;
            }
        }

        //Validation For Patient 2 to 15
        for (var i = 2; i <= 15; i++) {

            var txtBPatientName = $("#BillingLog input[id='txtBPatientName" + i + "']");
            if ($.trim(txtBPatientName.val()) != "") {

                var txtServiceDate = $("#BillingLog input[id='txtServiceDate" + i + "']");
                if ($.trim(txtServiceDate.val()) == "") {
                    txtServiceDate.addClass("reqvalidation");
                    jQuery("<span>").addClass("reqvalidation").html("").insertAfter(txtServiceDate);
                    txtServiceDate.closest('div').parent().append(jQuery("<span id='txtServiceDate" + i + "'>").addClass("reqvalidation").html("Please enter the service date"));
                    flag = false;
                }

                var txtProcCodes = $("#BillingLog input[id='txtProcCodes" + i + "']");
                if ($.trim(txtProcCodes.val()) == "") {
                    txtProcCodes.addClass("reqvalidation");
                    jQuery("<span id='txtProcCodes" + i + "'>").addClass("reqvalidation").html("Please enter the procedure codes").insertAfter(txtProcCodes);
                    flag = false;
                }

                //var txtNewDXCodes = $("#BillingLog input[id='txtNewDXCodes" + i + "']");
                //if ($.trim(txtNewDXCodes.val()) == "") {
                //    txtNewDXCodes.addClass("reqvalidation");
                //    jQuery("<span id='txtNewDXCodes" + i + "'>").addClass("reqvalidation").html("Please enter the new DX codes").insertAfter(txtNewDXCodes);
                //    flag = false;
                //}

                if ($("#cash" + i).is(":checked") || $("#creditcard" + i).is(":checked")) {
                    var txtCopayPaid = $("#BillingLog input[id='txtCopayPaid" + i + "']");
                    if ($.trim(txtCopayPaid.val()) == "") {
                        txtCopayPaid.addClass("reqvalidation");
                        jQuery("<span>").addClass("reqvalidation").html("").insertAfter(txtCopayPaid);
                        txtCopayPaid.parent().closest('div').append(jQuery("<span id='txtCopayPaid" + i + "'>").addClass("reqvalidation").html("Please enter the copay amount"));
                        flag = false;
                    }
                }
                if ($("#cheque" + i).is(":checked")) {
                    var txtCopayPaid = $("#BillingLog input[id='txtCopayPaid" + i + "']");
                    if ($.trim(txtCopayPaid.val()) == "") {
                        txtCopayPaid.addClass("reqvalidation");
                        jQuery("<span>").addClass("reqvalidation").html("").insertAfter(txtCopayPaid);
                        txtCopayPaid.parent().closest('div').append(jQuery("<span id='txtCopayPaid" + i + "'>").addClass("reqvalidation").html("Please enter the copay amount"));
                        flag = false;
                    }
                    var txtChequeNo = $("#BillingLog input[id='txtChequeNo" + i + "']");
                    if ($.trim(txtChequeNo.val()) == "") {
                        txtChequeNo.addClass("reqvalidation");
                        jQuery("<span>").addClass("reqvalidation").html("").insertAfter(txtChequeNo);
                        txtChequeNo.parent().closest('div').append(jQuery("<span id='txtChequeNo" + i + "'>").addClass("reqvalidation").html("Please enter the check no."));
                        flag = false;
                    }
                }
            }
        }
        if (flag == true)
            $("#busy-holder").show();
        return flag;
    });

    //$jpat(document).on('keypress', ':text,textarea', function (e) {
    //    var valid = (e.which >= 48 && e.which <= 57) || (e.which >= 65 && e.which <= 90) || (e.which >= 97 && e.which <= 122) || (e.which == 32) || (e.which == 8) || (e.which == 0);
    //    if (!valid) {
    //        e.preventDefault();
    //    }
    //});
});

$(function () {

    $("#BillingLog input:radio").click(function () {
        var seqno;
        //if (this.id.startsWith('cash')) {
        if (this.id.indexOf('cash') == 0){
            seqno = this.id.substring(4);
            $("#hdnCopayType" + seqno).val("cash");
        }
        //else if (this.id.startsWith('cheque')) {
        else if (this.id.indexOf('cheque') == 0) {
            seqno = this.id.substring(6);
            $("#hdnCopayType" + seqno).val("cheque");
        }
        //else if (this.id.startsWith('creditcard')) {
        else if (this.id.indexOf('creditcard') == 0) {
            seqno = this.id.substring(10);
            $("#hdnCopayType" + seqno).val("creditcard");
        }

    });

    $("#BillingLog input[name*='radio_copay']").click(function () {
        var seqno;
        if (this.id.indexOf('cash') ==0) {
            seqno = this.id.substring(4);
            $("#hdnCopayType" + seqno).val('cash');
            $("#divCheque" + seqno).hide();
            $("#divCopay" + seqno).show();
            $("#txtChequeNo" + seqno).val('');
        }
        else if (this.id.indexOf('cheque')==0) {
            seqno = this.id.substring(6);
            $("#hdnCopayType" + seqno).val('cheque');
            $("#divCheque" + seqno).show();
            $("#divCopay" + seqno).show();
            $("#txtChequeNo" + seqno).val('');
        }
        else if (this.id.indexOf('creditcard')==0) {
            seqno = this.id.substring(10);
            $("#hdnCopayType" + seqno).val('creditcard');
            $("#divCheque" + seqno).hide();
            $("#divCopay" + seqno).show();
            $("#txtChequeNo" + seqno).val('');
        }
    });

    //$("div[id*='ServiceDate']").datetimepicker({
    //    format: 'MM/DD/YYYY',
    //    maxDate: new Date(),
    //    showClear: true
    //});

});

function onkd(e) {
    if (e.keyCode == 9) {
        e.preventDefault();
        e.stopPropagation();
        $("#btnResetPatient").focus();
    }
}