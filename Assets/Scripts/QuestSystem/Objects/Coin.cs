using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Coin : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private float respawnTimeSeconds = 8;
    [SerializeField] private int goldGained = 1;

    private Collider collider;
    private Renderer visual;
    private bool isCollected = false;

    private void Awake()
    {
        collider = GetComponent<Collider>();
        visual = GetComponentInChildren<Renderer>();
    }

    private void CollectCoin()
    {
        if (isCollected) return;

        isCollected = true;
        collider.enabled = false;
        visual.enabled = false; // Disattiva solo il renderer, mantenendo l'oggetto attivo
        GameEventsManager.instance.goldEvents.GoldGained(goldGained);
        GameEventsManager.instance.miscEvents.CoinCollected();
        StartCoroutine(RespawnAfterTime());
    }

    private IEnumerator RespawnAfterTime()
    {
        yield return new WaitForSeconds(respawnTimeSeconds);
        collider.enabled = true;
        visual.enabled = true;
        isCollected = false;
    }

    private void OnTriggerEnter(Collider otherCollider)
    {
        if (otherCollider.CompareTag("Player"))
        {
            CollectCoin();
        }
    }
}
