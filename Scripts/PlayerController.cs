using System.Runtime.CompilerServices;
using System.Collections;
using NUnit.Framework.Constraints;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Scripting.APIUpdating;
using UnityEditor.UI;
using System;


public class PlayerController : MonoBehaviour
{
#region Variables
//Get_Pos()
public Vector3 targetPosition;
private bool isMoving = false;
public Vector3 yclickoffset;


//move()
public float speed = 0.5f;
private float gradual = 0;
public float speedcap;
public float jumpforce = 1f;
private bool battleposlock = false;
private bool movecheck = true;


//Time Control
private float timerjump = 0f;
private float timermove = 0f;
public float interval = 1f;
public float intervalmove = 0.1f;


//Self Reference
private Rigidbody rb;
private Animator animator;


//random
public Vector3 spawn = new Vector3(0,0.5f,0);
public static Collision touchedenemy;


#endregion
#region Get_Pos
    public void Get_Pos()
    {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                targetPosition = hit.point + yclickoffset;
                isMoving = true;
                animator.SetBool("walking", true); 
            }

    }
#endregion
#region Move
    public void move(Vector3 pos)
    {
        if (isMoving == true)
        {
            //teleport
            //transform.position = targetPosition + new Vector3(0,0.5f,0);
            if (gradual < 1)
            {
                gradual += Time.deltaTime * speed;


                //speed cap check
                if (gradual >= speedcap)
                {
                    gradual = speedcap;
                }

                transform.position = Vector3.MoveTowards(transform.position,pos,gradual);
            }
        }

        if (transform.position == targetPosition)
        {
            gradual = 0;
            isMoving = false;
            animator.SetBool("walking", false);
        }
    }
#endregion
#region Start
    void Start()
    {
        targetPosition = transform.position;
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        isMoving = false;
        animator.SetBool("walking", false); 
    }
    #endregion
#region Update
    void Update()
    {
        //get mouse position at a set rate
        if (Input.GetMouseButton(0))
        {
            if (timermove >= intervalmove)
                {
                    Get_Pos();
                    timermove = 0f; // Reset timer
                }  
            
        }
        
        
        //moving the character
        if (movecheck == true)
        {
            move(targetPosition);
        }
            
        
        
        //timer
        timerjump += Time.deltaTime;
        timermove += Time.deltaTime;

        //move player to spot when battle starts
        if (battleposlock == true)
        {   
            move(GameController.playerbattlepos);
            if (transform.position == targetPosition)
            {
                battleposlock = false;
            }
            
        }
           
  





    }
#endregion
#region Collision Control
//Collision Control
      void OnCollisionEnter(Collision random)
    {
        if (random.gameObject.CompareTag("ground"))
        {
            ContactPoint contact = random.contacts[0];
            Vector3 normal = contact.normal;
            if (Mathf.Abs(normal.y) < 0.5f)
            {
                if (timerjump >= interval)
                {
                    rb.AddForce(transform.up * jumpforce, ForceMode.Impulse);
                    Debug.Log("jump!");
                    timerjump = 0f; // Reset timer
                }   
            }  
        }

         //respawn if touch water
         if (random.gameObject.CompareTag("water"))
         {
            transform.position = spawn;
            isMoving = false;
            animator.SetBool("walking", false);
            Debug.Log("drown");
         }   

        //start battle if player runs into an enemy
         if (random.gameObject.CompareTag("enemy"))
         {
            //start battle set up
            GameController.battlesetup = true;
            //stop current movement
            isMoving = false;
            //turn off movement
            movecheck = false;
            
         }
         
         
    }
    #endregion
}

