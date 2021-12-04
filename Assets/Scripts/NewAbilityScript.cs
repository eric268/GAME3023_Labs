using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewAbilityScript : MonoBehaviour
{
    public static List<int> newAbilityID;

    public static void GenerateAbilityListPlayerDoesntHave()
    {
        newAbilityID = new List<int>();
        int abilitiesFound = 0;

        while (abilitiesFound < 4)
        {
            int randomAbilityID = Random.Range(0, EncounterAbilities.totalNumberOfAbilities);
            
            if (!PlayerAttributes.playerAbilityIDs.Contains(randomAbilityID) && !newAbilityID.Contains(randomAbilityID))
            {
                newAbilityID.Add(randomAbilityID);
                abilitiesFound++;
            }
        }
    }
}
