using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconManager : MonoBehaviour
{

    Transform Icon;
    [SerializeField]
    List<Material> materials = new List<Material>();
    [SerializeField]
    bool isFood;
    VilliagerAI villagerAI;

    MeshRenderer currentMesh;

    float offset = -90;
    float cameraRot;
    CameraController MainCam;

    // Start is called before the first frame update
    void Start()
    {
        Icon = transform;

        currentMesh = GetComponent<MeshRenderer>();
        if(currentMesh == null)
        {
            Debug.LogError("currentMesh was null");
        }
        currentMesh.enabled = false;

        villagerAI = GetComponentInParent<VilliagerAI>();
        if(villagerAI == null)
        {
            Debug.LogError("VillagerAI is null");
        }

        MainCam = CameraController.instance;
        if (MainCam == null)
        {
            Debug.LogError("Camera was null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        cameraRot = MainCam.transform.eulerAngles.y;
        Quaternion rot = Quaternion.Euler(0, cameraRot + offset, 90);
        transform.rotation = rot;

        UpdateIcon();
    }

    private void UpdateIcon()
    {
        //if(Icon == null)
        //{
        //    Debug.LogError("Icon was null");
        //    return;
        //}

        if(villagerAI.IsThirsty())
        {
            currentMesh.material = materials[1];
            currentMesh.enabled = true;

        }

        else if(villagerAI.IsHungry())
        {
            currentMesh.material= materials[0];
            currentMesh.enabled = true;
        }

        else
        {
            currentMesh.enabled = false;
        }

        
    }
}
