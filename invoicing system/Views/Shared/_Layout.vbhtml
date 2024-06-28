<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>@ViewBag.Title - My ASP.NET Application</title>
    @Styles.Render("~/Content/css")
    @Scripts.Render("~/bundles/modernizr")
</head
@Code
    ViewData("Title") = "Home Page"
    Dim loggedInCustomer As Customer = TryCast(Session("LoggedInCustomer"), Customer)
    Dim welcomeMessage As String = "Welcome, Guest"
    If loggedInCustomer IsNot Nothing Then
        welcomeMessage = "Welcome, " & loggedInCustomer.Name
    End If
End Code
<body>
    <nav class="navbar navbar-expand-sm navbar-toggleable-sm navbar-dark bg-dark">
        <div class="container">
            @Html.ActionLink("Invoicing System", "Index", "Home", New With {.area = ""}, New With {.class = "navbar-brand"})
            <button type="button" class="navbar-toggler" data-bs-toggle="collapse" data-bs-target=".navbar-collapse" title="Toggle navigation" aria-controls="navbarSupportedContent"
                    aria-expanded="false" aria-label="Toggle navigation">
                <span class="navbar-toggler-icon"></span>
            </button>

            <div class="collapse navbar-collapse d-sm-inline-flex justify-content-between">
                <ul class="navbar-nav flex-grow-1">


                    @If loggedInCustomer IsNot Nothing Then
                    @<li>@Html.ActionLink("Home", "Index", "Home", New With {.area = ""}, New With {.class = "nav-link"})</li>
                    @<li>@Html.ActionLink("Products", "Index", "Product", New With {.area = ""}, New With {.class = "nav-link"})</li>
                    @<li>@Html.ActionLink("Categories", "Index", "Category", New With {.area = ""}, New With {.class = "nav-link"})</li>
                    @<li>@Html.ActionLink("Cart", "Index", "Cart", New With {.area = ""}, New With {.class = "nav-link"})</li>
                    @<li>@Html.ActionLink("Logout", "Logout", "Account", New With {.area = ""}, New With {.class = "nav-link"})</li>


                    End If
                </ul>
            </div>
        </div>
    </nav>

    <div class="navbar navbar-inverse navbar-fixed-top">
        <div class="container">
            <div class="navbar-header">
                @If loggedInCustomer IsNot Nothing Then
                @<p class="navbar-text">@welcomeMessage  @Html.ActionLink("Edit My profile", "Edit", "Account", Nothing, New With {.class = "btn btn-link"})</p>
                Else
                @<p class="navbar-text">@welcomeMessage  @Html.ActionLink("Login/Register", "LoginRegister", "Account", New With {.area = ""}, New With {.class = "nav-link"})</p>
                End If
                
            </div>
            <div class="navbar-collapse collapse">
                <ul class="nav navbar-nav">
                    <li>@Html.ActionLink("Home", "Index", "Home")</li>
                    <!-- Add other menu items as needed -->
                </ul>
                @*<ul class="nav navbar-nav navbar-right">
                    <li><p class="navbar-text">@welcomeMessage</p></li>
                    @If loggedInCustomer IsNot Nothing Then
                        @<li>@Html.ActionLink("Logout", "Logout", "Account")</li>
                    Else
                        @<li>@Html.ActionLink("Login/Register", "LoginRegister", "Account")</li>
                    End If
                </ul>*@
            </div>
        </div>
    </div>

    <div class="container body-content">
        @RenderBody()
        <hr />
        <footer>
            <p>&copy; @DateTime.Now.Year - Invoice System</p>
        </footer>
    </div>

    @Scripts.Render("~/bundles/jquery")
    @Scripts.Render("~/bundles/bootstrap")
    @RenderSection("scripts", required:=False)
</body>
</html>
