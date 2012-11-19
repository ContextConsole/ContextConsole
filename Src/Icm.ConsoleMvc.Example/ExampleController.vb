Imports Icm.ConsoleMvc

Public Class MathController
    Inherits BackController

    Public Sub New(ByVal ctlman As IControllerManager, ByVal asker As IInteractor)
        MyBase.New(ctlman, asker)
    End Sub

    Public Sub Add()
        Dim sumando1 = Interactor.AskInteger("First number")
        Dim sumando2 = Interactor.AskInteger("Second number")

        Interactor.ShowMessage("Result = " & sumando1 + sumando2)
    End Sub
End Class
