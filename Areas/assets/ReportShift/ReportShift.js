Shift()
function Shift() {
    const currentTime = new Date();

    // Lấy giờ và phút từ đối tượng Date
    const currentHour = currentTime.getHours();
    const currentMinute = currentTime.getMinutes();

    var Shift;
    var currentShift = Number(currentHour) * 60 + currentMinute;
    var MidShift = 15 * 60;
    if (currentShift < MidShift) {
        Shift = 1
    } else {
        Shift=2
    }
    $('input[name="shift"]').val(Shift)
}

function GetShift(time) {
    const currentHour = time.substring(0, time.indexOf(":"));
    const currentMinute = time.substring(time.indexOf(":") + 1);
    var Shift;
    var currentShift = Number(currentHour) * 60 + currentMinute;
    var MidShift = 15 * 60;
    if (currentShift < MidShift) {
        Shift = 1
    } else {
        Shift = 2
    }
    return Shift;
}

var KTMaskDemo = function () {
    var demos = function () {
        $('input[name="kt_money_input"]').mask('000.000.000.000.000', {
            reverse: true
        });
    }
    return {
        init: function () {
            demos();
        }
    };
}();

jQuery(document).ready(function () {
    KTMaskDemo.init();
});