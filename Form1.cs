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
        Player[] players = null;
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
            RiverNumber.Text = 20.ToString();
            ClinicNumber.Text = 10.ToString();
            PitNumber.Text = 10.ToString();
            proprietorExists.Checked = true;
            eggExists.Checked = true;
            elfExists.Checked = true;
            hatExists.Checked = true;
            ozoneExists.Checked = true;
            combatForceOptions.SelectedIndex = 2;

            if (combatForceOptions.SelectedIndex < 0)
            {
                MessageBox.Show("Please choose combat force options", "Warning: No choice of combat options");
                return;
            }
            Random random = new Random(Guid.NewGuid().GetHashCode());

            BattleField.Top = groupBox1.Top;
            BattleField.Left = groupBox1.Left;
            BattleField.Width = this.Width - groupBox1.Left - 10 - 10;
            BattleField.Height = this.Height - groupBox1.Top - 35 - 10;
            groupBox1.Visible = false;
            groupBox2.Visible = false;
            BattleField.Visible = true;

            int Temp;
            if (!int.TryParse(textBox1.Text, out Temp))
            {
                textBox1.Text = "Invalid input";
                MessageBox.Show("Invalid player number");
                return;
            }
            else
            {
                Player.PlayerNumber = Temp;
                Player.PlayerRemainedNumber = Temp;

                if (proprietorExists.Checked)
                {
                    proprieter = new Proprieter(BattleField);
                }
                if (eggExists.Checked)
                {
                    egg = new Egg(BattleField);
                }
                if (elfExists.Checked)
                {
                    elf = new Elf(BattleField);
                }
                if (hatExists.Checked)
                {
                    hat = new Hat(combatForceOptions.SelectedIndex, BattleField);
                }
                if (ozoneExists.Checked)
                {
                    ozone = new Ozone(combatForceOptions.SelectedIndex, BattleField);
                }

                players = new Player[Temp];
                Player.CombatForceLevelCache = new double[Temp / 4];
                StreamReader PlayerName = new StreamReader(@"./PlayerName.txt");
                for (int i = 0; i < Temp; i++)
                {
                    players[i] = new Player(i, combatForceOptions.SelectedIndex, BattleField)
                    {
                        PlayerName = PlayerName.ReadLine()                 
                    };
                    players[i].PlayerLabel.Text = $"{players[i].PlayerName} {players[i].CombatForceLevel.ToString()}";
                    players[i].LocationChanged += new LocationChangedEventHandler(Moved);
                }
                PlayerName.Close();
                Array.Sort(players);
                // Horizontal Order Sorting
                //for (int i = 0; i < Player.PlayerNumber; i++)
                //{
                //    for (int j = 0; j < Player.PlayerNumber; j++)
                //    {
                //        if (players[i].PlayerLabel.Left > players[j].PlayerLabel.Left && players[i].HorizontalOrder < players[j].HorizontalOrder)
                //        {
                //            int temp = players[i].HorizontalOrder;
                //            players[i].HorizontalOrder = players[j].HorizontalOrder;
                //            players[j].HorizontalOrder = temp;
                //        }
                //    }
                //}
            }// player number
            if (!int.TryParse(MountainNumber.Text, out Temp))
            {
                MountainNumber.Text = "Invalid input";
                MessageBox.Show("Invalid mountain number");
                return;
            }
            else
            {
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
                RiverNumber.Text = "Invalid input";
                MessageBox.Show("Invalid river number");
                return;
            }
            else
            {
                rivers = new River[Temp];
                double switchL = 5 * (Math.Atan(-Temp / 10) / 8 + Math.PI / 16);
                for (int i = 0; i < Temp; i++)
                {
                    rivers[i] = new River(switchL, BattleField);
                }
            }// River
            if (!int.TryParse(ClinicNumber.Text, out Temp))
            {
                ClinicNumber.Text = "Invalid input";
                MessageBox.Show("Invalid clinic number");
                return;
            }
            else
            {
                clinics = new Clinic[Temp];
                Clinic.NumberOfClinic = Temp;
                for (int i = 0; i < Temp; i++)
                {
                    clinics[i] = new Clinic(10, 10, BattleField);
                }
            }// Clinic
            if (!int.TryParse(PitNumber.Text, out Temp))
            {
                PitNumber.Text = "Invalid input";
                MessageBox.Show("Invalid pit number");
                return;
            }
            else
            {
                pits = new Pit[Temp];
                for (int i = 0; i < Temp; i++)
                {
                    pits[i] = new Pit(i, 10, 10, BattleField);
                }
            }// Pit  

            isGenerated = true;
            playerRemained.Text = Player.PlayerRemainedNumber.ToString();
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
                elf.TimerOfRecharge.Enabled = true;
            }

            isStarted = true;
            timerForBattle.Enabled = true;
            //Timer3.Enabled = True
            //Timer4.Enabled = True
            //Timer5.Enabled = True
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
            gamingTime.Text = "000:00";
            if (hat != null)
            {
                hat.TimerCountDown.Enabled = false;
            }
            if (elf != null)
            {
                elf.TimerOfRecharge.Enabled = false;
            }
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
                if (elf != null)
                {
                    elf.TimerOfRecharge.Enabled = true;
                }
                pause.Text = "Pause";
                isStarted = true;
            }
        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            for (int i = 0; i < Player.PlayerNumber; ++i)
            {
                players[i].Move(BattleField);
                //players[i].UpdateSpeed();
                players[i].UpdateLabel();
            }
            Array.Sort(players);
            //for (int i = 0; i < Player.PlayerNumber; ++i)
            //{
            //    for (int j = i + 1; j < Player.PlayerNumber; ++j)
            //    {
            //        players[i].Battle(players[j], killOptions);
            //    }
            //    players[i].UpdateColor(BattleField);
            //}
            for (int i = 0; i < Player.PlayerNumber; ++i)
            {
                for (int j = 0; j < mountains.Length; ++j)
                {
                    players[i].CollapsedMountain(mountains[j]);
                    while (players[i].Polygon.IsCover(mountains[j].Polygon))
                    {
                        players[i].Move(BattleField);
                    }
                }
                //for (int j = 0; j < rivers.Length; ++j)
                //{
                //    players[i].CollapsedRiver(rivers[j]);
                //}
                //for (int j = 0; j < clinics.Length; ++j)
                //{
                //    players[i].CollapseClinic(clinics[j]);
                //}
                //for (int j = 0; j < pits.Length; ++j)
                //{
                //    players[i].CollapsedPit(pits[j], pits);
                //}
            }
            playerRemained.Text = Player.PlayerRemainedNumber.ToString();
        }

        private void BattleField_Paint(object sender, PaintEventArgs e)
        {
            Graphics DrawBarriers = e.Graphics;
            Pen DrawMountainPen = new Pen(Brushes.Black, 2);
            foreach (var mount in mountains)
                DrawBarriers.DrawLine(DrawMountainPen, mount.StartPoint, mount.EndPoint);

            Pen DrawRiverPen = new Pen(Brushes.Azure, 2);
            foreach (var river in rivers)
                DrawBarriers.DrawLine(DrawRiverPen, river.StartPoint, river.EndPoint);

            Pen DrawClinicPen = new Pen(Brushes.Chocolate, 2);
            Brush DrawClinicBrush = new SolidBrush(Color.Cornsilk);
            foreach (var clinic in clinics)
            {
                DrawBarriers.DrawRectangle(DrawClinicPen, clinic.Left, clinic.Top, clinic.Width, clinic.Height);
                DrawBarriers.FillRectangle(DrawClinicBrush, clinic.Left, clinic.Top, clinic.Width, clinic.Height);
            }

            Pen DrawPitPen = new Pen(Brushes.DarkSeaGreen, 2);
            Brush DrawPitBrush = new SolidBrush(Color.MintCream);
            foreach (var pit in pits)
            {
                DrawBarriers.DrawRectangle(DrawPitPen, pit.Left, pit.Top, pit.Width, pit.Height);
                DrawBarriers.FillRectangle(DrawPitBrush, pit.Left, pit.Top, pit.Width, pit.Height);
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
