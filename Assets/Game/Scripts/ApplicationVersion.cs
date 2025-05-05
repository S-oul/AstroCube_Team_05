using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ApplicationVersion : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    void Start()
    {
        text.text = " Version : " + Application.version;
    }

}
