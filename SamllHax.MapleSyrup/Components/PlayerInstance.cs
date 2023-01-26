using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
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
    public class PlayerInstance : ComponentBase, IDrawable, IUpdatable
    {
        const int footholdWidth = 5;

        private CommonData _commonData;
        private CollisionDetector _collisionDetector;

        //private readonly Game _game;

        public PlayerState State { get; private set; } = PlayerState.Stand;

        public float SpeedX { get; set; }
        public float SpeedY { get; set; }

        public IDrawable Sprite { get; set; }
        public MapInstance MapInstance { get; set; }

        public bool IsOnRail { get; set; } = false;
        public bool IsAirborn { get
            {
                return !IsOnRail;
            }
            set
            {
                IsOnRail = !value;
            }
        }
        public Foothold LastFoothold { get; set; }

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
            var footholdBelow = _collisionDetector.GetPlatformBelow(MapInstance.Footholds, X, Y - footholdWidth, out var _, out var isLastFoothold);
            if (IsAirborn && footholdBelow != null && LastFoothold != footholdBelow)
            {
                LastFoothold = footholdBelow;
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
                }
                else
                {
                    SpeedX -= Math.Sign(SpeedX) * drag;
                }
            }

            if (IsOnRail)
            {
                if (events.KeyboardState.IsKeyPressed(Keys.Down))
                {
                    if (footholdBelow != null && LastFoothold.Data.ForbidFallDown != true && !isLastFoothold)
                    {
                        Y += footholdWidth + 1;
                        IsAirborn = true;
                        return;
                    }
                }
                if (events.KeyboardState.IsKeyPressed(Keys.Up))
                {
                    SpeedY -= _commonData.Physics.JumpSpeed;
                    IsAirborn = true;
                }
            } else
            {
                if (SpeedY < _commonData.Physics.FallSpeed)
                {
                    SpeedY += (float)(events.Delta * _commonData.Physics.GravityAcc);
                    if (SpeedY > _commonData.Physics.GravityAcc)
                    {
                        SpeedY = _commonData.Physics.GravityAcc;
                    }
                }
            }

            
            var newX = X + events.Delta * SpeedX;
            var newY = Y + events.Delta * SpeedY;
            if (IsOnRail)
            {
                if (newX < LastFoothold.X1)
                {
                    if (LastFoothold.Previous == null)
                    {
                        IsAirborn = true;
                    } else if (LastFoothold.Previous?.Type != LineType.Vertical) //Continuation
                    {
                        LastFoothold = LastFoothold.Previous;
                        newX = LastFoothold.X2;
                        newY = LastFoothold.Y2;
                    } else if (LastFoothold.Previous?.Y1 < LastFoothold.Y1) // Wall
                    {
                        newX = LastFoothold.X1;
                        newY = LastFoothold.Y1;
                        SpeedX = 0;
                    } else // Cliff
                    {
                        newX = LastFoothold.Previous.X1 - 1;
                        IsAirborn = true;
                    }
                } else if (newX > LastFoothold.X2)
                {
                    if (LastFoothold.Next == null)
                    {
                        IsAirborn = true;
                    }
                    else if (LastFoothold.Next?.Type != LineType.Vertical) //Continuation
                    {
                        LastFoothold = LastFoothold.Next;
                        newX = LastFoothold.X1;
                        newY = LastFoothold.Y1;
                    }
                    else if (LastFoothold.Next?.Y2 < LastFoothold.Y2) // Wall
                    {
                        newX = LastFoothold.X2;
                        newY = LastFoothold.Y2;
                        SpeedX = 0;
                    }
                    else // Cliff
                    {
                        newX = LastFoothold.Next.X1 + 1;
                        IsAirborn = true;
                    }
                } else
                {
                    newY = (int)LastFoothold.GetY(newX);
                }
            }
            else
            {
                if (LastFoothold != null && SpeedY > 0)
                {
                    var maxY = LastFoothold.GetY(newX);
                    if (newY > maxY)
                    {
                        newY = maxY;
                        IsOnRail = true;
                        SpeedY = 0;
                        Z = LastFoothold.LayerId;
                    }
                }
                if (_collisionDetector.WillCollideWithWall(MapInstance.Footholds, this, newX, out var wall))
                {
                    newX = wall.X1 - Math.Sign(SpeedX);
                }
            }

            if (newY > MapInstance.MaxY)
            {
                newY = MapInstance.MaxY;
                SpeedY = 0;
            } else if (newY > MapInstance.MaxY)
            {
                newY = MapInstance.MaxY;
                SpeedY = 0;
            }
            /*else if (newY < mapBoundingBox.Top)
            {
                newY = mapBoundingBox.Top;
                SpeedY = 0;
            }*/

            if (newX < MapInstance.MinX)
            {
                newX = MapInstance.MinX;
            }
            else if (newX > MapInstance.MaxX)
            {
                newX = MapInstance.MaxX;
            }

            X = (float)newX;
            Y = (float)newY;
        }
    }
}
