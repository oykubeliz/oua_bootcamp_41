using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifetime = 5f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyControllerPlus enemyController = other.GetComponent<EnemyControllerPlus>();
            if (enemyController != null)
            {
                enemyController.Die();
            }
            Destroy(gameObject);
        }
    }
}