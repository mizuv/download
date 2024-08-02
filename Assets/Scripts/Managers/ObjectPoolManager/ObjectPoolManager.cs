using Mizuvt.Common;
using Download;
using Download.NodeSystem;
using UniRx;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;


public class ObjectPoolManager : PersistentSingleton<ObjectPoolManager> {
    private ObjectPool<DroppableArea> DroppableAreaPool;

    protected override void Awake() {
        base.Awake();
        DroppableAreaPool = new ObjectPool<DroppableArea>(
            createFunc: () => Instantiate(UIPrefab.DroppableAreaPrefab).GetComponent<DroppableArea>(),
            actionOnGet: (droppableArea) => droppableArea.OnGetFromPool(),
            actionOnRelease: (droppableArea) => droppableArea.OnReturnToPool(),
            actionOnDestroy: (droppableArea) => Destroy(droppableArea.gameObject),
            collectionCheck: true,
            defaultCapacity: 200
        );
    }

    public DroppableArea GetDroppableArea() {
        return DroppableAreaPool.Get();
    }

    public void ReturnDroppableArea(DroppableArea droppableArea) {
        DroppableAreaPool.Release(droppableArea);
    }
}
