// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class MouseInput : MonoBehaviour
// {   
//     [HideInInspector]
//     public Vector3 MouseInputPosition;

//     // Update is called once per frame
//     void Update()
//     {
//        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//        RaycastHit hit;
//        if (Physics.Raycast(ray, out hit, float.MaxValue))
//        {
//             MouseInputPosition = hit.point;
            
//        }
//     }
// }
