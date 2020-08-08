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
    private ITileState state;
    public ITileState State
    {
        get
        {
            return state;
        }
        set
        {
            state = value;
            state.Init();
        }
    }

    private void Awake()
    {
        gameObject.layer = 12;
    }
    
    private void Start()
    {
        switch (tileEnum)
        {
            case TileEnum.grass:
                break;
            case TileEnum.water:
                break;
            case TileEnum.soil:
                break;
            case TileEnum.flower:
                break;
        }
    }
}

public interface ITileState
{
    void Init();
    void Interact();
}