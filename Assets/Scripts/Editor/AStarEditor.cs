using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Mono.Cecil;
using System.Runtime.InteropServices.ComTypes;
using NUnit.Framework;

public class AStarEditor : MonoBehaviour
{
    [MenuItem("AStar/Generate Grid")]

    //ruudukkometodi:
    static void GenerateGrid()
    {
        for (int x = 0; x < 10; x++)
        {
            for (int i = 0; i < 10; i++)
            {
                Debug.Log("Forloop" + i);
                GameObject newNode = Resources.Load<GameObject>("Node");
                GameObject nodeInstance = Instantiate(newNode);
                nodeInstance.transform.name = "Node_" + i + "_" + x;
                nodeInstance.transform.position = new Vector3(0f+x, 0f, 0f+i);
            }
        }
    }

    //poistaminen:
    [MenuItem("AStar/Delete Grid")]
    static void DeleteGrid()
        {
        GameObject[] allNodes = GameObject.FindGameObjectsWithTag("AStarNode");

        for (int i = 0; i < allNodes.Length; i++)
        {
            GameObject destroyable = allNodes[i];
            DestroyImmediate(destroyable);
        }
    }

    [MenuItem("AStar/Check Barriers")]
    static void CheckBarriers()
    {
        GameObject[] allNodes = GameObject.FindGameObjectsWithTag("AStarNode");

        for (int i = 0; i < allNodes.Length; i++)
        {
            GameObject checkBarrierNode = allNodes[i];
            AstrNode nodescript = checkBarrierNode.GetComponent<AstrNode>();
            //tarkastetaan onko yläpuolella nodea estettä
            //rh = raycast hit
            RaycastHit rh = new RaycastHit();
            if (Physics.Raycast(checkBarrierNode.transform.position, Vector3.up, out rh ))
            {
                Debug.Log("Osuttiin: " + rh.collider.name);
                nodescript.isNotWalkable = true; 
                nodescript.SetColour(NodeType.unwalkableNodeColour);
            }

            if (nodescript.startpoint == true)
            {
                Debug.Log("Alkupiste löydetty");
                nodescript.SetColour(NodeType.startpoint);
            }

            if (nodescript.endpoint == true)
            {
                Debug.Log("Loppupiste löydetty");
                nodescript.SetColour(NodeType.endpoint);
            }
        }
    }

    [MenuItem("AStar/Search Neighbors")]
    static void SearchNeighbors()
    {
        GameObject[] allNodes = GameObject.FindGameObjectsWithTag("AStarNode");

        for (int i = 0; i < allNodes.Length; i++)
        {
            GameObject thisNode = allNodes[i];
            AstrNode nodescript = thisNode.GetComponent<AstrNode>();

            nodescript.neighborNodes.Clear();

            for (int j = 0; j < allNodes.Length; j++)
                {
                GameObject potentialNeighbor = allNodes[j];
                AstrNode neighborScript = potentialNeighbor.GetComponent<AstrNode>();

                //varmistetaan ettei liätä itseään naapureihin
                if (thisNode == potentialNeighbor) continue;

                //tarkistetaan nodejen välinen etäisyys:
                float distance = Vector3.Distance(thisNode.transform.position,
                    potentialNeighbor.transform.position);

                //jos etäisyys on pienempi kuin 1.8, niin silloin kyseessä on naapuri
                if (distance < 1.8f)
                    {
                    //lisätään naapuri listaan
                    nodescript.neighborNodes.Add(neighborScript);
                    Debug.Log(thisNode.name + " Naapuri lisätty: " + potentialNeighbor.name);
                    }
                }

        }
    }

}

