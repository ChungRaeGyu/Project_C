using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Card : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    //유닛 스폰은 GhostObject에서 한다.
    //[HideInInspector]
    public Unit unit;
   // [HideInInspector]
    public DescriptionPanel descriptionPanel;
    //[HideInInspector]
    public Image image;

    private GameObject ghostObject;   // 소환할 오브젝트
    public float holdTime = 1.0f;      // 몇 초 이상 눌러야 하는지

    private bool isPressed = false;
    private float pressTimer = 0f;

    private void Start()
    {
        image = GetComponent<Image>();
    }
    void Update()
    {
        if (SceneManager.sceneCount < 2)
            return;
        if (isPressed)
        {
            pressTimer += Time.deltaTime;

            if (pressTimer >= holdTime)
            {
                ResetPress();
                SpawnGhostObject();
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
        pressTimer = 0f;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isPressed)
        {
            //카드의 기본정보를 얻고 설명창을 열어 줄꺼야
            descriptionPanel.gameObject.SetActive(true);
            descriptionPanel.unit = unit;
            descriptionPanel.sprite = image.sprite;
            descriptionPanel.init();
            ResetPress();
        }
    }

    private void ResetPress()
    {
        Debug.Log("Reset Press");
        isPressed = false;
        pressTimer = 0f;


    }

    private async void SpawnGhostObject()
    {
        image.enabled = false;
        string path = $"Assets/Prefabs/Ghost{unit.unitName}.prefab";
        ghostObject = await AddressableManager.Instance.Instantiate(path);
        ghostObject.GetComponent<GhostObject>().card = this;
        Debug.Log("Object Spawned!");
    }

    public void CardRemove()
    {
        AddressableManager.Instance.ReleaseImage(unit.imagePath); //카드 이미지 삭제
        GameManager.Instance.RemoveCard(unit); //게임매니저에 있는 currenthand에서 이 카드 Unit으로 찾아서 삭제
        Destroy(gameObject); //카드 삭제 //오브젝트 풀링 가능 나중에
    }
}
