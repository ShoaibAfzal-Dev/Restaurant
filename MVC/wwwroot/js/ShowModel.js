var form = new FormData();
var totalPrice;

function showItemDetails(itemId, price, Name, description) {
    //var form = new FormData();
     totalPrice = parseInt(price);

    form.append("Name", Name);
    form.append("Price", price);
    form.append("Description", description);

    $.ajax({
        type: "GET",
        // add your url
        url: ``,
        success: function (data) {
            console.log(data);
            if (data == "No types in this Id") {
                $('#exam').html('<h1>No data exist on this request</h1>');
                $('#exampleModal2').modal('show');
                return;
            }

            $('#exam').empty();

            if (data.length > 0) {
                $('#itemDetailsContainer').text(itemId);

                var detailsHtml = '<div class="details-container">';
                detailsHtml += `<h6 style="width:300px; padding:10px;padding-bottom:0px">${description}</h6>`;

                data.forEach(category => {
                    detailsHtml += `<div class="category-details">
                        <h3>${category.name}</h3>`;

                    category.order_SubTypes.forEach(subType => {
                        detailsHtml += `<div class="category-sub-details">
                            <div>
                                <input type="checkbox" class="subtype-checkbox"
                                    data-subtypeid="${subType.id}" onchange="handleCheckboxChange(this)"/>
                                <label style="padding-left:10px">${subType.name}</label>
                            </div>
                            <p style="margin: 5px">+${subType.plusPrice}</p>
                        </div>`;
                    });

                    detailsHtml += '</div>';
                });

                detailsHtml += '<div class="adddd-dtal">';
                detailsHtml += '<h6>Special Instructions</h6><textarea onchange="handlechange()" class="ffefew special-inst" placeholder="Special Instructions"></textarea>';
                detailsHtml += '<h6>Quantity</h6><input onchange="handlechange()" class="ffefew gertert" type="number" value="1" min="1"/>';

                detailsHtml += '</div>';

                detailsHtml += '</div>';
                detailsHtml += `<div class="btn-add-cart" onclick="addToCart()">
                    <h6 class="pdatedprc" style="padding-right:20px;border-right:1px solid black">Kr ${totalPrice.toFixed(2)}</h6>
                    <h6>Add To Cart</h6>
                </div>`;

                $('#exam').html(detailsHtml);
                $('#exampleModal2').modal('show');
            } else {
                $('#exam').html('<h1>No data exist on this request</h1>');
                $('#exampleModal2').modal('show');
            }
        },
        error: function (error) {
            $('#exam').html('<h1>No data exist on this request</h1>');
            $('#exampleModal2').modal('show');
        }
    });

}

    function handleCheckboxChange(checkbox) {

        const subTypeId = $(checkbox).data('subtypeid');

        const isChecked = checkbox.checked;

        const parentDiv = $(checkbox).closest('.category-sub-details');

        const category = parentDiv.closest('.category-details').find('h3').text();

        var pt = parentDiv.find('p').text();

        var ptWithoutSign = pt.substring(1);

        var rt = parseInt(ptWithoutSign, 10);

        const name = parentDiv.find('label').text();

        const fin = `${category} ${name}`;

        if (isChecked) {

            totalPrice = rt + totalPrice;

            const uniqueKey = `${category}_${name}_${Date.now()}`;

            const pricekey = `${category}_${name}_${Date.now()}`


            form.append(`Addition_${uniqueKey}`, fin);
            form.append(`AdditionPrice_${pricekey}`, rt);

        } else {
            totalPrice -= rt;

            for (var pair of form.entries()) {
                if (pair[0].startsWith("Addition_") && pair[1] === fin) {
                    form.delete(pair[0]);
                    const priceKeyPrefix = 'AdditionPrice_' + pair[0].substring('Addition_'.length);
                    for (var pricePair of form.entries()) {
                        if (pricePair[0] === priceKeyPrefix) {
                            form.delete(pricePair[0]);
                        }
                    }
                }
            }
        }
        $(".pdatedprc").text(`Kr ${totalPrice.toFixed(2)}`);
    }

    function addToCart() {
        // console.log('Add to Cart clicked    ' + totalPrice);

        var qty = $(".gertert").val();
        var si = $(".special-inst").val();
        if (form.has("Instruction")) {
            form.delete("Instruction");
        }
        if (form.has("Qty")) {
            form.delete("Qty");
        }

        form.append("Instruction", si);
        form.append("Qty", qty);
        form.append("totalPrice", totalPrice);

        var userid = $(".user-main-id").text();
        if (userid === "") {
            window.location = `/Account/login`;
        }
        else {
            form.append("User-id", userid);
            for (var pair of form.entries()) {
                console.log(pair[0] + ', ' + pair[1]);
            }

            $.ajax({

                type: 'POST',
                // add your url
                url: '',
                data: JSON.stringify(Object.fromEntries(form.entries())),
                processData: false,
                contentType: 'application/json',
                dataType: 'json',
                success: function (response) {
                    window.location = `Orders/cart`;
                },
                error: function (error) {
                    console.log('Error:' + error);
                }
            });
        }
    }
