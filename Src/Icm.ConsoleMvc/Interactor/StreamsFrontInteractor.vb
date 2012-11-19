
Public Class StreamsFrontInteractor
    Inherits StreamsInteractor
    Implements IFrontInteractor

    Private ReadOnly controllers_ As IEnumerable(Of IController)

    Public Sub New(controllers As IEnumerable(Of IController))
        controllers_ = controllers
    End Sub

    Public Function AskController(ByVal prompt As String, caller As IController) As IController Implements IFrontInteractor.AskController
        Dim controllerName = AskString(prompt)

        Dim ctl As IController

        If String.IsNullOrWhiteSpace(controllerName) Then
            Return caller
        Else
            ctl = controllers_.SingleOrDefault(Function(ctrl) _
                                                   ctrl.Name = controllerName OrElse _
                                                   ctrl.Synonyms().Contains(controllerName))
        End If

        If ctl Is Nothing Then
            ErrorWriter.WriteLine("Could not find controller {0}", controllerName)
            ErrorWriter.WriteLine()
            Return Nothing
        Else
            Return ctl
        End If
    End Function


    Public Sub ShowTitles(title As String) Implements IFrontInteractor.ShowTitles
        Writer.WriteLine(title)
        Writer.WriteLine(New String("-"c, title.Length))
    End Sub

    Private _promptSeparator As Char = ":"c

    Public Overrides Function PromptSeparator() As Char
        Return _promptSeparator
    End Function

    Public Function AskCommand(prompt As String) As String Implements IFrontInteractor.AskCommand
        _promptSeparator = ">"c
        AskCommand = AskString(prompt)
        _promptSeparator = ":"c
    End Function
End Class