using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomActor : MonoBehaviour
{
    public Room RoomReference { get; private set; }

    private BaseEnemyModel baseEnemyModel;
    private CollectableItem collectableItem;
    private bool hasCalledToArms;

    public void SetRoomReference(Room room)
    {
        RoomReference = room;
        baseEnemyModel = GetComponent<BaseEnemyModel>();
        if (baseEnemyModel != null)
        {
            RoomReference.UpdateEnemyList(baseEnemyModel);
            return;
        }

        collectableItem = GetComponent<CollectableItem>();
        if (collectableItem != null)
        {
            RoomReference.UpdateCollectableItem(collectableItem);
        }
    }

    public void CallReinforcements()
    {
        if (hasCalledToArms) return;
        hasCalledToArms = true;
        foreach (var enemy in RoomReference.RoomEnemies)
        {
            enemy.CalledToArms();
        }
    }

    public void OnDie()
    {
        if (baseEnemyModel != null)
            RoomReference.UpdateEnemyList(baseEnemyModel, true);

        if (collectableItem != null)
            RoomReference.UpdateCollectableItem(collectableItem, true);
    }
}
