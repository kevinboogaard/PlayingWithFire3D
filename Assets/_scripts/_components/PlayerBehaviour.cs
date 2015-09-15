using UnityEngine;
using System.Collections;
[RequireComponent(typeof(Movement))]
public class PlayerBehaviour : MonoBehaviour {

    private Movement movementcomp;
    private Backpack backpackComp;
    private int _movementspeed = 10;

    public KeyCode kCLeft = KeyCode.A;
    public KeyCode kCright = KeyCode.D;
    public KeyCode kCdown = KeyCode.S;
    public KeyCode kCup = KeyCode.W;
    public KeyCode Bom = KeyCode.Space;

	void Start ()
    {
        movementcomp = GetComponent<Movement>();
        backpackComp = GetComponent<Backpack>();
	}
	
	void Update ()
    {
        if (Input.GetKey(kCLeft))
        {
            movementcomp.MoveX(-1);
        }
        else if (Input.GetKey(kCup))
        {
            movementcomp.MoveZ(1);
        }
        else if (Input.GetKey(kCright))
        {
            movementcomp.MoveX(1);
        }
        else if (Input.GetKey(kCdown))
        {
            movementcomp.MoveZ(-1);
        }
        else if (Input.GetKey(Bom))
        {
            backpackComp.DropBomb(TileSystem.GetTile(transform.position));
        }
	}
}
