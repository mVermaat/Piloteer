namespace Piloteer
{
    public interface IUserContextManager
    {
        /// <summary>
        /// Event that is raised when the user context changes to a different user. Allows dependent components to react.
        /// </summary>
        event UserContextManager.UserContextChangedEventHandler? OnUserContextChanged;
    }
}
