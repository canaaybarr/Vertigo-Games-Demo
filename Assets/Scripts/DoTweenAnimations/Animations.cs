using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.UI;
using DG.Tweening;


public class Animations : Singleton<Animations>
{
    
    public Text animatedText;
    public int targetrewardText;
    public bool textAnimationActive;

    void Start()
    {
        InvokeRepeating(nameof(TextAnimation), 0.0001f, 0.000020f);
    }
    void TextAnimation()
    {
        if (textAnimationActive)
        {
            animatedText.text = (int.Parse(animatedText.text) + 10).ToString();
    
            if (int.Parse(animatedText.text) >= targetrewardText)
            {
                animatedText.text = targetrewardText.ToString();
                textAnimationActive = false;
            }
        }
    }

}
