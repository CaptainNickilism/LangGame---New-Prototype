using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationManager : MonoBehaviour
{

    public static LocationManager instance = null;
    public Location currentLocation;

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

    public void ChangeLocation(Location newLocation)
    {
        currentLocation.gameObject.SetActive(false);
        currentLocation = newLocation;
        currentLocation.gameObject.SetActive(true);
    }


}
