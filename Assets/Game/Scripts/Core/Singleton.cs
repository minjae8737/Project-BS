using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T instance;

    [SerializeField] private bool persistAcrossScenes = false;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError($"[Singleton] {typeof(T).Name} 인스턴스가 씬에 없습니다.");
            }
            return instance;
        }
    }

    public static bool HasInstance => instance != null;

    protected virtual void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.LogWarning($"[Singleton] {typeof(T).Name} 중복 인스턴스를 제거합니다: {name}");
            Destroy(gameObject);
            return;
        }

        instance = (T)this;

        if (persistAcrossScenes)
        {
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
        }
    }

    protected virtual void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
}