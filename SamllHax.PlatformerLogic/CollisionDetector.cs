using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace SamllHax.PlatformerLogic
{
    public class CollisionDetector
    {
        public T GetPlatformBelow<T>(IEnumerable<T> platforms, IPoint point, out float? platformY, out bool isLastPlatform, float tolerance = 1) where T : class, ILineSegment
        {
            return GetPlatformBelow(platforms: platforms, x: point.X, y: point.Y, platformY: out platformY, isLastPlatform: out isLastPlatform, tolerance: tolerance);
        }

        public T GetPlatformBelow<T>(IEnumerable<T> platforms, float x, float y, out float? platformY, out bool isLastPlatform, float tolerance = 1) where T : class, ILineSegment
        {
            var offsetY = y - tolerance;
            var platfromsBelow = platforms.Where(testedPlatform => testedPlatform.ContainsHorizontally(x)).Select(platform => new { Platform = platform, Y = platform.GetY(x) }).Where(pair => pair.Y > offsetY).OrderBy(pair => pair.Y).ToArray();
            isLastPlatform = platfromsBelow.Count() == 1;
            var platformBelow = platfromsBelow.FirstOrDefault();
            platformY = platformBelow?.Y;
            return platformBelow?.Platform;
        }

        public bool WillCollideWithWall<T>(IEnumerable<T> walls, IPoint point, float nextX, out T wall, float tolerance = 1) where T : class, ILineSegment
        {
            return WillCollideWithWall(walls: walls, x: point.X, y: point.Y, nextX: nextX, wall: out wall, tolerance: tolerance);
        }

        public bool WillCollideWithWall<T>(IEnumerable<T> walls, float x, float y, float nextX, out T wall, float tolerance = 1) where T : class, ILineSegment
        {
            var minX = Math.Min(x, nextX) - tolerance;
            var maxX = Math.Max(x, nextX) + tolerance;
            wall = walls.Where(testedWall => testedWall.ContainsVertically(y) && minX < testedWall.X1 && maxX > testedWall.X1).FirstOrDefault();
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
