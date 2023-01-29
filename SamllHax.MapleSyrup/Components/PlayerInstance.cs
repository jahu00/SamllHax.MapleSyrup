using Logic = SamllHax.MapleSyrup.Logic;
using SamllHax.MapleSyrup.Draw;
using SamllHax.PlatformerLogic;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SamllHax.MapleSyrup.Logic;

namespace SamllHax.MapleSyrup.Components
{
    public class PlayerInstance : ComponentBase, IPhysicsObject, IDrawable, IUpdatable
    {
        const int footholdWidth = 5;
        const int ladderWidth = 5;

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
        public Logic.Ladder LastLadder { get; set; }

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

            if (IsOnRail || IsAirborn)
            {
                var hasWalked = false;
                if (events.InputEvents.IsDown(InputAction.Left))
                {
                    ScaleX = 1;
                    hasWalked = true;
                    this.MoveHorizontally(events.Delta, _commonData.Physics.WalkDrag - _commonData.Physics.WalkForce, _commonData.Physics.WalkSpeed);
                }

                if (events.InputEvents.IsDown(InputAction.Right))
                {
                    ScaleX = -1;
                    hasWalked = true;
                    this.MoveHorizontally(events.Delta, _commonData.Physics.WalkForce - _commonData.Physics.WalkDrag, _commonData.Physics.WalkSpeed);
                }

                if (!hasWalked && !SpeedX.IsStopped)
                {
                    var drag = _commonData.Physics.WalkDrag * SpeedX.OppositeDirection;
                    this.MoveHorizontally(events.Delta, drag, 0);
                }
            }
            if (IsOnRail)
            {
                if (events.InputEvents.IsDown(InputAction.Up) && _collisionDetector.WillCollideWithWall(MapInstance.Ladders, X, Y - footholdWidth, X, out var ladderUp, tolerance: ladderWidth))
                {
                    PlaceOnLadder(ladderUp);
                    Y = LastLadder.Y2;
                    return;
                }

                if (events.InputEvents.IsDown(InputAction.Down) && _collisionDetector.WillCollideWithWall(MapInstance.Ladders, X, Y + footholdWidth + 1, X, out var ladderDown, tolerance: ladderWidth))
                {
                    PlaceOnLadder(ladderDown);
                    Y = LastLadder.Y1;
                    return;
                }
            }
            if (IsClimbing)
            {
                var hasClimbed = false;
                if (events.InputEvents.IsDown(InputAction.Up))
                {
                    hasClimbed = true;
                    this.MoveVertically(events.Delta, _commonData.Physics.WalkDrag - _commonData.Physics.WalkForce, _commonData.Physics.WalkSpeed);
                }

                if (events.InputEvents.IsDown(InputAction.Down))
                {
                    hasClimbed = true;
                    this.MoveVertically(events.Delta, _commonData.Physics.WalkForce - _commonData.Physics.WalkDrag, _commonData.Physics.WalkSpeed);
                }

                if (!hasClimbed && !SpeedY.IsStopped)
                {
                    var drag = _commonData.Physics.WalkDrag * SpeedY.OppositeDirection;
                    this.MoveVertically(events.Delta, drag, 0);
                }

                if (events.InputEvents.IsPressed(InputAction.Jump))
                {
                    this.Jump(_commonData.Physics.JumpSpeed * 0.75f, retainMomentum: true);
                    PhysicsState = PhysicsState.Airborn;
                    events.InputEvents.Reset(InputAction.Up);
                }
            }


            if (IsOnRail && events.InputEvents.IsPressed(InputAction.Jump) && events.InputEvents.IsDown(InputAction.Down))
            {
                if (footholdBelow != null && LastPlatform.Data.ForbidFallDown != true && !isLastFoothold)
                {
                    SpeedY.Stop();
                    Y += footholdWidth + 1;
                    PhysicsState = PhysicsState.Airborn;
                    return;
                }
            } else if (IsOnRail && events.InputEvents.IsPressed(InputAction.Jump))
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
            else if (IsAirborn)
            {
                if (events.InputEvents.IsDown(InputAction.Up) && _collisionDetector.WillCollideWithWall(MapInstance.Ladders, this, newX, out var ladder, tolerance: ladderWidth))
                {
                    PlaceOnLadder(ladder);
                    return;
                }
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
            else if (IsClimbing)
            {
                if (newY > LastLadder.Y2)
                {
                    PhysicsState = PhysicsState.Airborn;
                }
                if (newY < LastLadder.Y1 && LastLadder.Data.CanExitUp)
                {
                    PhysicsState = PhysicsState.Airborn;
                } else if (newY < LastLadder.Y1 && !LastLadder.Data.CanExitUp) {
                    newY = LastLadder.Y1; 
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

        private void PlaceOnLadder(Ladder ladder)
        {
            LastLadder = ladder;
            Z = LastLadder.Data.LayerId;
            X = LastLadder.X1;
            PhysicsState = PhysicsState.Climb;
            SpeedY.Stop();
        }
    }
}
