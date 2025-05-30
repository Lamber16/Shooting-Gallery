using UnityEngine;

public class Player : Singleton<Player>
{
    [Header("Aiming")]
    [SerializeField]
    float YawChangeRate;
    [SerializeField]
    float CannonPitchChangeRate;
    [SerializeField]
    float MinCannonPitch;
    [SerializeField]
    float MaxCannonPitch;

    [Header("Meshes")]
    [SerializeField]
    GameObject Body;
    [SerializeField]
    GameObject Cannon;

    [Header("Bullet")]
    [SerializeField]
    [Tooltip("A reference to the bullet prefab that will be spawned when the cannon is fired.")]
    GameObject Bullet;
    [SerializeField]
    Transform BulletSpawnPoint;

    [Header("Cameras")]
    [SerializeField]
    Camera ThirdPersonView;
    [SerializeField]
    Camera AimView;


    private float _fCannonRotationX = 0;  //Tracks the cannon's local rotation's x value relative to its starting rotation

    public void Shoot()
    {
        Instantiate(Bullet, BulletSpawnPoint.position, BulletSpawnPoint.rotation, null);
        GameManager.Instance.PlayerFired();
    }

    public void AdjustYaw(float fMoveAmount)
    {
        Vector3 rotation = new Vector3(0, fMoveAmount * YawChangeRate * Time.deltaTime, 0);
        transform.Rotate(rotation);
    }

    public void AdjustCannonPitch(float fPitchAmount)
    {
        if(Cannon != null)
        {
            _fCannonRotationX -= fPitchAmount * CannonPitchChangeRate * Time.deltaTime;
            _fCannonRotationX = Mathf.Clamp(_fCannonRotationX, MinCannonPitch, MaxCannonPitch);

            Cannon.transform.localEulerAngles = new Vector3(_fCannonRotationX, Cannon.transform.localEulerAngles.y, Cannon.transform.localEulerAngles.z);
        }
    }

    public void ChangeCamera()
    {
        ThirdPersonView.gameObject.SetActive(!ThirdPersonView.gameObject.activeInHierarchy);
        AimView.gameObject.SetActive(!AimView.gameObject.activeInHierarchy);

        UIManager.Instance.ToggleCrosshair(AimView.gameObject.activeInHierarchy); //Displays crosshair only in aim view
    }

    public void ChangeCamera(bool isSwitchingToThirdPersonView)
    {
        ThirdPersonView.gameObject.SetActive(isSwitchingToThirdPersonView);
        AimView.gameObject.SetActive(!isSwitchingToThirdPersonView);

        UIManager.Instance.ToggleCrosshair(AimView.gameObject.activeInHierarchy); //Displays crosshair only in aim view
    }

    public void SetVisiblity(bool isVisible)
    {
        Cannon.SetActive(isVisible);
        Body.SetActive(isVisible);
    }
}
