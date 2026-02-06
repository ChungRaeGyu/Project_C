using UnityEngine;

public class Tower : MonoBehaviour
{
    //얘가 가져야하는 기능
    //점령당하기
    //GameManager Occupation호출하기
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Unit"))
        {
            Debug.Log("몬스터");
            string name = gameObject.name;
            char c = name[name.Length - 1];
            GameManager.Instance.Occupation(int.Parse(c.ToString()));
        }
        //이게 끝이야??????
    }
}
