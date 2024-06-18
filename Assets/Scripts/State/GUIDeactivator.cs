using UnityEngine;

public class GUIDeactivator : MonoBehaviour
{
    public GameObject combatGUI;

    private void OnEnable()
    {
        ShootingBarController.OnCombatStateExit += DeactivateCombatGUI;
    }

    private void OnDisable()
    {
        ShootingBarController.OnCombatStateExit -= DeactivateCombatGUI;
    }

    private void DeactivateCombatGUI()
    {
        if (combatGUI != null)
        {
            combatGUI.SetActive(false);
        }
    }
}
