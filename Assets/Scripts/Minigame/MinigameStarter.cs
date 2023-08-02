using Articy.Languagegamearticy;
using Articy.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This component inherits from Interactable, and is used for objects that have an ArticyReference component pointing to either an InspectableOBject or an NPC
public class MinigameStarter : Interactable
{
    public override void Interact()
    {
        if(TryGetComponent(out ArticyReference articyReference))                        // If there is an ArticyReference component...
        {
            ArticyObject articyObject = articyReference.GetObject<ArticyObject>();      // ...Fetch the Articy Object referenced in the ArticyReference component
            MinigameManager.instance.OpenMinigameUI(articyObject);                      // ...and Start a minigame passing the Articy Object as argument
        }
        else
        {
            Debug.LogWarning("Object has no Articy entity assigned");
        }
    }
}
