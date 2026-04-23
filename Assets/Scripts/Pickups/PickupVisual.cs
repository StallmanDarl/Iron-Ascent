using UnityEngine;

public class PickupVisual : MonoBehaviour
{
    public float floatHeight = 0.25f;
    public float floatSpeed = 2f;
    public float rotationSpeed = 90f;

    Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float y = Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = startPos + new Vector3(0, y, 0);

        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }
}