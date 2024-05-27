
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class InteractInput : MonoBehaviour
// {
 
//      [SerializeField] TMPro.TextMeshProUGUI textOnScreen;
//      [SerializeField] UIPoolBar hpBar;

//      GameObject currentHoverOverObject;
    
//      [HideInInspector]
//      public InteractableObject hoveringOverObject;

//      [HideInInspector]
//      public Character hoveringOverCharacter;

// // //     // Update is called once per frame
//      void Update()
//     {   
//         CheckInteractObject();

//         if (Input.GetMouseButtonDown(0))
//         {
//             if (hoveringOverObject != null)
//             {
//                 hoveringOverObject.Interact();
//             }
//         }
        

//     }

//     private void CheckInteractObject()
//     {
//         Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//         RaycastHit hit;

//         if (Physics.Raycast(ray, out hit))
//         {
//             if (currentHoverOverObject != hit.transform.gameObject)
//             {
//                 currentHoverOverObject = hit.transform.gameObject;  
//                 UpdateInteractableObject(hit);
//             }
            
//         }
//     }

//     private void UpdateInteractableObject(RaycastHit hit)
//     {
//         InteractableObject interactableObject = hit.transform.GetComponent<InteractableObject>();
//              if (interactableObject != null)
//             {
//                 hoveringOverObject= interactableObject;
//                 hoveringOverCharacter = interactableObject.GetComponent<Character>();
//                 textOnScreen.text = hoveringOverObject.objectName;
//             }
//             else {
//                 hoveringOverCharacter = null;
//                 hoveringOverObject = null;
//                 textOnScreen.text = "";                
//             }
//             UpdateHpBar();
//     }

//     private void UpdateHpBar()
//     {
//         if (hoveringOverCharacter != null)
//         {
//             hpBar.Show(hoveringOverCharacter.lifePool);
//         }
//         else{
//             hpBar.Clear();
//         }
//     }
// }


// // using System.Collections;
// // using System.Collections.Generic;
// // using UnityEngine;

// // public class InteractInput : MonoBehaviour
// // {
// //     [SerializeField] TMPro.TextMeshProUGUI textOnScreen;
// //     [SerializeField] UIPoolBar hpBar;

// //     [HideInInspector]
// //     public InteractableObject hoveringOverObject;

// //     [HideInInspector]
// //     public Character hoveringOverCharacter;

// //     private void Start()
// //     {
// //         if (textOnScreen == null)
// //         {
// //             Debug.LogError("TextOnScreen is not assigned in the Inspector");
// //         }
// //         if (hpBar == null)
// //         {
// //             Debug.LogError("HpBar is not assigned in the Inspector");
// //         }
// //     }

// //     private void OnTriggerEnter(Collider other)
// //     {
// //         Debug.Log("Trigger Entered: " + other.name);
// //         InteractableObject interactableObject = other.GetComponent<InteractableObject>();
// //         if (interactableObject != null)
// //         {
// //             Debug.Log("Interactable Object Found: " + interactableObject.objectName);
// //             hoveringOverObject = interactableObject;
// //             hoveringOverCharacter = interactableObject.GetComponent<Character>();
// //             textOnScreen.text = interactableObject.objectName;
// //             UpdateHpBar();
// //         }
// //         else
// //         {
// //             Debug.Log("No InteractableObject component found on: " + other.name);
// //         }
// //     }

// //     private void OnTriggerExit(Collider other)
// //     {
// //         Debug.Log("Trigger Exited: " + other.name);
// //         if (hoveringOverObject != null && other.gameObject == hoveringOverObject.gameObject)
// //         {
// //             hoveringOverObject = null;
// //             hoveringOverCharacter = null;
// //             textOnScreen.text = "";
// //             hpBar.Clear();
// //         }
// //     }

// //     private void UpdateHpBar()
// //     {
// //         if (hoveringOverCharacter != null)
// //         {
// //             hpBar.Show(hoveringOverCharacter.lifePool);
// //         }
// //         else
// //         {
// //             hpBar.Clear();
// //         }
// //     }
// // }

