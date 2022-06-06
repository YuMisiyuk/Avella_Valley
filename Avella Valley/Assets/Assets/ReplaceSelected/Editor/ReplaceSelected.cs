using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

// Staggart Creations http://staggart.xyz
// Copyright protected under Unity asset store EULA

public class ReplaceSelected : EditorWindow
{
#if UNITY_2019_3_OR_NEWER
    private const float HEIGHT = 265f;
#else
    private const float HEIGHT = 250f;
#endif

    [MenuItem("GameObject/Replace selected")]
    public static void OpenWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        ReplaceSelected window = (ReplaceSelected)EditorWindow.GetWindow(typeof(ReplaceSelected), true);

        //Options
        window.autoRepaintOnSceneChange = true;
        window.maxSize = new Vector2(230f, HEIGHT);
        window.minSize = window.maxSize;
        window.titleContent.image = EditorGUIUtility.IconContent("GameObject Icon").image;
        window.titleContent.text = "Replace selected";
        
        window.Show();
    }

    [SerializeField]
    private Object sourceObject;
    private static Object sourcePrefab;

    private Texture refreshIcon;
    private void OnEnable()
    {
        refreshIcon = EditorGUIUtility.IconContent("Refresh").image;
        
        if (sourceObject != null) return;

        if (LastTargetGUID != string.Empty)
        {
            string path = AssetDatabase.GUIDToAssetPath(LastTargetGUID);
            sourceObject = (Object)AssetDatabase.LoadAssetAtPath(path, typeof(Object));
        }
    }

    private static string LastTargetGUID
    {
        get { return EditorPrefs.GetString(PlayerSettings.productName + "_REPLACE_LASTGUID", string.Empty); }
        set { EditorPrefs.SetString(PlayerSettings.productName + "_REPLACE_LASTGUID", value); }
    }

    private static bool KeepScale
    {
        get { return EditorPrefs.GetBool(PlayerSettings.productName + "_REPLACE_KeepScale", true); }
        set { EditorPrefs.SetBool(PlayerSettings.productName + "_REPLACE_KeepScale", value); }
    }
    private static bool KeepRotation
    {
        get { return EditorPrefs.GetBool(PlayerSettings.productName + "_REPLACE_KeepRotation", true); }
        set { EditorPrefs.SetBool(PlayerSettings.productName + "_REPLACE_KeepRotation", value); }
    }
    private static bool KeepName
    {
        get { return EditorPrefs.GetBool(PlayerSettings.productName + "_REPLACE_KeepName", false); }
        set { EditorPrefs.SetBool(PlayerSettings.productName + "_REPLACE_KeepName", value); }
    }
    private static bool KeepPrefabOverrides
    {
        get { return EditorPrefs.GetBool(PlayerSettings.productName + "_REPLACE_KeepPrefabOverrides", false); }
        set { EditorPrefs.SetBool(PlayerSettings.productName + "_REPLACE_KeepPrefabOverrides", value); }
    }
    private static bool KeepLayer
    {
        get { return EditorPrefs.GetBool(PlayerSettings.productName + "_REPLACE_KeepLayer", false); }
        set { EditorPrefs.SetBool(PlayerSettings.productName + "_REPLACE_KeepLayer", value); }
    }
    private static bool KeepTag
    {
        get { return EditorPrefs.GetBool(PlayerSettings.productName + "_REPLACE_KeepTag", false); }
        set { EditorPrefs.SetBool(PlayerSettings.productName + "_REPLACE_KeepTag", value); }
    }
    private static bool KeepStaticFlags
    {
        get { return EditorPrefs.GetBool(PlayerSettings.productName + "_REPLACE_KeepStaticFlags", false); }
        set { EditorPrefs.SetBool(PlayerSettings.productName + "_REPLACE_KeepStaticFlags", value); }
    }
    
    private static bool SupportsUndo()
    {
        #if UNITY_2021_2_OR_NEWER
        //A bug in this version causes a crash when performing a redo operation while editing a prefab
        if (UnityEditor.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage() != null) return false;
        
        return true;
        #else
        return true;
        #endif
    }

    private void OnGUI()
    {
        if (Selection.gameObjects.Length == 0)
        {
            EditorGUILayout.HelpBox("Nothing selected", MessageType.Info);
            return;
        }
        
        EditorGUILayout.LabelField("Replacement object/prefab", EditorStyles.boldLabel);
        EditorGUI.BeginChangeCheck();
        sourceObject = (Object)EditorGUILayout.ObjectField(sourceObject, typeof(GameObject), true);
        if (EditorGUI.EndChangeCheck())
        {
            LastTargetGUID = sourceObject ? AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(sourceObject.GetInstanceID())) : string.Empty;
        }
        EditorGUILayout.Space();

        using (new EditorGUI.DisabledGroupScope(sourceObject == null))
        {
            EditorGUILayout.LabelField("Keep selection's", EditorStyles.boldLabel);
            KeepScale = EditorGUILayout.Toggle(new GUIContent("Scale", "Enable to keep the current object's scale"), KeepScale);
            KeepRotation = EditorGUILayout.Toggle(new GUIContent("Rotation", "Enable to keep the current object's rotation"), KeepRotation);
            KeepName = EditorGUILayout.Toggle(new GUIContent("Name", "Enable to keep the current object's name"), KeepName);
            KeepLayer = EditorGUILayout.Toggle(new GUIContent("Layer", "Enable to keep the current object's layer"), KeepLayer);
            KeepTag = EditorGUILayout.Toggle(new GUIContent("Tag", "Enable to keep the current object's tag"), KeepTag);
            KeepStaticFlags = EditorGUILayout.Toggle(new GUIContent("Static flags", "Enable to keep the current object's static flags"), KeepStaticFlags);
            KeepPrefabOverrides = EditorGUILayout.Toggle(new GUIContent("Prefab overrides", "If the selected- and replacement objects are a prefab, overrides are copied over to the replaced object"), KeepPrefabOverrides);

            EditorGUILayout.Space();

            if (GUILayout.Button(new GUIContent(" Replace " + Selection.gameObjects.Length + " GameObject" + (Selection.gameObjects.Length > 1 ? "s" : ""), refreshIcon), GUILayout.Height(25f)))
            {
                ReplaceCurrentSelection();
            }
        }
    }

    private void OnSelectionChange()
    {
        this.Repaint();
    }

    private void ReplaceCurrentSelection()
    {
        if (Selection.gameObjects.Length == 0 || sourceObject == null) return;

        if (PrefabUtility.GetPrefabAssetType(sourceObject) == PrefabAssetType.Variant)
        {
            //PrefabUtility.GetCorrespondingObjectFromSource still returns the base prefab. However, this does work.
            sourcePrefab = sourceObject;
        }
        else
        {
            sourcePrefab = PrefabUtility.GetCorrespondingObjectFromOriginalSource(sourceObject);
        }

        //Model prefabs don't count
        var isPrefab = sourcePrefab;
        
        foreach (GameObject selected in Selection.gameObjects)
        {
            Replace(sourceObject, selected, isPrefab);
        }
    }
    
    private static void Replace(Object source, GameObject target, bool sourceIsPrefab)
    {
        //Skip anything selected in the project window!
        if (target.scene.IsValid() == false) return;
        
        GameObject newObj = null;

        if (PrefabUtility.IsPartOfPrefabInstance(target) && !PrefabUtility.IsOutermostPrefabInstanceRoot(target))
        {
            Debug.LogError("Cannot replace an object that's part of a prefab instance. It must be unpacked first.", target);
            return;
        }

        if (sourceIsPrefab)
        {
            newObj = PrefabUtility.InstantiatePrefab(sourcePrefab, target.scene) as GameObject;
            if(SupportsUndo()) Undo.RegisterCreatedObjectUndo(newObj, "Replaced with prefabs");
            
            //Apply any overrides (added/removed components, parameters, etc)
            if (KeepPrefabOverrides)
            {
                if (PrefabUtility.HasPrefabInstanceAnyOverrides(target, false))
                {
                    CopyPrefabOverrides(target, newObj);
                }
            }
        }
        else
        {
            newObj = GameObject.Instantiate(source) as GameObject;
            if(SupportsUndo()) Undo.RegisterCreatedObjectUndo(newObj, "Replaced object");
            newObj.name = newObj.name.Replace("(Clone)", string.Empty);
        }

        newObj.transform.parent = target.transform.parent;
        newObj.transform.SetSiblingIndex(target.transform.GetSiblingIndex());
        newObj.transform.position = target.transform.position;

        if (KeepName) newObj.name = target.name;
        if (KeepRotation) newObj.transform.rotation = target.transform.rotation;
        if (KeepScale) newObj.transform.localScale = target.transform.localScale;
        if (KeepTag) newObj.tag = target.tag;
        if (KeepLayer) newObj.layer = target.layer;
        if (KeepStaticFlags) GameObjectUtility.SetStaticEditorFlags(newObj, GameObjectUtility.GetStaticEditorFlags(target));

        if (Selection.gameObjects.Length == 1) Selection.activeGameObject = newObj;
        
        EditorSceneManager.MarkSceneDirty(target.scene);
        
        //Remove the original object (can cause a crash if it is a prefab due to a Unity bug (2019.3))
        if(SupportsUndo()) Undo.DestroyObjectImmediate(target);
    }

    private static void CopyPrefabOverrides(GameObject source, GameObject target)
    {
        //Model prefab don't have real overrides, copy any components instead
        if (PrefabUtility.GetPrefabAssetType(target) == PrefabAssetType.Model)
        {
            Debug.Log(target.name + " is a model prefab");
            CopyComponents(target, source);
            return;
        }
        
        //Get all overrides
        PropertyModification[] overrides = PrefabUtility.GetPropertyModifications(source);
        List<AddedComponent> added = PrefabUtility.GetAddedComponents(source);
        List<RemovedComponent> removed = PrefabUtility.GetRemovedComponents(source);

        //Remove any components removed as an overrides
        for (int i = 0; i < removed.Count; i++)
        {
            Component comp = target.GetComponent(removed[i].assetComponent.GetType());
            DestroyImmediate(comp);
        }

        //Add any components added as overrides and copy the values over
        for (int i = 0; i < added.Count; i++)
        {
            Component copy = target.GetComponent(added[i].instanceComponent.GetType());
            if(!copy) copy = target.AddComponent(added[i].instanceComponent.GetType());
            EditorUtility.CopySerialized(added[i].instanceComponent, copy);
        }

        //PrefabUtility.ApplyPrefabInstance(target, InteractionMode.AutomatedAction);

        //Apply any modified parameters
        PrefabUtility.SetPropertyModifications(target, overrides);
    }

    private static void CopyComponents(GameObject source, GameObject destination)
    {
        Component[] sourceComponents = destination.GetComponents(typeof(Component));

        for (int i = 0; i < sourceComponents.Length; i++)
        {
            //Don't copy Transform or GameObject values
            if(sourceComponents[i].GetType() == typeof(Transform) || sourceComponents[i].GetType() == typeof(GameObject)) continue;
            
            Component newComp = source.GetComponent(sourceComponents[i].GetType());

            if (!newComp)
            {
                newComp = source.AddComponent(sourceComponents[i].GetType());
            }

            //Copy over values, on prefabs these will automatically becomes overrides
            if (newComp)
            {
                UnityEditorInternal.ComponentUtility.CopyComponent(sourceComponents[i]);
                UnityEditorInternal.ComponentUtility.PasteComponentValues(newComp);
            }
        }
    }
}