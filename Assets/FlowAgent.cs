using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowAgent : MonoBehaviour
{
    [SerializeField] CreateGrids gridsScriptRef;
    [SerializeField]Vector2Int startingPoint;
    GridScript curGrid;
    private IEnumerator Start()
    {
        startingPoint = new Vector2Int(Random.Range(0 , gridsScriptRef.x) , Random.Range(0, gridsScriptRef.z));
        yield return new WaitForFixedUpdate();
        curGrid = gridsScriptRef.gridReferences[startingPoint.x, startingPoint.y];
        transform.position = curGrid.transform.position + Vector3.up;
        StartCoroutine(Move());
    }
    IEnumerator Move()
    {
        while(true)
        {
            yield return new WaitForEndOfFrame();
            if (curGrid.cost != 0)
            {
                transform.position = gridsScriptRef.gridReferences[curGrid.bestNeighbor.x, curGrid.bestNeighbor.y].transform.position + Vector3.up;
                curGrid = gridsScriptRef.gridReferences[curGrid.bestNeighbor.x, curGrid.bestNeighbor.y];
            }
            
            yield return new WaitForSeconds(0.5f);
        }
        
    }
    //RVO
}
