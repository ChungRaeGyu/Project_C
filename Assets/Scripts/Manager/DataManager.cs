using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public List<Unit> unitSO = new List<Unit>();
    public GameObject cardPrefab;

    private void Awake()
    {
        if (Instance != null) {
            Destroy(gameObject);
            return; 
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
