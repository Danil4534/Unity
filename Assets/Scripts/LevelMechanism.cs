using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMechanism : MonoBehaviour
{
    [SerializeField] private string scene;
    [SerializeField] private string LevelStatus;
    public TMP_Text level1Info;
    public TMP_Text level2Info;
    public TMP_Text level3Info;
    public TMP_Text level4Info;
    public TMP_Text level5Info;
    public TMP_Text level6Info;
    public static LevelMechanism Instance { get; private set; }

  

    void Start()
    {
        //for (int i = 1; i <= 6; i++)
        //{
        //    Debug.Log($"LevelStatus{i}: {PlayerPrefs.GetString("LevelStatus" + i, "Play")}");
        //}
        //level1Info.text = PlayerPrefs.GetString("LevelStatus" + 1, "Play");
        //level2Info.text = PlayerPrefs.GetString("LevelStatus" + 2, "Play");
        //level3Info.text = PlayerPrefs.GetString("LevelStatus" + 3, "Play");
        //level4Info.text = PlayerPrefs.GetString("LevelStatus" + 4, "Play");
        //level5Info.text = PlayerPrefs.GetString("LevelStatus" + 5, "Play");
        //level6Info.text = PlayerPrefs.GetString("LevelStatus" + 6, "Play");

    }

    public void Level(int level)
    {
        if (UnitRoot.Instance == null)
        {
            Debug.LogError("UnitRoot.Instance is null!");
            return;
        }
        switch (level)
        {
            case 1:
                SceneManager.LoadScene("SampleScene");
                UnitRoot.Instance.transform.position = new Vector3(108, 10, 0);
                UnitRoot.Instance.isPaused = false;
                UnitRoot.Instance.rb.simulated = true;
                UnitRoot.Instance.gameObject.SetActive(true);
         
                break;
            case 2:
                SceneManager.LoadScene("SampleScene");
                UnitRoot.Instance.transform.position = new Vector3(104.5f, 0.997264f, 1);
                UnitRoot.Instance.isPaused = false;
                UnitRoot.Instance.rb.simulated = true;
                UnitRoot.Instance.gameObject.SetActive(true);
               
                break;
            case 3:
                SceneManager.LoadScene("SampleScene");
                UnitRoot.Instance.transform.position = new Vector3(10, -17.7f, -0.16f);
                UnitRoot.Instance.isPaused = false;
                UnitRoot.Instance.rb.simulated = true;
                UnitRoot.Instance.gameObject.SetActive(true);
                
                break;
            case 4:
                SceneManager.LoadScene("SampleScene");
                UnitRoot.Instance.transform.position = new Vector3(-15.27981f, -24.00274f, 0);
                UnitRoot.Instance.isPaused = false;
                UnitRoot.Instance.rb.simulated = true;
                UnitRoot.Instance.gameObject.SetActive(true);
              
                break;
            case 5:
                SceneManager.LoadScene("SampleScene");
                UnitRoot.Instance.transform.position = new Vector3(110.1f, -16.00037f, 0);
                UnitRoot.Instance.isPaused = false;
                UnitRoot.Instance.rb.simulated = true;
                UnitRoot.Instance.gameObject.SetActive(true);
              
                break;
            case 6:
                SceneManager.LoadScene("SampleScene");
                UnitRoot.Instance.transform.position = new Vector3(159.1858f, 1.99959f, 0);
                UnitRoot.Instance.isPaused = false;
                UnitRoot.Instance.rb.simulated = true;
                UnitRoot.Instance.gameObject.SetActive(true);
              
                break;
            default:
                Debug.LogWarning("Invalid level number: " + level);
                return;
        }

        
    }

    public void ChangeScene(string scene)
    {
  
        if (!string.IsNullOrEmpty(scene))
        {
            SceneManager.LoadScene(scene);
        }
        else
        {
            Debug.LogError("Scene name is not assigned!");
        }
    }
}
