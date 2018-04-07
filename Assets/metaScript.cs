using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class metaScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            other.gameObject.GetComponent<SimpleCharacterControl>().m_animator.Play("PickUp");
            Destroy(gameObject);

        }
    }
}
