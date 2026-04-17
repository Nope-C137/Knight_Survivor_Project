using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyAI : MonoBehaviour
{
    [Header("Enemy AI Settings")]
    [SerializeField] private Transform player;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 8f;
    [SerializeField] private float groundCheckDistance = 1.1f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float gravityForce = 25f;

    [Header("Climbing")]
    [SerializeField] private float climbSpeed = 3f;
    [SerializeField] private float wallCheckDistance = 1f;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float climbHopForce = 6f;

    [Header("Attack Settings")]
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float timeBetweenAttacks = 1.5f;

    private Rigidbody rb;
    private EnemyAttackAnimation enemyAttackAnimation;

    // Cache
    private bool isAttacking = false;
    private bool alreadyAttacked = false;
    private bool wasClimbing = false;
    private Vector3 moveDirection;

    private const float MinDistanceToMove = 0.2f; // Tránh enemy dí sát quá gây rung lắc

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        enemyAttackAnimation = GetComponent<EnemyAttackAnimation>();

        rb.freezeRotation = true;
        rb.useGravity = true;
    }

    private void Start()
    {
        if (player == null)
        {
            StartCoroutine(FindPlayerWithDelay());
        }
    }

    void FixedUpdate()
    {
        if (player == null || isAttacking) return;

        // Tính hướng đến player (bỏ qua chiều cao để di chuyển mượt)
        Vector3 directionToPlayer = (player.position - transform.position);
        float distanceToPlayer = directionToPlayer.magnitude;

        // Nếu quá gần thì không cần di chuyển nữa (tránh rung lắc)
        if (distanceToPlayer < MinDistanceToMove)
        {
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
            return;
        }

        directionToPlayer.y = 0;
        moveDirection = directionToPlayer.normalized;

        // Kiểm tra tường phía trước
        bool wallAhead = Physics.Raycast(transform.position + Vector3.up * 0.5f, transform.forward, wallCheckDistance, wallLayer);

        if (wallAhead)
        {
            HandleClimbing();
        }
        else
        {
            HandleNormalMovement();
        }

        // Luôn xoay về phía player
        RotateTowardsPlayer();

        // Cập nhật trạng thái leo
        wasClimbing = wallAhead;

        // Kiểm tra tấn công
        if (distanceToPlayer <= attackRange && !alreadyAttacked)
        {
            AttackPlayer();
        }
    }

    private void HandleClimbing()
    {
        rb.useGravity = false;
        rb.linearVelocity = Vector3.up * climbSpeed;

        // Ngăn không cho enemy trôi ngang khi leo
        rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
    }

    private void HandleNormalMovement()
    {
        rb.useGravity = true;

        // Nếu vừa thoát khỏi leo tường → nhảy qua mép (hop)
        if (wasClimbing)
        {
            Vector3 hopDirection = (transform.forward * 0.8f + Vector3.up).normalized;
            rb.AddForce(hopDirection * climbHopForce, ForceMode.VelocityChange);
        }

        // Di chuyển ngang
        Vector3 horizontalVelocity = moveDirection * moveSpeed;
        rb.linearVelocity = new Vector3(horizontalVelocity.x, rb.linearVelocity.y, horizontalVelocity.z);

        // Kiểm tra grounded và áp dụng gravity thủ công (mượt hơn mặc định)
        bool isGrounded = Physics.Raycast(transform.position + Vector3.up * 0.1f, 
                                        Vector3.down, 
                                        groundCheckDistance, 
                                        groundLayer);

        if (!isGrounded)
        {
            rb.AddForce(Vector3.down * gravityForce, ForceMode.Acceleration);
        }
    }

    private void RotateTowardsPlayer()
    {
        Vector3 lookDirection = (player.position - transform.position);
        lookDirection.y = 0;

        if (lookDirection.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            rb.MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, 
                                           rotationSpeed * Time.fixedDeltaTime));
        }
    }

    private void AttackPlayer()
    {
        isAttacking = true;
        alreadyAttacked = true;

        // Dừng di chuyển hoàn toàn khi tấn công
        rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);

        // Quay về phía player khi tấn công
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));

        // Thực hiện animation tấn công
        if (enemyAttackAnimation != null)
        {
            enemyAttackAnimation.PlayAttackAnimation();
        }

        Invoke(nameof(ResetAttack), timeBetweenAttacks);
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
        isAttacking = false;

        if (enemyAttackAnimation != null)
        {
            enemyAttackAnimation.StopAttackAnimation();
        }
    }

    private IEnumerator FindPlayerWithDelay()
    {
        yield return new WaitForSeconds(1.5f);
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (player == null)
        {
            Debug.LogWarning("EnemyAI: Cannot find Player with tag 'Player'!");
        }
    }

    // Gizmos hỗ trợ debug
    private void OnDrawGizmosSelected()
    {
        // Ground check
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundCheckDistance);

        // Wall check (có offset lên một chút để chính xác hơn)
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position + Vector3.up * 0.5f, 
                       transform.position + Vector3.up * 0.5f + transform.forward * wallCheckDistance);

        // Attack range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}