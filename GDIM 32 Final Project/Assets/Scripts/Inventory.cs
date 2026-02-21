using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;

public class Inventory 
{
    private readonly List<ItemId> items = new List<ItemId>();

public void Add(ItemId id)
    {
        items.Add(id);
    }
    public bool Has(ItemId id)
    {
        return items.Contains(id);
    }

    public void Remove(ItemId id)
    {
        if (items.Contains(id))
            items.Remove(id);
    }

    public void Clear()
    {
        items.Clear();
    }
}
