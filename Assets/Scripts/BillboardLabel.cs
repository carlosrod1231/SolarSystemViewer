using UnityEngine;

public class BillboardLabel : MonoBehaviour
{
    private Transform mainCamera;

    void Start()
    {
        mainCamera = Camera.main.transform;
    }

    void Update()
    {
        transform.LookAt(transform.position + mainCamera.forward);
    }
}