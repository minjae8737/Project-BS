using UnityEngine;

[CreateAssetMenu(menuName = "Skill/FiringMode/Fan")]
public class FanFiringModeData : FiringModeData
{
    [SerializeField] private float gapAngle;

    public float GapAngle => gapAngle;

    public override FiringMode CreateFiringMode(IProjectileLauncher projectileLauncher)
    {
        return new FanFiringMode(this, projectileLauncher); 
    }
}