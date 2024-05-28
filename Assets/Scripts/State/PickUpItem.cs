using System.Collections;
using UnityEngine;

public class PickUpItem : Interactable
{
	//[Header("Item Data")]	
	[SerializeField] string itemName;
	public Item Item;
    //[SerializeField] int amount = 1;

    public override void Start()
	{
		base.Start();
        //interactableName = item.itemName;
        interactableName = itemName;		
    }

    protected override void Interaction()
	{
		base.Interaction();
        print("I put " + itemName + " in my inventory.");
		InventoryManager.Instance.Add(Item);
        Destroy(this.gameObject);
		
		//animation
		//animator.SetTrigger("PickUp");

		//add to inventory via events
		//Events.AddInventoryItem(item,amount);

	}

}
