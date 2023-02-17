using UnityEngine;
using UnityEngine.EventSystems;

public class MobileJoystick : MonoBehaviour, IPointerUpHandler, IDragHandler, IPointerDownHandler
{
    [SerializeField] private float _dragThreshold = 0.2f;
    [SerializeField] private int _dragMovementDistance = 30;
    [SerializeField] private int _dragOffsetDistance = 30;

    [SerializeField] private RectTransform _joystickTransform;

    private Canvas _canvas;
    private Camera _cam;

    public Vector2 InputVector { get; private set; }

    private void Awake()
    {
        InputVector = Vector2.zero;
        _canvas = GetComponentInParent<Canvas>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_canvas.renderMode == RenderMode.ScreenSpaceCamera)
        {
            _cam = _canvas.worldCamera;
        }

        RectTransformUtility.ScreenPointToLocalPointInRectangle(_joystickTransform, eventData.position, _cam, out Vector2 offset);
        offset = Vector2.ClampMagnitude(offset, _dragOffsetDistance) / _dragOffsetDistance;
        offset.y = 0;

        _joystickTransform.anchoredPosition = offset * _dragMovementDistance;
        InputVector = CalculateMovementInput(offset);
    }

    private Vector2 CalculateMovementInput(Vector2 offset)
    {
        float x = Mathf.Abs(offset.x) > _dragThreshold ? offset.x : 0;
        return new Vector2(x, 0);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _joystickTransform.anchoredPosition = Vector2.zero;
        InputVector = Vector2.zero;
    }
}
