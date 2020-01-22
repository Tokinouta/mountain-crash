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
    public delegate void LocationChangedEventHandler(Player player, LocationChangedEventArgs e);

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

    public class Player : IComparable<Player>
    {
        static int playerNumber;
        static int playerRemainedNumber;
        static double[] combatForceLevelCache;

        //sx、sy为玩家速度方向，lf为玩家横坐标排序，hp为玩家生命主，lv为玩家战力等级,alive为判断是否幸存标志
        double combatForceLevel;
        int speedOfX;
        int speedOfY;
        int horizontalOrder;
        int hitPoint;
        bool isAlive;
        int creatingOrder;

        //score为玩家得分,time为幸存时间,kill为殉职人数,ranka幸存排名,rankb为总分排名,scoret是时间得分,
        //scorek为交战得分,bonus为加分
        double survivalTime;
        int score;
        int killNumber;
        int survivalRank;
        int scoreRank;
        int timeScore;
        int attackScore;
        int bonus;

        string playerName;
        string killedBy;

        Label playerLabel;
        Polygon polygon;

        public Player(GroupBox BattleField)
        {
            Random random = new Random(Guid.NewGuid().GetHashCode());
            speedOfX = random.Next(-10, 10);
            speedOfY = random.Next(-10, 10);
            combatForceLevel = 0;
            horizontalOrder = 0;
            creatingOrder = 0;
            hitPoint = 510;
            isAlive = true;

            survivalTime = 0;
            score = 0;
            killNumber = 0;
            survivalRank = 0;
            scoreRank = 0;
            timeScore = 0;
            attackScore = 0;
            bonus = 0;
            playerLabel = new Label
            {
                Height = 10,
                Width = 100,
                Top = random.Next(BattleField.Height - 10),
                Left = random.Next(BattleField.Width - 100),
                BackColor = Color.Aquamarine
            };
            BattleField.Controls.Add(playerLabel);
            Vector[] vectors = new Vector[] { new Vector(playerLabel.Left, playerLabel.Top),
                                              new Vector(playerLabel.Left, playerLabel.Bottom),
                                              new Vector(playerLabel.Right, playerLabel.Bottom),
                                              new Vector(playerLabel.Right, playerLabel.Top) };
            polygon = new Polygon(vectors);
        }

        public Player(int creatingOrder, int combatForceOption, GroupBox BattleField)
        {
            Random random = new Random(Guid.NewGuid().GetHashCode());
            speedOfX = random.Next(-10, 10);
            speedOfY = random.Next(-10, 10);
            horizontalOrder = creatingOrder;
            this.creatingOrder = creatingOrder;
            hitPoint = 510;
            isAlive = true;
            switch (combatForceOption)
            {
                case 0:
                    int temp = random.Next(5);
                    combatForceLevel = temp == 0 ?
                        (random.Next(1, 7) * 2) :
                        (random.Next(1, 7) * 1);
                    break;
                case 1:
                    combatForceLevel =
                        Convert.ToInt32(5 * 10 / PlayerNumber * (PlayerNumber - (creatingOrder + 1))) / 10 + 1;
                    break;
                case 2:
                    double tempd = random.NextDouble(), temp1 = random.NextDouble();
                    combatForceLevel = random.Next(1, 6);
                    if (tempd < 1 / (5 + 5 * (creatingOrder + 1) / (PlayerNumber - 1)))
                    {
                        if (combatForceLevel == 1 && temp1 > ((creatingOrder + 1) + 1) / PlayerNumber)
                        {
                            combatForceLevel = 12;
                        }
                        else
                        {
                            combatForceLevel *= 2;
                        }
                    }
                    else
                    {
                        if (combatForceLevel == 1 && temp1 > ((creatingOrder + 1) + 1) / PlayerNumber)
                        {
                            combatForceLevel = 6;
                        }
                    }
                    break;
                default: break;
            }

            survivalTime = 0;
            score = 0;
            killNumber = 0;
            survivalRank = 0;
            scoreRank = creatingOrder;
            timeScore = 0;
            attackScore = 0;
            bonus = 0;
            playerLabel = new Label
            {
                Height = 10,
                Width = 100,
                Top = random.Next(BattleField.Height - 10),
                Left = random.Next(BattleField.Width - 100),
                BackColor = Color.Aquamarine
            };
            BattleField.Controls.Add(playerLabel);
            Vector[] vectors = new Vector[] { new Vector(playerLabel.Left, playerLabel.Top),
                                              new Vector(playerLabel.Left, playerLabel.Bottom),
                                              new Vector(playerLabel.Right, playerLabel.Bottom),
                                              new Vector(playerLabel.Right, playerLabel.Top) };
            polygon = new Polygon(vectors);
        }

        private void Times()
        {
            speedOfX *= 10;
            speedOfY *= 10;
        }

        public event LocationChangedEventHandler LocationChanged;

        #region Property
        public static int PlayerNumber { get => playerNumber; set => playerNumber = value; }
        public static int PlayerRemainedNumber { get => playerRemainedNumber; set => playerRemainedNumber = value; }
        public static double[] CombatForceLevelCache { get => combatForceLevelCache; set => combatForceLevelCache = value; }

        public int SpeedOfX { get => speedOfX; set => speedOfX = value; }
        public int SpeedOfY { get => speedOfY; set => speedOfY = value; }
        public int HorizontalOrder { get => horizontalOrder; set => horizontalOrder = value; }
        public int HitPoint { get => hitPoint; set => hitPoint = value; }
        public double CombatForceLevel { get => combatForceLevel; set => combatForceLevel = value; }
        public bool IsAlive { get => isAlive; set => isAlive = value; }

        public int Score { get => score; set => score = value; }
        public int KillNumber { get => killNumber; set => killNumber = value; }
        public int SurvivalRank { get => survivalRank; set => survivalRank = value; }
        public int ScoreRank { get => scoreRank; set => scoreRank = value; }
        public int TimeScore { get => timeScore; set => timeScore = value; }
        public int AttackScore { get => attackScore; set => attackScore = value; }
        public int Bonus { get => bonus; set => bonus = value; }
        public double SurvivalTime { get => survivalTime; set => survivalTime = value; }
        public string PlayerName { get => playerName; set => playerName = value; }
        public string KilledBy { get => killedBy; set => killedBy = value; }
        public double Speed { get => Math.Sqrt(Math.Pow(SpeedOfX, 2) + Math.Pow(SpeedOfY, 2)); }
        public int CreatingOrder { get => creatingOrder; set => creatingOrder = value; }

        public Label PlayerLabel { get => playerLabel; set => playerLabel = value; }
        public Polygon Polygon { get => polygon; set => polygon = value; }
        public int CenterLeft { get => playerLabel.Left + playerLabel.Width / 2; }
        public int CenterTop { get => playerLabel.Top + playerLabel.Height / 2; }
        #endregion

        public void Move(GroupBox BattleField)
        {
            double x = playerLabel.Left;
            double y = playerLabel.Top;
            playerLabel.Top += Convert.ToInt32(SpeedOfY);
            playerLabel.Top = playerLabel.Top < 0 ? BattleField.Height - playerLabel.Height :
                (playerLabel.Top > BattleField.Height - playerLabel.Height ? 0 : playerLabel.Top);
            playerLabel.Left += Convert.ToInt32(SpeedOfX);
            playerLabel.Left = playerLabel.Left < 0 ? BattleField.Width - playerLabel.Width :
                (playerLabel.Left > BattleField.Width - playerLabel.Width ? 0 : playerLabel.Left);
            LocationChangedEventArgs e = 
                new LocationChangedEventArgs(playerLabel.Left - x, playerLabel.Top - y);
            LocationChanged(this, e);
        }

        public void Battle(Player player, ComboBox killOptions)
        {
            if (!player.IsAlive)
            {
                return;
            }
            if (polygon.IsCover(player.polygon))
            {
                if (CombatForceLevel > player.CombatForceLevel)
                {
                    switch (killOptions.SelectedIndex)
                    {
                        case 0:
                            player.HitPoint -= 1;
                            break;
                        case 1:
                            player.HitPoint -= Convert.ToInt32(Math.Abs(CombatForceLevel - player.CombatForceLevel) / 2.5);
                            break;
                        case 2:
                            player.HitPoint -= Convert.ToInt32(CombatForceLevel / 8);
                            HitPoint -= Convert.ToInt32(player.CombatForceLevel / 8);
                            break;
                        case 3:
                            player.HitPoint -= Convert.ToInt32(Math.Abs(CombatForceLevel - player.CombatForceLevel) / 2.5);
                            if (player.HitPoint < 510 - Convert.ToInt32(Math.Abs(CombatForceLevel - player.CombatForceLevel) / 2.5))
                                HitPoint += Convert.ToInt32(Math.Abs(CombatForceLevel - player.CombatForceLevel) / 2.5);
                            break;
                        default:
                            break;
                    }
                }
                if (CombatForceLevel < player.CombatForceLevel)
                {
                    switch (killOptions.SelectedIndex)
                    {
                        case 0:
                            HitPoint -= 1;
                            break;
                        case 1:
                            player.HitPoint -= Convert.ToInt32(Math.Abs(player.CombatForceLevel - CombatForceLevel) / 2.5);
                            break;
                        case 2:
                            player.HitPoint -= Convert.ToInt32(CombatForceLevel / 8);
                            HitPoint -= Convert.ToInt32(player.CombatForceLevel / 8);
                            break;
                        case 3:
                            HitPoint -= Convert.ToInt32(Math.Abs(player.CombatForceLevel - CombatForceLevel) / 2.5);
                            if (HitPoint < 510 - Convert.ToInt32(Math.Abs(player.CombatForceLevel - CombatForceLevel) / 2.5))
                                player.HitPoint += Convert.ToInt32(Math.Abs(player.CombatForceLevel - CombatForceLevel) / 2.5);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        //'结算幸存时间、殉职排名、殉职人数、交战得分、被殉职
        public void Settle(Player player, int survivalTime)
        {
            if (HitPoint <= 0 && IsAlive) 
            {
                SurvivalTime = survivalTime;
                SurvivalRank = PlayerRemainedNumber;
                player.KillNumber++;
                player.AttackScore += Convert.ToInt32(300 * Math.Sqrt(1 / 300 * survivalTime));
                KilledBy = player.PlayerName;
                IsAlive = false;
            }
            else if (player.HitPoint <= 0 && player.IsAlive)
            {
                player.SurvivalTime = survivalTime;
                player.SurvivalRank = PlayerRemainedNumber;
                KillNumber++;
                attackScore += Convert.ToInt32(300 * Math.Sqrt(1 / 300 * survivalTime));
                player.KilledBy = PlayerName;
                player.IsAlive = false;
            }
        }

        public void UpdateColor(GroupBox BattleField)
        {
            if (HitPoint > 255)
            {
                PlayerLabel.BackColor = Color.FromArgb(510 - HitPoint, 255, 0);
            }
            else if (HitPoint <= 255 && HitPoint > 0)
            {
                PlayerLabel.BackColor = Color.FromArgb(510 - HitPoint, 255, 0);
            }
            else
            {
                BattleField.Controls.Remove(PlayerLabel);
                IsAlive = false;
                PlayerRemainedNumber -= 1;
            }
        }

        public void UpdateSpeed()
        {
            Random random = new Random(Guid.NewGuid().GetHashCode());
            speedOfX = random.Next(-10, 10);
            speedOfY = random.Next(-10, 10);
        }

        public void UpdateLabel()
        {
            playerLabel.Text = $"{playerName} {combatForceLevel.ToString()}";
        }

        public void UpdateBattleForceLevel(ComboBox combatForceOptions, Player[] players)
        {
            Random random = new Random(Guid.NewGuid().GetHashCode());

            switch (combatForceOptions.SelectedIndex)//'战力相同随机
            {
                case 0:
                    combatForceLevel = random.Next(1, 6);
                    combatForceLevel *= random.Next(5) == 0 ? 2 : 1;
                    break;

                case 1:
                    int quarter = combatForceLevelCache.Length / 4;
                    if (creatingOrder < quarter)
                    {
                        combatForceLevelCache[creatingOrder] = players[creatingOrder].CombatForceLevel;
                        combatForceLevel = players[creatingOrder + quarter].CombatForceLevel;
                    }
                    else if (creatingOrder < players.Length - quarter)
                    {
                        combatForceLevel = players[creatingOrder + quarter].CombatForceLevel;
                    }
                    else
                    {
                        combatForceLevel = combatForceLevelCache[quarter - combatForceLevelCache.Length + creatingOrder];
                    }
                    break;

                case 2:
                    double temp1 = random.NextDouble();
                    double temp2 = random.NextDouble();
                    combatForceLevel = random.Next(1, 6);
                    if (combatForceLevel == 1 && temp1 > creatingOrder / playerNumber)
                    {
                        combatForceLevel = (temp2 < 1 / (5 + 5 * creatingOrder / (playerNumber - 1))) ? 12 : 6;
                    }
                    else
                    {
                        combatForceLevel *= (temp2 < 1 / (5 + 5 * creatingOrder / (playerNumber - 1))) ? 2 : 1;
                    }
                    break;
            }
        }

        #region Interaction
        public void CollapsedMountain(Mountain mountain)
        {
            Random random = new Random(Guid.NewGuid().GetHashCode());
            bool f = polygon.IsCover(mountain.Polygon);
            playerLabel.Text = $"{PlayerName} {combatForceLevel} {f.ToString()}";
            if (f)
            {
                //MessageBox.Show("collapsed");
                int temp = random.Next(2, 4);
                if (temp != 0)
                {
                    // 如果山是水平方向的
                    if (mountain.IsHorizontal)
                    {
                        speedOfY = -speedOfY;
                    }
                    // 如果山是竖直方向的
                    else if (mountain.IsVertical)
                    {
                        speedOfX = -speedOfX;
                    }
                    // 如果山是斜向的
                    else
                    {
                        Vector oldSpeed = new Vector(speedOfX, speedOfY);
                        if ((mountain.EndPoint.X - mountain.StartPoint.X) * speedOfX + (mountain.StartPoint.Y - mountain.EndPoint.Y) * speedOfY <= 0)
                        {
                            mountain.Normal = -mountain.Normal;
                        }
                        Vector newSpeed = oldSpeed - 2 * (oldSpeed * mountain.Normal) * mountain.Normal;
                        speedOfX = Convert.ToInt32(newSpeed.X);
                        speedOfY = Convert.ToInt32(newSpeed.Y);
                        //double tempx = speedOfX, tempy = speedOfY;
                        //if ((mountain.EndPoint.X - mountain.StartPoint.X) * speedOfX + (mountain.StartPoint.Y - mountain.EndPoint.Y) * speedOfY <= 0)
                        //{
                        //    speedOfX = Convert.ToInt32(tempx - 2 * (mountain.EndPoint.X - mountain.StartPoint.X)
                        //                                        * ((mountain.StartPoint.Y - mountain.EndPoint.Y) * tempy + (mountain.EndPoint.X - mountain.StartPoint.X) * tempx)
                        //                                        / Math.Pow(mountain.Length, 2));
                        //    speedOfY = Convert.ToInt32(tempy - 2 * (mountain.StartPoint.Y - mountain.EndPoint.Y)
                        //                                        * ((mountain.StartPoint.Y - mountain.EndPoint.Y) * tempy + (mountain.EndPoint.X - mountain.StartPoint.X) * tempx)
                        //                                        / Math.Pow(mountain.Length, 2));
                        //}
                        //else
                        //{
                        //    speedOfX = Convert.ToInt32(tempx - 2 * (mountain.StartPoint.X - mountain.EndPoint.X)
                        //                                        * ((mountain.EndPoint.Y - mountain.StartPoint.Y) * tempy + (mountain.StartPoint.X - mountain.EndPoint.X) * tempx)
                        //                                        / Math.Pow(mountain.Length, 2));
                        //    speedOfY = Convert.ToInt32(tempy - 2 * (mountain.EndPoint.Y - mountain.StartPoint.Y)
                        //                                        * ((mountain.EndPoint.Y - mountain.StartPoint.Y) * tempy + (mountain.StartPoint.X - mountain.EndPoint.X) * tempx)
                        //                                        / Math.Pow(mountain.Length, 2));
                        //}
                    }
                }
                // 如果山是水平方向的
                //if (mountain.IsHorizontal)
                //{
                //    if (PlayerLabel.Right > mountain.StartPoint.X && PlayerLabel.Left < mountain.EndPoint.X)
                //    {
                //        if (SpeedOfY > 0)
                //        {
                //            int temp = random.Next(4);
                //            if ((PlayerLabel.Bottom - mountain.StartPoint.Y) * (PlayerLabel.Bottom - mountain.EndPoint.Y - SpeedOfY) <= 0 && temp != 0)
                //            {
                //                SpeedOfY = -SpeedOfY;
                //            }
                //        }
                //        else
                //        {
                //            int temp = random.Next(4);
                //            if ((PlayerLabel.Top - mountain.StartPoint.Y) * (PlayerLabel.Top - mountain.EndPoint.Y - SpeedOfY) <= 0 && temp != 0)
                //            {
                //                SpeedOfY = -SpeedOfY;
                //            }
                //        }
                //    }
                //}
                // 如果山是竖直方向的
                //else if (mountain.IsVertical)
                //{
                //    if (PlayerLabel.Bottom > mountain.StartPoint.Y && PlayerLabel.Top < mountain.EndPoint.Y)
                //    {
                //        if (SpeedOfX > 0)
                //        {
                //            int temp = random.Next(4);
                //            if ((PlayerLabel.Right - mountain.StartPoint.X) * (PlayerLabel.Right - mountain.EndPoint.X - SpeedOfX) <= 0 && temp != 0)
                //            {
                //                SpeedOfX = -SpeedOfX;
                //            }
                //        }
                //        else
                //        {
                //            int temp = random.Next(4);
                //            if ((PlayerLabel.Left - mountain.StartPoint.X) * (PlayerLabel.Left - mountain.EndPoint.X - SpeedOfX) <= 0 && temp != 0)
                //            {
                //                SpeedOfX = -SpeedOfX;
                //            }
                //        }
                //    }
                //}
                // 如果山是斜向的并且玩家进入山的外接矩形中
                //else if (mountain.Closure.IsCollapsed(this))
                //{}
                //return ((a.X - StartPoint.X) * (EndPoint.Y - StartPoint.Y) == (a.Y - StartPoint.Y) * (EndPoint.X - StartPoint.X));
            }

        }

        public void CollapsedRiver(River river)
        {
            int Y = 1 / 2 * playerLabel.Width + playerLabel.Left;
            int X = 1 / 2 * playerLabel.Height + playerLabel.Top;
            int aa = river.EndPoint.X - river.StartPoint.X;
            int bb = river.StartPoint.Y - river.EndPoint.Y;
            int cc = river.StartPoint.X * (river.EndPoint.Y - river.StartPoint.Y) - river.StartPoint.Y * (river.EndPoint.X - river.StartPoint.X);
            if (Math.Abs(aa * X + bb * Y + cc) / Math.Sqrt(aa ^ 2 + bb ^ 2) <= 100 &&
                (X - river.StartPoint.Y) * (X - river.EndPoint.Y) <= 0 &&
                (Y - river.StartPoint.X) * (Y - river.EndPoint.X) <= 0)
            {
                playerLabel.Top += 200 / Convert.ToInt32(Math.Sqrt(aa ^ 2 + bb ^ 2) * (-bb));
                playerLabel.Left += 200 / Convert.ToInt32(Math.Sqrt(aa ^ 2 + bb ^ 2) * aa);
            }
        }

        public void CollapseClinic(Clinic clinic)
        {
            if (clinic.IsCollapsed(this))
            {
                if (playerRemainedNumber / (8 * Clinic.NumberOfClinic) > 1)
                {
                    if (hitPoint < 509)
                        hitPoint++;
                }
                else
                {
                    if (hitPoint < 510 - playerRemainedNumber / (8 * Clinic.NumberOfClinic))
                        hitPoint += playerRemainedNumber / (8 * Clinic.NumberOfClinic);
                }
            }
        }

        public void CollapsedPit(Pit pit, Pit[] pits)
        {
            if (pit.IsCollapsed(this))
            {
                Random random = new Random(Guid.NewGuid().GetHashCode());
                int temp;
                do
                {
                    temp = random.Next(pits.Length);
                } while (temp == pit.Number);
                int temp1 = 1 + pit.Width / (Convert.ToInt32(Speed) == 0 ? 1 : Convert.ToInt32(Speed));
                playerLabel.Left = pits[temp].Left + 1 / 2 * pits[temp].Width - 1 / 2 * playerLabel.Width + temp1 * speedOfY;
                playerLabel.Top = pits[temp].Top + 1 / 2 * pits[temp].Height - 1 / 2 * playerLabel.Height + temp1 * speedOfX;
                bonus--; // 掉坑减分

            }
        }
        #endregion

        #region IComparable
        public int CompareTo(Player player)
        {
            if (player == null) throw new ArgumentNullException("other");
            // compare to BookNo
            return this.PlayerLabel.Left.CompareTo(player.PlayerLabel.Left);
        }
        #endregion
    }
    public class Hat : Player
    {
        // 草帽大叔特权倒计时
        int countDown;
        // 殉职草帽大叔的玩家
        Player playerKilledHat;
        Timer timerCountDown;

        public Hat(GroupBox BattleField) : base(BattleField)
        {
            countDown = 60;
            playerKilledHat = null;
            PlayerLabel.BackColor = Color.Green;
            PlayerName = "草帽大叔";
        }

        public Hat(int combatForceOption, GroupBox BattleField) : base(0, combatForceOption, BattleField)
        {
            countDown = 60;
            playerKilledHat = null;
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
            timerCountDown = new Timer
            {
                Interval = 1000,
                Enabled = false
            };
            timerCountDown.Tick += new EventHandler(timerCountDown_Tick);
        }

        private void timerCountDown_Tick(object sender, EventArgs e)
        {
            countDown = countDown - 1;
            // determine whether playerKilledHat is null
            if (countDown > 0 && playerKilledHat != null)
            {
                playerKilledHat.CombatForceLevel = 12;
                playerKilledHat.HitPoint = 510;
            }
            else
            {
                timerCountDown.Enabled = false;
                countDown = 60;
                playerKilledHat = null;
            }
        }

        public int CountDown { get => countDown; set => countDown = value; }
        public Player PlayerKilledHat { get => playerKilledHat; set => playerKilledHat = value; }
        public Timer TimerCountDown { get => timerCountDown; set => timerCountDown = value; }

        public new void Settle(Player player, int survivalTime)
        {
            if (player.HitPoint <= 0 && player.IsAlive)
            {
                player.SurvivalTime = survivalTime;
                player.SurvivalRank = PlayerRemainedNumber;
                KillNumber++;
                player.KilledBy = PlayerName;
                player.IsAlive = false;
            }
            else if (HitPoint <= 0 && IsAlive)
            {
                SurvivalTime = survivalTime;
                player.Bonus += 500;
                KilledBy = player.PlayerName;
                IsAlive = false;
                player.HitPoint = 510;
                player.CombatForceLevel = 12;
                playerKilledHat = player;
                timerCountDown.Enabled = true;
            }
        }
    }

    public class Elf : Player
    {
        // 精灵回血周期
        int periodOfRecharge;
        // 精灵保护的玩家
        Player playerProtectedByElf;
        Timer timerOfRecharge;

        public Elf(GroupBox BattleField) : base(BattleField)
        {
            periodOfRecharge = 60;
            playerProtectedByElf = null;
            CombatForceLevel = 3.5;
            PlayerName = "精灵";
            PlayerLabel.Text = $"{PlayerName} {CombatForceLevel.ToString()}";
            PlayerLabel.BackColor = Color.Green;
            timerOfRecharge = new Timer
            {
                Interval = 1000,
                Enabled = false
            };
            timerOfRecharge.Tick += new EventHandler(TimerOfRecharge_Tick);
        }

        private void TimerOfRecharge_Tick(object sender, EventArgs e)
        {
            periodOfRecharge -= 1;
            if (periodOfRecharge == 0)
            {
                Random random = new Random(Guid.NewGuid().GetHashCode());
                timerOfRecharge.Enabled = false;
                periodOfRecharge = 60;
                PlayerLabel.Visible = true;
                PlayerLabel.Left = 0;
                PlayerLabel.Top = random.Next();
                HitPoint = 510;
                SpeedOfX = random.Next(-10, 10);
                SpeedOfY = random.Next(-10, 10);
                IsAlive = true;
                playerProtectedByElf = null;
            }
            else
            {
                if (playerProtectedByElf != null)
                {
                    playerProtectedByElf.HitPoint = 510;
                }
            }
        }

        public int PeriodOfRecharge { get => periodOfRecharge; set => periodOfRecharge = value; }
        public Player PlayerProtectedByElf { get => playerProtectedByElf; set => playerProtectedByElf = value; }
        public Timer TimerOfRecharge { get => timerOfRecharge; set => timerOfRecharge = value; }

        public new void Settle(Player player, int survivalTime)
        {
            if (player.HitPoint <= 0 && player.IsAlive)
            {
                player.SurvivalTime = survivalTime;
                player.SurvivalRank = PlayerRemainedNumber;
                KillNumber++;
                player.KilledBy = PlayerName;
                player.IsAlive = false;
            }
            else if (HitPoint <= 0 && IsAlive)
            {
                SurvivalTime = survivalTime;
                player.Bonus += 200;
                KilledBy = player.PlayerName;
                IsAlive = false;
                player.HitPoint = 510;
                playerProtectedByElf = player;
                timerOfRecharge.Enabled = true;
            }
        }

        public void Recharge()
        {
            var t = Task.Run(async delegate
            {
                await Task.Delay(2000);
                return 42;
            });
            t.Wait();
        }


    }

    public class Egg : Player
    {
        // 弱蛋遁地时刻
        int timeGettingIntoEarth;

        public Egg(GroupBox BattleField) : base(BattleField)
        {
            HitPoint = 100;
            PlayerLabel.BackColor = Color.PaleVioletRed;
            CombatForceLevel = 2;
            PlayerName = "弱蛋";
            PlayerLabel.Text = $"{PlayerName} {CombatForceLevel.ToString()}";
        }

        public int TimeGettingIntoEarth { get => timeGettingIntoEarth; set => timeGettingIntoEarth = value; }

        public void GetIntoEarth(Player player)
        {
            if (Polygon.IsCover(player.Polygon))
            {
                PlayerLabel.Enabled = false;
                var t = Task.Run(async delegate
                {
                    await Task.Delay(2000);
                    return 42;
                });
                t.Wait();
                HitPoint -= Convert.ToInt32(player.CombatForceLevel / 2);
                PlayerLabel.Enabled = true;
            }
        }

        public new void Settle(Player player, int survivalTime)
        {
            if (player.HitPoint <= 0 && player.IsAlive)
            {
                player.SurvivalTime = survivalTime;
                player.SurvivalRank = PlayerRemainedNumber;
                KillNumber++;
                player.KilledBy = PlayerName;
                player.IsAlive = false;
            }
            else if (HitPoint <= 0 && IsAlive)
            {
                SurvivalTime = survivalTime;
                player.Bonus += 200;
                KilledBy = player.PlayerName;
                IsAlive = false;
            }
        }
    }

    public class Proprieter : Player
    {
        //fist0为社长划拳状态
        int fistProprieter;
        //fist为玩家划拳状态,game为是否进行划拳
        int[] fingerGuessState;
        // 1 for proprieter won, -1 for player won, 0 for tie
        int[] fingerGuessWin;
        bool[] isStatemate;

        public Proprieter(GroupBox BattleField) : base(BattleField)
        {
            Random random = new Random(Guid.NewGuid().GetHashCode());
            PlayerLabel.BackColor = Color.Green;
            CombatForceLevel = 999;
            PlayerName = "社长";
            PlayerLabel.Text = $"{PlayerName} {CombatForceLevel.ToString()}";
            fingerGuessState = new int[PlayerNumber];
            isStatemate = new bool[PlayerNumber];
            fingerGuessWin = new int[PlayerNumber];

            fistProprieter = random.Next() % 3;
            for (int i = 0; i < PlayerNumber; ++i)
            {
                int temp = random.Next(14);
                if (temp == 0)
                {
                    switch (fistProprieter)
                    {
                        case 0: 
                            fingerGuessState[i] = 2; 
                            break;

                        case 1:
                            fingerGuessState[i] = 0; 
                            break;

                        case 2: 
                            fingerGuessState[i] = 1; 
                            break;

                        default: 
                            break;
                    }
                }
                else if (temp < 4)
                {
                    fingerGuessState[i] = fistProprieter;
                }
                else
                {
                    switch (fistProprieter)
                    {
                        case 0: 
                            fingerGuessState[i] = 1; 
                            break;

                        case 1: 
                            fingerGuessState[i] = 2;
                            break;

                        case 2:
                            fingerGuessState[i] = 0;
                            break;

                        default:
                            break;
                    }
                }
            }
        }

        public int FistProprieter { get => fistProprieter; set => fistProprieter = value; }
        public int[] FingerGuessState { get => fingerGuessState; set => fingerGuessState = value; }
        public int[] FingerGuessWin { get => fingerGuessWin; set => fingerGuessWin = value; }
        public bool[] IsStatemate { get => isStatemate; set => isStatemate = value; }

        public void FingerGame(Player[] players)
        {
            Random random = new Random(Guid.NewGuid().GetHashCode());

            fistProprieter = random.Next(3);
            PlayerLabel.Text = $"{PlayerName} {CombatForceLevel} " +
                $"{(fistProprieter == 0 ? "剪刀" : fistProprieter == 1 ? "石头" : "布")}";
            for (int i = 0; i < players.Length;++i)
            {
                int order = players[i].CreatingOrder;

                int temp = random.Next(14);
                if (temp == 0)
                {
                    switch (fistProprieter)
                    {
                        case 0:
                            fingerGuessState[order] = 2;
                            break;
                        case 1:
                            fingerGuessState[order] = 0;
                            break;
                        case 2:
                            fingerGuessState[order] = 1;
                            break;
                        default:
                            break;
                    }
                }
                else if (temp < 4)
                {
                    fingerGuessState[order] = fistProprieter;
                }
                else
                {
                    switch (fistProprieter)
                    {
                        case 0:
                            fingerGuessState[order] = 1;
                            break;
                        case 1:
                            fingerGuessState[order] = 2;
                            break;
                        case 2:
                            fingerGuessState[order] = 0;
                            break;
                        default:
                            break;
                    }
                }

                if ((Math.Abs(players[i].CenterLeft - CenterLeft) < 10) && (Math.Abs(players[i].CenterTop - CenterTop) < 10))
                {
                    PlayerLabel.Text = $"{players[i].PlayerName} {players[i].CombatForceLevel} " +
                        $"{(fingerGuessState[order] == 0 ? "剪刀" : fingerGuessState[order] == 1 ? "石头" : "布")}";
                } 
                else
                {
                    PlayerLabel.Text = $"{players[i].PlayerName} {players[i].CombatForceLevel} ";
                    isStatemate[order] = false;
                }

                if ((fistProprieter == 0 && fingerGuessState[order] == 1)
                 || (fistProprieter == 1 && fingerGuessState[order] == 2)
                 || (fistProprieter == 2 && fingerGuessState[order] == 0))
                {
                    fingerGuessWin[order] = -1;
                }
                else if ((fistProprieter == 0 && fingerGuessState[order] == 2)
                      || (fistProprieter == 1 && fingerGuessState[order] == 0)
                      || (fistProprieter == 2 && fingerGuessState[order] == 1))
                {
                    fingerGuessWin[order] = 1;
                }
                else 
                {
                    fingerGuessWin[order] = 0;
                }
            }
        }

        public new void Settle(Player player, int survivalTime)
        {
            int order = player.CreatingOrder;

            if (!isStatemate[order])
            {
                if (fingerGuessWin[order] == -1)
                {
                    HitPoint -= 38;
                }
                if (fingerGuessWin[order] == 1)
                {
                    player.HitPoint = 0;
                    HitPoint = 510;
                    player.SurvivalTime = survivalTime;
                    player.SurvivalRank = PlayerRemainedNumber;
                    player.KilledBy = PlayerName;
                    player.IsAlive = false;
                }
                isStatemate[order] = true;
            }
            else if (HitPoint <= 0 && IsAlive)
            {
                SurvivalTime = survivalTime;
                player.Bonus += 200;
                KilledBy = player.PlayerName;
                IsAlive = false;
            }
        }

    }

    public class Ozone : Player
    {
        public Ozone(GroupBox BattleField) : base(BattleField)
        {
            PlayerLabel.BackColor = Color.Green;
            PlayerName = "臭氧加速器";
            PlayerLabel.Text = $"{PlayerName} {CombatForceLevel.ToString()}";
        }
        public Ozone(int combatForceOption, GroupBox BattleField) : base(0, combatForceOption, BattleField)
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
        }

        public new void SpeedChange()
        {
            Random random = new Random(Guid.NewGuid().GetHashCode());
            SpeedOfX = 2 * random.Next(-10, 10);
            SpeedOfY = 2 * random.Next(-10, 10);
        }

        public void UpdateBattleForceLevel(ComboBox combatForceOptions)
        {
            Random random = new Random(Guid.NewGuid().GetHashCode());

            CombatForceLevel = random.Next(6) + combatForceOptions.SelectedIndex == 2 ? 1 : 0.5; //'战力相同
            PlayerLabel.Text = $"臭氧加速器 {CombatForceLevel.ToString()}";
        }

        public new void Settle(Player player, int survivalTime)
        {
            if (player.HitPoint <= 0 && player.IsAlive)
            {
                player.SurvivalTime = survivalTime;
                player.SurvivalRank = PlayerRemainedNumber;
                KillNumber++;
                player.KilledBy = PlayerName;
                player.IsAlive = false;
            }
            else if (HitPoint <= 0 && IsAlive)
            {
                SurvivalTime = survivalTime;
                player.Bonus += 200;
                KilledBy = player.PlayerName;
                IsAlive = false;
            }
        }

        public void Radius(Player player, int survivalTime)
        {
            double length = Math.Sqrt(Math.Pow(player.PlayerLabel.Left - PlayerLabel.Left, 2) + Math.Pow(player.PlayerLabel.Top - PlayerLabel.Top, 2));
            player.HitPoint -= Convert.ToInt32(HitPoint / 6 * Math.Exp(-length / 1000));

            // 再次结算
            if (player.HitPoint <= 0 && player.IsAlive)
            {
                player.SurvivalTime = survivalTime;
                player.SurvivalRank = PlayerRemainedNumber;
                KillNumber++;
                player.KilledBy = PlayerName;
                player.IsAlive = false;
            }
        }

    }
}
