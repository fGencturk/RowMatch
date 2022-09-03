namespace Common.InputSystem.Handlers
{
    public interface IClickable
    {
        void OnTouchDown(EventData eventData);

        void OnTouchUp(EventData eventData);
    }
}