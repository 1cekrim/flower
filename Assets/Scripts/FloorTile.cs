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
    FloorTile Tile { get; }
    ITileState Init(FloorTile tile);
    void Interact();
}

namespace TileState
{
    public class PlantableGrassTile : ITileState
    {
        private static PlantSeedDialog plantSeedDialog;
        private FloorTile floorTile;
        public FloorTile Tile
        {
            get => floorTile;
        }
        public ITileState Init(FloorTile tile)
        {
            if (plantSeedDialog == null)
            {
                plantSeedDialog = GameObject.Find("InteractCanvas").transform.Find("PlantSeedDialog").gameObject.GetComponent<PlantSeedDialog>();
            }
            floorTile = tile;
            return this;
        }

        public void Interact()
        {
            plantSeedDialog.UpdateDialog(floorTile);
            plantSeedDialog.MoveDialog(true);
        }
    }

    public class CompleteFlower : ITileState
    {
        private static InteractDialog interactDialog;
        private FloorTile floorTile;
        public FloorTile Tile
        {
            get => floorTile;
        }
        public ITileState Init(FloorTile tile)
        {
            if (interactDialog == null)
            {
                interactDialog = GameObject.Find("InteractCanvas").transform.Find("FlowerDialog").gameObject.GetComponent<FlowerDialog>();
            }
            GameObject.Destroy(tile.aboveBlock);
            tile.aboveBlock = BlockFactory.Instance.CreateBlock("CompleteFlower", tile.transform);
            floorTile = tile;
            Tile.canMove = false;
            return this;
        }

        public void Interact()
        {
            interactDialog.MoveDialog(true);
        }
    }
}
