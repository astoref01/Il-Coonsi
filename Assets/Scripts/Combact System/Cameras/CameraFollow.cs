using UnityEngine;

public class SimpleFollow : MonoBehaviour
{
    // Il target che la camera seguirà
    public Transform target;

    // La posizione relativa della camera rispetto al target
    private Vector3 offset;

    void Start()
    {
        if (target == null)
        {
            Debug.LogError("Target non assegnato!");
            return;
        }

        // Calcola l'offset iniziale
        offset = transform.position - target.position;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Mantieni la posizione relativa rispetto al target
        transform.position = target.position + offset;

        // Mantieni la stessa rotazione del target
        transform.rotation = target.rotation;
    }
}
