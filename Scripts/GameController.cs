using System.Collections.Generic;
using JetBrains.Annotations;
using NUnit.Framework.Internal;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SocialPlatforms.GameCenter;
using UnityEngine.UIElements;

public class GameController : MonoBehaviour
{
    public GameObject player;
    public Vector3 battlefield_center;
    public static bool battlesetup = false;

    public int rows = 5;
    public int columns = 5;

    public GameObject battletile;
    public static Vector3 playerbattlepos;
    public static Vector3 enemybattlepos;
    public float tilefix = 0.5f;

   




    public void start_battle()
    {
        //get player pos and round
        battlefield_center = new Vector3(Mathf.Round(player.transform.position.x),5,Mathf.Round(player.transform.position.z));
        //Debug.Log(battlefield_center);

        //set player spot
        playerbattlepos = new Vector3(battlefield_center.x-(rows/2),Mathf.Round(player.transform.position.y),battlefield_center.z+(columns/2));
        //Debug.Log(playerbattlepos);

        //set enemy spot
        enemybattlepos = new Vector3(battlefield_center.x+(rows/2),Mathf.Round(player.transform.position.y),battlefield_center.z-(columns/2));
        enemybattlepos += new Vector3(tilefix,0,tilefix);


        //set rounded pos to top left
        battlefield_center = battlefield_center + new Vector3((rows/2),0,(columns/2));



        



        //cycle through all cords
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < columns; c++)
            {
                Hit_check();
                battlefield_center.z -= 1;
            }  
            //restore the z to where it should be for the loop
            battlefield_center.z += 9;
            //lower x by 1
            battlefield_center.x -= 1; 
        }
        
        //move player to new bottom left
        player.transform.position = playerbattlepos;

        //move enemy to new position
        
        
        

        
        
        
        //stop the set up
        battlesetup = false;
    }
        

    public void Hit_check()
    {
        //ray cast down from rounded pos
        if (Physics.Raycast(battlefield_center,Vector3.down,out RaycastHit hit,Mathf.Infinity))
            {
                //spawn prefab using the hit.transform.position.y + 0.1
                Instantiate(battletile,new Vector3(battlefield_center.x,hit.transform.position.y+0.1f,battlefield_center.z), Quaternion.identity);
            }
        else
        {
            Debug.Log("Miss!");
        }
    }





    

















    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (battlesetup == true)
        {
            start_battle();
        }
    }

    void OnCollisionEnter(Collision random)
    {

    }




}
