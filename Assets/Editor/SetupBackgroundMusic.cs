using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SetupBackgroundMusic : EditorWindow
{
    [MenuItem("Tools/Setup Background Music")]
    public static void SetupAllScenes()
    {
        Debug.Log("🎵 [SetupBackgroundMusic] Memulai setup background music di semua scene...");

        // Cari audio clips yang akan dipasang
        AudioClip homepageClip = LoadClipByName("Mysterious Chapter");
        AudioClip instructionClip = LoadClipByName("Discovery 1");
        AudioClip creditClip = LoadClipByName("Cosmic Reveal");
        AudioClip sampleClip = LoadClipByName("Ice Magic");
        AudioClip arClip = LoadClipByName("Discovery 1");

        if (homepageClip == null) Debug.LogWarning("⚠️ [SetupBackgroundMusic] Clip 'Mysterious Chapter' tidak ditemukan.");
        if (instructionClip == null) Debug.LogWarning("⚠️ [SetupBackgroundMusic] Clip 'Discovery 1' tidak ditemukan.");
        if (creditClip == null) Debug.LogWarning("⚠️ [SetupBackgroundMusic] Clip 'Cosmic Reveal' tidak ditemukan.");
        if (sampleClip == null) Debug.LogWarning("⚠️ [SetupBackgroundMusic] Clip 'Ice Magic' tidak ditemukan.");

        // Daftar scene
        string[] scenePaths = new string[]
        {
            "Assets/Scenes/HOMEPAGE.unity",
            "Assets/Scenes/InstructionSceneAuto.unity",
            "Assets/Scenes/CreditScene.unity",
            "Assets/Scenes/SampleScene.unity",
            "Assets/Scenes/ARCardScan.unity"
        };

        // Simpan scene yang sedang aktif saat ini agar bisa dikembalikan nanti
        string originalScenePath = EditorSceneManager.GetActiveScene().path;

        foreach (string path in scenePaths)
        {
            if (System.IO.File.Exists(path))
            {
                Debug.Log($"🎵 [SetupBackgroundMusic] Membuka scene untuk dikonfigurasi: {path}");
                Scene scene = EditorSceneManager.OpenScene(path, OpenSceneMode.Single);

                // Cari gameobject background music manager lama
                BackgroundMusicManager manager = GameObject.FindObjectOfType<BackgroundMusicManager>();
                GameObject managerGo = null;

                if (manager == null)
                {
                    // Cari berdasarkan nama
                    managerGo = GameObject.Find("BackgroundMusicManager");
                    if (managerGo == null)
                    {
                        managerGo = GameObject.Find("[BackgroundMusic]");
                    }

                    if (managerGo == null)
                    {
                        managerGo = new GameObject("BackgroundMusicManager");
                    }
                    
                    manager = managerGo.GetComponent<BackgroundMusicManager>();
                    if (manager == null)
                    {
                        manager = managerGo.AddComponent<BackgroundMusicManager>();
                    }
                }
                else
                {
                    managerGo = manager.gameObject;
                }

                // Ubah nama GameObject agar bersih
                managerGo.name = "BackgroundMusicManager";

                // Konfigurasi manager
                manager.targetVolume = 0.35f;
                manager.fadeDuration = 1.2f;
                manager.defaultMusicClip = homepageClip != null ? homepageClip : instructionClip;

                // Atur list scene music khusus
                manager.sceneMusicList = new List<SceneMusicSetup>();

                AddSceneMusic(manager, "HOMEPAGE", homepageClip);
                AddSceneMusic(manager, "InstructionSceneAuto", instructionClip);
                AddSceneMusic(manager, "CreditScene", creditClip);
                AddSceneMusic(manager, "SampleScene", sampleClip);
                AddSceneMusic(manager, "ARCardScan", arClip);

                manager.InitializeMusicDictionary();

                // Simpan scene
                EditorUtility.SetDirty(managerGo);
                EditorSceneManager.MarkSceneDirty(scene);
                EditorSceneManager.SaveScene(scene);
                Debug.Log($"🎵 [SetupBackgroundMusic] Sukses menyimpan background music di scene: {scene.name}");
            }
            else
            {
                Debug.LogWarning($"🎵 [SetupBackgroundMusic] File scene tidak ditemukan di path: {path}");
            }
        }

        // Kembalikan ke scene semula
        if (!string.IsNullOrEmpty(originalScenePath) && System.IO.File.Exists(originalScenePath))
        {
            EditorSceneManager.OpenScene(originalScenePath, OpenSceneMode.Single);
        }

        Debug.Log("🎵 [SetupBackgroundMusic] SETUP SELESAI DENGAN SUKSES! Semua 5 scene telah terkonfigurasi dengan background music.");
    }

    private static AudioClip LoadClipByName(string name)
    {
        string[] guids = AssetDatabase.FindAssets(name + " t:AudioClip");
        if (guids != null && guids.Length > 0)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[0]);
            return AssetDatabase.LoadAssetAtPath<AudioClip>(path);
        }
        return null;
    }

    private static void AddSceneMusic(BackgroundMusicManager manager, string sceneName, AudioClip clip)
    {
        if (clip == null) return;
        SceneMusicSetup setup = new SceneMusicSetup();
        setup.sceneName = sceneName;
        setup.musicClip = clip;
        manager.sceneMusicList.Add(setup);
    }
}
