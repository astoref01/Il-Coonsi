using UnityEngine;
using UnityEngine.UI;

public class ShootingBarController : MonoBehaviour
{
    [SerializeField]
    private GunScriptableObject gunScriptableObject;
    public Slider shootingBar;
    public int maxShots = 100;
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

    private void UpdateShootingBar()
    {
        shootingBar.value = gunScriptableObject.i;
        if (shootingBar.value >= resetThreshold && Input.GetMouseButtonDown(1))
        {
            gunScriptableObject.i = 0;
            shootingBar.value = 0;
        }
    }
}