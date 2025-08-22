using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    IPawn character;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // This method is called when the script instance is being loaded
    void Awake()
    {
        character = GetComponent<IPawn>();
    }

    // Update is called once per frame
    void Update()
    {
        //ÌøÔ¾
        if (Input.GetButtonDown("Jump"))
        {
            character.Jump();
        }
    }

    //FixedUpdate is called at a fixed interval and is used for physics calculations
    void FixedUpdate()
    {
        //ÒÆ¶¯
        float moveInput = Input.GetAxis("Horizontal");
        character.Move(moveInput);
    }
}
