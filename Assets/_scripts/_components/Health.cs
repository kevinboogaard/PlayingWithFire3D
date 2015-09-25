using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Health component of the player. 
/// This component is required for every behaviour.
/// </summary>
public class Health : MonoBehaviour 
{
	/// <summary>
	/// Private Static List<Health>: Holds all the GameObjects with the component: Health.
	/// Generally used to check how many players are alive in the game.
	/// </summary>
	private static List<Health> amountPlayers = new List<Health>();

	/// <summary>
	/// Private Integer: Health of the Player.
	/// Default: 1.
	/// </summary>
	private int _health = 1;

	/// <summary>
	/// Public Bool: Is the player dead?
	/// Default: false.
	/// </summary>
	public bool _isDead = false;

	// <comment>
	// When the player starts, add itself to the AmountPlayers list.
	// </comment>
	private void Start()
	{
		amountPlayers.Add (gameObject.GetComponent<Health> ());

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < players.Length; i++ )
        {
            for (int j = 0; j < players.Length; j++)
            {
                Physics.IgnoreCollision(players[i].GetComponent<Collider>(), players[j].GetComponent<Collider>());
            }
        }
	}
	
	/// <summary>
	/// Public Function called when the player has won.
	/// </summary>
	public void WinGame()
	{
		// Go Back To Menu, Add Gold Because You Won.
	}
	
	/// <summary>
	/// Public Function called when the game is a tie.
	/// </summary>
	public void TieGame()
	{
		// Go Back To Menu, It is a tie.
	}

	/// <summary>
	/// Public Function called when the player has lost.
	/// </summary>
	public void LoseGame()
	{
		// Go Back To Menu, Less Gold Because You Lost.
	}

	/// <summary>
	/// Function called when the player has died.
	/// </summary>
	private void Death()
	{
		_isDead = true;
		CheckPlayers ();
        RemoveSelf();
	}

    /// <summary>
    /// Function called to remove the player when he died.
    /// </summary>
    private void RemoveSelf()
    {
        Renderer renderer = GetComponent<Renderer>();
        renderer.enabled = false;

    }

	/// <summary>
	/// Check if a player has won the game.
	/// </summary>
	private void CheckPlayers()
	{
		int amountDeath = 0;

		// For every player, add a death to the integer.
		for (int pId = 0; pId < amountPlayers.Count; pId++) 
		{
			if (amountPlayers[pId]._isDead) 
			{
				amountDeath++;
			}

			// If one player is left in the game, make him win, and the others lose.
			// If every player has died, make it a tie.
			if (amountDeath == amountPlayers.Count - 1) 
			{
				for (int i = 0; i < amountPlayers.Count; i++) { if (amountPlayers[i]._isDead == false){ amountPlayers[i].WinGame(); } else { amountPlayers[i].LoseGame(); }}
			}
			else if (amountDeath == amountPlayers.Count)
			{
				for (int i = 0; i < amountPlayers.Count; i++) { amountPlayers[i].TieGame(); }
			}
		}
	}

	/// <summary>
	/// Getter and Setter: Health of the Player.
	/// </summary>
	/// <value>New value of health.</value>
	public int health 
	{
		get 
		{
			return _health;
		}
		set 
		{
			_health = value;

			// Check if Health is below zero, if true: Call the death function.
			if (_health <= 0)
			{
				Death();
			}
		}
	}
}
