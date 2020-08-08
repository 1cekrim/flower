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
                state = new TileState.PlantableGrassTile().Init();
                break;
            case TileEnum.water:
                break;
            case TileEnum.soil:
                break;
            case TileEnum.flower:
                state = new TileState.CompleteFlower().Init();
                break;
        }
    }
}

public interface ITileState
{
    ITileState Init();
    void Interact();
}

namespace TileState
{
    public class PlantableGrassTile : ITileState
    {
        private static PlantSeedDialog plantSeedDialog;
        public ITileState Init()
        {
            if (plantSeedDialog == null)
            {
                plantSeedDialog = GameObject.Find("InteractCanvas").transform.Find("PlantSeedDialog").gameObject.GetComponent<PlantSeedDialog>();
            }
            return this;
        }

        public void Interact()
        {
            plantSeedDialog.UpdateDialog();
            plantSeedDialog.MoveDialog(true);
        }
    }

    public class CompleteFlower : ITileState
    {
        private static InteractDialog interactDialog;
        public ITileState Init()
        {
            if (interactDialog == null)
            {
                interactDialog = GameObject.Find("InteractCanvas").transform.Find("InteractDialog").gameObject.GetComponent<InteractDialog>();
            }
            return this;
        }

        public void Interact()
        {
            interactDialog.MoveDialog(true);
        }
    }
}
