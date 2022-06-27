using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomActor : MonoBehaviour
{
    public Room RoomReference { get; private set; }

    private BaseEnemyModel baseEnemyModel;
    private CollectableItem collectableItem;

    public void SetRoomReference(Room room)
    {
        RoomReference = room;
        baseEnemyModel = GetComponent<BaseEnemyModel>();
        if (baseEnemyModel != null)
        {
            RoomReference.UpdateEnemyCounter(1);
            return;
        }

        collectableItem = GetComponent<CollectableItem>();
        if (collectableItem != null)
        {
            RoomReference.UpdateCollectableItem(collectableItem);
        }
    }

    public void OnDie()
    {
        if (baseEnemyModel != null)
            RoomReference.UpdateEnemyCounter(-1);

        if (collectableItem != null)
            RoomReference.UpdateCollectableItem(collectableItem, true);
    }
}
