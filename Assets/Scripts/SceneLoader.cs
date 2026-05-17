using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadSampleScene()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadARCardScan()
    {
        SceneManager.LoadScene("ARCardScan");
    }

    public void LoadHOMEPAGE()
    {
        SceneManager.LoadScene("HOMEPAGE");
    }

    public void LoadInstructionScene()
    {
        // Memuat scene petunjuk bermain
        SceneManager.LoadScene("InstructionSceneAuto");
    }

    public void LoadCreditScene()
    {
        // Memuat scene credit pembuat
        SceneManager.LoadScene("CreditScene");
    }
}
