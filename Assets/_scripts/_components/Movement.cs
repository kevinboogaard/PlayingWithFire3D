using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {
    private Vector3 endpos;
    private int endrot;
    private bool moving = false;
    private bool rotation = false;
    public Tiles currentTile;

    public float movementSpeed = 10;
    public float rotationSpeed = 5;

    void Start()
    {
        endpos = transform.position;
    }

    void Update()
    {
        if (currentTile == null || currentTile.transform.position != transform.position)
        {
            currentTile = TileSystem.GetTile(transform.position);
        }

        if (transform.position == new Vector3(endpos.x, transform.position.y, endpos.z))
        {
            moving = false;
        }

        if (Mathf.RoundToInt(transform.rotation.eulerAngles.y) == endrot)
        {
            rotation = false;
        }

        transform.position = Vector3.MoveTowards(transform.position, endpos, Time.deltaTime * movementSpeed);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(endrot, Vector3.up), 5 * Time.deltaTime);
    }

    /// <summary>
    /// Move forward or backward.
    /// </summary>
    /// <param name="dir">Negative is backward, Positive is forward.</param>
    public void Move(int dir)
    {
        if (moving == false && rotation == false)
        {
            if (dir > 0)
            {
                Tiles targettedTile = TileSystem.GetTile(new Vector3(Mathf.Round(transform.position.x + transform.forward.x), 0, Mathf.Round(transform.position.z + transform.forward.z)));
                if (targettedTile != null && targettedTile.occupied == null)
                {
                    moving = true;
                    endpos = transform.position + transform.forward;
                }
            }
            else if (dir < 0)
            {
                Tiles targettedTile = TileSystem.GetTile(new Vector3(Mathf.Round(transform.position.x + (-transform.forward.x)), 0, Mathf.Round(transform.position.z + (-transform.forward.z))));

                if (targettedTile != null && targettedTile.occupied == null)
                {
                    moving = true;
                    endpos = transform.position + -transform.forward;
                }
            }
        }
    }
    
    /// <summary>
    /// Rotate left or right.
    /// </summary>
    /// <param name="dir">Negative is left, Positive is right.</param>
    public void Rotate(int dir)
    {
        if (rotation == false)
        {
            if (dir > 0)
            {
                if (endrot < 360)
                {
                    endrot = endrot + 90;
                }
                else
                {
                    endrot = 90;
                }
            }
            else if (dir < 0)
            {
                if (endrot > 0)
                {
                    endrot = endrot - 90;
                }
                else
                {
                    endrot = 270;
                }
            }

            rotation = true;
        }
    }
}