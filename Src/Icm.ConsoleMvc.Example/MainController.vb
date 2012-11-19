Imports Icm.ConsoleMvc

Class MainController
    Inherits FrontController

    Public Sub New(ByVal ctlman As IControllerManager, ByVal asker As IFrontInteractor)
        MyBase.New(ctlman, asker)

    End Sub
    Public Sub New(ByVal ctlman As IControllerManager, ByVal asker As IFrontInteractor, ByVal tokenParser As ITokenParser)
        MyBase.New(ctlman, asker, tokenParser)
        
    End Sub

    Protected Overrides Sub ShowTitles()
        FrontInteractor.ShowTitles("ConsoleMvc Example")
        FrontInteractor.ShowMessage("You have entered interactive mode. Write 'help' to see a full list of commands and 'quit' for exit the application")
    End Sub

End Class