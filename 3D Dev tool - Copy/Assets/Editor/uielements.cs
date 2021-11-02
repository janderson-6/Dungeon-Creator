using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class uielements : EditorWindow
{

    Texture2D texture;
    private float range = 0.0f;
    //private float xblock = 0.0f;
    //private float yblock = 0.0f;
    Color[] redblock = { new Color32(225, 0, 0, 255) };
    Color[] greenblock = { new Color32(0, 255, 0, 255) };
    Color[] blueblock = { new Color32(0, 0, 255, 255) };
    int blocksize = 50;
    int border = 50;
    int background = 500;  

    //Menu Item single drop down
    #region Menu Items
    [MenuItem("Dungeon Creator/Edit Dungeon")]
    private static void OpenRoomplacementWindow()
    {
        uielements window = (uielements)EditorWindow.GetWindow(typeof(uielements), true, "Edit Dungeon");
        window.Show();

       

    }

    void OnEnable()
    {
        redblock = new Color[blocksize  * blocksize];
        for (int i = 0; i < blocksize * blocksize ; i++)
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


        range = range + Time.deltaTime;
        if (range > 0.1f)
        {
            Event e = Event.current;
            Debug.Log("Mouse Co-ords" + e.mousePosition +" "+"Rounded to: " + Round(e.mousePosition.x) +" "+ Round(e.mousePosition.y));
            range = 0.0f;
        }
        //if button pressed
        if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
        {   //if within border place red square on left click
            if (Event.current.mousePosition.x < background - border && Event.current.mousePosition.x > 0 && Event.current.mousePosition.y < background - border && Event.current.mousePosition.y > 0)
            {
                texture.SetPixels((int)Round(Event.current.mousePosition.x), texture.height - (int)Round(Event.current.mousePosition.y), blocksize, blocksize, redblock);
                Debug.Log("Red Square placed at " + Round(Event.current.mousePosition.x) + " " + Round(Event.current.mousePosition.y));
            }
        }
        if (Event.current.type == EventType.MouseDown && Event.current.button == 1)
        {   //if within borderplace green square on right click
            if (Event.current.mousePosition.x < background - border && Event.current.mousePosition.x > 0 && Event.current.mousePosition.y < background - border && Event.current.mousePosition.y > 0)
            {
                texture.SetPixels((int)Round(Event.current.mousePosition.x), texture.height - (int)Round(Event.current.mousePosition.y), blocksize, blocksize, greenblock);
                Debug.Log("Green Square placed at " + Round(Event.current.mousePosition.x) + " " + Round(Event.current.mousePosition.y));
            }

        }
        if (Event.current.type == EventType.MouseDown && Event.current.button == 2)
        {   //if within borderplace green square on right click
            if (Event.current.mousePosition.x < background - border && Event.current.mousePosition.x > 0 && Event.current.mousePosition.y < background - border && Event.current.mousePosition.y > 0)
            {
                texture.SetPixels((int)Round(Event.current.mousePosition.x), texture.height - (int)Round(Event.current.mousePosition.y), blocksize, blocksize, blueblock);
                Debug.Log("Green Square placed at " + Round(Event.current.mousePosition.x) + " " + Round(Event.current.mousePosition.y));
            }

        }



        texture.Apply();
            Repaint();
            GUI.DrawTexture(new Rect(0, 0, background, background), texture);


        
    }

 
    private float Round(float input)
    {
        return blocksize * Mathf.Round((input / blocksize));
        
    }

}

    

  

    #endregion 


