@Code
    ViewData("Title") = "Home Page"
    Dim loggedInCustomer As Customer = TryCast(Session("LoggedInCustomer"), Customer)
    Dim welcomeMessage As String = "Welcome, Guest"
    If loggedInCustomer IsNot Nothing Then
        welcomeMessage = "Welcome, " & loggedInCustomer.Name
    End If
End Code

<main>
    <section class="row" aria-labelledby="aspnetTitle">
        <h1 id="title">Welcome to Invoice System</h1>
      
    </section>

    
</main>
