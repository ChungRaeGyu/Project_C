using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class ToastMessage : MonoBehaviour
{
    public static ToastMessage Instance; // ½Ì±ÛÅæ

    public TMP_Text messageText;
    public float duration = 2f;

    private Coroutine currentCoroutine;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // ¾À ÀüÈ¯¿¡µµ ÆÄ±«µÇÁö ¾ÊÀ½
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowMessage(string message)
    {
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);

        currentCoroutine = StartCoroutine(ShowRoutine(message));
    }

    private IEnumerator ShowRoutine(string message)
    {
        messageText.text = message;
        gameObject.SetActive(true);

        yield return new WaitForSeconds(duration);

        gameObject.SetActive(false);
    }
}
