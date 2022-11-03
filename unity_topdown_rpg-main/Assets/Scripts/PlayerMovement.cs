using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    int numberOfProjectiles;
    public Rigidbody2D rb;
    [SerializeField] Transform hand;
    private float activeMoveSpeed = 5f;
    public GameObject generade;
    public GameObject player;

    [SerializeField]
    GameObject projectile;

    Vector2 startPoint;

    public FixedJoystick Joystick;

    float radius;
    float rotationModifier;
    public float dashSpeed;
    public float dashLength = .5f, dashCooldown = 1f;
    private float dashCounter;
    private float dashCoolCounter;

    private bool isdash = false;
    private bool isbomb = false;
    private bool ismutiple = false;


    Vector2 movement;



    public void onClickDash()
    {
        isdash = true;
        Skill1.Instance.isCooldown1 = !Skill1.Instance.isCooldown1;
        GameObject.Find("Dash").GetComponent<Button>().interactable = false;
    }
    public void onClickMutiple()
    {
        ismutiple = true;
        Skill2.Instance.isCooldown2 = !Skill2.Instance.isCooldown2;
        GameObject.Find("3arrows").GetComponent<Button>().interactable = false;
    }
    public void onClickGen()
    {
        isbomb = true;
        Skill3.Instance.isCooldown3 = !Skill3.Instance.isCooldown3;
        GameObject.Find("explosion").GetComponent<Button>().interactable = false;
    }

    public void Movement()
    {
        //float mx = Input.GetAxisRaw("Horizontal");
        //float my = Input.GetAxisRaw("Vertical");

        float mx = Joystick.Horizontal;
        float my = Joystick.Vertical;

        movement = new Vector2(mx, my).normalized;
    }

    public void Update()
    {
        Movement();
        float distanceToClosestEnemy = Mathf.Infinity;
        Enemy closestEnemy = null;
        Enemy[] allEnemies = FindObjectsOfType<Enemy>();
        player = GameObject.FindGameObjectWithTag("Player");
        foreach (Enemy currentEnemy in allEnemies)
        {
            float distanceToEnemy = (currentEnemy.transform.position - player.transform.position).sqrMagnitude;
            if (distanceToEnemy < distanceToClosestEnemy && distanceToEnemy != 0)
            {
                distanceToClosestEnemy = distanceToEnemy;
                closestEnemy = currentEnemy;
            }
            rotateToTarget(closestEnemy);
        }

        if (isdash == true)
        {
            if (dashCoolCounter <= 0 && dashCounter <= 0)
            {
                activeMoveSpeed = dashSpeed;
                dashCounter = dashLength;
            }
        }
        if (dashCounter > 0)
        {
            dashCounter -= Time.deltaTime;
            if (dashCounter <= 0)
            {
                activeMoveSpeed = moveSpeed;
                dashCoolCounter = dashCooldown;
            }
        }
        if (dashCoolCounter > 0)
        {
            dashCoolCounter -= Time.deltaTime;
        }

        isdash = false;
        if (isbomb == true)
        {
            Instantiate(generade, transform.position, Quaternion.identity);
            isbomb = false;
        }
        if (ismutiple == true)
        {
            startPoint = player.transform.position;
            SpawnProjectiles(numberOfProjectiles);
            ismutiple = false;
        }
    }
    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * activeMoveSpeed * Time.deltaTime);
    }

    void SpawnProjectiles(int numberOfProjectiles)
    {
        float angleStep = 360f / numberOfProjectiles;
        float angle = 0f;

        for (int i = 0; i <= numberOfProjectiles - 1; i++)
        {

            float projectileDirXposition = startPoint.x + Mathf.Sin((angle * Mathf.PI) / 180) * radius;
            float projectileDirYposition = startPoint.y + Mathf.Cos((angle * Mathf.PI) / 180) * radius;

            Vector2 projectileVector = new Vector2(projectileDirXposition, projectileDirYposition);
            Vector2 projectileMoveDirection = (projectileVector - startPoint).normalized * moveSpeed;

            var proj = Instantiate(projectile, startPoint, Quaternion.identity);
            proj.GetComponent<Rigidbody2D>().velocity =
                new Vector2(projectileMoveDirection.x, projectileMoveDirection.y);

            angle += angleStep;
        }
    }
    void rotateToTarget(Enemy enemy)
    {
        if (player != null)
        {
            Vector3 vectorToTarget = player.transform.position - enemy.transform.position;
            float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg + 180f;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * 10f);
        }
    }

    public void AddSpeed()
    {
        this.moveSpeed += 1f;
    }
}



