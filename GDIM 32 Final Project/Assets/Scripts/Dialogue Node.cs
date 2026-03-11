using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueNode", menuName = "ScriptableObjects/DialogueNode", order = 1)]
public class DialogueNode : ScriptableObject
{
    [Header("NPC lines shown in order")]
    [TextArea(2, 5)]
    public string[] lines;

    [Header("Player choices")]
    public string[] playerReplyOptions;

    [Header("Next node for each choice")]
    public DialogueNode[] npcReplies;

    public bool IsEndingNode()
    {
        return npcReplies == null || npcReplies.Length == 0;
    }

    public bool HasChoices()
    {
        return playerReplyOptions != null && playerReplyOptions.Length > 0;
    }

    public bool IsValid()
    {
        if (!HasChoices()) return true;
        return playerReplyOptions.Length == npcReplies.Length;
    }
}