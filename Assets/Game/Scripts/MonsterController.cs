using System;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    private PlayerMovement movement;
    private UnitStat stat;

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        stat = GetComponent<UnitStat>();
    }
    
    
}