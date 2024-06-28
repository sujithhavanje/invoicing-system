@ModelType CartViewModel
@Code
    ViewData("Title") = "Shopping Cart"
End Code

<h2>Shopping Cart</h2>

<table class="table">
    <thead>
        <tr>
            <th>Product Name</th>
            <th>Price</th>
            <th>Quantity</th>
       
        </tr>
    </thead>
    <tbody>
        @For Each product In Model.Products
            @<tr>
                <td>@product.Name</td>
                <td>@product.Price.ToString("C")</td>
                <td>
                    @Using Html.BeginForm("AddToCart", "Cart", FormMethod.Post)
                        @Html.AntiForgeryToken()
                        @Html.Hidden("productId", product.Id)
                        @Html.TextBox("quantity", "1", New With {.type = "number", .min = "1", .class = "form-control"})
                        @<input type="submit" value="Add to Cart" class="btn btn-primary" />
                    End Using
                </td>
            </tr>
        Next
    </tbody>
</table>

<div class="col-md-4">
    @* Display items in the cart *@
    <h3>Cart Items</h3>
    <table class="table">
        <thead>
            <tr>
                <th>Product Name</th>
                <th>Quantity</th>
                <th>Price</th>
                <th>Total</th>
                <th>Action</th>
            </tr>
        </thead>
        <tbody>
            @If Model.CartItems IsNot Nothing AndAlso Model.CartItems.Any() Then
                @For Each cartItem In Model.CartItems
                    @<tr>
                        <td>@cartItem.ProductName</td>
                        <td>@cartItem.Quantity</td>
                        <td>@cartItem.UnitPrice</td>
                        <td>@(cartItem.Quantity * cartItem.UnitPrice)</td>
                        <td>
                            @Using Html.BeginForm("RemoveFromCart", "Cart", FormMethod.Post)
                                @Html.AntiForgeryToken()
                                @Html.Hidden("productId", cartItem.ProductId)
                                @<input type="submit" value="Remove" class="btn btn-danger" />
                            End Using
                        </td>
                    </tr>
                Next
            Else
                @<tr>
                    <td colspan="5">No items in the cart</td>
                </tr>
            End If
        </tbody>
    </table>
</div>

@Using Html.BeginForm("GenerateInvoice", "Cart", FormMethod.Post, New With {.id = "checkoutForm"})
    @Html.AntiForgeryToken()

    @<div Class="form-group">
        @Html.Label("Discount", htmlAttributes:=New With {.class = "control-label col-md-2"})
        <div Class="col-md-10">
            @Html.DropDownList("discountId", Model.Discounts, "Select discount", New With {.class = "form-control", .id = "discountId"})
            <span id="discountError" Class="text-danger"></span>
        </div>
    </div>

    @<div Class="form-group">
        @Html.Label("Tax", htmlAttributes:=New With {.class = "control-label col-md-2"})
        <div Class="col-md-10">
            @Html.DropDownList("taxId", Model.Taxes, "Select tax", New With {.class = "form-control", .id = "taxId"})
            <span id="taxError" Class="text-danger"></span>
        </div>
    </div>

    @<div Class="form-group">
        @Html.Label("Payment Method", htmlAttributes:=New With {.class = "control-label col-md-2"})
        <div Class="col-md-10">
            @Html.DropDownList("paymentMethodId", Model.PaymentMethods, "Select payment method", New With {.class = "form-control", .id = "paymentMethodId"})
            <span id="paymentMethodError" Class="text-danger"></span>
        </div>
    </div>

    @<div Class="form-group">
        <div Class="col-md-offset-2 col-md-10">
            <input type="submit" value="Checkout" Class="btn btn-primary" />
        </div>
    </div>
End Using

@section Scripts {
    <script>
        $(function () {
            $('#checkoutForm').submit(function (e) {
                // Validate Discount
                var discountId = $('#discountId').val();
                if (!discountId || discountId === "0") {
                    $('#discountError').text('Discount is required.');
                    e.preventDefault(); // Prevent form submission
                    return false;
                } else {
                    $('#discountError').text('');
                }

                // Validate Tax
                var taxId = $('#taxId').val();
                if (!taxId || taxId === "0") {
                    $('#taxError').text('Tax is required.');
                    e.preventDefault(); // Prevent form submission
                    return false;
                } else {
                    $('#taxError').text('');
                }

                // Validate Payment Method
                var paymentMethodId = $('#paymentMethodId').val();
                if (!paymentMethodId || paymentMethodId === "0") {
                    $('#paymentMethodError').text('Payment Method is required.');
                    e.preventDefault(); // Prevent form submission
                    return false;
                } else {
                    $('#paymentMethodError').text('');
                }

                // If all validations pass, allow form submission
                return true;
            });
        });
    </script>
    }
    End Section