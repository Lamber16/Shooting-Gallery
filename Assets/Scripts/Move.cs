using UnityEngine;

public class Move:MonoBehaviour
{

    [SerializeField]
    [Tooltip("The objects speeds for each of the x, y and z axes.")]
    Vector3 Speed;

    void Update()
    {
        float newX = transform.forward.x + Speed.x * Time.deltaTime;
        float newY = transform.forward.y + Speed.y * Time.deltaTime;
        float newZ = transform.forward.z + Speed.z * Time.deltaTime;

        transform.position += transform.right * Speed.x * Time.deltaTime;
        transform.position += transform.up * Speed.y * Time.deltaTime;
        transform.position += transform.forward * Speed.z * Time.deltaTime;

    }

}
