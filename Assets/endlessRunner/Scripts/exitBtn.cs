using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class exitBtn : MonoBehaviour
{

    public void ExitBtn()
    {
        SceneManager.LoadScene("gameList");
    }

}
