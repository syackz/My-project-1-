using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro;
using System.Linq;    

// Ini keranjang untuk menyimpan data tiap pemain
public class Player {
    public int id;
    public string name;
    public int score;
}

public class GameManager : MonoBehaviour
{
    [Header("Sambungan ke UI")]
    public GameObject panelMainMenu;
    public GameObject panelGameplay;
    public TextMeshProUGUI textTurnIndicator;
    public TextMeshProUGUI textCurrentScore;
    public GameObject panelResult;
    public TextMeshProUGUI textLeaderboard;
    
    [Header("Panel Pop-up (Simulasi AR)")]
    public GameObject panelInfoTanaman;
    public GameObject panelQuiz;
    public GameObject panelSerigala;

    // List/Daftar pemain yang sedang main
    private List<Player> players = new List<Player>();
    private int currentPlayerIndex = 0; // Penanda giliran siapa (dimulai dari 0)

    // Fungsi ini akan dipanggil saat tombol "2/3/4 Player" diklik
    public void StartGame(int numberOfPlayers)
    {
        players.Clear(); 

        for (int i = 1; i <= numberOfPlayers; i++)
        {
            players.Add(new Player { id = i, name = "Player " + i, score = 0 });
        }

        System.Random rnd = new System.Random();
        players = players.OrderBy(x => rnd.Next()).ToList();

        currentPlayerIndex = 0; 

        panelMainMenu.SetActive(false);
        panelGameplay.SetActive(true);

        UpdateUI();
    }

    // Fungsi untuk mengubah tulisan teks giliran & poin di layar
    private void UpdateUI()
    {
        textTurnIndicator.text = "Giliran: " + players[currentPlayerIndex].name;
        textCurrentScore.text = "Poin: " + players[currentPlayerIndex].score;
    }

    // --- INI FUNGSI YANG HILANG: Untuk memutar giliran ---
    private void NextTurn() {
        currentPlayerIndex++;
        // Jika sudah mencapai pemain terakhir, kembali ke index 0 (pemain pertama)
        if (currentPlayerIndex >= players.Count) {
            currentPlayerIndex = 0;
        }
        UpdateUI();
    }

    // Fungsi untuk mengakhiri game dan menampilkan ranking
    public void EndGame()
    {
        panelGameplay.SetActive(false);
        panelResult.SetActive(true);

        List<Player> urutanPemenang = players.OrderByDescending(p => p.score).ToList();

        string teksHasil = "KLASEMEN AKHIR:\n\n";
        for (int i = 0; i < urutanPemenang.Count; i++)
        {
            teksHasil += (i + 1) + ". " + urutanPemenang[i].name + " : " + urutanPemenang[i].score + " Poin\n";
        }

        textLeaderboard.text = teksHasil;
    }

    // --- FASE 2: MUNCULKAN POP-UP (Simulasi AR Scan) ---
    public void BukaPanelTanaman() {
        panelGameplay.SetActive(false);
        panelInfoTanaman.SetActive(true);
    }

    public void BukaPanelQuiz() {
        panelGameplay.SetActive(false);
        panelQuiz.SetActive(true);
    }

    public void BukaPanelSerigala() {
        panelGameplay.SetActive(false);
        panelSerigala.SetActive(true);
    }

    // --- FASE 3: AKSI SETELAH POP-UP (Klaim / Jawab) ---
    public void KlaimPoinTanaman() {
        players[currentPlayerIndex].score += 150;
        TutupPopUpDanGantiGiliran(panelInfoTanaman);
    }

    public void JawabQuiz(bool isCorrect) {
        if (isCorrect) {
            players[currentPlayerIndex].score += 100;
        }
        TutupPopUpDanGantiGiliran(panelQuiz);
    }

    public void JawabSerigala(bool isCorrect) {
        if (isCorrect) {
            players[currentPlayerIndex].score += 100;
        } else {
            players[currentPlayerIndex].score -= 100;
        }
        TutupPopUpDanGantiGiliran(panelSerigala);
    }

    // Fungsi bantuan untuk menutup pop-up dan lanjut ke pemain berikutnya
    private void TutupPopUpDanGantiGiliran(GameObject panelYangDitutup) {
        panelYangDitutup.SetActive(false);
        panelGameplay.SetActive(true);
        NextTurn(); // Sekarang fungsi NextTurn() sudah ada, jadi error akan hilang!
    }
}