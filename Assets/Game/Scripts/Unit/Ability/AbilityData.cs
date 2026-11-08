using UnityEngine;

public enum AbilityType
{
    Skill,
    Passive
}

public abstract class AbilityData : ScriptableObject
{
    [SerializeField] private string displayName;
    [SerializeField] private Sprite icon;
    [SerializeField, TextArea, Tooltip("어빌리티 설명")] private string description;

    public abstract AbilityType Type { get; }
    public const int MaxLevel = 5;
    public string DisplayName => displayName;
    public Sprite Icon => icon;
    public string Description => description;
}