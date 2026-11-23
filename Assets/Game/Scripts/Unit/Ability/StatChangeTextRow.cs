using System;
using UnityEngine;

[Serializable]
public class StatChangeTextRow<TEnum> : IEnumRow<TEnum> where TEnum : struct, Enum
{
    [SerializeField] private string typeName;
    [SerializeField, HideInInspector] private TEnum type;
    [SerializeField, TextArea, Tooltip("{0} 입력값(10 = 10%), {1} 변화량, {2} 현재, {3} 적용 후.")] private string format;
 
    public string TypeName => typeName;
    public TEnum Type => type;
    public string Format => format;

    public StatChangeTextRow() { }

    public StatChangeTextRow(TEnum type)
    {
        this.type = type;
    }

    public void RefreshName()
    {
        typeName = type.ToString();
    }
}