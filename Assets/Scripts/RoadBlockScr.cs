using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RoadBlockScr : MonoBehaviour
{

    GameManager GM;
    Vector3 moveVec;


    // Start is called before the first frame update
    void Start()
    {
        GM = FindObjectOfType<GameManager>();
        moveVec = new Vector3(-1, 0, 0);
        return;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        {
            if (GM.CanPlay)
                transform.position += (moveVec * Time.deltaTime * GM.MoveSpeed);
        }

    }
}

