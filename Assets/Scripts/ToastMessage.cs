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

    [SerializeField] GameObject background;
    [SerializeField] GameObject shadow;


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
        background.SetActive(true);
        shadow.SetActive(true);
        yield return new WaitForSeconds(duration);

        background.SetActive(false);
        shadow.SetActive(false);
    }
}
