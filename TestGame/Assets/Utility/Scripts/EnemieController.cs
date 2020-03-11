using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemieController : MonoBehaviour
{
    [Header("Enemy health")]
    public float enemyHealth;
    public Slider healthFill;
    public Transform target;
    Vector3 oldPosition;
    Animator animator;
    public NavMeshAgent navMeshAgent;
    public float distance;
    public bool isAttacking;
    public int hitDamage;
    public float attackSpeed;
    public float cullDown;
    Player player;

    bool isInCombat;
    void Start()
    {
        healthFill.maxValue = enemyHealth;
        animator = GetComponent<Animator>();
        player = GameObject.Find("Player").GetComponent<Player>();
        target = player.transform;
        oldPosition = transform.position;
        distance = 10f;
    }

    // Update is called once per frame
    void Update()
    {
        FollowPlayer();
        healthFill.value = enemyHealth;
        if (isAttacking)
        {
            if (isInCombat) return;
            GetDamageByPlayer(player.hitDamage);
            
            StartCoroutine(Wait());
        }
    }

    private void FollowPlayer()
    {
        if (Vector3.Distance(transform.position, target.position) < distance && player.health > 0)
        {
            animator.SetBool("isWalking", true);
            navMeshAgent.SetDestination(target.position);
            transform.LookAt(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z));
        }
        else
        {
            ReturnToDefaultPosition();
        }
    }

    private void ReturnToDefaultPosition()
    {
        navMeshAgent.SetDestination(oldPosition);
        transform.LookAt(new Vector3(oldPosition.x, transform.position.y, oldPosition.z));
    }
    private void Attack()
    {
        animator.SetBool("isAttacking", true);
        StartCoroutine(AttackRepeat());
    }
    IEnumerator AttackRepeat()
    {
        animator.SetBool("isAttacking", true);
        yield return new WaitForSeconds(cullDown);

        while (player.health > 0 && isAttacking)
        {
            player.health -= hitDamage;

            if (player.health <= 0)
            {
                player.health = 0;
                isAttacking = false;
                animator.SetBool("isAttacking", false);
                break;
            }
            yield return new WaitForSeconds(attackSpeed);
        }
    }
    void OnTriggerEnter(Collider col)
    {        
        if (col.CompareTag("Player"))
        {
            isAttacking = true;

            Attack();
        }
        if (col.CompareTag("Stop"))
        {
            animator.SetBool("isWalking", false);
        }
    }    
    void OnTriggerExit(Collider col)
    {
        animator.SetBool("isAttacking", false);
        if (col.CompareTag("Player"))
        {
            isAttacking = false;
        }
    }
    void GetDamageByPlayer(int damage)
    {        
        if (Input.GetButtonDown("Fire1"))
        {
            enemyHealth -= damage;
            isInCombat = true;
        }                       
        if (enemyHealth <= 0)
        {
            Destroy(gameObject);
        }        
    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(attackSpeed);
        isInCombat = false;
    }
}
