#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;

public class CSVtoSO
{
    private static string UnitCSVPath = "/CSV/Unit.csv";
    [MenuItem("Utilities/Generate Units")]
    public static void GenerateEnemies()
    {
        string[] allLines = File.ReadAllLines(Application.dataPath + UnitCSVPath);

        for(int i=1; i<allLines.Length; i++)
        {
            string[] splitData = allLines[i].Split(',');

            Unit unit = ScriptableObject.CreateInstance<Unit>();
            unit.unitName = splitData[0];
            unit.cost = int.Parse(splitData[1]);
            unit.damage = float.Parse(splitData[2]);
            unit.attackSpeed = float.Parse(splitData[3]);
            unit.speed = float.Parse(splitData[4]);
            unit.imagePath = splitData[5];
            AssetDatabase.CreateAsset(unit, $"Assets/ScriptableObject/{unit.unitName}.asset");
        }

        AssetDatabase.SaveAssets();
    }
}
#endif