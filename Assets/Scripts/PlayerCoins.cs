using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCoins : MonoBehaviour
{
//int points = 0;
    // Start is called before the first frame update
    void Start()
    {
      //  int points = 0;
    }

    // Update is called once per frame
    void Update()
    {
     //   Debug.Log(points);
    }
    void OnControllerColliderHit(ControllerColliderHit hit){
	
		if(hit.gameObject.CompareTag("moneta")){
		//	points++;
			
		}
	
	}
}
