using Photon.Pun;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Obstacle : MonoBehaviour
{
    public float hp = 10;
    [SerializeField]Slider hpSlider;
    public event Action dieEvent;
    [SerializeField] TMP_Text text;
    private PhotonView pv;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }

    private void Start()
    {
        hpSlider.maxValue = hp;
        
    }
    //여기다가 RPC를 가지고 해준다는 거잖아.
    private void Update()
    {
        hpSlider.value = hp;
        text.text = $"{hp}/{hpSlider.maxValue}";    
    }
    public void GetDamage(float damage)
    {
        pv.RPC("PGetDamage", RpcTarget.All, damage);
        if (hp < 0)
        {
            Die();//따로 해주는 이유 transform자체는 원래 공유할꺼니까 navi설정은 본체에서만 해주면 된다. 
        }
    }
    [PunRPC]
    public void PGetDamage(float damage)
    {
        hp -= damage;
        if (hp < 0)
        {
            Destroy(gameObject);
        }
    }
    private void Die()
    {
        dieEvent?.Invoke(); //골을 향해 가도록 하기
        
        //죽으면서 죽었다고 자기를 때리고 있는 오브젝트에게 모두 알려줘야함
        //사라지기 그그 Addressable사용해서 없어지면 될꺼같아
    }
}
