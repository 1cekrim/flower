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
        if (state.IsComplete)
        {
            getFlowerButton.onClick.RemoveAllListeners();
            getFlowerButton.onClick.AddListener(() =>
            {
                InventoryManager.Instance.GetItem(state.seed.Flower);
                AudioManager.Instance.PlayAudioClip(AudioManager.Instance.LoadAudioClip("effect/GetFlowerSound"), 1, 1.5f);
                // TODO: 꽃마다 주는 경험치 다르게
                ResourceManager.Instance.ChangeExp(1);
                floorTile.State = new TileState.StemFlower(state.seed).Init(floorTile);
                MoveDialog(false);
            });
        }
        removeFlowerButton.onClick.RemoveAllListeners();
        removeFlowerButton.onClick.AddListener(() =>
        {
            floorTile.State = new TileState.PlantableGrassTile().Init(floorTile);
            Destroy(floorTile.aboveBlock);
            MoveDialog(false);
        });
        sprinklerButton.onClick.RemoveAllListeners();
        sprinklerButton.onClick.AddListener(() =>
        {
            // TODO: 지금은 물 주면 바로 성장하지만, 나중에 수정 필요함
            state.ToNextFlowerState();
            AudioManager.Instance.PlayAudioClip(AudioManager.Instance.LoadAudioClip("effect/SprinklerSound"));
            MoveDialog(false);
        });
    }
}
