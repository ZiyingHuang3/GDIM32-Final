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

    [Header("Choice A")]
    [SerializeField] private Button choiceAButton;
    [SerializeField] private TMP_Text choiceAText;

    [Header("Choice B")]
    [SerializeField] private Button choiceBButton;
    [SerializeField] private TMP_Text choiceBText;

    private Action<int> onChoose;
    private Action onSingleConfirm;

    private void Awake()
    {
        Hide();

        choiceAButton.onClick.RemoveAllListeners();
        choiceBButton.onClick.RemoveAllListeners();

        choiceAButton.onClick.AddListener(OnChoiceAClicked);
        choiceBButton.onClick.AddListener(OnChoiceBClicked);
    }

    public void ShowOne(string line, string buttonText, Action onClick)
    {
        root.SetActive(true);
        lineText.text = line;

        onChoose = null;
        onSingleConfirm = onClick;

        choiceAButton.gameObject.SetActive(true);
        choiceAText.text = buttonText;

        choiceBButton.gameObject.SetActive(false);
    }

    public void ShowTwoChoices(string line,
        string choiceA, bool showA,
        string choiceB, bool showB,
        Action<int> onChooseIndex)
    {
        root.SetActive(true);
        lineText.text = line;

        onSingleConfirm = null;
        onChoose = onChooseIndex;

        choiceAButton.gameObject.SetActive(showA);
        if (showA)
        {
            choiceAText.text = choiceA;
        }

        choiceBButton.gameObject.SetActive(showB);
        if (showB)
        {
            choiceBText.text = choiceB;
        }
    }

    public void ShowLineOnly(string line)
    {
        root.SetActive(true);
        lineText.text = line;

        onChoose = null;
        onSingleConfirm = null;

        choiceAButton.gameObject.SetActive(false);
        choiceBButton.gameObject.SetActive(false);
    }

    public void ShowChoices(string line, string[] choices, Action<int> onChooseIndex)
    {
        root.SetActive(true);
        lineText.text = line;

        onSingleConfirm = null;
        onChoose = onChooseIndex;

        int count = choices == null ? 0 : choices.Length;

        if (count > 0)
        {
            choiceAButton.gameObject.SetActive(true);
            choiceAText.text = choices[0];
        }
        else
        {
            choiceAButton.gameObject.SetActive(false);
        }

        if (count > 1)
        {
            choiceBButton.gameObject.SetActive(true);
            choiceBText.text = choices[1];
        }
        else
        {
            choiceBButton.gameObject.SetActive(false);
        }
    }

    public void Hide()
    {
        root.SetActive(false);
        onChoose = null;
        onSingleConfirm = null;
    }

    private void OnChoiceAClicked()
    {
        if (onSingleConfirm != null)
        {
            var callback = onSingleConfirm;
            onSingleConfirm = null;
            callback.Invoke();
            return;
        }

        onChoose?.Invoke(0);
    }

    private void OnChoiceBClicked()
    {
        onChoose?.Invoke(1);
    }
}