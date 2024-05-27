// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class AttackHandler : MonoBehaviour
// {
//     Character character;
//     [SerializeField] float attackRange = 1f;
//     [SerializeField] float defaultTimeToAttack = 2f;
//     float attackTimer;
//     Animator animator;
//     CharacterMovement characterMovement;
//     Character target;
    
//     private void Awake()
//     {
//         animator = GetComponentInChildren<Animator>();
//         characterMovement = GetComponent<CharacterMovement>();
//         character = GetComponent<Character>();
//     }

//     // Metodo per impostare il bersaglio e iniziare l'attacco
//     internal void Attack(Character target)
//     {
//         this.target = target;
//         ProcessAttack();
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         attackTimerTick();

//         if (Input.GetButtonDown("Fire1")) // Usa il pulsante "Fire1" per attaccare
//         {
//             if (target != null)
//             {
//                 ProcessAttack();
//             }
//         }
//     }

//     private void attackTimerTick()
//     {
//         if (attackTimer > 0f)
//         {
//             attackTimer -= Time.deltaTime;
//         }
//     }

//     private void ProcessAttack()
//     {
//         if (target == null) return;

//         float distance = Vector3.Distance(transform.position, target.transform.position);

//         if (distance < attackRange)
//         {
//             if (attackTimer > 0f) return;

//             attackTimer = GetAttackTime();
//             animator.SetTrigger("Attack");

//             target.TakeDamage(character.TakeStats(Statistic.Damage).integer_value);
//             target = null; // Resetta il bersaglio dopo l'attacco
//         }
//         else
//         {
//             characterMovement.SetDestination(target.transform.position);
//         }
//     }

//     float GetAttackTime()
//     {
//         float attackTime = defaultTimeToAttack;
//         attackTime /= character.TakeStats(Statistic.AttackSpeed).float_value;
//         return attackTime;
//     }
// }
