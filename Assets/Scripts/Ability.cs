using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour
{
    public enum AbilityType { Action, Spell }
    public enum Aspect { Heat, Cold, Air, Earth, Entropy, Order, Dark, Light }

    public AbilityType Type = AbilityType.Action;
    public Aspect[] Aspects;
    public string abilityName = "Ability Name";
    public string id = "AspectsType1";

    public float damage = 0;
    public float range = 0;
    public int manaCost = 0;
    public float cooldown = 1;

    public GameObject abilityEffect;
    public GameObject projectile;
    private Projectile pScript;

    void Start()
    {
        projectile = Instantiate(projectile, transform.position, transform.rotation, transform);
        pScript = projectile.GetComponent<Projectile>();
        pScript.graphic = abilityEffect;
        Debug.Log("Loaded Ability: " + abilityName);
    }

    public Ability(AbilityType type, Aspect[] aspects, string name, string id, float damage, float range, int manaCost)
    {
        Type = type;
        Aspects = aspects;
        this.abilityName = name;
        this.id = id;
        this.damage = damage;
        this.range = range;
        this.manaCost = manaCost;
    }

    public void playEffect()
    {
        pScript.Play();
    }

    public float GetDamage()
    {
        return damage;
    }

    public float GetRange()
    {
        return range;
    }

    public float GetManaCost()
    {
        return manaCost;
    }
}
