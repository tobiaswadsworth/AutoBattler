using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FighterParent : MonoBehaviour, IPointerClickHandler
{
    //initialize variables
    public string unit = "Default"; //name of unit
    public int strength = 1; //physical attack of unit
    public Text strengthText; //Text element displaying strength
    public int armor = 1; //physical defense of unit
    public Text armorText; //Text element displaying armor
    public GameObject occupiedCell = null; //the cell the unit occupies
    public Animator animator; //animator for the animations
    public bool isAnimating = false; //indicates if a unit is animating

    //method to shift a unit through cells
    public void Move(GameObject unit, GameObject cell)
    {
        //null check
        if(unit.GetComponent<FighterParent>() != null && cell.GetComponent<CellScript>() != null)
        {
            //assign Script variables for ease of use
            CellScript cellScript = cell.GetComponent<CellScript>();
            FighterParent fighterParent = unit.GetComponent<FighterParent>();

            //make sure cell moved to is not currently occupied
            if (cellScript.IsOccupied == false)
            {
                //remove the unit from its current cell
                fighterParent.occupiedCell.GetComponent<CellScript>().IsOccupied = false;
                fighterParent.occupiedCell.GetComponent<CellScript>().Unit = null;

                //place the unit in the destination cell
                cellScript.Unit = unit;
                cellScript.IsOccupied = true;
                fighterParent.occupiedCell = cell;
            }
        }
        else
        {
            Debug.Log("Move command unit or cell have no script");
        }
    }

    // Start is called before the first frame update
    public void Start()
    {
        //initialize variables
        if(transform.position.x <= 0)
        {
            tag = "Ally Unit";
        }
        else //transform.position.x > 0
        {
            tag = "Enemy Unit";
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //moveTowards occupiedCell if not already there (or reasonably close)
        if(transform.position != occupiedCell.transform.position && isAnimating == false && Vector3.Distance(transform.position, occupiedCell.transform.position) >= 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, occupiedCell.transform.position, 0.1f);
        }

        //manage texts
        strengthText.text = "" + strength;
        armorText.text = "" + armor;

        //movement (debug)
        float xDirection = Input.GetAxis("Horizontal");
        float yDirection = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(xDirection, yDirection, 0.0f);
        transform.position += moveDirection * 0.1f;
    }

    #region //test stats and movement
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            strength++;
        }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            armor++;
        }
        //if (eventData.button == PointerEventData.InputButton.Left)
        //{
        //    Move(this.gameObject, this.GetComponent<FighterParent>().occupiedCell.GetComponent<CellScript>().Next);
        //    Debug.Log("Move Next Attempted");
        //}
        //if (eventData.button == PointerEventData.InputButton.Right)
        //{
        //    Move(this.gameObject, this.GetComponent<FighterParent>().occupiedCell.GetComponent<CellScript>().Previous);
        //    Debug.Log("Move Previous Attempted");
        //}
    }
    #endregion
}
