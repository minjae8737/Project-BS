using System;
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