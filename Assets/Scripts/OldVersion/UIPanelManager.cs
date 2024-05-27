// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class UIPanelManager : MonoBehaviour
// {
//     [SerializeField] GameObject inventoryPanel;
//     [SerializeField] GameObject statsPanel;
//     [SerializeField] GameObject questPanel;

//     private void Update()
//     {
//         if (Input.GetKeyDown(KeyCode.I))
//         {
//             OpenInventory();
//         }
//         if (Input.GetKeyDown(KeyCode.Q))
//         {
//             OpenStats();
//         }
//         if (Input.GetKeyDown(KeyCode.S))
//         {
//             OpenQuestPanel();
//         }
//     }

//     public void OpenInventory()
//     {
//         inventoryPanel.SetActive(!inventoryPanel.activeInHierarchy);
//     }
//         public void OpenStats()
//     {
//         statsPanel.SetActive(!statsPanel.activeInHierarchy);
//         questPanel.SetActive(false);
//     }
//         public void OpenQuestPanel()
//     {
//         questPanel.SetActive(!questPanel.activeInHierarchy);
//         statsPanel.SetActive(false);
//     }
// }
