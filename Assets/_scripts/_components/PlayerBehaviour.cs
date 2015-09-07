using UnityEngine;
using System.Collections;
[RequireComponent(typeof(Movement))]
public class PlayerBehaviour : MonoBehaviour {

    private Movement movementcomp;
    private int _movementspeed = 10;
	// Use this for initialization
	void Start () {
        movementcomp = GetComponent<Movement>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.A))
        {
            movementcomp.MoveX(-1, _movementspeed);
        }
        else if (Input.GetKey(KeyCode.W))
        {
            movementcomp.MoveZ(1, _movementspeed);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            movementcomp.MoveX(1, _movementspeed);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            movementcomp.MoveZ(-1, _movementspeed);
        }

	}
}
