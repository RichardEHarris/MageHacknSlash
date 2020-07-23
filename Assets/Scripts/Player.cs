using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private MasterGameController GC;
    float health;
    public float maxHealth = 100;
    float mana;
    public float maxMana = 100;
    public Ability[] abilities = new Ability[4];
    public float healthRegen;
    public float manaRegen;
    public float cooldownReduction;
    public float manaCostReduction;

    void Awake()
    {
        GC = FindObjectOfType<MasterGameController>();
        for (int i = 0; i < abilities.Length; i++)
        {
            if (abilities[i])
            {
                abilities[i] = Instantiate(abilities[i], transform.position, new Quaternion(), transform);
            }
        }
    }

    void Start()
    {
        health = maxHealth;
        mana = maxMana;
    }
}
