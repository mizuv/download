using System.Collections.Generic;
using Mizuvt.Common;
using UnityEngine;
using System;

[Serializable]
public struct SpriteEntry {
    public string key;
    public Sprite sprite;
}

public class SpriteManager : PersistentSingleton<SpriteManager> {
    public static Sprite SquareCenterPivot => Instance.GetSprite("Square.CenterPivot");

    [SerializeField]
    private List<SpriteEntry> spriteList = new();

    private Dictionary<string, Sprite> spriteMap;

    protected override void Awake() {
        base.Awake();
        InitializeSpriteMap();
    }

    private void InitializeSpriteMap() {
        spriteMap = new Dictionary<string, Sprite>();
        foreach (var entry in spriteList) {
            if (!spriteMap.ContainsKey(entry.key)) {
                spriteMap.Add(entry.key, entry.sprite);
            } else {
                Debug.LogWarning($"Sprite with key {entry.key} is already registered.");
            }
        }
    }

    public Sprite GetSprite(string key) {
        if (spriteMap.TryGetValue(key, out Sprite sprite)) {
            return sprite;
        } else {
            Debug.LogError($"Sprite with key {key} not found.");
            throw new System.Exception("Sprite not found");
        }
    }
}