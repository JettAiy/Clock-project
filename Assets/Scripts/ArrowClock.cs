using UnityEngine;
using UnityEngine.EventSystems;

public class ArrowClock : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    public static ArrowClock selected;

    private bool isSetupMode;

    private bool isHolding;

    #region CORE
    public void SetupMode(bool value)
    {
        isHolding = false;
        isSetupMode = value;
    }

    private void Update()
    {
        if (!isSetupMode) return;

        if (isHolding) LookTowardsMousePosition();
    }

    private void LookTowardsMousePosition()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 direction = mousePosition - transform.position;
        float angle = Vector2.SignedAngle(Vector2.right, direction) -90f;
        transform.eulerAngles = new Vector3(0, 0, angle);
    }

    #endregion

    #region UTILS
    public void SetupAngle(float angle)
    {
        transform.localRotation = Quaternion.Euler(0f, 0f, angle);
    }

    public float GetAngle()
    {
        return transform.localEulerAngles.z;
    }
    #endregion

    #region INTERFACE
    public void OnPointerDown(PointerEventData eventData)
    {
        isHolding = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isHolding = false;
    }
    #endregion

}
