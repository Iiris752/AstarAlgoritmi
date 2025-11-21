using System.Data.Common;
using UnityEngine;

public class AstrNode : MonoBehaviour
{
    public bool startpoint = false;
    public bool endpoint = false;
    public bool isNotWalkable = false;
    public Material unwalkableNodeColour;
    public Material endpointNodeColour;
    public Material startpointNodeColour;

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
