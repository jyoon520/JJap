using UnityEngine;

public class Exclamationmark : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 0.5f); // 0.5�ʿ� ����
    }

    
    void Update()
    {
        transform.Translate(Vector3.up * 1 * Time.deltaTime); // ����ǥ ���� �̵�
    }
}
