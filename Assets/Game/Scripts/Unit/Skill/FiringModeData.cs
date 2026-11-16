using UnityEngine;

public abstract class FiringModeData : ScriptableObject
{
    public abstract FiringMode CreateFiringMode(IProjectileLauncher projectileLauncher);
}