using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class MovePink : MonoBehaviour
{
  
    void Start()
    {
        Destroy(gameObject, 5); // 5�ʵ� �ı�
    }

    private void Update()
    {
        StartCoroutine(EnemyEntry(1)); // 1�� �ڿ� ���������� �̵�
    }
    IEnumerator EnemyEntry(float delay)
    {
        yield return new WaitForSeconds(delay); // ������ �ð�
        transform.Translate(Vector3.right * 8 * Time.deltaTime); 
    }
}
