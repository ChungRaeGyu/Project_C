using System;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    float hp = 10;
    public event Action dieEvent;
    public void GetDamage(float damage)
    {
        hp -= damage;
        if (hp < 0)
        {
            Die();
        }
    }

    private void Die()
    {
        dieEvent?.Invoke();
        Destroy(gameObject);
        //죽으면서 죽었다고 자기를 때리고 있는 오브젝트에게 모두 알려줘야함
        //사라지기 그그 Addressable사용해서 없어지면 될꺼같아
    }
}
