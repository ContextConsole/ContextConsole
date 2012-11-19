Public MustInherit Class ConsoleMvcApplication

    Public Sub Start()
        Initialize()

        Dim mainctl = Icm.Ninject.Instance(Of IFrontController)()

        If mainctl.InitializeApplication Then
            Do Until mainctl.ExecuteCommand()
            Loop
        End If
    End Sub

    Protected Overridable Sub Initialize(Optional loadAllBackControllers As Boolean = True)
        ' By default, use reflection to get controllers and actions
        With Icm.Ninject.Kernel
            .Bind(Of IControllerManager).To(Of ReflectionControllerManager).InSingletonScope()
            .Bind(Of IFrontInteractor).To(Of StreamsFrontInteractor).InSingletonScope()
            .Bind(Of IInteractor).To(Of StreamsInteractor).InSingletonScope()
            If loadAllBackControllers Then
                Dim backControllerTypes = Icm.Reflection.GetAllImplementors(Of IBackController)()

                For Each backControllerType In backControllerTypes
                    .Bind(Of IBackController).To(backControllerType)
                Next

            End If
        End With

    End Sub

End Class

Public Class ConsoleMvcApplication(Of T As IFrontController)
    Inherits ConsoleMvcApplication

    Public Sub New()
        With Icm.Ninject.Kernel
            .Bind(Of IFrontController).To(Of T).InSingletonScope()
        End With
    End Sub
End Class