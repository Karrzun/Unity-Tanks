using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject(typeof(T).ToString()).AddComponent<T>();
                instance.gameObject.name = instance.GetType().ToString();
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (instance != null)
            Destroy(this);

        instance = this as T;
        DontDestroyOnLoad(gameObject);
    }

}