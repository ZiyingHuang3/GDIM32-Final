using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyQuestNPC : MonoBehaviour, IInteractable
{
    [Header("Quest Items (5)")]
    [SerializeField] private ItemId[] requiredItems = new ItemId[5];

    [Header("Reward")]
    [SerializeField] private ItemId rewardItem = ItemId.Key;

    [Header("Settings")]
    [SerializeField] private bool consumeRequiredItems = true;

    private bool questStarted;
    private bool questCompleted;

    public string GetHint() => "Talk";

    public void Interact(Player player)
    {
        if (player == null) return;

        if (!questStarted)
        {
            questStarted = true;
            Debug.Log($"Bring me these items: {ListItems(requiredItems)}");
            return;
        }

        if (questCompleted)
        {
            Debug.Log("You already have the key.");
            return;
        }

        bool hasAll = QuestManager.Instance.HasAll(player, requiredItems);

        if (!hasAll)
        {
            Debug.Log("You're missing: " + MissingItemsText(player, requiredItems));
            return;
        }

        if (consumeRequiredItems)
            QuestManager.Instance.TryConsumeAll(player, requiredItems);

        player.Inventory.Add(rewardItem);
        questCompleted = true;

        Debug.Log($"Good. Here is the {rewardItem}.");
    }

    private string MissingItemsText(Player player, ItemId[] req)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        bool first = true;

        foreach (var item in req)
        {
            if (!player.Inventory.Has(item))
            {
                if (!first) sb.Append(", ");
                sb.Append(item.ToString());
                first = false;
            }
        }
        return sb.ToString();
    }

    private string ListItems(ItemId[] req)
    {
        if (req == null || req.Length == 0) return "(none)";
        return string.Join(", ", System.Array.ConvertAll(req, x => x.ToString()));
    }
}

