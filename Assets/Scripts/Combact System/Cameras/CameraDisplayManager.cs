using UnityEngine;

public class CameraDisplayManager : MonoBehaviour
{
    public Camera Main;
    public Camera cameraMoveA;
    public Camera cameraMoveB;
    public Camera cameraMoveY;
    public Camera cameraMoveX;

    private void DisableAllCameras()
    {
        cameraMoveA.gameObject.SetActive(false);
        cameraMoveB.gameObject.SetActive(false);
        cameraMoveY.gameObject.SetActive(false);
        cameraMoveX.gameObject.SetActive(false);
        Main.gameObject.SetActive(false);
    }

    public void HandleSpecialA()
    {
        DisableAllCameras();
        cameraMoveA.gameObject.SetActive(true);
        
    }

    public void HandleSpecialB()
    {
        DisableAllCameras();
        cameraMoveB.gameObject.SetActive(true);
   
    }

    public void HandleSpecialY()
    {
        DisableAllCameras();
        cameraMoveY.gameObject.SetActive(true);
     
    }

    public void HandleSpecialX()
    {
        DisableAllCameras();
        cameraMoveX.gameObject.SetActive(true);
    
    }
}
