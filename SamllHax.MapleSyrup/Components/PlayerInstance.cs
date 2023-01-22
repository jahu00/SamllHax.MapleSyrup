using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SamllHax.MapleSyrup.Draw;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamllHax.MapleSyrup.Components
{
    public class PlayerInstance : ComponentBase, IDrawable, IUpdatable
    {
        private CommonData _commonData;
        //private readonly Game _game;

        public PlayerState State { get; private set; } = PlayerState.Stand;

        public float SpeedX { get; set; }
        public float SpeedY { get; set; }

        public IDrawable Sprite { get; set; }
        public MapInstance MapInstance { get; set; }

        public PlayerInstance(CommonData commonData)//, Game game)
        {
            _commonData = commonData;
            //_game = game;
        }

        public void Draw(SKCanvas canvas, SKMatrix matrix)
        {
            Sprite.Draw(canvas, this.GetTransformMatrix(matrix));
        }

        public void OnUpdate(UpdateEvents events)
        {
            if (events.KeyboardState.IsKeyPressed(Keys.Down))
            {
                Y += 1;
            }
            if (events.KeyboardState.IsKeyPressed(Keys.Up))
            {
                //_mapInstance.Character.Y -= move;
                SpeedY -= _commonData.Physics.JumpSpeed;
            }
            var isWalking = false;
            if (events.KeyboardState.IsKeyDown(Keys.Left))
            {
                ScaleX = 1;
                isWalking = true;
                var maxSpeed = -1 * _commonData.Physics.WalkSpeed;
                if (SpeedX > maxSpeed)
                {
                    SpeedX -= events.Delta * (_commonData.Physics.WalkForce - _commonData.Physics.WalkDrag);
                    if (SpeedX < maxSpeed)
                    {
                        SpeedX = maxSpeed;
                    }
                }
            }
            if (events.KeyboardState.IsKeyDown(Keys.Right))
            {
                ScaleX = -1;
                isWalking = true;
                var maxSpeed = _commonData.Physics.WalkSpeed;
                if (SpeedX < maxSpeed)
                {
                    SpeedX += events.Delta * (_commonData.Physics.WalkForce - _commonData.Physics.WalkDrag);
                    if (SpeedX > maxSpeed)
                    {
                        SpeedX = maxSpeed;
                    }
                }
            }

            if (!isWalking)
            {
                var drag = _commonData.Physics.WalkDrag * events.Delta;
                if (drag > Math.Abs(SpeedX))
                {
                    SpeedX = 0;
                } else
                {
                    SpeedX -= Math.Sign(SpeedX) * drag;
                }
            }

            if (SpeedY < _commonData.Physics.FallSpeed)
            {
                SpeedY += (float)(events.Delta * _commonData.Physics.GravityAcc);
            }

            var mapBoundingBox = MapInstance.GetBoundingBox();
            var newX = X + events.Delta * SpeedX;
            var newY = Y + events.Delta * SpeedY;

            var movementLine = new Line(X, Y, (float)newX, (float)newY);

            //var intersectingFootholds = MapInstance.Footholds.Select(x => new { Foothold = x, IntersectPoint = x.LineSegmentIntersectPoint(movementLine) }).Where(x => x.IntersectPoint != null).OrderBy(x => x.IntersectPoint.Value.Y).ToList();

            foreach(var foothold in MapInstance.Footholds)
            /*foreach(var intersectingFoothold in intersectingFootholds)*/
            {
                var intersectionPoint = foothold.LineSegmentIntersectPoint(movementLine);
                if (intersectionPoint == null)
                {
                    continue;
                }
                intersectionPoint = foothold.LineSegmentIntersectPoint(movementLine);
                if (foothold.IsVertical)
                {
                    if (SpeedX == 0)
                    {
                        continue;
                    }
                    newX = (int)intersectionPoint.Value.X - 1 * Math.Sign(SpeedX);
                    SpeedX = 0;
                    continue;
                }
                if (SpeedY <= 0)
                {
                    continue;
                }
                newY = (int)intersectionPoint.Value.Y - 1;
                SpeedY = 0;
            }

            if (newY > mapBoundingBox.Bottom)
            {
                newY = mapBoundingBox.Bottom;
                SpeedY = 0;
            }
            else if (newY < mapBoundingBox.Top)
            {
                newY = mapBoundingBox.Top;
                SpeedY = 0;
            }

            if (newX < mapBoundingBox.Left)
            {
                newX = mapBoundingBox.Left;
            }
            else if (newX > mapBoundingBox.Right)
            {
                newX = mapBoundingBox.Right;
            }

            X = (float)newX;
            Y = (float)newY;
        }
    }
}
