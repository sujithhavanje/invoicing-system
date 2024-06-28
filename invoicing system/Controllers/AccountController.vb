Imports System.Web.Mvc
Imports invoicing_system

Public Class AccountController
    Inherits Controller

    Function LoginRegister() As ActionResult
        Return View(New LoginRegister())
    End Function

    ' POST: Account/Login
    <HttpPost()>
    <ValidateAntiForgeryToken()>
    Function Login(model As LoginRegister) As ActionResult
        Try
            If String.IsNullOrEmpty(model.Username) OrElse String.IsNullOrEmpty(model.Password) Then
                ModelState.AddModelError("", "Username and password are required.")
                'Return View(model)
            End If

            Dim filePath As String = Server.MapPath("~/App_Data/Customer.csv")
            Dim customers = CsvHelper.ReadCsv(filePath, AddressOf CreateCustomer)
            Dim customer = customers.FirstOrDefault(Function(c) c.Username = model.Username AndAlso c.Password = model.Password)

            If customer IsNot Nothing Then
                Session("LoggedInCustomer") = customer
                Return RedirectToAction("Index", "Home")
            Else
                ModelState.AddModelError("", "Invalid username or password.")
                Return RedirectToAction("LoginRegister", "Account")
                ' Return View(model)
            End If
        Catch ex As Exception
            Logger.LogError(ex.Message)
        End Try

    End Function

    ' POST: Account/Register
    <HttpPost()>
    <ValidateAntiForgeryToken()>
    Function Register(model As LoginRegister) As ActionResult
        Try
            If ModelState.IsValid Then
                Dim filePath As String = Server.MapPath("~/App_Data/Customer.csv")
                Dim customers = CsvHelper.ReadCsv(filePath, AddressOf CreateCustomer)
                model.Customer.Id = If(customers.Count > 0, customers.Max(Function(c) c.Id) + 1, 1)
                customers.Add(model.Customer)
                CsvHelper.WriteCsv(filePath, customers, AddressOf CustomerToCsv)
                Dim message As String = "You are registered successfully!"
                ViewBag.AlertMessage = message
                Return RedirectToAction("LoginRegister")
            End If
            Return View("Login", model)
        Catch ex As Exception
            Logger.LogError(ex.Message)
        End Try
        Return View("Login", model)
    End Function

    ' GET: Account/Logout
    Function Logout() As ActionResult
        Session.Clear()
        Return RedirectToAction("Index", "Home")
    End Function


    Private Function CreateCustomer(values As String()) As Customer
        Return New Customer With {
            .Id = Integer.Parse(values(0)),
            .Username = values(1),
            .Password = values(2),
            .Name = values(3),
            .Email = values(4),
            .Address = values(5),
            .ContactNumber = values(6)
        }
    End Function

    ' GET: Account/Edit
    Function Edit() As ActionResult
        Try
            Dim loggedInCustomer As Customer = TryCast(Session("LoggedInCustomer"), Customer)

            If loggedInCustomer Is Nothing Then
                Return RedirectToAction("LoginRegister")
            End If

            Return View(loggedInCustomer)
        Catch ex As Exception
            Logger.LogError(ex.Message)
        End Try

    End Function

    ' POST: Account/Edit
    <HttpPost()>
    <ValidateAntiForgeryToken()>
    Function Edit(model As Customer) As ActionResult
        Try

            If ModelState.IsValid Then

                Dim filePath As String = Server.MapPath("~/App_Data/Customer.csv")
                Dim customers = CsvHelper.ReadCsv(filePath, AddressOf CreateCustomer).ToList()

                Dim customer = customers.FirstOrDefault(Function(c) c.Username = model.Username)
                If customer IsNot Nothing Then
                    customer.Password = model.Password
                    customer.Name = model.Name
                    customer.Email = model.Email
                    customer.Address = model.Address
                    customer.ContactNumber = model.ContactNumber

                    CsvHelper.WriteCsv(filePath, customers, Function(c) CustomerToCsv(c))
                    Session("LoggedInCustomer") = customer
                    Return RedirectToAction("Index", "Home")
                End If
            End If
        Catch ex As Exception
            Logger.LogError(ex.Message)
        End Try

        Return View(model)
    End Function

    Private Function CustomerToCsv(customer As Customer) As String()
        Return New String() {customer.Id.ToString(), customer.Username, customer.Password, customer.Name, customer.Email, customer.Address, customer.ContactNumber}
    End Function
End Class
