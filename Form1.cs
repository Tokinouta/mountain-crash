using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        List<Player> players = null;
        Mountain[] mountains = null;
        River[] rivers = null;
        Clinic[] clinics = null;
        Pit[] pits = null;

        Proprieter proprieter = null;
        Hat hat = null;
        Egg egg = null;
        Elf elf = null;
        Ozone ozone = null;

        //sec为秒计时，mini为分计时
        int TimeInSecond, TimeInMinute;
        bool isGenerated = false, isStarted = false;
        
        void CleanUp()
        {
            BattleField.Refresh();
            BattleField.Controls.Clear();
            BattleField.Visible = false;
            groupBox1.Visible = true;
            groupBox2.Visible = true;
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Generation_Click(object sender, EventArgs e)
        {
            // horisontal: form width = max object width - 18
            // vertical: form height = max object height - 48
            textBox1.Text = 3.ToString();
            MountainNumber.Text = 1.ToString();
            RiverNumber.Text = 1.ToString();
            ClinicNumber.Text = 10.ToString();
            PitNumber.Text = 10.ToString();
            proprietorExists.Checked = true;
            eggExists.Checked = true;
            elfExists.Checked = true;
            hatExists.Checked = true;
            ozoneExists.Checked = true;
            combatForceOptions.SelectedIndex = 2;
            killOptions.SelectedIndex = 3;

            label1.Text = "";
            if (killOptions.SelectedIndex < 0)
            {
                MessageBox.Show("Please choose kill options", "Warning: No choice of kill options");
                return;
            }
            if (combatForceOptions.SelectedIndex < 0)
            {
                MessageBox.Show("Please choose combat force options", "Warning: No choice of combat options");
                return;
            }
            Random random = new Random(Guid.NewGuid().GetHashCode());

            BattleField.Top = groupBox1.Top;
            BattleField.Left = groupBox1.Left;
            BattleField.Width = Width - groupBox1.Left - 10 - 10;
            BattleField.Height = Height - groupBox1.Top - 35 - 10;
            groupBox1.Visible = false;
            groupBox2.Visible = false;
            BattleField.Visible = true;

            #region player initialization
            players = new List<Player>();
            StreamReader PlayerName;
            try
            {
                PlayerName = new StreamReader(@"./PlayerName.txt");
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show(@"请确认PlayerName.txt和此程序在同一路径。", "未发现PlayerName.txt");
                CleanUp();
                return;
            }
            catch (DirectoryNotFoundException)
            {
                MessageBox.Show(@"指定的路径无效，例如位于未映射的驱动器上。");
                CleanUp();
                return;
            }
            int order = 0;
            string nameTemp = "";
            while (!PlayerName.EndOfStream)
            {
                nameTemp = PlayerName.ReadToEnd();
            }
            string[] names = nameTemp.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            Player.PlayerNumber = names.Length;
            Player.PlayerRemainedNumber = names.Length;
            foreach (var name in names)
            {
                var p = new Player(order, combatForceOptions.SelectedIndex, BattleField)
                {
                    PlayerName = name
                };
                p.PlayerLabel.Text = $"{ p.PlayerName} {p.CombatForceLevel.ToString()}";
                p.LocationChanged += new LocationChangedEventHandler(Moved);
                players.Add(p);
                ++order;
            }
            PlayerName.Close();
            Player.CombatForceLevelCache = new double[Player.PlayerNumber / 4];

            if (proprietorExists.Checked)
            {
                proprieter = new Proprieter(BattleField);
                proprieter.LocationChanged += new LocationChangedEventHandler(Moved);
                players.Add(proprieter);
            }
            if (eggExists.Checked)
            {
                egg = new Egg(BattleField);
                egg.LocationChanged += new LocationChangedEventHandler(Moved);
                players.Add(egg);
            }
            if (elfExists.Checked)
            {
                elf = new Elf(BattleField);
                elf.LocationChanged += new LocationChangedEventHandler(Moved);
                players.Add(elf);
            }
            if (hatExists.Checked)
            {
                hat = new Hat(combatForceOptions.SelectedIndex, BattleField);
                hat.LocationChanged += new LocationChangedEventHandler(Moved);
                players.Add(hat);
            }
            if (ozoneExists.Checked)
            {
                ozone = new Ozone(combatForceOptions.SelectedIndex, BattleField);
                ozone.LocationChanged += new LocationChangedEventHandler(Moved);
                players.Add(ozone);
            }
            #endregion

            int Temp;
            if (!int.TryParse(MountainNumber.Text, out Temp))
            {
                MessageBox.Show("Invalid mountain number");
                CleanUp();
                MountainNumber.Text = "Invalid input";
                return;
            }
            else
            {
                Temp = Temp <= 0 ? 1 : Temp;
                mountains = new Mountain[Temp];
                double switchL = 5 * (Math.Atan(-Temp / 10) / 2 + Math.PI / 4);
                for (int i = 0; i < Temp; i++)
                {
                    int rand= random.Next(5);
                    mountains[i] = new Mountain(2, switchL, BattleField);
                }
            }// Mountain
            if (!int.TryParse(RiverNumber.Text, out Temp))
            {
                MessageBox.Show("Invalid river number");
                CleanUp();
                RiverNumber.Text = "Invalid input";
                return;
            }
            else
            {
                Temp = Temp <= 0 ? 1 : Temp;
                rivers = new River[Temp];
                double switchL = 5 * (Math.Atan(-Temp / 10) / 8 + Math.PI / 16);
                for (int i = 0; i < Temp; i++)
                {
                    rivers[i] = new River(switchL, BattleField);
                }
            }// River
            if (!int.TryParse(ClinicNumber.Text, out Temp))
            {
                MessageBox.Show("Invalid clinic number");
                CleanUp();
                ClinicNumber.Text = "Invalid input";
                return;
            }
            else
            {
                Temp = Temp <= 0 ? 1 : Temp;
                clinics = new Clinic[Temp];
                Clinic.NumberOfClinic = Temp;
                for (int i = 0; i < Temp; i++)
                {
                    clinics[i] = new Clinic(10, 10, BattleField);
                }
            }// Clinic
            if (!int.TryParse(PitNumber.Text, out Temp))
            {
                MessageBox.Show("Invalid pit number");
                CleanUp();
                PitNumber.Text = "Invalid input";
                return;
            }
            else
            {
                Temp = Temp <= 0 ? 1 : Temp;
                pits = new Pit[Temp];
                for (int i = 0; i < Temp; i++)
                {
                    pits[i] = new Pit(i, 10, 10, BattleField);
                }
            }// Pit  

            isGenerated = true;
            playerRemained.Text = players.Count.ToString();
            textBox1.Text = Player.PlayerNumber.ToString();
            generation.Enabled = false;
            start.Enabled = true;
            clear.Enabled = true;
        }
        private void Moved(Player player, LocationChangedEventArgs e)
        {
            player.Polygon.Move(e.DisplacementX, e.DisplacementY);
        }

        private void start_Click(object sender, EventArgs e)
        {
            Random random = new Random(Guid.NewGuid().GetHashCode());
            if (players == null)
            {
                MessageBox.Show("Please initialize first", "Warning: No players");
                return;
            }
            if (!isGenerated)
            {
                MessageBox.Show("Please initialize first", "Warning: No initialization");
            }
            generation.Enabled = false;
            start.Enabled = false;
            pause.Enabled = true;
            pause.Text = "Pause";
            clear.Enabled = true;

            TimeInSecond = 0;
            TimeInMinute = 0;
            timer.Enabled = true;
            if (hat != null)
            {
                hat.TimerCountDown.Enabled = true;
            }
            if (elf != null)
            {
                //elf.TimerOfProtection.Enabled = true;
                elf.TimerOfRecharge.Enabled = true;
            }

            isStarted = true;
            timerForBattle.Enabled = true;
            //Timer3.Enabled = True
            //Timer4.Enabled = True
            //Timer5.Enabled = True
        }

        private void pause_Click(object sender, EventArgs e)
        {
            if (isStarted)
            {
                timer.Enabled = false;
                timerForBattle.Enabled = false;
                //Timer3.Enabled = False
                //Timer5.Enabled = False
                if (hat != null)
                {
                    hat.TimerCountDown.Enabled = false;
                }
                if (elf != null)
                {
                    elf.TimerOfProtection.Enabled = false;
                    elf.TimerOfRecharge.Enabled = false;
                }
                pause.Text = "Continue";
                isStarted = false;
            }
            else
            {
                //Timer2.Enabled = True
                //Timer3.Enabled = True
                timer.Enabled = true;
                timerForBattle.Enabled = true;
                if (hat != null)
                {
                    hat.TimerCountDown.Enabled = true;
                }
                //if (elf != null)
                //{
                //    elf.TimerOfRecharge.Enabled = true;
                //}
                pause.Text = "Pause";
                isStarted = true;
            }
        }
        
        private void Clear_Click(object sender, EventArgs e)
        {
            if (isStarted)
            {
                MessageBox.Show("Please do not clear", "Warning: Game running");
                return;
            }
            BattleField.Refresh();
            BattleField.Controls.Clear();
            BattleField.Visible = false;

            generation.Enabled = true;
            start.Enabled = false;
            pause.Enabled = false;
            isGenerated = false;
            
            groupBox1.Visible = true;
            groupBox2.Visible = true;
            timer.Enabled = false;
            timerForBattle.Enabled = false;
            gamingTime.Text = "000:00";
            if (hat != null)
            {
                hat.TimerCountDown.Enabled = false;
            }
            if (elf != null)
            {
                elf.TimerOfProtection.Enabled = false;
                elf.TimerOfRecharge.Enabled = false;
            }
            label1.Text = "";
        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            foreach (var player in players)
            {
                if (!player.IsAlive)
                {
                    continue;
                }
                player.Move(BattleField);
                player.UpdateLabel();
            }

            foreach (var player1 in players)
            {
                if (!player1.IsAlive)
                {
                    player1.UpdateColor(BattleField);
                    continue;
                }
                foreach (var mountain in mountains)
                {
                    player1.CollapsedMountain(mountain);
                    while (player1.Polygon.IsCover(mountain.Polygon))
                    {
                        player1.Move(BattleField);
                    }
                }
                foreach (var river in rivers)
                {
                    player1.CollapsedRiver(river);
                }
                foreach (var clinic in clinics)
                {
                    player1.CollapseClinic(clinic);
                }
                foreach (var pit in pits)
                {
                    player1.CollapsedPit(pit, pits);
                }
            }

            if (proprieter != null && proprieter.IsAlive)
            {
                proprieter.FingerGame(players);
                foreach (var player2 in players)
                {
                    if (proprieter != player2 && player2.Polygon.IsCover(proprieter.Polygon))
                    {
                        proprieter.Settle(TimeInSecond);
                        player2.Settle(TimeInSecond);
                        player2.UpdateColor(BattleField);
                    }
                }
            }

            if (egg != null && egg.IsAlive)
            {
                foreach (var player2 in players)
                {
                    if (player2 != egg && player2.IsAlive && !egg.IsInEarth)
                    {
                        egg.GetIntoEarth(player2);
                        if (egg.HitPoint > 0)
                        {
                            egg.Settle(TimeInSecond);
                        }
                        egg.UpdateColor(BattleField);
                    }
                }
            }

            if (ozone != null && ozone.IsAlive)
            {
                foreach (var player2 in players)
                {
                    if (player2 != ozone && player2.IsAlive)
                    {
                        ozone.Radius(player2);
                        player2.Settle(TimeInSecond);
                        player2.UpdateColor(BattleField);
                    }
                }
            }

            foreach (var player1 in players)
            { 
                foreach (var player2 in players)
                {
                    if (player1 == proprieter)
                    {
                        player1.Settle(TimeInSecond);
                        player1.UpdateColor(BattleField);
                        continue;
                    }
                    else
                    {
                        if (player1 != player2 && player2 != proprieter && player2.IsAlive && player1.IsAlive)
                        {
                            player1.Battle(player2, killOptions);
                            player1.Settle(TimeInSecond);
                            player2.Settle(TimeInSecond);
                        }
                        player1.UpdateColor(BattleField);
                    }
                }
            }

            playerRemained.Text = Player.PlayerRemainedNumber.ToString();
            if (Player.PlayerRemainedNumber <= 1 && isStarted)
            {
                if (hat != null)
                {
                    hat.TimerCountDown.Enabled = false;
                }
                if (elf != null)
                {
                    elf.TimerOfProtection.Enabled = false;
                    elf.TimerOfRecharge.Enabled = false;
                }
                if (egg != null)
                {
                    egg.GettingIntoEarth.Enabled = false;
                }

                BattleField.Refresh();
                BattleField.Controls.Clear();
                BattleField.Visible = false;

                generation.Enabled = true;
                start.Enabled = false;
                pause.Enabled = false;
                isGenerated = false;
                isStarted = false;

                groupBox1.Visible = true;
                groupBox2.Visible = true;
                timer.Enabled = false;

                players.Sort();
                players[0].SurvivalTime = TimeInSecond;
                players[0].SurvivalRank = 1;
                foreach (var player in players)
                {
                    label1.Text += player.ShowInfo();
                }

                timerForBattle.Enabled = false;
            }
        }

        private void UpdateSpeed_Tick(object sender, EventArgs e)
        {
            foreach (var player in players)
            {
                player.UpdateSpeed();
            }
        }

        private void Textbox_Click(object sender, EventArgs e)
        {
            TextBox box = (TextBox)sender;
            box.SelectAll();
        }

        private void BattleField_Paint(object sender, PaintEventArgs e)
        {
            Graphics DrawBarriers = e.Graphics;
            Pen DrawMountainPen = new Pen(Brushes.Black, 2);
            if (mountains != null)
            {
                foreach (var mount in mountains)
                {
                    DrawBarriers.DrawLine(DrawMountainPen, mount.StartPoint, mount.EndPoint);
                }
            }

            Pen DrawRiverPen = new Pen(Brushes.Azure, 2);
            if (rivers != null)
            {
                foreach (var river in rivers)
                {
                    DrawBarriers.DrawLine(DrawRiverPen, river.StartPoint, river.EndPoint);
                }
            }

            Pen DrawClinicPen = new Pen(Brushes.Chocolate, 2);
            Brush DrawClinicBrush = new SolidBrush(Color.Cornsilk);
            if (clinics != null)
            {
                foreach (var clinic in clinics)
                {
                    DrawBarriers.DrawRectangle(DrawClinicPen, clinic.Left, clinic.Top, clinic.Width, clinic.Height);
                    DrawBarriers.FillRectangle(DrawClinicBrush, clinic.Left, clinic.Top, clinic.Width, clinic.Height);
                }
            }

            Pen DrawPitPen = new Pen(Brushes.DarkSeaGreen, 2);
            Brush DrawPitBrush = new SolidBrush(Color.MintCream);
            if (pits != null)
            {
                foreach (var pit in pits)
                {
                    DrawBarriers.DrawRectangle(DrawPitPen, pit.Left, pit.Top, pit.Width, pit.Height);
                    DrawBarriers.FillRectangle(DrawPitBrush, pit.Left, pit.Top, pit.Width, pit.Height);
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            TimeInSecond++;
            TimeInMinute = Convert.ToInt32(Math.Floor(Convert.ToDouble(TimeInSecond) / 60));
            gamingTime.Text = $"{TimeInMinute:D3}:{TimeInSecond % 60:D2}";
        }
    }
}
