using UnityEngine;

public class Selfdestruct : MonoBehaviour
{
    [SerializeField]
    [Tooltip("How long this object lasts before destroying itself.")]
    float Lifetime;

    private void Update()
    {
        Lifetime -= Time.deltaTime;
        if(Lifetime < 0 )
        {
            Destroy(gameObject);
        }
    }
}
