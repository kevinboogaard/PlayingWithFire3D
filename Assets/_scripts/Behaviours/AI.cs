﻿using UnityEngine;
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
    public List<Tiles> knownTiles;

    private Movement _movementComp;

    private int amountRaycasts = 24;
    private int startAmountAngle = -60;
    private int stepAmountAngle = 5;

    void Start()
    {
        _movementComp = gameObject.GetComponent<Movement>();
        CheckFieldOfView();
    }

    private static List<Tiles> CheckPath(Tiles start, Tiles end, List<Tiles> forbidden)
    {
        TileSystem.ResetAll(); 

        List<Tiles> openList = new List<Tiles>();
        List<Tiles> closedList = new List<Tiles>();
        List<Tiles> forbiddenList = new List<Tiles>();

        Tiles currentTile;
        Tiles neighbour;

        float gScore;
        bool gScoreIsBest;

        if (forbidden.Count > 0)
        {
            for (int i = 0; i < forbidden.Count; i++)
            {
                forbiddenList.Add(forbidden[i]);
            }
        }

        openList.Add(start);

        while (openList.Count > 0)
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
            currentTile.open = true;

            for (int i = 0; i < currentTile.neighbours.Count; i++)
            {
                neighbour = currentTile.neighbours[i];

                if (neighbour.closed && forbiddenList.Contains(neighbour) && neighbour.occupied == null && !neighbour.isWarned)
                {
                    continue;
                }

                gScore = currentTile.g + 1;
                gScoreIsBest = false;

                if (!neighbour.open)
                {
                    gScoreIsBest = true;

                    neighbour.h = heuristic(neighbour.transform.position, end.transform.position);
                    openList.Add(neighbour);
                    neighbour.open = true;
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

        return new List<Tiles>();
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

    void CheckFieldOfView()
    {
        Quaternion startingAngle = Quaternion.AngleAxis(startAmountAngle, Vector3.up);
        Quaternion stepAngle = Quaternion.AngleAxis(stepAmountAngle, Vector3.up);

        Quaternion angle = transform.rotation * startingAngle;
        Vector3 direction = angle * Vector3.forward;
        Vector3 pos = transform.position;

        RaycastHit hit;

        for (int i = 0; i < amountRaycasts; i++)
        {
            if (Physics.Raycast(pos, direction, out hit, 500))
            {
                Debug.DrawLine(transform.position, hit.point, Color.red);

                Renderer rend = hit.collider.GetComponent<Renderer>();

                if (rend)
                {
                    if (hit.transform.tag == "Obstacle")
                    {
                        rend.material.color = Color.red;
                    }
                    else if (hit.transform.tag == "Tile")
                    {
                        Tiles cTile = hit.transform.GetComponent<Tiles>();

                        if (!knownTiles.Contains(cTile))
                        {
                            knownTiles.Add(cTile);
                        }
                    }
                }
            }

            direction = stepAngle * direction;
        }
    }
}
