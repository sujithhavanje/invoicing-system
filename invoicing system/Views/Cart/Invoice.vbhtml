@ModelType Invoice

@Code
    ViewData("Title") = "Invoice"

    Dim loggedInCustomer As Customer = TryCast(Session("LoggedInCustomer"), Customer)

End Code


<h2>Invoice</h2>

<div class="invoice-details">
    <h3>Customer Information</h3>
    <p>Name: @loggedInCustomer.Name</p>
    <p>Email: @loggedInCustomer.Email</p>
    <p>Address: @loggedInCustomer.ContactNumber</p>
    <p>Address: @loggedInCustomer.Address</p>
</div>

<div class="invoice-items">
    <h3>Purchased Products</h3>
    <table class="table">
        <thead>
            <tr>
                <th>Product Name</th>
                <th>Quantity</th>
                <th>Unit Price</th>
                <th>Total</th>
            </tr>
        </thead>
        <tbody>
            @For Each item In Model.CartItems
                @<tr>
                    <td>@item.ProductName</td>
                    <td>@item.Quantity</td>
                    <td>@item.UnitPrice.ToString("C")</td>
                    <td>@(item.Quantity * item.UnitPrice)</td>
                </tr>
            Next
        </tbody>
    </table>
</div>

<div class="invoice-summary">
    <h3>Invoice Summary</h3>
    <p>Subtotal: @Model.Subtotal.ToString("C")</p>
    <p>Discount: @Model.DiscountAmount.ToString("C")</p>
    <p>Tax: @Model.TaxAmount.ToString("C")</p>
    <p><strong>Total Amount: @Model.TotalAmount.ToString("C")</strong></p>
    <p>Payment Method: @Model.PaymentMethod</p>
</div>

<div>
    @Html.ActionLink("Back to Shopping", "Index", "Product", New With {.class = "btn btn-default"})
</div>
