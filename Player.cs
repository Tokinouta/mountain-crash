// 2020.1.19
// TODO: division by 0 (DONE)
// 2020.1.20
// TODO: 解决抖动问题。推测是因为碰撞检测时进入太深导致一个interval之内不能完全退出line之后引起第二次调用
//       可以考虑缩小interval
// TODO: 反弹之后的方向不对，需修正

using System;
using System.Drawing;
using System.Windows.Forms;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    //public delegate void LocationChangedEventHandler(Player player, LocationChangedEventArgs e);
    //public delegate void HitPointChangedEventHandler(Player player, HitPointChangedEventArgs e);

    public class LocationChangedEventArgs : EventArgs
    {
        double displacementX;
        double displacementY;
        public LocationChangedEventArgs(double displacementX, double displacementY) : base()
        {
            this.displacementX = displacementX;
            this.displacementY = displacementY;
        }

        public double DisplacementX { get => displacementX; set => displacementX = value; }
        public double DisplacementY { get => displacementY; set => displacementY = value; }
    }
    public class HitPointChangedEventArgs : EventArgs
    {
        public double HitPoint { get; set; }
        public GroupBox Battlefield { get; set; }
        public HitPointChangedEventArgs(double hitPoint, GroupBox battlefield)
        {
            HitPoint = hitPoint;
            Battlefield = battlefield;
        }
    }
    public class CombatForceLevelChangedEventArgs : EventArgs
    {
        public double ConbatForceLevel { get; set; }
        public CombatForceLevelChangedEventArgs(double conbatForceLevel)
        {
            ConbatForceLevel = conbatForceLevel;
        }
    }

    public class Player : IComparable<Player>
    {
        #region Property
        public static int PlayerNumber { get; set; }
        public static int PlayerRemainedNumber { get; set; }

        public GroupBox BattleField { get; set; }
        public int SpeedOfX { get; set; }
        public int SpeedOfY { get; set; }
        public double Speed { get => Math.Sqrt(Math.Pow(SpeedOfX, 2) + Math.Pow(SpeedOfY, 2)); }
        public int HitPoint { get; set; }
        public double CombatForceLevel { get; set; }
        public bool IsAlive { get; set; }
        public int Team { get; set; }

        public int CreatingOrder { get; set; }
        public int Score { get; set; }
        public int KillNumber { get; set; }
        public int SurvivalRank { get; set; }
        public int ScoreRank { get; set; }
        public int TimeScore { get; set; }
        public int AttackScore { get; set; }
        public int Bonus { get; set; }
        public double SurvivalTime { get; set; }
        public string PlayerName { get; set; }
        public Player KilledBy { get; set; }

        public Label PlayerLabel { get; set; }
        public Polygon Polygon { get; set; }
        public int CenterLeft { get => PlayerLabel.Left + PlayerLabel.Width / 2; }
        public int CenterTop { get => PlayerLabel.Top + PlayerLabel.Height / 2; }
        public bool IsPlayer { get => !GetType().IsSubclassOf(typeof(Player)); }


        #endregion

        public event EventHandler<LocationChangedEventArgs> LocationChanged;
        public event EventHandler<HitPointChangedEventArgs> HitPointChanged;
        public event EventHandler<CombatForceLevelChangedEventArgs> CombatForceLevelChanged;

        //The event-invoking method that derived classes can override.
        public virtual void OnHitPointChanged(HitPointChangedEventArgs e)
        {
            // Make a temporary copy of the event to avoid possibility of
            // a race condition if the last subscriber unsubscribes
            // immediately after the null check and before the event is raised.
            EventHandler<HitPointChangedEventArgs> handler = HitPointChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }   
        public void OnLocationChanged(LocationChangedEventArgs e)
        {
            EventHandler<LocationChangedEventArgs> handler = LocationChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        public void OnCombatForceLevelChanged(CombatForceLevelChangedEventArgs e)
        {
            EventHandler<CombatForceLevelChangedEventArgs> handler = CombatForceLevelChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }
       
        public Player(GroupBox BattleField)
        {
            Random random = new Random(Guid.NewGuid().GetHashCode());
            SpeedOfX = random.Next(-10, 10);
            SpeedOfY = random.Next(-10, 10);
            CombatForceLevel = 0;
            CreatingOrder = 0;
            HitPoint = 510;
            IsAlive = true;

            SurvivalTime = 0;
            Score = 0;
            KillNumber = 0;
            KilledBy = null;
            SurvivalRank = 0;
            ScoreRank = 0;
            TimeScore = 0;
            AttackScore = 0;
            Bonus = 0;
            PlayerLabel = new Label
            {
                Height = 15,
                Width = 100,
                Top = random.Next(BattleField.Height - 10),
                Left = random.Next(BattleField.Width - 100),
                BackColor = Color.Aquamarine
            };
            this.BattleField = BattleField;
            BattleField.Controls.Add(PlayerLabel);
            Vector[] vectors = new Vector[] { new Vector(PlayerLabel.Left, PlayerLabel.Top),
                                              new Vector(PlayerLabel.Left, PlayerLabel.Bottom),
                                              new Vector(PlayerLabel.Right, PlayerLabel.Bottom),
                                              new Vector(PlayerLabel.Right, PlayerLabel.Top) };
            Polygon = new Polygon(vectors);
            Team = -1;
        }
        public Player(int creatingOrder, int combatForceOption, GroupBox BattleField, int teamNumber)
        {
            Random random = new Random(Guid.NewGuid().GetHashCode());
            SpeedOfX = random.Next(-10, 10);
            SpeedOfY = random.Next(-10, 10);
            CreatingOrder = creatingOrder;
            HitPoint = 510;
            IsAlive = true;
            switch (combatForceOption)
            {
                case 0:
                    int temp = random.Next(5);
                    CombatForceLevel = temp == 0 ?
                        (random.Next(1, 7) * 2) :
                        (random.Next(1, 7) * 1);
                    break;
                case 1:
                    CombatForceLevel =
                        Convert.ToInt32(5 * 10 / PlayerNumber * (PlayerNumber - (creatingOrder + 1))) / 10 + 1;
                    break;
                case 2:
                    double tempd = random.NextDouble();
                    double temp1 = random.NextDouble();
                    CombatForceLevel = random.Next(1, 6);
                    if (tempd < 1 / (5 + 5 * (creatingOrder + 1) / (PlayerNumber - 1)))
                    {
                        if (CombatForceLevel == 1 && temp1 > ((creatingOrder + 1) + 1) / PlayerNumber)
                        {
                            CombatForceLevel = 12;
                        }
                        else
                        {
                            CombatForceLevel *= 2;
                        }
                    }
                    else
                    {
                        if (CombatForceLevel == 1 && temp1 > ((creatingOrder + 1) + 1) / PlayerNumber)
                        {
                            CombatForceLevel = 6;
                        }
                    }
                    break;
                default: break;
            }

            SurvivalTime = 0;
            Score = 0;
            KillNumber = 0;
            KilledBy = null;
            SurvivalRank = 0;
            ScoreRank = creatingOrder;
            TimeScore = 0;
            AttackScore = 0;
            Bonus = 0;
            PlayerLabel = new Label
            {
                Height = 15,
                Width = 100,
                Top = random.Next(BattleField.Height - 10),
                Left = random.Next(BattleField.Width - 100),
                BackColor = Color.Aquamarine
            };
            this.BattleField = BattleField;
            BattleField.Controls.Add(PlayerLabel);
            Vector[] vectors = new Vector[] { new Vector(PlayerLabel.Left, PlayerLabel.Top),
                                              new Vector(PlayerLabel.Left, PlayerLabel.Bottom),
                                              new Vector(PlayerLabel.Right, PlayerLabel.Bottom),
                                              new Vector(PlayerLabel.Right, PlayerLabel.Top) };
            Polygon = new Polygon(vectors);
            if (teamNumber == 0)
            {
                Team = creatingOrder;
            }
            else
            {
                Team = (teamNumber > 1 ? teamNumber : 1) * creatingOrder / PlayerNumber;
            }
        }

        public void Move(GroupBox BattleField)
        {
            double x = PlayerLabel.Left;
            double y = PlayerLabel.Top;
            PlayerLabel.Top += Convert.ToInt32(SpeedOfY);
            PlayerLabel.Left += Convert.ToInt32(SpeedOfX);
            //if (playerLabel.Top < 0)
            //{
            //    speedOfY = -speedOfY;
            //    playerLabel.Top = 1;
            //}
            //else if (playerLabel.Top > BattleField.Height - playerLabel.Height)
            //{
            //    speedOfY = -speedOfY;
            //    playerLabel.Top = BattleField.Height - playerLabel.Height - 1;
            //}

            //if (playerLabel.Left < 0)
            //{
            //    speedOfX = -speedOfX;
            //    playerLabel.Left = 1;
            //}
            //else if (playerLabel.Left > BattleField.Width - playerLabel.Width)
            //{
            //    speedOfX = -speedOfX;
            //    playerLabel.Left = BattleField.Width - playerLabel.Width - 1;
            //}
            PlayerLabel.Top = PlayerLabel.Top < 0 ? BattleField.Height - PlayerLabel.Height :
                (PlayerLabel.Top > BattleField.Height - PlayerLabel.Height ? 0 : PlayerLabel.Top);
            PlayerLabel.Left = PlayerLabel.Left < 0 ? BattleField.Width - PlayerLabel.Width :
                (PlayerLabel.Left > BattleField.Width - PlayerLabel.Width ? 0 : PlayerLabel.Left);

            OnLocationChanged(new LocationChangedEventArgs(PlayerLabel.Left - x, PlayerLabel.Top - y));
        }

        public void Battle(Player player, ComboBox killOptions, NumericUpDown teamNumber)
        {
            if (!player.IsAlive)
            {
                return;
            }
            switch (teamNumber.Value)
            {
                case 0:
                    break;
                case 1:
                    if (IsPlayer && player.IsPlayer)
                    {
                        return;
                    }
                    break;
                default:
                    if (IsPlayer && player.IsPlayer && Team == player.Team)
                    {
                        return;
                    }
                    break;
            }
            if (Polygon.IsCover(player.Polygon))
            {
                switch (killOptions.SelectedIndex)
                {
                    case 0:
                        player.HitPoint -= 1;
                        break;
                    case 1:
                        if (CombatForceLevel >= player.CombatForceLevel)
                        {
                            player.HitPoint -= Convert.ToInt32(Math.Abs(CombatForceLevel - player.CombatForceLevel) / 2.5);
                        }
                        if (CombatForceLevel <= player.CombatForceLevel)
                        {
                            HitPoint -= Convert.ToInt32(Math.Abs(CombatForceLevel - player.CombatForceLevel) / 2.5);
                        }
                        break;
                    case 2:
                        if (CombatForceLevel > player.CombatForceLevel)
                        {
                            player.HitPoint -= Convert.ToInt32(CombatForceLevel);
                            HitPoint -= Convert.ToInt32(player.CombatForceLevel);
                        }
                        else
                        {
                            player.HitPoint -= Convert.ToInt32(CombatForceLevel / 8);
                            HitPoint -= Convert.ToInt32(player.CombatForceLevel / 8);
                        }
                        break;
                    case 3:
                        if (CombatForceLevel > player.CombatForceLevel)
                        {
                            player.HitPoint -= Convert.ToInt32(Math.Abs(CombatForceLevel - player.CombatForceLevel) / 2.5);
                            //if (player.HitPoint < 510 - Convert.ToInt32(Math.Abs(CombatForceLevel - player.CombatForceLevel) / 2.5))
                            HitPoint += Convert.ToInt32(Math.Abs(CombatForceLevel - player.CombatForceLevel) / 2.5);
                            HitPoint = HitPoint > 510 ? 510 : HitPoint;
                        }
                        else if (CombatForceLevel < player.CombatForceLevel)
                        {
                            HitPoint -= Convert.ToInt32(Math.Abs(player.CombatForceLevel - CombatForceLevel) / 2.5);
                            //if (HitPoint < 510 - Convert.ToInt32(Math.Abs(player.CombatForceLevel - CombatForceLevel) / 2.5))
                            player.HitPoint += Convert.ToInt32(Math.Abs(player.CombatForceLevel - CombatForceLevel) / 2.5);
                            HitPoint = HitPoint > 510 ? 510 : HitPoint;
                        }
                        else
                        {
                            player.HitPoint -= Convert.ToInt32(Math.Abs(CombatForceLevel - player.CombatForceLevel) / 2.5);
                            HitPoint -= Convert.ToInt32(Math.Abs(player.CombatForceLevel - CombatForceLevel) / 2.5);
                        }
                        break;
                    default:
                        break;
                }

                OnHitPointChanged(new HitPointChangedEventArgs(HitPoint, BattleField));
                player.OnHitPointChanged(new HitPointChangedEventArgs(player.HitPoint, BattleField));

                if (HitPoint <= 0 && IsAlive)
                {
                    KilledBy = player;
                }
                else if (player.HitPoint <= 0 && player.IsAlive)
                {
                    player.KilledBy = this;
                }
            }
        }


        //'结算幸存时间、殉职排名、殉职人数、交战得分、被殉职
        public void Settle(int survivalTime)
        {
            if (HitPoint <= 0 && IsAlive) 
            {
                SurvivalTime = survivalTime;
                SurvivalRank = PlayerRemainedNumber;
                KilledBy.KillNumber++;
                KilledBy.AttackScore += Convert.ToInt32(300 * Math.Sqrt((double)survivalTime / 300));
                IsAlive = false;
                if (PlayerRemainedNumber != 0 && IsPlayer)
                {
                    PlayerRemainedNumber--;
                }
            }
        }

        public void UpdateColor(GroupBox BattleField)
        {
            if (HitPoint > 510)
            {
                PlayerLabel.BackColor = Color.FromArgb(255, 255, 0);
            }
            else if (HitPoint > 255)
            {
                PlayerLabel.BackColor = Color.FromArgb(510 - HitPoint, 255, 0);
            }
            else if (HitPoint > 0)
            {
                PlayerLabel.BackColor = Color.FromArgb(255, HitPoint, 0);
            }
            else
            {
                BattleField.Controls.Remove(PlayerLabel);
            }
        }

        public void UpdateSpeed()
            {
                Random random = new Random(Guid.NewGuid().GetHashCode());
                SpeedOfX = random.Next(-10, 10);
                SpeedOfY = random.Next(-10, 10);
            }

        public string[] ShowInfo()
        {
            string killedby;
            if (IsPlayer)
            {
                killedby = KilledBy == null ? "winner" : KilledBy.PlayerName;
            }
            else
            {
                killedby = KilledBy == null ? "none" : KilledBy.PlayerName;
            }
            TimeScore = Convert.ToInt32(300 * Math.Sqrt(SurvivalTime / 300));
            Score = TimeScore + AttackScore + Bonus;
            return new string[] { 
                $"{PlayerName}", 
                $"{SurvivalRank.ToString()}",
                $"{SurvivalTime.ToString()}", 
                $"{Score.ToString()}", 
                $"{TimeScore.ToString()}", 
                $"{AttackScore.ToString()}",
                $"{Bonus.ToString()}",
                $"{KillNumber.ToString()}",
                $"{killedby}" 
            };
        }

        #region Interaction
        public void CollapsedMountain(Mountain mountain)
        {
            Random random = new Random(Guid.NewGuid().GetHashCode());
            bool isCollapsed = Polygon.IsCover(mountain.Polygon);
            //playerLabel.Text = $"{PlayerName} {combatForceLevel} {isCollapsed.ToString()}";
            if (isCollapsed)
            {
                int temp = random.Next(2, 4);
                if (temp != 0)
                {
                    // 如果山是水平方向的
                    if (mountain.IsHorizontal)
                    {
                        SpeedOfY = -SpeedOfY;
                    }
                    // 如果山是竖直方向的
                    else if (mountain.IsVertical)
                    {
                        SpeedOfX = -SpeedOfX;
                    }
                    // 如果山是斜向的
                    else
                    {
                        Vector oldSpeed = new Vector(SpeedOfX, SpeedOfY);
                        if ((mountain.EndPoint.X - mountain.StartPoint.X) * SpeedOfX + (mountain.StartPoint.Y - mountain.EndPoint.Y) * SpeedOfY <= 0)
                        {
                            mountain.Normal = -mountain.Normal;
                        }
                        Vector newSpeed = oldSpeed - 2 * (oldSpeed * mountain.Normal) * mountain.Normal;
                        SpeedOfX = Convert.ToInt32(newSpeed.X);
                        SpeedOfY = Convert.ToInt32(newSpeed.Y);
                    }
                }
            }

        }

        public void CollapsedRiver(River river)
        {
            bool isCollapsed = river.IsCollapsed(this);
            //playerLabel.Text = $"{PlayerName} {combatForceLevel} {isCollapsed.ToString()}";

            if (isCollapsed)
            {
                PlayerLabel.Top +=  (river.EndPoint.Y - river.StartPoint.Y) * Math.Sign(SpeedOfY);
                PlayerLabel.Left += (river.EndPoint.X - river.StartPoint.X) * Math.Sign(SpeedOfX);
                // 引起位置变化的地方都要触发LocationChanged事件
                LocationChangedEventArgs e = new LocationChangedEventArgs(
                    (river.EndPoint.X - river.StartPoint.X) * Math.Sign(SpeedOfX),
                    (river.EndPoint.Y - river.StartPoint.Y) * Math.Sign(SpeedOfY));
                OnLocationChanged(e);
            }
        }

        public void CollapseClinic(Clinic clinic)
        {
            if (clinic.IsCollapsed(this))
            {
                if (PlayerRemainedNumber / (8 * Clinic.NumberOfClinic) > 1)
                {
                    if (HitPoint < 509)
                        HitPoint++;
                }
                else
                {
                    if (HitPoint < 510 - PlayerRemainedNumber / (8 * Clinic.NumberOfClinic))
                        HitPoint += PlayerRemainedNumber / (8 * Clinic.NumberOfClinic);
                }
                OnHitPointChanged(new HitPointChangedEventArgs(HitPoint, BattleField));
            }
        }

        public void CollapsedPit(Pit pit, Pit[] pits)
        {
            if (pit.IsCollapsed(this))
            {
                Random random = new Random(Guid.NewGuid().GetHashCode());
                double x = PlayerLabel.Left;
                double y = PlayerLabel.Top;

                int temp;
                do
                {
                    temp = random.Next(pits.Length);
                } while (temp == pit.Number);
                int temp1 = 1 + pit.Width / (Convert.ToInt32(Speed) == 0 ? 1 : Convert.ToInt32(Speed));
                PlayerLabel.Left = pits[temp].Left + 1 / 2 * pits[temp].Width - 1 / 2 * PlayerLabel.Width + temp1 * SpeedOfY;
                PlayerLabel.Top = pits[temp].Top + 1 / 2 * pits[temp].Height - 1 / 2 * PlayerLabel.Height + temp1 * SpeedOfX;
                Bonus--; // 掉坑减分
                
                // 引起位置变化的地方都要触发LocationChanged事件
                OnLocationChanged(new LocationChangedEventArgs(PlayerLabel.Left - x, PlayerLabel.Top - y));
            }
        }
        #endregion

        #region IComparable
        public int CompareTo(Player player)
        {
            if (player == null) throw new ArgumentNullException("other");
            // compare to BookNo
            int result = SurvivalRank.CompareTo(player.SurvivalRank);
            if (!IsPlayer)
            {
                return 1;
            }
            else if (!player.IsPlayer)
            {
                return -1;
            }
            else
            {
                return result;
            }
        }
        #endregion
    }
    public class Hat : Player
    {
        public int CountDown { get; set; }
        public Player PlayerKilledHat { get; set; }
        public Timer TimerCountDown { get; set; }
        public override void OnHitPointChanged(HitPointChangedEventArgs e)
        {
            base.OnHitPointChanged(e);
        }

        public Hat(int combatForceOption, GroupBox BattleField) : base(0, combatForceOption, BattleField, 0)
        {
            CountDown = 60;
            PlayerKilledHat = null;
            switch (combatForceOption)
            {
                case 0:
                case 1:
                    CombatForceLevel = 5;
                    break;
                case 2:
                    CombatForceLevel = 5.5;
                    break;
                default:
                    break;
            }
            PlayerName = "草帽大叔";
            PlayerLabel.Text = $"{PlayerName} {CombatForceLevel.ToString()}";
            PlayerLabel.BackColor = Color.Green;
            TimerCountDown = new Timer
            {
                Interval = 1000,
                Enabled = false
            };
            TimerCountDown.Tick += new EventHandler(TimerCountDown_Tick);
            Team = -1;
        }


        private void TimerCountDown_Tick(object sender, EventArgs e)
        {
            CountDown -= 1;
            // determine whether playerKilledHat is null
            if (CountDown > 0 && PlayerKilledHat != null)
            {
                PlayerKilledHat.CombatForceLevel = 12;
                PlayerKilledHat.HitPoint = 510;
                OnHitPointChanged(new HitPointChangedEventArgs(PlayerKilledHat.HitPoint, BattleField));
            }
            else
            {
                TimerCountDown.Enabled = false;
                CountDown = 60;
                PlayerKilledHat = null;
            }
        }


        public new void Settle(int survivalTime)
        {
            if (HitPoint <= 0 && IsAlive)
            {
                SurvivalTime = survivalTime;
                KilledBy.Bonus += 500;
                IsAlive = false;
                KilledBy.HitPoint = 510;
                KilledBy.CombatForceLevel = 12;
                PlayerKilledHat = KilledBy;
                TimerCountDown.Enabled = true;
            }
        }
    }

    public class Elf : Player
    {
        public int PeriodOfRecharge { get; set; }
        public Player PlayerProtectedByElf { get; set; }
        public Timer TimerOfProtection { get; set; }
        public Timer TimerOfRecharge { get; set; }
        public override void OnHitPointChanged(HitPointChangedEventArgs e)
        {
            base.OnHitPointChanged(e);
        }

        public Elf(GroupBox BattleField) : base(BattleField)
        {
            PeriodOfRecharge = 60;
            PlayerProtectedByElf = null;
            CombatForceLevel = 3.5;
            PlayerName = "精灵";
            PlayerLabel.Text = $"{PlayerName} {CombatForceLevel.ToString()}";
            PlayerLabel.BackColor = Color.Green;
            TimerOfProtection = new System.Windows.Forms.Timer
            {
                Interval = 1000,
                Enabled = false
            };
            TimerOfProtection.Tick += new EventHandler(Protection);
            TimerOfRecharge = new Timer
            {
                Interval = 60000,
                Enabled = false
            };
            TimerOfRecharge.Tick += Recharge;
        }



        private void Recharge(object sender, EventArgs e)
        {
            HitPoint = 510;
            OnHitPointChanged(new HitPointChangedEventArgs(HitPoint, BattleField));
        }

        private void Protection(object sender, EventArgs e)
        {
            PeriodOfRecharge -= 1;
            if (PeriodOfRecharge == 0)
            {
                Random random = new Random(Guid.NewGuid().GetHashCode());
                TimerOfProtection.Enabled = false;
                PeriodOfRecharge = 60;
                PlayerLabel.Visible = true;
                double x = PlayerLabel.Left;
                double y = PlayerLabel.Top;
                PlayerLabel.Left = 0;
                PlayerLabel.Top = random.Next();
                OnLocationChanged(new LocationChangedEventArgs(PlayerLabel.Left - x, PlayerLabel.Top - y));
                HitPoint = 510;
                SpeedOfX = random.Next(-10, 10);
                SpeedOfY = random.Next(-10, 10);
                IsAlive = true;
                PlayerProtectedByElf = null;
                OnHitPointChanged(new HitPointChangedEventArgs(HitPoint, BattleField));
            }
            else
            {
                if (PlayerProtectedByElf != null)
                {
                    PlayerProtectedByElf.HitPoint = 510;
                    PlayerProtectedByElf.OnHitPointChanged(
                        new HitPointChangedEventArgs(PlayerProtectedByElf.HitPoint, BattleField));
                }
            }
        }


        public new void Settle(int survivalTime)
        {
            if (HitPoint <= 0 && IsAlive)
            {
                SurvivalTime = survivalTime;
                KilledBy.Bonus += 200;
                IsAlive = false;
                KilledBy.HitPoint = 510;
                PlayerProtectedByElf = KilledBy;
                TimerOfProtection.Enabled = true;
                TimerOfRecharge.Enabled = false;
            }
        }
    }

    public class Egg : Player
    {
        public int TimeGettingIntoEarth { get; set; }
        public Timer GettingIntoEarth { get; set; }
        public bool IsInEarth { get; set; }
        private delegate void GetIntoEarthCallback();
        public override void OnHitPointChanged(HitPointChangedEventArgs e)
        {
            base.OnHitPointChanged(e);
        }

        public Egg(GroupBox BattleField) : base(BattleField)
        {
            HitPoint = 100;
            PlayerLabel.BackColor = Color.PaleVioletRed;
            CombatForceLevel = 2;
            PlayerName = "弱蛋";
            PlayerLabel.Text = $"{PlayerName} {CombatForceLevel.ToString()}";
            GettingIntoEarth = new System.Windows.Forms.Timer
            {
                Interval = 2000,
                Enabled = false
            };
            GettingIntoEarth.Tick += Timer_Tick;
            IsInEarth = false;
        }

        public void GetIntoEarth(Player player)
        {
            if (Polygon.IsCover(player.Polygon))
            {
                HitPoint -= Convert.ToInt32(player.CombatForceLevel / 2);
                OnHitPointChanged(new HitPointChangedEventArgs(HitPoint, BattleField));
                if (HitPoint <= 0 && IsAlive)
                {
                    KilledBy = player;
                }
                else if (player.HitPoint <= 0 && player.IsAlive)
                {
                    player.KilledBy = this;
                }
                PlayerLabel.Visible = false;
                IsAlive = false;
                IsInEarth = true;
                GettingIntoEarth.Enabled = true;
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            PlayerLabel.Visible = true;
            IsAlive = true;
            IsInEarth = false;
            GettingIntoEarth.Enabled = false;
        }

        public new void Settle(int survivalTime)
        {
            if (HitPoint <= 0 && IsAlive)
            {
                SurvivalTime = survivalTime;
                KilledBy.Bonus += 200;
                IsAlive = false;
            }
        }
    }

    public class Proprieter : Player
    {
        public int FistProprieter { get; set; }
        public int[] FingerGuessState { get; set; }
        public int[] FingerGuessWin { get; set; }
        public bool[] IsStatemate { get; set; }
        public override void OnHitPointChanged(HitPointChangedEventArgs e)
        {
            base.OnHitPointChanged(e);
        }

        public Proprieter(GroupBox BattleField) : base(BattleField)
        {
            Random random = new Random(Guid.NewGuid().GetHashCode());
            PlayerLabel.BackColor = Color.Green;
            CombatForceLevel = 999;
            PlayerName = "社长";
            PlayerLabel.Text = $"{PlayerName} {CombatForceLevel.ToString()}";
            FingerGuessState = new int[PlayerNumber];
            IsStatemate = new bool[PlayerNumber];
            FingerGuessWin = new int[PlayerNumber];

            FistProprieter = random.Next() % 3;
            for (int i = 0; i < PlayerNumber; ++i)
            {
                int temp = random.Next(14);
                if (temp == 0)
                {
                    switch (FistProprieter)
                    {
                        case 0: 
                            FingerGuessState[i] = 2; 
                            break;

                        case 1:
                            FingerGuessState[i] = 0; 
                            break;

                        case 2: 
                            FingerGuessState[i] = 1; 
                            break;

                        default: 
                            break;
                    }
                }
                else if (temp < 4)
                {
                    FingerGuessState[i] = FistProprieter;
                }
                else
                {
                    switch (FistProprieter)
                    {
                        case 0: 
                            FingerGuessState[i] = 1; 
                            break;

                        case 1: 
                            FingerGuessState[i] = 2;
                            break;

                        case 2:
                            FingerGuessState[i] = 0;
                            break;

                        default:
                            break;
                    }
                }
            }
        }

        public void FingerGame(List<Player> players)
        {
            Random random = new Random(Guid.NewGuid().GetHashCode());

            FistProprieter = random.Next(3);
            PlayerLabel.Text = $"{PlayerName} {CombatForceLevel.ToString()} " +
                $"{(FistProprieter == 0 ? "剪刀" : FistProprieter == 1 ? "石头" : "布")}";
            for (int i = 0; i < PlayerNumber;++i)
            {
                if (!players[i].IsAlive)
                {
                    continue;
                }
                int order = players[i].CreatingOrder;

                int temp = random.Next(14);
                if (temp == 0)
                {
                    switch (FistProprieter)
                    {
                        case 0:
                            FingerGuessState[order] = 2;
                            break;
                        case 1:
                            FingerGuessState[order] = 0;
                            break;
                        case 2:
                            FingerGuessState[order] = 1;
                            break;
                        default:
                            break;
                    }
                }
                else if (temp < 4)
                {
                    FingerGuessState[order] = FistProprieter;
                }
                else
                {
                    switch (FistProprieter)
                    {
                        case 0:
                            FingerGuessState[order] = 1;
                            break;
                        case 1:
                            FingerGuessState[order] = 2;
                            break;
                        case 2:
                            FingerGuessState[order] = 0;
                            break;
                        default:
                            break;
                    }
                }

                //if ((Math.Abs(players[i].CenterLeft - CenterLeft) < 10) && (Math.Abs(players[i].CenterTop - CenterTop) < 10))
                if (Polygon.IsCover(players[i].Polygon))
                {
                    IsStatemate[order] = true;
                    players[i].PlayerLabel.Text = $"{players[i].PlayerName} {players[i].CombatForceLevel.ToString()} " +
                        $"{(FingerGuessState[order] == 0 ? "剪刀" : FingerGuessState[order] == 1 ? "石头" : "布")}";
                } 
                else
                {
                    players[i].PlayerLabel.Text = $"{players[i].PlayerName} {players[i].CombatForceLevel.ToString()} ";
                    IsStatemate[order] = false;
                }

                if ((FistProprieter == 0 && FingerGuessState[order] == 1)
                 || (FistProprieter == 1 && FingerGuessState[order] == 2)
                 || (FistProprieter == 2 && FingerGuessState[order] == 0))
                {
                    FingerGuessWin[order] = -1;
                }
                else if ((FistProprieter == 0 && FingerGuessState[order] == 2)
                      || (FistProprieter == 1 && FingerGuessState[order] == 0)
                      || (FistProprieter == 2 && FingerGuessState[order] == 1))
                {
                    FingerGuessWin[order] = 1;
                }
                else 
                {
                    FingerGuessWin[order] = 0;
                }

                if (IsStatemate[order])
                {
                    if (FingerGuessWin[order] == -1)
                    {
                        HitPoint -= 38;
                        OnHitPointChanged(new HitPointChangedEventArgs(HitPoint, BattleField));
                    }
                    if (FingerGuessWin[order] == 1)
                    {
                        HitPoint = 510;
                        players[i].HitPoint = 0;
                        players[i].OnHitPointChanged(new HitPointChangedEventArgs(players[i].HitPoint, BattleField));
                    }
                    IsStatemate[order] = false;
                }


                if (HitPoint <= 0 && IsAlive)
                {
                    KilledBy = players[i];
                    return;
                }
                else if (players[i].HitPoint <= 0 && players[i].IsAlive)
                {
                    players[i].KilledBy = this;
                }
            }
        }

        public new void Settle(int survivalTime)
        {
            if (HitPoint <= 0 && IsAlive)
            {
                SurvivalTime = survivalTime;
                KilledBy.Bonus += 200;
                IsAlive = false;
            }
        }
    }

    public class Ozone : Player
    {
        public Ozone(int combatForceOption, GroupBox BattleField) : base(0, combatForceOption, BattleField, 0)
        {
            Random random = new Random(Guid.NewGuid().GetHashCode());
            switch (combatForceOption)
            {
                case 0:
                case 1:
                    CombatForceLevel = random.Next(6) + 0.5;
                    break;
                case 2:
                    CombatForceLevel = random.Next(6) + 1;
                    break;
                default:
                    break;
            }
            PlayerLabel.BackColor = Color.Green;
            PlayerName = "臭氧加速器";
            PlayerLabel.Text = $"{PlayerName} {CombatForceLevel.ToString()}";
            Team = -1;
        }

        public override void OnHitPointChanged(HitPointChangedEventArgs e)
        {
            base.OnHitPointChanged(e);
        }

        public new void UpdateSpeed()
        {
            Random random = new Random(Guid.NewGuid().GetHashCode());
            SpeedOfX = 2 * random.Next(-10, 10);
            SpeedOfY = 2 * random.Next(-10, 10);
        }

        public void UpdateBattleForceLevel(ComboBox combatForceOptions)
        {
            Random random = new Random(Guid.NewGuid().GetHashCode());

            CombatForceLevel = random.Next(6) + combatForceOptions.SelectedIndex == 2 ? 1 : 0.5; //'战力相同
            PlayerLabel.Text = $"{PlayerName} {CombatForceLevel.ToString()}";
        }

        public new void Settle(int survivalTime)
        {
            if (HitPoint <= 0 && IsAlive)
            {
                SurvivalTime = survivalTime;
                KilledBy.Bonus += 200;
                IsAlive = false;
            }
        }

        public void Radius(Player player)
        {
            double length = Math.Sqrt(Math.Pow(player.PlayerLabel.Left - PlayerLabel.Left, 2) + Math.Pow(player.PlayerLabel.Top - PlayerLabel.Top, 2));
            player.HitPoint -= Convert.ToInt32((double)HitPoint / 200 * Math.Exp(-length / 50));
            player.OnHitPointChanged(new HitPointChangedEventArgs(player.HitPoint, BattleField));

            if (player.HitPoint <= 0 && player.IsAlive)
            {
                player.KilledBy = this;
            }
        }
    }

    // Represents the surface on which the shapes are drawn
    // Subscribes to shape events so that it knows
    // when to redraw a shape.
    public class PlayerGroup
    {
        public PlayerGroup()
        {
            PlayerList = new List<Player>();
        }

        public List<Player> PlayerList { get; set; }
        public int Count { get => PlayerList.Count; }
        public void AddPlayer(Player player)
        {
            PlayerList.Add(player);
            // Subscribe to the base class event.
            player.HitPointChanged += UpdateLabel;
            player.HitPointChanged += HandleHitPointChanged;
            player.LocationChanged += HandleLocationChanged;
            player.CombatForceLevelChanged += UpdateLabel;
        }

        // ...Other methods to draw, resize, etc.
        private void UpdateLabel(object sender, HitPointChangedEventArgs e)
        {
            Player player = (Player)sender;
            player.PlayerLabel.Text = $"{player.Team.ToString()} {player.PlayerName} {player.CombatForceLevel.ToString()} {e.HitPoint.ToString()}";
        }

        private void HandleHitPointChanged(object sender, HitPointChangedEventArgs e)
        {
            Player player = (Player)sender;
            if (e.HitPoint > 510)
            {
                player.PlayerLabel.BackColor = Color.FromArgb(0, 255, 0);
            }
            else if (e.HitPoint > 255)
            {
                player.PlayerLabel.BackColor = Color.FromArgb(510 - (int)e.HitPoint, 255, 0);
            }
            else if (e.HitPoint > 0)
            {
                player.PlayerLabel.BackColor = Color.FromArgb(255, (int)e.HitPoint, 0);
            }
            else
            {
                e.Battlefield.Controls.Remove(player.PlayerLabel);
            }
        }

        private void HandleLocationChanged(object sender, LocationChangedEventArgs e)
        {
            Player player = (Player)sender;
            player.Polygon.Move(e.DisplacementX, e.DisplacementY);
        }

        private void UpdateLabel(object sender, CombatForceLevelChangedEventArgs e)
        {
            Player player = (Player)sender;
            player.PlayerLabel.Text = $"{player.Team.ToString()} {player.PlayerName} {e.ConbatForceLevel.ToString()} {player.HitPoint.ToString()}";
        }

        private void Settle(object sender, HitPointChangedEventArgs e)
        {
            Player player = (Player)sender;
            //player.Settle();
        }

        public void UpdateBattleForceLevel(ComboBox combatForceOptions)
        {
            Random random = new Random(Guid.NewGuid().GetHashCode());

            switch (combatForceOptions.SelectedIndex)
            {
                case 0://'战力相同随机
                    foreach (var p in PlayerList)
                    {
                        if (!p.IsPlayer)
                        {
                            continue;
                        }
                        p.CombatForceLevel = random.Next(1, 6);
                        p.CombatForceLevel *= random.Next(5) == 0 ? 2 : 1;
                    }
                    break;

                case 1:
                    int quarter = Player.PlayerNumber / 4;
                    double[] combatForceLevelCache = new double[quarter];
                    foreach (var p in PlayerList)
                    {
                        if (!p.IsPlayer)
                        {
                            continue;
                        }
                        if (p.CreatingOrder < quarter)
                        {
                            combatForceLevelCache[p.CreatingOrder] = PlayerList[p.CreatingOrder].CombatForceLevel;
                            p.CombatForceLevel = PlayerList[p.CreatingOrder + quarter].CombatForceLevel;
                        }
                        else if (p.CreatingOrder < Player.PlayerNumber - quarter)
                        {
                            p.CombatForceLevel = PlayerList[p.CreatingOrder + quarter].CombatForceLevel;
                        }
                        else
                        {
                            p.CombatForceLevel = combatForceLevelCache[quarter - Player.PlayerNumber + p.CreatingOrder];
                        }
                    }
                    break;

                case 2:
                    foreach (var p in PlayerList)
                    {
                        if (!p.IsPlayer)
                        {
                            continue;
                        }
                        double temp1 = random.NextDouble();
                        double temp2 = random.NextDouble();
                        p.CombatForceLevel = random.Next(1, 6);
                        if (p.CombatForceLevel == 1 && temp1 > p.CreatingOrder / Player.PlayerNumber)
                        {
                            p.CombatForceLevel = (temp2 < 1 / (5 + 5 * p.CreatingOrder / (Player.PlayerNumber - 1))) ? 12 : 6;
                        }
                        else
                        {
                            p.CombatForceLevel *= (temp2 < 1 / (5 + 5 * p.CreatingOrder / (Player.PlayerNumber - 1))) ? 2 : 1;
                        }
                    }
                    break;
            }
        }
    }
}
