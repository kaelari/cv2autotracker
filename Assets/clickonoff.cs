using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
public class clickonoff : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        toggle();


    }

    public void toggle()
    {
        //Debug.Log("clicked!");
        Image image = this.gameObject.GetComponent<Image>();
        Color color = image.color;
        if (color.a == 1)
        {
            color.a = 0.15f;
        }else
        {
            color.a = 1.0f;
        }
        image.color = color;
    }
    public void toggleon()
    {
        Image image = this.gameObject.GetComponent<Image>();
        Color color = image.color;
        color.a = 1.0f;
        image.color = color;
    }
    public void toggleoff()
    {
        Image image = this.gameObject.GetComponent<Image>();
        Color color = image.color;
        color.a = 0.15f;
        image.color = color;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
