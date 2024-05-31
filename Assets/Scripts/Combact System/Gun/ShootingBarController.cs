using UnityEngine;
using UnityEngine.UI;

public class ShootingBarController : MonoBehaviour
{
    [SerializeField]
    private GunScriptableObject gunScriptableObject;
    public Slider shootingBar;
    public int maxShots = 10;
    public int resetThresholdA = 3;
    public int resetThresholdB = 3;
    public int resetThresholdY = 3;
    public int resetThresholdX = 3;

    void Start()
    {
        shootingBar.maxValue = maxShots;
        gunScriptableObject.i = 0;
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
        }
    }
    public void HandleSpecialB()
    {
        if (shootingBar.value >= resetThresholdB)
        {
            gunScriptableObject.i = 0;
            shootingBar.value = 0;
        }
    }
    public void HandleSpecialY()
    {
        if (shootingBar.value >= resetThresholdY)
        {
            gunScriptableObject.i = 0;
            shootingBar.value = 0;
        }
    }
    public void HandleSpecialX()
    {
        if (shootingBar.value >= resetThresholdX)
        {
            gunScriptableObject.i = 0;
            shootingBar.value = 0;
        }
    }
}