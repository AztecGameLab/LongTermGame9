using UnityEngine;
using TriInspector;

/// <summary>
/// The game's settings (Currently only the editor's play method)
/// </summary>
[HideMonoScript]
[CreateAssetMenu(menuName = "Saguaro/Settings")]
public class GameSettings : ScriptableObject
{
    /// <summary> The scene to begin with when playing in the Editor. </summary>
    public EditorPlayMethod editorPlay;
    public const string PersistentScenePath = "Assets/Scenes/PersistentScene.unity";
    public const string StartScenePath = "Assets/Scenes/MenuScenes/MainMenu.unity";
    
    /// <summary> Methods of starting the game in the Editor </summary>
    public enum EditorPlayMethod
    {
        StartOfGame,
        CurrentScene
    }
}
