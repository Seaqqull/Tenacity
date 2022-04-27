Thanks for purchase Loading Screen.
Version 1.5

Requires:

Unity 2019.4++

To open the documentation go to (Unity Toolbar)Window -> Loading Screen -> Documentation.

Any problem or question contact us at:
http://www.lovattostudio.com/en/select-support/

Change Log:

1.5
- Add: Global loading screen (optional), when this mode is set, you only need to set up a loading screen that will work for all the scenes,
  optionally, you can define a different loading screen design per scene.
- Fix: Error when trying to load a scene by script after launch the app/game.
- Improve: Now can load scene by the scene Build Index.
- Improve: Add more information of possible invalid load requests.

1.4.7
- Change: Unity 2019.4 or later version required.
- Improve: Remove all ReorderableList dependences to avoid conflicts with third-party assets.
- Improve: Clean up UI after load a scene.

1.4.5
- Improve: Build-in documentation.
- Add: Added a new Loader prefab: 8-Bit style.

1.4.2
- Remove warnings when use another Lovatto Studio asset in the same project.
- Tested up to Unity 2020.2 and 2021.1a

1.4
- Improve: Separated the Scene and UI management in different scripts to be easier to understand the code (in case that is required)
- Add: A utility script bl_ButtonSceneLoad.cs, simply attach this script to the button with which you wanna load a scene.

1.3.7
- Fix: 'Tip Text' reference field doesn't appear in the reference section of bl_SceneLoader inspector.

1.3.6
- Fix: Incompatibility with the Asset Odin Inspector.

1.3.5
- Add: Added 4 New Loader prefabs with new designs.
- Improve: Added 'OnLoaded' Unity Event in bl_SceneLoader inspector, you can add listeners direct in the inspector to get called when the scene load completely.
- Improve: Replace the HTML documentation with an In-Editor documentation window.
- Improve: Now you only have to assign the Scene asset instead of the scene name in the LoadingScreen scene manager.
- Fix: Incompatibility issues with third party assets that also include Reorderable list.
- Fix: Hidden Loading bar on load option was not working on some prefabs.

1.3
- Improve: bl_SceneLoader now have a custom inspector script.
- Add: New Loader prefab 'SceneLoader 10' with a new design.
- Add: New Loader prefab 'SceneLoader 11' with a cartoon style design.
- Improve: Add AnimationCurves for fade effects in bl_SceneLoader, in order to customizer the fade effects.

1.2.5
- Improve: Now can override 'Skip type' for each scene, if skip type from scene info = none will take the default from SceneLoaderManager.
- Add: Fake Loading: with this you can simulate the time that take load a scene instead of ASync operation time, you can set up per each scene.
- Add: New Loader prefab 'SceneLoader 8' with a new design.

1.2.2
- Fix: problems with Unity 2017
- Add: New Loader template design (SceneLoader 7) 
- Add: On Trigger Load example script.

1.2:
- Improve: Now automatically add all levels in listed in Build Settings on unity start or package imported.
- Quick menu item to setup all levels in build settings automatically.
- Improved: Now SceneLoader prefabs inside canvas automatically set as Last Sibling on start.
- Add: Volume option for 'Finish sound'.
- Improve: Documentation.
- Improve: Some Scene loader options.
- Improve: example backgrounds images.

Version 1.1.5

- Add: New skip option "InstantComplete" use this when load huge levels.
- Improved: Add option for take random tips instead of chain.
- Fix: First tip text is the default from the editor.
- Improved: Add option for use or not timeScale in bl_SceneLoader.cs -> useTimeScale.