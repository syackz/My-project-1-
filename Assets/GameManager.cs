using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

// Data tiap pemain
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

    // Daftar pemain yang sedang main
    private List<Player> players = new List<Player>();
    private int currentPlayerIndex = 0;

    // =============================================
    // FASE 1: MULAI GAME
    // =============================================
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

    private void UpdateUI()
    {
        textTurnIndicator.text = "Giliran: " + players[currentPlayerIndex].name;
        textCurrentScore.text = "Poin: " + players[currentPlayerIndex].score;
    }

    private void NextTurn()
    {
        currentPlayerIndex++;
        if (currentPlayerIndex >= players.Count)
        {
            currentPlayerIndex = 0;
        }
        UpdateUI();
    }

    // =============================================
    // FASE 2: TAMPILKAN POP-UP (Simulasi AR Scan)
    // =============================================
    public void BukaPanelTanaman()
    {
        panelGameplay.SetActive(false);
        panelInfoTanaman.SetActive(true);
    }

    public void BukaPanelQuiz()
    {
        panelGameplay.SetActive(false);
        panelQuiz.SetActive(true);
    }

    public void BukaPanelSerigala()
    {
        panelGameplay.SetActive(false);
        panelSerigala.SetActive(true);
    }

    // =============================================
    // FASE 3: AKSI SETELAH POP-UP → TAMPIL NOTIF
    // =============================================
    public void KlaimPoinTanaman()
    {
        players[currentPlayerIndex].score += 150;
        // Tanaman tidak perlu notifikasi pilihan, langsung lanjut
        panelInfoTanaman.SetActive(false);
        TampilkanNotif("Selamat! Kamu berhasil mengklaim tanaman.\n+150 Poin! 🌿");
    }

    public void JawabQuiz(bool isCorrect)
    {
        panelQuiz.SetActive(false);
        if (isCorrect)
        {
            players[currentPlayerIndex].score += 100;
            TampilkanNotif("Jawaban kamu BENAR! ✅\nSelamat, kamu mendapat +100 Poin.");
        }
        else
        {
            TampilkanNotif("Sayang sekali, jawabanmu SALAH! ❌\nPoin kamu tidak bertambah.");
        }
    }

    public void JawabSerigala(bool isCorrect)
    {
        panelSerigala.SetActive(false);
        if (isCorrect)
        {
            players[currentPlayerIndex].score += 100;
            TampilkanNotif("Jawaban kamu BENAR! 🐺✅\nKamu berhasil menghindar dan mendapat +100 Poin.");
        }
        else
        {
            players[currentPlayerIndex].score -= 100;
            TampilkanNotif("Oh tidak! Jawaban kamu SALAH! 🐺❌\nKamu terkena serangan Serigala. Poin dikurangi -100.");
        }
    }

    // =============================================
    // NOTIFIKASI: Tampil & Tutup
    // =============================================

    // Tampilkan panel notif dengan pesan tertentu
    private void TampilkanNotif(string pesan)
    {
        if (panelNotif == null)
        {
            // Jika panelNotif belum diset di Inspector, langsung lanjut giliran
            NextTurn();
            panelGameplay.SetActive(true);
            return;
        }
        textNotif.text = pesan;
        panelNotif.SetActive(true);
    }

    // Dipanggil oleh tombol OK/Lanjut di dalam PanelNotif
    public void TutupNotifDanGantiGiliran()
    {
        panelNotif.SetActive(false);
        panelGameplay.SetActive(true);
        NextTurn();
    }

    // =============================================
    // AKHIR GAME & NAVIGASI
    // =============================================
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

    // Tombol "Kembali" di PanelResult → kembali ke main menu
public void KembaliKeMenu() { UnityEngine.SceneManagement.SceneManager.LoadScene("HOMEPAGE"); }
}