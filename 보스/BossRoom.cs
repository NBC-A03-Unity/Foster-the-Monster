using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoom : MonoBehaviour
{
    public Transform target;
    public Animator animator;

    public GameObject[] traps;

    public void SetTarget(Transform player)
    {
        target = player;
    }
    public void SetTrap(int index)
    {
        if (target == null) { return; }
        Vector2 newPosition = traps[index].transform.position;
        newPosition.x = target.position.x;
        traps[index].transform.position = newPosition;
    }
}