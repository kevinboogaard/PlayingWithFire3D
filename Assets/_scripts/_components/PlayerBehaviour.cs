using UnityEngine;
using System.Collections;
[RequireComponent(typeof(Movement))]
public class PlayerBehaviour : MonoBehaviour {

    private Movement movementcomp;
    private Backpack backpackComp;
    private Health healthComp;
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
        healthComp = GetComponent<Health>();
	}
	
	void Update ()
    {
        if (Input.GetKey(kCLeft))
        {
            movementcomp.Rotate(-1);
        }
        else if (Input.GetKey(kCup))
        {
            movementcomp.Move(1);
        }
        else if (Input.GetKey(kCright))
        {
            movementcomp.Rotate(1);
        }
        else if (Input.GetKey(kCdown))
        {
            movementcomp.Move(-1);
        }
        else if (Input.GetKey(Bom))
        {
            if (healthComp._isDead == false)
            {
                backpackComp.DropBomb(TileSystem.GetTile(new Vector3(Mathf.Round(transform.position.x), 0, Mathf.Round(transform.position.z))));
            }
        }
	}
}
