using SFML.Graphics;
using SFML.System;
using System;

namespace _11_Catch_Cube.Model
{
    public abstract class ObjectBase : Drawable
    {
        protected static Color _colorClick = new Color(100, 100, 100);
        protected static Color _colorNotClick = new Color(235, 95, 95);

        protected Vector2f _direction;
        protected float _speed;
        protected float _sizeMin = 40;
        protected float _sizeMax = 300;
        protected Random _random;
        protected IntRect _screenSize;
        protected int _size = 160;

        protected float _stepShrink = 20;
        protected float _stepGrow = 20;

        protected abstract Vector2f Position { get; set; }
        protected abstract Vector2f Size { get; set; }

        public ObjectType Type { get; set; }
        public bool IsCompleted { get; protected set; }

        protected ObjectBase(ObjectType type, float speed, Random random, IntRect screenSize)
        {
            Type = type;
            
            _speed = speed;
            _random = random;
            _screenSize = screenSize;
        }

        protected Vector2f GenerateDirection()
        {
            var dirX = (float)_random.NextDouble();
            var dirY = 1f - dirX;
            var dirVec = new Vector2f(dirX, dirY);

            return dirVec;
        }

        public void DetectCollisionAndBounce()
        {
            var isCoughtX = Position.X + Size.X - _screenSize.Width >= 0;
            var isCoughtY = Position.Y + Size.Y - _screenSize.Height >= 0;

            if (Position.X < _screenSize.Left || Position.X + Size.X > _screenSize.Width)
                if (isCoughtX)
                    _direction.X = -Math.Abs(_direction.X);
                else
                    _direction.X *= -1;


            if (Position.Y < _screenSize.Top || Position.Y + Size.Y > _screenSize.Height)
                if (isCoughtY)
                    _direction.Y = -Math.Abs(_direction.Y);
                else
                    _direction.Y *= -1;
        }

        public void Shrink()
        {
            if (Type == ObjectType.NotClick)
                return;

            if (Size.X <= _sizeMin)
            {
                IsCompleted = true;
                return;
            }

            Size = new Vector2f(Size.X - _stepShrink, Size.Y - _stepShrink);
        }

        public void Grow()
        {
            if (Type == ObjectType.Click)
                return;

            if (Size.X >= _sizeMax)
            {
                IsCompleted = true;
                return;
            }

            Size = new Vector2f(Size.X + _stepGrow, Size.Y + _stepGrow);
        }

        protected Vector2f GeneratePosition()
        {
            return new Vector2f(
               _random.Next(_screenSize.Left, _screenSize.Width - _size),
               _random.Next(_screenSize.Top, _screenSize.Height - _size)
           );
        }

        public void RestartObject()
        {
            Position = GeneratePosition();

            _direction = GenerateDirection();

            Size = new Vector2f(_size, _size);

            IsCompleted = false;
        }

        public abstract void Draw(RenderTarget target, RenderStates renderStates);

        public abstract bool CheckClicked(int ClickX, int ClickY);

        public abstract void Move();

        public enum ObjectType
        {
            Click,
            NotClick,
        }
    }
}
