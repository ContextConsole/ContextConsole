Imports NUnit.Framework
Imports Icm.ContextConsole
Imports Moq
Imports Icm.IO


Public Class TestFrontContext
    Inherits RootContext

    Public Sub New(ByVal interactor As IInteractor)
        MyBase.New()
        Me.Interactor = interactor
        BaseInitialize(New StandardApplication(Me, interactor))
    End Sub

End Class
