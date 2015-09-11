using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Bomb Behaviour of the Bomb.
/// This class is put on every bomb that is spawned. 
/// </summary>
public class Bomb : MonoBehaviour
{
    public List<Bomb> bombs = new List<Bomb>();

    public int firePower = 1;
    public Vector2 tilePlaced;

    public float countdown = 5f;
    public bool ticking = true;

    private float currentCount;

    private void Start()
    {
        bombs.Add(gameObject.GetComponent<Bomb>());
    }

    private void Update()
    {
        if (ticking)
        {
            currentCount += Time.deltaTime;

            if (currentCount >= countdown)
            {
                Explode();
            }

            Warn();
        }
    }

    private void Warn()
    {
       
    }

    public void Explode()
    {
        
    }
}
