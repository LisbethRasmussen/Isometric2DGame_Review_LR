
//--- Lisbeth: Alternatives to singletons used in the industry:

// Zenject

// VContainer


//--- Lisbeth: Poor man's version of the above:

// Create a non-MonoBehaviour static class, and hold instances in that central place instead.
// Sign instances up in their Awake methods, call all instances by refering to the central place


//--- Lisbeth: Fail safe suggestions:

// use 'return' along with 'enable = false' instead of 'Destroy'

// If something has not signed up, search for it, and if it does not exists, make an 'exception'
// case instead of creating a new one, and solve the issue of whatever made the thing not exist
// when it was needed.
// The reason why you don't want to create a new one on the fly:
// Consider your MenuManager. If the Singleton creates a new one of it on during the game, the
// references for the menu _menuScreen and _endScreen would return null, and everything would
// break regardless of the new instance which was just created.

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

    //--- Lisbeth: This virtual Awake method will create problems if overriden.
    // Reason: Any logic placed outside the 'if' part, will still be executed, even when 'Destroy' is called.
    // You could consider not making this Awake virtual in any way, but create another virtual method
    // Which gets calle directly after setting the _instance, inside the 'if' part.

    protected virtual void Awake()
    {
        // Ensure that only one instance of the singleton exists and destroy any duplicates
        if (_instance == null)
        {
            _instance = this as T;
            //--- Lisbeth: Call to 'X' virtual method
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    //--- Lisbeth: virtual 'X' method located here
}
