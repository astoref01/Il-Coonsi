using UnityEngine.InputSystem;
using UnityEngine;
using System;

public class Interactor : MonoBehaviour
{
    [SerializeField] float interactingRadius = 10f;

    LayerMask layerMask;
    InputAction interactAction;
    InputAction dialogueAction;

    [HideInInspector] public Interactable interactableTarget;
    [HideInInspector] public Transform npcTarget;

    Character character;
    InteractableNameText interactableNameText;
    DialogueManager dialogueManager;

    void Start()
    {
        character = GetComponent<Character>();
        interactableNameText = FindObjectOfType<InteractableNameText>(); // Trova il componente InteractableNameText nella scena
        layerMask = LayerMask.GetMask("Interactable", "Enemy", "NPC");
//<<<<<<< HEAD


//=======
        dialogueManager = FindObjectOfType<DialogueManager>(); // Trova il componente DialogManager nella scena
//>>>>>>> origin/main
        interactAction = GetComponent<PlayerInput>().actions["Interact"];
        dialogueAction = GetComponent<PlayerInput>().actions["Dialogue"];
        interactAction.performed += Interact;
        dialogueAction.performed += ContinueDialogue;
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
                if (!character.IsCombatting() && !character.IsAttacking())
                {
                    character.EnterCombat();
                    enemyFound = true;
                }
            }
            else if (hitCollider.TryGetComponent<Interactable>(out interactableTarget))
            {
                interactableTarget.TargetOn();
                interactableNameText.ShowText(interactableTarget); // Mostra il testo dell'interagibile
                interactableNameText.SetInteractableNamePosition(interactableTarget); // Imposta la posizione del testo
                interactableFound = true;
            }
            else if (hitCollider.CompareTag("NPC") && !dialogueManager.end)
            {
                npcTarget = hitCollider.transform;
                if (!dialogueManager.dialogueCanvas.gameObject.activeSelf)
                {
                    dialogueManager.StartDialogue();
                }
                interactableFound = true;
            }
        }

        if (!enemyFound)
        {
            character.ExitCombat();
        }

        if (!interactableFound)
        {
            if (interactableTarget)
            {
                interactableTarget.TargetOff();
                interactableNameText.HideText(); // Nascondi il testo dell'interagibile
                interactableTarget = null;
            }

            if (npcTarget)
            {
                dialogueManager.EndDialogue();
                dialogueManager.end = false;
                npcTarget = null;
            }
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, interactingRadius);
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

    private void ContinueDialogue(InputAction.CallbackContext obj)
    {
        if (npcTarget != null && dialogueManager.dialogueCanvas.gameObject.activeSelf)
        {
            dialogueManager.ShowNextPhrase();
        }
    }


    private void OnDestroy()
    {
        interactAction.performed -= Interact;
    }
}