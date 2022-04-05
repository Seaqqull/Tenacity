using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Lovatto.SceneLoader;

[CustomEditor(typeof(bl_SceneLoader))]
public class bl_SceneLoaderEditor : Editor
{
    private bl_SceneLoader Script;

    private void OnEnable()
    {
        Script = (bl_SceneLoader)target;
    }

    public override void OnInspectorGUI()
    {
        if (Script == null) { Script = (bl_SceneLoader)target; }

        GUILayout.BeginVertical("box");

        GUILayout.BeginVertical("box");
        GUILayout.Label("Settings", EditorStyles.helpBox);
        GUILayout.Space(2);
        Script.SkipType = (SceneSkipType)EditorGUILayout.EnumPopup("Skip Type", Script.SkipType);
        GUILayout.Space(2);
        Script.SceneSmoothLoad = EditorGUILayout.Slider("Progress Smoothing", Script.SceneSmoothLoad, 0.5f, 7);
        Script.FadeInSpeed = EditorGUILayout.Slider("FadeIn Speed", Script.FadeInSpeed, 0.5f, 7);
        Script.FadeOutSpeed = EditorGUILayout.Slider("FadeOut Speed", Script.FadeOutSpeed, 0.5f, 7);
        Script.useTimeScale = EditorGUILayout.ToggleLeft("Use Time Scale", Script.useTimeScale, EditorStyles.toolbarButton);
        GUILayout.EndVertical();

        GUILayout.BeginVertical("box");
        GUILayout.Label("Background", EditorStyles.helpBox);
        GUILayout.Space(2);
        Script.useBackgrounds = EditorGUILayout.ToggleLeft("Use Backgrounds", Script.useBackgrounds, EditorStyles.toolbarButton);
        GUILayout.Space(2);
        if (Script.useBackgrounds)
        {
            Script.TimePerBackground = EditorGUILayout.Slider("Time Per Background", Script.TimePerBackground, 1, 30);
            Script.TimeBetweenBackground = EditorGUILayout.Slider("Time Between Backgrounds", Script.TimeBetweenBackground, 0.5f, 5);
            Script.FadeBackgroundSpeed = EditorGUILayout.Slider("Fade Background Speed", Script.FadeBackgroundSpeed, 0.5f, 7);
        }
        Script.ShowDescription = EditorGUILayout.ToggleLeft("Show scene description", Script.ShowDescription, EditorStyles.toolbarButton);
        GUILayout.Space(2);
        Script.StartFadeInCurve = EditorGUILayout.CurveField("Start FadeIn Curve", Script.StartFadeInCurve);
        GUILayout.EndVertical();

        GUILayout.BeginVertical("box");
        GUILayout.Label("Tips", EditorStyles.helpBox);
        GUILayout.Space(2);
        Script.RandomTips = EditorGUILayout.ToggleLeft("Show Random Tips", Script.RandomTips, EditorStyles.toolbarButton);
        GUILayout.Space(2);
        if (Script.RandomTips)
        {
            Script.TimePerTip = EditorGUILayout.Slider("Time Per Tip", Script.TimePerTip, 1, 30);
            Script.FadeTipsSpeed = EditorGUILayout.Slider("Fade Tip UI Speed", Script.FadeTipsSpeed, 0.5f, 5);
        }
        GUILayout.EndVertical();

        GUILayout.BeginVertical("box");
        GUILayout.Label("Progress", EditorStyles.helpBox);
        GUILayout.Space(2);
        Script.FadeLoadingBarOnFinish = EditorGUILayout.ToggleLeft("Hide loading bar after finish", Script.FadeLoadingBarOnFinish, EditorStyles.toolbarButton);
        GUILayout.Space(2);
        if(Script.mainLoadScale < 1)
        {
            EditorGUILayout.HelpBox(new GUIContent("A 'Main Load Scale' less than '1' means that the remaining value will managed by code from you (custom behave)"));
        }
        Script.mainLoadScale = EditorGUILayout.Slider("Main Load Scale", Script.mainLoadScale, 0, 1);
        Script.RoundBarProgress = EditorGUILayout.Slider("Round Progress Bar Value", Script.RoundBarProgress, 0, 1);
        Script.LoadingCircleSpeed = EditorGUILayout.Slider("Loading Circle Speed", Script.LoadingCircleSpeed, 50, 1000);
        using (new EditorGUILayout.HorizontalScope())
        {
            GUILayout.Label("Loading Text Format", GUILayout.Width(150));
            Script.LoadingTextFormat = EditorGUILayout.TextArea(Script.LoadingTextFormat);
        }
        GUILayout.EndVertical();

        GUILayout.BeginVertical("box");
        GUILayout.Label("Audio", EditorStyles.helpBox);
        GUILayout.Space(2);
        Script.AudioVolume = EditorGUILayout.Slider("Audio Volume", Script.AudioVolume, 0.01f, 1);
        Script.FadeAudioSpeed = EditorGUILayout.Slider("Fade Audio Speed", Script.FadeAudioSpeed, 0.5f, 5);
        Script.FinishSoundVolume = EditorGUILayout.Slider("Finish Sound Volume", Script.FinishSoundVolume, 0.01f, 1);
        Script.BackgroundAudio = EditorGUILayout.ObjectField("Background Audio", Script.BackgroundAudio, typeof(AudioClip), false) as AudioClip;
        Script.FinishSound = EditorGUILayout.ObjectField("Finish Sound", Script.FinishSound, typeof(AudioClip), false) as AudioClip;
        GUILayout.EndVertical();

        EditorGUILayout.BeginVertical("box");
        GUILayout.Label("Event", EditorStyles.helpBox);
        SerializedProperty eventProp = serializedObject.FindProperty("onLoaded");
        EditorGUILayout.PropertyField(eventProp);
        EditorGUILayout.EndVertical();

        GUILayout.BeginVertical("box");
        GUILayout.Label("References", EditorStyles.helpBox);
        GUILayout.Space(2);
        Script.ScreenUI = EditorGUILayout.ObjectField("Loader UI", Script.ScreenUI, typeof(bl_LoadingScreenUI), true) as bl_LoadingScreenUI;
        GUILayout.EndVertical();

        GUILayout.EndVertical();
        if (GUI.changed)
        {
            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(target);
        }
    }
}