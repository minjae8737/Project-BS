using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class DamageTextSpawner : Singleton<DamageTextSpawner>
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private GameObject damageTextPrefab;
    
    [SerializeField] private List<DamageText> texts;

    protected override void Awake()
    {
        base.Awake();
        texts = new();
        Assert.IsNotNull(cameraTransform, $"{gameObject.name} 에 cameraTransform이 할당 되어야합니다.");
    }

    private DamageText GetText()
    {
        GameObject newTextObj = Instantiate(damageTextPrefab, transform);
        DamageText newText = newTextObj.GetComponent<DamageText>();
        texts.Add(newText);

        return newText;
    }

    public void Show(Vector3 position, int damage)
    {
        DamageText text = GetText();
        text.SetDamageText(damage, position);
        text.gameObject.SetActive(true);
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
                (texts[i], texts[texts.Count - 1]) = (texts[texts.Count - 1], texts[i]);
                //TODO PoolManager에 반납 코드추가
                texts.RemoveAt(texts.Count - 1);
            }
        }
    }
}