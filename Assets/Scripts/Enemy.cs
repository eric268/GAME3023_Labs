using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemy", menuName = "EnemyCreater/Enemy")]
public class Enemy : ScriptableObject
{
    public new string name = "Enemy";
    public Sprite spriteIcon;
    public int health;
    public float Intelligence;
    public int xpGiven;
}
