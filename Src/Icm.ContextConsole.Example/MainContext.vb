Imports Icm.ContextConsole

Public Class MainContext
    Inherits RootContext

    Protected Overrides Sub Initialize()
        Interactor.ShowMessage("ConsoleMvc Example")
    End Sub

End Class
