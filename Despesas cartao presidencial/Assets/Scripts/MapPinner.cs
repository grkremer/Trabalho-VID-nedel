using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.Geospatial;
using Microsoft.Maps.Unity;

public class MapPinner : MonoBehaviour
{
    public MapPinLayer pinLayer;
    public MapPin pin;

    public MapRenderer mapRenderer;

    //public LatLon location;

    // Start is called before the first frame update
    void Start()
    {
        var mapPin = Instantiate(pin);
        mapPin.Location = new LatLon(0, 0);
        pinLayer.MapPins.Add(mapPin);
        //MapPin.UpdateScales(new List<MapPin>(){mapPin}, mapRenderer);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
