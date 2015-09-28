using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Artificial Intelligence Behaviour.
/// The class is put on every non player character in the game.
/// </summary>
[RequireComponent(typeof(Movement))]
public class AI : MonoBehaviour
{
    public List<Tiles> knownTiles = new List<Tiles>();
    public List<Tiles> walkableList = new List<Tiles>();

    private Movement _movementComp;

    private static float horizontalScore = 1f;
    private static float diagonalScore = 1.414f;

    private int amountRaycasts = 24;
    private int startAmountAngle = -60;
    private int stepAmountAngle = 5;

    private int _amountTurns = 0;
    private int _lookTurns = 4;

    private bool finishedTurning = true;
    private bool readyToFind = false;

    private enum AIStates
    {

    }

    void Start()
    {
        _movementComp = gameObject.GetComponent<Movement>();

        while (_lookTurns > 0)
        {
            if (finishedTurning == true)
            {
                _movementComp.Rotate(-1);
                finishedTurning = false;
            }
            else
            {
                if (_movementComp.rotation)
                {
                    CheckForTiles();
                    finishedTurning = true;
                    _lookTurns--;

                    if (_lookTurns <= 0)
                    {
                        readyToFind = true;
                    }
                }
            }
        }
    }

    void Update()
    {
        FollowList();
    }

    void FollowList()
    {
        if (walkableList.Count > 0)
        {
            if (_movementComp.moving == false && _movementComp.rotation == false && _amountTurns == 0)
            {
                if (_movementComp.tilePosition != walkableList[0].transform.position)
                {
                    CalculateTurns();
                    
                    if (_amountTurns == 0)
                    {
                        _movementComp.Move(1);
                        walkableList.Remove(walkableList[0]);
                    }
                }
                else if (walkableList.Count > 0)
                {
                    walkableList.Remove(walkableList[0]);
                }
            }
            else if (_movementComp.moving == false && _movementComp.rotation == false && _amountTurns != 0)
            {
                CheckForTiles();

                if (_amountTurns > 0)
                {
                    _movementComp.Rotate(1);
                    _amountTurns--;
                }
                else if (_amountTurns < 0)
                {
                    _movementComp.Rotate(-1);
                    _amountTurns++;
                }
            }
        }
        else if (walkableList.Count == 0 && readyToFind == true)
        {
            Tiles randomTile = knownTiles[Random.Range(0, knownTiles.Count - 1)];

            walkableList = CheckPath(TileSystem.GetTile(transform.position), randomTile, knownTiles);

            //foreach (Tiles tile in walkableList)
            //{
            //    tile.transform.GetComponent<Renderer>().material.color = Color.blue;
            //}
        }
    }

    void CalculateTurns()
    {
        int angle = Mathf.RoundToInt(Vector3.Angle(transform.forward, new Vector3(walkableList[0].transform.position.x - _movementComp.tilePosition.x, 0, walkableList[0].transform.position.z - _movementComp.tilePosition.z)));
        int aTurns = 0;

        CheckForTiles();

        if (angle > 0)
        {
            aTurns = angle / 90;
        }
        else if (angle < 0)
        {
            aTurns = -angle / 90;
        }

        _amountTurns = aTurns;
    }

    private static List<Tiles> CheckPath(Tiles start, Tiles end, List<Tiles> known, List<Tiles> forbidden = null)
    {
        if (known.Contains(end))
        {
            TileSystem.ResetAll();

            //end.transform.GetComponent<Renderer>().material.color = Color.blue;

            List<Tiles> openList = new List<Tiles>();
            List<Tiles> closedList = new List<Tiles>();
            List<Tiles> forbiddenList = new List<Tiles>();

            Tiles currentTile;
            Tiles neighbour;

            float gScore;
            bool gScoreIsBest;

            if (forbidden != null && forbidden.Count > 0)
            {
                for (int i = 0; i < forbidden.Count; i++)
                {
                    forbiddenList.Add(forbidden[i]);
                }
            }
            
            openList.Add(start);
            while (openList.Count > 0)
            {
                if (openList[0] != null)
                {
                    openList.Sort(sortOnF);

                    currentTile = openList[0];

                    if (currentTile == end)
                    {
                        return getPathToTile(currentTile);
                    }

                    openList.Remove(currentTile);
                    closedList.Add(currentTile);
                    currentTile.closed = true;
                    currentTile.open = false;
                    
                    for (int i = 0; i < currentTile.neighbours.Count; i++)
                    {
                        neighbour = currentTile.neighbours[i];

                        if (neighbour.closed || forbiddenList.Contains(neighbour) || !known.Contains(neighbour) || neighbour.occupied)
                        {
                            continue;
                        }

                        if (isDiagonal(currentTile, neighbour))
                        {
                            gScore = currentTile.g + diagonalScore;
                        }
                        else
                        {
                            gScore = currentTile.g + horizontalScore;
                        }

                        gScoreIsBest = false;

                        if (!neighbour.open)
                        {
                            gScoreIsBest = true;

                            neighbour.h = heuristic(neighbour.transform.position, end.transform.position);

                            print(neighbour);
                            openList.Add(neighbour);
                            neighbour.open = true;
                            //neighbour.GetComponent<Renderer>().material.color = Color.cyan;
                        }
                        else if (gScore < neighbour.g)
                        {
                            gScoreIsBest = true;
                        }

                        if (gScoreIsBest)
                        {
                            neighbour.parent = currentTile;
                            neighbour.g = gScore;
                            neighbour.f = neighbour.g + neighbour.h;
                        }
                    }
                }
                else
                {
                    Debug.LogWarning("Warning: Tile[0] is null.");
                    openList.RemoveAt(0);
                }
            }
        }

        return new List<Tiles>();
    }

    private static bool isDiagonal(Tiles center, Tiles neighbour)
    {
        if (center.transform.position.x != neighbour.transform.position.x && center.transform.position.z != center.transform.position.z)
        {
            return true;
        }
        return false;
    }

    private static List<Tiles> getPathToTile(Tiles tile)
    {
        List<Tiles> path = new List<Tiles>();

        while (tile.parent)
        {
            path.Add(tile);
            tile = tile.parent;
        }

        path.Reverse();
        return path;
    }

    private static int sortOnF(Tiles a, Tiles b)
    {
        if (a.f > b.f || a.f == b.f && a.h > b.h)
        {
            return 1;
        }
        else
        {
            return -1;
        }
    }

    private static int heuristic(Vector3 pos0, Vector3 pos1)
    {
        // Manhattan distance.
        int d1 = Mathf.Abs((int)pos1.x - (int)pos0.x);
        int d2 = Mathf.Abs((int)pos1.z - (int)pos0.z);
        return d1 + d2;
    }

    GameObject CheckForObstacle()
    {
        Quaternion startingAngle = Quaternion.AngleAxis(startAmountAngle, Vector3.up);
        Quaternion stepAngle = Quaternion.AngleAxis(stepAmountAngle, Vector3.up);

        Quaternion angle = transform.rotation * startingAngle;
        Vector3 direction = angle * Vector3.forward;
        Vector3 pos = new Vector3(transform.position.x, 1, transform.position.z);

        RaycastHit hit;
        List<GameObject> obstacles = new List<GameObject>();

        for (int h = 0; h < amountRaycasts; h++)
        {
            if (Physics.Raycast(pos, direction, out hit, 5000))
            {
                Renderer rend = hit.collider.GetComponent<Renderer>();

                if (rend)
                {
                    if (hit.transform.tag == "Obstacle")
                    {
                        obstacles.Add(hit.transform.gameObject);
                    }
                }

                direction = stepAngle * direction;
            }
        }

        if (obstacles.Count > 0)
        {
            return obstacles[Random.Range(0, obstacles.Count)];
        }
        else
        {
            return null;
        }
    }

    void CheckForTiles()
    {
        Quaternion startingAngle = Quaternion.AngleAxis(startAmountAngle, Vector3.up);
        Quaternion stepAngle = Quaternion.AngleAxis(stepAmountAngle, Vector3.up);

        Quaternion angle = transform.rotation * startingAngle;
        Vector3 direction = angle * Vector3.forward;
        Vector3 pos = new Vector3(transform.position.x, 0, transform.position.z);

        for (int h = 0; h < amountRaycasts; h++)
        {
            RaycastHit[] hits = Physics.RaycastAll(pos, direction, 500);
            bool end = false;

            for (int i = 0; i < hits.Length; i++)
            {
                Renderer rend = hits[i].collider.GetComponent<Renderer>();

                if (rend)
                {
                    if (hits[i].transform.tag == "Tile")
                    {
                        Tiles cTile = hits[i].transform.GetComponent<Tiles>();

                        if (cTile.occupied != null && cTile.occupied.tag == "Obstacle" || cTile.occupied != null && cTile.occupied.name == "Cube")
                        {
                            end = true;
                        }

                        if (!knownTiles.Contains(cTile) && end == false)
                        {
                            knownTiles.Add(cTile);
                        }
                    }
                }

                direction = stepAngle * direction;
            }
        }
    }
}
