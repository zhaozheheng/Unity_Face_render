using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menus : MonoBehaviour {

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnGUI () {
        if (GUI.Button(new Rect(460, 0.3f * Screen.height, 160, 80), "Capture Video"))
        {
            SceneManager.LoadScene("VideoPlayer");
        }

        if (GUI.Button(new Rect(460, 0.6f * Screen.height, 160, 80), "Mesh Player"))
        {
            SceneManager.LoadScene("MeshRender");
        }
    }
}
