using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// This class reacts to the ClickAction called within InteractionManager,
// and calls the Interact() method of the Interactable child class.

public class Clickable : MonoBehaviour
{

    public void ClickAction()
    {        
        if (TryGetComponent(out Interactable interactableComponent))
        {
            interactableComponent.Interact();
        }
        else
        {
            if (InteractionManager.instance.enableDebugMode) Debug.LogWarning("Clicked object has Clickable component, but no Interactable component.");
        }
    }

}
