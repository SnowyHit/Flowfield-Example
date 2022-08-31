using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CreateGrids : MonoBehaviour
{
    public int x;
    public int z;
    public float offset;
    public GameObject gridPrefab;
    public GameObject player;
    public Transform gridParent;
    public GridScript[,] gridReferences;
    public bool showBestCost;
    [SerializeField]Vector2Int clickedGridCoordinate = new Vector2Int(0,0);

    IEnumerator Start()
    {
        gridReferences = new GridScript[x, z];
        InitialiseGrids();
        yield return new WaitForEndOfFrame();
        OnGridClicked(new Vector2Int(0,0));
    }

    private void InitialiseGrids()
    {
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < z; j++)
            {
                GameObject tempGo = Instantiate(gridPrefab, gridParent);
                tempGo.name = "Grid[" + i + "," + j + "]";
                tempGo.transform.position = new Vector3(i * offset, 0, j * offset);
                GridScript tempGS = tempGo.GetComponent<GridScript>();
                gridReferences[i, j] = tempGS;
                tempGS.coordinates = new Vector2Int(i, j);
                tempGS.parent = this;
                if (i - 1 >= 0)
                    tempGS.neighboorGrids[0, 0] = new Vector2Int(i - 1, j);
                else
                    tempGS.neighboorGrids[0, 0] = new Vector2Int(-1, -1);
                if (i + 1 < x)
                    tempGS.neighboorGrids[1, 0] = new Vector2Int(i + 1, j);
                else
                    tempGS.neighboorGrids[1, 0] = new Vector2Int(-1, -1);
                if (j - 1 >= 0)
                    tempGS.neighboorGrids[1, 1] = new Vector2Int(i, j - 1);
                else
                    tempGS.neighboorGrids[1, 1] = new Vector2Int(-1, -1);
                if (j + 1 < z)
                    tempGS.neighboorGrids[0, 1] = new Vector2Int(i, j + 1);
                else
                    tempGS.neighboorGrids[0, 1] = new Vector2Int(-1, -1);

                if (j + 1 < z && i + 1 < x)
                    tempGS.neighboorGrids[2, 0] = new Vector2Int(i + 1, j + 1);
                else
                    tempGS.neighboorGrids[2, 0] = new Vector2Int(-1, -1);

                if (j + 1 < z && i-1 >= 0 )
                    tempGS.neighboorGrids[2, 1] = new Vector2Int(i -1 , j + 1);
                else
                    tempGS.neighboorGrids[2, 1] = new Vector2Int(-1, -1);

                if (j - 1 >= 0 && i-1 >= 0)
                    tempGS.neighboorGrids[3, 0] = new Vector2Int(i-1, j - 1);
                else
                    tempGS.neighboorGrids[3, 0] = new Vector2Int(-1, -1);

                if (j - 1 >= 0 && i + 1 < x)
                    tempGS.neighboorGrids[3, 1] = new Vector2Int(i+1, j - 1);
                else
                    tempGS.neighboorGrids[3, 1] = new Vector2Int(-1, -1);
            }
        }

    }
    public void OnGridClicked(Vector2Int coordinate)
    {
        if(clickedGridCoordinate.x != -1)
        {
            gridReferences[clickedGridCoordinate.x, clickedGridCoordinate.y].clicked = false;
        }
        clickedGridCoordinate = coordinate;
        gridReferences[coordinate.x, coordinate.y].ColorChange(Color.cyan);
        foreach (var item in gridReferences)
        {
            item.bestCost = ushort.MaxValue;
            if(item.cost == 0)
            {
                item.cost = 1;
            }
        }

        CreateIntegrationField();
    }

    public void CreateIntegrationField()
    {
        gridReferences[clickedGridCoordinate.x, clickedGridCoordinate.y].cost = 0;
        gridReferences[clickedGridCoordinate.x, clickedGridCoordinate.y].bestCost = 0;

        Queue<GridScript> gridsToCheck = new Queue<GridScript>();

        gridsToCheck.Enqueue(gridReferences[clickedGridCoordinate.x, clickedGridCoordinate.y]);

        while (gridsToCheck.Count > 0)
        {
            GridScript curGrid = gridsToCheck.Dequeue();
            List<GridScript> gridScripts = new List<GridScript>();
            foreach (var neighbor in curGrid.neighboorGrids)
            {
                if(neighbor.x != -1)
                    gridScripts.Add(gridReferences[neighbor.x, neighbor.y]);
            }

            foreach (var item in gridScripts)
            {
                if(item.cost == byte.MaxValue) { continue; }
                if(item.cost + curGrid.bestCost < item.bestCost)
                {
                    item.bestCost =(ushort)(item.cost + curGrid.bestCost);
                    gridsToCheck.Enqueue(item);
                }
            }
        }
        GenerateFlowField();
    }

    public void GenerateFlowField()
    {
        foreach (var item in gridReferences)
        {
            int bestCost = item.bestCost;

            foreach (var neighbor in item.neighboorGrids)
            {
                if(neighbor.x != -1 && gridReferences[neighbor.x , neighbor.y].bestCost < bestCost)
                {
                    item.bestNeighbor = neighbor;
                    bestCost = gridReferences[neighbor.x, neighbor.y].bestCost;
                }
            }
        }
    }

    
}
