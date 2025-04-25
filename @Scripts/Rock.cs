using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Rock : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject, 0.3f);
    }
    void Update()
    {
        Vector3 targetPos = new Vector3(4.5f, 1.5f, 0);
        transform.position = Vector3.MoveTowards(transform.position, targetPos, 50 * Time.deltaTime);
    }
}
