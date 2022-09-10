using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PublicUtility
{
    public static class Utils
    {

        public static float PI = 3.141592653f;
        
        //Functions for MouseWorldPosition.
        public static Vector3 GetMouseWorldPosition()
        {
            Vector3 pos = GetMouseWorldPositionZ(Input.mousePosition, Camera.main);
            return pos;
        }
        public static Vector3 GetMouseWorldPositionZ(Vector3 screenPosition, Camera worldCamera)
        {
            Ray ray = worldCamera.ScreenPointToRay(screenPosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                Vector3 worldpos = hitInfo.point;
                return worldpos;
            }

            //Debug.LogError("Raycast didn't hit @ GetMouseWorldPositionZ");
            return Vector3.zero;
        }
    }
}
