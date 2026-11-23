using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public static class EnumRowSynchronizer
{
    public static void SyncToEnum<TEnum, TRow>(List<TRow> rows, Func<TEnum, TRow> createDefault, Object context)
        where TEnum : struct, Enum
        where TRow : class, IEnumRow<TEnum>
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

    public static TRow[] BuildLookup<TEnum, TRow>(List<TRow> rows, Func<TEnum, TRow> createDefault, Object context)
        where TEnum : struct, Enum
        where TRow : class, IEnumRow<TEnum>
    {
        TEnum[] enumValues = (TEnum[])Enum.GetValues(typeof(TEnum));
        TRow[] lookup = new TRow[enumValues.Length];

        foreach (TRow row in rows)
        {
            int index = Convert.ToInt32(row.Type);

            if (lookup[index] != null)
            {
                Debug.LogWarning($"[{context.name}] 중복 : {row.Type}", context);
                continue;
            }

            try
            {
                lookup[index] = row;
            }
            catch (IndexOutOfRangeException e)
            {
                Debug.LogError($"[{context.name}] 중간에 빈값이 존재함. {row.Type}", context);
            }
        }

        for (int i = 0; i < lookup.Length; i++)
        {
            if (lookup[i] != null) continue;

            Debug.LogWarning($"[{context.name}] 존재하지 않음. 기본값 추가. : {enumValues[i]}", context);

            lookup[i] = createDefault(enumValues[i]);
        }

        return lookup;
    }
}