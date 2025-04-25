using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    Transform targetTransform; // Ÿ�� �÷��̾�
    [SerializeField]
    Vector3 cameraPosition; // ī�޶� ��ġ

    [SerializeField]
    Vector2 center; 
    [SerializeField]
    Vector2 mapSize;

    [SerializeField]
    float cameraMoveSpeed;
    float height;
    float width;


    void Start()
    {
        targetTransform = GameObject.Find("Player").GetComponent<Transform>();

        height = Camera.main.orthographicSize; // Camera.main.orthographicSize�� ī�޶��� ������
        width = height * Screen.width / Screen.height;
    }


    void FixedUpdate()
    {
        LimitCameraArea();
    }

    void LimitCameraArea()
    { 
            // ī�޶��� �������� �ε巴�� �ϴ� �Լ� 
            transform.position = Vector3.Lerp(transform.position, targetTransform.position + cameraPosition, Time.deltaTime * cameraMoveSpeed);

        float lx = mapSize.x - width;
        float clampX = Mathf.Clamp(transform.position.x, -lx + center.x, lx + center.x);

        float ly = mapSize.y - height;
        float clampY = Mathf.Clamp(transform.position.y, -ly + center.y, ly + center.y);

        transform.position = new Vector3(clampX, clampY, -10f);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(center, mapSize * 2);
    }
}
