using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class uielements : EditorWindow
{

    Texture2D texture;
    private float range = 0.0f;
    Color[] colorsred = { new Color32(225, 0, 0, 255) };
    int blocksize = 30;
    int background = 300;

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
        colorsred = new Color[blocksize  * blocksize];
        for (int i = 0; i < blocksize * blocksize ; i++)
        {
            colorsred[i] = Color.red;   
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
    }

    void OnGUI()
    {
        range = range + Time.deltaTime;

        if (range > 0.1f)
        {
            Event e = Event.current;
            Debug.Log(e.mousePosition);
            range = 0.0f;
        }

        if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
        {
            if (Event.current.mousePosition.x < background - blocksize && Event.current.mousePosition.x > blocksize && Event.current.mousePosition.y < background - blocksize && Event.current.mousePosition.y > blocksize)
            {
                texture.SetPixels((int)Event.current.mousePosition.x, texture.height - (int)Event.current.mousePosition.y, blocksize, blocksize, colorsred);
            }

        }
        texture.Apply();
        GUI.DrawTexture(new Rect(0, 0, background, background), texture);

    }


}

    

  

    #endregion 


