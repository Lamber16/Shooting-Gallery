using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{

    public static T Instance;

    protected virtual void Awake()
    {
        if(Instance == null)
        {
            Instance = (T)this;
        }
        else
        {
            Destroy(gameObject);  //Ensure there is only one gameobject with this singleton
        }
    }
}
