Public Class Invoice

    Public Property CustomerName As String
        Public Property CustomerEmail As String
        Public Property CustomerAddress As String
        Public Property CartItems As List(Of CartItem)
        Public Property Subtotal As Decimal
        Public Property DiscountAmount As Decimal
        Public Property TaxAmount As Decimal
        Public Property TotalAmount As Decimal
        Public Property PaymentMethod As String

End Class
