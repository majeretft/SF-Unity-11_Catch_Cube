using SFML.Graphics;
using SFML.System;
using System;

namespace _11_Catch_Cube.Model
{
    public class ObjectCircle : ObjectBase
    {
        private CircleShape _shape;

        protected override Vector2f Position
        {
            get
            {
                return _shape.Position;
            }
            set
            {
                _shape.Position = value;
            }
        }

        protected override Vector2f Size
        {
            get
            {
                return new Vector2f(_shape.Radius * 2, _shape.Radius * 2);
            }
            set
            {
                _shape.Radius = value.X / 2;
            }
        }

        public ObjectCircle(ObjectType type, float speed, Random random, IntRect screenSize)
            : base(type, speed, random, screenSize)
        {
            _shape = new CircleShape(_size)
            {
                FillColor = type == ObjectType.Click ? _colorClick : _colorNotClick,
            };

            RestartObject();
        }

        public override void Draw(RenderTarget target, RenderStates renderStates)
        {
            _shape.Draw(target, renderStates);
        }

        public override bool CheckClicked(int ClickX, int ClickY)
        {
            var bounds = _shape.GetGlobalBounds();

            if (bounds.Contains(ClickX, ClickY) == false)
                return false;

            var radius = _shape.Radius;
            var centerX = bounds.Left + radius;
            var centerY = bounds.Top + radius;

            var delta = Math.Sqrt(Math.Pow(centerX - ClickX, 2) + Math.Pow(centerY - ClickY, 2));

            var isClicked =  radius > delta;

            if (isClicked && Type == ObjectType.Click)
                Shrink();

            return isClicked;
        }

        public override void Move()
        {
            _shape.Position += _direction * _speed;
        }
    }
}
