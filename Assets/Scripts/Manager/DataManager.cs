using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

public enum Deck
{
    FirstDeck,
    SecondDeck,
    ThirdDeck,
    Length
}
public class DataManager : MonoBehaviour
{
    public static DataManager Instance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public List<Unit[]> unitSO = new List<Unit[]>();
    public Unit[] firstDeck;
    public Unit[] secondDeck;
    public Unit[] thiredDeck;
    public GameObject cardPrefab;
    
    public DescriptionPanel descriptionPanel;

    private Unit[] useDeck; //실제 사용할 덱
    [HideInInspector]
    public int deckIndex;
    private void Awake()
    {
        if (Instance != null) {
            Destroy(gameObject);
            return; 
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        unitSO.Add(firstDeck);
        unitSO.Add(secondDeck);
        unitSO.Add(thiredDeck);
    }

    public void DeckSetting()
    {
        //QuickMatch를 눌렀을 때 실제 사용할 덱 선택
        deckIndex = 0;//Random.Range(0, (int)Deck.Length);
        useDeck = unitSO[deckIndex];
    }
    public void DeckReset()
    {
        //게임 종료 후 덱 초기화
        useDeck = null;
        deckIndex = -1;
    }

    public async Task ShowDeck(int num, Transform content)
    {
        //덱별로 나누게 된다면 
        //얘를 나중에 DataManager에 빼가지고 Room에서도 자기에 해당하는 덱을 보여줄 수 있게 하면 되겠다.
        foreach (var unit in unitSO[num])
        {
            await ShowCard(content, unit);

        }
    }
    public async Task ShowCard(Transform content, Unit unit)
    {
        //cardContent도 여러개를 받아 줘야한다.
        AsyncOperationHandle<Sprite> handle = await AddressableManager.Instance.LoadImage(unit.imagePath);

        GameObject obj = Instantiate(cardPrefab, content);
        obj.GetComponent<Card>().Init(unit, descriptionPanel, handle);

    }
    public Unit[] SuffleDeck()
    {
        //덱 섞기
        for (int i = 0; i < useDeck.Length; i++)
        {
            Unit temp = useDeck[i];
            int randomIndex = Random.Range(i, useDeck.Length);
            useDeck[i] = useDeck[randomIndex];
            useDeck[randomIndex] = temp;
        }

        return useDeck;
    }
}
