public interface IUserInteractionState
{
    void OnEnter(UserInteractionHandler handler);
    void OnUpdate(UserInteractionHandler handler);
    void OnExit(UserInteractionHandler handler);
}
