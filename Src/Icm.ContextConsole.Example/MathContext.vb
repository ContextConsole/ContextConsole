Imports Icm.ContextConsole

Public Class MathContext
    Inherits BaseContext

    Public Sub Add()
        Dim num1 = Interactor.AskInteger("Number 1")
        Dim num2 = Interactor.AskInteger("Number 2")

        Interactor.ShowMessage(String.Format("{0} + {1} = {2}", num1, num2, num1 + num2))
    End Sub

    Public Sub Multiply()
        Dim num1 = Interactor.AskInteger("Number 1")
        Dim num2 = Interactor.AskInteger("Number 2")

        Interactor.ShowMessage(String.Format("{0} * {1} = {2}", num1, num2, num1 * num2))
    End Sub

End Class
