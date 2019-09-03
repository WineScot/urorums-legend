using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallJump : MonoBehaviour {
    
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Enemy")
        {
           
            float vel = Mathf.Sqrt(24000 * Mathf.Pow(this.transform.parent.transform.localScale.y, 1.15f)); //Mathf.Sqrt(2000 * height);
            other.gameObject.GetComponent<ScorpionAI>().ChangeVelocity(new Vector2(0, vel));// tu trzeba zrobić tak by dzialało dla wszystkich
        }
    }
}
