//using UnityEngine.InputSystem;
//using UnityEngine;
//using System;

//public class Interactor : MonoBehaviour
//{
//    [SerializeField] float maxInteractingDistance = 10;
//    [SerializeField] float interactingRadius = 1;

//    LayerMask layerMask;
//    Transform cameraTransform;
//    InputAction interactAction;

//    // For Gizmo
//    Vector3 origin;
//    Vector3 direction;
//    Vector3 hitPosition;
//    float hitDistance;

//    [HideInInspector] public Interactable interactableTarget;
//    [HideInInspector] public Transform enemyTarget;

//    Character character;

//    void Start()
//    {
//        character = GetComponent<Character>();
//        cameraTransform = Camera.main.transform;
//        layerMask = LayerMask.GetMask("Interactable", "Enemy", "NPC");

//        interactAction = GetComponent<PlayerInput>().actions["Interact"];
//        interactAction.performed += Interact;
//    }

//    void Update()
//    {
//        direction = cameraTransform.forward;
//        origin = cameraTransform.position;
//        RaycastHit hit;

//        if (Physics.SphereCast(origin, interactingRadius, direction, out hit, maxInteractingDistance, layerMask))
//        {
//            hitPosition = hit.point;
//            hitDistance = hit.distance;
//            if (hit.transform.CompareTag("Enemy"))
//            {
//                enemyTarget = hit.transform;
//                character.EnterCombat();
//            }
//            else if (hit.transform.TryGetComponent<Interactable>(out interactableTarget))
//            {
//                interactableTarget.TargetOn();
//            }
//        }
//        else
//        {
//            if (interactableTarget)
//            {
//                interactableTarget.TargetOff();
//                interactableTarget = null;
//            }

//            if (enemyTarget)
//            {
//                enemyTarget = null;
//                character.ExitCombat();
//            }
//        }
//    }

//    private void Interact(InputAction.CallbackContext obj)
//    {
//        if (interactableTarget != null)
//        {
//            if (Vector3.Distance(transform.position, interactableTarget.transform.position) <= interactableTarget.interactionDistance)
//            {
//                interactableTarget.Interact();
//            }
//        }
//        else
//        {
//            print("nothing to interact!");
//        }
//    }

//    private void OnDrawGizmos()
//    {
//        Gizmos.color = Color.green;
//        Gizmos.DrawLine(origin, origin + direction * hitDistance);
//        Gizmos.DrawWireSphere(hitPosition, interactingRadius);
//    }

//    private void OnDestroy()
//    {
//        interactAction.performed -= Interact;
//    }
//}


using UnityEngine.InputSystem;
using UnityEngine;
using System;

public class Interactor : MonoBehaviour
{
    [SerializeField] float interactingRadius = 10f;
    //[SerializeField] float maxInteractingDistance = 10;

    LayerMask layerMask;
    InputAction interactAction;

    [HideInInspector] public Interactable interactableTarget;
    [HideInInspector] public Transform enemyTarget;

    Character character;
    InteractableNameText interactableNameText;

    void Start()
    {
        character = GetComponent<Character>();
        interactableNameText = FindObjectOfType<InteractableNameText>(); // Trova il componente InteractableNameText nella scena
        layerMask = LayerMask.GetMask("Interactable", "Enemy", "NPC");

        interactAction = GetComponent<PlayerInput>().actions["Interact"];
        interactAction.performed += Interact;
    }

    void Update()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, interactingRadius, layerMask);

        bool enemyFound = false;
        bool interactableFound = false;

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                enemyTarget = hitCollider.transform;
                character.EnterCombat();
                enemyFound = true;
            }
            else if (hitCollider.TryGetComponent<Interactable>(out interactableTarget))
            {
                interactableTarget.TargetOn();
                interactableNameText.ShowText(interactableTarget); // Mostra il testo dell'interagibile
                interactableNameText.SetInteractableNamePosition(interactableTarget); // Imposta la posizione del testo
                interactableFound = true;
            }
        }

        if (!enemyFound && enemyTarget)
        {
            enemyTarget = null;
            character.ExitCombat();
        }

        if (!interactableFound && interactableTarget)
        {
            interactableTarget.TargetOff();
            interactableNameText.HideText(); // Nascondi il testo dell'interagibile
            interactableTarget = null;
        }
    }

    private void Interact(InputAction.CallbackContext obj)
    {
        if (interactableTarget != null)
        {
            if (Vector3.Distance(transform.position, interactableTarget.transform.position) <= interactingRadius)
            {
                interactableTarget.Interact();
            }
        }
        else
        {
            print("nothing to interact!");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, interactingRadius);
    }

    private void OnDestroy()
    {
        interactAction.performed -= Interact;
    }
}


