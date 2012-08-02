''' <summary>
''' A front interactor is an interactor that provide some more interactions required by FrontController.
''' </summary>
''' <remarks></remarks>
Public Interface IFrontInteractor
    Inherits IInteractor

    Sub ShowTitles()
    Function AskController(ByVal prompt As String, caller As IController) As IController

End Interface
