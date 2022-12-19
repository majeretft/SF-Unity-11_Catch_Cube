using SFML.Graphics;
using SFML.System;
using System;

namespace _11_Catch_Cube.Model
{
    public class ObjectSprite : ObjectBase
    {
        private Sprite _sprite;

        protected override Vector2f Position
        {
            get
            {
                return _sprite.Position;
            }
            set
            {
                _sprite.Position = value;
            }
        }

        protected override Vector2f Size
        {
            get
            {
                var x = _sprite.TextureRect.Width * _sprite.Scale.X;
                var y = _sprite.TextureRect.Height * _sprite.Scale.Y;

                return new Vector2f(x, y);
            }
            set
            {
                var sizeX = value.X;
                var sizeY = value.Y;

                var x = sizeX / _sprite.TextureRect.Width;
                var y = sizeY / _sprite.TextureRect.Height;

                _sprite.Scale = new Vector2f(x, y);
            }
        }

        public ObjectSprite(ObjectType type, float speed, Random random, IntRect screenSize, Texture texture)
            : base(type, speed, random, screenSize)
        {
            _sprite = new Sprite(texture);

            RestartObject();
        }

        public override void Draw(RenderTarget target, RenderStates renderStates)
        {
            _sprite.Draw(target, renderStates);
        }

        public override bool CheckClicked(int ClickX, int ClickY)
        {
            var bounds = _sprite.GetGlobalBounds();

            if (bounds.Contains(ClickX, ClickY) == false)
                return false;

            var radius = _sprite.TextureRect.Width * _sprite.Scale.X / 2f;
            var centerX = bounds.Left + radius;
            var centerY = bounds.Top + radius;

            var delta = Math.Sqrt(Math.Pow(centerX - ClickX, 2) + Math.Pow(centerY - ClickY, 2));

            var isClicked = radius > delta;

            if (isClicked && Type == ObjectType.Click)
                Shrink();

            return isClicked;
        }

        public override void Move()
        {
            _sprite.Position += _direction * _speed;
        }
    }
}
