// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class AttackInput : MonoBehaviour
// {
//     InteractInput interactInput;
//     AttackHandler attackHandler;

//     // Start is called before the first frame update
//     private void Awake()
//     {
//         interactInput = GetComponent<InteractInput>();
//         attackHandler = GetComponent<AttackHandler>();
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         if (Input.GetMouseButtonDown(0))
//         {
//             if (interactInput.hoveringOverCharacter != null)
//             {
//                 attackHandler.Attack(interactInput.hoveringOverCharacter);
//             }
//         }
//     }
// }

// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class AttackInput : MonoBehaviour
// {
//     InteractInput interactInput;
//     AttackHandler attackHandler;

//     private void Awake()
//     {
//         interactInput = GetComponent<InteractInput>();
//         attackHandler = GetComponent<AttackHandler>();
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         // Usa un pulsante del controller per l'attacco, ad esempio il pulsante "Fire1"
//         if (Input.GetButtonDown("Fire1")) // "Fire1" di solito corrisponde al pulsante "A" o "Cross" sui controller
//         {
//             if (interactInput.hoveringOverCharacter != null)
//             {
//                 attackHandler.Attack(interactInput.hoveringOverCharacter);
//             }
//         }
//     }
// }
