@ModelType IEnumerable(Of invoicing_system.Product)

@Code
    ViewData("Title") = "Products"
End Code

<h2>Products</h2>

<p>
    @Html.ActionLink("Create New", "Create")
</p>

@Code
    Dim grid = New WebGrid(Model, canPage:=True, canSort:=True, rowsPerPage:=10)
End Code

@grid.GetHtml(
                                                                tableStyle:="table",
                                                                headerStyle:="thead-dark",
                                                                alternatingRowStyle:="table-striped",
                                                                columns:=grid.Columns(
                                                                    grid.Column("Name", "Name"),
                                                                    grid.Column("Description", "Description"),
                                                                    grid.Column("Price", "Price"),
                                                                    grid.Column("Quantity", "Quantity"),
                                                                    grid.Column("CategoryName", "CategoryName"),
                                                                    grid.Column("Actions", format:=Function(item) Html.Raw(
                                                                        Html.ActionLink("Edit", "Edit", New With {.id = item.Id}).ToString() + " | " +
                                                                        Html.ActionLink("Delete", "Delete", New With {.id = item.Id}).ToString()
                                                                    ))
                                                                )
                                                            )
