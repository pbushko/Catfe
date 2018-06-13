using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingBar : MonoBehaviour {

    public GameObject foreground;
    public GameObject background;
    Image foregroundImage;
    bool finished;
    bool processing;
    Sprite food;
    public SpriteRenderer foodRenderer;
    Recipe recipe;
    float fill;
    float time;

	// Use this for initialization
	void Start () {
        foregroundImage = foreground.GetComponent<Image>();
        foreground.SetActive(false);
        background.SetActive(false);
        finished = false;
        processing = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (foregroundImage.IsActive())
        {
            if (fill <= 1f)
            {
                processing = true;
                fill += 0.01f/time;
            }
            else
            {
                processing = false;
                foreground.SetActive(false);
                background.SetActive(false);
                finished = true;
                
                foodRenderer.sprite = food;
                Debug.Log("got here..." + foodRenderer.sprite);
            }
            foregroundImage.fillAmount = fill;
        }
	}

    public Recipe loading(float fillTime, Sprite f, Recipe r)
    {
        if (!finished && !processing)
        {
            food = f;
            recipe = r;
            foreground.SetActive(true);
            background.SetActive(true);
            time = fillTime;
            return null;
        }
        else if (finished)
        {
            foodRenderer.sprite = null;
            reset();
            return recipe;
        }
        return null;
    }

    public void reset()
    {
        fill = 0f;
        foregroundImage.fillAmount = fill;
        finished = false;
    }
}
