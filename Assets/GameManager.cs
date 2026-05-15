using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro;
using System.Linq;    

public class Player {
    public int id;
    public string name;
    public int score;
}

public class GameManager : MonoBehaviour
{
    [Header("Sambungan ke UI Utama")]
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

    [Header("Notifikasi Feedback")]
    public GameObject panelNotif;
    public TextMeshProUGUI textNotif;

    private List<Player> players = new List<Player>();
    private int currentPlayerIndex = 0;

    // --- FASE 1: START GAME ---
    public void StartGame(int numberOfPlayers)
    {
        players.Clear(); 
        for (int i = 1; i <= numberOfPlayers; i++) {
            players.Add(new Player { id = i, name = "Player " + i, score = 0 });
        }

        System.Random rnd = new System.Random();
        players = players.OrderBy(x => rnd.Next()).ToList();
        currentPlayerIndex = 0; 

        panelMainMenu.SetActive(false);
        panelGameplay.SetActive(true);
        UpdateUI();
    }

    private void UpdateUI() {
        textTurnIndicator.text = "Giliran: " + players[currentPlayerIndex].name;
        textCurrentScore.text = "Poin: " + players[currentPlayerIndex].score;
    }

    private void NextTurn() {
        currentPlayerIndex++;
        if (currentPlayerIndex >= players.Count) {
            currentPlayerIndex = 0;
        }
        UpdateUI();
    }

    // --- REVISI 3: KEMBALI KE MENU ---
    public void KembaliKeMenu() {
        panelResult.SetActive(false);
        panelMainMenu.SetActive(true);
    }

    public void EndGame() {
        panelGameplay.SetActive(false);
        panelResult.SetActive(true);

        List<Player> urutanPemenang = players.OrderByDescending(p => p.score).ToList();
        string teksHasil = "KLASEMEN AKHIR:\n\n";
        for (int i = 0; i < urutanPemenang.Count; i++) {
            teksHasil += (i + 1) + ". " + urutanPemenang[i].name + " : " + urutanPemenang[i].score + " Poin\n";
        }
        textLeaderboard.text = teksHasil;
    }

    // --- FASE 2: MUNCULKAN POP-UP ---
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

    // --- FASE 3: AKSI & REVISI NOTIFIKASI ---
    public void KlaimPoinTanaman() {
        players[currentPlayerIndex].score += 150;
        // Tanaman tidak pakai notif, langsung tutup & ganti giliran
        panelInfoTanaman.SetActive(false);
        panelGameplay.SetActive(true);
        NextTurn(); 
    }

    // REVISI 1: NOTIF QUIZ
    public void JawabQuiz(bool isCorrect) {
        panelQuiz.SetActive(false);
        panelNotif.SetActive(true); // Munculkan layar notif

        if (isCorrect) {
            players[currentPlayerIndex].score += 100;
            textNotif.text = "Jawaban kamu BENAR!\nSelamat, kamu mendapat +100 Poin.";
        } else {
            textNotif.text = "Sayang sekali, jawabanmu SALAH!\nPoin kamu tidak bertambah.";
        }
    }

    // REVISI 2: NOTIF SERIGALA
    public void JawabSerigala(bool isCorrect) {
        panelSerigala.SetActive(false);
        panelNotif.SetActive(true); // Munculkan layar notif

        if (isCorrect) {
            players[currentPlayerIndex].score += 100;
            textNotif.text = "Jawaban kamu BENAR!\nKamu berhasil menghindar dan mendapat +100 Poin.";
        } else {
            players[currentPlayerIndex].score -= 100;
            textNotif.text = "Oh tidak! Jawaban kamu SALAH!\nKamu terkena serangan Serigala. Poin dikurangi -100.";
        }
    }

    // Fungsi ini dipanggil saat tombol "OK/Lanjut" di layar Notif ditekan
    public void TutupNotifDanGantiGiliran() {
        panelNotif.SetActive(false);
        panelGameplay.SetActive(true);
        NextTurn();
    }
}