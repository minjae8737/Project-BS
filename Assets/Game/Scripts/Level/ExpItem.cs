using UnityEngine;

public enum ExpItemState
{
    Idle,
    PickUp,
}

public class ExpItem : MonoBehaviour
{
    private int exp;

    public void Init(Vector3 position, int exp)
    {
        transform.position = position;
        this.exp = exp;
    }
    
    public void Tick()
    {
        
    }
}