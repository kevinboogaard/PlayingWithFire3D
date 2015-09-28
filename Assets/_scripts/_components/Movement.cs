using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {
    public bool moving = false;
    public bool rotation = false;

    public Vector3 tilePosition;

    private Vector3 endpos;
    private int endrot;
    public Tiles currentTile;

    public float movementSpeed = 10;
    public float rotationSpeed = 5;

    private enum rotations
    {
        north = 1,
        east = 2,
        south = 3,
        west = 4
    }

    private rotations _curRotation;

    void Start()
    {
        endpos = transform.position;
    }

    void Update()
    {
        if (Mathf.Round(transform.position.x) != tilePosition.x || Mathf.Round(transform.position.z) != tilePosition.z)
        {
            tilePosition = new Vector3(Mathf.Round(transform.position.x), 0, Mathf.Round(transform.position.z));
        }

        if (currentTile == null || currentTile.transform.position != transform.position)
        {
            currentTile = TileSystem.GetTile(transform.position);
        }

        if (Mathf.Round(transform.position.x) == Mathf.Round(endpos.x) && Mathf.Round(transform.position.z) == Mathf.Round(endpos.z))
        {
            moving = false;
        }

        if (Mathf.RoundToInt(transform.rotation.eulerAngles.y) == endrot || Mathf.RoundToInt(transform.rotation.eulerAngles.y) == 360 || Mathf.RoundToInt(transform.rotation.eulerAngles.y) == 0)
        {
            rotation = false;
        }

        transform.position = Vector3.MoveTowards(transform.position, endpos, movementSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(endrot, Vector3.up), rotationSpeed * Time.deltaTime);
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
                    endpos = new Vector3(Mathf.Round(transform.position.x), transform.position.y, Mathf.Round(transform.position.z)) + transform.forward;
                }
            }
            else if (dir < 0)
            {
                Tiles targettedTile = TileSystem.GetTile(new Vector3(Mathf.Round(transform.position.x + (-transform.forward.x)), 0, Mathf.Round(transform.position.z + (-transform.forward.z))));

                if (targettedTile != null && targettedTile.occupied == null)
                {
                    moving = true;
                    endpos = new Vector3(Mathf.Round(transform.position.x), transform.position.y, Mathf.Round(transform.position.z)) + -transform.forward;
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

        if (endrot == 0 || endrot == 360)
        {
            _curRotation = rotations.north;
        }
        else if (endrot == 90)
        {
            _curRotation = rotations.east;
        }
        else if (endrot == 180)
        {
            _curRotation = rotations.south;
        }
        else if (endrot == 270)
        {
            _curRotation = rotations.west;
        }
    }
}