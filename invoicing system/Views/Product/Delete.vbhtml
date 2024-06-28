@ModelType invoicing_system.Product

@Code
    ViewData("Title") = "Delete Product"
End Code


<div>
    <h4>Are you sure you want to delete this product?</h4>
    <hr />
    <dl class="dl-horizontal">
        <dt>
            @Html.DisplayNameFor(Function(model) model.Name)
        </dt>
        <dd>
            @Html.DisplayFor(Function(model) model.Name)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.Description)
        </dt>
        <dd>
            @Html.DisplayFor(Function(model) model.Description)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.Price)
        </dt>
        <dd>
            @Html.DisplayFor(Function(model) model.Price)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.Quantity)
        </dt>
        <dd>
            @Html.DisplayFor(Function(model) model.Quantity)
        </dd>

       
    </dl>

    @Using (Html.BeginForm("Delete", "Product", New With {.id = Model.Id}, FormMethod.Post, New With {.class = "form-horizontal", .role = "form"}))
        @Html.AntiForgeryToken()

        @<div class="form-group">
            <div class="col-md-offset-2 col-md-10">
                <input type="submit" value="Delete" class="btn btn-danger" />
                @Html.ActionLink("Cancel", "Index", Nothing, New With {.class = "btn btn-default"})
            </div>
        </div>
    End Using
</div>


@section Scripts
    @Scripts.Render("~/bundles/jqueryval")
End Section

