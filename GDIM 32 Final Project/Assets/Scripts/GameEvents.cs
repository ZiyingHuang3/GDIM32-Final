using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameEvents 
{
    //player
    public static event Action<int> OnHealthChanged;
    public static event Action OnPlayerDied;

    //UI
    public static event Action OnGameStarted;

    //inventory
    public static event Action<ItemId> OnItemPickedUp;
    public static event Action<ItemId> OnItemRemoved;
}
