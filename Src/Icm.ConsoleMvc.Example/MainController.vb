Imports Icm.ConsoleMvc

Class MainController
    Inherits FrontController

    Public Sub New(ByVal ctlman As IControllerManager, ByVal asker As IFrontInteractor, ByVal tokenParser As ITokenParser)
        MyBase.New(ctlman, asker, tokenParser)
    End Sub

    Protected Overrides Sub ShowTitles()
        FrontInteractor.ShowTitles("ConsoleMvc Example")
        FrontInteractor.ShowMessage("You have entered interactive mode. Write 'help' to see a full list of commands and 'quit' for exit the application")
    End Sub

    Public Sub Multiply()
        Dim n1 = Interactor.AskInteger("First number")
        Dim n2 = Interactor.AskInteger("Second number")

        Interactor.ShowMessage("Result = " & n1 * n2)
    End Sub
End Class