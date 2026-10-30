using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class DamageTextSpawner : Singleton<DamageTextSpawner>
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private DamageText damageTextPrefab;
    
    private List<DamageText> texts;

    protected override void Awake()
    {
        base.Awake();
        texts = new();
        Assert.IsNotNull(cameraTransform, $"{gameObject.name} 에 cameraTransform이 할당 되어야합니다.");
        Assert.IsNotNull(damageTextPrefab, $"{gameObject.name} 에 damageTextPrefab이 할당 되어야합니다.");
    }

    private DamageText GetText()
    {
        DamageText newText = PoolManager.Instance.Get(damageTextPrefab);
        texts.Add(newText);

        return newText;
    }

    public void Show(Vector3 position, int damage)
    {
        DamageText text = GetText();
        text.SetDamageText(damage, position);
    }

    private void LateUpdate()
    {
        float deltaTime = Time.deltaTime;

        Quaternion cameraRotation = cameraTransform.rotation;

        for (int i = texts.Count - 1; i >= 0; i--)
        {
            bool isEnd = texts[i].Tick(deltaTime, cameraRotation);

            if (isEnd)
            {
                int lastIndex = texts.Count - 1;
                (texts[i], texts[lastIndex]) = (texts[lastIndex], texts[i]);
                PoolManager.Instance.Release(texts[lastIndex]);
                texts.RemoveAt(lastIndex);
            }
        }
    }
}