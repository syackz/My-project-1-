using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadSampleScene()
    {
        ButtonSoundManager.PlayDefaultSound();
        SceneTransitionManager.Instance.TransitionToScene("SampleScene");
    }

    public void LoadScene(string sceneName)
    {
        ButtonSoundManager.PlayDefaultSound();
        SceneTransitionManager.Instance.TransitionToScene(sceneName);
    }

    public void LoadARCardScan()
    {
        ButtonSoundManager.PlayDefaultSound();
        SceneTransitionManager.Instance.TransitionToScene("ARCardScan");
    }

    public void LoadHOMEPAGE()
    {
        ButtonSoundManager.PlayDefaultSound();
        SceneTransitionManager.Instance.TransitionToScene("HOMEPAGE");
    }

    public void LoadInstructionScene()
    {
        ButtonSoundManager.PlayDefaultSound();
        // Memuat scene petunjuk bermain
        SceneTransitionManager.Instance.TransitionToScene("InstructionSceneAuto");
    }

    public void LoadCreditScene()
    {
        ButtonSoundManager.PlayDefaultSound();
        // Memuat scene credit pembuat
        SceneTransitionManager.Instance.TransitionToScene("CreditScene");
    }
}
