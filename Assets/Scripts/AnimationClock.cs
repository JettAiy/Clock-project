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

    private float _hourCorrection;
    private float _minutesCorrection;
    private float _secondsCorrection;


    private float _timeToServerCheck = 3600f;
    private float _lastServerTimeCheck;
    #region INIT
    private void Start()
    {
        ServerTimeSync();
    }
    #endregion


    #region CORE

    private void Update()
    {
        if (isSetupMode)
            SetupClock();
        else
            AnimateClock();

        if (Time.time - _lastServerTimeCheck >= _timeToServerCheck)
        {
            ServerTimeSync();
        }
    }

    private void AnimateClock()
    {
        TimeSpan timeSpan = DateTime.Now.TimeOfDay;

        SetArrow(_hoursArrow,(float)(timeSpan.TotalHours + _hourCorrection) * -_hoursToDegree);
        SetArrow(_minutesArrow, (float)(timeSpan.TotalMinutes + _minutesCorrection) * -_minutesToDegree);
        SetArrow(_secondsArrow, (float)(timeSpan.TotalSeconds + _secondsCorrection) * -_secondsToDegree);
    }

    private void SetupClock()
    {
        int hour = (int)(WrapAngle(_hoursArrow.GetAngle()) /  _hoursToDegree);
        int minutes = (int)(WrapAngle(_minutesArrow.GetAngle()) / _minutesToDegree); 
        int seconds = (int)(WrapAngle(_secondsArrow.GetAngle()) / _secondsToDegree);

        _callbackAction?.Invoke(Mathf.Abs(hour), Mathf.Abs(minutes), Mathf.Abs(seconds));
    }
    #endregion

    #region SERVER TIME
    private void ServerTimeSync()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.Log("Unable to reach time servers!");
            return;
        }
        DateTime current = DateTime.Now;
        DateTime serverTime = ServerTime.GetNetworkTime(); 

        _hourCorrection = serverTime.Hour - current.Hour;
        _minutesCorrection = serverTime.Minute - current.Minute;
        _secondsCorrection = serverTime.Second - current.Second;

        _lastServerTimeCheck = Time.time;

        Debug.Log($"server time: {serverTime}. Correction H {_hourCorrection} M {_minutesCorrection} S {_secondsCorrection}");
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
