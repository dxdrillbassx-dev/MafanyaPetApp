using System;
using System.Drawing;
using System.Media;
using System.Windows.Forms;
using System.IO;

namespace MafanyaPetApp
{
    public partial class Form1 : Form
    {
        private bool isDragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;
        private PictureBox pictureBox;
        private Timer animationTimer;
        private int currentFrame = 0;
        private System.Timers.Timer soundTimer;

        private Image[] animationFrames;

        private bool isPushing = false;
        private bool isPantsAnimationPlaying = false;
        private bool isWeaponSelectionOpen = false;
        private bool isSoundPlaying = false;
        private bool isTrousers3AnimationPlaying = false;
        private DateTime trousers3StartTime;
        private Random random = new Random();

        private ContextMenuStrip weaponMenu;
        private Label petSpeechLabel;

        public Form1()
        {
            InitializeComponent();
            this.TopMost = true;
            InitializePetSpeechLabel();
            SetupForm();
            InitializePictureBox();
            LoadAnimationFrames();
            SetupAnimationTimer();
            SetupMouseEvents();
            CreateWeaponMenu();
            SetupSoundTimer();
        }

        private void InitializePetSpeechLabel()
        {
            petSpeechLabel = new Label
            {
                AutoSize = true,
                Location = new Point(10, 10),
                ForeColor = Color.White,
                BackColor = Color.Transparent
            };
            this.Controls.Add(petSpeechLabel);
        }

        private void SetupSoundTimer()
        {
            soundTimer = new System.Timers.Timer(3000); // 3 секунды
            soundTimer.Elapsed += OnSoundTimerElapsed;
            soundTimer.AutoReset = false; // Таймер не будет сбрасываться автоматически
        }

        private void SetupForm()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.Magenta;
            this.TransparencyKey = Color.Magenta;
        }

        private void InitializePictureBox()
        {
            pictureBox = new PictureBox
            {
                SizeMode = PictureBoxSizeMode.Zoom,
                Width = 300,
                Height = 300,
                BackColor = Color.Transparent
            };
            this.Controls.Add(pictureBox);
        }

        private void LoadAnimationFrames()
        {
            // Теперь используйте AnimationData
            animationFrames = AnimationData.StickFrames; // начальная анимация
        }

        private void SetupAnimationTimer()
        {
            animationTimer = new Timer
            {
                Interval = 100 // Интервал в миллисекундах
            };
            animationTimer.Tick += new EventHandler(OnAnimationTick);
            animationTimer.Start();
        }

        private void SetupMouseEvents()
        {
            pictureBox.MouseDown += new MouseEventHandler(PetForm_MouseDown);
            pictureBox.MouseMove += new MouseEventHandler(PetForm_MouseMove);
            pictureBox.MouseUp += new MouseEventHandler(PetForm_MouseUp);
            this.Click += new EventHandler(Form_Click);
        }

        private void CreateWeaponMenu()
        {
            weaponMenu = new ContextMenuStrip();
            weaponMenu.Items.Add("Палка", null, (s, e) => ChangeAnimation("stick"));
            weaponMenu.Items.Add("Пистолет", null, (s, e) => ChangeAnimation("gun"));
            weaponMenu.Items.Add("Автомат", null, (s, e) => ChangeAnimation("rifle"));
            weaponMenu.Items.Add("Трусики", null, (s, e) => ChangeAnimation("trousers"));
            weaponMenu.Closing += (s, e) => isWeaponSelectionOpen = false;
        }

        private void ChangeAnimation(string weapon)
        {
            // Сбрасываем флаги анимации
            isPantsAnimationPlaying = false;
            isTrousers3AnimationPlaying = false;

            switch (weapon)
            {
                case "stick":
                    animationFrames = AnimationData.StickFrames; // Убедитесь, что вы загружаете правильные кадры
                    currentFrame = 0;
                    break;
                case "gun":
                    animationFrames = AnimationData.GunFrames;
                    currentFrame = 0;
                    break;
                case "rifle":
                    animationFrames = AnimationData.RifleFrames;
                    currentFrame = 0;
                    break;
                case "trousers":
                    SetTrousersAnimation(AnimationData.TrousersFrames);
                    break;
                case "trousers2":
                    SetTrousersAnimation(AnimationData.Trousers2Frames);
                    break;
                case "trousers3":
                    SetTrousers3Animation();
                    break;
            }
        }

        private void SetTrousersAnimation(Image[] frames)
        {
            animationFrames = frames;
            isPantsAnimationPlaying = true;
            currentFrame = 0;

            // Проигрываем звук при активации анимации трусиков
            PlayTrousersStartSound();
        }

        private void SetTrousers3Animation()
        {
            animationFrames = AnimationData.Trousers3Frames; // Замените на ваши кадры анимации "трусики3"
            currentFrame = 0;
            isTrousers3AnimationPlaying = true;
            trousers3StartTime = DateTime.Now; // Запоминаем время начала анимации
        }

        private void PlayTrousersStartSound()
        {
            if (SoundData.TrousersStartSounds.Length > 0)
            {
                int index = random.Next(SoundData.TrousersStartSounds.Length);
                string soundFilePath = SoundData.TrousersStartSounds[index];

                if (!File.Exists(soundFilePath))
                {
                    MessageBox.Show($"Файл звука не найден: {soundFilePath}");
                    return;
                }

                try
                {
                    SoundPlayer player = new SoundPlayer(soundFilePath);
                    player.Play();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при воспроизведении звука: {ex.Message}");
                }
            }
        }

        private void OnAnimationTick(object sender, EventArgs e)
        {
            if (isDragging)
            {
                UpdateDragAnimation();
            }
            else if (isPushing)
            {
                UpdatePushAnimation();
            }
            else
            {
                UpdateNormalAnimation();
            }
        }

        private void UpdateDragAnimation()
        {
            currentFrame = (currentFrame + 1) % AnimationData.DragFrames.Length;
            pictureBox.Image = AnimationData.DragFrames[currentFrame];
        }

        private void UpdatePushAnimation()
        {
            currentFrame++;
            if (animationFrames == AnimationData.StickFrames && currentFrame >= AnimationData.PushStickFrames.Length)
            {
                isPushing = false;
                currentFrame = 0;
            }
            else if (animationFrames == AnimationData.GunFrames && currentFrame >= AnimationData.PushGunFrames.Length)
            {
                isPushing = false;
                currentFrame = 0;
            }
            else if (animationFrames == AnimationData.RifleFrames && currentFrame >= AnimationData.PushRifleFrames.Length)
            {
                isPushing = false;
                currentFrame = 0;
            }
            else
            {
                pictureBox.Image = GetPushFrame();
            }
        }

        private void UpdateNormalAnimation()
        {
            currentFrame = (currentFrame + 1) % animationFrames.Length;

            if (animationFrames == AnimationData.TrousersFrames && currentFrame >= AnimationData.TrousersFrames.Length)
            {
                currentFrame = 0;
            }
            else if (animationFrames == AnimationData.Trousers2Frames)
            {
                if (currentFrame >= AnimationData.Trousers2Frames.Length)
                {
                    ChangeAnimation("trousers3");
                    return;
                }
                PlayRandomTrousers2Sound(); // Воспроизведение звуков для "трусики 2"
            }
            else if (animationFrames == AnimationData.Trousers3Frames && currentFrame >= AnimationData.Trousers3Frames.Length)
            {
                currentFrame = 0;
            }

            if (isTrousers3AnimationPlaying)
            {
                TimeSpan elapsed = DateTime.Now - trousers3StartTime;
                if (elapsed.TotalSeconds >= 3) // Если прошло 3 секунды
                {
                    // Возвращаемся к анимации "палка"
                    ChangeAnimation("stick");
                    return;
                }
            }

            // Обычное обновление анимации
            currentFrame = (currentFrame + 1) % animationFrames.Length;
            pictureBox.Image = animationFrames[currentFrame];

            pictureBox.Image = animationFrames[currentFrame];
        }

        private void PlayRandomTrousers2Sound()
        {
            if (isSoundPlaying) return; // Если звук уже играет, выходим

            int index = random.Next(SoundData.Trousers2Sounds.Length);
            string soundFilePath = SoundData.Trousers2Sounds[index];

            if (!File.Exists(soundFilePath))
            {
                MessageBox.Show($"Файл звука не найден: {soundFilePath}");
                return;
            }

            try
            {
                SoundPlayer player = new SoundPlayer(soundFilePath);
                player.Play();
                isSoundPlaying = true; // Устанавливаем флаг, что звук играет
                soundTimer.Start(); // Запускаем таймер
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при воспроизведении звука: {ex.Message}");
            }
        }

        private void OnSoundTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            isSoundPlaying = false; // Звук завершен
        }

        private Image GetPushFrame()
        {
            if (animationFrames == AnimationData.StickFrames) return AnimationData.PushStickFrames[currentFrame % AnimationData.PushStickFrames.Length];
            if (animationFrames == AnimationData.GunFrames) return AnimationData.PushGunFrames[currentFrame % AnimationData.PushGunFrames.Length];
            if (animationFrames == AnimationData.RifleFrames) return AnimationData.PushRifleFrames[currentFrame % AnimationData.PushRifleFrames.Length];
            return null;
        }

        private void PetForm_MouseDown(object sender, MouseEventArgs e)
        {
            // Проверка, если анимация "трусики3" активна
            if (isTrousers3AnimationPlaying)
            {
                // Не обрабатываем нажатие, просто выходим
                return;
            }

            if (isPantsAnimationPlaying)
            {
                HandlePantsAnimation();
                return;
            }

            if (e.Button == MouseButtons.Left) StartPush();
            else if (e.Button == MouseButtons.Right) ShowWeaponMenu();
            else if (e.Button == MouseButtons.Middle) StartDragging();
        }

        private void HandlePantsAnimation()
        {
            if (animationFrames == AnimationData.TrousersFrames) ChangeAnimation("trousers2");
            else if (animationFrames == AnimationData.Trousers2Frames) ChangeAnimation("trousers3");
        }

        private void StartPush()
        {
            if (!isPushing)
            {
                isPushing = true;
                currentFrame = 0;
                PlayRandomPushSound();
            }
        }

        private void PlayRandomPushSound()
        {
            int index = random.Next(SoundData.PushSounds.Length);
            string soundFilePath = SoundData.PushSounds[index];

            if (!File.Exists(soundFilePath))
            {
                MessageBox.Show($"Файл звука не найден: {soundFilePath}");
                return;
            }

            try
            {
                SoundPlayer player = new SoundPlayer(soundFilePath);
                player.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при воспроизведении звука: {ex.Message}");
            }
        }

        private void ShowWeaponMenu()
        {
            isWeaponSelectionOpen = true;
            weaponMenu.Show(Cursor.Position);
        }

        private void StartDragging()
        {
            isDragging = true;
            dragCursorPoint = Cursor.Position;
            dragFormPoint = this.Location;
        }

        private void PetForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                int deltaX = Cursor.Position.X - dragCursorPoint.X;
                int deltaY = Cursor.Position.Y - dragCursorPoint.Y;
                this.Location = new Point(dragFormPoint.X + deltaX, dragFormPoint.Y + deltaY);
            }
        }

        private void PetForm_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;
        }

        private void Form_Click(object sender, EventArgs e)
        {
            if (isWeaponSelectionOpen)
            {
                weaponMenu.Hide();
                isWeaponSelectionOpen = false;
            }
        }
    }
}
