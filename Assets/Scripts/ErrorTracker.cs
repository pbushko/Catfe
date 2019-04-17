using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ErrorTracker : MonoBehaviour
{
    private static Text errorText;

	// Use this for initialization
	void Start ()
    {
        errorText = GetComponent<Text>();
	}

    public static void ChangeErrorText(string s)
    {
        errorText.text = s;
    }

}
