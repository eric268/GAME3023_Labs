using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class EncounterAbilities : MonoBehaviour
{
    public static List<Ability> abilityList;

    public void Start()
    {
        LoadAbilities();
    }
    public static void LoadAbilities()
    {
        string path = Application.dataPath + "/TextDocs/Abilities.txt";
        if (!File.Exists(path))
        {
            File.WriteAllText(path, "");
        }

        if (File.Exists(path))
        {
            abilityList = new List<Ability>();
            StreamReader sr = new StreamReader(path);
            string line = "";

            while ((line = sr.ReadLine()) != null)
            {
                string[] arr = line.Split(',');
                int idNumber = int.Parse(arr[0]);
                string name = arr[1];
                string description = arr[2];
                float damage = int.Parse(arr[3]);
                float critChance = float.Parse(arr[4]);
                float accuracy = float.Parse(arr[5]);
                Ability ability = new Ability(idNumber, name, description, damage, critChance, accuracy);
                abilityList.Add(ability);
            }
        }

    }
}

public class Ability
{
    public int iDNumber;
    public string abilityName;
    public string description;
    public float damage;
    public float critChance;
    public float accuracy;

    public Ability()
    {
        iDNumber = -1;
        abilityName = "";
        description = "";
        damage = -1;
        critChance = -1;
        accuracy = -1;
    }

    public Ability(int id, string n, string d, float dam, float crit, float acc)
    {
        iDNumber = id;
        abilityName = n;
        description = d;
        damage = dam;
        critChance = crit;
        accuracy = acc;
    }

    public static Ability CopyAbility(Ability ability)
    {
        return new Ability(ability.iDNumber, ability.abilityName, ability.description, ability.damage, ability.critChance, ability.accuracy);
    }
}


