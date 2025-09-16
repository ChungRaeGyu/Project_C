using UnityEngine;

[CreateAssetMenu(fileName = "Unit", menuName = "Scriptable Objects/Unit")]
public class Unit : ScriptableObject
{
    public string unitName;
    public int cost;
    public int damage;
    public float attackSpeed;
    public float speed;
    public string imagePath;
}
