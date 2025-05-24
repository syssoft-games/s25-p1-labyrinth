using System;
using UnityEngine;

public class IntroCam : MonoBehaviour
{
    public float speed = 5f;
    bool isMoving = false;

    // Update is called once per frame
    void Update()
    {
        if (!isMoving)
            return;
        float step = speed * Time.deltaTime;
        Vector3 targetPosition = GameManager.Instance.mainCamera.transform.position;
        Vector3 move = Vector3.Slerp(transform.position, targetPosition, step);
        transform.position = move;
        Vector3 targetRotation = GameManager.Instance.mainCamera.transform.rotation.eulerAngles;
        Vector3 rotation = Vector3.Slerp(transform.rotation.eulerAngles, targetRotation, step);
        transform.rotation = Quaternion.Euler(rotation);

        // Intro Cam finished, switch to main camera
        if (Vector3.Distance(transform.position, targetPosition) < 1f)
        {
            isMoving = false;
            enabled = false;
            GameManager.Instance.StartPlaying();
        }
    }
    public void StartPan(float x, float z)
    {
        transform.position = new Vector3(x, transform.position.y, z);
        isMoving = true;
    }
}
