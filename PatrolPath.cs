using System;
using System.Collections.Generic;
using System.Globalization;

struct Point : IComparable<Point>
{
    public double X;
    public double Y;

    public Point(double x, double y) { X = x; Y = y; }

    public int CompareTo(Point other)
    {
        int cmp = X.CompareTo(other.X);
        return cmp != 0 ? cmp : Y.CompareTo(other.Y);
    }
}

// ---------- Convex Hull (Andrew's Monotone Chain) 
static class ConvexHullBuilder
{
    static double Cross(Point o, Point a, Point b)
    {
        return (a.X - o.X) * (b.Y - o.Y)
             - (a.Y - o.Y) * (b.X - o.X);
    }

    static double Distance(Point a, Point b)
    {
        double dx = a.X - b.X;
        double dy = a.Y - b.Y;
        return Math.Sqrt(dx * dx + dy * dy);
    }

    public static Point[] Build(List<Point> points)
    {
        int m = points.Count;
        if (m == 0) return new Point[0];
        if (m == 1) return new Point[] { points[0] };

        Point[] sorted = points.ToArray();
        Array.Sort(sorted);  

        Point[] hull = new Point[2 * m];
        int k = 0;

        for (int i = 0; i < m; i++)
        {
            while (k >= 2 && Cross(hull[k - 2], hull[k - 1], sorted[i]) < 0)
                k--;
            hull[k++] = sorted[i];
        }

        for (int i = m - 2, t = k + 1; i >= 0; i--)
        {
            while (k >= t && Cross(hull[k - 2], hull[k - 1], sorted[i]) < 0)
                k--;
            hull[k++] = sorted[i];
        }

        Point[] result = new Point[k - 1];
        Array.Copy(hull, result, k - 1);
        return result;
    }

    public static double Perimeter(Point[] hull)
    {
        int n = hull.Length;
        if (n < 2) return 0.0;

        double perimeter = 0.0;
        for (int i = 0; i < n; i++)
            perimeter += Distance(hull[i], hull[(i + 1) % n]);
        return perimeter;
    }
}

class Program
{
    static void Main()
    {
        CultureInfo inv = CultureInfo.InvariantCulture;

        int n = int.Parse(Console.ReadLine().Trim(), inv);

        List<Point> points = new List<Point>(4 * n);

        for (int i = 0; i < n; i++)
        {
            string[] parts = Console.ReadLine().Trim().Split(
                new char[] { ' ', '\t' },
                StringSplitOptions.RemoveEmptyEntries);

            double x1 = double.Parse(parts[0], NumberStyles.Float, inv);
            double y1 = double.Parse(parts[1], NumberStyles.Float, inv);
            double x2 = double.Parse(parts[2], NumberStyles.Float, inv);
            double y2 = double.Parse(parts[3], NumberStyles.Float, inv);

            double minX = Math.Min(x1, x2), maxX = Math.Max(x1, x2);
            double minY = Math.Min(y1, y2), maxY = Math.Max(y1, y2);

            points.Add(new Point(minX, minY));
            points.Add(new Point(minX, maxY));
            points.Add(new Point(maxX, minY));
            points.Add(new Point(maxX, maxY));
        }

        Point[] hull = ConvexHullBuilder.Build(points);
        double answer = ConvexHullBuilder.Perimeter(hull);

        Console.WriteLine(answer.ToString("G9", inv));
    }
}
