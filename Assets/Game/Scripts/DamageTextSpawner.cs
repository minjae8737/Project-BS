using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class DamageTextSpawner : Singleton<DamageTextSpawner>
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private GameObject damageTextPrefab;
    
    private List<TextMeshPro> texts;

    protected override void Awake()
    {
        base.Awake();
        texts = new();
        Assert.IsNotNull(cameraTransform, $"{gameObject.name} 에 cameraTransform이 할당 되어야합니다.");
    }

    private TextMeshPro GetText()
    {
        GameObject newTextObj = Instantiate(damageTextPrefab, transform);
        TextMeshPro newText = newTextObj.GetComponent<TextMeshPro>();
        texts.Add(newText);

        return newText;
    }

    public void Show(Vector3 position, int damage)
    {
        TextMeshPro text = GetText();
        text.SetText(damage + "");
        text.transform.position = position;
    }
}