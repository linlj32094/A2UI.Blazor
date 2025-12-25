using A2UI.Core.Messages;

namespace A2UI.Core.Processing;

/// <summary>
/// Handles dispatching of user actions and errors to the server.
/// </summary>
public class EventDispatcher
{
    /// <summary>
    /// Event raised when a user action occurs.
    /// </summary>
    public event EventHandler<UserActionEventArgs>? UserActionDispatched;

    /// <summary>
    /// Event raised when an error occurs.
    /// </summary>
    public event EventHandler<ErrorEventArgs>? ErrorDispatched;

    /// <summary>
    /// Dispatches a user action.
    /// </summary>
    public void DispatchUserAction(UserActionMessage action)
    {
        UserActionDispatched?.Invoke(this, new UserActionEventArgs(action));
    }

    /// <summary>
    /// Dispatches an error.
    /// </summary>
    public void DispatchError(Dictionary<string, object> error)
    {
        ErrorDispatched?.Invoke(this, new ErrorEventArgs(error));
    }

    /// <summary>
    /// Creates a user action message.
    /// </summary>
    public static UserActionMessage CreateUserAction(
        string actionName,
        string surfaceId,
        string sourceComponentId,
        Dictionary<string, object> context)
    {
        return new UserActionMessage
        {
            Name = actionName,
            SurfaceId = surfaceId,
            SourceComponentId = sourceComponentId,
            Timestamp = DateTime.UtcNow,
            Context = context
        };
    }

    /// <summary>
    /// Creates a client-to-server message with a user action.
    /// </summary>
    public static ClientToServerMessage CreateUserActionMessage(UserActionMessage action)
    {
        return new ClientToServerMessage
        {
            UserAction = action
        };
    }

    /// <summary>
    /// Creates a client-to-server message with an error.
    /// </summary>
    public static ClientToServerMessage CreateErrorMessage(Dictionary<string, object> error)
    {
        return new ClientToServerMessage
        {
            Error = error
        };
    }
}

/// <summary>
/// Event args for user actions.
/// </summary>
public class UserActionEventArgs : EventArgs
{
    public UserActionMessage Action { get; }

    public UserActionEventArgs(UserActionMessage action)
    {
        Action = action;
    }
}

/// <summary>
/// Event args for errors.
/// </summary>
public class ErrorEventArgs : EventArgs
{
    public Dictionary<string, object> Error { get; }

    public ErrorEventArgs(Dictionary<string, object> error)
    {
        Error = error;
    }
}

