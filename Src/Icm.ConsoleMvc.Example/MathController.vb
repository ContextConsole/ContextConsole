Imports Icm.ConsoleMvc

Public Class MathController
    Inherits BackController

    Public Sub New(ByVal ctlman As IControllerManager, ByVal asker As IInteractor)
        MyBase.New(ctlman, asker)
    End Sub

    Public Sub Add()
        Dim n1 = Interactor.AskInteger("First number")
        Dim n2 = Interactor.AskInteger("Second number")

        Interactor.ShowMessage("Result = " & n1 + n2)
    End Sub
End Class
