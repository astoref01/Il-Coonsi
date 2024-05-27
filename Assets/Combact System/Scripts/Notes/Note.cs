using UnityEngine;

public class Note : MonoBehaviour
{
    public float speed = 10f;
    public KeyCode keyToPress;

    private bool isPressed = false;

    void Update()
    {
        // Muovere la nota verso il basso
        GetComponent<RectTransform>().anchoredPosition += Vector2.down * speed * Time.deltaTime;

        // Rilevare la pressione del tasto
        if (Input.GetKeyDown(keyToPress) && !isPressed)
        {
            isPressed = true;
            float timingAccuracy = Mathf.Abs(GetComponent<RectTransform>().anchoredPosition.y); // Regola in base ai tuoi requisiti
            FindObjectOfType<NoteManager>().NoteHit(timingAccuracy, shootStartPosition, hitPosition);
            Destroy(gameObject);
        }

        // Distruggere la nota se esce dallo schermo
        if (GetComponent<RectTransform>().anchoredPosition.y < -Screen.height / 2)
        {
            Destroy(gameObject);
        }
    }

    // Qui dovrai passare shootStartPosition e hitPosition
    private Vector3 shootStartPosition;
    private Vector3 hitPosition;

    public void Initialize(Vector3 startPosition, Vector3 endPosition)
    {
        shootStartPosition = startPosition;
        hitPosition = endPosition;
    }
}
