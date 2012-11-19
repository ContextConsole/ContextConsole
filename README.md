It is most easy to explain ConsoleMvc by starting at the beginning, which is ConsoleMvcApplication. This class fires up the system by initializing with Initialize. Initialize configures the dependency injector with the standard services for the following interfaces:

- IControllerManager: This service is used by the front controller to instantiate controllers and actions.
- IFrontInteractor: This service is used by the front controller to interact with the user.
- IInteractor: This service is used by the back controllers to interact with the user.
- ITokenParser: This service splits lines into tokens (taking into account quotes, quote escaping, etc.).
- IBackController: There can be many back controllers, and they must be bound to the interface if we use the standard IControllerManager.
After initialization, the front controller is instantiated and initialized.

Finally, it enters a loop in which the front controller is asked to execute commands until one of them returns False upon execution.

What does a front controller to execute commands?

The standard FrontController abstract class provides a simple implementation that:

- Uses the interactor to ask a command line
- Uses the token parser to split the command line
- Enqueues the tokens for further use within back controllers
- Gets first token to determine controller & action names
- Uses controller manager to get the action instance
- Executes the action
- If the action is a "quit" action then returns true
- If the mode is not fully interactive (something has come from the environment command line) then returns true
- Else, returns false

Notable facts: nowhere it is used the Console class directly. An appropriate IFrontInteractor could be used to get or set values from a GUI, for example, or a network connection. In fact, the provided default interactors to not use Console either but streams. The Console streams are taken only when you use the default constructor.

## OK, so how do I start

You can do something as simple as creating a front controller inheriting from FrontController. Add some routines. In the Main, create a ConsoleMvcApplication instance and call Start(). That's all!

If you want to classify all the actions of your ConsoleMvc app into controllers, just create more classes, each of them inheriting from BackController and put the actions into them.

If you want to customize the services, you can inherit from ConsoleMvcApplication and override Initialize. Get Icm.Ninject.Kernel and bind the services as you please. Just remember that all the aforementioned services must be bound (except for IBackController).