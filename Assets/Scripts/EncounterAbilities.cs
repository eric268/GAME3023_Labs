using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Linq;
using System.Text.RegularExpressions;

public enum AbilityType
{
    NONE,
    NORMAL,
    FIGHTING,
    SHADOW,
    FIRE,
    EARTH,
    WIND,
    WATER,
    ROCK,
    NUM_ABILITY_TYPES,
};


public class EncounterAbilities : MonoBehaviour
{
    public static List<Ability> abilityList;

    public static int totalNumberOfAbilities = 16;

    public void Start()
    {
        LoadAbilities();
    }
    public static void LoadAbilities()
    {
        TextAsset abilityDoc = (TextAsset)Resources.Load("Abilities", typeof(TextAsset));
        if (abilityDoc)
        {
            abilityList = new List<Ability>();
            string abilityDocText = abilityDoc.text;
            string[][] abilityTextLine = abilityDocText.Split(new string[] { "\n" }, StringSplitOptions.None).Select(x => x.Split(',')).ToArray();

            for (int i = 0; i < abilityTextLine.Length; i++)
            {
                for (int j = 0; j < abilityTextLine[j].Length; j++)
                {
                    int idNumber = int.Parse(abilityTextLine[j][0]);
                    string name = abilityTextLine[j][1];
                    string description = abilityTextLine[j][2];
                    int damage = int.Parse(abilityTextLine[j][3]);
                    float critChance = float.Parse(abilityTextLine[j][4]);
                    float accuracy = float.Parse(abilityTextLine[j][5]);
                    AbilityType type = (AbilityType)(int.Parse(abilityTextLine[j][6]));
                    float accuracyAfflication = float.Parse(abilityTextLine[j][7]);
                    Ability ability = new Ability(idNumber, name, description, damage, critChance, accuracy, type, accuracyAfflication);
                    abilityList.Add(ability);
                }
            }

        }
        else
            Debug.LogError("Could not find Abilities text file");

    }
}

public class Ability
{
    public int iDNumber;
    public string abilityName;
    public string description;
    public int damage;
    public float critChance;
    public float accuracy;
    public AbilityType abilityType;
    public float accuraccyAfflication;

    public Ability()
    {
        iDNumber = -1;
        abilityName = "";
        description = "";
        damage = -1;
        critChance = -1;
        accuracy = -1;
        abilityType = ConvertIntToAbilityType(0);
        accuraccyAfflication = 0;
    }

    public Ability(int id, string n, string d, int dam, float crit, float acc, AbilityType type, float accuraccyAffl)
    {
        iDNumber = id;
        abilityName = n;
        description = d;
        damage = dam;
        critChance = crit;
        accuracy = acc;
        abilityType = type;
        accuraccyAfflication = accuraccyAffl;
    }

    public static Ability CopyAbility(Ability ability)
    {
        return new Ability(ability.iDNumber, ability.abilityName, ability.description, ability.damage, ability.critChance, ability.accuracy, ability.abilityType, ability.accuraccyAfflication);
    }
    public static AbilityType ConvertIntToAbilityType(int val)
    {
        switch(val)
        {
            case -1:
                return AbilityType.NONE;
            case 0:
                return AbilityType.NORMAL;
            case 1:
                return AbilityType.FIGHTING;
            case 2:
                return AbilityType.SHADOW;
            case 3:
                return AbilityType.FIRE;
            case 4:
                return AbilityType.EARTH;
            case 5:
                return AbilityType.WIND;
            case 6:
                return AbilityType.WATER;
            case 7:
                return AbilityType.ROCK;
        }
        return AbilityType.NONE;
    }
}


