using UnityEngine;
using UnityEngine.UI;

public class ShootingBarController : MonoBehaviour
{
    [SerializeField]
    private GunScriptableObject gunScriptableObject;
    public Character character;
    public Slider shootingBar;
    public EnemyHealth enemyHealth;
    public int maxShots = 10;
    public int resetThresholdA = 3;
    public int resetThresholdB = 3;
    public int resetThresholdY = 3;
    public int resetThresholdX = 3;
    public int damageA = 8;
    public int damageB = 20;
    public int damageY = 30;
    public int damageX = 50;


    public CameraDisplayManager cameraDisplayManager;

    void Start()
    {
        shootingBar.maxValue = maxShots;
        gunScriptableObject.i = 0;

        cameraDisplayManager = FindObjectOfType<CameraDisplayManager>();
    }

    void Update()
    {
        UpdateShootingBar();
    }

    public void UpdateShootingBar()
    {
        shootingBar.value = gunScriptableObject.i;
    }

    public void HandleSpecialA()
    {
        if (shootingBar.value >= resetThresholdA)
        {
            gunScriptableObject.i = 0;
            shootingBar.value = 0;
            character.animator.SetTrigger("SpecialA");
            cameraDisplayManager.HandleSpecialA();
        }
    }

    public void OnEndSpecialA()
    {
        character.animator.SetTrigger("EndSpecialA");
        cameraDisplayManager.DisableAllCameras();
        enemyHealth._Health = enemyHealth._Health - damageA;
    }


    public void HandleSpecialB()
    {
        if (shootingBar.value >= resetThresholdB)
        {
            gunScriptableObject.i = 0;
            shootingBar.value = 0;
            cameraDisplayManager.HandleSpecialB();
        }
    }

    public void HandleSpecialY()
    {
        if (shootingBar.value >= resetThresholdY)
        {
            gunScriptableObject.i = 0;
            shootingBar.value = 0;
            cameraDisplayManager.HandleSpecialY();
        }
    }

    public void HandleSpecialX()
    {
        if (shootingBar.value >= resetThresholdX)
        {
            gunScriptableObject.i = 0;
            shootingBar.value = 0;
            cameraDisplayManager.HandleSpecialX();
        }
    }
}