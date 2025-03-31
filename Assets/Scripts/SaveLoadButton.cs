using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class SaveLoadButton : MonoBehaviour
{
    public TMP_Text slot1Info;
    public Button saveSlot1Button;
    public Button LoadSlot1Button;
    public TMP_Text slot2Info;
    public Button saveSlot2Button;
    public Button LoadSlot2Button;
    public TMP_Text slot3Info;
    public Button saveSlot3Button;
    public Button LoadSlot3Button;

    void Start()
    {
        //slot1Info.text = $"X: {PlayerPrefs.GetFloat("Hero_PosX_Slot1", 0):F2} | Y: {PlayerPrefs.GetFloat("Hero_PosY_Slot1", 0):F2} | Hearts: {PlayerPrefs.GetFloat("Hero_Health_Slot1", 0):F2}";
        //saveSlot1Button.onClick.AddListener(() => SaveGame(1));
        //LoadSlot1Button.onClick.AddListener(() => LoadGame(1));

        //slot2Info.text = $"X: {PlayerPrefs.GetFloat("Hero_PosX_Slot2", 0):F2} | Y: {PlayerPrefs.GetFloat("Hero_PosY_Slot2", 0):F2} | Hearts: {PlayerPrefs.GetFloat("Hero_Health_Slot2", 0):F2}";
        //saveSlot2Button.onClick.AddListener(() => SaveGame(2));
        //LoadSlot2Button.onClick.AddListener(() => LoadGame(2));

        //slot3Info.text = $"X: {PlayerPrefs.GetFloat("Hero_PosX_Slot3", 0):F2} | Y:{PlayerPrefs.GetFloat("Hero_PosY_Slot3", 0):F2} | Hearts: {PlayerPrefs.GetFloat("Hero_Health_Slot3", 0):F2}";
        //saveSlot3Button.onClick.AddListener(() => SaveGame(3));
        //LoadSlot3Button.onClick.AddListener(() => LoadGame(3));
    }

    public void SaveGame(int slot)
    {
        if (UnitRoot.Instance == null)
        {
            Debug.LogWarning("UnitRoot instance is not set. Cannot save the game.");
            return;
        }

        PlayerPrefs.SetFloat("Hero_PosX_Slot" + slot, UnitRoot.Instance.transform.position.x);
        PlayerPrefs.SetFloat("Hero_PosY_Slot" + slot, UnitRoot.Instance.transform.position.y);
        PlayerPrefs.SetFloat("Hero_PosZ_Slot" + slot, UnitRoot.Instance.transform.position.z);
        slot1Info.text = $"X: {PlayerPrefs.GetFloat("Hero_PosX_Slot1", 0):F2} | Y: {PlayerPrefs.GetFloat("Hero_PosY_Slot1", 0):F2} | Hearts: {PlayerPrefs.GetFloat("Hero_Health_Slot1", 0):F2}";
        slot2Info.text = $"X: {PlayerPrefs.GetFloat("Hero_PosX_Slot2", 0):F2} | Y: {PlayerPrefs.GetFloat("Hero_PosY_Slot2", 0):F2} | Hearts: {PlayerPrefs.GetFloat("Hero_Health_Slot2", 0):F2}";
        slot3Info.text = $"X: {PlayerPrefs.GetFloat("Hero_PosX_Slot3", 0):F2} | Y:{PlayerPrefs.GetFloat("Hero_PosY_Slot3", 0):F2} | Hearts: {PlayerPrefs.GetFloat("Hero_Health_Slot3", 0):F2}";
        PlayerPrefs.SetFloat("Hero_Health_Slot" + slot, UnitRoot.Instance.lives);
        PlayerPrefs.Save();
        Debug.Log("Game saved to slot " + slot);
    }

    public void LoadGame(int slot)
    {
        if (PlayerPrefs.HasKey("Hero_PosX_Slot" + slot))
        {
            float posX = PlayerPrefs.GetFloat("Hero_PosX_Slot" + slot);
            float posY = PlayerPrefs.GetFloat("Hero_PosY_Slot" + slot);
            float posZ = PlayerPrefs.GetFloat("Hero_PosZ_Slot" + slot);

            if (UnitRoot.Instance != null)
            {

                UnitRoot.Instance.transform.position = new Vector3(posX, posY, posZ);
                UnitRoot.Instance.lives = PlayerPrefs.GetFloat("Hero_Health_Slot" + slot);
                //UnitRoot.Instance.UpdateHealthUI();
                UnitRoot.Instance.rb.simulated = true;
                UnitRoot.Instance.isPaused = false;
                UnitRoot.Instance.gameObject.SetActive(true);
                Debug.Log("Game loaded from slot " + slot);
                SceneManager.LoadScene("SampleScene");
            }
        }
        
    }
}
