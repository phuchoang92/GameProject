namespace Game.Control
{
    public interface IRaycastable
    {
        CursorType GetCursorType();
        bool HandleRaycast(PlayerController callingController);
    }
}