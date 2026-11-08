using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RewardOptionGenerator
{
    [SerializeField] private AbilityCatalogData catalog;
    
    public void Init()
    {
        catalog.ValidateData();
    }
}