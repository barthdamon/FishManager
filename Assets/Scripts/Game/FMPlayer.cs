using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMPlayer : MonoBehaviourSingleton<FMPlayer>
{
    public delegate void CapitalIncreaseEvent(float value);
    public event CapitalIncreaseEvent OnCapitalIncrease;
    
    public float m_Capital
    {
        get;
        private set;
    }

    public void IncrementCapital(float value)
    {
        m_Capital += value;
        OnCapitalIncrease?.Invoke(value);
    }

    private void Awake()
    {
        m_Capital = 100f;
    }
}
