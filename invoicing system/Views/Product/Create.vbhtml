@ModelType invoicing_system.Product
@Imports System.Collections
@Code
    ViewData("Title") = "Create Product"
    ' Read categories from CSV file
    Dim categoriesFilePath As String = Server.MapPath("~/App_Data/Category.csv")
    Dim categories As New List(Of SelectListItem)()

    Using reader As New StreamReader(categoriesFilePath)
        While Not reader.EndOfStream
            Dim line As String = reader.ReadLine()
            Dim parts As String() = line.Split(","c)
            If parts.Length = 3 Then
                categories.Add(New SelectListItem() With {
                    .Value = parts(0),
                    .Text = parts(1)
                })
            End If
        End While
    End Using

    ViewData("Categories") = New SelectList(categories, "Value", "Text")
End Code

<h2>Create Product</h2>

@Using Html.BeginForm()
    @Html.AntiForgeryToken()
    @<div class="form-horizontal">
        <hr />
        @Html.ValidationSummary(True, "", New With {.class = "text-danger"})

        <div class="form-group">
            @Html.LabelFor(Function(model) model.Name, htmlAttributes:=New With {.class = "control-label col-md-2"})
            <div class="col-md-10">
                @Html.EditorFor(Function(model) model.Name, New With {.htmlAttributes = New With {.class = "form-control"}})
                @Html.ValidationMessageFor(Function(model) model.Name, "", New With {.class = "text-danger"})
            </div>
        </div>

        <div class="form-group">
            @Html.LabelFor(Function(model) model.Description, htmlAttributes:=New With {.class = "control-label col-md-2"})
            <div class="col-md-10">
                @Html.EditorFor(Function(model) model.Description, New With {.htmlAttributes = New With {.class = "form-control"}})
                @Html.ValidationMessageFor(Function(model) model.Description, "", New With {.class = "text-danger"})
            </div>
        </div>

        <div class="form-group">
            @Html.LabelFor(Function(model) model.Price, htmlAttributes:=New With {.class = "control-label col-md-2"})
            <div class="col-md-10">
                @Html.EditorFor(Function(model) model.Price, New With {.htmlAttributes = New With {.class = "form-control"}})
                @Html.ValidationMessageFor(Function(model) model.Price, "", New With {.class = "text-danger"})
            </div>
        </div>

        <div class="form-group">
            @Html.LabelFor(Function(model) model.Quantity, htmlAttributes:=New With {.class = "control-label col-md-2"})
            <div class="col-md-10">
                @Html.EditorFor(Function(model) model.Quantity, New With {.htmlAttributes = New With {.class = "form-control"}})
                @Html.ValidationMessageFor(Function(model) model.Quantity, "", New With {.class = "text-danger"})
            </div>
        </div>

        <div class="form-group">
            @Html.LabelFor(Function(model) model.CategoryId, "Category", htmlAttributes:=New With {.class = "control-label col-md-2"})
            <div class="col-md-10">
                @Html.DropDownListFor(Function(model) model.CategoryId, CType(ViewData("Categories"), SelectList), "Select Category", New With {.class = "form-control"})
                @Html.ValidationMessageFor(Function(model) model.CategoryId, "", New With {.class = "text-danger"})
            </div>
        </div>

        <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
                <input type="submit" value="Create" class="btn btn-default" />
            </div>
        </div>
    </div>
End Using

<div>
    @Html.ActionLink("Back to List", "Index")
</div>

@section Scripts
    @Scripts.Render("~/bundles/jqueryval")
End Section
