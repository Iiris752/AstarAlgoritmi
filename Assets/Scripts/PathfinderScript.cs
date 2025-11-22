using System.Collections;
using UnityEngine;

public class PathfinderScript : MonoBehaviour
{
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
        yield return new WaitForSeconds(5f);

        

    }
}
