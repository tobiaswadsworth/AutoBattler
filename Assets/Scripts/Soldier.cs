using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : FighterParent
{
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        //initialize default values
        strength = 3;
        armor = 3;
        unit = "Soldier";
    }

    // Update is called once per frame
    void Update()
    {

    }
}
