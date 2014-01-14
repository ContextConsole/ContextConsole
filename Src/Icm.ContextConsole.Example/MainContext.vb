Imports Icm.ContextConsole

Public Class MainContext
    Inherits RootContext

    Public Sub Test()
        Dim tup2 = Interactor.AskOneByKey("Choose", {Tuple.Create(1, "a"), Tuple.Create(2, "b")}, Function(tup) tup.Item1.ToString)
        Interactor.ShowMessage("Chosen: " & tup2.Item2)
    End Sub

    Public Overrides Sub Credits()
        Interactor.ShowMessage("Icm.ContextConsole Example")
    End Sub
End Class
