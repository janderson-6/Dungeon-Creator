using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class uielements : EditorWindow
{
    #region Menu Items
    [MenuItem("Dungeon Creator/Edit Dungeon")]
    private static void OpenRoomplacementWindow()
    {
        uielements window = (uielements)EditorWindow.GetWindow(typeof(uielements), true, "Edit Dungeon");
        window.Show();
    }

    //[MenuItem("Dungeon Creator/Room Editor")]
    //private static void OpenRoomEditorWindow()
    //{
    //    uielements window = (uielements)EditorWindow.GetWindow(typeof(uielements), false, "Room Editor");
    //    window.Show();
    //}

    #endregion 

}
