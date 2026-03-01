using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameEvents 
{
    //player
    public static Action<int> OnHealthChanged;
    public static Action OnPlayerDied;

    //UI
    public static Action OnGameStarted;

    //inventory
    public static Action<ItemId> OnItemPickedUp;
    public static Action<ItemId> OnItemRemoved;
}
