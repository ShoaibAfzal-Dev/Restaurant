
$(document).ready(function () {
   


    var form = new FormData();
    $('.Submit-personal').on('click', function () {
        var firstName = $('#firstName').val();
        var lastName = $('#lastName').val();
        var email = $('#email').val();
        var telephone = $('#telephone').val();

        if (firstName === "")
        { $('#firstName').css('border-color', 'red') }
        else if (firstName !== "")
        { $('#firstName').css('border-color', '') }

        if (lastName === "")
        { $('#lastName').css('border-color', 'red') }
        else if (lastName !== "")
        { $('#lastName').css('border-color', '') }

        if (email === "")
        { $('#email').css('border-color', 'red') }
        else if (email !== "")
        { $('#email').css('border-color', '') }

        if (telephone === "")
        { $('#telephone').css('border-color', 'red') }
        else if (telephone !== "")
        { $('#telephone').css('border-color', '') }

        if (firstName === "" || lastName === ""
            || email === "" || telephone === "")
        {
            return;
        }

        if (form.has("First Name:")) {
            form.delete("First Name:");
        }
        if (form.has("Last Name:")) {
            form.delete("Last Name:");
        }
        if (form.has("Email:")) {
            form.delete("Email:");
        }
        if (form.has("Telephone:")) {
            form.delete("Telephone:");
        }
        form.append('First Name:', firstName)
        form.append('Last Name:', lastName)
        form.append('Email:', email)
        form.append('Telephone:', telephone)


        for (var pair of form.entries()) {
            console.log(pair[0] + ' - ' + pair[1]);
        }


        $(".cancel-btn").css('display', 'none')
        $(".personal-info").css('display', 'none')
        $(".details-button").css('display', 'block')


    });




    $(".cancel-btn").click(function () {


        if (form.has("First Name:")) {
            form.delete("First Name:");
        }

        if (form.has("Last Name:")) {
            form.delete("Last Name:");
        }

        if (form.has("Email:")) {
            form.delete("Email:");
        }

        if (form.has("Telephone:")) {
            form.delete("Telephone:");
        }

        $('#firstName').val('');
        $('#lastName').val('');
        $('#email').val('');
        $('#telephone').val('');

        $(".cancel-btn").css('display', 'none')
        $(".personal-info").css('display', 'none')
        $(".details-button").css('display', 'block');


    });


    $('.Submit-time-personal').on('click', function () {
        var orderDay = $('#order-day').val();
        var orderTime = $('#order-time').val();
       /* var dsx = 0;
        for (var pair of form.entries()) {
            dsx++;
        }
        if (dsx < 4) {
            console.log("form is not complete")
        }
        else if (dsx >3 ) {*/
            if (form.has('Order Day:')) {
                form.delete('Order Day:');
            }
            if (form.has('Order Time:')) {
                form.delete('Order Time:');
            }
            form.append('Order Day:', orderDay);
            form.append('Order Time:', orderTime);
        /*}*/
        for (var pair of form.entries()) {
            console.log(pair[0] + ' - ' + pair[1]);
        }

        $(".time-button").css('display', 'block');
        $(".cancel-btn-time").css('display', 'none');
        $(".order-date-info").css('display', 'none');


    });

    $(".cancel-time-btn").click(function () {
        $('#order-day').val('');
        $('#order-time').val('');
    });


    $(".Submit-payment-personal").on("click", function () {
       
        form.delete("PaymentMethod");
        $(".checkbox-container").on("click", function () {
            var checkbox = $(this).find("input[type='checkbox']");
            checkbox.prop("checked", !checkbox.prop("checked"));
        });
       $(".checkbox-container input:checked").each(function () {
            form.append("PaymentMethod", $(this).attr("id"));
        });

        for (var pair of form.entries()) {
            console.log(pair[0] + ' - ' + pair[1]);
        }
        $(".payment-button").css('display', 'block');
        $(".cancel-btn-payment").css('display', 'none');
        $(".order-payment-info").css('display', 'none');
    });
    $(".Place-Order-button").click(function () {



       // console.log(context.ses)
        var cartIds = $("[data-cart-id]").map(function () {
             return $(this).data("cart-id");
         }).get();
        if (form.has("ids")) {
            form.delete("ids");
        }
        if (cartIds.length == 0) {
            toastr.error("Data not exist");
            return;
        }

        form.append("ids", cartIds);

        if (form.has("ids") !==[] && form.has("PaymentMethod") && form.has("Order Day:") && form.has("Order Time:")
            && form.has("Telephone:") && form.has("First Name:") && form.has("Last Name:")
            && form.has("Email:")) {

            $.ajax({
                type: "POST",
                //add your url
                url: ``,
                data: JSON.stringify(Object.fromEntries(form.entries())),
                contentType: 'application/json',
                dataType: 'json',
                success: function (response) {
                    console.log(response);
                   // toastr.success("Your order is placed successfully")
                    window.location.reload();
                },
                error: function (error) {
                   toastr.error("Something went wrong please try again")

                }
            });

        }
        else {
            toastr.error("Please fill all the details")
        }

        /*var count=0;
        for (var pair of form.entries()) {
            count++;
        }
        console.log(count);*/

       /* $.ajax({
            type: "POST",
            url: ``,
            data: JSON.stringify(Object.fromEntries(form.entries())),
            contentType: 'application/json',
            dataType: 'json',
            success: function (response) {
                console.log(response);
            },
            error: function (error) {
                console.log('Error: ' + error.fail);
            }
        });*/


    });

    function deleteCart(id) {
        console.log(id);
        $.ajax({
            type: "DELETE",
            // add your url
            url: ``,
            success: function () {
                window.location.reload();
            },
            error: function () {  
                toastr.error("Some thing went wrong");
            }
        })
    }

    $(".delete-cart-dta").on("click", function () {
        var id = $(this).data("cart-id");
        deleteCart(id);
    });
});
