using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldLightingManager : MonoBehaviour
{
    Transform m_Transform;

    TimeManager m_TimeManager;

    Vector3 Rotation = Vector3.zero;

    int timePerDay;

    float incrementAmount = 0.97f;
    float incrementPerTick;

    // Start is called before the first frame update
    void Start()
    {
        m_Transform = transform;
        m_TimeManager = GameObject.Find("_GAMEMANAGER_").GetComponent<TimeManager>();
        if(m_TimeManager == null)
        {
            Debug.LogError("Time Manager was Null");
            return;
        }
        timePerDay = m_TimeManager.timePerDay;
    }

    // Update is called once per frame
    void Update()
    {
        SpinLight();
    }

    private void SpinLight()
    {
        incrementPerTick = incrementAmount / (timePerDay / m_TimeManager.timeMultiplier);

        Rotation.x += incrementPerTick;

        m_Transform.eulerAngles = Rotation;
    }
}
