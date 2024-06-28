@ModelType invoicing_system.LoginRegister

@Code
    ViewData("Title") = "Login or Register"
End Code

<h2>Login</h2>

@Html.ValidationSummary(True, "", New With {.class = "text-danger"})

@If ViewBag.AlertMessage IsNot Nothing Then
@<script>
$(function () {
alert('@ViewBag.AlertMessage');
});
</script>
End If

@Using Html.BeginForm("Login", "Account", FormMethod.Post, New With {.onsubmit = "return validateForm()"})
    @Html.AntiForgeryToken()
    @<div class="form-horizontal">
        <hr />
        @Html.ValidationSummary(True, "", New With {.class = "text-danger"})

        <div class="form-group">
            @Html.Label("Username", htmlAttributes:=New With {.class = "control-label col-md-2"})
            <div class="col-md-10">
                @Html.TextBox("Username", Nothing, New With {.class = "form-control", .id = "username"})
                @Html.ValidationMessageFor(Function(model) model.Username, "", New With {.class = "text-danger"})
            </div>
        </div>

        <div class="form-group">
            @Html.Label("Password", htmlAttributes:=New With {.class = "control-label col-md-2"})
            <div class="col-md-10">
                @Html.Password("Password", Nothing, New With {.class = "form-control", .id = "password"})
                @Html.ValidationMessageFor(Function(model) model.Password, "", New With {.class = "text-danger"})
            </div>
        </div>

        <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
                <input type="submit" value="Login" class="btn btn-default" />
            </div>
        </div>
    </div>
End Using

@Using Html.BeginForm("Register", "Account", FormMethod.Post)
    @Html.AntiForgeryToken()
    @<div class="form-horizontal">
        <h4>Register</h4>
        <hr />
        @Html.ValidationSummary(True, "", New With {.class = "text-danger"})

        <div class="form-group">
            @Html.LabelFor(Function(model) model.Customer.Username, htmlAttributes:=New With {.class = "control-label col-md-2"})
            <div class="col-md-10">
                @Html.EditorFor(Function(model) model.Customer.Username, New With {.htmlAttributes = New With {.class = "form-control"}})
                @Html.ValidationMessageFor(Function(model) model.Customer.Username, "", New With {.class = "text-danger"})
            </div>
        </div>

        <div class="form-group">
            @Html.LabelFor(Function(model) model.Customer.Password, htmlAttributes:=New With {.class = "control-label col-md-2"})
            <div class="col-md-10">
                @Html.EditorFor(Function(model) model.Customer.Password, New With {.htmlAttributes = New With {.class = "form-control"}})
                @Html.ValidationMessageFor(Function(model) model.Customer.Password, "", New With {.class = "text-danger"})
            </div>
        </div>

        <div class="form-group">
            @Html.LabelFor(Function(model) model.Customer.Name, htmlAttributes:=New With {.class = "control-label col-md-2"})
            <div class="col-md-10">
                @Html.EditorFor(Function(model) model.Customer.Name, New With {.htmlAttributes = New With {.class = "form-control"}})
                @Html.ValidationMessageFor(Function(model) model.Customer.Name, "", New With {.class = "text-danger"})
            </div>
        </div>

        <div class="form-group">
            @Html.LabelFor(Function(model) model.Customer.Email, htmlAttributes:=New With {.class = "control-label col-md-2"})
            <div class="col-md-10">
                @Html.EditorFor(Function(model) model.Customer.Email, New With {.htmlAttributes = New With {.class = "form-control"}})
                @Html.ValidationMessageFor(Function(model) model.Customer.Email, "", New With {.class = "text-danger"})
            </div>
        </div>

        <div class="form-group">
            @Html.LabelFor(Function(model) model.Customer.Address, htmlAttributes:=New With {.class = "control-label col-md-2"})
            <div class="col-md-10">
                @Html.EditorFor(Function(model) model.Customer.Address, New With {.htmlAttributes = New With {.class = "form-control"}})
                @Html.ValidationMessageFor(Function(model) model.Customer.Address, "", New With {.class = "text-danger"})
            </div>
        </div>

        <div class="form-group">
            @Html.LabelFor(Function(model) model.Customer.ContactNumber, htmlAttributes:=New With {.class = "control-label col-md-2"})
            <div class="col-md-10">
                @Html.EditorFor(Function(model) model.Customer.ContactNumber, New With {.htmlAttributes = New With {.class = "form-control"}})
                @Html.ValidationMessageFor(Function(model) model.Customer.ContactNumber, "", New With {.class = "text-danger"})
            </div>
        </div>

        <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
                <input type="submit" value="Register" class="btn btn-default" />
            </div>
        </div>
    </div>
End Using

@section Scripts
    @Scripts.Render("~/bundles/jqueryval")
    <script>
        function validateForm() {
            var username = document.getElementById("username").value;
            var password = document.getElementById("password").value;

            if (username === "" || password === "") {
                alert("Username and password are required.");
                return false;
            }
            return true;
        }
    </script>
End Section
