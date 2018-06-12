using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingBar : MonoBehaviour {

    Image foregroundImage;
    float fill;

	// Use this for initialization
	void Start () {
        foregroundImage = GetComponent<Image>();
        fill = 0f;
        foregroundImage.fillAmount = fill;
	}
	
	// Update is called once per frame
	void Update () {
        if (fill >= 1f)
        {
            fill = 0;
        }
        else
        {
            fill += 0.005f;
        }
        foregroundImage.fillAmount = fill;
	}
}
