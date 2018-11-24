using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.Experimental.UIElements;
using System.IO;
using Button = UnityEngine.UI.Button;

public class MyGameMode : MonoBehaviour {
    public Camera firstPersonCamera;
    public GameObject Platform;
    public GameObject FirstPlatform;
    public GameObject Player;
    short CamMode = 0;
    public int speedMult = 1;
    private int speedMultS=1;
    public bool inMainMenu = true;
    public AudioClip ElevatorMusic;
    public AudioClip Furi;
    public Canvas PauseCanvas;
    public Canvas MainMenuCanvas;
    public Canvas GameOverCanvas;
    public Canvas LayOnCanvas;
    public Canvas MainOptionsCanvas;
    public GameObject MobilePanel;
    public Text Credits;
    public Text HowToPlay;
    public Text LeaderBoard;
    public int[] Highscores=new int[10];
    public String[] names=new string[10];
    public Text GameoverText;
    public Text NameInput;
    String name = "Unknown";
    public InputField InputField;
    public Button Confirm;
    public Button reset;
    public Button quit;
    public Button backtomainmenu;
    public Text ConfirmText;
    Resolution res;
    bool loser;




    // Use this for initialization
    void Start () {

        res = Screen.currentResolution;
        if (res.refreshRate == 60)
            QualitySettings.vSyncCount = 1;
        if (res.refreshRate == 120)
            QualitySettings.vSyncCount = 2;
        print(QualitySettings.vSyncCount);

        loadLeaderBoard();

        //DontDestroyOnLoad(this.gameObject);
        AudioSource audio = GetComponent<AudioSource>();
        audio.PlayOneShot(ElevatorMusic);

        Time.timeScale = 0.0f;
        speedMultS = speedMult;
        speedMult = 0;


    }

    // Update is called once per frame
    void Update () {
        
        //Pause game
        if (Input.GetKeyUp(KeyCode.Escape) && !inMainMenu && !loser)
            {
            if (Time.timeScale == 1.0f)
            {
                PauseCanvas.enabled = true;
                Time.timeScale = 0.0f;
                speedMultS = speedMult;
                speedMult = 0;
                AudioSource audio = GetComponent<AudioSource>();
                audio.Stop();
                audio.PlayOneShot(ElevatorMusic);
                Cursor.visible = true;
            }
            else {
                Cursor.visible = false;
                PauseCanvas.enabled = false;
                speedMult = speedMultS;
                Time.timeScale = 1.0f;
                // Adjust fixed delta time according to timescale
                // The fixed delta time will now be 0.02 frames per real-time second
                Time.fixedDeltaTime = 0.02f * Time.timeScale;
                AudioSource audio = GetComponent<AudioSource>();
                audio.Stop();
                audio.PlayOneShot(Furi);
            }

        }

        if (Input.GetKeyUp(KeyCode.C))
        {
            if (CamMode == 1)
                ShowOverheadView();
            else
                ShowFirstPersonView();
        }


    }

    //Camera Change View
    public void ShowOverheadView()
    {
        firstPersonCamera.transform.position = new Vector3(0, 4.5f, -7.25f);
        firstPersonCamera.transform.rotation = Quaternion.Euler(18.5f, 0, 0);
        CamMode = 0;
    }

    public void ShowFirstPersonView()
    {
        firstPersonCamera.transform.position = new Vector3(0.15f, 19.1f, -4.4f);
        firstPersonCamera.transform.rotation = Quaternion.Euler(62, 0, 0);
        CamMode = 1;
    }


    public void QuitGame()
    {
        Application.Quit();
    }

    public void onGameStart()
    {
        Cursor.visible = false;
        AudioSource audio = GetComponent<AudioSource>();
        audio.Stop();
        audio.PlayOneShot(Furi);
        inMainMenu = false;
        MainMenuCanvas.enabled = false;
        speedMult = speedMultS;
        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        LayOnCanvas.enabled = true;
         Cursor.visible = false;

            #if UNITY_STANDALONE
                    MobilePanel.SetActive(false);
                
            #endif

    }

    public void onResume()
    {
        Cursor.visible = false;
        PauseCanvas.enabled = false;
        speedMult = speedMultS;
        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        AudioSource audio = GetComponent<AudioSource>();
        audio.Stop();
        audio.PlayOneShot(Furi);
    }

    public void onRestart()
    {
        // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        // GameObject[] floors;
        //GameObject[] ScoreSphere;
        // floors = GameObject.FindGameObjectsWithTag("Floor");
        //ScoreSphere = GameObject.FindGameObjectsWithTag("ScoreSphere"); 

        loser = false;
        AudioSource audio = GetComponent<AudioSource>();
        audio.Stop();
        MainMenuCanvas.enabled = false;
        PauseCanvas.enabled = false;
        audio.PlayOneShot(Furi);
        inMainMenu = false;
        speedMult = speedMultS;
        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        MainBall MBScript = Player.GetComponent<MainBall>();
        MBScript.DestroyPlatforms();
        speedMult=MBScript.speedMult = 1;
        MBScript.Score = 0;
        MBScript.First2 = 2;
        MBScript.Start();
        MBScript.HighestScore = 0;
        GameOverCanvas.enabled = false;

        MBScript.GetComponent<MeshRenderer>().enabled = true;
        Player.transform.position = new Vector3(0, 0, 0);


    }
  

    public void GameOver()
    {
        loser = true;
        Cursor.visible = true;
        // NameInput.gameObject.SetActive(false);
        InputField.gameObject.SetActive(false);
        MainBall MBScript = Player.GetComponent<MainBall>();
        AudioSource audio = GetComponent<AudioSource>();
        audio.Stop();
        audio.PlayOneShot(ElevatorMusic);
        GameOverCanvas.enabled = true;
        Time.timeScale = 0.0f;
        speedMultS = speedMult;
        speedMult = 0;
        LayOnCanvas.enabled = false;

        int S = MBScript.HighestScore;
        if (S >= Highscores[9]&&S>0)
        {
            Confirm.gameObject.SetActive(true);
            InputField.gameObject.SetActive(true);
            NameInput.gameObject.SetActive(true);
            InputField.gameObject.SetActive(true);
            GameoverText.text = "Congrats! New High Score Achieved: " + S;
            reset.interactable = false;
            backtomainmenu.interactable = false;
 }



    }


    public void Pause()
    {
        if (Time.timeScale == 1.0f)
        {
            PauseCanvas.enabled = true;
            Time.timeScale = 0.0f;
            speedMultS = speedMult;
            speedMult = 0;
            AudioSource audio = GetComponent<AudioSource>();
            audio.Stop();
            audio.PlayOneShot(ElevatorMusic);
        }
        else
        {
            PauseCanvas.enabled = false;
            speedMult = speedMultS;
            Time.timeScale = 1.0f;
            // Adjust fixed delta time according to timescale
            // The fixed delta time will now be 0.02 frames per real-time second
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            AudioSource audio = GetComponent<AudioSource>();
            audio.Stop();
            audio.PlayOneShot(Furi);
        }
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void onCamButton()
    {
        if (CamMode == 1)
            ShowOverheadView();
        else
            ShowFirstPersonView();
    }

    public void BackToMainMenu2()
    {
        MainOptionsCanvas.enabled = false;
        inMainMenu = true;
        MainMenuCanvas.enabled = true;
        Credits.enabled = false;
        HowToPlay.enabled = false;
    }

    public void onCreditButton()
    {
        Credits.enabled = !Credits.enabled;
        LeaderBoard.enabled = false;
        HowToPlay.enabled = false;
    }
    public void onHowToPlayButton()
    {
        HowToPlay.enabled = !HowToPlay.enabled;
        LeaderBoard.enabled = false;
        Credits.enabled = false;
    }

    public void onMainOptionsButton()
    {
        LeaderBoard.enabled = false;
        HowToPlay.enabled = false;
        Credits.enabled = false;
        MainOptionsCanvas.enabled = true;
        MainMenuCanvas.enabled = false;
    }
    public void onLeaderBoardButton()
    {
        LeaderBoard.text = "Leader Board \n \n";
        for (int i = 0; i < Highscores.Length; i++)
        {
            LeaderBoard.text = LeaderBoard.text + Highscores[i] +"          " +names[i] + "\n";
        }
  

        LeaderBoard.enabled =!LeaderBoard.enabled;
        HowToPlay.enabled = false;
        Credits.enabled = false;

    }

    public void onNameConfirmButton()
    {
        name = NameInput.text;

        MainBall MBScript = Player.GetComponent<MainBall>();
        int S = MBScript.HighestScore;
        int i;
        reset.interactable = true;
        backtomainmenu.interactable = true;
        Confirm.gameObject.SetActive(false);
        InputField.gameObject.SetActive(false);


        int Stemp = 0;
        String nametemp = "";
        for (i = 0; i < 10; i++)
        {
            if (S > Highscores[i])
            {
                if (i < 9)
                {
                    Stemp = Highscores[i + 1];
                    if (names[i + 1 ] == null) { names[i + 1] = "unknown"; }
                    nametemp = names[i + 1];

                    Highscores[i + 1] = Highscores[i];
                    names[i + 1] = names[i];
                }

                Highscores[i] = S;
                names[i] = name;


                S = Stemp;
                name = nametemp;

            }
            Debug.Log(Highscores[i]);
        }
        LayOnCanvas.enabled = true;
        SaveLeaderBoard();

    }



    public void SaveLeaderBoard()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath +"/LeaderBoard.dat");
        Debug.Log(Application.persistentDataPath + "/LeaderBoard.dat");
        PlayerDataSave data = new PlayerDataSave();
        data.scores = Highscores;
        data.name = names;
        bf.Serialize(file, data);
        file.Close();

    }

    public void loadLeaderBoard()
    {
        if (File.Exists(Application.persistentDataPath + "/LeaderBoard.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/LeaderBoard.dat", FileMode.Open);
            PlayerDataSave data = (PlayerDataSave)bf.Deserialize(file);
            Highscores = data.scores;
            names = data.name;
            file.Close();
        }
    }

}

[Serializable]
class PlayerDataSave 
{

    public int[] scores;
    public String[] name;
}
