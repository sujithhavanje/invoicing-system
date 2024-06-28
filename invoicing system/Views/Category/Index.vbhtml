@ModelType IEnumerable(Of invoicing_system.Category)

@Code
    ViewData("Title") = "Category"
End Code

<h2>Category</h2>

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
                                                                    grid.Column("Actions", format:=Function(item) Html.Raw(
                                                                        Html.ActionLink("Edit", "Edit", New With {.id = item.Id}).ToString() + " | " +
                                                                        Html.ActionLink("Delete", "Delete", New With {.id = item.Id}).ToString()
                                                                    ))
                                                                )
                                                            )
