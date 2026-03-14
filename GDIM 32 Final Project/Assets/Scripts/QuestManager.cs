using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }
    [SerializeField] private List<CollectObjective> objectives;
    //[SerializeField] private UIManager uiManager;
    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    [System.Serializable]
    public class CollectObjective
    {
        public ItemId itemId;
        public bool collected;
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

    private void Start()
    {
       // uiManager=FindObjectOfType<UIManager>();
        UpdateChecklistUI();
    }

    public void CollectItem(ItemId id)
    {
        for (int i = 0; i < objectives.Count; i++)
        {
            if (objectives[i].itemId == id)
            {
                objectives[i].collected = true;
            }
        }
        UpdateChecklistUI();
    }

    private void UpdateChecklistUI()
    {
        string text = "Checklist\n";

        for (int i = 0; i < objectives.Count; i++)
        {
            if (objectives[i].collected == true)
            {
                text = text + "[x] " + objectives[i].itemId.ToString()+ "\n";
            }
            else
            {
                text = text + "[ ] " + objectives[i].itemId.ToString() + "\n";
            }
        }

       // uiManager.SetChecklistText(text);
       GameEvents.OnSetChecklistText?.Invoke(text);
    }
    }
