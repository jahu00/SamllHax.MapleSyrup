using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Logic = SamllHax.MapleSyrup.Logic;
using SamllHax.MapleSyrup.Draw;
using SamllHax.PlatformerLogic;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamllHax.MapleSyrup.Components
{
    public class PlayerInstance : ComponentBase, IPhysicsObject, IDrawable, IUpdatable
    {
        const int footholdWidth = 5;

        private CommonData _commonData;
        private CollisionDetector _collisionDetector;

        //private readonly Game _game;

        public PhysicsState PhysicsState { get; private set; } = PhysicsState.Airborn;

        public bool IsOnRail => PhysicsState == PhysicsState.OnPlatform;
        public bool IsAirborn => PhysicsState == PhysicsState.Airborn;
        public bool IsClimbing => PhysicsState == PhysicsState.Climb;
        public bool IsFalling => IsAirborn && SpeedY.Value > 0;

        public Speed SpeedX { get; } = new Speed();
        public Speed SpeedY { get; } = new Speed();

        public IDrawable Sprite { get; set; }
        public MapInstance MapInstance { get; set; }

        public Logic.Foothold LastPlatform { get; set; }

        public PlayerInstance(CommonData commonData, CollisionDetector collisionDetector)
        {
            _commonData = commonData;
            _collisionDetector = collisionDetector;
        }

        public void Draw(SKCanvas canvas, SKMatrix matrix)
        {
            Sprite.Draw(canvas, this.GetTransformMatrix(matrix));
        }


        public void OnUpdate(UpdateEvents events)
        {
            var footholdBelow = _collisionDetector.GetPlatformBelow(MapInstance.Footholds, this, out var _, out var isLastFoothold, tolerance: footholdWidth);
            if (IsAirborn && footholdBelow != null && LastPlatform != footholdBelow)
            {
                LastPlatform = footholdBelow;
            }

            if (IsOnRail)
            {
                SpeedY.Stop();
            }
            if (IsClimbing)
            {
                SpeedX.Stop();
            }
            if (IsAirborn)
            {
                this.ApplyGravity(events.Delta, _commonData.Physics.GravityAcc, _commonData.Physics.FallSpeed);
            }

            var isWalking = false;
            if (events.KeyboardState.IsKeyDown(Keys.Left))
            {
                ScaleX = 1;
                isWalking = true;
                this.MoveHorizontally(events.Delta, _commonData.Physics.WalkDrag - _commonData.Physics.WalkForce, _commonData.Physics.WalkSpeed);
            }

            if (events.KeyboardState.IsKeyDown(Keys.Right))
            {
                ScaleX = -1;
                isWalking = true;
                this.MoveHorizontally(events.Delta, _commonData.Physics.WalkForce - _commonData.Physics.WalkDrag, _commonData.Physics.WalkSpeed);
            }

            if (!isWalking && !SpeedX.IsStopped)
            {
                var drag = _commonData.Physics.WalkDrag * SpeedX.OppositeDirection;
                this.MoveHorizontally(events.Delta, drag, 0);
            }


            if (IsOnRail && events.KeyboardState.IsKeyPressed(Keys.Down))
            {
                if (footholdBelow != null && LastPlatform.Data.ForbidFallDown != true && !isLastFoothold)
                {
                    Y += footholdWidth + 1;
                    PhysicsState = PhysicsState.Airborn;
                    return;
                }
            }

            if (IsOnRail && events.KeyboardState.IsKeyPressed(Keys.Up))
            {
                this.Jump(_commonData.Physics.JumpSpeed);
                PhysicsState = PhysicsState.Airborn;
            }
           
            this.GetNewPosition(events.Delta, out var newX, out var newY);
            if (IsOnRail)
            {
                if (newX < LastPlatform.X1)
                {
                    if (LastPlatform.Previous == null)
                    {
                        PhysicsState = PhysicsState.Airborn;
                    } else if (LastPlatform.Previous?.Type != LineType.Vertical) //Continuation
                    {
                        LastPlatform = LastPlatform.Previous;
                        newX = LastPlatform.X2;
                        newY = LastPlatform.Y2;
                    } else if (LastPlatform.Previous?.Y1 < LastPlatform.Y1) // Wall
                    {
                        newX = LastPlatform.X1;
                        newY = LastPlatform.Y1;
                        SpeedX.Stop();
                    } else // Cliff
                    {
                        newX = LastPlatform.Previous.X1 - 1;
                        PhysicsState = PhysicsState.Airborn;
                    }
                } else if (newX > LastPlatform.X2)
                {
                    if (LastPlatform.Next == null)
                    {
                        PhysicsState = PhysicsState.Airborn;
                    }
                    else if (LastPlatform.Next?.Type != LineType.Vertical) //Continuation
                    {
                        LastPlatform = LastPlatform.Next;
                        newX = LastPlatform.X1;
                        newY = LastPlatform.Y1;
                    }
                    else if (LastPlatform.Next?.Y2 < LastPlatform.Y2) // Wall
                    {
                        newX = LastPlatform.X2;
                        newY = LastPlatform.Y2;
                        SpeedX.Stop();
                    }
                    else // Cliff
                    {
                        newX = LastPlatform.Next.X1 + 1;
                        PhysicsState = PhysicsState.Airborn;
                    }
                } else
                {
                    newY = (int)LastPlatform.GetY(newX);
                }
            }
            else
            {
                if (LastPlatform != null && IsFalling)
                {
                    var maxY = LastPlatform.GetY(newX);
                    if (newY > maxY)
                    {
                        newY = maxY;
                        PhysicsState = PhysicsState.OnPlatform;
                        Z = LastPlatform.LayerId;
                    }
                }
                if (_collisionDetector.WillCollideWithWall(MapInstance.Walls, this, newX, out var wall))
                {
                    newX = wall.X1 + SpeedX.OppositeDirection;
                    SpeedX.Stop();
                }
            }

            if (newY > MapInstance.MaxY)
            {
                newY = MapInstance.MaxY;
                SpeedY.Stop();
            } else if (newY > MapInstance.MaxY)
            {
                newY = MapInstance.MaxY;
                SpeedY.Stop();
            }
            /*else if (newY < mapBoundingBox.Top)
            {
                newY = mapBoundingBox.Top;
                SpeedY = 0;
            }*/

            if (newX < MapInstance.MinX)
            {
                newX = MapInstance.MinX;
                SpeedX.Stop();
            }
            else if (newX > MapInstance.MaxX)
            {
                newX = MapInstance.MaxX;
                SpeedX.Stop();
            }

            X = (float)newX;
            Y = (float)newY;
        }
    }
}
