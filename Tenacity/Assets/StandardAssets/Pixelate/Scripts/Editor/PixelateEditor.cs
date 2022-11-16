using System.Collections;
using System.IO;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PixelateCaptureManager))]
public class PixelateEditor : Editor
{
    Texture2D headerTexture;
    Texture2D pixelateLogo;
    Texture2D previewImage;

    Rect headerSection;

    private PixelateCaptureManager helper;

    private GUIContent discordButtonTexture;

    private const string ASSIGN_REFS_INFO = "Assign the Target and SourceClip to start previewing!";

    private const string LEGACY_ANIM_WARN = "The SourceClip must be marked as Legacy!";

    private const string ASSIGN_CAMERA_INFO = "Assign a camera to start capturing!";

    private IEnumerator _currentCaptureRoutine;

    void OnEnable()
    {
        InitTextures();
    }

    void InitTextures()
    {
        if (EditorGUIUtility.isProSkin == true)
            pixelateLogo = Resources.Load<Texture2D>("Editor\\Pixelate Logo Light");
        else
            pixelateLogo = Resources.Load<Texture2D>("Editor\\Pixelate Logo Dark");

        previewImage = Resources.Load<Texture2D>("Editor\\preview");

        headerTexture = new Texture2D(1, 1);

        if (EditorGUIUtility.isProSkin == true)
            headerTexture.SetPixel(0, 0, new Color32(37, 37, 37, 255));
        else
            headerTexture.SetPixel(0, 0, new Color32(187, 187, 187, 255));

        headerTexture.Apply();
    }

    public override void OnInspectorGUI()
    {
        using (new EditorGUI.DisabledScope(_currentCaptureRoutine != null))
        {
            headerSection.x = 0;
            headerSection.y = 0;
            headerSection.width = Screen.width;
            headerSection.height = 72;

            GUI.DrawTexture(headerSection, headerTexture);

            GUILayout.Space(7);
            //GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label(pixelateLogo, GUILayout.Width(175), GUILayout.Height(45));
            GUILayout.FlexibleSpace();
            //GUILayout.EndHorizontal();
            GUILayout.Space(30);

            helper = (PixelateCaptureManager)target;
            var targetProp = serializedObject.FindProperty("_target");
            var useAnimation = serializedObject.FindProperty("_useAnimation");
            var sourceClipProp = serializedObject.FindProperty("_sourceClip");

            EditorGUILayout.PropertyField(targetProp);
            EditorGUILayout.PropertyField(useAnimation);

            if (useAnimation.boolValue == true)
                EditorGUILayout.PropertyField(sourceClipProp);

            GUILayout.Space(4);

            if (targetProp.objectReferenceValue == null || sourceClipProp.objectReferenceValue == null && useAnimation.boolValue == true)
            {
                EditorGUILayout.HelpBox(ASSIGN_REFS_INFO, MessageType.Info);
                serializedObject.ApplyModifiedProperties();
                return;
            }

            if (targetProp.objectReferenceValue != null && useAnimation.boolValue == false)
            {
                using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
                {
                    EditorGUILayout.LabelField("Capture Options", EditorStyles.boldLabel);

                    var captureCameraProp = serializedObject.FindProperty("_captureCamera");
                    EditorGUILayout.ObjectField(captureCameraProp, typeof(Camera));

                    if (captureCameraProp.objectReferenceValue == null)
                    {
                        EditorGUILayout.HelpBox(ASSIGN_CAMERA_INFO, MessageType.Info);
                        serializedObject.ApplyModifiedProperties();
                        return;
                    }

                    var resolutionProp = serializedObject.FindProperty("_cellSize");
                    EditorGUILayout.PropertyField(resolutionProp);

                    var createNormalMapProp = serializedObject.FindProperty("_createNormalMap");
                    EditorGUILayout.PropertyField(createNormalMapProp);

                    //var createAutoMaterial = serializedObject.FindProperty("_createAutoMaterial");
                    //EditorGUILayout.PropertyField(createAutoMaterial);

                    GUILayout.Space(5);

                    var spriteSavePath = serializedObject.FindProperty("_spriteSavePath");
                    /*var materialSavePath = serializedObject.FindProperty("_materialSavePath");

                    if (createAutoMaterial.boolValue == true)
                    {
                        if (GUILayout.Button("Change Material Export Location", GUILayout.Height(25)))
                        {
                            materialSavePath.stringValue = EditorUtility.OpenFolderPanel("Material Export Location", "", "");
                        }

                        if (materialSavePath.stringValue != "")
                            EditorGUILayout.LabelField(materialSavePath.stringValue, EditorStyles.miniLabel);
                    }*/

                    if (GUILayout.Button("Change Sprite Export Location", GUILayout.Height(25)))
                    {
                        spriteSavePath.stringValue = EditorUtility.OpenFolderPanel("Sprite Export Location", "", "");
                    }

                    if (spriteSavePath.stringValue != "")
                        EditorGUILayout.LabelField(spriteSavePath.stringValue, EditorStyles.miniLabel);

                    if (GUILayout.Button("Capture", GUILayout.Height(25)))
                    {
                        RunRoutine(helper.CaptureFrame(SaveCapture));
                    }

                    GUILayout.Space(5);
                    GUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    GUILayout.Box(" ", GUILayout.Width(Screen.width - 80), GUILayout.Height(Screen.width - 80));
                    GUI.DrawTexture(new Rect(0, 290, Screen.width - 50, Screen.width - 80), previewImage, ScaleMode.ScaleToFit);
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                    GUILayout.Space(0);
                }

                if (GUILayout.Button("Join The Community Discord Server", GUILayout.Height(40)))
                {
                    Application.OpenURL("https://discord.gg/ASkVNuet8K");
                }
                GUILayout.Space(20);

                serializedObject.ApplyModifiedProperties();
                return;
            }

            var sourceClip = (AnimationClip)sourceClipProp.objectReferenceValue;

            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                EditorGUILayout.LabelField("Animation Options", EditorStyles.boldLabel);

                var fpsProp = serializedObject.FindProperty("_framesPerSecond");
                EditorGUILayout.PropertyField(fpsProp);

                var previewFrameProp = serializedObject.FindProperty("_currentFrame");
                var numFrames = (int)(sourceClip.length * fpsProp.intValue) + 1;

                using (var changeScope = new EditorGUI.ChangeCheckScope())
                {
                    var frame = previewFrameProp.intValue;
                    frame = EditorGUILayout.IntSlider("Current Frame", frame, 0, numFrames - 1);

                    if (changeScope.changed)
                    {
                        previewFrameProp.intValue = frame;
                        helper.SampleAnimation(frame / (float)numFrames * sourceClip.length);
                    }
                }
            }

            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                EditorGUILayout.LabelField("Capture Options", EditorStyles.boldLabel);

                var captureCameraProp = serializedObject.FindProperty("_captureCamera");
                EditorGUILayout.ObjectField(captureCameraProp, typeof(Camera));

                if (captureCameraProp.objectReferenceValue == null)
                {
                    EditorGUILayout.HelpBox(ASSIGN_CAMERA_INFO, MessageType.Info);
                    serializedObject.ApplyModifiedProperties();
                    return;
                }

                var resolutionProp = serializedObject.FindProperty("_cellSize");
                EditorGUILayout.PropertyField(resolutionProp);

                var createNormalMapProp = serializedObject.FindProperty("_createNormalMap");
                EditorGUILayout.PropertyField(createNormalMapProp);

                GUILayout.Space(5);

                var spriteSavePath = serializedObject.FindProperty("_spriteSavePath");

                if (GUILayout.Button("Change Sprite Export Location", GUILayout.Height(25)))
                {
                    spriteSavePath.stringValue = EditorUtility.OpenFolderPanel("Sprite Export Location", "", "");
                }

                if (spriteSavePath.stringValue != "")
                    EditorGUILayout.LabelField(spriteSavePath.stringValue, EditorStyles.miniLabel);

                if (GUILayout.Button("Capture", GUILayout.Height(25)))
                {
                    RunRoutine(helper.CaptureAnimation(SaveCapture));
                }

                GUILayout.Space(5);
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.Box(" ", GUILayout.Width(Screen.width - 80), GUILayout.Height(Screen.width - 80));
                GUI.DrawTexture(new Rect(0, 375, Screen.width - 50, Screen.width - 80), previewImage, ScaleMode.ScaleToFit);
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                GUILayout.Space(0);
            }

            if (GUILayout.Button("Join The Community Discord Server", GUILayout.Height(40)))
            {
                Application.OpenURL("https://discord.gg/ASkVNuet8K");
            }
            GUILayout.Space(20);

            serializedObject.ApplyModifiedProperties();
        }
    }

    private void RunRoutine(IEnumerator routine)
    {
        _currentCaptureRoutine = routine;
        EditorApplication.update += UpdateRoutine;
    }

    private void UpdateRoutine()
    {
        if (!_currentCaptureRoutine.MoveNext())
        {
            EditorApplication.update -= UpdateRoutine;
            _currentCaptureRoutine = null;
        }
    }

    private void SaveCapture(Texture2D diffuseMap, Texture2D normalMap, bool createNormalMap, bool useAnimation, Vector2 cellSize)
    {
        string setSpritePath = serializedObject.FindProperty("_spriteSavePath").stringValue;
        var diffusePath = "";

        if (setSpritePath == "")
            diffusePath = EditorUtility.SaveFilePanel("Save Capture", "", "NewCapture", "png");
        else
        {
            diffusePath = EditorUtility.SaveFilePanel("Save Capture", setSpritePath, "NewCapture", "png");
        }

        if (string.IsNullOrEmpty(diffusePath))
        {
            return;
        }

        var fileName = Path.GetFileNameWithoutExtension(diffusePath);
        var directory = Path.GetDirectoryName(diffusePath);
        var normalPath = string.Format("{0}/{1}{2}.{3}", directory, fileName, "NormalMap", "png");

        File.WriteAllBytes(diffusePath, diffuseMap.EncodeToPNG());
        
        if (createNormalMap)
            File.WriteAllBytes(normalPath, normalMap.EncodeToPNG());

        AssetDatabase.Refresh();

        var diffuseAssetDirectory = diffusePath.Replace(Application.dataPath, "Assets");
        Texture2D diffuseAsset = (Texture2D)AssetDatabase.LoadAssetAtPath(diffuseAssetDirectory, typeof(Texture2D));

        AutoSpriteSlicer.Slice(diffuseAsset, cellSize, useAnimation);

        if (createNormalMap)
        {
            normalPath = diffusePath.Remove(diffusePath.Length - 4) + "NormalMap.png";

            var normalAssetDirectory = normalPath.Replace(Application.dataPath, "Assets");
            Texture2D normalAsset = (Texture2D)AssetDatabase.LoadAssetAtPath(normalAssetDirectory, typeof(Texture2D));

            AutoSpriteSlicer.Slice(normalAsset, cellSize, useAnimation);
        }
    }
}
