using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlowerDialog : InteractDialog
{
    [SerializeField]
    private Button matingFlowerButton;
    [SerializeField]
    private Button getFlowerButton;
    [SerializeField]
    private Button removeFlowerButton;
    [SerializeField]
    private Text flowerNameText;
    [SerializeField]
    private Button sprinklerButton;
    [SerializeField]
    private RawImage waterIcon;

    public void UpdateDialog<T>(FloorTile floorTile, T state) where T : TileState.FlowerState
    {
        getFlowerButton.gameObject.SetActive(state.IsComplete);
        flowerNameText.text = state.seed.Flower.Name + " : " + state.LevelName;
    }
}
