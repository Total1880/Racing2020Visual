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

        private IList<TrackTile> _trackTiles;
        private IList<TrackTileVisual> _trackTileVisuals;
        private IList<Cyclist> _cyclists;

        private float _screenPosition;

        private float _scrollSpeed = 200f;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _trackTiles = new List<TrackTile>();
            _cyclists = new List<Cyclist>();
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

            _cyclists.Add(new Cyclist(100f, 50f, 150f));
            _cyclists.Add(new Cyclist(90f, 60f, 170f));
            _cyclists.Add(new Cyclist(95f, 55f, 160f));
            _cyclists.Add(new Cyclist(110f, 40f, 150f));

            _trackTileVisuals = DrawTrack.Track(_trackTiles, GraphicsDevice.DisplayMode.Width / 2);

            foreach (var cyclist in _cyclists)
            {
                cyclist.CyclistPositionX = GraphicsDevice.DisplayMode.Width / 2;
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
            if (kstate.IsKeyDown(Keys.Right))
            {
                _screenPosition += _scrollSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            foreach (var cyclist in _cyclists)
            {
                var positionCentertrack = _trackTileVisuals.Where(x => (x.X - _screenPosition) <= cyclist.CyclistPositionX).Max(x => x.X);
                var centreTrack = _trackTileVisuals.Where(x => x.X == positionCentertrack).FirstOrDefault();

                if (centreTrack.TrackTile == TrackTile.Horizontal)
                {
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


                if (kstate.IsKeyDown(Keys.Right))
                {
                    cyclist.CyclistPositionX -= _scrollSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
            }

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
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
