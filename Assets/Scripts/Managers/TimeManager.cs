using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    enum ETImeSpeed
    {
        NormalSpeed = 0,
        HalfSpeed,
        DoubleSpeed,
        CovidSpeed
    }
    public static TimeManager Instance { get; private set; }

    public event Action OnDayChanged;
    public event Action OnWeekChanged;
    public event Action OnMonthChanged;
    public event Action OnYearChanged;

    int CurrentDay = 1;
    int CurrentWeek = 1;
    int CurrentMonth = 1;
    int CurrentYear = 1;

    int timeSpeed;
    public float timeMultiplier { get; private set; }

    ETImeSpeed m_TimeSpeed;

    [SerializeField]
    public int timePerDay;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        timeMultiplier = 1.0f;
        m_TimeSpeed = ETImeSpeed.NormalSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.GetGameState() == GameManager.GameState.Paused) return;

        timeSpeed++;

        if (timeSpeed >= timePerDay / timeMultiplier)
        {
            UpdateDay();
        }

        if(CurrentDay % 8 == 0 && 8 * CurrentWeek == CurrentDay)
        {
            UpdateWeek();
        }

        if (CurrentDay >= 32)
        {
            UpdateMonth();
        }
        if (CurrentMonth >= 13)
        {
            UpdateYear();
        }

    }

    private void UpdateDay()
    {
        CurrentDay++;
        timeSpeed = 0;

        if (OnDayChanged != null)
        {
            OnDayChanged();
        }
    }

    private void UpdateWeek()
    {

        CurrentWeek++;
        if(CurrentWeek >= 5)
        {
            CurrentWeek = 1;
        }

        if(OnWeekChanged != null)
        {
            OnWeekChanged();
        }
        Debug.Log(CurrentWeek);
    }

    private void UpdateMonth()
    {
        CurrentMonth++;
        CurrentDay = 1;
        CurrentWeek = 1;

        if (OnMonthChanged != null)
        {
            OnMonthChanged();
        }
    }

    private void UpdateYear()
    {
        CurrentYear++;
        CurrentMonth = 1;

        if (OnYearChanged != null)
        {
            OnYearChanged();
        }
    }

    public void Set1XSpeed()
    {
        timeMultiplier = 1;
        m_TimeSpeed = ETImeSpeed.NormalSpeed;
    }
    public void Set2XSpeed()
    {
        timeMultiplier = 2;
        m_TimeSpeed = ETImeSpeed.DoubleSpeed;
    }
    public void SetHalfSpeed()
    {
        timeMultiplier = 0.5f;
        m_TimeSpeed = ETImeSpeed.HalfSpeed;
    }


    public void CovidSpeed()
    {
        timeMultiplier = 20;
        m_TimeSpeed = ETImeSpeed.CovidSpeed;
    }

    public int GetDay() { return CurrentDay; }
    public int GetMonth() { return CurrentMonth; }
    public int GetYear() { return CurrentYear; }
}
