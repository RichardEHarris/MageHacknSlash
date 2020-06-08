using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private MasterGameController GC;
    public float health;
    public float mana;
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
                abilities[i] = Instantiate(abilities[i], new Vector3(), new Quaternion(), transform);
            }
        }
    }
}
