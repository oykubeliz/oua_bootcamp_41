using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControllerPlus : MonoBehaviour
{
    public Transform[] patrolPoints;
    public float detectionRange = 10f;
    public float attackRange = 2f;
    public Transform player;
    public Collider attackCollider; // Eklenen Collider

    private UnityEngine.AI.NavMeshAgent agent;
    private int currentPatrolIndex;
    private bool playerInSight;
    private bool playerInRange;
    
    private bool isDead = false;
    public Animator animator; // Animator bileşeni
    
    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        //animator = GetComponent<Animator>(); // Animator bileşenini al
        currentPatrolIndex = 0;
        PatrolToNextPoint();

        attackCollider.enabled = false; // Saldırı Collider'ı başlangıçta devre dışı
    }

    void Update()
    {
        if (isDead)
            return;
        
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);
        playerInSight = distanceToPlayer <= detectionRange;
        playerInRange = distanceToPlayer <= attackRange;

        if (playerInRange)
        {
            attackCollider.enabled = true; // Saldırı Collider'ını etkinleştir
            AttackPlayer();
        }
        else
        {
            attackCollider.enabled = false; // Saldırı Collider'ını devre dışı bırak
            if (playerInSight)
            {
                FollowPlayer();
            }
            else
            {
                if (!agent.pathPending && agent.remainingDistance < 0.5f)
                {
                    PatrolToNextPoint();
                }
            }
        }
    }

    void PatrolToNextPoint()
    {
        if (patrolPoints.Length == 0)
            return;

        agent.destination = patrolPoints[currentPatrolIndex].position;
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        animator.SetBool("Walking", true); // Yürüme animasyonunu başlat
    }

    void FollowPlayer()
    {
        agent.destination = player.position;
        animator.SetBool("Walking", true); // Yürüme animasyonunu başlat
    }

    void AttackPlayer()
    {
        // Saldırı işlemleri burada gerçekleştirilebilir
        Debug.Log("Saldırı Menzilinde");
        animator.SetBool("Attacking", true); // Saldırı animasyonunu başlat
    }

    public void Die()
    {
        if (isDead)
            return;

        isDead = true;
        animator.SetTrigger("isDead"); // Ölüm animasyonunu tetikle
        Debug.LogError("Düşman Öldü");
        Destroy(gameObject, 2f); // 2 saniye sonra düşmanı yok et
    }
}
