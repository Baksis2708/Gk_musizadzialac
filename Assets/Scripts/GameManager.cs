using UnityEngine;
using UnityEngine.UI;  // Potrzebne do pracy z UI Text
using UnityEngine.SceneManagement;  // Potrzebne do restartu sceny

public class GameManager : MonoBehaviour
{
    // Punkty dla gracza i przeciwnika
    public int playerPoints = 0;
    public int enemyPoints = 0;

    // Odwo³anie do Text w UI, gdzie wyœwietlane bêd¹ punkty
    public Text pointsText;

    // Instancja GameManagera do u¿ytku w innych skryptach
    public static GameManager instance;

    private void Awake()
    {
        // Ustawiamy instancjê GameManagera, aby by³a dostêpna globalnie
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Na pocz¹tku ustawiamy punkty na 0 i aktualizujemy wyœwietlanie
        playerPoints = 0;
        enemyPoints = 0;
        UpdatePointsDisplay();
    }

    // Metoda dodaj¹ca punkt dla gracza
    public void AddPointToPlayer()
    {
        playerPoints++;
        UpdatePointsDisplay();  // Zaktualizuj wyœwietlanie punktów
        CheckForWinner();  // SprawdŸ, czy ktoœ wygra³
    }

    // Metoda dodaj¹ca punkt dla przeciwnika
    public void AddPointToEnemy()
    {
        enemyPoints++;
        UpdatePointsDisplay();  // Zaktualizuj wyœwietlanie punktów
        CheckForWinner();  // SprawdŸ, czy ktoœ wygra³
    }

    // Aktualizacja wyœwietlania punktów na ekranie
    void UpdatePointsDisplay()
    {
        pointsText.text = $"{playerPoints} : {enemyPoints}";
    }

    // Sprawdzenie, czy ktoœ osi¹gn¹³ 3 punkty i og³oszenie zwyciêzcy
    void CheckForWinner()
    {
        if (playerPoints >= 3)
        {
            Debug.Log("Gratulacje! Gracz wygra³!");
            RestartScene();  // Restart sceny po wygranej gracza
        }
        else if (enemyPoints >= 3)
        {
            Debug.Log("Niestety! Bot wygra³!");
            RestartScene();  // Restart sceny po wygranej bota
        }
    }

    // Restartowanie sceny po wygranej
   public  void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);  // Restartuje bie¿¹c¹ scenê
    }
}
