﻿using UnityEngine;
using System.Collections;
using System;
using System.IO;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveManagerScript : MonoBehaviour {

	int selGridInt = 0;
	string [] selStrings;
    public Sprite [] images;
	public Image pic1;
	public Text date1;
	public Text userName1;
	public Image pic2;
	public Text date2;
	public Text userName2;
	public Image pic3;
	public Text date3;
	public Text userName3;


	private string loadedtime;

	private string[] levelloaded = new string[3];



	void Start () 
	{
		selStrings = new string[3];
		for (int i = 0; i < selStrings.Length; i++) 
		{
			selStrings[i] = "Save " + (i + 1);
		}

		images = Resources.LoadAll<Sprite> ("Images");

		pic1 = pic1.GetComponent<Image> ();
		pic1.enabled = false;
		pic2 = pic2.GetComponent<Image> ();
		pic2.enabled = false;
		pic3 = pic3.GetComponent<Image> ();
		pic3.enabled = false;
		date1 = date1.GetComponent<Text> ();
		date2 = date2.GetComponent<Text> ();
		date3 = date3.GetComponent<Text> ();
		userName1 = userName1.GetComponent<Text> ();
		userName2 = userName2.GetComponent<Text> ();
		userName3 = userName2.GetComponent<Text> ();

		if (File.Exists (Application.persistentDataPath + "/filesaved0.dat")) 
			pic1.enabled = true;
		if (File.Exists (Application.persistentDataPath + "/filesaved1.dat"))
			pic2.enabled = true;
		if (File.Exists (Application.persistentDataPath + "/filesaved2.dat"))
			pic3.enabled = true;

		/*for (int i = 0; i < 3; i++) 
		{
			Load(i);
		}*/

		Debug.Log (Application.persistentDataPath);
		Debug.Log (levelloaded [0]);
		Debug.Log (levelloaded [1]);
		Debug.Log (levelloaded [2]);
	}
	

	void Update () 
	{
		if (File.Exists (Application.persistentDataPath + "/filesaved0.dat")) 
			pic1.enabled = true;
		if (File.Exists (Application.persistentDataPath + "/filesaved1.dat"))
			pic2.enabled = true;
		if (File.Exists (Application.persistentDataPath + "/filesaved2.dat"))
			pic3.enabled = true;

		/*Load(0);
		Load(1);
		Load(2);*/
			
	}

	void OnGUI()
	{
		selGridInt = GUI.SelectionGrid (new Rect (120, 60, 120, 365), selGridInt, selStrings, 1);

		if (GUI.Button (new Rect (450, 550, 100, 50), "Save")) 
		{
			switch (selGridInt) 
			{
			case 0:
				if(Save(selGridInt))
				{
					date1.text = DateTime.Now.ToString();
					userName1.text = "Game 1";
				}
				chooseIcon(pic1);
				Load(0);
				break;
			case 1:
				if(Save(selGridInt))
				{
					date2.text = DateTime.Now.ToString();
					userName2.text = "Game 2";
				}
				chooseIcon (pic2);
				Load (1);
				break;
			case 2:
				if(Save (selGridInt))
				{
					date3.text = DateTime.Now.ToString();
					userName3.text = "Game 3";
				}
				chooseIcon(pic3);
				Load (2);
				break;
			}
		}

		if (GUI.Button (new Rect (575, 550, 100, 50), "Load")) 
		{
			switch (selGridInt) 
			{
			case 0:
				if(levelloaded[0] == null)
					levelloaded[0] = "Monde1";
				Application.LoadLevel(levelloaded[0]);
				break;
			case 1:
				if(levelloaded[1] == null)
					levelloaded[1] = "Monde1";
				Application.LoadLevel(levelloaded[1]);
				break;
			case 2:
				if(levelloaded[2] == null)
					levelloaded[2] = "Monde1";
				Application.LoadLevel(levelloaded[2]);
				break;
			}
		}

		if (GUI.Button (new Rect (700, 550, 100, 50), "Menu")) 
		{
			Application.LoadLevel("Menu");
		}
	}

	public bool Save(int i)
	{
		if (!doesExist () && !InGameCommandController.isStarted)
			return false;

		BinaryFormatter save = new BinaryFormatter ();
		FileStream file;

		if(!(File.Exists(Application.persistentDataPath + "/filesaved" + i + ".dat")))
			file = File.Create (Application.persistentDataPath + "/filesaved" + i + ".dat");
		else
			file = File.Open(Application.persistentDataPath + "/filesaved" + i + ".dat", FileMode.Open);
		
		WorldData data = new WorldData ();
		data.world1finished = UIManagerScript.isWorld1finished;
		data.world2finished = UIManagerScript.isWorld2finished;
		data.world3finished = UIManagerScript.isWorld3finished;
		data.world4finished = UIManagerScript.isWorld4finished;
		data.savedTime = DateTime.Now.ToString ();
		data.level = UIManagerScript.level;
		
		save.Serialize (file, data);
		file.Close ();

		return true;
	}
	
	public void Load(int i)
	{
		if(File.Exists(Application.persistentDataPath + "/filesaved" + i + ".dat"))
		{
			BinaryFormatter load = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/filesaved" + i + ".dat", FileMode.Open);
			WorldData data = (WorldData)load.Deserialize(file);
			file.Close ();
			
			UIManagerScript.isWorld1finished = data.world1finished;
			UIManagerScript.isWorld2finished = data.world2finished;
			UIManagerScript.isWorld3finished = data.world3finished;
			UIManagerScript.isWorld4finished = data.world4finished;
			loadedtime = data.savedTime;
			UIManagerScript.level = data.level;

			if(i == 0)
			{
				chooseIcon(pic1);
				date1.text = loadedtime;
				levelloaded[0] = UIManagerScript.level;
			}

			if(i == 1)
			{
				chooseIcon(pic2);
				date2.text = loadedtime;
				levelloaded[1] = UIManagerScript.level;
			}

			if(i == 2)
			{
				chooseIcon(pic3);
				date3.text = loadedtime;
				levelloaded[2] = UIManagerScript.level;
			}
		}
	}

	public void chooseIcon(Image img)
	{
		if (UIManagerScript.isWorld3finished)
			img.sprite = images [3];
		else if (UIManagerScript.isWorld2finished)
			img.sprite = images [2];
		else if (UIManagerScript.isWorld1finished)
			img.sprite = images [1];
		else
			img.sprite = images [0];
	}

	public bool doesExist()
	{
		return ((File.Exists (Application.persistentDataPath + "/filesaved0.dat"))
		        && (File.Exists (Application.persistentDataPath + "/filesaved1.dat"))
		        && (File.Exists (Application.persistentDataPath + "/filesaved2.dat")));
	}
	[Serializable]
	class WorldData
	{
		public bool world1finished;
		public bool world2finished;
		public bool world3finished;
		public bool world4finished;
		public string username;
		public string level;
		public string savedTime;
	}
}
