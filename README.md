## Introduction

Icm.ContextConsole is a framework for building applications in the style of [Netsh][1] tool. This tool exposes a series of top-level commands and organizes the rest of them in a hierarchical structure of contexts. This is also a typical organization for command-line interfaces embedded in hardware devices.

[1]:http://en.wikipedia.org/wiki/Netsh

Let's see an example for showing the capabilities of the framework. Imagine this context hierarchy:

* (root context): commands CMD_R1, CMD_R2
 * Context A: commands CMD_A1, CMD_A2
 * Context B: commands CMD_B1, CMD_B2
     * Context C: command CMD_C1

Our application is called APP.EXE. We can do the following things:

Directly execute any command:

    > APP.EXE CMD_R1
    Executing command CMD_R1
    > APP.EXE A CMD_A2
    Executing command CMD_A2
    > APP.EXE B C CMD_C1
    Executing command CMD_C1

Enter interactive mode and execute commands:

    > APP.EXE
    Welcome to APP's interactive mode!
    APP> CMD_R1
    Executing command CMD_R1
    APP> B
    Going to context B
    APP B> C CMD_C1
    Executing command CMD_C1
    APP B> C
    Going to context C
    APP B C> CMD_R1
    Executing CMD_R1
    APP B C> ..
    Going back to context B
    APP B> ..
    Going back to root context A
    APP> B C
    Going to context C
    APP B C> quit
    Bye!
    >

As you can see, at any moment you are able to execute the commands of the current context, all the ancestors and all the descendants; but you must always qualify the commands of the descendants.

## The beginning: ApplicationFactory

This class fires up the system by initializing with `Initialize`. This method configures the dependency injector with the standard services for the following interfaces:

- `IContextManager`: This service is used by the root context to instantiate contexts and actions.
- `IInteractor`: This service is used by the actions to interact with the user.
- `ITokenParser`: This service splits lines into tokens (taking into account quotes, quote escaping, etc.).
- `IContextTreeBuilder`: This service builds the hierarchy of context, in the form of a `ITreeNode(Of IContext)`.
- `IContext`: There can be many contexts, and they must be bound to the interface if we use the standard `IContextTreeBuilder`.

After initialization, the root context is instantiated and initialized.

Finally, it enters a loop in which the root context is asked to execute commands until one of them returns `false` upon execution.

What does an application do to execute commands?

The `StandardApplication` class provides a simple implementation that:

- Uses the interactor to ask a command line
- Uses the token parser to split the command line
- Gets the execution context by interpreting tokens as controller names (that must be each children of the former) until one token fails to be interpreted as such or we run out of tokens.
- If run out of tokens, change to the final context
- In other case, the next token must be an action name and the rest of tokens are enqueued for further use within the selected action
- Uses action finder to get the action instance.
- Executes the action (`IAction.Execute`)
- If the action is a "quit" action then returns true
- If the mode is not fully interactive (something has come from the environment command line) then returns true
- Else, returns false

Notable facts: nowhere it is used the `Console` class directly. An appropriate `IFrontInteractor` could be used to get or set values from a GUI, for example, or a network connection. In fact, the provided default interactors to not use `Console` either but streams. The `Console` streams are taken only when you use the default constructor.

## OK, so how do I start

You can do something as simple as creating a front controller inheriting from `FrontController`. Add some routines. In your `Main` method, create a `ConsoleMvcApplication` instance and call `Start()`. That's all!

If you want to classify all the actions of your ConsoleMvc app into controllers, just create more classes, each of them inheriting from `BackController` and put the actions into them.

If you want to customize the services, you can inherit from `ConsoleMvcApplication` and override `Initialize`. Get `Icm.Ninject.Kernel` and bind the services as you please. Just remember that all the aforementioned services must be bound (except for `IBackController`).