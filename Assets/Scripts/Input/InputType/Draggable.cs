using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// This class makes it possible to set the dragging behaviour of the object with default methods.
// The DropAction method is called within InteractionManager and calls the Interact() method of the Interactable child class

public class Draggable : MonoBehaviour
{
    [Tooltip("Should the item go back to its original position if it is dropped in the wrong place?")]
    public bool bindedToPosition = true;
    [Range(0.001f, 1f)]public float returnSpeed = 0.1f;
    public Vector2 defaultPos, currentPos;
    public bool inPos;

    private void Start()
    {
        ResetDefaultPosition();
        StartCoroutine(RestorePosition());
    }

    private void Update()
    {
        gameObject.transform.position = currentPos;
    }

    public void DropAction(GameObject hoveredObject)
    {
        if (TryGetComponent(out Interactable interactableComponent)) // If item is interactable...
        {
            if (hoveredObject)                                              //...And there is an hovered object...
            {
                //if (hoveredObject.TryGetComponent(out DropZone dropZone)) //...And the hovered object is interactable
                //{
                //    dropZone.Interact(interactableComponent);
                //    Debug.Log("Interaction started between: " + gameObject.name + " and " + hoveredObject.name);
                //    return;
                //}
                //else
                //{
                //    if (InteractionManager.instance.enableDebugMode) Debug.LogWarning("Dropped object has Interactable component and was dropped on Object, but that is NOT INTERACTABLE");
                //}
            }
            else
            {
                if (InteractionManager.instance.enableDebugMode) Debug.LogWarning("Dropped object has Interactable component, but was dropped on nothing");
            }
        }
        else
        {
            if (InteractionManager.instance.enableDebugMode) Debug.LogWarning("Dropped object has NO Interactable component");
            
        }
        if (bindedToPosition)
        {
            StartCoroutine(RestorePosition());
            if (InteractionManager.instance.enableDebugMode) Debug.Log("Restoring position");
        }
    }


    public void ResetDefaultPosition()
    {
        defaultPos = gameObject.transform.position;
    }

    public IEnumerator RestorePosition()
    {
        inPos = false;
        while (!inPos)
        {
            if (Vector2.Distance(currentPos, defaultPos) > 1)
            {
                currentPos = Vector2.Lerp(gameObject.transform.position, defaultPos, returnSpeed);
                yield return new WaitForFixedUpdate();
            }
                
            else
            {
                inPos = true;
            }
        }
    }

}
