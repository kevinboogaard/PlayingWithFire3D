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
    private Movement _movementComp;

    private int amountRaycasts = 24;
    private int startAmountAngle = -60;
    private int stepAmountAngle = 5;

    void Start()
    {
        _movementComp = gameObject.GetComponent<Movement>();
        CheckFieldOfView();
    }

    void CheckPath(Tiles end, List<Tiles> forbidden)
    {
        List<Tiles> openList = new List<Tiles>();
        List<Tiles> closedList = new List<Tiles>();
        List<Tiles> forbiddenList = new List<Tiles>();

        //Tiles currentTile;

        if (forbidden.Count > 0)
        {
            for (int i = 0; i < forbidden.Count; i++)
            {
                forbiddenList.Add(forbidden[i]);
            }
        }

        openList.Add(_movementComp.currentTile);
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
                }
            }

            direction = stepAngle * direction;
        }
    }
}
