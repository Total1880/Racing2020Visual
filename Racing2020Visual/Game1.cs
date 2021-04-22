using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
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
        private Texture2D _cyclist;
        private IList<TrackTile> _trackTiles;
        private IList<TrackTileVisual> _trackTileVisuals;
        private float _screenPosition;
        private float _cyclistPositionX;
        private float _cyclistPositionY = 600;
        private float _quotient;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _trackTiles = new List<TrackTile>();
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

            _trackTileVisuals = DrawTrack.Track(_trackTiles, GraphicsDevice.DisplayMode.Width / 2);
            _cyclistPositionX = GraphicsDevice.DisplayMode.Width / 2;

            _quotient = ((float)TextureParameters.Horizontal / (float)TextureParameters.UpDown);


            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            _trackHorizontal = Content.Load<Texture2D>("TrackHorizontal");
            _trackUp = Content.Load<Texture2D>("TrackDownUp");
            _trackDown = Content.Load<Texture2D>("TrackUpDown");
            _cyclist = Content.Load<Texture2D>("Cyclist");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            // TODO: Add your update logic here
            var kstate = Keyboard.GetState();
            if (kstate.IsKeyDown(Keys.Right))
            {
                _screenPosition += 100f * (float)gameTime.ElapsedGameTime.TotalSeconds;

                var positionCentertrack = _trackTileVisuals.Where(x => (x.X - _screenPosition) <= _cyclistPositionX).Max(x => x.X);
                var centreTrack = _trackTileVisuals.Where(x => x.X == positionCentertrack).FirstOrDefault();

                if (centreTrack.TrackTile == TrackTile.Horizontal)
                {
                    _cyclistPositionY = centreTrack.Y;
                }
                else if (centreTrack.TrackTile == TrackTile.Up)
                {
                    var differenceX = _cyclistPositionX - (positionCentertrack - _screenPosition);
                    _cyclistPositionY = (centreTrack.Y + TextureParameters.UpDown / 2) - differenceX / 2;
                }
                else if (centreTrack.TrackTile == TrackTile.Down)
                {
                    var differenceX = _cyclistPositionX - (positionCentertrack - _screenPosition);
                    _cyclistPositionY = centreTrack.Y + differenceX / 2;
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


            _spriteBatch.Draw(_cyclist, new Vector2(_cyclistPositionX, _cyclistPositionY), Color.White);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }

    public static class DrawTrack
    {
        public static List<TrackTileVisual> Track(IList<TrackTile> trackTiles, int startPositionX)
        {
            var track = new List<TrackTileVisual>();
            int x = startPositionX;
            int y = 600;

            foreach (var tile in trackTiles)
            {


                switch (tile)
                {
                    case TrackTile.Horizontal:
                        track.Add(new TrackTileVisual(tile, x, y));

                        x += TextureParameters.Horizontal;
                        break;
                    case TrackTile.Up:
                        y -= TextureParameters.UpDown - TextureParameters.Horizontal;

                        track.Add(new TrackTileVisual(tile, x, y));

                        x += TextureParameters.UpDown;
                        break;
                    case TrackTile.Down:
                        track.Add(new TrackTileVisual(tile, x, y));

                        x += TextureParameters.UpDown;
                        y += TextureParameters.UpDown - TextureParameters.Horizontal;
                        break;
                    default:
                        throw Exception(tile + " not found");
                }

            }

            return track;
        }

        private static Exception Exception(string message)
        {
            throw new NotImplementedException(message);
        }
    }
}
