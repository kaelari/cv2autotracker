using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controller : MonoBehaviour
{
    // Start is called before the first frame update

    public Dictionary<string, bool> curitems = new Dictionary<string, bool>();
    public Dictionary<string, bool> olditems = new Dictionary<string, bool>();
    public GameObject square;
    public GameObject flat;
    void Start()
    {
        
    }
    public void gowide()
    {
        Screen.SetResolution(1000, 50, false);
        square.SetActive(false);
        flat.SetActive(true);
    }
    public void gosquare()
    {
        Screen.SetResolution(500, 400, false);
        square.SetActive(true);
        flat.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        foreach (var item in curitems.Keys)
        {
            if (curitems[item])
            {
                olditems[item] = true;
            }
        }
        curitems.Clear();
    }
}
