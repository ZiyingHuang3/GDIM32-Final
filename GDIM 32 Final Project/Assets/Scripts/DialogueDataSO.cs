using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/Branching Dialogue")]
public class DialogueDataSO : ScriptableObject
{
    public DialogueNode[] nodes;
}
