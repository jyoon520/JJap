using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class MovePink : MonoBehaviour
{
  
    void Start()
    {
        Destroy(gameObject, 5); // 5초뒤 파괴
    }

    private void Update()
    {
        StartCoroutine(EnemyEntry(1)); // 1초 뒤에 오른쪽으로 이동
    }
    IEnumerator EnemyEntry(float delay)
    {
        yield return new WaitForSeconds(delay); // 딜레이 시간
        transform.Translate(Vector3.right * 8 * Time.deltaTime); 
    }
}
