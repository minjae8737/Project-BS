using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/Catalog")]
public class AbilityCatalogData : ScriptableObject
{
    [SerializeField] private List<AbilityData> abilities;

    public IReadOnlyList<AbilityData> Abilities => abilities;
    
    private void OnValidate()
    {
        ValidateData();
    }

    public void ValidateData()
    {
        HashSet<AbilityData> hashSet = new();

        for (int i = 0; i < abilities.Count; i++)
        {
            if (abilities[i] == null)
                Debug.LogError($"[AbilityCatalogData] {i} 인덱스 데이터가 비어있습니다.");
            else
            {
                if (!hashSet.Add(abilities[i]))
                {
                    Debug.LogError($"[AbilityCatalogData] {i} 인덱스 데이터 중복입니다.");
                }
            }
        }
    }
}