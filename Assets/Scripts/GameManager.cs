using UnityEngine;
using UnityEngine.UI;  // Potrzebne do pracy z UI Text
using UnityEngine.SceneManagement;  // Potrzebne do restartu sceny

public class GameManager : MonoBehaviour
{
    // Punkty dla gracza i przeciwnika
    public int playerPoints = 0;
    public int enemyPoints = 0;

    // Odwo�anie do Text w UI, gdzie wy�wietlane b�d� punkty
    public Text pointsText;

    // Instancja GameManagera do u�ytku w innych skryptach
    public static GameManager instance;

    private void Awake()
    {
        // Ustawiamy instancj� GameManagera, aby by�a dost�pna globalnie
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
        // Na pocz�tku ustawiamy punkty na 0 i aktualizujemy wy�wietlanie
        playerPoints = 0;
        enemyPoints = 0;
        UpdatePointsDisplay();
    }

    // Metoda dodaj�ca punkt dla gracza
    public void AddPointToPlayer()
    {
        playerPoints++;
        UpdatePointsDisplay();  // Zaktualizuj wy�wietlanie punkt�w
        CheckForWinner();  // Sprawd�, czy kto� wygra�
    }

    // Metoda dodaj�ca punkt dla przeciwnika
    public void AddPointToEnemy()
    {
        enemyPoints++;
        UpdatePointsDisplay();  // Zaktualizuj wy�wietlanie punkt�w
        CheckForWinner();  // Sprawd�, czy kto� wygra�
    }

    // Aktualizacja wy�wietlania punkt�w na ekranie
    void UpdatePointsDisplay()
    {
        pointsText.text = $"{playerPoints} : {enemyPoints}";
    }

    // Sprawdzenie, czy kto� osi�gn�� 3 punkty i og�oszenie zwyci�zcy
    void CheckForWinner()
    {
        if (playerPoints >= 3)
        {
            Debug.Log("Gratulacje! Gracz wygra�!");
            RestartScene();  // Restart sceny po wygranej gracza
        }
        else if (enemyPoints >= 3)
        {
            Debug.Log("Niestety! Bot wygra�!");
            RestartScene();  // Restart sceny po wygranej bota
        }
    }

    // Restartowanie sceny po wygranej
   public  void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);  // Restartuje bie��c� scen�
    }
}
