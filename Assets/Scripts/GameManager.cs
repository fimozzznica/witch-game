using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Ключи для сохранения данных
    private const string SAVED_LEVEL_KEY = "SavedLevel";
    private const string SAVED_LIVES_KEY = "SavedLives";
    private const string SAVED_SCORE_KEY = "SavedScore";

    // Метод для кнопки "Новая игра"
    public void StartNewGame()
    {
        // Удаляем все сохранения
        PlayerPrefs.DeleteAll();

        // Устанавливаем начальные значения
        PlayerPrefs.SetInt(SAVED_LEVEL_KEY, 1); // Начинаем с 1 уровня
        PlayerPrefs.SetInt(SAVED_LIVES_KEY, 3); // 3 жизни
        PlayerPrefs.SetInt(SAVED_SCORE_KEY, 0); // Счет 0

        // Загружаем сцену с игрой
        SceneManager.LoadScene("GameScene");
    }

    // Метод для кнопки "Продолжить"
    public void ContinueGame()
    {
        // Проверяем, есть ли сохранение
        if (PlayerPrefs.HasKey(SAVED_LEVEL_KEY))
        {
            // Загружаем сцену с игрой
            SceneManager.LoadScene("GameScene");
        }
        else
        {
            Debug.Log("Нет сохраненной игры!");
            // Можно показать сообщение игроку
        }
    }

    // Метод для сохранения игры
    public static void SaveGame(int level, int lives, int score)
    {
        PlayerPrefs.SetInt(SAVED_LEVEL_KEY, level);
        PlayerPrefs.SetInt(SAVED_LIVES_KEY, lives);
        PlayerPrefs.SetInt(SAVED_SCORE_KEY, score);
        PlayerPrefs.Save(); // Важно вызвать Save()
    }
}