using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITimer : MonoBehaviour
{
    int m_timer = 0;

    // Update is called once per frame
    void Update()
    {
        m_timer++;

        if(m_timer > 5000)
        {
            m_timer = 0;
            this.gameObject.SetActive(false);
        }

        if (this.gameObject.activeInHierarchy == false && m_timer != 0)
        {
            m_timer = 0;
        }
    }
}
