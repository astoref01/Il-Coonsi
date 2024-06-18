using UnityEngine;

public class GUIActivator : MonoBehaviour
{
    public GameObject combatGUI;

    private void OnEnable()
    {
        CombatState.OnCombatStateEnter += ActivateCombatGUI;
    }

    private void OnDisable()
    {
        CombatState.OnCombatStateEnter -= ActivateCombatGUI;
    }

    private void ActivateCombatGUI()
    {
        if (combatGUI != null)
        {
            combatGUI.SetActive(true);
        }
    }

    private void DeactivateCombatGUI()
    {
        if (combatGUI != null)
        {
            combatGUI.SetActive(false);
        }
    }
}
