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

    public void DropBomb(Tiles spawnPoint)
    {
        if (spawnPoint != null && !spawnPoint.occupied)
        {
            Vector3 spawnPosition = new Vector3(spawnPoint.transform.position.x, 1, spawnPoint.transform.position.z);
            GameObject Bom = (GameObject)Instantiate(Resources.Load("Bom"), spawnPosition, transform.rotation);
            spawnPoint.occupied = Bom;
        }
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
