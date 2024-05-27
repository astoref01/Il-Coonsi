// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

  
// public class CharacterMovement : MonoBehaviour
// {      
//     UnityEngine.AI.NavMeshAgent agent;

//     // Start is called before the first frame update
//     private void Awake()
//     {
//         agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
//     }

//     public void SetDestination(Vector3 destinationPosition)
//     {
//         agent.SetDestination(destinationPosition);
//     }
//     internal void Stop()
//     {
//         agent.isStopped=true;
//     }
// }
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class CharacterMovement : MonoBehaviour
// {      
//     UnityEngine.AI.NavMeshAgent agent;

//     // Start is called before the first frame update
//     private void Awake()
//     {
//         agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
//     }

//     public void SetDestination(Vector3 destinationPosition)
//     {
//         agent.SetDestination(destinationPosition);
//     }

//     internal void Stop()
//     {
//         agent.isStopped = true;
//     }

//     // Aggiungi questa funzione per impostare una direzione di movimento continua
//     public void Move(Vector3 direction)
//     {
//         agent.Move(direction * Time.deltaTime * agent.speed);
//     }
// }
