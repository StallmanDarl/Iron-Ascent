using UnityEngine;

public class HologramFloat : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(0, 50 * Time.deltaTime, 0);
        transform.position = new Vector3(
            transform.position.x,
            1.5f + Mathf.Sin(Time.time) * 0.1f,
            transform.position.z);
    }
}