using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLevel : MonoBehaviour
{
    [SerializeField] private string scene;
    [SerializeField] private int level;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ( UnitRoot.Instance == null) return;
        if (collision.gameObject == UnitRoot.Instance.gameObject)
        {
            PlayerPrefs.SetString("LevelStatus" + level, "Win");
            PlayerPrefs.Save();
            UnitRoot.Instance.isPaused = true;
            UnitRoot.Instance.rb.simulated = false;
            UnitRoot.Instance.gameObject.SetActive(false);
            UnitRoot.Instance.UpdateHealthUI();
            SceneManager.LoadScene(scene); 
        }
    }
}
