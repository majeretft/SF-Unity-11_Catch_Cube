using SFML.Graphics;
using SFML.System;
using System;

namespace _11_Catch_Cube.Model
{
    public class ObjectCube : ObjectBase
    {
        private RectangleShape _shape;

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
                return _shape.Size;
            }
            set
            {
                _shape.Size = value;
            }
        }

        public ObjectCube(ObjectType type, float speed, Random random, IntRect screenSize)
            : base(type, speed, random, screenSize)
        {
            _shape = new RectangleShape(new Vector2f(_size, _size))
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
            var isClicked = _shape.GetGlobalBounds().Contains(ClickX, ClickY);

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
