using System;
using System.Drawing;

namespace WindowsFormsApp1
{
    public class Vector
    {
        private double x, y;

        public double X { get => x; set => x = value; }
        public double Y { get => y; set => y = value; }

        public Vector(Point point)
        {
            x = point.X;
            y = point.Y;
        }
        public Vector(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public static double DotProduct(Vector a, Vector b) => a.X * b.X + a.Y * b.Y;

        public static Vector operator +(Vector a) => a;
        public static Vector operator +(Vector a, Vector b) => new Vector(a.X + b.X , a.Y + b.Y);

        public static Vector operator -(Vector a) => new Vector(-a.X, -a.Y);
        public static Vector operator -(Vector a, Vector b) => new Vector(a.X - b.X, a.Y - b.Y);

        public static double operator *(Vector a, Vector b) => a.X * b.X + a.Y * b.Y;
        public static Vector operator *(Vector a, double b) => new Vector(b * a.X, b * a.Y);
        public static Vector operator *(double b, Vector a) => new Vector(b * a.X, b * a.Y);

        public static Vector Normalize(Vector a)
        {
            double magnitude = Math.Sqrt(a.X * a.X + a.Y * a.Y);
            // vector b is only of distance 1 from the origin
            return new Vector(a.X / magnitude, a.Y / magnitude);
        }

        public static Vector Normal(Vector v) => new Vector(v.Y, -v.X);
    }

    //public class Segment
    //{
    //    private Vector p0, p1, dir;
    //    public Segment(Vector start, Vector end)
    //    {
    //        p0 = start;
    //        p1 = end;
    //        dir = end - start;
    //    }

    //    public Vector P0 { get => p0; set => p0 = value; }
    //    public Vector P1 { get => p1; set => p1 = value; }
    //    public Vector Dir { get => dir; set => dir = value; }
    //}
    
    // Assumption: Simply connected => chain vertices together
    public class Polygon 
    {
        private int n;
        private Vector[] vertices;
        //private Segment[] edges;
        private Vector[] edges;

        public Polygon(Vector[] vertex)
        {
            n = vertex.Length;
            edges = new Vector[n];
            vertices = vertex;
            for (int i = 0; i < n - 1; ++i)
            {
                edges[i] = vertex[i] - vertex[i + 1];
            }
            // The last edge is between the first vertex and the last vertex
            edges[n - 1] = vertex[n - 1] - vertex[0]; 
        }

        public void Move(double displacementX, double displacementY)
        {
            for (int i = 0; i < n; ++i)
            {
                vertices[i].X += displacementX;
                vertices[i].Y += displacementY;
            }
        }

        double[] Project(Vector axis)
        {
            axis = Vector.Normalize(axis);

            // min and max are the start and finish points
            double min = Vector.DotProduct(vertices[0], axis);
            double max = min; 
            for (int i = 0; i < n; ++i)
            {
                // find the projection of every point on the polygon onto the line.
                double proj = Vector.DotProduct(vertices[i], axis);
                if (proj < min) min = proj; 
                if (proj > max) max = proj;
            }
            return new double[] { min, max };
        }

        //bool IsContaining(double n, double[] range)
        //{
        //    if (range[1] < range[0])
        //    {
        //        var temp = range[0];
        //        range[0] = range[1];
        //        range[1] = temp; 
        //    }
        //    return (n >= range[0] && n <= range[1]);
        //}

        bool IsOverlap(double[] a_, double[] b_)
        {
            if (a_[1] < a_[0])
            {
                var t = a_[0];
                a_[0] = a_[1];
                a_[1] = t;
            }

            if (b_[1] < b_[0])
            {
                var t = b_[0];
                b_[0] = b_[1];
                b_[1] = t;
            }

            return !(a_[0] > b_[1] || b_[0] > a_[1]);
            //if (IsContaining(a_[0], b_)) return true;
            //if (IsContaining(a_[1], b_)) return true;
            //if (IsContaining(b_[0], a_)) return true;
            //if (IsContaining(b_[1], a_)) return true;
            //return false;
        }

        public bool IsCover(Polygon b)
        {
            for (int i = 0; i < n; i++)
            {
                Vector axis = edges[i]; // Get the direction vector
                axis = Vector.Normal(axis); // Get the normal of the vector (90 degrees)
                double[] a_ = Project(axis), b_ = b.Project(axis); // Find the projection of a and b onto axis
                if (!IsOverlap(a_, b_))
                {
                    return false; // If they do not overlap, then no collision
                }
            }
            
            // repeat for b
            for (int i = 0; i < b.n; i++)
            { 
                Vector axis = b.edges[i];
                axis = Vector.Normal(axis);
                double[] a_ = Project(axis), b_ = b.Project(axis);
                if (!IsOverlap(a_, b_))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
