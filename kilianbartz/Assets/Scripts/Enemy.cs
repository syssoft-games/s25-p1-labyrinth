using UnityEngine;

public class Enemy : MonoBehaviour
{

    public float speed = 5f;
    Transform target;
    void Start()
    {
        target = GameManager.Instance.player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.gameState != GameState.Playing)
        {
            return;
        }
        FaceTarget();
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, target.position) < 1f)
        {
            GameManager.Instance.Die();
        }
    }
    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
}
