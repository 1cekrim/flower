﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlantSeedDialog : InteractDialog
{
    public GameObject SeedContent;
    public GameObject SeedElementPrefab;
    private void InitDialog()
    {
        foreach (Transform child in SeedContent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
    public void UpdateDialog()
    {
        InitDialog();
        List<Seed> seeds = InventoryManager.Instance.SearchItem<Seed>();
        Debug.Log(seeds.Count);
        foreach (Seed seed in seeds)
        {
            GameObject obj = Instantiate(SeedElementPrefab, Vector3.zero, Quaternion.identity);
            obj.transform.SetParent(SeedContent.transform, true);
            obj.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            // ItemComponent 재사용함. SetItem과 ItemCount는 이렇게 재사용해도 문제없음.
            ItemComponent component = obj.GetComponent<ItemComponent>();
            component.SetItem(seed);
            component.ItemCount = seed.Component.ItemCount;
            seed.UpdatePlantSeedDialogElement(component);
            // TODO: SeedDialogElementComponent 따로 만들어서 GetComponent 호출 줄이기
            Button button = obj.GetComponent<Button>();
            button.onClick.AddListener(() => {

            });
        }
    }
}