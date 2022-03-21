using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    //list of SpawnPoints
    public GameObject[] cells;

    //list of spawnable objects
    public GameObject fighterParent;
    //public GameObject soldier;

    #region //METHODS
    //method to delay damage calculation until attack animation is completed
    private IEnumerator DamageCalculation(GameObject unit1, GameObject unit2)
    {
        if (unit1.GetComponent<Animator>() != null && unit2.GetComponent<Animator>() != null)
        {
            //assign script variables for ease of use
            Animator animator1 = unit1.GetComponent<Animator>();
            Animator animator2 = unit2.GetComponent<Animator>();
            FighterParent fighterParent1 = unit1.GetComponent<FighterParent>();
            FighterParent fighterParent2 = unit2.GetComponent<FighterParent>();
            RuntimeAnimatorController ac = unit1.GetComponent<Animator>().runtimeAnimatorController;

            //play fight animations
            switch(unit1.tag)
            {
                case "Ally Unit":
                    animator1.Play("Ally Attacking");
                    break;
                case "Enemy Unit":
                    animator1.Play("Enemy Attacking");
                    break;
            }
            switch(unit2.tag)
            {
                case "Ally Unit":
                    animator2.Play("Ally Attacking");
                    break;
                case "Enemy Unit":
                    animator2.Play("Enemy Attacking");
                    break;
            }
            fighterParent1.isAnimating = true;
            fighterParent2.isAnimating = true;

            //calculate attack animation length and wait for it to finish before calculating damage
            float time = 0;
            for (int i = 0; i < ac.animationClips.Length; i++)
            {
                if (ac.animationClips[i].name == "AllyAttackAnimation")
                {
                    time = ac.animationClips[i].length;
                }
            }
            yield return new WaitForSeconds(time);
            fighterParent1.isAnimating = false;
            fighterParent2.isAnimating = false;

            //calculate damage and destroy killed units
            fighterParent1.armor -= fighterParent2.strength;
            fighterParent2.armor -= fighterParent1.strength;
            if (fighterParent1.armor <= 0)
            {
                DestroyUnit(unit1);
            }
            if (fighterParent2.armor <= 0)
            {
                DestroyUnit(unit2);
            }
            Cleanup();
        }
    }

    //method to resolve the cleanup of a Fight
    public void Cleanup()
    {
        foreach (GameObject cell in cells)
        {
            //if unit exists in this cell
            if(cell.GetComponent<CellScript>().Unit != null)
            {
                //find how far forward a cell needs to move
                GameObject cellToMove = cell;
                while (cellToMove.GetComponent<CellScript>().Next != null && cellToMove.GetComponent<CellScript>().Next.GetComponent<CellScript>().IsOccupied == false)
                {
                    cellToMove = cellToMove.GetComponent<CellScript>().Next;
                }
                cell.GetComponent<CellScript>().Unit.GetComponent<FighterParent>().Move(cell.GetComponent<CellScript>().Unit, cellToMove);
            }
        }
    }

    //method to destroy a unit properly
    public void DestroyUnit(GameObject unit)
    {
        //null check
        if (unit.GetComponent<FighterParent>() != null && unit.GetComponent<FighterParent>().occupiedCell.GetComponent<CellScript>() != null)
        {
            //assign script variables for ease of use
            CellScript cellScript = unit.GetComponent<FighterParent>().occupiedCell.GetComponent<CellScript>();

            cellScript.IsOccupied = false;
            cellScript.Unit = null;
            Destroy(unit);
        }
        else
        {
            Debug.Log("Unit or its cell have no script in a Destroy command");
        }
    }

    //method to calculate a fight between 2 units
    public void Fight(GameObject unit1, GameObject unit2)
    {
        //null check
        if (unit1.GetComponent<FighterParent>() != null && unit2.GetComponent<FighterParent>() != null && unit1.GetComponent<FighterParent>().occupiedCell.GetComponent<CellScript>() != null && unit2.GetComponent<FighterParent>().occupiedCell.GetComponent<CellScript>() != null)
        {
            StartCoroutine(DamageCalculation(unit1, unit2));
        }
        else
        {
            Debug.Log("One of the units or their cells passed into the Fight method have no script");
        }
    }

    //method to spawn a unit
    public void SpawnUnit(int cell, GameObject unit)
    {
        //null check
        if (cells[cell].GetComponent<CellScript>() != null && unit.GetComponent<FighterParent>() != null)
        {
            //assign script variable for ease of use
            CellScript cellscript = cells[cell].GetComponent<CellScript>();

            //make sure there is no object currently in the call
            if (cellscript.IsOccupied == false)
            {
                //spawn unit and occupy cell
                GameObject newUnit = Instantiate(unit, cells[cell].transform.position, Quaternion.identity);
                cellscript.Unit = newUnit;
                cellscript.IsOccupied = true;
                newUnit.GetComponent<FighterParent>().occupiedCell = cells[cell];
            }
        }
        else
        {
            Debug.Log("Cell or unit have no script in Spawn command");
        }
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        #region //test fighting
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (cells[0].GetComponent<CellScript>().IsOccupied == true && cells[6].GetComponent<CellScript>().IsOccupied == true)
            {
                Fight(cells[0].GetComponent<CellScript>().Unit, cells[6].GetComponent<CellScript>().Unit);
            }
        }
        #endregion

        #region //test spawning
        //create test object in position 1C-S
        if (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Alpha1))
        {
            SpawnUnit(0, fighterParent);
        }
        //create test object in position 2C-S
        if (Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Alpha2))
        {
            SpawnUnit(1, fighterParent);
        }
        //create test object in position 3C-S
        if (Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.Alpha3))
        {
            SpawnUnit(2, fighterParent);
        }
        //create test object in position 4C-S
        if (Input.GetKeyDown(KeyCode.Keypad4) || Input.GetKeyDown(KeyCode.Alpha4))
        {
            SpawnUnit(3, fighterParent);
        }
        //create test object in position 5C-S
        if (Input.GetKeyDown(KeyCode.Keypad5) || Input.GetKeyDown(KeyCode.Alpha5))
        {
            SpawnUnit(4, fighterParent);
        }
        //create test object in position 6C-S
        if (Input.GetKeyDown(KeyCode.Keypad6) || Input.GetKeyDown(KeyCode.Alpha6))
        {
            SpawnUnit(5, fighterParent);
        }
        //create test object in position 1C-E
        if (Input.GetKeyDown(KeyCode.Keypad7) || Input.GetKeyDown(KeyCode.Alpha7))
        {
            SpawnUnit(6, fighterParent);
        }
        //create test object in position 2C-E
        if (Input.GetKeyDown(KeyCode.Keypad8) || Input.GetKeyDown(KeyCode.Alpha8))
        {
            SpawnUnit(7, fighterParent);
        }
        //create test object in position 3C-E
        if (Input.GetKeyDown(KeyCode.Keypad9) || Input.GetKeyDown(KeyCode.Alpha9))
        {
            SpawnUnit(8, fighterParent);
        }
        //create test object in position 4C-E
        if (Input.GetKeyDown(KeyCode.Keypad0) || Input.GetKeyDown(KeyCode.Alpha0))
        {
            SpawnUnit(9, fighterParent);
        }
        //create test object in position 5C-E
        if (Input.GetKeyDown(KeyCode.KeypadMinus) || Input.GetKeyDown(KeyCode.Minus))
        {
            SpawnUnit(10, fighterParent);
        }
        //create test object in position 6C-E
        if (Input.GetKeyDown(KeyCode.KeypadPlus) || Input.GetKeyDown(KeyCode.Equals))
        {
            SpawnUnit(11, fighterParent);
        }
        #endregion
    }
}
