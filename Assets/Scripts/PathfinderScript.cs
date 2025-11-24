using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class PathfinderScript : MonoBehaviour
{
    List<AstrNode> openList = new List<AstrNode>();
    List<AstrNode> closedList = new List<AstrNode>();

    //visualisointi:
    public AstrNode[] openListVisible;
    public AstrNode[] closedListVisible;

    public AstrNode startNode;
    public AstrNode endNode;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown("p"))
        {
            Debug.Log("Polunetsintä aloitettu");

            //Coroutine antaa aikaa, tekee viiveitä, jotta näen mitä tapahtuu
            StartCoroutine("PathfindingStarts");
        }
    }

    //coroutinen palautusarvo: Ienumerator
    IEnumerator PathfindingStarts()
    {
        Debug.Log("Polunetsintä käynnissä...");

        //etsitään polunetsintää varten alku- ja loppupisteet
        GameObject[] allNodes = GameObject.FindGameObjectsWithTag("AStarNode");

        for (int i = 0; i < allNodes.Length; i++)
        {
            GameObject checkBarrierNode = allNodes[i];
            AstrNode nodescript = checkBarrierNode.GetComponent<AstrNode>();

            if (nodescript.startpoint == true)
            {
                startNode = nodescript;
                Debug.Log("Lähtösolmu asetettu: " + startNode.gameObject.name);
            }

            if (nodescript.endpoint == true)
            {
                endNode = nodescript;
                Debug.Log("Maalisolmu asetettu: " + endNode.gameObject.name);
            }
        }

        //laitetaan käsiteltävä node open listaan
        //vaihdetaan sen väri ja odotetaan hetki
        AstrNode currentNode = startNode;
        openList.Add(currentNode);
        currentNode.SetColour(NodeType.OpenListNode);
        yield return new WaitForSeconds(1f);

        //käytetään tässä for-loop, vältetään ikiluuppi
        for (int i = 0; i < 200; i++)
            {
                //laitetaan käsiteltävä item closed listaan ja vaihdetaan väri
                openList.Remove(currentNode);
                closedList.Add(currentNode);
                currentNode.SetColour(NodeType.ClosedListNode);

                openListVisible = new AstrNode[openList.Count];
                openList.CopyTo(openListVisible);

                closedListVisible = new AstrNode[closedList.Count];
                closedList.CopyTo(closedListVisible);
                openList.CopyTo(openListVisible);

                closedListVisible = new AstrNode[closedList.Count];
                closedList.CopyTo(closedListVisible);

                CheckCurrentNode(currentNode, currentNode.neighborNodes);

                //halutaan löytää open listasta pienimmän f-arvon omaava node
                AstrNode smallestFvalue = null; 
                float smallestV = float.MaxValue;
                for ( int openi = 0; openi < openList.Count; openi++)
                    {
                    AstrNode openlistNode = (AstrNode)openList[openi];

                    if (openlistNode.f < smallestV)
                        {
                            smallestV = openlistNode.f;
                            smallestFvalue = openlistNode;
                        }
                    }
                
                if(smallestFvalue == null)
                    {
                        Debug.Log("Openlist tyhjä, ei polkua");
                        yield break;
                    }

                currentNode = smallestFvalue; 
                yield return new WaitForSeconds(1f);

                if (currentNode == endNode)
                {
                    Debug.Log("Loppupiste saavutettiin. Polku löytyi!");
                    break; //lopeta coroutine
                }

                if (openList.Count <= 0)
                {
                    Debug.Log("POLKUA EI LÖYDY");
                    yield break; //lopeta coroutine
                }
            }


        yield return new WaitForSeconds(0.5f);
        AstrNode PathNode = endNode;
        for (int final = 0; final < 50; final ++)
            {
                PathNode.SetColour(NodeType.PathNode);
                PathNode = PathNode.parent;
            }

        }

    void CheckCurrentNode(AstrNode ThisNode, List<AstrNode> neighborNodes)
    {
        //käydään läpi kaikki naapuri nodet
        //tarkistetaan ensin onko naapuri closed listassa
        for (int i = 0; i < neighborNodes.Count; i++)
        {
            AstrNode checkNode = neighborNodes[i];
            
            bool CheckNodeClosed = false;

            for(int closedi = 0; closedi < closedList.Count; closedi++)
                {
                    if (checkNode == closedList[closedi])
                    {
                        CheckNodeClosed = true;
                        closedi = closedList.Count;
                    }
                }

            //suoritetaan tämä vain jos naapuri EI ole closed listassa:
            // ja tarkistetaan onko esteitä
            if (CheckNodeClosed == false && checkNode.isNotWalkable == false)
                {
                    bool checkNodeisOpen = false;
                    for (int openin = 0; openin < openList.Count; openin++)
                        {
                            if (openList[openin] == checkNode)
                                {
                                    checkNodeisOpen = true;
                                    openin = openList.Count;
                                }
                        }
                    
                    if ( checkNodeisOpen == false )
                        {
                            openList.Add(checkNode);
                            checkNode.SetColour(NodeType.OpenListNode);

                            checkNode.g = ThisNode.g+1;
                            checkNode.parent = ThisNode;
                        }
                    else
                        {
                            Debug.Log("Naapuri on jo openlistassa" + checkNode.g + " vs " + ThisNode.g);
                        }

                    //h-arvon laskeminen manhattan metodilla:
                    float manhattanx = endNode.transform.position.x - checkNode.transform.position.x;
                    float manhattanz = endNode.transform.position.z - checkNode.transform.position.z;
                    //otetaan vain positiiviset arvot Mathf.Abs:n avulla
                    checkNode.h = Mathf.Abs(manhattanx) + Mathf.Abs(manhattanz); 
                    //f = g+h
                    checkNode.f = checkNode.g + checkNode.h;
                }


        }
    }
}
