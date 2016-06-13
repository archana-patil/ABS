
$(document).ready(function () {

    $(".txtdatetimepicker").each(function () {
        $(this).keydown(function (e) {
            // Allow: backspace, delete, tab, escape, enter and .
            if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||
                // Allow: Ctrl+A, Command+A
                    (e.keyCode == 65 && (e.ctrlKey === true || e.metaKey === true)) ||
                // Allow: / & -
                (e.keyCode == 111) || (e.keyCode == 191) ||
                (e.keyCode == 109) || (e.keyCode == 189) ||
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
        $(this).val($(this).val().replace(/\-/g, "/"));
    });

    $(".txtdatetimepicker").focusout(function (e) {
        var data;
        var parts;
        //debugger;
        data = this.value;
        if (data != "") {
            data = data.replace(/\-/g, "/");
            var slashcount = (data.match(/\//g) || []).length;
            //var dashcount = (data.match(/-/g) || []).length;
            if (slashcount == 2) {
                var n = data.lastIndexOf('/');
                var result = data.substring(n + 1);//get year only
                if (result != '') {
                    var dformat = new Date(data);
                    if (dformat != "Invalid Date") {
                        //Chk leap year date
                        var year = "" + dformat.getFullYear();
                        var month = "" + (dformat.getMonth() + 1); //if (month.length == 1) { month = "0" + month; }
                        var day = "" + dformat.getDate(); //if (day.length == 1) { day = "0" + day; }
                        var arr = data.split("/");
                        if (parseInt(arr[0]) == month && parseInt(arr[1]) == day){// && parseInt(arr[2]) == year) {
                            if (dformat.getFullYear() >= 1900) {
                                dformat = [(("0" + (dformat.getMonth() + 1)).slice(-2)),
                                    ("0" + dformat.getDate()).slice(-2),
                                    dformat.getFullYear()].join('/');

                                if (dformat.split("/").length == 3) {
                                    var pastDate = new Date(dformat);
                                    var todaysDate = new Date();
                                    if (pastDate > todaysDate) {
                                        alert("Future date is not allowed.");
                                        $(this).focus();
                                        this.value = "";
                                    }
                                    else { $(this).val(dformat); }
                                }
                                else { $(this).val(dformat); }
                            }
                            else if (dformat.getFullYear().length == 4) {
                                alert("Date should be greater than 19 century.");
                                $(this).focus(); this.value = "";
                            }
                            else {
                                alert("Date should be in mm/dd/yyyy format.");
                                $(this).focus(); this.value = "";
                                //$(this).val('');
                            }
                        }
                        else {
                            alert("Invalid date.");
                            $(this).focus(); this.value = "";
                            //$(this).val('');
                        }
                    }
                    else {
                        alert("Date should be in mm/dd/yyyy format.");
                        $(this).focus(); this.value = "";
                        //$(this).val('');
                    }
                }
                else {
                    alert("Date should be in mm/dd/yyyy format.");
                    $(this).focus(); this.value = "";
                    //$(this).val('');
                }
            }
            else {
                alert("Date should be in mm/dd/yyyy format.");
                $(this).focus(); this.value = "";
                //$(this).val('');
            }
        }
    });

});