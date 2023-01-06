using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Return : MonoBehaviour
{
    public Button rtnButton;

    public void rtn()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

}
