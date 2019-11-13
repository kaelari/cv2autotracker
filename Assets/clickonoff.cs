using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
public class clickonoff : MonoBehaviour
{
    controller controller;
    // Start is called before the first frame update
    void Start()
    {
        controller = FindObjectOfType<controller>();
        controller.olditems[this.name] = false;
    }

    public void toggle()
    {
        //Debug.Log("clicked!");
        Debug.Log("toggling");
        if (controller.olditems[this.name])
        {
            controller.olditems[this.name] = false;
        }else
        {
            controller.olditems[this.name] = true;
        }
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
        bool on;
        if (controller.olditems.TryGetValue(this.name, out on))
        { 
            if (on)
            {
                toggleon();
            }
            else
            {
                toggleoff();
            }
        }
    }
}
