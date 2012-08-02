Public MustInherit Class BackController
    Inherits BaseController
    Implements IBackController

    Public Sub New(ctlman As IControllerManager, asker As IInteractor)
        MyBase.New(ctlman, asker)
    End Sub

End Class
