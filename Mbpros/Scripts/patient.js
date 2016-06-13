
$(document).ready(function () {

    $(window).keydown(function (event) {
        if ((event.keyCode == 13 || event.keyCode == 169) && event.target.type != "textarea" && event.target.type != "submit" && event.target.type != "reset") {
            event.preventDefault();
            return false;
        }
    });

    var len = $("#txtAdditionalInfo").val().length;
    $(".lmtchr").val(len);

    //$.fn.datetimepicker.defaults.language = 'en';
    isNonLoggedIn();

    $("input:radio:first").prop("checked", true).trigger("click");
    $("#txtPatSSN").mask("999-99-9999");
    $("#txtInsuranceCompanyPhone").mask('999-999-9999');
    $("#txtSecInsuranceCompanyPhone").mask('999-999-9999');
    $("#txtInsuranceFax").mask('999-999-9999');
    $("#txtZipCode").mask("99999");
    $("#txtReferringProviderNPI").mask("9999999999");

    //if ($("#hdnPatientID").val() > 0)
    //$("#busy-holder").show();

    //Checked the radio button on model value
    if ($("#hdnSex").val() == 'M') {
        $("#rbtnSexM").prop("checked", true);
    }
    else if ($("#hdnSex").val() == 'F') {
        $("#rbtnSexF").prop("checked", true);
    }
    else {
        $("#rbtnSexM").removeAttr("checked");
        $("#rbtnSexF").removeAttr("checked");
    }

    if ($("#hdnIsInsured").val() == "YES") {
        $("#rbtnIsInsuredY").prop("checked", true);
        $("#PatNoInsured").hide();
    }
    else if ($("#hdnIsInsured").val() == "NO") {
        $("#rbtnIsInsuredN").prop("checked", true);
        $("#PatNoInsured").show();
    }


    $("#txtReferringProviderName").blur(function () {
        if ($("#txtReferringProviderName").val().length > 0) {
            $("#starDivNPI").addClass("required");
            $("#starDateLastseen").addClass("required");
        }
        else {
            $("#starDivNPI").removeClass("required");
            $("#starDateLastseen").removeClass("required");
        }
    });

    $("#txtFacilityName").blur(function () {
        if ($("#txtFacilityName").val().length > 0) {
            $("#starDivDtAddmission").addClass("required");
        }
        else {
            $("#starDivDtAddmission").removeClass("required");
        }
    });

    switch ($("#hdnIsConditionRelatedTo").val()) {
        case "N": $("#rbtnIsCondRelatedToN").prop("checked", true);
            $("#divAccidentDate").hide();
            $("#divInsuranceFax").hide();
            $("#divAdjusterName").hide(); break;
        case "A": $("#rbtnIsCondRelatedToA").prop("checked", true);
            $("#divAccidentDate").show();
            $("#divInsuranceFax").show();
            $("#divAdjusterName").show(); break;
        case "E": $("#rbtnIsCondRelatedToE").prop("checked", true);
            $("#divAccidentDate").show();
            $("#divInsuranceFax").show();
            $("#divAdjusterName").show(); break;
        case "O": $("#rbtnIsCondRelatedToO").prop("checked", true);
            $("#divAccidentDate").show();
            $("#divInsuranceFax").hide();
            $("#divAdjusterName").hide(); break;
    }

    if ($("#hdnIsSecInsurance").val() == 'True') {
        $("#rbtnIsSecInsuranceY").prop("checked", true);
        $("#divSecInsurance").show();
    }
    else if ($("#hdnIsSecInsurance").val() == 'False') {
        $("#rbtnIsSecInsuranceN").prop("checked", true);
        $("#divSecInsurance").hide();
    }

    if ($("#hdnIsVerify").val() == 'True') {
        $("#rbtnIsVerifyY").prop("checked", true);
    }
    else if ($("#hdnIsVerify").val() == 'False') {
        $("#rbtnIsVerifyN").prop("checked", true);
    }
    //Text value should be numeric
    $("#txtReferringProviderNPI, #txtZipCode,#txtPatSSN, #txtInsuranceCompanyPhone, #txtGroupNumber, #txtInsuranceFax, #txtSecInsuranceCompanyPhone").keydown(function (e) {
        //debugger;
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
    
    $(".form-group .secdiv").blur(function (e) {
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

    $("#btnSave").click(function () {
        isNonLoggedIn();
        jQuery("span.reqvalidation").remove();
        jQuery(".reqvalidation").removeClass("reqvalidation");

        var flag = true;
        //Secondary Insurance
        if ($("#rbtnIsSecInsuranceY").is(":checked")) {
            $("span[data-valmsg-for='IsSecondaryInsurance']").hide();
            $txtSecInsuranceCompanyName = $("#txtSecInsuranceCompanyName");
            if ($.trim($txtSecInsuranceCompanyName.val()) == "") {
                $txtSecInsuranceCompanyName.addClass("reqvalidation");
                jQuery("<span id='txtSecInsuranceCompanyName'>").addClass("reqvalidation").html("Please enter the secondary insurance company name").insertAfter($txtSecInsuranceCompanyName);
                flag = false;
            }

            $txtSecInsuranceCompanyPhone = $("#txtSecInsuranceCompanyPhone");
            if ($.trim($txtSecInsuranceCompanyPhone.val()) == "") {
                $txtSecInsuranceCompanyPhone.addClass("reqvalidation");
                jQuery("<span id='txtSecInsuranceCompanyPhone'>").addClass("reqvalidation").html("Please enter the secondary insurance company phone").insertAfter($txtSecInsuranceCompanyPhone);
                flag = false;
            }


            $txtSecondaryInsuranceID = $("#txtSecondaryInsuranceID");
            if ($.trim($txtSecondaryInsuranceID.val()) == "") {
                $txtSecondaryInsuranceID.addClass("reqvalidation");
                jQuery("<span id='txtSecondaryInsuranceID'>").addClass("reqvalidation").html("Please enter the secondary insurance ID").insertAfter($txtSecondaryInsuranceID);
                flag = false;
            }

            $txtSecondaryInsuredName = $("#txtSecondaryInsuredName");
            if ($.trim($txtSecondaryInsuredName.val()) == "") {
                $txtSecondaryInsuredName.addClass("reqvalidation");
                jQuery("<span id='txtSecondaryInsuredName'>").addClass("reqvalidation").html("Please enter the secondary insured name").insertAfter($txtSecondaryInsuredName);
                flag = false;
            }

            $txtSecondaryInsuredDOB = $("#txtSecondaryInsuredDOB");
            if ($.trim($txtSecondaryInsuredDOB.val()) == "") {
                $txtSecondaryInsuredDOB.addClass("reqvalidation");
                $("#txtSecondaryInsuredDOB").closest("div").parent().append(jQuery("<span id='txtSecondaryInsuredDOB'>").addClass("reqvalidation").html("Please enter the secondary insured date of birth"));
                flag = false;
            }
        }
        else {
            $("span[data-valmsg-for='IsSecondaryInsurance']").hide();
        }

        if ($("#rbtnIsInsuredN").is(":checked")) {
            $("span[data-valmsg-for='IsPatInsured']").hide();
            $txtPrimaryInsuredName = $("#txtPrimaryInsuredName");
            if ($.trim($txtPrimaryInsuredName.val()) == "") {
                $txtPrimaryInsuredName.addClass("reqvalidation");
                jQuery("<span id='txtPrimaryInsuredName'>").addClass("reqvalidation").html("Please enter insured name").insertAfter($txtPrimaryInsuredName);
                flag = false;
            }

            $txtPrimaryInsuredDOB = $("#txtPrimaryInsuredDOB");
            if ($.trim($txtPrimaryInsuredDOB.val()) == "") {
                $txtPrimaryInsuredDOB.addClass("reqvalidation");
                $("#txtPrimaryInsuredDOB").closest('div').parent().append(jQuery("<span id='txtPrimaryInsuredDOB'>").addClass("reqvalidation").html("Please enter insured date of birth"));
                flag = false;
            }
        }
        else {
            $("span[data-valmsg-for='IsPatInsured']").hide();
        }

        if ($("#rbtnIsCondRelatedToA, #rbtnIsCondRelatedToE, #rbtnIsCondRelatedToO").is(":checked")) {
            $("span[data-valmsg-for='IsConditionRelatedTo']").hide();
            $txtDateofAccident = $("#txtDateofAccident");
            if ($.trim($txtDateofAccident.val()) == "") {
                $txtDateofAccident.addClass("reqvalidation");
                $("#txtDateofAccident").closest('div').parent().append(jQuery("<span id='txtDateofAccident'>").addClass("reqvalidation").html("Please enter date of accident"));
                flag = false;
            }
        }

        if ($("#rbtnIsCondRelatedToA, #rbtnIsCondRelatedToE").is(":checked")) {
            $("span[data-valmsg-for='IsConditionRelatedTo']").hide();
            $txtInsuranceFax = $("#txtInsuranceFax");
            if ($.trim($txtInsuranceFax.val()) == "") {
                $txtInsuranceFax.addClass("reqvalidation");
                jQuery("<span id='txtInsuranceFax'>").addClass("reqvalidation").html("Please enter Insurance Fax #").insertAfter($txtInsuranceFax);
                flag = false;
            }

            $txtAdjusterName = $("#txtAdjusterName");
            if ($.trim($txtAdjusterName.val()) == "") {
                $txtAdjusterName.addClass("reqvalidation");
                jQuery("<span id='txtAdjusterName'>").addClass("reqvalidation").html("Please enter adjuster name").insertAfter($txtAdjusterName);
                flag = false;
            }
        }

        if ($("#rbtnIsCondRelatedToA").is(":checked") || $("#rbtnIsCondRelatedToE").is(":checked")) {
            $("span[data-valmsg-for='IsConditionRelatedTo']").hide();
            $txtInsuranceCompanyPhone = $("#txtInsuranceCompanyPhone");
            if ($.trim($txtInsuranceCompanyPhone.val()) == "") {
                $txtInsuranceCompanyPhone.addClass("reqvalidation");
                jQuery("<span id='txtInsuranceCompanyPhone'>").addClass("reqvalidation").html("Please enter insurance company phone").insertAfter($txtInsuranceCompanyPhone);
                flag = false;
            }
            $("#starDivInsCompPhone").addClass("required");
        }
        else { $("#starDivInsCompPhone").removeClass("required"); $("span[data-valmsg-for='IsConditionRelatedTo']").hide(); }

        if ($.trim($("#txtFacilityName").val()) != "") {
            $txtAdmissionDate = $("#txtAdmissionDate");
            if ($.trim($txtAdmissionDate.val()) == "") {
                $txtAdmissionDate.addClass("reqvalidation");
                $("#txtAdmissionDate").closest('div').parent().append(jQuery("<span id='txtAdmissionDate'>").addClass("reqvalidation").html("Please enter admission date"));
                flag = false;
            }
        }

        if ($.trim($("#txtReferringProviderName").val()) != "") {
            $txtReferringProviderNPI = $("#txtReferringProviderNPI");
            if ($.trim($txtReferringProviderNPI.val()) == "") {
                $txtReferringProviderNPI.addClass("reqvalidation");
                jQuery("<span id='txtReferringProviderNPI'>").addClass("reqvalidation").html("Please enter referring provider NPI").insertAfter($txtReferringProviderNPI);
                flag = false;
            }

            $txtLastSeenByRefProvider = $("#txtLastSeenByRefProvider");
            if ($.trim($txtLastSeenByRefProvider.val()) == "") {
                $txtLastSeenByRefProvider.addClass("reqvalidation");
                $("#txtLastSeenByRefProvider").closest('div').parent().append(jQuery("<span id='txtLastSeenByRefProvider'>").addClass("reqvalidation").html("Please enter last seen by referring provider"));
                flag = false;
            }
        }

        if (flag == true)
            $("#busy-holder").show();
        return flag;
    });


    $("#txtDiagnosisCodes").keydown(function (e) {

        var txt = $("#txtDiagnosisCodes").val();
        if (txt.match(/\,/g).length == 3 && e.keyCode == 188) {
            e.preventDefault();
        }
        else if (txt.match(/\,/g).length == 3) {
            if ((e.keyCode >= 35 && e.keyCode <= 40) || e.keyCode != 188) {
                // if (/^([^,]*,){3}[^,]*$/.test(txt)) { e.preventDefault(); }

            } else {
                e.preventDefault();
            }
        }
        else if (txt.match(/\,/g).length > 3) {
            e.preventDefault();
        }
        else {
            if (/^([^,]*,){0,2}[^,]*$/.test(txt) || ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1) || (e.keyCode >= 35 && e.keyCode <= 40)) {
                // if (/^([^,]*,){3}[^,]*$/.test(txt)) { e.preventDefault(); }

            } else {
                e.preventDefault();
            }
        }
    });

    if ($("#txtPolicyID").val() == "0") {
        $("#txtPolicyID").val("");
    }

    if ($("#txtFacilityName").val() != "") {
        $("#starDivDtAddmission").addClass("required");
    }
    if ($("#rbtnIsCondRelatedToA, #rbtnIsCondRelatedToE").is(":checked")) {
        $("#starDivInsCompPhone").addClass("required");
    }

    if ($("#txtReferringProviderName").val() != "") {
        $("#starDivNPI").addClass("required");
        $("#starDateLastseen").addClass("required");
    }
  

    //$(document).on('keypress', function (e) {
    //    var valid = true;
    //    if (e.target.id == 'txtPatStreetAddress' || e.target.id == 'txtInsuranceCompanyAddress' || e.target.id == 'txtAdditionalInfo')
    //        valid = true;
    //    else
    //        valid = (e.which >= 48 && e.which <= 57) || (e.which >= 65 && e.which <= 90) || (e.which >= 97 && e.which <= 122) || (e.which == 32) || (e.which == 8) || (e.which == 0);
    //    if (!valid) {
    //        e.preventDefault();
    //    }
    //});
});


$(function () {

    $("input[name='IsPatInsured']").click(function () {
        if ($("#rbtnIsInsuredN").is(":checked")) {
            $("#PatNoInsured").show();
        } else {
            $("#PatNoInsured").hide();

            $("#txtPrimaryInsuredName").val('');
            $("#txtPrimaryInsuredDOB").val('');

        }
    });

    //                   none/    Auto / Employement/ Other
    //DateofAccident     N         Y       Y       Y
    //InsuranceFax       N         Y       Y       N
    //AdjusterName       N         Y       Y       N

    $("input[name='IsConditionRelatedTo']").click(function () {
        $("#starDivInsCompPhone").removeClass("required");
        if ($("#rbtnIsCondRelatedToA, #rbtnIsCondRelatedToE").is(":checked")) {
            $("#divAccidentDate").show();
            $("#divInsuranceFax").show();
            $("#divAdjusterName").show();
            $("#starDivInsCompPhone").addClass("required");
        } else if ($("#rbtnIsCondRelatedToO").is(":checked")) {
            $("#divAccidentDate").show();
            $("#divInsuranceFax").hide();
            $("#divAdjusterName").hide();

            $("#txtInsuranceFax").val('');
            $("#txtAdjusterName").val('');
        }
        else {
            $("#divAccidentDate").hide();
            $("#divInsuranceFax").hide();
            $("#divAdjusterName").hide();

            $("#txtDateofAccident").val('');
            $("#txtInsuranceFax").val('');
            $("#txtAdjusterName").val('');
        }
    });

    $("input[name='IsVerifyEligibility']").click(function () {
        if ($("#rbtnIsVerifyY").is(":checked")) {
            $("span[data-valmsg-for='IsVerifyEligibility']").hide();
            $("#hdnIsVerify").val('True');
        }
        else {
            $("span[data-valmsg-for='IsVerifyEligibility']").hide();
            $("#hdnIsVerify").val('False');
        }
    });

    $("input[name='IsSecondaryInsurance']").click(function () {
        if ($("#rbtnIsSecInsuranceY").is(":checked")) {
            $("span[data-valmsg-for='IsSecondaryInsurance']").hide(); 
            $("#hdnIsSecInsurance").val('True');
            $("#divSecInsurance").show();

        } else {
            $("span[data-valmsg-for='IsSecondaryInsurance']").hide();
            $("#hdnIsSecInsurance").val('False');
            $("#divSecInsurance").hide();

            $("#txtSecInsuranceCompanyName").val('');
            $("#txtSecInsuranceCompanyPhone").val('');
            $("#txtSecondaryCompanyAddr").val('');
            $("#txtSecondaryInsuranceID").val('');
            $("#txtSecInsuranceGroupID").val('');
            $("#txtSecondaryInsuredName").val('');
            $("#txtSecondaryInsuredDOB").val('');
        }
    });

    $("#patient_form input:radio").click(function () {

        if (this.id == "rbtnSexM")
        { $("#hdnSex").val('M'); $("span[data-valmsg-for='Sex']").hide(); }
        else if (this.id == "rbtnSexF")
        { $("#hdnSex").val('F'); $("span[data-valmsg-for='Sex']").hide(); }

        if (this.id == "rbtnIsInsuredY")
        { $("#hdnIsInsured").val("YES"); $("span[data-valmsg-for='IsPatInsured']").hide(); }
        else if (this.id == "rbtnIsInsuredN")
        { $("#hdnIsInsured").val("NO"); $("span[data-valmsg-for='IsPatInsured']").hide(); }

        if (this.id == "rbtnIsCondRelatedToN")
        { $("#hdnIsConditionRelatedTo").val('N'); $("span[data-valmsg-for='IsConditionRelatedTo']").hide(); }
        else if (this.id == "rbtnIsCondRelatedToA")
        { $("#hdnIsConditionRelatedTo").val('A'); $("span[data-valmsg-for='IsConditionRelatedTo']").hide(); }
        else if (this.id == "rbtnIsCondRelatedToE")
        { $("#hdnIsConditionRelatedTo").val('E'); $("span[data-valmsg-for='IsConditionRelatedTo']").hide(); }
        else if (this.id == "rbtnIsCondRelatedToO")
        { $("#hdnIsConditionRelatedTo").val('O'); $("span[data-valmsg-for='IsConditionRelatedTo']").hide(); }

        if (this.id == "rbtnIsSecInsuranceY")
        { $("#hdnIsSecInsurance").val('True'); $("span[data-valmsg-for='IsSecondaryInsurance']").hide(); }
        else if (this.id == "rbtnIsSecInsuranceN")
        { $("#hdnIsSecInsurance").val('False'); $("span[data-valmsg-for='IsSecondaryInsurance']").hide(); }

        if (this.id == "rbtnIsVerifyY")
        { $("#hdnIsVerify").val("True"); $("span[data-valmsg-for='IsVerifyEligibility']").hide(); }
        else if (this.id == "rbtnIsVerifyN")
        { $("#hdnIsVerify").val("False"); $("span[data-valmsg-for='IsVerifyEligibility']").hide(); }

    });

});

function onkd(e) {
    if (e.keyCode == 9 ){
        e.preventDefault();
        e.stopPropagation();
        $("#btnResetPatient").focus();
    }
}

//$(function () {
//    $("div[id*='datetimepicker']").datetimepicker({
//        format: 'MM/DD/YYYY',
//        maxDate: new Date(),
//        minDate: '01/01/1900',
//        showClear: true,
//        ignoreReadonly: true
//    });
//});