''' <summary>
''' A front interactor is an interactor that provide some more interactions required by FrontController.
''' </summary>
''' <remarks></remarks>
Public Interface IFrontInteractor
    Inherits IInteractor

    Function AskCommand(ByVal prompt As String) As String

    Sub ShowTitles(title As String)
    Function AskController(ByVal prompt As String, caller As IController) As IController

End Interface
