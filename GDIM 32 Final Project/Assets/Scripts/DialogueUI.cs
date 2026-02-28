using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private GameObject root;
    [SerializeField] private TMP_Text lineText;

    [Header("Two Choices")]
    [SerializeField] private Button choiceAButton;
    [SerializeField] private TMP_Text choiceAText;
    [SerializeField] private Button choiceBButton;
    [SerializeField] private TMP_Text choiceBText;

    private Action<int> onChoose;

    private void Awake() => Hide();

    public void ShowTwoChoices(string line,
        string choiceA, bool showA,
        string choiceB, bool showB,
        Action<int> onChooseIndex)
    {
        root.SetActive(true);
        lineText.text = line;
        onChoose = onChooseIndex;

        choiceAButton.gameObject.SetActive(showA);
        if (showA)
        {
            choiceAText.text = choiceA;
            choiceAButton.onClick.RemoveAllListeners();
            choiceAButton.onClick.AddListener(() => onChoose?.Invoke(0));
        }

        choiceBButton.gameObject.SetActive(showB);
        if (showB)
        {
            choiceBText.text = choiceB;
            choiceBButton.onClick.RemoveAllListeners();
            choiceBButton.onClick.AddListener(() => onChoose?.Invoke(1));
        }
    }

    public void ShowOne(string line, string okText = "OK", Action onOk = null)
    {
        root.SetActive(true);
        lineText.text = line;

        choiceAButton.gameObject.SetActive(true);
        choiceBButton.gameObject.SetActive(false);

        choiceAText.text = okText;
        choiceAButton.onClick.RemoveAllListeners();
        choiceAButton.onClick.AddListener(() => onOk?.Invoke());
    }

    public void Hide()
    {
        root.SetActive(false);
        if (choiceAButton != null) choiceAButton.gameObject.SetActive(true);
        if (choiceBButton != null) choiceBButton.gameObject.SetActive(true);
    }
}