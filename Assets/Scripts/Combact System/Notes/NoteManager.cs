using UnityEngine;

public class NoteManager : MonoBehaviour
{
    // Oggetto prefab che rappresenta la nota da spawnare
    public GameObject notePrefab;
    // Punto nello spazio dove spawnare le note
    public Transform noteSpawnPoint;
    // Riferimento all'oggetto ScriptableObject che rappresenta l'arma
    public GunScriptableObject gun;
    // Frequenza con cui le note vengono spawnate (in secondi)
    public float spawnRate = 0.1f;

    // Tempo in cui la prossima nota verrà spawnata
    private float nextSpawnTime;

    // Metodo chiamato all'avvio dello script
    void Start()
    {
        // Controlla se notePrefab non è assegnato nell'Inspector e logga un errore se è null
        if (notePrefab == null) Debug.LogError("notePrefab is not assigned in the Inspector.");
        // Controlla se noteSpawnPoint non è assegnato nell'Inspector e logga un errore se è null
        if (noteSpawnPoint == null) Debug.LogError("noteSpawnPoint is not assigned in the Inspector.");
        // Controlla se gun non è assegnato nell'Inspector e logga un errore se è null
        if (gun == null) Debug.LogError("gun is not assigned in the Inspector.");
    }

    // Metodo chiamato ad ogni frame
    void Update()
    {
        // Controlla se è il momento di spawnare una nuova nota
        if (Time.time >= nextSpawnTime)
        {
            // Spawna una nuova nota
            SpawnNote();
            // Aggiorna il tempo per la prossima nota da spawnare
            nextSpawnTime = Time.time + spawnRate;
        }
    }

    // Metodo per spawnare una nuova nota
    void SpawnNote()
    {
        // Controlla se notePrefab o noteSpawnPoint sono null e logga un errore se uno dei due lo è
        if (notePrefab == null || noteSpawnPoint == null)
        {
            Debug.LogError("Cannot spawn note. Either notePrefab or noteSpawnPoint is null.");
            return;
        }

        // Crea una nuova istanza della nota nel punto specificato
        GameObject note = Instantiate(notePrefab, noteSpawnPoint.position, Quaternion.identity, noteSpawnPoint);
        // Cerca il componente Note nell'oggetto appena creato
        Note noteComponent = note.GetComponent<Note>();

        // Se il componente Note è presente, imposta il tasto da premere
        if (noteComponent != null)
        {
            noteComponent.keyToPress = KeyCode.Space; // Imposta il tasto appropriato
        }
        else
        {
            // Se il componente Note non è presente, logga un errore
            Debug.LogError("Spawned note does not have a Note component.");
        }
    }

    // Metodo chiamato quando una nota viene colpita
    public void NoteHit(float timingAccuracy, Vector3 shootStartPosition, Vector3 hitPosition)
    {
        // Controlla se gun è null e logga un errore se lo è
        if (gun == null)
        {
            Debug.LogError("Cannot apply damage. Gun is null.");
            return;
        }

        // Calcola il danno basato sulla precisione del timing
        int damage = CalculateDamage(timingAccuracy);
        // Applica il danno usando il metodo dell'arma
        gun.ApplyDamage(shootStartPosition, hitPosition, damage);
    }

    // Metodo per calcolare il danno basato sulla precisione del timing
    private int CalculateDamage(float timingAccuracy)
    {
        // Calcola il danno in base alla precisione, qui puoi implementare la tua logica
        return Mathf.CeilToInt(Mathf.Max(0, 100 - (timingAccuracy * 100))); // Esempio di calcolo
    }
}
