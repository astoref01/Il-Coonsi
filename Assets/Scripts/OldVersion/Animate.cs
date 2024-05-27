// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class Animate : MonoBehaviour
// {
//     Animator animator;

//     UnityEngine.AI.NavMeshAgent agent;

//     private void Awake()
//     {
//         animator = GetComponentInChildren<Animator>();
//         agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
//     }

//     private void Update()
//     {
//         float motion = agent.velocity.magnitude;

//         animator.SetFloat("motion", motion);
        
//     }
// }
