using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    // private Animator wheelsanimation;
    private Vector3 dir;
    private Score score;
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float gravity;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private GameObject zeroFuel;
    [SerializeField] private GameObject crash;
    [SerializeField] private GameObject bonus;
    [SerializeField] private int coins;
    [SerializeField] private Text coinsText;
    [SerializeField] private GameObject scoreText;
    [SerializeField] private GameObject shield;
    [SerializeField] private GameObject Whelsanim;
    private int lineToMove = 1;
    public float lineDistance = 4;
    private int maxSpeed = 110;

    private bool isImmortal;

    public float fuelSize;
    public float fuelUsage;
    public float canisterSize;
    public GameObject fuelProgressBar;
    private float cureentFuel;

    void Start()
    {
        //     wheelsanimation = GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();
        score = scoreText.GetComponent<Score>();
        StartCoroutine(SpeedIncrease());
        score.scoreMultiplier = 1;
        cureentFuel = fuelSize;
        isImmortal = false;
        coins = PlayerPrefs.GetInt("coins");
        coinsText.text = coins.ToString();
    }

    private void Update()
    {
        if (SwipeController.swipeRight)
        {
            if (lineToMove < 2)
                lineToMove++;

        }

        if (SwipeController.swipeLeft)
        {
            if (lineToMove > 0)
                lineToMove--;
        }

        if (SwipeController.swipeUp)
        {
            StartCoroutine(Boost());
        }

        if (SwipeController.swipeDown)
        {
            StartCoroutine(Slowing());
        }
        cureentFuel -= fuelUsage * Time.deltaTime;
        fuelProgressBar.transform.localScale = new Vector2(1, cureentFuel / fuelSize);


        //    if (controller.isGrounded)
        //       anim.SetTrigger("isRunning");

        Vector3 targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;
        if (lineToMove == 0)
            targetPosition += Vector3.left * lineDistance;
        else if (lineToMove == 2)
            targetPosition += Vector3.right * lineDistance;

        if (transform.position == targetPosition)
            return;
        Vector3 diff = targetPosition - transform.position;
        Vector3 moveDir = diff.normalized * 25 * Time.deltaTime;
        if (moveDir.sqrMagnitude < diff.sqrMagnitude)
            controller.Move(moveDir);
        else
            controller.Move(diff);
    }

    private void Jump()
    {
        dir.y = jumpForce;
        //      anim.SetTrigger("isJumping");
    }

    void FixedUpdate()
    {
        dir.z = speed;
        dir.y += gravity * Time.fixedDeltaTime;
        controller.Move(dir * Time.fixedDeltaTime);

        if (cureentFuel <= 0)
        {
            zeroFuel.SetActive(true);
            losePanel.SetActive(true);
            Time.timeScale = 0;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "obstacle")
        {
            if (isImmortal)
                Destroy(hit.gameObject);
            else
            {
                crash.SetActive(true);
                losePanel.SetActive(true);
                Time.timeScale = 0;
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Coin")
        {
            coins++;
            PlayerPrefs.SetInt("coins", coins);
            coinsText.text = coins.ToString();
            Destroy(other.gameObject);
        }

        if (other.gameObject.tag == "Gas")
        {
            if (cureentFuel < fuelSize)
                cureentFuel += canisterSize;
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "Bonus")
        {

            StartCoroutine(BonusStar());
            Destroy(other.gameObject);
        }

        if (other.gameObject.tag == "Shield")
        {

            StartCoroutine(ShieldBonus());
            Destroy(other.gameObject);
        }

    }

    private IEnumerator SpeedIncrease()
    {
        yield return new WaitForSeconds(4);
        if (speed < maxSpeed)
        {
            speed += 3;
            StartCoroutine(SpeedIncrease());
        }

    }
    private IEnumerator Boost()
    {
        speed += 20;

        yield return new WaitForSeconds(1);

        speed -= 20;
    }

    private IEnumerator Slowing()
    {
        speed -= 20;

        yield return new WaitForSeconds(1);

        speed += 20;
    }

    private IEnumerator BonusStar()
    {
        score.scoreMultiplier = 2;
        bonus.SetActive(true);
        yield return new WaitForSeconds(5);

        score.scoreMultiplier = 1;
        bonus.SetActive(false);
    }

    private IEnumerator ShieldBonus()
    {
        isImmortal = true;
        shield.SetActive(true);
        yield return new WaitForSeconds(5);

        isImmortal = false;
        shield.SetActive(false);
    }

    
}