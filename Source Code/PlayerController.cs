using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public int chocolateChips;
    public int cookieDough;
    public int maxCookieDough = 20;

    public Rigidbody2D rb;
    public float velocity = 2.5f;
    private Vector2 moveDelta;
    public bool isGameOver;

    public HealthBar healthBar;
    public ChipCounterText chipCounterText;
    public ChipCounterText chipCountText2;
    public UIGameStart uiGameStart;

    [SerializeField] GameObject chocolateChipPrefab;
    [SerializeField] GameObject cookieDoughPrefab;
    [SerializeField] GameObject spawnColliderRef;

    const float MAXFALLSPEED = -5f;

    public AudioSource jumpAudioSource;
    public AudioSource pickupAudioSource;
    public AudioSource gameOverAudioSource;

    void Start()
    {
        chocolateChips = 0;
        cookieDough = maxCookieDough;
        rb = GetComponent<Rigidbody2D>();
        isGameOver = false;

        healthBar.SetMaxHealth(maxCookieDough);
        chipCounterText.ChipCountText(chocolateChips.ToString());
        chipCountText2.ChipCountText(chocolateChips.ToString());

        ChipSpawn();
        DoughSpawn();
    }

    void Update()
    {
        if (cookieDough > 0)
        {
            Move();
            Jump();
        }
        
        if (cookieDough <= 0)
            GameOver();
    }

    private void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        var xPosition = transform.position.x;

        moveDelta = new Vector2(x, 0);
        transform.Translate(moveDelta * Time.deltaTime);

        if (xPosition > 1.645)
            transform.position = new Vector2(-1.645f, transform.position.y);
        if (xPosition < -1.645)
            transform.position = new Vector2(1.645f, transform.position.y);
    }

    private void Jump()
    {
        if (Input.GetKeyDown("space"))
        {
            //Debug.Log("Jump");
            rb.velocity = Vector2.up * velocity;
            cookieDough--;
            Debug.Log("You Have " + cookieDough + " Cookie Dough");

            healthBar.SetHealth(cookieDough);

            if (uiGameStart.gameStarted == true)
                jumpAudioSource.Play();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == ("ChocolateChip"))
        {
            Destroy(collision.gameObject);
            chocolateChips += 5;
            chipCounterText.ChipCountText(chocolateChips.ToString());
            chipCountText2.ChipCountText(chocolateChips.ToString());

            Debug.Log("You Have " + chocolateChips + " Chocolate Chips!");

            pickupAudioSource.Play();
        }

        if (collision.tag == ("CookieDough"))
        {
            if (cookieDough > maxCookieDough)
            {
                cookieDough = maxCookieDough;
                Debug.Log("You Have " + cookieDough + " Cookie Dough");
                healthBar.SetHealth(cookieDough);
            }
            else
            {
                Destroy(collision.gameObject);
                cookieDough += 3;
                if (cookieDough > maxCookieDough)
                    cookieDough = maxCookieDough;

                Debug.Log("You Have " + cookieDough + " Cookie Dough");
                healthBar.SetHealth(cookieDough);
            }

            pickupAudioSource.Play();
        }

        if (collision.tag == ("SpawnCollider"))
        {
            var spawnColliderY = spawnColliderRef.transform.position.y;
            spawnColliderRef.transform.position = new Vector3(0, spawnColliderY + 2, 0);

            ChipSpawn();
            DoughSpawn();
        }
    }

    private void ChipSpawn()
    {
        var spawnPositionX = transform.position.x;
        var spawnPositionY = transform.position.y;

        for (int i = 0; i < 5; i++)
        {
            Instantiate(chocolateChipPrefab, new Vector3(Random.Range(-0.8f, 0.8f), 
            Random.Range(spawnPositionY + 1.645f, spawnPositionY + 5f), 0), Quaternion.identity);
        }
    }
    
    private void DoughSpawn()
    {
        var spawnPositionX = transform.position.x;
        var spawnPositionY = transform.position.y;

        for (int i = 0; i < 1; i++)
        {
            Instantiate(cookieDoughPrefab, new Vector3(Random.Range(-0.8f, 0.8f), 
            Random.Range(spawnPositionY + 1.645f, spawnPositionY + 5f), 0), Quaternion.identity);
        }
    }

    private void GameOver()
    {
        if (rb.velocity.y <= MAXFALLSPEED)
        {
            isGameOver = true;
            uiGameStart.lose.SetActive(true);
            uiGameStart.hud.SetActive(false);

            gameOverAudioSource.Play();
            rb.Sleep();
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGameOver == true)
        {
            Debug.Log("Continue");
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }
}
