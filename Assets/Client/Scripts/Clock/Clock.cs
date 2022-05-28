using System;
using CustomTimer;
using Tools;
using UnityEngine;

public class Clock : MonoBehaviour, IInteractable
{
    [SerializeField] private ClockHands _clockHands;
    [SerializeField] private TextMesh _textSpace;
    [SerializeField] private AlarmClock _alarmClock;
    [SerializeField] private RaycastHitCheck _hitCheck;

    public event Action UiUpdateEvent;
    public event Action<int,int> MinutesLeftEvent;
    public bool PanelIsActive { get; private set; } = false;

    private DateTime time;


    private int hourse;
    private int minutes;
    private int seconds;

    public int Seconds
    {
        get => seconds;
        set
        {
            seconds = value;
            _clockHands.SecondHandRotation(Seconds);
            UpdateDigitTime();
        }
    }
    public int Minutes
    {
        get => minutes;
        set
        {
            minutes = value;
            _clockHands.MinuteHandRotation(Hourse, Minutes);
            MinutesLeftEvent?.Invoke(Hourse, Minutes);
        }
    }
    public int Hourse
    {
        get => hourse;
        set
        {
            hourse = value;
            UpdateDigitTime();
        }
    }

    public bool CanInterract { get; set; } = true;

    public void Start()
    {
        Initialize();
    }
    public void Update()
    {       
            if (_hitCheck.CurObj == null && _hitCheck.uiObjectsCount <= 0)
                _alarmClock.HidePanel();
    }
    private void CourseOfTime()
    {
        Seconds++;
        if (Seconds >= 60)
        {
            Seconds = 0;
            Minutes++;
        }
        if (Minutes >= 60)
            Minutes = 0;
    }
    private void CheckNetworkTime()
    {
        time = TimeChecker.TimeCheck();
        Hourse = time.Hour;
        Minutes = time.Minute;
        Seconds = time.Second;

        _clockHands.UpdateHands(Hourse, Minutes, Seconds);
    }
    private void UpdateDigitTime()
    {
        _textSpace.text = $"{Hourse}:{Minutes}:{Seconds}";
    }
    private void Initialize()
    {
        CheckNetworkTime();
        Timer.instance.OnSecondLeftEvent += CourseOfTime;
        Timer.instance.OnHourLeftEvent += CheckNetworkTime;
        MinutesLeftEvent += _alarmClock.InspectAlarmClockTime;
        _clockHands.SetAlarmClockEvent += _alarmClock.SetAlarmClock;
    }

    public void Interract()
    {
        if(CanInterract)
            _alarmClock.ShowPanel();   
    }
}
