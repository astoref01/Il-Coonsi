using UnityEngine;
using UnityEngine.UI;

public class ShootingBarController : MonoBehaviour
{
    [SerializeField]
    private GunScriptableObject gunScriptableObject;
    public Slider shootingBar;
    public int maxShots = 10;
    public int resetThreshold = 3;

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

    public void HandleSpecialX()
    {
        if (shootingBar.value >= resetThreshold)
        {
            gunScriptableObject.i = 0;
            shootingBar.value = 0;
        }
    }
}