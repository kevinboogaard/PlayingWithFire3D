using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

    
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void MoveX(int dir ,float movementspeed)
    {
        transform.Translate(new Vector3(dir, 0, 0) * movementspeed * Time.deltaTime);
    }

    public void MoveZ(int dir ,float movementspeed)
    {
        transform.Translate(new Vector3(0, 0, dir) * movementspeed * Time.deltaTime);
    }
}
