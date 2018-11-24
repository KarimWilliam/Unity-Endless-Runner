using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreSphereScript : MonoBehaviour {
    float originalY;
    float floatStrength = 1/8f;
    float x;
    public short color;



    // Use this for initialization
    void Start () {
        this.originalY = this.transform.position.y;
        x = Random.Range(0f, 180f);

    }

    // Update is called once per frame
    void Update () {
        transform.position = new Vector3(transform.position.x,
            originalY + ((float)Mathf.Sin((Time.time+x)*5) / 10),
            transform.position.z);
    }
}
