using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    [SerializeField] KeyCode restartButton = KeyCode.R;
    [SerializeField] Text subText;

    private void Start()
    {
        subText.text = "Press " + restartButton + " to play a new level";
    }

    private void Update()
    {
        if (Input.GetKeyDown(restartButton))
        {
            SceneManager.LoadSceneAsync(0);
        }
    }


}
