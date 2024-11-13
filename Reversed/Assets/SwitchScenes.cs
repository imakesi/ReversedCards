using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScenes : MonoBehaviour
{
    private void OnMouseDown() {
        Debug.Log("Switch");
        SceneManager.LoadScene("SwitchScene", LoadSceneMode.Single);
    }
}
