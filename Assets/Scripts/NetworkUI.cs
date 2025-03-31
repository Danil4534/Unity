using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NetworkUI : MonoBehaviour
{
    [SerializeField] private Button startHostButton;
    [SerializeField] private Button startClientButton;
    [SerializeField] private string sceneName;

    public void ChangeSceneState()
    {
        if (sceneName == "PauseScene")
        {
            PlayerPrefs.SetFloat("Hero_PosX_Slot" + 0, UnitRoot.Instance.transform.position.x);
            PlayerPrefs.SetFloat("Hero_PosY_Slot" + 0, UnitRoot.Instance.transform.position.y);
            PlayerPrefs.SetFloat("Hero_PosZ_Slot" + 0, UnitRoot.Instance.transform.position.z);
            UnitRoot.Instance.isPaused = true;
            UnitRoot.Instance.rb.simulated = false;
            UnitRoot.Instance.gameObject.SetActive(false);
        }
        SceneManager.LoadScene(sceneName);
    }
    void Start()
    {
        startHostButton.onClick.AddListener(StartAsHost);
        startClientButton.onClick.AddListener(StartAsClient);
    }

    private void StartAsHost()
    {
        SceneManager.LoadScene(sceneName);

        if (NetworkManager.Singleton.StartHost())
        {
            Debug.Log("Started as Host");
        }
        else
        {
            Debug.LogError("Failed to start as Host");
        }

    }

    private void StartAsClient()

    {
        SceneManager.LoadScene(sceneName);

        if (NetworkManager.Singleton.StartClient())
        {
            Debug.Log("Started as Client");
        }
        else
        {
            Debug.LogError("Failed to start as Client");
        }
    }
}
