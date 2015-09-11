using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Backpack of the player.
/// This component is required for every behaviour.
/// The Backpack holds every upgrade. Function to drop a bomb. 
/// </summary>
public class Backpack : MonoBehaviour
{
    /// <summary>
    /// List of every upgrade in the player.
    /// </summary>
    private List<Upgrade> _currentUpgrades = new List<Upgrade>();

    public KeyCode bombKeyCode;

    void Update()
    {
        if (Input.GetKeyDown(bombKeyCode))
        {
            DropBomb();
        }
    }

    public void DropBomb()
    {

    }

    public Upgrade GetUpgrade(int id)
    {
        return _currentUpgrades[id];
    }

    public List<Upgrade> GetUpgrades()
    {
        return _currentUpgrades;
    }

    public void AddUpgrade()
    {
        // Add Upgrade.
    }

    public void RemoveUpgrade(int id)
    {
        // Remove Upgrade.
    }
}
