using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class uielements : EditorWindow
{

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
        Texture2D texture = new Texture2D(128, 128);
        
        for (int y = 0; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {
                Color color = ((x & y) != 0 ? Color.white : Color.gray);
                texture.SetPixel(x, y, color);
            }
        }
        texture.Apply();
    }

  //  void OnGUI()
  //  { 
  //      GUI.DrawTexture(new Rect(0, 0, 100, 100))
  //  
  //  
  //  
  //  
  //  }


}

    

  

    #endregion 


