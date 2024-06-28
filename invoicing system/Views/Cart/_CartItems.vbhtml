@ModelType List(Of CartItem)

<tbody>
    @For Each cartItem In Model
@<tr>
    <td>@cartItem.ProductName</td>
                <td>@cartItem.Quantity</td>
                            <td>@cartItem.UnitPrice.ToString("C")</td>
                                        <td>@(cartItem.Quantity * cartItem.UnitPrice).ToString("C")</td>
                    </tr>
                Next
</tbody>
