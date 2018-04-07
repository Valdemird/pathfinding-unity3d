using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyEnemy : MonoBehaviour {
    MapCreatorScript mapCreatorScript;
	// Use this for initialization
	void Start () {
        mapCreatorScript = GameObject.FindGameObjectWithTag("mapcreator").GetComponent<MapCreatorScript>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player")) {
            foreach (GameObject element in mapCreatorScript.turttles) {
                GameObject.Destroy(element);
            }
            
        }
    }

}
