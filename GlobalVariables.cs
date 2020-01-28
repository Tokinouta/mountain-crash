using System;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public class LinearBarrier
    {
        // 线性物体的起始点和终点
        private Point startPoint;
        private Point endPoint;
        // 是否水平，是否垂直
        private bool isHorizontal;
        private bool isVertical;
        // 外接矩形
        //RectangleBarrier closure;
        // 对角线多边形
        Polygon polygon;
        Vector normal;

        // 通过给定起始点和终点构建物体（点结构）
        public LinearBarrier(Point start,Point end)
        {
            startPoint = start;
            endPoint = end;
            //var Height = Math.Abs(start.Y - end.Y);
            //var Width = Math.Abs(start.X - end.X);
            //var Top = Math.Min(start.Y, end.Y);
            //var Left = Math.Min(start.X, end.X);
            //closure = new RectangleBarrier(Top, Left, Height, Width);

            Vector[] vectors = new Vector[2] { new Vector(start), new Vector(end) };
            polygon = new Polygon(vectors);
            normal = Vector.Normal(vectors[1] - vectors[0]);
            normal = Vector.Normalize(normal);
        }

        // 通过给定起始点和终点构建物体（直接给定坐标）
        public LinearBarrier(int startX, int startY, int endX, int endY)
        {
            startPoint.X = startX;
            startPoint.Y = startY;
            endPoint.X = endX;
            endPoint.Y = endY;
            //var Height = Math.Abs(startY - endY);
            //var Width = Math.Abs(startX - endX);
            //var Top = Math.Min(startY, endY);
            //var Left = Math.Min(startX, endX);
            //closure = new RectangleBarrier(Top, Left, Height, Width);
            Vector[] vectors = new Vector[2] { new Vector(startPoint), new Vector(endPoint) };
            polygon = new Polygon(vectors);
            normal = Vector.Normal(vectors[1] - vectors[0]);
            normal = Vector.Normalize(normal);
        }

        // 通过游戏中关键数字和对战场地构建物体
        public LinearBarrier(int temp, double switchL, GroupBox BattleField)
        {
            Random random = new Random(Guid.NewGuid().GetHashCode());

            // 是否已经产生符合要求的物体
            bool flag;
            // 物体长度
            int controlL;
            do
            {
                flag = false;
                startPoint.X = random.Next(BattleField.Width);
                startPoint.Y = random.Next(BattleField.Height);
                switch (temp)
                {
                    // 产生水平物体
                    case 0:
                        controlL = random.Next(Convert.ToInt32(80 * switchL) + 1) + Convert.ToInt32(20 * switchL);
                        endPoint.Y = StartPoint.Y;
                        endPoint.X = StartPoint.X + controlL;
                        flag = EndPoint.X > BattleField.Right;
                        break;

                    // 产生竖直物体
                    case 1:
                        controlL = random.Next(Convert.ToInt32(80 * switchL) + 1) + Convert.ToInt32(20 * switchL);
                        endPoint.X = StartPoint.X;
                        endPoint.Y = StartPoint.Y + controlL;
                        flag = EndPoint.Y > BattleField.Bottom;
                        break;

                    // 产生斜向物体
                    default:
                        controlL = Convert.ToInt32((50 + random.Next(150)) * Math.Sqrt(5) * switchL + 1);
                        int alpha = random.Next(-89, 89);
                        if (startPoint.X + controlL * Math.Cos(alpha * Math.PI / 180) > BattleField.Width ||
                            startPoint.Y + controlL * Math.Sin(alpha * Math.PI / 180) > BattleField.Height ||
                            startPoint.Y + controlL * Math.Sin(alpha * Math.PI / 180) < 0)
                        {
                            flag = true;
                        }
                        else
                        {
                            endPoint.X = Convert.ToInt32(startPoint.X + controlL * Math.Cos(alpha * Math.PI / 180));
                            endPoint.Y = Convert.ToInt32(startPoint.Y + controlL * Math.Sin(alpha * Math.PI / 180));
                        }
                        break;
                }
                IsHorizontal = StartPoint.Y == EndPoint.Y;
                IsVertical = StartPoint.X == EndPoint.X;
            } while (flag);
            //if (!IsHorizontal && !isVertical)
            //{
            //    var Height = Math.Abs(startPoint.Y - endPoint.Y);
            //    var Width = Math.Abs(startPoint.X - endPoint.X);
            //    var Top = Math.Min(startPoint.Y, endPoint.Y);
            //    var Left = Math.Min(startPoint.X, endPoint.X);
            //    closure = new RectangleBarrier(Top, Left, Height, Width);
            //}
            //else
            //{
            //    closure = null;
            //}
            Vector[] vectors = new Vector[2] { new Vector(startPoint), new Vector(endPoint) };
            polygon = new Polygon(vectors);
            normal = Vector.Normal(vectors[1] - vectors[0]);
            normal = Vector.Normalize(normal);
        }

        public bool IsHorizontal { get => isHorizontal; set => isHorizontal = value; }
        public bool IsVertical { get => isVertical; set => isVertical = value; }
        public Point StartPoint { get => startPoint; set => startPoint = value; }
        public Point EndPoint { get => endPoint; set => endPoint = value; }
        //public RectangleBarrier Closure { get => closure; }
        public Polygon Polygon { get => polygon; }
        public double Length { get => Math.Sqrt(Math.Pow((endPoint.X - startPoint.X), 2) + Math.Pow((endPoint.Y - startPoint.Y), 2)); }
        public Vector Normal { get => normal; set => normal = value; }
    }

    public class Mountain : LinearBarrier
    {
        // 直接继承的构造函数
        public Mountain(Point start, Point end) : base(start, end) { }
        public Mountain(int startX, int startY, int endX, int endY) : base(startX, startY, endX, endY) { }
        public Mountain(int catagory, double switchL, GroupBox BattleField) :
            base(catagory, switchL, BattleField) { }
    }

    public class River : LinearBarrier
    {
        // 直接继承的构造函数
        public River(Point start, Point end) : base(start, end) { }
        public River(int startX, int startY, int endX, int endY) : base(startX, startY, endX, endY) { }
        // 河没有水平或垂直的，所以直接构造斜向
        public River(double switchL, GroupBox BattleField) : base(2, switchL, BattleField) { }

        public bool IsCollapsed(Player player)
        {
            int aa = EndPoint.X - StartPoint.X;
            int bb = StartPoint.Y - EndPoint.Y;
            int cc = StartPoint.X * (EndPoint.Y - StartPoint.Y) - StartPoint.Y * (EndPoint.X - StartPoint.X);
            bool isCollapsed = Math.Abs(aa * player.CenterTop + bb * player.CenterLeft + cc) / Math.Sqrt(Math.Pow(aa, 2) + Math.Pow(bb, 2)) <= 100 &&
                (player.CenterTop - StartPoint.Y) * (player.CenterTop - EndPoint.Y) <= 0 &&
                (player.CenterLeft - StartPoint.X) * (player.CenterLeft - EndPoint.X) <= 0;
            return isCollapsed;
        }
    }

    public class RectangleBarrier
    {
        int top;
        int left;
        int height;
        int width;

        public RectangleBarrier(int topArgument, int leftArgument, int heightArgument, int widthArgument)
        {
            top = topArgument;
            left = leftArgument;
            height = heightArgument;
            width = widthArgument;
        }

        public RectangleBarrier(int heightArgument, int widthArgument, GroupBox BattleField)
        {
            Random random = new Random(Guid.NewGuid().GetHashCode());
            height = heightArgument;
            width = widthArgument;
            left = random.Next(BattleField.Width - Width + 1);
            top = random.Next(BattleField.Height - Height + 1);
        }
        public int Top { get => top; set => top = value; }
        public int Left { get => left; set => left = value; }
        public int Height { get => height; set => height = value; }
        public int Width { get => width; set => width = value; }
        public int Right { get => left + width; }
        public int Bottom { get => top + height; }
        public int CenterLeft { get => left + width / 2; }
        public int CenterTop { get => top + height / 2; }

        public bool IsCollapsed(Player player)
        {
            bool isFromLeft = player.PlayerLabel.Right > Left;
            bool isFromRight = player.PlayerLabel.Left < Left + Width;
            bool isFromTop = player.PlayerLabel.Bottom > Top;
            bool isFromBottom = player.PlayerLabel.Top < Top + Height;
            return isFromLeft && isFromRight && isFromTop && isFromBottom;
        }
    }

    public class Clinic : RectangleBarrier
    {
        static int numberOfClinic;

        public static int NumberOfClinic { get => numberOfClinic; set => numberOfClinic = value; }

        public Clinic(int topArgument, int leftArgument, int heightArgument, int widthArgument)
            : base(topArgument, leftArgument, heightArgument, widthArgument) { }
        public Clinic(int heightArgument, int widthArgument, GroupBox BattleField)
            : base(heightArgument, widthArgument, BattleField) { }

    }

    public class Pit : RectangleBarrier
    {
        static int numberOfPit;
        int number;

        public static int NumberOfPit { get => numberOfPit; set => numberOfPit = value; }
        public int Number { get => number; set => number = value; }

        public Pit(int num, int topArgument, int leftArgument, int heightArgument, int widthArgument)
            : base(topArgument, leftArgument, heightArgument, widthArgument) 
        {
            number = num;
        }
        public Pit(int num, int heightArgument, int widthArgument, GroupBox BattleField)
            : base(heightArgument, widthArgument, BattleField) 
        {
            number = num;
        }

        public new bool IsCollapsed(Player player)
        {
            return ((Math.Abs(player.CenterLeft - CenterLeft) < Width / 2) &&
                    (Math.Abs(player.CenterTop - CenterTop) < Height / 2));
        }
    }

}
