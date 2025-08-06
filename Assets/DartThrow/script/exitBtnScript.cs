using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class exitBtnScript : MonoBehaviour
{
    public void exitBtn() {

        SceneManager.LoadScene("gameList");

    }
}
