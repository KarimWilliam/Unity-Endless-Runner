using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformScript : MonoBehaviour {
    public GameObject ScoreSphere;
    public Vector3 speed;
    public short color = 1;
    public int ZoneFreq=5; //how frequent the changing color zones appear
    public bool Balls =true;
    public GameObject DirectionalGameMode;
    float speedMult;



    void Start () {
        speed = new Vector3 (0, 0, -5f);
        // score sphere initialization
        float h = Random.Range(1, 6);
        int[] values = new int[9];

        for (int i = 1; i < 9; i++)
        {
            values[i] = i;
        }
        ShuffleArray<int>(values);

        if (Balls)
        {
            for (int k = 0; k < h; k++)
            {
           
                spawnSphere(values[k]);

            }
            
        }
        ColorChanger();

        DirectionalGameMode = GameObject.FindWithTag("GameController");

    }
	
	// Update is called once per frame
	void Update () {
        MyGameMode MGMScript = DirectionalGameMode.GetComponent<MyGameMode>();
        speedMult=MGMScript.speedMult;
        this.transform.position += speed* speedMult * Time.deltaTime;

    }



    void spawnSphere(int n)
    {

        // X and Y of the GRID
        float x1 = 1.165f-3.5f;
        float x2 = 0;
        float x3 = 5.8f-3.5f;
        float y1 = 1.125f-4.5f;   //3.375f-4.5f
        float y2 = 3.375f-4.5f;
        float y3 = 5.625f-4.5f;
        float y4 = 7.875f-4.5f;
        float x; float y;

        switch (n)
        {
            case 0:
                x = x1; y = y1;
                break;
            case 1:
                x = x1; y = y2;
                break;
            case 2:
                x = x1; y = y3;
                break;
            case 3:
                x = x2; y = y1+1;
                break;
            case 4:
                x = x2; y = y2+1;
                break;
            case 5:
                x = x2; y = y3+1;
                break;

            case 6:
                x = x3; y = y1;
                break;
            case 7:
                x = x3; y = y2;
                break;
            case 8:
                x = x3; y = y3;
                break;

            default:
                x = 0; y = 0;
                break;
        }

        GameObject g = Instantiate(ScoreSphere);
        // float x = this.transform.position.x+ Random.Range(-3.5f, 3.5f);
        // float z =this.transform.position.z+ Random.Range(-4.5f, 4.5f);
         float w = this.transform.position.x+x;
         float z =this.transform.position.z+y;


        int color = Random.Range(1, 4);
        if (color == 1) {

   
            g.GetComponent<Renderer>().material.color = new Color32(0x00, 0x03, 0x5B, 0xFF); 
            g.GetComponent<Light>().color = new Color32(0x00, 0x03, 0x5B, 0xFF);
            ScoreSphereScript SSScript = g.GetComponent<ScoreSphereScript>();
            SSScript.color = 1;

        }
        else if (color == 2) //11FF00
        {
            g.GetComponent<Renderer>().material.color = new Color32(0x11, 0xff, 0x00, 0xFF);
            g.GetComponent<Light>().color = new Color32(0x11, 0xff, 0x00, 0xFF); 
            ScoreSphereScript SSScript = g.GetComponent<ScoreSphereScript>();
            SSScript.color = 2;
        }
        else
        {
            g.GetComponent<Renderer>().material.color = new Color32(0xB0, 0x00, 0x0B, 0xFF);
            g.GetComponent<Light>().color = new Color32(0x72, 0x00, 0x07, 0xFF);
            ScoreSphereScript SSScript = g.GetComponent<ScoreSphereScript>();
            SSScript.color = 3;
        }
        g.transform.position = new Vector3(w, g.transform.position.y, z);
        g.transform.parent = this.transform;


    }

    void ColorChanger()
    {
        GameObject ColorZone = this.transform.Find("ColorZone").gameObject;

        int y = Random.Range(1, 4);
        if (ZoneFreq != 0) {
            Destroy(ColorZone);
        }

       

        if (ColorZone != null)
        {
            if (y == 1)
            {
                //blue 00035B
                Material mymat = ColorZone.GetComponent<Renderer>().material;
                mymat.SetColor("_EmissionColor", new Color32(0x00, 0x03, 0x5B, 0xFF));
                color = 1;
            }
            else if (y == 2)
            {
                //green 042906
                Material mymat = ColorZone.GetComponent<Renderer>().material;
                mymat.SetColor("_EmissionColor", new Color32(0x04, 0x29, 0x06, 0xFF));
                color = 2;
            }
            else
            {
                //red B0000B  720107
                Material mymat = ColorZone.GetComponent<Renderer>().material;
                mymat.SetColor("_EmissionColor", new Color32(0x72, 0x00, 0x07, 0xFF));
                color = 3;
            }

            
        }
      
    }


    public static void ShuffleArray<T>(T[] arr)
    {
        for (int i = arr.Length - 1; i > 0; i--)
        {
            int r = Random.Range(0, i + 1);
            T tmp = arr[i];
            arr[i] = arr[r];
            arr[r] = tmp;
        }
    }

}
