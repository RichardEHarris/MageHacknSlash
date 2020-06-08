using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    protected float health;
    protected float mana;
    protected Ability[] abilities = new Ability[4];
    float healthRegen;
    float manaRegen;
    float cooldownReduction;
    float manaCostReduction;
}
