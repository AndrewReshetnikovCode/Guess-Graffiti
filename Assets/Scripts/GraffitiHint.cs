using System.Collections;
using UnityEngine;
using TMPro;

public class GraffitiHint : MonoBehaviour
{
    [SerializeField] private GraffitiGuessGame game;
    [SerializeField] private TMP_Text hintText;
    [SerializeField] private float showTime = 2f;

    private Coroutine routine;

    public void ShowHint()
    {
        if (routine != null)
            StopCoroutine(routine);

        routine = StartCoroutine(ShowRoutine());
    }

    private IEnumerator ShowRoutine()
    {
        string text = game.GetCurrentText();

        hintText.gameObject.SetActive(true);
        hintText.text = text;

        yield return new WaitForSeconds(showTime);

        hintText.gameObject.SetActive(false);
        routine = null;
    }
}
