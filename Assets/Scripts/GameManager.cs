﻿using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [ColorUsage(true,true)]
    public List<Color> ChannelColors;

    [ContextMenuItem("Sort","SortObjects",order = 0)]
    public List<LevelObject> ObjectForLoadingLevels;

    public static LevelEditorScript Editor;


    [HideInInspector]
    public bool IsGravityOn = true;

    public bool DebugMode = false;

    [Header("Message System")]
    public GameObject MessageObject;
    public float TimeMessageIsShown;
    public List<GameObject> Messages;

    public Level LevelToLoad;

    private void Start()
    {
        if (Application.isEditor||Application.installMode == ApplicationInstallMode.DeveloperBuild)
        {
            DebugMode = true;
        }
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SortObjects();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void SortObjects()
    {
        ObjectForLoadingLevels.Sort((x, y) => x.Id.CompareTo(y.Id));
    }

    public static void LoadLevel(Level level)
    {
        for (int i = 0; i < level.Content.GetLength(0); i++)
        {
            for (int j = 0; j < level.Content.GetLength(1); j++)
            {
                for (int k = 0; k < level.Content.GetLength(2); k++)
                {
                    if (level.Content[i, j, k].Object != 0)
                    {
                        GameObject obj = Instantiate(GameManager.Instance.ObjectForLoadingLevels[level.Content[i, j, k].Object - 1].Object, i == 0 ? new Vector3(-31.5f + j, 1.0f + k) : new Vector3(-31.5f + j, -19.0f + k), Quaternion.Euler(new Vector3(0, 0, Editor.EditLevel.Content[i, j, k].Rotation)));
                        if (obj.GetComponent<GateScript>() != null)
                        {
                            obj.GetComponent<GateScript>().Channel = level.Content[i, j, k].Channel;
                            obj.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", GameManager.Instance.ChannelColors[level.Content[i, j, k].Channel]);
                        }
                        else if (obj.GetComponent<PlayerPhysicController>() != null)
                        {
                            obj.GetComponent<Rigidbody>().isKinematic = false;
                            obj.GetComponent<PlayerPhysicController>().enabled = true;
                        }
                        else if (obj.transform.childCount > 0)
                        {
                            if (obj.transform.GetChild(0).GetComponent<TriggerSliderScript>() != null)
                            {
                                obj.transform.GetChild(0).GetComponent<TriggerSliderScript>().Channel = level.Content[i, j, k].Channel;
                                obj.transform.GetChild(0).GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", GameManager.Instance.ChannelColors[level.Content[i, j, k].Channel]);
                            }
                            else if (obj.transform.GetChild(0).GetComponent<LevelCompleteScript>() != null)
                            {
                                obj.transform.GetChild(0).GetComponent<LevelCompleteScript>().LevelLoad = "LevelEditor";
                            }
                            else if (obj.transform.GetChild(0).GetComponent<SwitchScript>() != null)
                            {
                                obj.transform.GetChild(0).GetComponent<SwitchScript>().Channel = level.Content[i, j, k].Channel;
                                obj.transform.GetChild(0).GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", GameManager.Instance.ChannelColors[level.Content[i, j, k].Channel]);
                            }
                        }
                    }
                }
            }
        }
    }

    public static void LoadLevelIntoMapEditor()
    {
        for (int i = 0; i < Editor.EditLevel.Content.GetLength(0); i++)
        {
            for (int j = 0; j < Editor.EditLevel.Content.GetLength(1); j++)
            {
                for (int k = 0; k < Editor.EditLevel.Content.GetLength(2); k++)
                {
                    Editor.PlaceObjectInLevel(Editor.EditLevel.Content[i, j, k].Object, Editor.EditLevel.Content[i, j, k].Rotation, j, k, i, Editor.EditLevel.Content[i, j, k].Channel);
                }
            }
        }
    }

    public void PrintMessageInCanvas(string message)
    {
        GameObject obj = Instantiate(MessageObject,GameObject.Find("Canvas").transform);
        obj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = message;
        Messages.Add(obj);
        moveMessagesUp();
    }
    public void PrintMessageInCanvas(string message,int height)
    {
        GameObject obj = Instantiate(MessageObject, GameObject.Find("Canvas").transform);
        obj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = message;
        Messages.Add(obj);
        moveMessagesUp();
    }

    private void moveMessagesUp()
    {
        if(Messages.Count > 0)
        {
            for (int i = 0; i < Messages.Count; i++)
            {
                if(Messages[i] == null)
                {
                    Messages.RemoveAt(i);
                    i--;
                    continue;
                }
                Messages[i].GetComponent<RectTransform>().anchoredPosition += new Vector2(0, Messages[i].GetComponent<RectTransform>().rect.height);
            }
        }
    }
}