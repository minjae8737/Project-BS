using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public static class EnumRowSynchronizer
{
    public static void SyncToEnum<TEnum, TRow>(List<TRow> rows, Func<TEnum, TRow> createDefault, Object context)
        where TEnum : struct, Enum
        where TRow : IEnumRow<TEnum>
    {
        TEnum[] enumValues = (TEnum[])Enum.GetValues(typeof(TEnum));
        List<TRow> syncedRows = new();

        for (int i = 0; i < enumValues.Length; i++)
        {
            bool found = false;

            for (int j = 0; j < rows.Count; j++)
            {
                if (EqualityComparer<TEnum>.Default.Equals(rows[j].Type, enumValues[i]))
                {
                    if (!found)
                    {
                        syncedRows.Add(rows[j]);
                        found = true;
                    }
                    else
                    {
                        Debug.LogWarning($"[{context.name}] {enumValues[i]}은 중복되었습니다.", context);
                    }
                }
            }

            if (!found)
            {
                Debug.LogWarning($"[{context.name}] {enumValues[i]} 기본값으로 추가했습니다. 값을 채워 주세요.", context);
                syncedRows.Add(createDefault(enumValues[i]));
            }
        }

        rows.Clear();

        for (int i = 0; i < syncedRows.Count; i++)
        {
            rows.Add(syncedRows[i]);
        }

        for (int i = 0; i < rows.Count; i++)
        {
            rows[i].RefreshName();
        }
    }
}