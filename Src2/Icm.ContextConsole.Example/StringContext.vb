Imports Icm.ContextConsole

Public Class StringContext
    Inherits BaseContext

    Public Sub Concat()
        Dim num1 = Interactor.AskString("String 1")
        Dim num2 = Interactor.AskString("String 2")

        Interactor.ShowMessage(String.Format("{0} + {1} = {2}", num1, num2, num1 & num2))
    End Sub

End Class