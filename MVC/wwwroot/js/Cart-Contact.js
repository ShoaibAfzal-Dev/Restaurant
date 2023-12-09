
$(document).ready(function () {
    $(".details-button").click(function () {
        $(".cancel-btn").css('display', 'flex')
        $(".personal-info").css({
            'display': 'flex',
            'flex-direction':'column',
        })
        $(".details-button").css('display','none')
    })
    
    $(".time-button").click(function () {
        $(".cancel-btn-time").css('display', 'flex');
        $(".time-button").css('display', 'none');
        $(".order-date-info").css({
            'display': 'flex',
            'flex-direction': 'column',
        })
    })
    $(".cancel-btn-time").click(function () {
        $(".time-button").css('display', 'block');
        $(".cancel-btn-time").css('display', 'none');
        $(".order-date-info").css('display', 'none');

    })
    $(".payment-button").click(function () {
        $(".cancel-btn-payment").css('display', 'flex');
        $(".payment-button").css('display', 'none');
        $(".order-payment-info").css({
            'display': 'flex',
            'flex-direction': 'column',
        })
    })
    $(".cancel-btn-payment").click(function () {
        $(".payment-button").css('display', 'block');
        $(".cancel-btn-payment").css('display', 'none');
        $(".order-payment-info").css('display', 'none');

    })
    $(".coupon").click(function () {
        $(".coupon-form").css('display', 'flex')
        $(".coupon").css('display','none')
    })
    $(".Cancel-coupon").click(function () {
        $(".coupon-form").css('display', 'none')
        $(".coupon").css('display', 'block')
    })

    var select = document.getElementById("order-time");

    for (var hours = 10; hours <= 22; hours++) {
        for (var minutes = 0; minutes < 60; minutes += 15) {
            var formattedHours = hours.toString().padStart(2, '0');
            var formattedMinutes = minutes.toString().padStart(2, '0');
            var timeValue = formattedHours + ':' + formattedMinutes;
            var option = document.createElement("option");
            option.value = timeValue;
            option.text = formattedHours + ':' + formattedMinutes;
            select.appendChild(option);
            if (hours === 22 && minutes === 45) {
                break;
            }
        }
    }
    let currentDate = new Date();

    let currentDayIndex = currentDate.getDay(); 

    let daysOfWeek = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'];

    
    let remainingDays = daysOfWeek.slice(currentDayIndex + 0).concat(daysOfWeek.slice(0, currentDayIndex));


    let formattedOutput = remainingDays.map((day, index) => {
        let futureDate = new Date();
        futureDate.setDate(currentDate.getDate() + index + 0);
        let month = futureDate.toLocaleString('en-us', { month: 'long' });
        let date = futureDate.getDate();
        return `${day}, ${month} ${date}`;
    });

    let d = formattedOutput;

    var select = document.getElementById("order-day");

    for (var day = 0; day <=4 ; day++) {
       
            var option = document.createElement("option");
            option.value = d[day];
            option.text = d[day];
        select.appendChild(option);

    }

});