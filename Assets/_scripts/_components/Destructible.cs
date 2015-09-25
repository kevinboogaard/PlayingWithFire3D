using UnityEngine;
using System.Collections;

public class Destructible : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        if (Random.Range(4, 0) == 4)
        {
            //pwerup
        }
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
 