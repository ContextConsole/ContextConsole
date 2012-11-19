''' <summary>
''' A front controller is responsible of the general management of the application (not domain-specific):
''' show titles, initialization, ask for commands, command execution, and 4 basic commands:
''' <list>
''' <item>Show list of actions</item>
''' <item>Show list of controllers</item>
''' <item>Show help</item>
''' <item>Quit application</item>
''' </list>
''' </summary>
''' <remarks></remarks>
Public Interface IFrontController
    Inherits IController

    Sub ShowTitles()
    Function InitializeApplication() As Boolean
    Function ExecuteCommand() As Boolean

    Sub Actions()
    Sub Controllers()
    Sub Help()
    Sub Quit()
End Interface
