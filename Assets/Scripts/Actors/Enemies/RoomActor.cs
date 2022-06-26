using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomActor : MonoBehaviour
{
    public Room RoomReference { get; private set; }

    private BaseEnemyModel baseEnemyModel;
    private IStealable collectableItem;

    private void Awake()
    {
        baseEnemyModel = GetComponent<BaseEnemyModel>();
        collectableItem = GetComponent<IStealable>();
    }

    public void SetRoomReference(Room room)
    {
        RoomReference = room;
        if (baseEnemyModel != null)
            RoomReference.UpdateEnemyCounter(1);

        if (collectableItem != null)
            RoomReference.UpdateCollectableItem(collectableItem);
    }

    public void OnDie()
    {
        if (baseEnemyModel != null)
            RoomReference.UpdateEnemyCounter(-1);

        if (collectableItem != null)
            RoomReference.UpdateCollectableItem(collectableItem, true);
    }
}
