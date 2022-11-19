using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class BTManager : MonoBehaviour
{
    BTree VillagerBT;
    BTree OutlawBT;

    // Start is called before the first frame update
    void Start()
    {
        VillagerBT = GetComponent<VillageBT>();
        OutlawBT = GetComponent<TempBT>();

        if (VillagerBT == null || OutlawBT == null)
        {
            Debug.LogError("VillagerBT or OutlawBT were null");
        }

        VillagerBT.enabled = true;
        OutlawBT.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            VillagerBT.enabled = false;
            OutlawBT.enabled = true;
        }
    }
}
