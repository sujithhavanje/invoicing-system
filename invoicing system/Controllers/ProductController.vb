Imports System.Web.Mvc
Imports System.IO
Imports System.Net

Public Class ProductController
    Inherits Controller

    ' GET: Product
    Function Index() As ActionResult
        Try
            If Session("LoggedInCustomer") IsNot Nothing Then
                Dim productsFilePath As String = Server.MapPath("~/App_Data/Products.csv")
                Dim products = CsvHelper.ReadCsv(productsFilePath, AddressOf CreateProduct)

                Dim categoriesFilePath As String = Server.MapPath("~/App_Data/Category.csv")
                Dim categories = CsvHelper.ReadCsv(categoriesFilePath, AddressOf CreateCategory)

                ' Join products with categories to get category names
                Dim productsWithCategoryNames = From product In products
                                                Join category In categories
                                                On product.CategoryId Equals category.Id
                                                Select New Product With {
                                                    .Id = product.Id,
                                                    .Name = product.Name,
                                                    .Description = product.Description,
                                                    .Price = product.Price,
                                                    .Quantity = product.Quantity,
                                                    .CategoryName = category.Name
                                                }

                Return View(productsWithCategoryNames)
            Else
                Return RedirectToAction("LoginRegister", "Account")
            End If
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

    ' GET: Product/Create
    Function Create() As ActionResult
        If Session("LoggedInCustomer") IsNot Nothing Then
            Return View()
        Else
            Return RedirectToAction("LoginRegister", "Account")
        End If
    End Function

    ' POST: Product/Create
    <HttpPost()>
    <ValidateAntiForgeryToken()>
    Function Create(product As Product) As ActionResult
        Try
            If ModelState.IsValid Then
                Dim filePath As String = Server.MapPath("~/App_Data/Products.csv")
                Dim products = CsvHelper.ReadCsv(filePath, AddressOf CreateProduct)
                product.Id = If(products.Count > 0, products.Max(Function(p) p.Id) + 1, 1)
                products.Add(product)
                CsvHelper.WriteCsv(filePath, products, AddressOf ProductToCsv)
                Return RedirectToAction("Index")
            End If
            Return View(product)
        Catch ex As Exception
            Logger.LogError(ex.Message)
            ' Optionally handle or redirect to an error view
            Return RedirectToAction("Error", "Home")
        End Try
    End Function

    ' Helper method for CSV
    Private Function CreateProduct(values As String()) As Product
        Return New Product With {
            .Id = Integer.Parse(values(0)),
            .Name = values(1),
            .Description = values(2),
            .Price = Decimal.Parse(values(3)),
            .Quantity = Integer.Parse(values(4)),
            .CategoryId = Integer.Parse(values(5))
        }
    End Function

    ' GET: Product/Edit/5
    Function Edit(id As Integer?) As ActionResult
        Try
            If id Is Nothing Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim filePath As String = Server.MapPath("~/App_Data/Products.csv")
            Dim products = CsvHelper.ReadCsv(filePath, AddressOf CreateProduct)
            Dim product = products.FirstOrDefault(Function(p) p.Id = id)
            If product Is Nothing Then
                Return HttpNotFound()
            End If

            Return View(product)
        Catch ex As Exception
            Logger.LogError(ex.Message)
            ' Optionally handle or redirect to an error view
            Return RedirectToAction("Error", "Home")
        End Try
    End Function

    ' POST: Product/Edit/5
    <HttpPost()>
    <ValidateAntiForgeryToken()>
    Function Edit(product As Product) As ActionResult
        Try
            If ModelState.IsValid Then
                Dim filePath As String = Server.MapPath("~/App_Data/Products.csv")
                Dim products = CsvHelper.ReadCsv(filePath, AddressOf CreateProduct)
                Dim existingProduct = products.FirstOrDefault(Function(p) p.Id = product.Id)
                If existingProduct IsNot Nothing Then
                    existingProduct.Name = product.Name
                    existingProduct.Description = product.Description
                    existingProduct.Price = product.Price
                    existingProduct.Quantity = product.Quantity
                    existingProduct.CategoryId = product.CategoryId
                    CsvHelper.WriteCsv(filePath, products, AddressOf ProductToCsv)
                End If
                Return RedirectToAction("Index")
            End If
            Return View(product)
        Catch ex As Exception
            Logger.LogError(ex.Message)
            ' Optionally handle or redirect to an error view
            Return RedirectToAction("Error", "Home")
        End Try
    End Function

    ' GET: Product/Delete/5
    Function Delete(id As Integer?) As ActionResult
        Try
            If id Is Nothing Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim filePath As String = Server.MapPath("~/App_Data/Products.csv")
            Dim products = CsvHelper.ReadCsv(filePath, AddressOf CreateProduct)
            Dim product = products.FirstOrDefault(Function(p) p.Id = id)
            If product Is Nothing Then
                Return HttpNotFound()
            End If

            Return View(product)
        Catch ex As Exception
            Logger.LogError(ex.Message)
            ' Optionally handle or redirect to an error view
            Return RedirectToAction("Error", "Home")
        End Try
    End Function

    ' POST: Product/Delete/5
    <HttpPost()>
    <ActionName("Delete")>
    <ValidateAntiForgeryToken()>
    Function DeleteConfirmed(id As Integer) As ActionResult
        Try
            Dim filePath As String = Server.MapPath("~/App_Data/Products.csv")
            Dim products = CsvHelper.ReadCsv(filePath, AddressOf CreateProduct)
            Dim productToRemove = products.FirstOrDefault(Function(p) p.Id = id)
            If productToRemove IsNot Nothing Then
                products.Remove(productToRemove)
                CsvHelper.WriteCsv(filePath, products, AddressOf ProductToCsv)
            End If
            Return RedirectToAction("Index")
        Catch ex As Exception
            Logger.LogError(ex.Message)
            ' Optionally handle or redirect to an error view
            Return RedirectToAction("Error", "Home")
        End Try
    End Function

    Private Function ProductToCsv(product As Product) As String()
        Return New String() {product.Id.ToString(), product.Name, product.Description, product.Price.ToString(), product.Quantity.ToString(), product.CategoryId.ToString()}
    End Function
End Class
