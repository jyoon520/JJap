using UnityEngine;

public class Exclamationmark : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 0.5f); // 0.5초에 삭제
    }

    
    void Update()
    {
        transform.Translate(Vector3.up * 1 * Time.deltaTime); // 느낌표 위로 이동
    }
}
