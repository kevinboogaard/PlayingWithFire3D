using UnityEngine;
using System.Collections;
[RequireComponent(typeof(Movement))]
public class PlayerBehaviour : MonoBehaviour {

    private Movement movementcomp;
    private int _movementspeed = 10;
	// Use this for initialization

    public KeyCode kCLeft = KeyCode.A;
    public KeyCode kCright = KeyCode.D;
    public KeyCode kCdown = KeyCode.S;
    public KeyCode kCup = KeyCode.W;

	void Start () {
        movementcomp = GetComponent<Movement>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(kCLeft))
        {
            movementcomp.MoveX(-1, _movementspeed);
        }
        else if (Input.GetKey(kCup))
        {
            movementcomp.MoveZ(1, _movementspeed);
        }
        else if (Input.GetKey(kCright))
        {
            movementcomp.MoveX(1, _movementspeed);
        }
        else if (Input.GetKey(kCdown))
        {
            movementcomp.MoveZ(-1, _movementspeed);
        }
	}
}
