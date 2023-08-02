using Articy.Unity;
using Articy.Unity.Interfaces;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

// This class instantiates the collider and prepares virtual methods to be overridden by child classes
public class Interactable : MonoBehaviour 
{
    
    
    void Awake()
    {
        Destroy(gameObject.GetComponent<PolygonCollider2D>());
        _ = gameObject.AddComponent(typeof(PolygonCollider2D)) as PolygonCollider2D; // Draw a pixel-perfect collider based on the 2D sprite shape
    }

    public virtual void Interact() // Call on simple interaction e.g. click
    {
        Debug.LogWarning("No override found");
    }

    public virtual void Interact(Interactable otherObject) // Call when dropping OTHER on THIS
    {
        Debug.LogWarning("No override found");
    }

    
}
