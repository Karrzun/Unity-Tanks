using UnityEngine;

/**
 * Use this class by deriving and accessing without manual instantiation.
 */
public abstract class LazySingleton<T> : MonoBehaviour where T : LazySingleton<T>
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

}