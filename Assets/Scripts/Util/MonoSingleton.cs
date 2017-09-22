using UnityEngine;

/// <summary>
/// 需要使用Unity生命周期的单例模式
/// </summary>
public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    protected static T mInstance = null;

    public static T Instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = FindObjectOfType<T>();
                if (FindObjectsOfType<T>().Length > 1)
                {
                    Debug.LogError("More than 1!");

                    return mInstance;
                }

                if (mInstance == null)
                {
                    string instanceName = typeof(T).Name;

                    GameObject instanceGO = GameObject.Find(instanceName);

                    if (instanceGO == null)
                        instanceGO = new GameObject(instanceName);
                    mInstance = instanceGO.AddComponent<T>();

                    DontDestroyOnLoad(instanceGO);

                    //Debug.Log("Add New Singleton " + mInstance.name + " in Game!"); pppppp

                }
                else
                {
                    Debug.LogWarning("Already exist: " + mInstance.name);
                }
            }

            return mInstance;

        }
    }


    protected virtual void OnDestroy()
    {
        mInstance = null;
    }
}