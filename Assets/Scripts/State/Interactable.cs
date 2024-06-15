using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
	public string interactableName="";
	[SerializeField] bool isInteractable = true;

	InteractableNameText interactableNameText;
	GameObject interactableNameCanvas;

	public virtual void Start()
	{
		interactableNameCanvas = GameObject.FindGameObjectWithTag("Canvas");
		interactableNameText = interactableNameCanvas.GetComponentInChildren<InteractableNameText>();
	}

	public void TargetOn()
	{
        interactableNameText.ShowText(this);
        interactableNameText.SetInteractableNamePosition(this);
	}

	public void TargetOff()
	{
        interactableNameText.HideText();
    }

	public void Interact()
	{
		if (isInteractable) Interaction();
	}

	protected virtual void Interaction()
	{
        //print("interact with: " + this.name);
    }

	


	private void OnDestroy()
	{
		TargetOff();
    }
}
