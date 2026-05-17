using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

[System.Serializable]
public class ButtonSoundSetup
{
    [Tooltip("Tarik (drag & drop) objek tombol dari Hierarchy ke kotak ini")]
    public Button targetButton;
    [Tooltip("Masukkan file suara khusus untuk tombol tersebut")]
    public AudioClip soundEffect;
}

public class ButtonSoundManager : MonoBehaviour
{
    public static ButtonSoundManager Instance { get; private set; }

    [Header("Suara Bawaan (Default)")]
    [Tooltip("Suara standar yang dimainkan jika tombol tidak punya suara khusus")]
    public AudioClip defaultClickSound;
    
    [Header("Suara Khusus per Tombol")]
    [Tooltip("Tambahkan elemen baru, lalu tarik objek tombol dan masukkan file suaranya")]
    public List<ButtonSoundSetup> customSounds = new List<ButtonSoundSetup>();

    private Dictionary<Button, AudioClip> soundDictionary;

    void Awake()
    {
        // Singleton pattern: pertahankan SoundManager di seluruh scene
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        InitializeSoundDictionary();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        BindAllButtonsInScene();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Setiap kali scene baru dimuat, otomatis scan dan pasang suara ke semua tombol baru!
        BindAllButtonsInScene();
    }

    private void InitializeSoundDictionary()
    {
        soundDictionary = new Dictionary<Button, AudioClip>();
        foreach (var custom in customSounds)
        {
            if (custom.targetButton != null && custom.soundEffect != null)
            {
                if (!soundDictionary.ContainsKey(custom.targetButton))
                {
                    soundDictionary.Add(custom.targetButton, custom.soundEffect);
                }
            }
        }
    }

    public void BindAllButtonsInScene()
    {
        // Bersihkan dictionary lama (karena tombol di scene sebelumnya sudah hancur)
        InitializeSoundDictionary();

        // Temukan semua tombol di scene yang aktif
        Button[] allButtons = Resources.FindObjectsOfTypeAll<Button>();

        foreach (Button btn in allButtons)
        {
            // Pastikan tombol berada di scene (bukan asset di project window)
            if (btn.gameObject.scene.IsValid() && btn.gameObject.scene.isLoaded)
            {
                Button currentBtn = btn;
                
                // Hapus listener duplikat agar suara tidak dobel
                currentBtn.onClick.RemoveListener(() => PlaySpecificSound(currentBtn));
                
                // Tambahkan listener suara
                currentBtn.onClick.AddListener(() => PlaySpecificSound(currentBtn));
            }
        }
    }

    public void PlaySpecificSound(Button clickedButton)
    {
        AudioClip clipToPlay = defaultClickSound;

        if (soundDictionary != null && soundDictionary.ContainsKey(clickedButton))
        {
            clipToPlay = soundDictionary[clickedButton];
        }

        PlaySound(clipToPlay);
    }

    // Fungsi statis yang bisa dipanggil dari script mana saja (mencegah suara terpotong saat pindah scene)
    public static void PlayDefaultSound()
    {
        if (Instance != null && Instance.defaultClickSound != null)
        {
            Instance.PlaySound(Instance.defaultClickSound);
        }
    }

    public void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            GameObject tempAudioObj = new GameObject("TempAudio_" + clip.name);
            AudioSource tempSource = tempAudioObj.AddComponent<AudioSource>();
            tempSource.clip = clip;
            
            // Suara tetap hidup meskipun scene dihancurkan
            DontDestroyOnLoad(tempAudioObj);
            tempSource.Play();
            
            Destroy(tempAudioObj, clip.length);
        }
        else
        {
            Debug.LogWarning("ButtonSoundManager: Mencoba memutar suara, tapi AudioClip kosong!");
        }
    }
}
