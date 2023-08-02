using Articy.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connection : Interactable
{

    public Location targetLocation;

    public override void Interact()
    {
        if (targetLocation)
        {
            LocationManager.instance.ChangeLocation(targetLocation);
        }
        else
        {
            Debug.LogWarning("Connection has no target Location!");
        }
    }

}
