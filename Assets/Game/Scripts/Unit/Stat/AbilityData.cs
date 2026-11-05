using UnityEngine;

public class AbilityData : ScriptableObject
{
    [SerializeField] private string displayName;
    [SerializeField] private Sprite icon;
    [SerializeField, TextArea, Tooltip("어빌리티 설명")] private string description;
    
    protected static int maxLevel = 5;
    public string DisplayName => displayName;
    public Sprite Icon => icon;
    public string Description => description;
}