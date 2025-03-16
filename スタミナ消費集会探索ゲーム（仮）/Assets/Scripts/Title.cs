using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{

    bool start=false;//˜A‘±‰Ÿ‚µ–hŽ~


    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown && !start)
        {
            SceneManager.LoadScene("Quest");
        }
    }
}
