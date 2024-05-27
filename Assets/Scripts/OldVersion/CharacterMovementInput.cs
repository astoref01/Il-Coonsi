// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class CharacterMovementInput : MonoBehaviour
//{
    // [SerializeField] MouseInput mouseInput;
    // CharacterMovement characterMovement;

    // private void Awake()
    // {
    //     characterMovement = GetComponent<CharacterMovement>();
    // }

    // // Update is called once per frame
    // void Update()
    // {
    //     if (Input.GetMouseButtonDown(0))
    //     {
    //         characterMovement.SetDestination(mouseInput.MouseInputPosition);
    //     }
         
    // }}
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class CharacterMovementInput : MonoBehaviour
// {
//     CharacterMovement characterMovement;

//     private void Awake()
//     {
//         characterMovement = GetComponent<CharacterMovement>();
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         // Ottieni l'input del controller
//         float horizontal = Input.GetAxis("Horizontal");
//         float vertical = Input.GetAxis("Vertical");

//         // Crea un vettore di movimento basato sull'input del controller
//         Vector3 movement = new Vector3(horizontal, 0, vertical);

//         // Se c'è input, imposta la destinazione
//         if (movement.magnitude > 0.1f)  // Usa una soglia per evitare piccoli movimenti
//         {
//             Vector3 destinationPosition = transform.position + movement;
//             characterMovement.SetDestination(destinationPosition);
//         }
//     }
// }


