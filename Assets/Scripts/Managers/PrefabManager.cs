using System.Collections.Generic;
using Mizuvt.Common;
using UnityEngine;
using System;

[Serializable]
public struct PrefabEntry {
    public string key;
    public GameObject prefab;
}

public class PrefabManager : PersistentSingleton<PrefabManager> {
    [SerializeField]
    private List<PrefabEntry> prefabList = new List<PrefabEntry>();

    private Dictionary<string, GameObject> prefabMap;

    protected override void Awake() {
        base.Awake();
        InitializePrefabMap();
    }

    private void InitializePrefabMap() {
        prefabMap = new Dictionary<string, GameObject>();
        foreach (var entry in prefabList) {
            if (!prefabMap.ContainsKey(entry.key)) {
                prefabMap.Add(entry.key, entry.prefab);
            } else {
                Debug.LogWarning($"Prefab with key {entry.key} is already registered.");
            }
        }
    }

    public GameObject? GetPrefab(string key) {
        if (prefabMap.TryGetValue(key, out GameObject prefab)) {
            return prefab;
        } else {
            Debug.LogError($"Prefab with key {key} not found.");
            return null;
        }
    }
}
