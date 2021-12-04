using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewAbilityScript : MonoBehaviour
{
    public static List<int> newAbilityID;

    // Start is called before the first frame update
    void Start()
    {
        newAbilityID = new List<int>();
        newAbilityID.Add(1);
        newAbilityID.Add(2);
        newAbilityID.Add(3);
        newAbilityID.Add(6);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
