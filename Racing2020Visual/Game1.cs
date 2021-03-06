using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace Racing2020Visual
{
    // to do
    // - add track tiles to variables
    // - If statement in draw to check which tile to draw
    // - Get variable X and Y to draw next to eachother
    // - put into variable/class/list at initialize so its calculated only once
    // - of toch met slideshow, zoals cycling manager 1? scrollend veld en enkel den eerste int midden (voor nu)
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D _trackHorizontal;
        private Texture2D _trackUp;
        private Texture2D _trackDown;
        private SpriteFont _spriteFont;

        private IList<TrackTile> _trackTiles;
        private IList<TrackTileVisual> _trackTileVisuals;
        private IList<Cyclist> _cyclists;

        private Queue<Cyclist> _finishedCyclists;

        private float _screenPosition;
        private float _centerX;
        private float _scrollSpeed = 200f;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _trackTiles = new List<TrackTile>();
            _cyclists = new List<Cyclist>();
            _finishedCyclists = new Queue<Cyclist>();
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
            _graphics.ApplyChanges();

            _trackTiles.Add(TrackTile.Horizontal);
            _trackTiles.Add(TrackTile.Horizontal);
            _trackTiles.Add(TrackTile.Horizontal);
            _trackTiles.Add(TrackTile.Up);
            _trackTiles.Add(TrackTile.Horizontal);
            _trackTiles.Add(TrackTile.Horizontal);
            _trackTiles.Add(TrackTile.Down);
            _trackTiles.Add(TrackTile.Horizontal);
            _trackTiles.Add(TrackTile.Horizontal);

            var counter = 0;
            do
            {
                _cyclists.Add(new Cyclist(RandomFloat(50f, 100f), RandomFloat(50f, 100f), RandomFloat(50f, 100f), "Cyclist " + counter));
                counter++;
            } while (counter < 10);


            _trackTileVisuals = DrawTrack.Track(_trackTiles, GraphicsDevice.DisplayMode.Width / 2);
            _centerX = GraphicsDevice.DisplayMode.Width / 2;

            foreach (var cyclist in _cyclists)
            {
                cyclist.CyclistPositionX = _centerX;
            }

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            _trackHorizontal = Content.Load<Texture2D>("TrackHorizontal");
            _trackUp = Content.Load<Texture2D>("TrackDownUp");
            _trackDown = Content.Load<Texture2D>("TrackUpDown");
            _spriteFont = Content.Load<SpriteFont>("Fonts/DefaultFont");

            foreach (var cyclist in _cyclists)
            {
                cyclist.CyclistTexture = Content.Load<Texture2D>("Cyclist");
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            // TODO: Add your update logic here
            var kstate = Keyboard.GetState();

            var raceLeader = _cyclists[0];

            foreach (var cyclist in _cyclists)
            {
                var positionCentertrack = _trackTileVisuals.Where(x => (x.X - _screenPosition) <= cyclist.CyclistPositionX).Max(x => x.X);
                var centreTrack = _trackTileVisuals.Where(x => x.X == positionCentertrack).FirstOrDefault();
                var oldPosition = cyclist.CyclistPositionX;

                if (centreTrack.TrackTile == TrackTile.Horizontal)
                {
                    if (centreTrack.X + TextureParameters.Horizontal < cyclist.CyclistPositionX + _screenPosition)
                    {
                        if (!_finishedCyclists.Contains(cyclist))
                        {
                            _finishedCyclists.Enqueue(cyclist);
                        }
                        continue;
                    }
                    cyclist.CyclistPositionY = centreTrack.Y;
                    cyclist.CyclistPositionX += cyclist.CyclistSpeedHorizontal * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                else if (centreTrack.TrackTile == TrackTile.Up)
                {
                    var differenceX = cyclist.CyclistPositionX - (positionCentertrack - _screenPosition);
                    cyclist.CyclistPositionY = (centreTrack.Y + TextureParameters.UpDown / 2) - differenceX / 2;
                    cyclist.CyclistPositionX += cyclist.CyclistSpeedUp * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                else if (centreTrack.TrackTile == TrackTile.Down)
                {
                    var differenceX = cyclist.CyclistPositionX - (positionCentertrack - _screenPosition);
                    cyclist.CyclistPositionY = centreTrack.Y + differenceX / 2;
                    cyclist.CyclistPositionX += cyclist.CyclistSpeedDown * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }

                if (cyclist.CyclistPositionX > raceLeader.CyclistPositionX)
                {
                    raceLeader = cyclist;
                }

            }
            var _raceLeaderGain = (raceLeader.CyclistPositionX - _centerX);

            foreach (var cyclist in _cyclists)
            {
                cyclist.CyclistPositionX -= _raceLeaderGain;
            }
            _screenPosition += _raceLeaderGain;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            foreach (var trackTileVisual in _trackTileVisuals)
            {
                switch (trackTileVisual.TrackTile)
                {
                    case TrackTile.Horizontal:
                        _spriteBatch.Draw(_trackHorizontal, new Vector2(trackTileVisual.X - _screenPosition, trackTileVisual.Y), Color.White);
                        break;
                    case TrackTile.Up:
                        _spriteBatch.Draw(_trackUp, new Vector2(trackTileVisual.X - _screenPosition, trackTileVisual.Y), Color.White);
                        break;
                    case TrackTile.Down:
                        _spriteBatch.Draw(_trackDown, new Vector2(trackTileVisual.X - _screenPosition, trackTileVisual.Y), Color.White);
                        break;
                    default:
                        break;
                }
            }

            foreach (var cyclist in _cyclists)
            {
                _spriteBatch.Draw(cyclist.CyclistTexture, new Vector2(cyclist.CyclistPositionX, cyclist.CyclistPositionY), Color.White);
                _spriteBatch.DrawString(_spriteFont, cyclist.Name, new Vector2(cyclist.CyclistPositionX, cyclist.CyclistPositionY - TextureParameters.FontSize), Color.White);
            }

            var counter = 0;
            foreach (var finishedCyclist in _finishedCyclists)
            {
                counter++;
                _spriteBatch.DrawString(_spriteFont, finishedCyclist.Name, new Vector2(0, TextureParameters.FontSize * counter), Color.White);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        static float RandomFloat(float min, float max)
        {
            System.Random random = new System.Random();
            double val = (random.NextDouble() * (max - min) + min);
            return (float)val;
        }
    }
}
