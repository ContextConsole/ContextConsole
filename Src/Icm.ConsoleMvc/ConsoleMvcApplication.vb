Public MustInherit Class ConsoleMvcApplication

    Public Sub Main()
        Initialize()

        Dim mainctl = Icm.Ninject.Instance(Of IFrontController)()

        If mainctl.InitializeApplication Then
            Do Until mainctl.ExecuteCommand()
            Loop
        End If
    End Sub

    Protected Overridable Sub Initialize()
        ' By default, use reflection to get controllers and actions
        With Icm.Ninject.Kernel
            .Bind(Of IControllerManager).To(Of ReflectionControllerManager).InSingletonScope()
        End With

    End Sub

End Class
