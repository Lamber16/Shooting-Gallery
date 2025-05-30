using UnityEngine;

public class Damageable : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The gameobject that should be destroyed to remove the Damageable object from the game once it is hit by a Damaging object.")]
    GameObject ToBeDestroyed;

    private void Start()
    {
        if (ToBeDestroyed == null)
        {
            ToBeDestroyed = gameObject;
        }
    }

    public GameObject GetToBeDestroyed()
    {
        return ToBeDestroyed;
    }

    public void OnDestroyedByDamage()
    {
        GameManager.Instance.TargetHit();
    }
}
