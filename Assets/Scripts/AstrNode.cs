using System.Collections;
using System.Data.Common;
using UnityEngine;
using System.Collections.Generic;

//merkitään muistiin ja esitellään kaikki node colourit:
public enum NodeType
{
    BarrierMaterial = 0,
    ClosedListNode,
    endpoint,
    NodeMaterial,
    OpenListNode,
    PathNode,
    startpoint,
    unwalkableNodeColour
}

public class AstrNode : MonoBehaviour
{
    public bool startpoint = false;
    public bool endpoint = false;
    public bool isNotWalkable = false;

    //Astarissa vaaditut arvot
    //f = g + h
    public float f = 0; //Astarissa vaadittu f-arvo
    public float g = 0; //Astarissa vaadittu g-arvo 
    public float h = 0; //Astarissa vaadittu h-arvo

    public AstrNode parent;

    public List<AstrNode> neighborNodes = new List<AstrNode>();

    public Material[] nodeColours;

    public void SetColour(NodeType colourIndex)
    {
        MeshRenderer mr = GetComponent<MeshRenderer>();
        mr.material = nodeColours[(int)colourIndex];
    }

}
