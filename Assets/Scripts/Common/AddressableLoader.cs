using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Mizuvt.Common {
    public class AddressablesLoader : MonoBehaviour {
        // Addressable Asset Key
        private readonly string assetKey;

        public AddressablesLoader(string assetKey) {
            this.assetKey = assetKey;
        }

        // Loaded GameObject
        private GameObject? loadedObject;

        // Method to load the asset using a string key
        public void LoadAsset() {
            if (!string.IsNullOrEmpty(assetKey)) {
                Addressables.LoadAssetAsync<GameObject>(assetKey).Completed += OnAssetLoaded;
            } else {
                Debug.LogError("Asset key is empty or null.");
            }
        }

        // Callback when the asset is loaded
        private void OnAssetLoaded(AsyncOperationHandle<GameObject> obj) {
            if (obj.Status == AsyncOperationStatus.Succeeded) {
                loadedObject = Instantiate(obj.Result);
                Debug.Log("Asset loaded and instantiated successfully.");
            } else {
                Debug.LogError("Failed to load asset.");
            }
        }

        // Method to unload the asset
        public void UnloadAsset() {
            if (loadedObject != null) {
                Destroy(loadedObject);
                loadedObject = null;

                Addressables.ReleaseInstance(loadedObject);
                Debug.Log("Asset unloaded successfully.");
            }
        }
    }
}