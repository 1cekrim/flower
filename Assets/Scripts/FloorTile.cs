using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTile : MonoBehaviour
{
    public bool canMove;
    private void Awake()
    {
        gameObject.layer = 12;
    }
}
