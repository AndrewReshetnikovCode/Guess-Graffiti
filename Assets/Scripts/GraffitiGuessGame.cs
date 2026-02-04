using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using System.Collections.Generic;

public class GraffitiGuessGame : MonoBehaviour
{
    public static GraffitiGuessGame I;

    [Header("Database")]
    public SpriteProgressionManager database;
    public DifficultyDatabaseSO diffDatabase;

    [Header("Scene refs")]
    [SerializeField] private Image targetRenderer;
    [SerializeField] private Button confirmButton;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TMP_Text hintText; 


    [Header("Callbacks")]
    public UnityEvent OnCorrect;
    public UnityEvent OnWrong;

    
    private void Awake()
    {
        I = this;

        confirmButton.onClick.AddListener(CheckAnswer);
    }

    private void Start()
    {
        Next();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            CheckAnswer();
        }
    }
    public void DisplayHint()
    {
        hintText.text = database.GetCurrent().Value.text;
    }
    public string GetCurrentText()
    {
        return database.GetCurrent().Value.text;
    }

    private void ShowCurrent()
    {
        targetRenderer.sprite = database.GetCurrent().Value.sprite;
        inputField.text = string.Empty;
        inputField.ActivateInputField();
    }

    public void CheckAnswer()
    {
        string user = inputField.text.Trim();
        string correct = GetCurrentText().Trim();

        if (string.Equals(user, correct, StringComparison.OrdinalIgnoreCase))
        {
            OnCorrect?.Invoke();
            Next();
        }
        else
        {
            OnWrong?.Invoke();
        }
    }

    private void Next()
    {
        hintText.text = string.Empty;

        database.Next();

        ShowCurrent();
    }
}
