using UnityEngine;

public class Damaging : MonoBehaviour
{

    [SerializeField]
    [Tooltip("The gameoOject that should be destroyed to remove the Damaging object from the game once it hits a Damageable object.")]
    GameObject ToBeDestroyed;

    private void Start()
    {
        //Destroys its own gameObject if no alternative is given
        if(ToBeDestroyed == null)
        {
            ToBeDestroyed = gameObject;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<Damageable>() != null)
        {
            other.gameObject.GetComponent<Damageable>().OnDestroyedByDamage();
            if(other.GetComponent<Damageable>().GetToBeDestroyed() != null)
            {
                Destroy(other.GetComponent<Damageable>().GetToBeDestroyed());
            }
            if (ToBeDestroyed != null)
            {
                Destroy(ToBeDestroyed);
            }
        }
    }
}
