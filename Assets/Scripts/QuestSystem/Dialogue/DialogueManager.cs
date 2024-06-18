using UnityEngine;
using TMPro;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public Canvas dialogueCanvas;
    public string[] dialoguePhrases;
    public int currentPhraseIndex = 0;
    private bool isTyping = false;
    public float typingSpeed = 0.05f;
    public bool end = false;
    public AudioSource audioSource; // Aggiunto campo per l'audio
    public AudioClip[] dialogueAudioClips;

    void Start()
    {
        Debug.Log(dialogueAudioClips);
        dialogueCanvas.gameObject.SetActive(false);
    }

    public void StartDialogue()
    {
        dialogueCanvas.gameObject.SetActive(true);
        currentPhraseIndex = 0;
        ShowNextPhrase();
    }

    public void ShowNextPhrase()
    {
        if (dialogueCanvas.gameObject.activeSelf && (currentPhraseIndex < dialoguePhrases.Length))
        {
            if (!isTyping)
            {
                if (currentPhraseIndex < dialogueAudioClips.Length && dialogueAudioClips[currentPhraseIndex] != null)
                {
                    audioSource.clip = dialogueAudioClips[currentPhraseIndex];
                    audioSource.Play();
                }
                StartCoroutine(TypePhrase(dialoguePhrases[currentPhraseIndex]));
            }
        }
        else
        {
            EndDialogue(); // Chiamata a EndDialogue quando tutte le frasi sono state mostrate o non c'è più un NPC
        }
    }

    private IEnumerator TypePhrase(string phrase)
    {
        isTyping = true;
        dialogueText.text = "";
        foreach (char letter in phrase.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
        currentPhraseIndex++; // Sposta l'incremento dell'indice qui
    }

    public void EndDialogue()
    {
        dialogueCanvas.gameObject.SetActive(false);
        end = true;
    }
}
