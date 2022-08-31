using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridScript : MonoBehaviour
{
    public float range;
    public float depth;
    [HideInInspector]public Material mat;
    [HideInInspector]public Vector2Int coordinates;
    [HideInInspector]public CreateGrids parent;
    [HideInInspector]public bool clicked = false;
    public ushort bestCost;
    public Vector2Int[,] neighboorGrids = new Vector2Int[4, 2];
    public Vector2Int bestNeighbor;
    [Range(0, 255)]
    public byte cost; 
    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<MeshRenderer>().material;
        bestCost = ushort.MaxValue;
    }

    // Update is called once per frame
    void Update()
    {
        if(!clicked)
        {
            if (Vector3.Distance(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, depth)), transform.position) < range)
            {

                mat.color = Color.red;
                if (Input.GetMouseButtonDown(0))
                {
                    
                    parent.OnGridClicked(coordinates);
                    clicked = true;
                }
            }
            else
            {
                if(parent.showBestCost)
                    mat.color = new Color(0, bestCost / 3529f, bestCost / 3529f);
                else
                    mat.color = new Color(cost/255f , cost / 255f, cost / 255f);
            }
        }
    }

    public void ColorChange(Color color)
    {
        mat.color = color;
    }
    public void ShowNeighbors()
    {
        foreach (var item in neighboorGrids)
        {
            Debug.Log(this.name +" : " + item);
        }
    }
}
