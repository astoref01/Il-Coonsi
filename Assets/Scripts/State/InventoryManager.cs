using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public List<Item> Items = new List<Item>();

    public Transform ItemContent;
    public GameObject InventoryItem;

    public Toggle EnableRemove;
    public InventoryItemController[] InventoryItems;

    public InputActionReference inventoryActionReference; // Riferimento all'azione Inventory
    public GameObject inventoryUI; // Riferimento al GameObject dell'inventario
    private bool isInventoryOpen = false;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        inventoryActionReference.action.performed += ToggleInventory;
        inventoryActionReference.action.Enable();
    }

    private void OnDisable()
    {
        inventoryActionReference.action.performed -= ToggleInventory;
        inventoryActionReference.action.Disable();
    }

    private void ToggleInventory(InputAction.CallbackContext context)
    {
        if (isInventoryOpen)
        {
            CloseInventory();
        }
        else
        {
            OpenInventory();
        }
    }

    public void Add(Item item)
    {
        Items.Add(item);
    }

    public void Remove(Item item)
    {
        Items.Remove(item);
    }

    public void ListItems()
    {
        foreach (Transform item in ItemContent)
        {
            Destroy(item.gameObject);
        }

        foreach (var item in Items)
        {
            GameObject obj = Instantiate(InventoryItem, ItemContent);
            var itemName = obj.transform.Find("ItemName").GetComponent<TMP_Text>();
            var itemIcon = obj.transform.Find("ItemIcon").GetComponent<Image>();
            var removeButton = obj.transform.Find("RemoveButton").GetComponent<Button>();

            itemName.text = item.itemName;
            itemIcon.sprite = item.icon;

            if (EnableRemove.isOn)
                removeButton.gameObject.SetActive(true);
        }
        SetInventoryItems();

    }

    public void EnableItemRemove()
    {
        if (EnableRemove.isOn)
        {
            foreach (Transform item in ItemContent)
            {
                item.Find("RemoveButton").gameObject.SetActive(true);
            }
        }
        else
        {
            foreach (Transform item in ItemContent)
            {
                item.Find("RemoveButton").gameObject.SetActive(false);
            }
        }
    }

    public void SetInventoryItems()
    {
        InventoryItems = ItemContent.GetComponentsInChildren<InventoryItemController>();
        for (int i = 0; i < Items.Count; i++)
        {
            InventoryItems[i].AddItem(Items[i]);
        }
    }

    private void OpenInventory()
    {
        // Codice per aprire l'inventario (es. abilitare UI dell'inventario)
        inventoryUI.SetActive(true);
        ListItems();
        isInventoryOpen = true;
    }

    private void CloseInventory()
    {
        // Codice per chiudere l'inventario (es. disabilitare UI dell'inventario)
        inventoryUI.SetActive(false);
        isInventoryOpen = false;
    }
}
