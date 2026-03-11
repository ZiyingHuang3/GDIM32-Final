using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [SerializeField] private DialogueUI dialogueUI;

    private DialogueNode currentNode;
    private int currentLineIndex;
    private bool isTalking;

    public bool IsTalking => isTalking;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void StartDialogue(DialogueNode startNode)
    {
        if (startNode == null)
        {
            Debug.LogWarning("Start node is null.");
            return;
        }

        currentNode = startNode;
        currentLineIndex = 0;
        isTalking = true;

        ShowCurrentLine();
    }

    public void AdvanceLine()
    {
        if (!isTalking || currentNode == null) return;

        currentLineIndex++;

        if (currentNode.lines != null && currentLineIndex < currentNode.lines.Length)
        {
            ShowCurrentLine();
        }
        else
        {
            ShowChoicesOrEnd();
        }
    }

    private void ShowCurrentLine()
    {
        if (currentNode == null || currentNode.lines == null || currentNode.lines.Length == 0)
        {
            EndDialogue();
            return;
        }

        dialogueUI.ShowLineOnly(currentNode.lines[currentLineIndex]);
    }

    private void ShowChoicesOrEnd()
    {
        bool hasChoices = currentNode.playerReplyOptions != null &&
                          currentNode.playerReplyOptions.Length > 0;

        if (!hasChoices)
        {
            EndDialogue();
            return;
        }

        dialogueUI.ShowChoices(
            currentNode.lines[currentNode.lines.Length - 1],
            currentNode.playerReplyOptions,
            ChooseReply
        );
    }

    private void ChooseReply(int index)
    {
        if (currentNode == null || currentNode.npcReplies == null) return;
        if (index < 0 || index >= currentNode.npcReplies.Length) return;

        DialogueNode nextNode = currentNode.npcReplies[index];

        if (nextNode == null)
        {
            EndDialogue();
            return;
        }

        currentNode = nextNode;
        currentLineIndex = 0;
        ShowCurrentLine();
    }

    public void EndDialogue()
    {
        isTalking = false;
        currentNode = null;
        currentLineIndex = 0;
        dialogueUI.Hide();
    }
}