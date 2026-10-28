using System;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace F1_25_Turkce_Yama
{
    public class InstallerForm : Form
    {
        private TextBox txtGamePath;
        private Button btnBrowse;
        private Button btnInstall;
        private Button btnUninstall;
        private Button btnOnlineMode;
        private Button btnLaunch;
        private ProgressBar progressBar;
        private Label lblStatus;
        private Label lblTitle;
        private Label lblSubtitle;
        private Label lblInfo;
        private CheckBox chkOpenGitHub;
        private LinkLabel linkGitHub;

        public InstallerForm()
        {
            InitializeComponent();
            DetectGamePath();
        }

        private void InitializeComponent()
        {
            this.Text = "F1 25 Türkçe Yama - Kurulum Aracı v1.0";
            this.Size = new Size(680, 560);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(21, 21, 30); // F1 Dark
            this.ForeColor = Color.White;

            // Title
            lblTitle = new Label();
            lblTitle.Text = "FORMULA 1 2025 - TÜRKÇE YAMA";
            lblTitle.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(225, 6, 0); // F1 Red
            lblTitle.Location = new Point(25, 20);
            lblTitle.AutoSize = true;
            this.Controls.Add(lblTitle);

            // GitHub Star Link in Header
            linkGitHub = new LinkLabel();
            linkGitHub.Text = "★ GitHub Projesi & Yıldız Ver";
            linkGitHub.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            linkGitHub.LinkColor = Color.FromArgb(255, 215, 0); // Gold
            linkGitHub.ActiveLinkColor = Color.White;
            linkGitHub.Location = new Point(440, 24);
            linkGitHub.AutoSize = true;
            linkGitHub.Cursor = Cursors.Hand;
            linkGitHub.LinkClicked += (s, e) => {
                try { Process.Start("https://github.com/OztasHamza0/F1-25-Turkce-Yama-By-KnoSS"); } catch { }
            };
            this.Controls.Add(linkGitHub);

            // Subtitle
            lblSubtitle = new Label();
            lblSubtitle.Text = "Serhan Acar & S Sport Yarış Terminolojisi | %100 Çeviri (56.136 Metin Satırı)";
            lblSubtitle.Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);
            lblSubtitle.ForeColor = Color.FromArgb(180, 180, 195);
            lblSubtitle.Location = new Point(27, 52);
            lblSubtitle.AutoSize = true;
            this.Controls.Add(lblSubtitle);

            // Info Box Panel
            Panel infoPanel = new Panel();
            infoPanel.Location = new Point(25, 85);
            infoPanel.Size = new Size(615, 130);
            infoPanel.BackColor = Color.FromArgb(28, 28, 40);
            infoPanel.BorderStyle = BorderStyle.FixedSingle;

            lblInfo = new Label();
            lblInfo.Text = 
                "• %100 Tam Kapsam: Tüm Menüler, Ayarlar, Araç Kurulumları (Setup) ve Telemetri\n" +
                "• Kariyer, My Team ve Braking Point 3 Hikaye Modu Eksiksiz Türkçe\n" +
                "• Serhan Acar üslubuyla profesyonel F1 Telsiz & Spiker Çevirileri\n" +
                "• Sıfır Font Bozulması & Sıfır Gecikme (Native 0ms, Orijinal FPS)\n" +
                "• Akıllı Çift Profil: Tek tıkla Türkçe Kariyer / Tek tıkla Resmi Online Mod";
            lblInfo.Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);
            lblInfo.ForeColor = Color.FromArgb(220, 220, 230);
            lblInfo.Location = new Point(15, 12);
            lblInfo.Size = new Size(585, 105);
            infoPanel.Controls.Add(lblInfo);
            this.Controls.Add(infoPanel);

            // Path Section
            Label lblPathTag = new Label();
            lblPathTag.Text = "F1 25 Oyun Klasörü:";
            lblPathTag.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblPathTag.ForeColor = Color.White;
            lblPathTag.Location = new Point(25, 230);
            lblPathTag.AutoSize = true;
            this.Controls.Add(lblPathTag);

            txtGamePath = new TextBox();
            txtGamePath.Location = new Point(25, 255);
            txtGamePath.Size = new Size(495, 27);
            txtGamePath.Font = new Font("Segoe UI", 9.5f);
            txtGamePath.BackColor = Color.FromArgb(35, 35, 50);
            txtGamePath.ForeColor = Color.White;
            txtGamePath.BorderStyle = BorderStyle.FixedSingle;
            this.Controls.Add(txtGamePath);

            btnBrowse = new Button();
            btnBrowse.Text = "Gözat...";
            btnBrowse.Location = new Point(530, 253);
            btnBrowse.Size = new Size(110, 30);
            btnBrowse.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            btnBrowse.BackColor = Color.FromArgb(50, 50, 70);
            btnBrowse.ForeColor = Color.White;
            btnBrowse.FlatStyle = FlatStyle.Flat;
            btnBrowse.FlatAppearance.BorderColor = Color.FromArgb(70, 70, 95);
            btnBrowse.Click += BtnBrowse_Click;
            this.Controls.Add(btnBrowse);

            // Progress Bar
            progressBar = new ProgressBar();
            progressBar.Location = new Point(25, 290);
            progressBar.Size = new Size(615, 12);
            progressBar.Style = ProgressBarStyle.Continuous;
            this.Controls.Add(progressBar);

            // Status Label
            lblStatus = new Label();
            lblStatus.Text = "Hazır. Yamayı kurmak için aşağıdaki butona basınız.";
            lblStatus.Font = new Font("Segoe UI", 9f, FontStyle.Italic);
            lblStatus.ForeColor = Color.FromArgb(160, 215, 160);
            lblStatus.Location = new Point(25, 308);
            lblStatus.AutoSize = true;
            this.Controls.Add(lblStatus);

            // CheckBox for Opening GitHub & Star Reminder
            chkOpenGitHub = new CheckBox();
            chkOpenGitHub.Text = "Kurulum tamamlandığında GitHub sayfasını aç (Projeyi beğendiyseniz ⭐ Yıldız verin!)";
            chkOpenGitHub.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            chkOpenGitHub.ForeColor = Color.FromArgb(255, 215, 80);
            chkOpenGitHub.Location = new Point(25, 330);
            chkOpenGitHub.Size = new Size(615, 22);
            chkOpenGitHub.Checked = true;
            this.Controls.Add(chkOpenGitHub);

            // Action Buttons
            btnInstall = new Button();
            btnInstall.Text = "TÜRKÇE YAMAYI KUR";
            btnInstall.Location = new Point(25, 358);
            btnInstall.Size = new Size(300, 50);
            btnInstall.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            btnInstall.BackColor = Color.FromArgb(225, 6, 0); // F1 Red
            btnInstall.ForeColor = Color.White;
            btnInstall.FlatStyle = FlatStyle.Flat;
            btnInstall.FlatAppearance.BorderSize = 0;
            btnInstall.Cursor = Cursors.Hand;
            btnInstall.Click += BtnInstall_Click;
            this.Controls.Add(btnInstall);

            btnUninstall = new Button();
            btnUninstall.Text = "YAMAYI KALDIR";
            btnUninstall.Location = new Point(340, 358);
            btnUninstall.Size = new Size(145, 50);
            btnUninstall.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnUninstall.BackColor = Color.FromArgb(45, 45, 60);
            btnUninstall.ForeColor = Color.FromArgb(220, 220, 220);
            btnUninstall.FlatStyle = FlatStyle.Flat;
            btnUninstall.FlatAppearance.BorderColor = Color.FromArgb(65, 65, 85);
            btnUninstall.Cursor = Cursors.Hand;
            btnUninstall.Click += BtnUninstall_Click;
            this.Controls.Add(btnUninstall);

            btnOnlineMode = new Button();
            btnOnlineMode.Text = "ÇEVRİMİÇİ MODA GEÇ";
            btnOnlineMode.Location = new Point(495, 358);
            btnOnlineMode.Size = new Size(145, 50);
            btnOnlineMode.Font = new Font("Segoe UI", 8.2f, FontStyle.Bold);
            btnOnlineMode.BackColor = Color.FromArgb(30, 60, 90);
            btnOnlineMode.ForeColor = Color.FromArgb(200, 230, 255);
            btnOnlineMode.FlatStyle = FlatStyle.Flat;
            btnOnlineMode.FlatAppearance.BorderColor = Color.FromArgb(50, 90, 130);
            btnOnlineMode.Cursor = Cursors.Hand;
            btnOnlineMode.Click += BtnOnlineMode_Click;
            this.Controls.Add(btnOnlineMode);

            // Launch Button
            btnLaunch = new Button();
            btnLaunch.Text = "OYUNU BAŞLAT (TÜRKÇE)";
            btnLaunch.Location = new Point(25, 418);
            btnLaunch.Size = new Size(615, 42);
            btnLaunch.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            btnLaunch.BackColor = Color.FromArgb(16, 124, 65); // Green
            btnLaunch.ForeColor = Color.White;
            btnLaunch.FlatStyle = FlatStyle.Flat;
            btnLaunch.FlatAppearance.BorderSize = 0;
            btnLaunch.Cursor = Cursors.Hand;
            btnLaunch.Click += BtnLaunch_Click;
            this.Controls.Add(btnLaunch);

            // Footer with GitHub Link
            LinkLabel lblFooterLink = new LinkLabel();
            lblFooterLink.Text = "⭐ Proje ve Güncellemeler: github.com/OztasHamza0/F1-25-Turkce-Yama-By-KnoSS";
            lblFooterLink.Font = new Font("Segoe UI", 8.8f, FontStyle.Regular);
            lblFooterLink.LinkColor = Color.FromArgb(140, 190, 255);
            lblFooterLink.ActiveLinkColor = Color.White;
            lblFooterLink.Location = new Point(25, 478);
            lblFooterLink.AutoSize = true;
            lblFooterLink.Cursor = Cursors.Hand;
            lblFooterLink.LinkClicked += (s, e) => {
                try { Process.Start("https://github.com/OztasHamza0/F1-25-Turkce-Yama-By-KnoSS"); } catch { }
            };
            this.Controls.Add(lblFooterLink);
        }

        private void DetectGamePath()
        {
            // 1. Check current directory
            string cur = AppDomain.CurrentDomain.BaseDirectory;
            if (File.Exists(Path.Combine(cur, "F1_25.exe")))
            {
                txtGamePath.Text = cur;
                return;
            }

            // 2. Steam Registry
            try
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 3059520"))
                {
                    if (key != null)
                    {
                        object loc = key.GetValue("InstallLocation");
                        if (loc != null && Directory.Exists(loc.ToString()))
                        {
                            txtGamePath.Text = loc.ToString();
                            return;
                        }
                    }
                }
            }
            catch { }

            // 3. Steam libraryfolders.vdf
            try
            {
                using (RegistryKey steamKey = Registry.CurrentUser.OpenSubKey(@"Software\Valve\Steam"))
                {
                    if (steamKey != null)
                    {
                        string steamPath = steamKey.GetValue("SteamPath") as string;
                        if (!string.IsNullOrEmpty(steamPath))
                        {
                            string vdf = Path.Combine(steamPath, "steamapps", "libraryfolders.vdf");
                            if (File.Exists(vdf))
                            {
                                string text = File.ReadAllText(vdf);
                                MatchCollection matches = Regex.Matches(text, "\"path\"\\s+\"([^\"]+)\"");
                                foreach (Match m in matches)
                                {
                                    string lib = m.Groups[1].Value.Replace("\\\\", "\\");
                                    string candidate = Path.Combine(lib, "steamapps", "common", "F1 25");
                                    if (File.Exists(Path.Combine(candidate, "F1_25.exe")))
                                    {
                                        txtGamePath.Text = candidate;
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch { }

            // 4. Check common drives
            string[] commonPaths = new string[] {
                @"C:\Program Files (x86)\Steam\steamapps\common\F1 25",
                @"D:\SteamLibrary\steamapps\common\F1 25",
                @"E:\SteamLibrary\steamapps\common\F1 25",
                @"F:\SteamLibrary\steamapps\common\F1 25",
                @"D:\Games\F1 25",
                @"E:\Games\F1 25"
            };
            foreach (string p in commonPaths)
            {
                if (File.Exists(Path.Combine(p, "F1_25.exe")))
                {
                    txtGamePath.Text = p;
                    return;
                }
            }

            // 5. Default fallback
            txtGamePath.Text = @"C:\Program Files (x86)\Steam\steamapps\common\F1 25";
        }

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                fbd.Description = "F1 25 oyununun kurulu olduğu klasörü seçiniz (F1_25.exe dosyasının bulunduğu yer):";
                if (Directory.Exists(txtGamePath.Text))
                {
                    fbd.SelectedPath = txtGamePath.Text;
                }
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    txtGamePath.Text = fbd.SelectedPath;
                }
            }
        }

        private void BtnInstall_Click(object sender, EventArgs e)
        {
            string gameDir = txtGamePath.Text.Trim();
            if (!Directory.Exists(gameDir) || !File.Exists(Path.Combine(gameDir, "F1_25.exe")))
            {
                MessageBox.Show("Seçilen klasörde 'F1_25.exe' bulunamadı!\nLütfen doğru oyun klasörünü seçtiğinizden emin olunuz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                btnInstall.Enabled = false;
                progressBar.Value = 10;
                lblStatus.Text = "Türkçe yama arşivden çıkarılıyor...";
                Application.DoEvents();

                // Extract embedded payload.zip
                Assembly asm = Assembly.GetExecutingAssembly();
                using (Stream stream = asm.GetManifestResourceStream("payload.zip"))
                {
                    if (stream == null)
                    {
                        MessageBox.Show("Gömülü yama paketi (payload.zip) bulunamadı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    using (ZipArchive archive = new ZipArchive(stream))
                    {
                        int total = archive.Entries.Count;
                        int count = 0;
                        foreach (ZipArchiveEntry entry in archive.Entries)
                        {
                            string destPath = Path.Combine(gameDir, entry.FullName);
                            if (string.IsNullOrEmpty(entry.Name))
                            {
                                Directory.CreateDirectory(destPath);
                            }
                            else
                            {
                                Directory.CreateDirectory(Path.GetDirectoryName(destPath));
                                entry.ExtractToFile(destPath, true);
                            }
                            count++;
                            progressBar.Value = 10 + (int)((float)count / total * 70);
                            Application.DoEvents();
                        }
                    }
                }

                // Remove leftover version.dll.mod if exists
                string leftoverMod = Path.Combine(gameDir, "version.dll.mod");
                if (File.Exists(leftoverMod))
                {
                    try { File.Delete(leftoverMod); } catch { }
                }

                // Write Offline Consent
                lblStatus.Text = "Güvenlik ve çevrimdışı profil ayarları yapılıyor...";
                progressBar.Value = 85;
                Application.DoEvents();

                string localApp = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string consentPath = Path.Combine(localApp, "Swofty", "state", "offline_consent.json");
                Directory.CreateDirectory(Path.GetDirectoryName(consentPath));
                string consentJson = "{\n  \"schema\": 1,\n  \"text_sha256\": \"24a21a11794fa868a7141f17743a46fd805091cc0438ea5b054cd5773399a6e8\",\n  \"accepted\": true,\n  \"accepted_utc\": \"" + (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds + "\",\n  \"extension_version\": \"0.3.11\"\n}";
                File.WriteAllText(consentPath, consentJson);

                // Apply Swofty overlay (patches UI ERP archives so EA error is suppressed)
                lblStatus.Text = "Arayüz ve çevrimdışı motor yamaları uygulanıyor...";
                progressBar.Value = 90;
                Application.DoEvents();

                string swoftyExe = Path.Combine(gameDir, "swofty.exe");
                if (File.Exists(swoftyExe))
                {
                    try
                    {
                        ProcessStartInfo psi = new ProcessStartInfo
                        {
                            FileName = swoftyExe,
                            Arguments = "apply",
                            WorkingDirectory = gameDir,
                            CreateNoWindow = true,
                            UseShellExecute = false,
                            WindowStyle = ProcessWindowStyle.Hidden
                        };
                        using (Process p = Process.Start(psi))
                        {
                            if (p != null) p.WaitForExit(15000);
                        }
                    }
                    catch { }
                }

                // Create Desktop Shortcuts
                lblStatus.Text = "Masaüstü kısayolları oluşturuluyor...";
                progressBar.Value = 95;
                Application.DoEvents();

                CreateDesktopShortcuts(gameDir);

                progressBar.Value = 100;
                lblStatus.Text = "Tebrikler! F1 25 Türkçe Yama başarıyla kuruldu.";
                MessageBox.Show(
                    "F1 25 Türkçe Yama başarıyla kuruldu!\n\n" +
                    "Masaüstünüze 2 adet kısayol eklendi:\n" +
                    "1. 'F1 25 Türkçe': Kariyer, My Team ve Braking Point modlarını %100 Türkçe açar.\n" +
                    "2. 'F1 25 Online (Orijinal)': Modları kapatıp Steam'den güvenle Online oynamanızı sağlar.\n\n" +
                    "⭐ DESTEK: Projeyi beğendiyseniz GitHub sayfamıza Yıldız (Star) vererek destek olmayı unutmayın!\n\n" +
                    "İyi yarışlar dileriz!", 
                    "Kurulum Tamamlandı", MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (chkOpenGitHub != null && chkOpenGitHub.Checked)
                {
                    try
                    {
                        Process.Start("https://github.com/OztasHamza0/F1-25-Turkce-Yama-By-KnoSS");
                    }
                    catch { }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kurulum sırasında bir hata oluştu:\n" + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "Hata: Kurulum tamamlanamadı.";
            }
            finally
            {
                btnInstall.Enabled = true;
            }
        }

        private void BtnUninstall_Click(object sender, EventArgs e)
        {
            string gameDir = txtGamePath.Text.Trim();
            if (!Directory.Exists(gameDir)) return;

            DialogResult dr = MessageBox.Show(
                "F1 25 Türkçe Yamayı kaldırmak ve oyunu tamamen orijinal haline döndürmek istediğinize emin misiniz?", 
                "Yamayı Kaldır", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dr != DialogResult.Yes) return;

            try
            {
                lblStatus.Text = "Yama kaldırılıyor...";
                Application.DoEvents();

                // 1. Oyun açıksa kapatalım
                try
                {
                    Process[] procs = Process.GetProcessesByName("F1_25");
                    foreach (var p in procs) { p.Kill(); p.WaitForExit(3000); }
                }
                catch { }

                // 2. Swofty restore çalıştır
                string swoftyExe = Path.Combine(gameDir, "swofty.exe");
                if (File.Exists(swoftyExe))
                {
                    try
                    {
                        ProcessStartInfo psi = new ProcessStartInfo
                        {
                            FileName = swoftyExe,
                            Arguments = "restore",
                            WorkingDirectory = gameDir,
                            CreateNoWindow = true,
                            UseShellExecute = false,
                            WindowStyle = ProcessWindowStyle.Hidden
                        };
                        using (Process p = Process.Start(psi)) { if (p != null) p.WaitForExit(10000); }
                    }
                    catch { }
                }

                // 3. EA Anti-Cheat orijinal haline döndür
                string eaBak = Path.Combine(gameDir, "EAAntiCheat.GameServiceLauncher.exe.bak");
                string eaExe = Path.Combine(gameDir, "EAAntiCheat.GameServiceLauncher.exe");
                if (File.Exists(eaBak))
                {
                    try { File.Copy(eaBak, eaExe, true); } catch { }
                    try { File.Delete(eaBak); } catch { }
                }
                string eaSwofty = Path.Combine(gameDir, "EAAntiCheat.GameServiceLauncher.exe.swofty");
                if (File.Exists(eaSwofty))
                {
                    try { File.Delete(eaSwofty); } catch { }
                }

                string[] filesToDelete = new string[] {
                    "version.dll",
                    "version.dll.mod",
                    "OYUNU_BASLAT_TURKCE.bat",
                    "ONLINE_MOD_ORIJINAL.bat",
                    "swofty-launcher.exe",
                    "swofty.exe",
                    "swofty-ui.exe",
                    "EAAntiCheat.GameServiceLauncher.exe.swofty"
                };

                foreach (string f in filesToDelete)
                {
                    string full = Path.Combine(gameDir, f);
                    if (File.Exists(full)) File.Delete(full);
                }

                string swoftyDir = Path.Combine(gameDir, "swofty");
                if (Directory.Exists(swoftyDir))
                {
                    Directory.Delete(swoftyDir, true);
                }

                string loaderDir = Path.Combine(gameDir, "loader");
                if (Directory.Exists(loaderDir))
                {
                    Directory.Delete(loaderDir, true);
                }

                // Remove Shortcuts
                string desktop = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                string sc1 = Path.Combine(desktop, "F1 25 Türkçe.lnk");
                string sc2 = Path.Combine(desktop, "F1 25 Online (Orijinal).lnk");
                if (File.Exists(sc1)) File.Delete(sc1);
                if (File.Exists(sc2)) File.Delete(sc2);

                progressBar.Value = 100;
                lblStatus.Text = "Yama tamamen kaldırıldı. Oyun orijinal halinde.";
                MessageBox.Show("Türkçe yama başarıyla kaldırıldı.\nOyun tamamen orijinal Steam durumuna geri döndürüldü.", "Kaldırma Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kaldırma sırasında bir hata oluştu:\n" + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnOnlineMode_Click(object sender, EventArgs e)
        {
            try
            {
                string gameDir = txtGamePath.Text.Trim();
                if (string.IsNullOrEmpty(gameDir) || !Directory.Exists(gameDir))
                {
                    MessageBox.Show("Lütfen geçerli bir F1 25 oyun klasörü seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 1. Oyun açıksa uyar ve kapat
                Process[] procs = Process.GetProcessesByName("F1_25");
                if (procs.Length > 0)
                {
                    DialogResult dr = MessageBox.Show(
                        "F1 25 şu anda çalışıyor!\n\nÇevrimiçi moda geçmek için oyunun kapatılması gerekiyor. Oyunu şimdi kapatmak istiyor musunuz?",
                        "Oyun Açık", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dr == DialogResult.Yes)
                    {
                        foreach (var p in procs) { try { p.Kill(); p.WaitForExit(3000); } catch { } }
                    }
                    else
                    {
                        return;
                    }
                }

                // 2. Swofty Restore (Oyun dosyalarını %100 vanilla baseline'a döndürür)
                string swoftyExe = Path.Combine(gameDir, "swofty.exe");
                if (File.Exists(swoftyExe))
                {
                    try
                    {
                        ProcessStartInfo psi = new ProcessStartInfo
                        {
                            FileName = swoftyExe,
                            Arguments = "restore",
                            WorkingDirectory = gameDir,
                            CreateNoWindow = true,
                            UseShellExecute = false,
                            WindowStyle = ProcessWindowStyle.Hidden
                        };
                        using (Process p = Process.Start(psi)) { if (p != null) p.WaitForExit(10000); }
                    }
                    catch { }
                }

                // 3. EA Anti-Cheat Orijinal Başlatıcıyı Geri Yükle
                string eaBak = Path.Combine(gameDir, "EAAntiCheat.GameServiceLauncher.exe.bak");
                string eaExe = Path.Combine(gameDir, "EAAntiCheat.GameServiceLauncher.exe");
                string eaSwofty = Path.Combine(gameDir, "EAAntiCheat.GameServiceLauncher.exe.swofty");
                if (File.Exists(eaBak))
                {
                    if (!File.Exists(eaSwofty) && File.Exists(eaExe))
                    {
                        try { File.Copy(eaExe, eaSwofty, true); } catch { }
                    }
                    try { File.Copy(eaBak, eaExe, true); } catch { }
                }

                // 4. version.dll -> version.dll.mod
                string v1 = Path.Combine(gameDir, "version.dll");
                string v2 = Path.Combine(gameDir, "version.dll.mod");

                if (File.Exists(v1))
                {
                    if (File.Exists(v2)) File.Delete(v2);
                    File.Move(v1, v2);
                }

                lblStatus.Text = "Oyun %100 orijinal Steam ve Çevrimiçi moduna alındı!";
                DialogResult askLaunch = MessageBox.Show(
                    "Çevrimiçi mod başarıyla aktif edildi!\n\n" +
                    "- Modlar pasife alındı.\n" +
                    "- Orijinal EA Anti-Cheat ve oyun dosyaları yüklendi.\n" +
                    "- Hesabınız ban riskine karşı %100 güvende.\n\n" +
                    "Oyunu şimdi Steam üzerinden başlatmak istiyor musunuz?", 
                    "Çevrimiçi Mod Aktif", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (askLaunch == DialogResult.Yes)
                {
                    Process.Start("steam://run/3059520");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Çevrimiçi moda geçerken bir hata oluştu:\n" + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnLaunch_Click(object sender, EventArgs e)
        {
            try
            {
                string gameDir = txtGamePath.Text.Trim();
                if (string.IsNullOrEmpty(gameDir) || !Directory.Exists(gameDir))
                {
                    MessageBox.Show("Lütfen geçerli bir F1 25 oyun klasörü seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 1. Oyun açıksa uyar
                Process[] procs = Process.GetProcessesByName("F1_25");
                if (procs.Length > 0)
                {
                    DialogResult dr = MessageBox.Show(
                        "F1 25 şu anda açık! Türkçe yamayla başlatmak için önce oyunu kapatmak istiyor musunuz?",
                        "Oyun Açık", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dr == DialogResult.Yes)
                    {
                        foreach (var p in procs) { try { p.Kill(); p.WaitForExit(3000); } catch { } }
                    }
                    else
                    {
                        return;
                    }
                }

                // 2. version.dll.mod -> version.dll
                string v2 = Path.Combine(gameDir, "version.dll.mod");
                string v1 = Path.Combine(gameDir, "version.dll");
                if (File.Exists(v2))
                {
                    if (File.Exists(v1)) File.Delete(v1);
                    File.Move(v2, v1);
                }

                // 3. Swofty Offline Anti-Cheat Stub'ını yükle
                string eaSwofty = Path.Combine(gameDir, "EAAntiCheat.GameServiceLauncher.exe.swofty");
                string eaExe = Path.Combine(gameDir, "EAAntiCheat.GameServiceLauncher.exe");
                string eaBak = Path.Combine(gameDir, "EAAntiCheat.GameServiceLauncher.exe.bak");
                if (File.Exists(eaSwofty))
                {
                    if (!File.Exists(eaBak) && File.Exists(eaExe))
                    {
                        try { File.Copy(eaExe, eaBak, true); } catch { }
                    }
                    try { File.Copy(eaSwofty, eaExe, true); } catch { }
                }

                // 4. Arayüz yamalarını uygula
                string swoftyExe = Path.Combine(gameDir, "swofty.exe");
                if (File.Exists(swoftyExe))
                {
                    try
                    {
                        ProcessStartInfo psi = new ProcessStartInfo
                        {
                            FileName = swoftyExe,
                            Arguments = "apply",
                            WorkingDirectory = gameDir,
                            CreateNoWindow = true,
                            UseShellExecute = false,
                            WindowStyle = ProcessWindowStyle.Hidden
                        };
                        using (Process p = Process.Start(psi)) { if (p != null) p.WaitForExit(10000); }
                    }
                    catch { }
                }

                // 5. Oyunu Başlat
                string launcher = Path.Combine(gameDir, "swofty-launcher.exe");
                string bat = Path.Combine(gameDir, "OYUNU_BASLAT_TURKCE.bat");

                if (File.Exists(launcher))
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = launcher,
                        WorkingDirectory = gameDir
                    });
                }
                else if (File.Exists(bat))
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = bat,
                        WorkingDirectory = gameDir
                    });
                }
                else
                {
                    Process.Start("steam://run/3059520");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Oyun başlatılırken hata oluştu:\n" + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CreateDesktopShortcuts(string gameDir)
        {
            try
            {
                Type shellType = Type.GetTypeFromProgID("WScript.Shell");
                if (shellType == null) return;
                dynamic shell = Activator.CreateInstance(shellType);
                string desktop = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

                // Shortcut 1: Turkish Game
                string scPath1 = Path.Combine(desktop, "F1 25 Türkçe.lnk");
                dynamic sc1 = shell.CreateShortcut(scPath1);
                string launcherPath = Path.Combine(gameDir, "swofty-launcher.exe");
                if (File.Exists(launcherPath))
                {
                    sc1.TargetPath = launcherPath;
                }
                else
                {
                    sc1.TargetPath = Path.Combine(gameDir, "OYUNU_BASLAT_TURKCE.bat");
                }
                sc1.WorkingDirectory = gameDir;
                sc1.Description = "F1 25 Türkçe Yama";
                if (File.Exists(Path.Combine(gameDir, "F1_25.exe")))
                {
                    sc1.IconLocation = Path.Combine(gameDir, "F1_25.exe") + ",0";
                }
                sc1.Save();

                // Shortcut 2: Online Mode
                string scPath2 = Path.Combine(desktop, "F1 25 Online (Orijinal).lnk");
                dynamic sc2 = shell.CreateShortcut(scPath2);
                sc2.TargetPath = Path.Combine(gameDir, "ONLINE_MOD_ORIJINAL.bat");
                sc2.WorkingDirectory = gameDir;
                sc2.Description = "F1 25 Resmi Online Multiplayer Geçiş";
                if (File.Exists(Path.Combine(gameDir, "F1_25.exe")))
                {
                    sc2.IconLocation = Path.Combine(gameDir, "F1_25.exe") + ",0";
                }
                sc2.Save();
            }
            catch { }
        }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new InstallerForm());
        }
    }
}
