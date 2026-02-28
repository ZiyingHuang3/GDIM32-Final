using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public bool HasAll(Player player, ItemId[] requiredItems)
    {
        if (player == null || requiredItems == null) return false;

        foreach (var item in requiredItems)
        {
            if (!player.Inventory.Has(item))
                return false;
        }
        return true;
    }

    public bool TryConsumeAll(Player player, ItemId[] requiredItems)
    {
        if (!HasAll(player, requiredItems)) return false;

        foreach (var item in requiredItems)
            player.Inventory.Remove(item);

        return true;
    }
}
