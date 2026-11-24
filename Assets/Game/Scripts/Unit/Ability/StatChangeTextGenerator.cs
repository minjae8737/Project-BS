using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using UnityEngine;

[Serializable]
public class StatChangeTextGenerator
{
    [SerializeField] private StatChangeTextData statChangeTextData;

    public string BuildSkillStatText(List<StatChange<SkillStatType>> statChanges)
    {
        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < statChanges.Count; i++)
        {
            if (i > 0) sb.Append("\n");
            sb.Append(BuildSkillStatLine(statChanges[i]));
        }
        
        return sb.ToString();
    }

    public string BuildStatText(List<StatChange<StatType>> statChanges)
    {
        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < statChanges.Count; i++)
        {
            if (i > 0) sb.Append("\n");
            sb.Append(BuildStatLine(statChanges[i]));
        }
        
        return sb.ToString();
    }

    private string BuildSkillStatLine(StatChange<SkillStatType> statChange)
    {
        string format = statChangeTextData.GetSkillStatFormat(statChange.Type);
        string changeText = FormatText(format, statChange.Type.ToString(), statChange.InputValue, statChange.CurrentValue, statChange.AfterValue);

        return changeText;
    }

    private string BuildStatLine(StatChange<StatType> statChange)
    {
        string format = statChangeTextData.GetStatFormat(statChange.Type);
        string changeText = FormatText(format, statChange.Type.ToString(), statChange.InputValue, statChange.CurrentValue, statChange.AfterValue);

        return changeText;
    }

    private string FormatText(string format, string typeName, float inputValue, float currentValue, float afterValue)
    {
        string changeText;

        if (string.IsNullOrEmpty(format))
        {
            changeText = string.Format(CultureInfo.InvariantCulture, "{0} +{1}", typeName, inputValue);
            Debug.LogWarning($"[StatChangeTextGenerator] 텍스트 포멧이 비어있습니다. 기본 문장으로 대체됩니다.", statChangeTextData);
            return changeText;
        }
        
        try
        {
            changeText = string.Format(CultureInfo.InvariantCulture, format, inputValue, afterValue - currentValue, currentValue, afterValue);
        }
        catch (FormatException)
        {
            changeText = string.Format(CultureInfo.InvariantCulture, "{0} +{1}", typeName, inputValue);
            Debug.LogWarning($"[StatChangeTextGenerator] {typeName}의 텍스트 포멧팅이 실패하였습니다. 기본 문장으로 대체됩니다.", statChangeTextData);
        }

        return changeText;
    }
}