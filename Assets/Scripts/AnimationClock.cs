using System;
using UnityEngine;

public class AnimationClock : MonoBehaviour
{

    private const float _hoursToDegree = 360f / 12f;
    private const float _minutesToDegree = 360f / 60f;
    private const float _secondsToDegree = 360f / 60f;

    [SerializeField] private ArrowClock _hoursArrow;
    [SerializeField] private ArrowClock _minutesArrow;
    [SerializeField] private ArrowClock _secondsArrow;

    private bool isSetupMode;

    private Action<int, int, int> _callbackAction;

    #region CORE

    private void Update()
    {
        if (isSetupMode)
            SetupClock();
        else
            AnimateClock();   
    }

    private void AnimateClock()
    {
        TimeSpan timeSpan = DateTime.Now.TimeOfDay;

        SetArrow(_hoursArrow,(float)timeSpan.TotalHours * -_hoursToDegree);
        SetArrow(_minutesArrow, (float)timeSpan.TotalMinutes * -_minutesToDegree);
        SetArrow(_secondsArrow, (float)timeSpan.TotalSeconds * -_secondsToDegree);
    }

    private void SetupClock()
    {
        int hour = (int)(WrapAngle(_hoursArrow.GetAngle()) /  _hoursToDegree);
        int minutes = (int)(WrapAngle(_minutesArrow.GetAngle()) / _minutesToDegree); 
        int seconds = (int)(WrapAngle(_secondsArrow.GetAngle()) / _secondsToDegree);

        _callbackAction?.Invoke(Mathf.Abs(hour), Mathf.Abs(minutes), Mathf.Abs(seconds));
    }
    #endregion

    #region UTIL
    private void SetArrow(ArrowClock arrowClock, float angle)
    {
        arrowClock.SetupAngle(angle);
    }

    public void SetupHour(int hour)
    {
        SetArrow(_hoursArrow, hour * -_hoursToDegree);
    }

    public void SetupMinute(int minute)
    {
        SetArrow(_minutesArrow, minute * -_minutesToDegree);
    }

    private static float WrapAngle(float angle)
    {
        if (angle > 360)
            return angle - 360;

        return 360 - angle;
    }

    public void SetupMode(bool value, Action<int, int, int> callbackAction)
    {
        isSetupMode = value;
        _hoursArrow.SetupMode(value);
        _minutesArrow.SetupMode(value);
        _secondsArrow.SetupMode(value);

        _callbackAction = callbackAction;
    }
    #endregion

}
