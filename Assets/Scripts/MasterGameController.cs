using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterGameController : MonoBehaviour
{
    public Player player;
    public AbilityList spellBook;
    void Awake()
    {
        //player = GetComponentInChildren<Player>();
        spellBook = GetComponentInChildren<AbilityList>();
    }

    void Start()
    {
        
    }
}
