using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RingBellClock : MonoBehaviour
{
    [SerializeField] private TMP_InputField _hoursInputField;
    [SerializeField] private TMP_InputField _minutesInputField;

    [Space]
    [SerializeField] private Button _ringbellSetupButton;
    [SerializeField] private TextMeshProUGUI _ringbellButtonText;
    [SerializeField] private Image _ringBellIcon;

    [SerializeField] private Sprite _ringBellUnsetSprite;
    [SerializeField] private Sprite _ringBellSetSprite;

    [Space]
    [SerializeField] private AnimationClock _animationClock;
    [SerializeField] private DigitalClock _digitalClock;

    [Space]
    [SerializeField] private Animator _ringbellAnimator;
    private float _animationTime = 3f;

    private bool isRingSetup;


    private const string HOURS_KEY = "HOUR";
    private const string MINUTES_KEY = "MINUTES";
    private const string SECONDS_KEY = "SECONDS";
    private const string RINBELLSET_KEY = "RINGSET";

    private int _hours;
    private int _minutes;
    private int _seconds;
    private bool _ringBellSet;

    #region INIT
    private void Awake()
    {
        _ringbellSetupButton.onClick.AddListener(OnRingBellSetupButtonClick);

        _hoursInputField.onValueChanged.AddListener(OnHourDirectChange);
        _minutesInputField.onValueChanged.AddListener(OnMinutesDirectChange);

        ShowHideRingbell(false);
        ShowHideDirectInput(false);

        UpdateVisual();
    }

    private void Start()
    {
        LoadRingbellSetup();
    }
    #endregion

    #region CORE
    private void Update()
    {
        if (_ringBellSet && !isRingSetup) CheckRingTime();
    }

    private void CheckRingTime()
    {
        DateTime time = DateTime.Now;

        if (time.Hour == _hours && time.Minute == _minutes)
        {
            _ringBellSet = false;
            _hours = 0;
            _minutes = 0;
            _seconds = 0;
            SaveRingbellSetup();

            StartCoroutine(RingbellAnimation());
        }
    }

    IEnumerator RingbellAnimation()
    {
        ShowHideRingbell(true);

        yield return new WaitForSeconds(_animationTime);

        ShowHideRingbell(false);
    }


    #endregion

    #region UTILS

    private void ShowHideRingbell(bool value)
    {
        _ringbellAnimator.gameObject.SetActive(value);
    }

    private void ShowHideDirectInput(bool value)
    {
        _hoursInputField.gameObject.SetActive(value);
        _minutesInputField.gameObject.SetActive(value);
    }

    private void UpdateVisual()
    {
        _ringBellIcon.sprite = isRingSetup ? _ringBellSetSprite : _ringBellUnsetSprite;
    }

    private void UpdateAnimationClock()
    {
        _animationClock.SetupHour(_hours);
        _animationClock.SetupMinute(_minutes);
    }

    private void SaveRingbellSetup()
    {
        PlayerPrefs.SetInt(HOURS_KEY, _hours);
        PlayerPrefs.SetInt(MINUTES_KEY, _minutes);
        PlayerPrefs.SetInt(SECONDS_KEY, _seconds);
        PlayerPrefs.SetInt(RINBELLSET_KEY, _ringBellSet ? 1 : 0);
    }

    private void LoadRingbellSetup()
    {
        _hours = PlayerPrefs.GetInt(HOURS_KEY);
        _minutes = PlayerPrefs.GetInt(HOURS_KEY);
        _seconds = PlayerPrefs.GetInt(HOURS_KEY);
        _ringBellSet = PlayerPrefs.GetInt(RINBELLSET_KEY) == 1;
    }
    #endregion

    #region BUTTON
    private void OnRingBellSetupButtonClick()
    {
        isRingSetup = !isRingSetup;

        ShowHideDirectInput(isRingSetup);

        _ringbellButtonText.text = isRingSetup ? "Save" : "Ringbell setup";

        UpdateVisual();
        _animationClock.SetupMode(isRingSetup, OnClockSetupCallback);

        if (!isRingSetup)
        {
            //check if time was set correctly
            DateTime time = DateTime.Now;

            if (time.Hour != _hours || time.Minute != _minutes)
            {
                _ringBellSet = true;
                SaveRingbellSetup();
            }
            
        }
    }
    #endregion

    #region CALLBACK
    private void OnClockSetupCallback(int hours, int minutes, int seconds)
    {
        _hours = hours;
        _minutes = minutes;
        _seconds = seconds;

        _digitalClock.ShowTime(hours, minutes, seconds);     
    }

    private void OnHourDirectChange(string textValue)
    {
        _hours = int.Parse(textValue);

        _hours = Mathf.Clamp(_hours, 0, 23);

        _hoursInputField.text = _hours.ToString();

        OnClockSetupCallback(_hours, _minutes, 0);
        UpdateAnimationClock();
    }

    private void OnMinutesDirectChange(string textValue)
    {
        _minutes = int.Parse(textValue);

        _minutes = Mathf.Clamp(_minutes, 0, 59);

        _minutesInputField.text = _minutes.ToString();

        OnClockSetupCallback(_hours, _minutes, 0);
        UpdateAnimationClock();
    }

    #endregion


}
