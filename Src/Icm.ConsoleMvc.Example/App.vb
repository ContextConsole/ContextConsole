Imports Icm.ConsoleMvc


Public Module App

    Public Sub Main()
        Dim app As New ConsoleMvcApplication(Of MainController)

        app.Start()
    End Sub

End Module
