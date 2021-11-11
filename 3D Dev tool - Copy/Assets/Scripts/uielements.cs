
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
enum UIdata
{
    MainMenu,
    Drawing
    
}

public class uielements : EditorWindow
{
    UIdata currentState = UIdata.MainMenu;
    Tilemanager currentManager;
    Tilemanager[] allTileManagers = new Tilemanager[0];
    Texture2D texture;
    private float range = 0.0f;
    int xblock;
    int yblock;
    Color[] redblock = { new Color32(225, 0, 0, 255) };
    Color[] greenblock = { new Color32(0, 255, 0, 255) };
    Color[] blueblock = { new Color32(0, 0, 255, 255) };
    int blocksize = 50;
    int border = 0;
    int background = 500;
    static int unityxvalue = 0;
    static int unityyvalue = 0;

    //Menu Item single drop down
    #region Open Editor
    [MenuItem("Dungeon Creator/Open Editor")]
    private static void OpenRoomplacementWindow()
    {
        uielements window = (uielements)EditorWindow.GetWindow(typeof(uielements), true, "Open Editor");
        window.Show();
    }
    void Create()
    { 
        if (currentManager != null)
        {
            if (currentManager.tilesInThisObject[unityxvalue, unityyvalue] == null)
            {
                GameObject instance = Instantiate(Resources.Load("Assets/Single Tile")) as GameObject;
                Renderer rend = instance.GetComponent<Renderer>();
                instance.transform.position = new Vector3Int(-unityxvalue, 0, unityyvalue);
                currentManager.tilesInThisObject[unityxvalue, unityyvalue] = instance;
                instance.transform.SetParent(currentManager.gameObject.transform, true);
            }
        }
    }
    
    void OnEnable()
    {
        ChangeState(UIdata.MainMenu);
        //Example grid block colours
        redblock = new Color[blocksize * blocksize];
        for (int i = 0; i < blocksize * blocksize; i++)
        {
            redblock[i] = Color.red;
        }
        greenblock = new Color[blocksize * blocksize];
        for (int i = 0; i < blocksize * blocksize; i++)
        {
            greenblock[i] = Color.green;
        }
        blueblock = new Color[blocksize * blocksize];
        for (int i = 0; i < blocksize * blocksize; i++)
        {
            blueblock[i] = Color.blue;
        }

        texture = new Texture2D(background, background);
        for (int x = 0; x < texture.height; x++)
        {
            for (int y = 0; y < texture.width; y++)
            {
                Color color = Color.gray;
                texture.SetPixel(x, y, color);
            }
        }
        texture.Apply();
        Repaint();
    }

    void OnGUI()
    {
        #region Mainmenu
        if (currentState == UIdata.MainMenu)
        {
            for (int i = 0; i < allTileManagers.Length; i++)
            {
                if (GUILayout.Button("Open " + allTileManagers[i].name))
                {
                    currentManager = allTileManagers[i];
                    ChangeState(UIdata.Drawing);
                }
            }
            if (GUILayout.Button("Draw new room"))
            {
                GameObject NewTileManager = new GameObject();
                NewTileManager.name = "Room " + allTileManagers.Length;
                NewTileManager.AddComponent<Tilemanager>().tilesInThisObject = new GameObject[background / blocksize, background / blocksize];
                currentManager = NewTileManager.GetComponent<Tilemanager>();
                ChangeState(UIdata.Drawing);
            }
        }
        #endregion

        #region Drawing
        if (currentState == UIdata.Drawing)
        {
            //debug descriptions for co-ords
            range = range + Time.deltaTime;
            if (range > 0.1f)
            {
                Event e = Event.current;
                Debug.Log("Base Co-ords " + e.mousePosition + " " + "Placement at: " + Floor(e.mousePosition.x) + ", " + Ceil(e.mousePosition.y) + Environment.NewLine + "Upper Bounds " + Ceil(e.mousePosition.x) + ", " + Ceil(e.mousePosition.y) + " " + "Lower Bounds " + Floor(e.mousePosition.x) + ", " + Floor(e.mousePosition.y) + " " + "Unity Co-ords: " + unityxvalue + ", " + unityyvalue);
                range = 0.0f;
            }

            //Using mouse co-ords for square co-ords
            if (Event.current.mousePosition.x >= Floor(Event.current.mousePosition.x) && Event.current.mousePosition.x < Ceil(Event.current.mousePosition.x))
            {
                xblock = (int)Floor(Event.current.mousePosition.x);
                unityxvalue = ((int)Floor(Event.current.mousePosition.x) / blocksize);
            }
            if (Event.current.mousePosition.y >= Floor(Event.current.mousePosition.y) && Event.current.mousePosition.y < Ceil(Event.current.mousePosition.y))
            {
                yblock = (int)Ceil(Event.current.mousePosition.y);
                unityyvalue = ((int)Floor(Event.current.mousePosition.y) / blocksize);
            }

            //changing input for changing colours
            if (Event.current.type == EventType.MouseDrag || Event.current.type == EventType.MouseDown)
            {
                if (Event.current.mousePosition.x < background - border && Event.current.mousePosition.x > 0 && Event.current.mousePosition.y < background - border && Event.current.mousePosition.y > 0)
                {
                    if (Event.current.button == 0)
                    {
                        texture.SetPixels(xblock, texture.height - yblock, blocksize, blocksize, redblock);
                        Create();
                        Debug.Log("Red Square placed at LB x and UB y: " + Floor(Event.current.mousePosition.x) + " " + Ceil(Event.current.mousePosition.y));
                    }
                    if (Event.current.button == 1)
                    {
                        texture.SetPixels(xblock, texture.height - yblock, blocksize, blocksize, greenblock);
                        Debug.Log("Green Square placed at LB x and UB y: " + Floor(Event.current.mousePosition.x) + " " + Ceil(Event.current.mousePosition.y));
                    }
                    if (Event.current.button == 2)
                    {
                        texture.SetPixels(xblock, texture.height - yblock, blocksize, blocksize, blueblock); 
                        Debug.Log("Green Square placed at LB x and UB y: " + Floor(Event.current.mousePosition.x) + " " + Ceil(Event.current.mousePosition.y));
                    }
                }
            }
            texture.Apply();
            Repaint();
            GUI.DrawTexture(new Rect(0, 0, background, background), texture);
        }
        #endregion
    }

    void ChangeState(UIdata newState)
    {
        currentState = newState;
        if (currentState == UIdata.MainMenu)
        {
           allTileManagers = FindObjectsOfType<Tilemanager>();
        }
        if (currentState == UIdata.Drawing)
        {
            for (int x = 0; x < currentManager.tilesInThisObject.GetLength(0); x++)
            {
                for (int y = 0; y < currentManager.tilesInThisObject.GetLength(1); y++)
                {
                    if (currentManager.tilesInThisObject[x, y] != null)
                    {   //here is the problem, this value does not save correctly
                        texture.SetPixels((blocksize * x), blocksize * (currentManager.tilesInThisObject.GetLength(1) - (y+1)), blocksize, blocksize, redblock);
                    }
                }
            }
        }      
    }


    //Math for rounding for upper and lower bounds
    private float Round(float input)
    {
        return blocksize * Mathf.Round((input / blocksize));

    }
    private float Floor(float input)
    {
        return blocksize * Mathf.Floor((input / blocksize));

    }
    private float Ceil(float input)
    {
        return blocksize * Mathf.Ceil((input / blocksize));

    }

}
#endregion


    


