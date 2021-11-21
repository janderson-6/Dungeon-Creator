
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
enum UIdata
{
    MainMenu,
    Drawing,
    Materials
}

public class uielements : EditorWindow
{

    //Variables
    UIdata currentState = UIdata.MainMenu;
    Tilemanager currentManager;
    Tilemanager[] allTileManagers = new Tilemanager[0];
    Texture2D texture;
    private float range = 0.0f;
    int xblock;
    int yblock;
    Color[] redblock = { new Color32(225, 0, 0, 255) };
    Color[] grayblock = { new Color32(128, 128, 128, 1) };
    int blocksize = 50;
    int border = 0;
    int background = 500;
    int unityxvalue = 0;
    int unityyvalue = 0;

    //Menu Item single drop down
    #region Open Editor
    [MenuItem("Dungeon Creator/Open Editor")]
    private static void OpenRoomplacementWindow()
    {
        uielements window = (uielements)EditorWindow.GetWindow(typeof(uielements), false, "Open Editor");
        window.Show();
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
     
        grayblock = new Color[blocksize * blocksize];
        for (int i = 0; i < blocksize * blocksize; i++)
        {
            grayblock[i] = Color.gray;
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
        //Main Menu Options
        #region Mainmenu
        if (currentState == UIdata.MainMenu)
        {
            for (int i = 0; i < allTileManagers.Length; i++)
            {
                if (GUILayout.Button("Edit " + allTileManagers[i].name + " tiles"))
                {
                    currentManager = allTileManagers[i];
                    ChangeState(UIdata.Drawing);
                }
                if (GUILayout.Button("Edit " + allTileManagers[i].name + " Materials"))
                {
                    currentManager = allTileManagers[i];
                    ChangeState(UIdata.Materials);
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
                //Debug.Log("Base Co-ords " + e.mousePosition + " " + "Placement at: " + Floor(e.mousePosition.x) + ", " + Ceil(e.mousePosition.y) + Environment.NewLine + "Upper Bounds " + Ceil(e.mousePosition.x) + ", " + Ceil(e.mousePosition.y) + " " + "Lower Bounds " + Floor(e.mousePosition.x) + ", " + Floor(e.mousePosition.y) + " " + "Unity Co-ords: " + unityxvalue + ", " + unityyvalue);
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

            //Set pixels on texture
            if (Event.current.type == EventType.MouseDrag || Event.current.type == EventType.MouseDown)
            {
                if (Event.current.mousePosition.x < background - border && Event.current.mousePosition.x > 0 && Event.current.mousePosition.y < background - border && Event.current.mousePosition.y > 0)
                {
                    if (Event.current.button == 0)
                    {
                        texture.SetPixels(xblock, texture.height - yblock, blocksize, blocksize, redblock);
                        Create();
                        //Debug.Log("Red Square placed at LB x and UB y: " + Floor(Event.current.mousePosition.x) + " " + Ceil(Event.current.mousePosition.y));
                    }
                    if (Event.current.button == 1)
                    {
                        texture.SetPixels(xblock, texture.height - yblock, blocksize, blocksize, grayblock);
                        Remove();
                    }
                  
                }
            }
            texture.Apply();
            Repaint();
            GUI.DrawTexture(new Rect(0, 0, background, background), texture);
           
            }
        #endregion

        #region Materials
        if (currentState == UIdata.Materials)
        {
            //this code does nothing
            Changematerials();
        }


            #endregion
      }


    //Changing states
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
                    {
                        texture.SetPixels((blocksize * x), blocksize * (currentManager.tilesInThisObject.GetLength(1) - (y + 1)), blocksize, blocksize, redblock);
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
    //Spawning floors
    void Create()
    {
        if (currentManager != null)

        {
            if (currentManager.tilesInThisObject[unityxvalue, unityyvalue] == null)
            {
                GameObject floortile = Instantiate(Resources.Load("Assets/Tile Cobblestone 1")) as GameObject;
                Renderer rend = floortile.GetComponent<Renderer>();
                floortile.transform.position = new Vector3Int((-unityxvalue + (int)currentManager.transform.position.x), 0, (unityyvalue + (int)currentManager.transform.position.z));
                currentManager.tilesInThisObject[unityxvalue, unityyvalue] = floortile;
                floortile.transform.SetParent(currentManager.gameObject.transform, true);


                //LEFTWALLS
                //IF ON VERY LEFT
                if (0 == unityxvalue)
                {
                    Leftwall();
                }
                //REMOVING WRONG WALLS
                else if (currentManager.tilesInThisObject[unityxvalue - 1, unityyvalue] != null)
                {
                    DestroyImmediate(currentManager.tilesInThisObject[unityxvalue - 1, unityyvalue].GetComponent<Allwalls>().walltiles[1]);
                    DestroyImmediate(currentManager.tilesInThisObject[unityxvalue, unityyvalue].GetComponent<Allwalls>().walltiles[0]);
                }
                //MAKING WALLS FOR EDGE LEFT FLOOR
                else if (currentManager.tilesInThisObject[unityxvalue - 1, unityyvalue] == null)
                {
                    Leftwall();
                }


                //RIGHTWALLS
                //IF ON VERY RIGHT
                if (currentManager.tilesInThisObject.GetLength(0)-1 == unityxvalue)
                {
                    Rightwall();
                }
                //REMOVING WRONG WALLS
                else if (currentManager.tilesInThisObject[unityxvalue + 1, unityyvalue] != null)
                {
                    DestroyImmediate(currentManager.tilesInThisObject[unityxvalue + 1, unityyvalue].GetComponent<Allwalls>().walltiles[0]);
                    DestroyImmediate(currentManager.tilesInThisObject[unityxvalue, unityyvalue].GetComponent<Allwalls>().walltiles[1]);
                }
                //MAKING WALLS FOR EDGE RIGHT FLOOR
                else if (currentManager.tilesInThisObject[unityxvalue + 1, unityyvalue] == null)
                {
                    Rightwall();
                }


                //FRONTWALLS
                //IF ON VERY FRONT
                if (0 == unityyvalue) 
                {
                    Frontwall();
                }
                //REMOVING WRONG WALLS
                else if (currentManager.tilesInThisObject[unityxvalue, unityyvalue - 1] != null)
                {
                    DestroyImmediate(currentManager.tilesInThisObject[unityxvalue, unityyvalue - 1].GetComponent<Allwalls>().walltiles[2]);
                    DestroyImmediate(currentManager.tilesInThisObject[unityxvalue, unityyvalue].GetComponent<Allwalls>().walltiles[3]);
                }
                //MAKING WALLS FOR EDGE FRONT FLOOR
                else if (currentManager.tilesInThisObject[unityxvalue, unityyvalue - 1] == null)
                {
                    Frontwall();
                }


                //BACKWALLS
                //IF ON VERY BACK
                if (currentManager.tilesInThisObject.GetLength(1)-1 == unityyvalue)
                {
                    Backwall();
                }
                //REMOVING WRONG WALLS
                else if (currentManager.tilesInThisObject[unityxvalue, unityyvalue + 1] != null)
                {
                    DestroyImmediate(currentManager.tilesInThisObject[unityxvalue, unityyvalue + 1].GetComponent<Allwalls>().walltiles[3]);
                    DestroyImmediate(currentManager.tilesInThisObject[unityxvalue, unityyvalue].GetComponent<Allwalls>().walltiles[2]);
                }
                //MAKING WALLS FOR EDGE FRONT FLOOR
                else if (currentManager.tilesInThisObject[unityxvalue, unityyvalue + 1] == null)
                {
                    Backwall();
                }
                

            }
        }
    }

    void Changematerials()
    {//this function does nothing
        if (currentManager.tilesInThisObject[unityxvalue, unityyvalue] != null)
        {//this calls nothing
            currentManager.tilesInThisObject[unityxvalue, unityyvalue].GetComponent<Renderer>().sharedMaterials[0] = (Material)Resources.Load("Materials/Dark Stone");
            
        }
    }

    //Remove Floor
    void Remove()
    {
        if (currentManager != null)
        {
            if (currentManager.tilesInThisObject[unityxvalue, unityyvalue] != null)
            {
                for (int i = 0; i < 4; i++)
                {
                    
                    if (currentManager.tilesInThisObject[unityxvalue, unityyvalue].GetComponent<Allwalls>().walltiles[i] != null)
                    {
                        DestroyImmediate(currentManager.tilesInThisObject[unityxvalue, unityyvalue].GetComponent<Allwalls>().walltiles[i]);
                        
                    }
                } 

                        DestroyImmediate(currentManager.tilesInThisObject[unityxvalue, unityyvalue].gameObject);
                        
                        //Debug.Log("Square removed");
                        
            }
            
        }
        
    }

    //Spawning walls
    void Leftwall()
    {   //check spaces to the left
        GameObject walltile = Instantiate(Resources.Load("Assets/Wall tile Wooden")) as GameObject;
        Renderer rendwall = walltile.GetComponent<Renderer>();
        walltile.transform.position = new Vector3Int(((-unityxvalue + (int)currentManager.transform.position.x)), 0, ((unityyvalue + (int)currentManager.transform.position.z)));
        walltile.transform.Rotate(0.0f, 270.0f, 0.0f, Space.Self);
        currentManager.tilesInThisObject[unityxvalue, unityyvalue].GetComponent<Allwalls>().walltiles[0] = walltile;
        walltile.transform.SetParent(currentManager.gameObject.transform, true);
    }
    void Rightwall()
    {   //check spaces to the right
        GameObject walltile = Instantiate(Resources.Load("Assets/Wall tile Wooden")) as GameObject;
        Renderer rendwall = walltile.GetComponent<Renderer>();
        walltile.transform.position = new Vector3Int(((-unityxvalue + (int)currentManager.transform.position.x) - 1), 0, ((unityyvalue + (int)currentManager.transform.position.z) + 1));
        walltile.transform.Rotate(0.0f, 90.0f, 0.0f, Space.Self);
        currentManager.tilesInThisObject[unityxvalue, unityyvalue].GetComponent<Allwalls>().walltiles[1] = walltile;
        walltile.transform.SetParent(currentManager.gameObject.transform, true);
    }
    void Backwall()
    {   //check spaces behind
        GameObject walltile = Instantiate(Resources.Load("Assets/Wall tile Wooden")) as GameObject;
        Renderer rendwall = walltile.GetComponent<Renderer>();
        walltile.transform.position = new Vector3Int(((-unityxvalue + (int)currentManager.transform.position.x)), 0, ((unityyvalue + (int)currentManager.transform.position.z) + 1));
        walltile.transform.Rotate(0.0f, 0.0f, 0.0f, Space.Self);
        currentManager.tilesInThisObject[unityxvalue, unityyvalue].GetComponent<Allwalls>().walltiles[2] = walltile;
        walltile.transform.SetParent(currentManager.gameObject.transform, true);
    }

    void Frontwall()
    {   //check spaces in front
        GameObject walltile = Instantiate(Resources.Load("Assets/Wall tile Wooden")) as GameObject;
        Renderer rendwall = walltile.GetComponent<Renderer>();
        walltile.transform.position = new Vector3Int(((-unityxvalue + (int)currentManager.transform.position.x) - 1), 0, ((unityyvalue + (int)currentManager.transform.position.z)));
        walltile.transform.Rotate(0.0f, 180.0f, 0.0f, Space.Self);
        currentManager.tilesInThisObject[unityxvalue, unityyvalue].GetComponent<Allwalls>().walltiles[3] = walltile;
        walltile.transform.SetParent(currentManager.gameObject.transform, true);
    }

}
#endregion


    


