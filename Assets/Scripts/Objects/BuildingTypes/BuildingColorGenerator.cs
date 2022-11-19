using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingColorGenerator : MonoBehaviour
{

    [SerializeField]
    List<Color> mainColorList = new List<Color>();

    [SerializeField]
    List<Color> secondaryColorList = new List<Color>();

    int mainNum;
    int secondaryNum;

    MeshRenderer meshRender;
    Material buildingMat;

    private void Awake()
    {
        meshRender = GetComponent<MeshRenderer>();
        buildingMat = meshRender.material;
        mainNum = Random.Range(0, mainColorList.Count);
        secondaryNum = Random.Range(0, secondaryColorList.Count);
        if (mainColorList.Count == 0) return;
        buildingMat.SetColor("_ColorMain", mainColorList[mainNum]);
        if (secondaryColorList.Count == 0) return;
        buildingMat.SetColor("_ColorSecondary", secondaryColorList[secondaryNum]);
    }
}
