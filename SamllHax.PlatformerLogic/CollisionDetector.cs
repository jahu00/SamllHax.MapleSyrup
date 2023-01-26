using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace SamllHax.PlatformerLogic
{
    public class CollisionDetector
    {
        public T GetPlatformBelow<T>(IEnumerable<T> wallsAndPlatforms, IPoint point, out float? platformY, out bool isLastPlatform) where T : class, ILineSegment
        {
            return GetPlatformBelow(wallsAndPlatforms: wallsAndPlatforms, x: point.X, y: point.Y, platformY: out platformY, isLastPlatform: out isLastPlatform);
        }

        public T GetPlatformBelow<T>(IEnumerable<T> wallsAndPlatforms, float x, float y, out float? platformY, out bool isLastPlatform) where T : class, ILineSegment
        {
            var platfromsBelow = wallsAndPlatforms.Where(wallOrPlatform => wallOrPlatform.Type != LineType.Vertical && wallOrPlatform.ContainsHorizontally(x)).Select(platform => new { Platform = platform, Y = platform.GetY(x) }).Where(pair => pair.Y > y).OrderBy(pair => pair.Y).ToArray();
            isLastPlatform = platfromsBelow.Count() == 1;
            var platformBelow = platfromsBelow.FirstOrDefault();
            platformY = platformBelow?.Y;
            return platformBelow?.Platform;
        }

        public bool WillCollideWithWall<T>(IEnumerable<T> wallsAndPlatforms, IPoint point, float nextX, out T wall) where T : class, ILineSegment
        {
            return WillCollideWithWall(wallsAndPlatforms: wallsAndPlatforms, x: point.X, y: point.Y, nextX: nextX, wall: out wall);
        }

        public bool WillCollideWithWall<T>(IEnumerable<T> wallsAndPlatforms, float x, float y, float nextX, out T wall) where T : class, ILineSegment
        {
            var minX = Math.Floor(Math.Min(x, nextX));
            var maxX = Math.Ceiling(Math.Max(x, nextX));
            wall = wallsAndPlatforms.Where(wallOrPlatform => wallOrPlatform.Type == LineType.Vertical && wallOrPlatform.ContainsVertically(y) && minX < wallOrPlatform.X1 && maxX > wallOrPlatform.X1).FirstOrDefault();
            return wall != null;
        }

        public Point LineIntersectPoint(ILineSegment line1, ILineSegment line2)
        {
            if (line1.Type == line2.Type)
            {
                return null;
            }
            if (line1.A == line2.A)
            {
                return null;
            }
            if (line1.Type == LineType.Vertical)
            {
                if (line2.Type == LineType.Horizontal)
                {
                    return new Point(line1.X1, line2.Y1);
                }
                else
                {
                    return new Point(line1.X1, line2.GetY(line1.X1));
                }
            }
            if (line2.Type == LineType.Vertical)
            {
                if (line1.Type == LineType.Horizontal)
                {
                    return new Point(line2.X1, line1.Y1);
                }
                else
                {
                    return new Point(line2.X1, line2.GetY(line2.X1));
                }
            }
            var x = (line1.B - line2.B) / (line2.A - line1.A);
            var y = line1.GetY(x);
            return new Point(x, y);
        }

        public Point LineSegmentIntersectPoint(LineSegment line1, LineSegment line2)
        {
            var point = LineIntersectPoint(line1, line2);
            if (point == null)
            {
                return null;
            }
            if (line1.ContainsPoint(point))
            {
                return null;
            }
            if (!line2.ContainsPoint(point))
            {
                return null;
            }
            return point;
        }
    }
}
