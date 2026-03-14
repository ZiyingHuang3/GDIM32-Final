using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KeyQuestNPC : MonoBehaviour, IInteractable
{
    [Header("UI")]
    [SerializeField] private DialogueUI dialogueUI;

    [Header("Quest Items (5)")]
    [SerializeField] private ItemId[] requiredItems = new ItemId[5];

    [Header("Reward (choose in Inspector)")]
    [SerializeField] private ItemId rewardItem;
    [SerializeField] private bool consumeRequiredItems = true;

    [Header("Talk UI")]
    [SerializeField] private GameObject talkPrompt;
    [SerializeField] private TMP_Text talkText;
    [SerializeField] private string talkMsg = "Click to talk";

    [Header("Dialogue Nodes")]
    [SerializeField] private DialogueNode introNode;
    [SerializeField] private DialogueNode introEndingNode;
    [SerializeField] private DialogueNode notReadyNode;
    [SerializeField] private DialogueNode readyNode;
    [SerializeField] private DialogueNode giveKeyNode;
    [SerializeField] private DialogueNode completedNode;

    private bool introASeen;
    private bool introBSeen;
    private bool questStarted;
    private bool questCompleted;
    private Player currentPlayer;

    private void Start()
    {
        if (talkPrompt != null) talkPrompt.SetActive(false);
        if (talkText != null) talkText.text = talkMsg;
        if (dialogueUI == null) dialogueUI = FindObjectOfType<DialogueUI>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (talkPrompt != null && !questCompleted) talkPrompt.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (talkPrompt != null) talkPrompt.SetActive(false);
    }

    public string GetHint() => "Talk";

    public void Interact(Player player)
    {
        if (player == null) return;

        currentPlayer = player;
        currentPlayer.SetCanMove(false);

        if (talkPrompt != null) talkPrompt.SetActive(false);

        if (dialogueUI == null)
        {
            Debug.LogError("DialogueUI not found in scene or not assigned.");
            currentPlayer.SetCanMove(true);
            return;
        }

        if (questCompleted)
        {
            ShowSingleNode(completedNode, EndDialogue);
            return;
        }

        if (!questStarted)
        {
            ShowIntroMenu();
            return;
        }

        ShowQuestCheck();
    }

    private void EndDialogue()
    {
        if (dialogueUI != null) dialogueUI.Hide();
        if (currentPlayer != null) currentPlayer.SetCanMove(true);
        UIManager ui = FindObjectOfType<UIManager>();
        if (ui != null)
        {
            ui.ShowChecklistUI();
        }
    }

    private void ShowIntroMenu()
    {
        if (introNode == null)
        {
            Debug.LogWarning("Intro node is missing.");
            EndDialogue();
            return;
        }

        if (introASeen && introBSeen)
        {
            ShowSingleNode(introEndingNode, () =>
            {
                questStarted = true;
                EndDialogue();
            });
            return;
        }

        string introLine = GetFirstLine(introNode);
        string choiceA = GetChoiceText(introNode, 0);
        string choiceB = GetChoiceText(introNode, 1);

        dialogueUI.ShowTwoChoices(
            introLine,
            choiceA, !introASeen,
            choiceB, !introBSeen,
            OnIntroPick
        );
    }

    private void OnIntroPick(int idx)
    {
        if (introNode == null || introNode.npcReplies == null || idx < 0 || idx >= introNode.npcReplies.Length)
        {
            EndDialogue();
            return;
        }

        DialogueNode replyNode = introNode.npcReplies[idx];

        if (idx == 0)
        {
            introASeen = true;
        }
        else
        {
            introBSeen = true;
        }

        ShowSingleNode(replyNode, ShowIntroMenu);
    }

    private void ShowQuestCheck()
    {
        if (!QuestManager.Instance.HasAll(currentPlayer, requiredItems))
        {
            ShowSingleNode(notReadyNode, EndDialogue);
            return;
        }

        if (readyNode == null)
        {
            Debug.LogWarning("Ready node is missing.");
            EndDialogue();
            return;
        }

        string readyLine = GetFirstLine(readyNode);
        string choiceA = GetChoiceText(readyNode, 0, "Give items");
        string choiceB = GetChoiceText(readyNode, 1, "Not yet");

        dialogueUI.ShowTwoChoices(
            readyLine,
            choiceA, true,
            choiceB, true,
            OnTurnInPick
        );
    }

    private void OnTurnInPick(int idx)
    {
        if (idx == 1)
        {
            EndDialogue();
            return;
        }

        if (consumeRequiredItems)
            QuestManager.Instance.TryConsumeAll(currentPlayer, requiredItems);

        currentPlayer.Inventory.Add(rewardItem);
        questCompleted = true;

        currentPlayer.RefreshKeyInHand(currentPlayer.Inventory);

        ShowSingleNode(giveKeyNode, EndDialogue);
    }

    private void ShowSingleNode(DialogueNode node, System.Action onDone)
    {
        if (node == null)
        {
            onDone?.Invoke();
            return;
        }

        string line = GetFirstLine(node);
        dialogueUI.ShowOne(line, "OK", onDone);
    }

    private string GetFirstLine(DialogueNode node)
    {
        if (node == null || node.lines == null || node.lines.Length == 0)
            return "...";
        return node.lines[0];
    }

    private string GetChoiceText(DialogueNode node, int index, string fallback = "OK")
    {
        if (node == null || node.playerReplyOptions == null) return fallback;
        if (index < 0 || index >= node.playerReplyOptions.Length) return fallback;
        return node.playerReplyOptions[index];
    }
}