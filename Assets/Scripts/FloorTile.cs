using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileEnum
{
    grass,
    water,
    soil,
    flower
}

public class FloorTile : MonoBehaviour
{
    public bool canMove;
    public TileEnum tileEnum;
    private void Awake()
    {
        gameObject.layer = 12;
    }
}
