using UnityEngine;

public class Spin : MonoBehaviour
{
    [SerializeField]
    Vector3 RotateSpeeds;

    private void Update()
    {
        transform.Rotate(RotateSpeeds);
    }
}
