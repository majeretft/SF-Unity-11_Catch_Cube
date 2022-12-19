using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;

namespace _11_Catch_Cube.Model
{
    public class Game
    {
        private RenderWindow _window;
        private Font _font;
        private Text _textMenu;
        private Text _textLoose;
        private Text _textScore;
        private Color _bgColor = new Color(200, 200, 200);

        private const string _messageMenu = @"Press 'R' to start/restart 
Press 'ESC' to exit
Press '1' to select shape 'Cube'
Press '2' to select shape 'Circle'
Press '3' to select shape 'Sprite'
";
        private const string _messageLoose = @"You LOST !!!

Press 'R' to start/restart 
Press 'ESC' to exit
Press '1' to select shape 'Cube'
Press '2' to select shape 'Circle'
Press '3' to select shape 'Sprite'
";

        private GameStatus _currentGameStatus = GameStatus.Menu;
        private GameObject _currentGameObject = GameObject.Cube;

        private List<ObjectBase> _gameObjects;

        private Texture _textureClick;
        private Texture _textureNotClick;

        private IntRect _screenSize;

        private int _scoreMax = 0;
        private int _score = 0;

        private int _intervalGrowSec = 5;
        private int _speed = 2;

        private readonly Random _random = new Random();
        private readonly Clock _clock = new Clock();

        public Game()
        {
            _window = new RenderWindow(
                new VideoMode(800, 800),
                "Catch that cube!",
                Styles.Titlebar | Styles.Close,
                new ContextSettings() { AntialiasingLevel = 16 }
            );

            _screenSize = new IntRect(0, 0, 800, 800);

            _window.SetFramerateLimit(165);

            _window.KeyPressed += HandleKeyPressed;
            _window.MouseButtonPressed += HandleMouseButtonPressed;
            _window.Closed += (s, e) => Environment.Exit(0);
        }

        private void HandleMouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            if (e.Button != Mouse.Button.Left)
                return;

            foreach (var go in _gameObjects)
            {
                if (go.CheckClicked(e.X, e.Y))
                {
                    if (go.Type == ObjectBase.ObjectType.NotClick)
                        _currentGameStatus = GameStatus.Loose;

                    if (go.Type == ObjectBase.ObjectType.Click)
                    {
                        ++_score;
                        _textScore.DisplayedString = $"Score: {_score}\nMax Score: {_scoreMax}";
                    }

                    if (go.IsCompleted)
                        go.RestartObject();


                    if (_score >= _scoreMax)
                    {
                        _scoreMax = _score;
                        _textScore.DisplayedString = $"Score: {_score}\nMax Score: {_scoreMax}";
                    }
                }
            }
        }

        private void HandleKeyPressed(object sender, KeyEventArgs e)
        {
            var suitableMods = GameStatus.Menu | GameStatus.Loose;

            if ((suitableMods & _currentGameStatus) <= 0)
                return;

            switch (e.Code)
            {
                case Keyboard.Key.R:
                    InitGame();
                    _currentGameStatus = GameStatus.Game;
                    break;

                case Keyboard.Key.Escape:
                    _currentGameStatus = GameStatus.Exit;
                    break;

                case Keyboard.Key.Num1:
                    _currentGameObject = GameObject.Cube;
                    break;

                case Keyboard.Key.Num2:
                    _currentGameObject = GameObject.Circle;
                    break;

                case Keyboard.Key.Num3:
                    _currentGameObject = GameObject.Sprite;
                    break;
            }
        }

        public void Start()
        {
            LoadAssets();
            InitGame();

            while (_currentGameStatus != GameStatus.Exit)
            {
                if (_currentGameStatus == GameStatus.Game)
                    DrawGameCycle();

                if (_currentGameStatus == GameStatus.Menu)
                    DrawMenuCycle();

                if (_currentGameStatus == GameStatus.Loose)
                    DrawLooseCycle();
            }
        }

        private void InitGame()
        {
            _score = 0;
            _clock.Restart();

            _textMenu = new Text(_messageMenu, _font, 36)
            {
                Position = new Vector2f(120, 300),
                FillColor = Color.Black,
            };

            _textLoose = new Text(_messageLoose, _font, 36)
            {
                Position = new Vector2f(120, 240),
                FillColor = Color.Black,
            };

            _textScore = new Text($"Score: {_score}\nMax Score: {_scoreMax}", _font, 16)
            {
                Position = new Vector2f(20, 20),
                FillColor = Color.Black,
            };

            _gameObjects = new List<ObjectBase>();

            switch (_currentGameObject)
            {
                case GameObject.Cube:
                    _gameObjects.Add(new ObjectCube(ObjectBase.ObjectType.Click, _speed, _random, _screenSize));
                    _gameObjects.Add(new ObjectCube(ObjectBase.ObjectType.Click, _speed, _random, _screenSize));
                    _gameObjects.Add(new ObjectCube(ObjectBase.ObjectType.Click, _speed, _random, _screenSize));
                    _gameObjects.Add(new ObjectCube(ObjectBase.ObjectType.NotClick, _speed, _random, _screenSize));
                    _gameObjects.Add(new ObjectCube(ObjectBase.ObjectType.NotClick, _speed, _random, _screenSize));
                    break;

                case GameObject.Circle:
                    _gameObjects.Add(new ObjectCircle(ObjectBase.ObjectType.Click, _speed, _random, _screenSize));
                    _gameObjects.Add(new ObjectCircle(ObjectBase.ObjectType.Click, _speed, _random, _screenSize));
                    _gameObjects.Add(new ObjectCircle(ObjectBase.ObjectType.Click, _speed, _random, _screenSize));
                    _gameObjects.Add(new ObjectCircle(ObjectBase.ObjectType.NotClick, _speed, _random, _screenSize));
                    _gameObjects.Add(new ObjectCircle(ObjectBase.ObjectType.NotClick, _speed, _random, _screenSize));
                    break;

                case GameObject.Sprite:
                    _gameObjects.Add(new ObjectSprite(ObjectBase.ObjectType.Click, _speed, _random, _screenSize, _textureClick));
                    _gameObjects.Add(new ObjectSprite(ObjectBase.ObjectType.Click, _speed, _random, _screenSize, _textureClick));
                    _gameObjects.Add(new ObjectSprite(ObjectBase.ObjectType.Click, _speed, _random, _screenSize, _textureClick));
                    _gameObjects.Add(new ObjectSprite(ObjectBase.ObjectType.NotClick, _speed, _random, _screenSize, _textureNotClick));
                    _gameObjects.Add(new ObjectSprite(ObjectBase.ObjectType.NotClick, _speed, _random, _screenSize, _textureNotClick));
                    break;
            }
        }

        private void LoadAssets()
        {
            _font = new Font("asset/arial.ttf");
            _textureClick = new Texture("asset/Q.png");
            _textureNotClick = new Texture("asset/E.png");
        }

        private bool DrawGameCycle()
        {
            _window.DispatchEvents();
            _window.Clear(_bgColor);

            foreach (var go in _gameObjects)
            {
                go.DetectCollisionAndBounce();
                go.Move();

                if (_clock.ElapsedTime.AsSeconds() > _intervalGrowSec)
                    go.Grow();

                _window.Draw(go);
            }

            if (_clock.ElapsedTime.AsSeconds() > _intervalGrowSec)
                _clock.Restart();

            _window.Draw(_textScore);

            _window.Display();

            return true;
        }

        private void DrawMenuCycle()
        {
            _window.DispatchEvents();
            _window.Clear(_bgColor);

            _window.Draw(_textMenu);

            _window.Display();
        }

        private void DrawLooseCycle()
        {
            _window.DispatchEvents();
            _window.Clear(_bgColor);

            _window.Draw(_textLoose);

            _window.Display();
        }

        [Flags]
        private enum GameStatus
        {
            Game,
            Menu,
            Loose,
            Exit,
        }

        private enum GameObject
        {
            Cube,
            Circle,
            Sprite,
        }
    }
}
