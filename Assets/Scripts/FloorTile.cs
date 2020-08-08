using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileEnum
{
    grass,
    water,
    soil,
    seed,
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
            state.Init(this);
        }
    }

    public GameObject aboveBlock;

    private void Awake()
    {
        gameObject.layer = 12;
    }

    private void Start()
    {
        switch (tileEnum)
        {
            case TileEnum.grass:
                state = new TileState.PlantableGrassTile().Init(this);
                break;
            case TileEnum.water:
                break;
            case TileEnum.soil:
                break;
            case TileEnum.seed:
                state = new TileState.CompleteFlower().Init(this);
                break;
            case TileEnum.flower:
                state = new TileState.CompleteFlower().Init(this);
                break;
        }
    }
}

public interface ITileState
{
    ITileState Init(FloorTile tile);
    void Interact();
}

namespace TileState
{
    public class PlantableGrassTile : ITileState
    {
        private static PlantSeedDialog plantSeedDialog;
        private FloorTile flootTile;
        public ITileState Init(FloorTile tile)
        {
            if (plantSeedDialog == null)
            {
                plantSeedDialog = GameObject.Find("InteractCanvas").transform.Find("PlantSeedDialog").gameObject.GetComponent<PlantSeedDialog>();
            }
            flootTile = tile;
            return this;
        }

        public void Interact()
        {
            plantSeedDialog.UpdateDialog(flootTile);
            plantSeedDialog.MoveDialog(true);
        }
    }

    public class CompleteFlower : ITileState
    {
        private static InteractDialog interactDialog;
        public ITileState Init(FloorTile tile)
        {
            if (interactDialog == null)
            {
                interactDialog = GameObject.Find("InteractCanvas").transform.Find("InteractDialog").gameObject.GetComponent<InteractDialog>();
            }
            GameObject.Destroy(tile.aboveBlock);
            tile.aboveBlock = GameObject.Instantiate(Resources.Load("blocks/CompleteFlower") as GameObject, tile.transform.position + Vector3.up * 2, Quaternion.identity);
            return this;
        }

        public void Interact()
        {
            interactDialog.MoveDialog(true);
        }
    }
}
