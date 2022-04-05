using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using LoadingScreen.TutorialWizard;
using System;

public class LoadingScreenDocumentation : TutorialWizard
{
    //required//////////////////////////////////////////////////////
    public string FolderPath = "loading-screen/editor/";
    public NetworkImages[] m_ServerImages = new NetworkImages[]
    {
        new NetworkImages{Name = "img-0.jpg", Image = null},
        new NetworkImages{Name = "img-1.png", Image = null},
        new NetworkImages{Name = "img-2.png", Image = null},
        new NetworkImages{Name = "img-3.png", Image = null},
        new NetworkImages{Name = "img-4.png", Image = null},
        new NetworkImages{Name = "img-5.png", Image = null},
        new NetworkImages{Name = "img-6.png", Image = null},
        new NetworkImages{Name = "img-7.png", Image = null},
        new NetworkImages{Name = "img-8.png", Image = null},
        new NetworkImages{Name = "img-9.png", Image = null},
        new NetworkImages{Name = "img-10.png", Image = null},
        new NetworkImages{Name = "img-11.png", Image = null},
        new NetworkImages{Name = "img-12.png", Image = null},
    };
    public Steps[] AllSteps = new Steps[] {
    new Steps { Name = "Get Started", StepsLenght = 0 , DrawFunctionName = nameof(DrawGetStarted)},
    new Steps { Name = "Add new scene", StepsLenght = 0, DrawFunctionName = nameof(DrawAddNewScene) },
    new Steps { Name = "Load by Button", StepsLenght = 0, DrawFunctionName = nameof(LoadByButton) },
    new Steps { Name = "Load by Code", StepsLenght = 0, DrawFunctionName = nameof(DrawLoadByCode) },
    new Steps { Name = "Loading Screen Mode", StepsLenght = 0, DrawFunctionName = nameof(LoadingScreenModeDoc) },
    new Steps { Name = "Scene Load Type", StepsLenght = 0, DrawFunctionName = nameof(DrawLoadType) },
    new Steps { Name = "Edit UI", StepsLenght = 0, DrawFunctionName = nameof(DrawEditUI) },
    new Steps { Name = "Add Tips", StepsLenght = 0, DrawFunctionName = nameof(DrawAddTips) },
    new Steps { Name = "bl_SceneLoader", StepsLenght = 0, DrawFunctionName = nameof(DrawblSceneLoader) },
    new Steps { Name = "Common Q/A", StepsLenght = 0, DrawFunctionName = nameof(CommonProblems) },
    };
    private readonly GifData[] AnimatedImages = new GifData[]
   {
        new GifData{ Path = "loadings-editui.gif" },
        new GifData{Path = "loadings-qstart.gif" },
   };

    public override void OnEnable()
    {
        base.OnEnable();
        base.Initizalized(m_ServerImages, AllSteps, FolderPath, AnimatedImages);
        allowTextSuggestions = true;
        Style.highlightColor = new Color(0.2336686f, 0.5726451f, 0.8396226f, 1.00f);
        //FetchWebTutorials("loading-screen/tutorials/");
    }

    public override void WindowArea(int window)
    {
        AutoDrawWindows();
    }

    //final required////////////////////////////////////////////////

    void DrawGetStarted()
    {
        if (subStep == 0)
        {
            DrawHyperlinkText("<b><size=18><b>Quick Start:</b></size></b>\n\n<b>■ 1.</b> In the Unity top menu go to <b>Window ➔ Loading Screen ➔ (Click) Add Levels.</b>\n\n<b>■ 2.</b> Fill all the information of your scenes in the <link=asset:Assets/Loading Screen/Content/Resources/SceneLoaderManager.asset>SceneLoaderManager</link>.\n\n<b>■ 3.</b> Drag one of the Scene Loader prefabs inside of one of your Canvas in the hierarchy of your scene, the Scene Loader prefabs are located in <b>Loading Screen ➔ Content ➔ Prefabs ➔ Scene Loaders ➔ *</b>\n\n<b>■ 4.</b> Load the scene by a button or by code <i>(check the respective section for how to load the scenes)</i>.");
            DrawAnimatedImage(1);
            DownArrow();
            DrawText("<b>If you wanna preview</b> any of the Scene Loaders, you can do so by open the respective example scene, you find them in the folder: <i><b>Assets➔Loading Screen➔Example➔Scenes➔*</b></i>");
        }
    }

    void LoadByButton()
    {
        DrawText("For load a scene by a button you have two simple options:\n\n1 - Simple attach the script bl_ButtonSceneLoad.cs to the UI Button and then set the scene name to load in the inspector of the script and that's.\n");
        DrawServerImage(9);
        DownArrow();
        DrawText("2 - Add the <b>bl_SceneLoader</b> script <i>(from the scene)</i> directly in the UI Button -> OnClick list pointing to LoadLevel function.\n\n- After you have the <b>'LoadingScreen'</b> prefab in the canvas of your scene, select the button that you want to use and setup like this:\n");
        DrawServerImage(2);
        DrawText("That's");
    }

    private void DrawLoadByCode()
    {
        DrawText("Load a scene by script is really simple, instead of use the default Unity method:\n");
        DrawCodeText("SceneManager.LoadScene('SCENE_NAME');");
        DrawText("Just replace with:");
        DrawCodeText("bl_SceneLoaderManager.LoadScene('SCENE_NAME');");
        DrawText("That's!, is that simple :)");

        DrawText("A question that I receive very frequently regarding this topic <i>(how to load by code)</i> from new Unity users or non-programmer users is <b>\"where to add this line of code?\"</b> if you also have this doubt, unfortunately, I can't tell you exactly in where since it depends entirely in your project, in the scripts of your project to be more specific.\n\nThis is the hint, if your project does already load scenes by script, you have to find out which script in your project does load the scenes and then replace the line of code with the Loading Screen one. In case your project still not load scenes by the script but you wanna implement it, simply create a new script ➔ Create a new function and paste the loading screen line inside of the function ➔ Call that function whenever you wanna load the scene, e.g:");
        DrawCodeText("using UnityEngine;\n \npublic class LoadSceneScript : MonoBehaviour\n{\n    public string SceneName = \"LoadExample\";\n \n    public void LoadMyScene()\n    {\n        bl_SceneLoaderManager.LoadScene(SceneName);\n    }\n}");
    }

    private void DrawAddNewScene()
    {
        DrawHyperlinkText("To load a scene with Loading Screen you only need 2 things:\n\n1. The scene in question is listed in the Unity <b>Build Settings</b>, if you don't know what Build Settings is or how to add a scene to it, check this: <link=https://docs.unity3d.com/Manual/BuildSettings.html>https://docs.unity3d.com/Manual/BuildSettings.html</link>\n\n2. The scene in question is listed in the <link=asset:Assets/Loading Screen/Content/Resources/SceneLoaderManager.asset>SceneLoaderManager</link>.\n\nThe <link=asset:Assets/Loading Screen/Content/Resources/SceneLoaderManager.asset>SceneLoaderManager</link> is a scriptable object where all the scenes custom info used by the loading screen is stored, you can locate this in the <b>Resources</b> folder of Loading Screen.");
        DrawText("If this is <b>your first time adding your project scenes</b> you can automatically add all the scenes in your Build Settings to the SceneLoaderManager. For it go to: <b>Window ➔ Loading Screen ➔ All Levels.</b>\n");
        DrawServerImage(3);
        DrawText("with this all your scenes will added on the list of SceneLoaderManager, then you only need add the Description, Preview Image, Backgrounds, etc..");
        DrawHyperlinkText("For it simply opens the SceneLoaderManager (<link=asset:Assets/Loading Screen/Content/Resources/SceneLoaderManager.asset>Click Here</link>) ➔ In the inspector window go to Scene Manager List ➔ foldout your scene ➔ modify the properties.");
        DrawServerImage(11);
        DownArrow();
        DrawText("A brief explanation of the values.");
        DrawServerImage(12);
        DrawText("Now you can load the scene with loading screen.");
    }

    void LoadingScreenModeDoc()
    {
        DrawHyperlinkText("Before version 1.5 there was only one way to set up the loading screens, that was instance one loading screen in each scene of the game, which comes in handy in certain games but is tedious for most developers that don't require a different loading screen in each scene, so a 'global' option was requested, and it was added.\n \nSince version 1.5 you can use the Loading Screen per scene (Legacy) or one single/global loading screen that will work for all scenes so you only need to instance and set up 1 loading screen for your game.\n \nThere are not many differences in how to set up both modes, you can define which mode you want to use in the <i><link=asset:Assets/Loading Screen/Content/Resources/SceneLoaderManager.asset>LoaderSceneManager</link> ➔ <b>Loading Screen Mode</b>.</i>");
        DrawServerImage("img-13.png", TextAlignment.Center);
        DownArrow();
        DrawSuperText("The <b>One Per Scene</b> mode is the legacy mode in which you instance a loading screen in each scene of your game, the other sections of this documentation cover all need to know about this mode and how to set it up.\n \nThe <b>One For All</b> mode is the global mode in which you only <b>have to instance and set up one loading screen in the first scene of your game</b>, basically you set it up the same as the other mode with the only difference that you have to instance in a predefined canvas which you also have to drop in your scene:\n\n<?background=#fffff>SET UP ONE FOR ALL MODE</background>");
        DrawHyperlinkText("<b>1.</b> In the first scene of your game, the one that appears after launching the game, drag and drop the prefab located at <i>Assets ➔ Loading Screen ➔ Content ➔ Prefabs ➔ LoaderCanvas ➔ <link=asset:Assets/Loading Screen/Content/Prefabs/LoaderCanvas/SceneLoaderCanvas.prefab>SceneLoaderCanvas</link></i>, make sure it doesn't have any transform parent in the hierarchy.\n \n<b>2.</b> Same steps as described in the <b>Get Started</b> section with the only difference that you have to drag the Scene Loader inside the canvas that you just instanced from step 1.\n \n3. That's.");
        DrawServerImage("img-14.png", TextAlignment.Center);
    }

    private void DrawLoadType()
    {
        DrawText("With loading screen you have 2 options for load a level <b>'Async'</b> and <b>'Fake'</b>, both do the same job but with a different process to load the level.\n\n<size=24><b>Async</b>:</size>\nThis is the Unity system for load levels asynchronously in the background, the time it take to load a level depend of the size of level, a level with a lots of models and high resolution textures will take more time to load, there is some cases in which this method will not load smoothly and will make some jumps in the load percentage, unfortunately that is how the internal system <i>(Unity Source side)</i> works.\n\n\n\n<size=24><b>Fake:</b></size>\nWith this method as the name say, you simulate the time that take load a level, you can set up how much  in second will take \"load\" the level, this method is useful for small scenes, we not recommend use for large scenes where the time can take much longer depending on each device.\n\n\n\n- So now you know how work each method you decide in witch levels use it, for set up simple go to the scene info in 'SceneLoaderManager' and in the 'LoadType' enum select the option:\n");
        DrawServerImage(6);
    }

    void DrawEditUI()
    {
        DrawText("In order to customize your loading screen design, you simply need to use one of the ready-made prefabs as a reference and modify it.\n\nDrag one of the Scene Loader prefabs located at <i>Assets➔Loading Screen➔Content➔Prefabs➔SceneLoaders➔*</i> and drop it inside a Canvas in the hierarchy of your target scene ➔ enable the second child object of the Scene Loader <i>(LoaderRoot)</i> ➔ start making the modifications.\n\nFeel free to delete any UI Component that you want, the system null-check all the UI references.");
        DrawAnimatedImage(0);
    }

    void DrawAddTips()
    {
        DrawText("For add / replace 'Tips' text:");
        DrawServerImage(7);
    }

    void DrawblSceneLoader()
    {
        DrawText("The script <b>bl_SceneLoader.cs</b> is the script attached in the root of each Scene Loader prefabs, it handles all the logic to load the asynchronous scene and update the UI.\n\nIn the inspector of this script you will find a lot of variables that you can set up to personalize the loading screen to your taste, here a brief explanation of some of them:");
        DrawServerImage(10);
        DownArrow();
        DrawPropertieInfo("Skip Type", "enum", "Override the global skip type defined in the SceneLoaderManager, tell how the loading screen will be transition to the next scene after the async load finish.");
        DrawPropertieInfo("Progress Smoothing", "float", "Smooth amount applied to the loading bar.");
        DrawPropertieInfo("FadeIn Speed", "float", "The fade speed with which the loading screen appears.");
        DrawPropertieInfo("FadeOut Speed", "float", "The fade speed with which the loading screen hide before load the next scene.");
        DrawPropertieInfo("Start FadeIn Curve", "AnimationCurve", "Control how the fade amount is applied when the loading screen appear.");
        DrawPropertieInfo("Round Progress Bar Value", "float", "Define how the loading progress will be applied in the loading bar, 0 = linear, 0.02 = steps of 0.02, 0.04 = steps of 0.04, etc...");
    }

    void CommonProblems()
    {
        DrawSpoilerBox("Scene 'SCENE_NAME' couldn't be loaded because it has not been added to the build settings...", "This error is pretty self described, it is caused because the scene you are trying to load has not been added in the Build Settings window in your project.\n\nIf you don't know how to add an scene in the Build Settings, check this: <link=https://docs.unity3d.com/Manual/BuildSettings.html>https://docs.unity3d.com/Manual/BuildSettings.html</link>");

        DrawSpoilerBox("Not found any scene with this name: SCENE_NAME", "If when you try to load an scene, you get this message, it means that the scene that you are trying to load has not been added in the '<b>SceneLoaderManager ➔ Scene Manager ➔ List</b>'.\n\nFor more info about how to add an scene and what you need to set up, check the <b>Add New Scene</b> section.");

        DrawSpoilerBox("How to integrate with MFPS", "If you want to integrate with MFPS, check this: <link=https://www.lovattostudio.com/en/integrate-loading-screen-to-mfps-2-0/>https://www.lovattostudio.com/en/integrate-loading-screen-to-mfps-2-0/</link>");
    }

    [MenuItem("Window/Loading Screen/Documentation")]
    static void Open()
    {
        GetWindow<LoadingScreenDocumentation>();
    }
}