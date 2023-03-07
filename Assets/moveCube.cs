using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveCube : MonoBehaviour
{

    public bool moveAnyway;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveThisCube();
    }




    private void MoveThisCube()
    {
        if(Input.GetKeyDown(KeyCode.Space) || moveAnyway)
        {
            transform.Translate(5, 5, 5);
            print("Done");
        }
    }
}
