Imports System.IO
Imports System.Linq
Imports System.Web.Mvc

Public Class CartController
    Inherits Controller

    ' GET: Cart
    Function Index() As ActionResult
        Try
            Dim viewModel As New CartViewModel()

            Dim productsFilePath As String = Server.MapPath("~/App_Data/Products.csv")
            viewModel.Products = CsvHelper.ReadCsv(productsFilePath, AddressOf CreateProduct)

            Dim discountsFilePath As String = Server.MapPath("~/App_Data/Discounts.csv")
            viewModel.Discounts = LoadDiscounts(discountsFilePath)

            Dim taxesFilePath As String = Server.MapPath("~/App_Data/Taxes.csv")
            viewModel.Taxes = LoadTaxes(taxesFilePath)

            viewModel.PaymentMethods = New List(Of SelectListItem) From {
                New SelectListItem With {.Value = "1", .Text = "Credit Card"},
                New SelectListItem With {.Value = "2", .Text = "Debit Card"},
                New SelectListItem With {.Value = "3", .Text = "PayPal"},
                New SelectListItem With {.Value = "4", .Text = "Cash"}
            }

            viewModel.CartItems = GetCartItems()
            Return View(viewModel)
        Catch ex As Exception
            Logger.LogError(ex.Message)
            ' Optionally handle or redirect to an error view
            Return RedirectToAction("Error", "Home")
        End Try
    End Function

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

    ' POST: Add product to cart
    <HttpPost>
    Function AddToCart(productId As Integer, quantity As Integer) As ActionResult
        Try
            Dim product As Product = FindProduct(productId)
            If product IsNot Nothing Then
                Dim cartItem As New CartItem With {
                    .ProductId = productId,
                    .ProductName = product.Name,
                    .Quantity = quantity,
                    .UnitPrice = product.Price
                }
                AddOrUpdateCartItem(cartItem)
            End If
            Return RedirectToAction("Index")
        Catch ex As Exception
            Logger.LogError(ex.Message)
            ' Optionally handle or redirect to an error view
            Return RedirectToAction("Error", "Home")
        End Try
    End Function

    Function RemoveFromCart(productId As Integer) As ActionResult
        Try
            Dim cartItems As List(Of CartItem)

            If Session("CartItems") IsNot Nothing Then
                cartItems = CType(Session("CartItems"), List(Of CartItem))
                Dim itemToRemove = cartItems.FirstOrDefault(Function(item) item.ProductId = productId)
                If itemToRemove IsNot Nothing Then
                    cartItems.Remove(itemToRemove)
                End If
                Session("CartItems") = cartItems
            End If

            Return RedirectToAction("Index")
        Catch ex As Exception
            Logger.LogError(ex.Message)
            ' Optionally handle or redirect to an error view
            Return RedirectToAction("Error", "Home")
        End Try
    End Function

    Private Sub AddOrUpdateCartItem(cartItem As CartItem)
        Dim cartItems As List(Of CartItem)

        If Session("CartItems") IsNot Nothing Then
            cartItems = CType(Session("CartItems"), List(Of CartItem))
        Else
            cartItems = New List(Of CartItem)()
        End If

        Dim existingItem = cartItems.FirstOrDefault(Function(item) item.ProductId = cartItem.ProductId)

        If existingItem IsNot Nothing Then
            existingItem.Quantity += cartItem.Quantity
        Else
            cartItems.Add(cartItem)
        End If

        Session("CartItems") = cartItems
    End Sub

    Private Function GetCartItems() As List(Of CartItem)
        Dim cartItems As List(Of CartItem) = New List(Of CartItem)()
        If Session("CartItems") IsNot Nothing Then
            cartItems = CType(Session("CartItems"), List(Of CartItem))
        End If
        Return cartItems
    End Function

    ' POST: Generate invoice
    <HttpPost>
    Function GenerateInvoice(discountId As Integer, taxId As Integer, paymentMethodId As Integer) As ActionResult
        Try
            If discountId = 0 Then
                ModelState.AddModelError("discountId", "Discount is required.")
            End If

            If taxId = 0 Then
                ModelState.AddModelError("taxId", "Tax is required.")
            End If

            If paymentMethodId = 0 Then
                ModelState.AddModelError("paymentMethodId", "Payment Method is required.")
            End If

            Dim cartItems As List(Of CartItem) = GetCartItems()

            Dim subtotal As Decimal = cartItems.Sum(Function(item) item.Quantity * item.UnitPrice)
            Dim discount As Decimal = CalculateDiscount(discountId, subtotal)
            Dim tax As Decimal = CalculateTax(taxId, subtotal - discount)
            Dim totalAmount As Decimal = (subtotal - discount) + tax

            Dim invoice As New Invoice With {
                .CartItems = cartItems,
                .Subtotal = subtotal,
                .DiscountAmount = discount,
                .TaxAmount = tax,
                .TotalAmount = totalAmount,
                .PaymentMethod = GetPaymentMethodName(paymentMethodId)
            }

            Return View("Invoice", invoice)
        Catch ex As Exception
            Logger.LogError(ex.Message)
            ' Optionally handle or redirect to an error view
            Return RedirectToAction("Error", "Home")
        End Try
    End Function

    Private Function FindProduct(productId As Integer) As Product
        Dim productsFilePath As String = Server.MapPath("~/App_Data/Products.csv")
        Dim products As List(Of Product) = CsvHelper.ReadCsv(productsFilePath, AddressOf CreateProduct)
        Return products.FirstOrDefault(Function(p) p.Id = productId)
    End Function

    Private Function LoadDiscounts(filePath As String) As List(Of SelectListItem)
        Dim discounts As New List(Of SelectListItem)()
        Using reader As New StreamReader(filePath)
            While Not reader.EndOfStream
                Dim line As String = reader.ReadLine()
                Dim parts As String() = line.Split(","c)
                If parts.Length = 3 Then
                    discounts.Add(New SelectListItem() With {
                        .Value = parts(2),
                        .Text = parts(1)
                    })
                End If
            End While
        End Using
        Return discounts
    End Function

    Private Function LoadTaxes(filePath As String) As List(Of SelectListItem)
        Dim taxes As New List(Of SelectListItem)()
        Using reader As New StreamReader(filePath)
            While Not reader.EndOfStream
                Dim line As String = reader.ReadLine()
                Dim parts As String() = line.Split(","c)
                If parts.Length = 3 Then
                    taxes.Add(New SelectListItem() With {
                        .Value = parts(2),
                        .Text = parts(1)
                    })
                End If
            End While
        End Using
        Return taxes
    End Function

    Private Function CalculateDiscount(discountId As Integer, subtotal As Decimal) As Decimal
        Dim discountsFilePath As String = Server.MapPath("~/App_Data/Discounts.csv")
        Dim discounts As List(Of SelectListItem) = LoadDiscounts(discountsFilePath)
        Dim selectedDiscount = discounts.FirstOrDefault(Function(d) Convert.ToInt32(d.Value) = discountId)
        If selectedDiscount IsNot Nothing Then
            Dim discountPercentage As Decimal = Decimal.Parse(selectedDiscount.Value)
            Return subtotal * (discountPercentage / 100)
        End If
        Return 0
    End Function

    Private Function CalculateTax(taxId As Integer, subtotalAfterDiscount As Decimal) As Decimal
        Dim taxesFilePath As String = Server.MapPath("~/App_Data/Taxes.csv")
        Dim taxes As List(Of SelectListItem) = LoadTaxes(taxesFilePath)
        Dim selectedTax = taxes.FirstOrDefault(Function(t) Convert.ToInt32(t.Value) = taxId)
        If selectedTax IsNot Nothing Then
            Dim taxPercentage As Decimal = Decimal.Parse(selectedTax.Value)
            Return subtotalAfterDiscount * (taxPercentage / 100)
        End If
        Return 0
    End Function

    Private Function GetPaymentMethodName(paymentMethodId As Integer) As String
        Select Case paymentMethodId
            Case 1
                Return "Credit Card"
            Case 2
                Return "Debit Card"
            Case 3
                Return "PayPal"
            Case 4
                Return "Cash"
            Case Else
                Return "Unknown"
        End Select
    End Function
End Class
