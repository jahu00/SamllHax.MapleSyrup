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
        const int footholdWidth = 2;

        private CommonData _commonData;
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
        public Foothold Foothold { get; set; }

        public Line LastMoveVector { get; set; }
        public SKPaint MoveVectorPaint { get; set; } = new SKPaint() { Color = SKColors.Blue };

        public PlayerInstance(CommonData commonData)//, Game game)
        {
            _commonData = commonData;
            //_game = game;
        }

        public void Draw(SKCanvas canvas, SKMatrix matrix)
        {
            Sprite.Draw(canvas, this.GetTransformMatrix(matrix));
            LastMoveVector?.Draw(canvas, matrix);
        }

        public MatchingFoothold TryGettingMatchingFoothold(float x, float y)
        {
            var matchingFootholds = MapInstance.GetFootholdsForX(x);
            if (matchingFootholds.Count() == 0)
            {
                return null;
            }
            var matchingFoothold = matchingFootholds.Where(pair => pair.Y + footholdWidth > y).OrderBy(pair => pair.Y).FirstOrDefault();
            /*if (matchingFoothold == null)
            {
                matchingFoothold = matchingFootholds.Last();
            }*/
            return matchingFoothold;
        }

        public void OnUpdate(UpdateEvents events)
        {
            var matchingFoothold = TryGettingMatchingFoothold(X, Y);
            if (IsAirborn && matchingFoothold != null && Foothold != matchingFoothold.Foothold)
            {
                Foothold = matchingFoothold.Foothold;
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
                    if (matchingFoothold?.IsLast == false && Foothold.Data.ForbidFallDown != true)
                    {
                        Y += footholdWidth + 1;
                        IsAirborn = true;
                        return;
                    }
                }
                if (events.KeyboardState.IsKeyPressed(Keys.Up))
                {
                    //_mapInstance.Character.Y -= move;
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
                if (newX < Foothold.X1)
                {
                    if (Foothold.Previous == null)
                    {
                        IsAirborn = true;
                    } else if (Foothold.Previous?.Type != LineType.Vertical) //Continuation
                    {
                        Foothold = Foothold.Previous;
                        newX = Foothold.X2;
                        newY = Foothold.Y2;
                    } else if (Foothold.Previous?.Y1 < Foothold.Y1) // Wall
                    {
                        newX = Foothold.X1;
                        newY = Foothold.Y1;
                        SpeedX = 0;
                    } else // Cliff
                    {
                        IsAirborn = true;
                    }
                } else if (newX > Foothold.X2)
                {
                    if (Foothold.Next == null)
                    {
                        IsAirborn = true;
                    }
                    else if (Foothold.Next?.Type != LineType.Vertical) //Continuation
                    {
                        Foothold = Foothold.Next;
                        newX = Foothold.X1;
                        newY = Foothold.Y1;
                    }
                    else if (Foothold.Next?.Y2 < Foothold.Y2) // Wall
                    {
                        newX = Foothold.X2;
                        newY = Foothold.Y2;
                        SpeedX = 0;
                    }
                    else // Cliff
                    {
                        IsAirborn = true;
                    }
                } else
                {
                    newY = (int)Foothold.GetY(newX);
                }
            }
            else
            {
                if (Foothold != null && SpeedY > 0)
                {
                    var maxY = Foothold.GetY(newX);
                    if (newY > maxY)
                    {
                        newY = maxY;
                        IsOnRail = true;
                        SpeedY = 0;
                        Z = Foothold.LayerId;
                    }
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
