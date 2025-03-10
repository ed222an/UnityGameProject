﻿using UnityEngine;
using System.Collections;

public class GameControllerScript : MonoBehaviour
{
	// Variables.
	public GUIStyle menuHeaderStyle;
	public GUIStyle menuInstructionsHeaderStyle;
	public GUIStyle menuInstructionsContentStyle;
	public GUIStyle scoreStyle;
	public AudioClip pauseMenuClip;
	
	private GUISkin guiSkin;
	private AudioClip originalSong;
	private bool pauseIsActive;
	private bool showInstructions;
	private GameObject spawnPoint;

	private GameObject[] coins;
	public Texture2D coinImage;
	public int coinScore = 0;
	
	private PlayerHealth ph;
	private int livesLeft;
	
	void Start()
	{
		guiSkin = null;
		pauseIsActive = false;
		showInstructions = false;
		
		originalSong = audio.clip;

		coins = GameObject.FindGameObjectsWithTag ("Coin");

		ph = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerHealth> ();
	}
	
	void Update ()
	{
		// Loads the next level.
		if(coinScore == coins.Length)
		{
			Application.LoadLevel(Application.loadedLevel + 1);
		}

		livesLeft = ph.health;

		// Enables pause-functionality.
		if(Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
		{
			if(Time.timeScale == 1.0f)
			{
				playSong(pauseMenuClip);

				Time.timeScale = 0.0f;
				pauseIsActive = true;
			}
			else
			{
				Time.timeScale = 1.0f;
				pauseIsActive = false;
				showInstructions = false;

				playSong(originalSong);
			}
		}
	}
	
	// The pause menu.
	void OnGUI ()
	{
		string pauseText = "Pause: \"Esc\" || \"P\"";
		string livesText = "Lives: ";
		string instructionsHeaderText = "Instructions!";
		string instructionsBodyText = 	"Collect all coins to win.\n" +
									"Use the A,D- or the left, right arrow keys to move Kat around.\n" +
									"Use the space key to jump\n" +
									"Jump on enemies to kill them and try not to fall down\n\n" +
									"Keep it safe kids and be careful not to die!\nI hear that's dangerous!";

		// Set up gui skin
		GUI.skin = guiSkin;
		
		// Pause prompt.
		GUI.Label(new Rect(Screen.width - 120, 0, 200, 20), pauseText);

		// Score text
		GUI.DrawTexture(new Rect(0, 0, 60, 60), coinImage, ScaleMode.ScaleToFit, true, 0);
		GUI.Label(new Rect(42, 0, 200, 20), ": " + coinScore + "/" + coins.Length, scoreStyle);

		// Lives text
		GUI.Label(new Rect(20, Screen.height - 50, 200, 20), livesText + livesLeft, scoreStyle);
		
		if(pauseIsActive)
		{
			// Begins a GUI-group to help with organization.
			GUI.BeginGroup(new Rect((Screen.width/2) - 50, (Screen.height/2) - 250, 150, 200));
			
			// The Pause-text.
			GUI.Label(new Rect(20, 0, 100, 24), "PAUSED!", menuHeaderStyle);
			
			// The "How To Play"-button.
			if(GUI.Button(new Rect(0, 20, 100, 50), "How To Play"))
			{
				pauseIsActive = false;
				showInstructions = true;
			}
			
			// The "Restart"-button.
			if(GUI.Button (new Rect(0, 70, 100, 50), "Restart"))
			{
				// Restarts the level.
				Application.LoadLevel(Application.loadedLevel);
				Time.timeScale = 1.0f;
				pauseIsActive = false;
			}
			
			// The "Quit"-button.
			if(GUI.Button(new Rect(0, 120, 100, 50), "Quit"))
			{
				// Loads the startscreen.
				Application.LoadLevel(0);
				Time.timeScale = 1.0f;
				pauseIsActive = false;
			}
			
			// Ends the GUI-group.
			GUI.EndGroup();
		}
		
		if(showInstructions)
		{
			int offset = 120;

			// Begins a GUI-group to help with organization.
			GUI.BeginGroup(new Rect(0, 0, Screen.width, Screen.height));
			
			// Instructiontext.
			GUI.Label(new Rect((Screen.width/2 - 25),20,100,20), instructionsHeaderText, menuInstructionsHeaderStyle);
			GUI.Label(new Rect((Screen.width/2 - offset),50,100,20), instructionsBodyText, menuInstructionsContentStyle);
			
			// Return to pause screen.
			if(GUI.Button(new Rect((Screen.width/2 - 100),300,100,50), "Back"))
			{
				showInstructions = false;
				pauseIsActive = true;
			}
			
			// Resumes the game.
			if(GUI.Button(new Rect((Screen.width/2),300,100,50), "Continue"))
			{
				showInstructions = false;
				pauseIsActive = false;
				Time.timeScale = 1.0f;

				playSong(originalSong);
			}
			
			// Ends the GUI-group.
			GUI.EndGroup();
		}
	}

	void playSong(AudioClip clip)
	{
		audio.clip = clip;
		audio.volume = 0.25f;
		audio.Play();
	}
}
