using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class WinLoseUI : MonoBehaviour
{
    [SerializeField] private GameObject winPanel; // starts inactive, turned on when someone dies
    [SerializeField] private TMP_Text winnerText;

    public void ShowWinner(string winnerName)
    {
        winPanel.SetActive(true);
        winnerText.text = winnerName + " Wins!";
        Time.timeScale = 0f; // pause the game so the fight stops
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // un-pause before reloading, or the new scene loads frozen
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}