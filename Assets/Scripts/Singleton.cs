using UnityEngine;

/// <summary>
/// A generic singleton class for MonoBehaviour-based components in Unity.
/// </summary>
/// <typeparam name="T">The type of the singleton, which must inherit from MonoBehaviour.</typeparam>
public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    private static readonly object _lock = new object();

    /// <summary>
    /// Provides a globally accessible instance of the singleton.
    /// </summary>
    public static T Instance
    {
        get
        {
            // Lock the object to protect it from parallel threads
            lock (_lock)
            {
                // If no instance of the object is assigned, try to find one in the scene
                if (_instance == null)
                {
                    _instance = (T)FindFirstObjectByType(typeof(T));

                    // If there is no instance in the scene, create a new one
                    if (_instance == null)
                    {
                        GameObject singletonObject = new GameObject();
                        _instance = singletonObject.AddComponent<T>();
                        singletonObject.name = $"{typeof(T)} (Singleton)";
                    }
                }
                return _instance;
            }
        }
    }

    protected virtual void Awake()
    {
        // Ensure that only one instance of the singleton exists and destroy any duplicates
        if (_instance == null)
        {
            _instance = this as T;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }
}
