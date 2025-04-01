using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class BindingKey : MonoBehaviour
{
    public TMP_Text moveLeftText;
    public TMP_Text moveRightText;
    public TMP_Text jumpText;
    public TMP_Text attackText;


    public TMP_InputField moveLeftInput;
    public TMP_InputField moveRightInput;
    public TMP_InputField jumpInput;
    public TMP_InputField attackInput;



    private void Start()
    {
       
        LoadSavedKeys();
    }

    private void LoadSavedKeys()
{
    if (moveLeftText != null)
        moveLeftText.text = PlayerPrefs.GetString("MoveLeftKey", "A");
    else
        Debug.LogWarning("moveLeftText is not assigned!");

    if (moveRightText != null)
        moveRightText.text = PlayerPrefs.GetString("MoveRightKey", "D");
    else
        Debug.LogWarning("moveRightText is not assigned!");

    if (jumpText != null)
        jumpText.text = PlayerPrefs.GetString("JumpKey", "Space");
    else
        Debug.LogWarning("jumpText is not assigned!");

    if (attackText != null)
        attackText.text = PlayerPrefs.GetString("AttackKey", "Mouse0");
    else
        Debug.LogWarning("attackText is not assigned!");

 
}


    public void SaveCustomBindings()
    {
      
        string moveLeft = moveLeftInput.text;
        string moveRight = moveRightInput.text;
        string jump = jumpInput.text;
        string attack = attackInput.text;
       

        if (!string.IsNullOrEmpty(moveLeft))
        {
            PlayerPrefs.SetString("MoveLeftKey", moveLeft);
        }
        if (!string.IsNullOrEmpty(moveRight))
        {

         PlayerPrefs.SetString("MoveRightKey", moveRight);
        }
        if (!string.IsNullOrEmpty(jump))
        {
         PlayerPrefs.SetString("JumpKey", jump);
        }
        if (!string.IsNullOrEmpty(attack))
        {
          PlayerPrefs.SetString("AttackKey", attack);
        }
        PlayerPrefs.Save();
        LoadSavedKeys();
    }
}
