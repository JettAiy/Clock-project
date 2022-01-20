using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class DigitalClock : MonoBehaviour
{

    private TextMeshProUGUI _digitalClickText;

    private void Awake()
    {
        _digitalClickText = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        TimeSpan timeSpan = DateTime.Now.TimeOfDay;

        _digitalClickText.text = timeSpan.ToString(@"hh\:mm\:ss");
    }

    public void ShowTime(int hours, int minutes, int seconds)
    {
        _digitalClickText.text = $"{hours}:{minutes}:{seconds}";
    }
}
