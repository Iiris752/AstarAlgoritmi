using System.Collections;
using System.Data.Common;
using UnityEngine;
using System.Collections.Generic;

public class AstrNode : MonoBehaviour
{
    public bool startpoint = false;
    public bool endpoint = false;
    public bool isNotWalkable = false;
    public Material unwalkableNodeColour;

    //Astarissa vaaditut arvot
    //f = g + h
    public float f = 0; //Astarissa vaadittu f-arvo
    public float g = 0; //Astarissa vaadittu g-arvo 
    public float h = 0; //Astarissa vaadittu h-arvo

    public Material endpointNodeColour;
    public Material startpointNodeColour;

    public List<AstrNode> neighborNodes = new List<AstrNode>();

    public Material[] nodeColours;

    public void SetColourUnwalkable()
    {
        isNotWalkable = true;
        MeshRenderer mr = GetComponent<MeshRenderer>();
        mr.sharedMaterial = unwalkableNodeColour;
    }

    public void SetColourStartpoint()
    {
        isNotWalkable = true;
        MeshRenderer mr = GetComponent<MeshRenderer>();
        mr.sharedMaterial = startpointNodeColour;
    }

    public void SetColourEndpoint()
    {
        isNotWalkable = true;
        MeshRenderer mr = GetComponent<MeshRenderer>();
        mr.sharedMaterial = endpointNodeColour;
    }

}
