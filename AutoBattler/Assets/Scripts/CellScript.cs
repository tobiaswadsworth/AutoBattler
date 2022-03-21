using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellScript : MonoBehaviour
{
    public bool IsOccupied = false; //tells whether a cell is currently occupied by a unit
    public GameObject Unit = null; //current unit in this cell
    public GameObject Next = null; //the next cell (closer to the center) if one exists
    public GameObject Previous = null; //the previous cell (further from the center) if one exists

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
