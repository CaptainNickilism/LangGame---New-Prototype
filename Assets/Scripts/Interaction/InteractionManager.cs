using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using DigitalRuby.AdvancedPolygonCollider;

public class InteractionManager : MonoBehaviour
{
    static public InteractionManager instance = null;
    public bool enableDebugMode;

    public Holdable heldObject;
    public Draggable draggedObject;
    public Interactable interactedObject;
    void Awake()
    {

        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ClickAction(GameObject clickedObject)
    {
        if (clickedObject.TryGetComponent(out Clickable clickable)) // If the clicked object has the Clickable component
        {
            clickable.ClickAction();                                // Call the action of the Cickable component of the object
        }
        else if (enableDebugMode) Debug.Log("Clicked on a non interactable surface");
    }

    public void StartHoldAction(GameObject loggedObject) // Actions to perform when holding starts
    {
        if (loggedObject && loggedObject.TryGetComponent(out Holdable holdable)) // If the held object has the Holdable component
        {
            heldObject = holdable;                                                      // Log the Holdable component of the object being held
        }
        else if (enableDebugMode) Debug.LogWarning("No Holdable component found.");
    }

    public void HoldAction(GameObject loggedObject)
    {
        // Things to do while holding
    }

    public void StopHoldAction(GameObject loggedObject) // Actions to perform when holding stops
    {
        if (heldObject)
        {
            // Things to do when stopping hold           
            heldObject = null;
        }
        else if (enableDebugMode) Debug.LogError("Attempting to stop hold, but no Holdable object found");
    }

    public void StartDragAction(GameObject loggedObject) // Actions to perform when dragging starts
    {  
        if (loggedObject && loggedObject.TryGetComponent(out Draggable draggable)) // If the dragged object has the Draggable component
        {
            draggedObject = draggable;                                                  // Log the draggeed object
            draggedObject.gameObject.GetComponent<PolygonCollider2D>().enabled = false; // Disable collision to be able to raycast behind the currently dragged object
        }
        else if (enableDebugMode) Debug.LogWarning("No Draggable component found.");
    }

    public void DragAction(GameObject loggedObject,  Vector3 cursorPos)
    {
        if (draggedObject)                  // If there's a draggable object logged
        {
            draggedObject.currentPos = cursorPos;   // Set its current position to the cursor Position
            draggedObject.inPos = true;
        }
        else if (enableDebugMode) Debug.LogWarning("No item is being dragged!");
    }


    public void StopDragAction(GameObject hoveredObject) // Actions to perform when dragging stops
    {
        if (draggedObject)
        {
            if (enableDebugMode) Debug.Log("Dropped " + draggedObject.name);
            draggedObject.gameObject.GetComponent<PolygonCollider2D>().enabled = true; // Enables collision again
            draggedObject.DropAction(hoveredObject);                                   // Call the Drop Action of the object behind the currently dragged object (the one on which you're dropping)
            
            
            draggedObject = null;
        }
        else if (enableDebugMode) Debug.LogError("Attempting to stop drag, but no Draggable object is logged");
    }


}
