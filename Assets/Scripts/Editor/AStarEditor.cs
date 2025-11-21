using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Mono.Cecil;

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
    static async void DeleteGrid()
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

                nodescript.SetColourUnwalkable();
            }

            if (nodescript.startpoint == true)
            {
                Debug.Log("Alkupiste löydetty");
                nodescript.SetColourStartpoint();
            }

            if (nodescript.endpoint == true)
            {
                Debug.Log("Loppupiste löydetty");
                nodescript.SetColourEndpoint();
            }
        }

    }
}

