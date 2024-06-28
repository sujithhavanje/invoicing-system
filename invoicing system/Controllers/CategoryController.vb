Imports System.Web.Mvc
Imports System.IO
Imports System.Net

Public Class CategoryController
    Inherits Controller

    ' GET: Category
    Function Index() As ActionResult
        Try
            If Session("LoggedInCustomer") IsNot Nothing Then
                Dim filePath As String = Server.MapPath("~/App_Data/Category.csv")
                Dim categories = CsvHelper.ReadCsv(filePath, AddressOf CreateCategory)
                Return View(categories)
            Else
                Return RedirectToAction("LoginRegister", "Account")
            End If
        Catch ex As Exception
            Logger.LogError(ex.Message)
            ' Optionally handle or redirect to an error view
            Return RedirectToAction("Error", "Home")
        End Try
    End Function

    ' GET: Category/Create
    Function Create() As ActionResult
        Return View()
    End Function

    ' POST: Category/Create
    <HttpPost()>
    <ValidateAntiForgeryToken()>
    Function Create(category As Category) As ActionResult
        Try
            If ModelState.IsValid Then
                Dim filePath As String = Server.MapPath("~/App_Data/Category.csv")
                Dim categories = CsvHelper.ReadCsv(filePath, AddressOf CreateCategory)
                category.Id = If(categories.Count > 0, categories.Max(Function(p) p.Id) + 1, 1)
                categories.Add(category)
                CsvHelper.WriteCsv(filePath, categories, AddressOf CategoryToCsv)
                Return RedirectToAction("Index")
            End If
            Return View(category)
        Catch ex As Exception
            Logger.LogError(ex.Message)
            ' Optionally handle or redirect to an error view
            Return RedirectToAction("Error", "Home")
        End Try
    End Function

    Function Edit(id As Integer?) As ActionResult
        Try
            If id Is Nothing Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim filePath As String = Server.MapPath("~/App_Data/Category.csv")
            Dim categories = CsvHelper.ReadCsv(filePath, AddressOf CreateCategory)
            Dim category = categories.FirstOrDefault(Function(p) p.Id = id)
            If category Is Nothing Then
                Return HttpNotFound()
            End If

            Return View(category)
        Catch ex As Exception
            Logger.LogError(ex.Message)
            ' Optionally handle or redirect to an error view
            Return RedirectToAction("Error", "Home")
        End Try
    End Function

    <HttpPost()>
    <ValidateAntiForgeryToken()>
    Function Edit(category As Category) As ActionResult
        Try
            If ModelState.IsValid Then
                Dim filePath As String = Server.MapPath("~/App_Data/Category.csv")
                Dim categories = CsvHelper.ReadCsv(filePath, AddressOf CreateCategory)
                Dim existingCategory = categories.FirstOrDefault(Function(p) p.Id = category.Id)
                If existingCategory IsNot Nothing Then
                    existingCategory.Name = category.Name
                    existingCategory.Description = category.Description
                    CsvHelper.WriteCsv(filePath, categories, AddressOf CategoryToCsv)
                End If
                Return RedirectToAction("Index")
            End If
            Return View(category)
        Catch ex As Exception
            Logger.LogError(ex.Message)
            ' Optionally handle or redirect to an error view
            Return RedirectToAction("Error", "Home")
        End Try
    End Function

    Function Delete(id As Integer?) As ActionResult
        Try
            If id Is Nothing Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim filePath As String = Server.MapPath("~/App_Data/Category.csv")
            Dim categories = CsvHelper.ReadCsv(filePath, AddressOf CreateCategory)
            Dim category = categories.FirstOrDefault(Function(p) p.Id = id)
            If category Is Nothing Then
                Return HttpNotFound()
            End If

            Return View(category)
        Catch ex As Exception
            Logger.LogError(ex.Message)
            ' Optionally handle or redirect to an error view
            Return RedirectToAction("Error", "Home")
        End Try
    End Function

    <HttpPost()>
    <ActionName("Delete")>
    <ValidateAntiForgeryToken()>
    Function DeleteConfirmed(id As Integer) As ActionResult
        Try
            Dim filePath As String = Server.MapPath("~/App_Data/Category.csv")
            Dim categories = CsvHelper.ReadCsv(filePath, AddressOf CreateCategory)
            Dim categoryToRemove = categories.FirstOrDefault(Function(p) p.Id = id)
            If categoryToRemove IsNot Nothing Then
                categories.Remove(categoryToRemove)
                CsvHelper.WriteCsv(filePath, categories, AddressOf CategoryToCsv)
            End If
            Return RedirectToAction("Index")
        Catch ex As Exception
            Logger.LogError(ex.Message)
            ' Optionally handle or redirect to an error view
            Return RedirectToAction("Error", "Home")
        End Try
    End Function

    Private Function CreateCategory(values As String()) As Category
        Return New Category With {
            .Id = Integer.Parse(values(0)),
            .Name = values(1),
            .Description = values(2)
        }
    End Function

    Private Function CategoryToCsv(Category As Category) As String()
        Return New String() {Category.Id.ToString(), Category.Name, Category.Description}
    End Function
End Class
