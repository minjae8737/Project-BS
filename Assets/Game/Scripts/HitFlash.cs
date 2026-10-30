using UnityEngine;
using UnityEngine.Assertions;

public class HitFlash : MonoBehaviour
{
    [SerializeField] private Material flashMaterial;
    [SerializeField, Min(0.1f)] private float duration = 0.2f;
    
    private Renderer[] targetRenderers;
    private Material[] originMaterials;

    private float elapsed;
    
    private void Awake()
    {
        Assert.IsNotNull(flashMaterial, $"{name}의 HitFlash에 flashMaterial을 할당해야합니다.");
        
        targetRenderers = GetComponentsInChildren<Renderer>();
        originMaterials = new Material[targetRenderers.Length];
        
        for (int i = 0; i < targetRenderers.Length; i++)
        {
            originMaterials[i] = targetRenderers[i].sharedMaterial;
        }
        
        enabled = false;
    }

    private void Update()
    {
        elapsed += Time.deltaTime;

        if (duration < elapsed)
            Stop();
    }

    public void Play()
    {
        for (int i = 0; i < targetRenderers.Length; i++)
        {
            targetRenderers[i].sharedMaterial = flashMaterial;
        }
        
        elapsed = 0f;
        
        enabled = true;
    }


    public void Stop()
    {
        for (int i = 0; i < targetRenderers.Length; i++)
        {
            targetRenderers[i].sharedMaterial = originMaterials[i];
        }
        
        enabled = false;
    }
}