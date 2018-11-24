using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;


public class MainBall : MonoBehaviour {

    public Rigidbody rb;
    private float forceMult = 700f;
    private Vector3 RespawnLocation;
    public GameObject Platform;
    public GameObject FirstPlatform;
    public GameObject DirectionalGameMode;
    public int Score=0;
    private short color=1;
    float originalY;
    float floatStrength = 1 / 8f;
    float x;
    bool platsize = true;
    short ZoneFreqO = 5; //Frequency of colorzones
    public short First2 = 2;
    public int speedMult = 1;
    short mycolor=1;
    public AudioClip GoodPickUp;
    public AudioClip BadPickUp;
    public AudioClip ColorChange;
    public AudioClip Death;
    public Camera firstPersonCamera;
    public float transitionDuration = 2;
    List<GameObject> allTheFloors = new List<GameObject>();
    public Canvas PauseCanvas;
    public Canvas MainMenuCanvas;
    public Slider volumeSlider;
    public Slider volumeSlider2;
    public Text ScoreText;
    public int HighestScore = 0;
    public Text HighestScoreText;


    // Use this for initialization
    public void Start () {
        speedMult = 1;
        AudioListener.volume =PlayerPrefs.GetFloat("Volume");
        volumeSlider.value = PlayerPrefs.GetFloat("Volume");
        volumeSlider2.value = PlayerPrefs.GetFloat("Volume");
        Debug.Log("volume is:");
        Debug.Log(PlayerPrefs.GetFloat("Volume"));
        ScoreText.text = Score.ToString();
        ScoreText.color = new Color32(0xFF, 0xFF, 0xFF, 0xFF);
        AudioListener.volume = volumeSlider.value;
        rb = GetComponent<Rigidbody>();
        this.originalY = this.transform.position.y;
        FirstPlatform = Instantiate(Platform);
        allTheFloors.Add(FirstPlatform);
        FirstPlatform.transform.position = new Vector3(0, 0, -11);

        //Initialize first few platforms
        for (int i = 0; i < 11; i++)
        {
            InitialPlatforms();
        }

    }

    // Update is called once per frame
    void FixedUpdate() {
        //transform.Translate(Input.acceleration.x * forceMult/2, 0, 0);
        float moveHorizontal = Input.acceleration.x* 2;
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, 0.0f);
        rb.AddForce(movement * forceMult );

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        { 
            rb.AddForce(Vector3.left * forceMult, ForceMode.Force);
            //rb.velocity = new Vector3(-1, 0,0) * forceMult;
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
             rb.AddForce(Vector3.right * forceMult, ForceMode.Force);
           // rb.velocity = new Vector3(1, 0, 0) * forceMult;
        }

       // if (Input.GetKeyUp (KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow) ||Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow))
        //{
       //      rb.AddForce(Vector3.right * forceMult, ForceMode.Force);
           // rb.velocity = new Vector3(0, 0, 0) * forceMult;
       // }

        transform.position = new Vector3(transform.position.x,
        originalY + ((float)Mathf.Sin((Time.time + x) * 5) / 10),
        transform.position.z);
    }

    public void InitialPlatforms()
    {

       

        GameObject g = Instantiate(Platform);
        allTheFloors.Add(g);
        float x = FirstPlatform.transform.position.x;
            float z = FirstPlatform.transform.position.z + 9.9f;
            g.transform.position = new Vector3(x, g.transform.position.y, z);
            FirstPlatform = g;
             PlatformScript pScript = g.GetComponent<PlatformScript>();

        if (First2 > 0) { pScript.Balls = false; First2--; }
       


        if (ZoneFreqO > 1) {
            pScript.ZoneFreq = 1;
            ZoneFreqO--;
        }
        else
        {
            pScript.ZoneFreq = 0;
            ZoneFreqO = 5; //Frequency of colorzones
        }
            


        if (platsize)
        {
            GameObject platWall1 = g.transform.Find("Wall1").gameObject;
            GameObject platWall2 = g.transform.Find("Wall2").gameObject;
            GameObject horn1 = g.transform.Find("Horn1").gameObject;
            GameObject horn2 = g.transform.Find("Horn2").gameObject;
            platWall1.transform.localScale += new Vector3(0.2F, 0.2f, 0);
            platWall2.transform.localScale += new Vector3(0.5F, 0.5f, 0);
            Destroy(horn1);
            Destroy(horn2);

        }

          platsize = !platsize;
        
    }

     void OnTriggerEnter(Collider other)
    {
        MyGameMode MGMScript = DirectionalGameMode.GetComponent<MyGameMode>();
        if (other.tag == "Respawn")
        {
            InitialPlatforms();

            StartCoroutine(DestoyPlatform(other.transform.parent.gameObject, 3f));
        }

        if (other.tag == "ScoreSphere")
        {
            
            ScoreSphereScript SSScript = other.GetComponent<ScoreSphereScript>();
            int Scorecolor = SSScript.color;

            if (Scorecolor == color)
            {
                Score += 10;
                if (Score > HighestScore) { HighestScore = Score; }
                AudioSource audio = GetComponent<AudioSource>();
                audio.PlayOneShot(GoodPickUp);
            }
            else
            {
                Score = Score / 2;
                AudioSource audio = GetComponent<AudioSource>();
                audio.PlayOneShot(BadPickUp);
            }

            if (Score == 0)
            {

                HighestScoreText.text = "Your Highest Score is:  "+ HighestScore.ToString();
                Debug.Log("you lost");
                AudioSource audio = GetComponent<AudioSource>();
                audio.PlayOneShot(Death);
                speedMult =0;
                MGMScript.speedMult = 0;
                Destroy(other.gameObject);
                this.GetComponent<MeshRenderer>().enabled = false;
               // this.rb.constraints = RigidbodyConstraints.FreezePositionX;
                StartCoroutine(Transition());

                MGMScript.GameOver();





                return;
            }
            ScoreText.color = new Color32(0xFF, 0xFF, 0xFF, 0xFF);
            ScoreText.text =  Score.ToString() ;
            if (Score > 100)
            {
                ScoreText.color = new Color32(0xFF, 0x8B, 0x00, 0xFF); //orange FF8B00
            }
            if (Score > 200)
            {
                ScoreText.color = new Color32(0x72, 0x00, 0x07, 0xFF);
            }
            if (Score > 300)//FF00DB Pink
            {
                ScoreText.color = new Color32(0xFF, 0x00, 0xDB, 0xFF);
            }
            if (Score > 400)
            {
                ScoreText.text = Score.ToString() + " O_o";
            }
            if (Score > 500)
            {
                ScoreText.text = Score.ToString() + " O_o ?????";
            }
            if (Score > 550)
            {
                ScoreText.text = Score.ToString() + " O_o Dev = Surpassed";
            }
            if (Score > 700)
            {
                ScoreText.text = Score.ToString() + " Whata God";
            }

            if (Score > 1000)
            {
                ScoreText.text = Score.ToString() + " Cheat!!!";
            }

            speedMult = 1 + Score / 50;
            MGMScript.speedMult = speedMult;



            Destroy(other.gameObject);
            Debug.Log(Score.ToString());
        }

        if(other.tag== "ColorZone")
        {
            //HOW TO REFERENCE SCRIPTS ***
            GameObject k = other.transform.parent.gameObject;
            PlatformScript pScript = k.GetComponent<PlatformScript>();
            color = pScript.color;


            if (color == 1 && mycolor!=1)//blue
            {
                MaterialPropertyBlock props = new MaterialPropertyBlock();
                props.SetColor("_Color", Color.yellow);
                GetComponent<Renderer>().SetPropertyBlock(props);
                Material mymat = this.GetComponent<Renderer>().material;
                mymat.SetColor("_EmissionColor", new Color32(0x00, 0x03, 0x5B, 0xFF));
                Light lt = GetComponent<Light>();
                lt.color = new Color32(0x00, 0x03, 0x5B, 0xFF);
                mycolor = 1;
                AudioSource audio = GetComponent<AudioSource>();
                audio.PlayOneShot(ColorChange);
            }
            else if(color==2 && mycolor != 2)//green
            {
   
                MaterialPropertyBlock props = new MaterialPropertyBlock();
                props.SetColor("_Color", Color.yellow);
                GetComponent<Renderer>().SetPropertyBlock(props);
                Material mymat = this.GetComponent<Renderer>().material;
                mymat.SetColor("_EmissionColor", new Color32(0x04, 0x29, 0x06, 0xFF));
                Light lt = GetComponent<Light>();
                lt.color = new Color32(0x04, 0x29, 0x06, 0xFF);
                mycolor = 2;
                AudioSource audio = GetComponent<AudioSource>();
                audio.PlayOneShot(ColorChange);

            }
            else if(color == 3 && mycolor != 3)//red 720107
            {
          
                MaterialPropertyBlock props = new MaterialPropertyBlock();
                props.SetColor("_Color", Color.yellow);
                GetComponent<Renderer>().SetPropertyBlock(props);
                Material mymat = this.GetComponent<Renderer>().material;
                mymat.SetColor("_EmissionColor", new Color32(0x72, 0x00, 0x07, 0xFF));
                Light lt = GetComponent<Light>();
                lt.color = new Color32(0x72, 0x00, 0x07, 0xFF);
                mycolor = 3;
                AudioSource audio = GetComponent<AudioSource>();
                audio.PlayOneShot(ColorChange);
            }


        }


    }


    public void DestroyPlatforms()
    {
        for (int i = 0; i < allTheFloors.Count; i++)
        {
            Destroy(allTheFloors[i]);
        }
        allTheFloors.Clear();
    }


    public void OnValueChanged()
    {
        AudioListener.volume = volumeSlider.value;
        volumeSlider2.value = AudioListener.volume;
        PlayerPrefs.SetFloat("Volume",AudioListener.volume);
    }

    public void OnValueChanged2()
    {
        AudioListener.volume = volumeSlider2.value;
        volumeSlider.value = AudioListener.volume;
        PlayerPrefs.SetFloat("Volume", AudioListener.volume);
    }


    IEnumerator DestoyPlatform(GameObject GO, float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(GO);
       
    }


    IEnumerator Transition()
    {
        float t = 1.0f;
        Vector3 startingPos = firstPersonCamera.transform.position;
        while (t > 0.0f)
        {
            t -= Time.deltaTime * (transitionDuration);


            firstPersonCamera.transform.position = Vector3.Lerp(startingPos, firstPersonCamera.transform.position + new Vector3(0, 0, 1f), t);
            yield return 0;
        }

    }
}


//https://assetstore.unity.com/packages/2d/textures-materials/sky/skybox-volume-2-nebula-3392
//https://www.dafont.com/
//textures.com