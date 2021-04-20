using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Racing2020Visual
{
    // to do
    // - add track tiles to variables
    // - If statement in draw to check which tile to draw
    // - Get variable X and Y to draw next to eachother
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private IList<TrackTile> _track;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _track = new List<TrackTile>();
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            _track.Add(TrackTile.Horizontal);
            _track.Add(TrackTile.Horizontal);
            _track.Add(TrackTile.Horizontal);
            _track.Add(TrackTile.Horizontal);
            _track.Add(TrackTile.LeftDown);
            _track.Add(TrackTile.Vertical);
            _track.Add(TrackTile.LeftUp);
            _track.Add(TrackTile.Horizontal);
            _track.Add(TrackTile.UpDown);
            _track.Add(TrackTile.DownUp);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            foreach (var tile in _track)
            {
                _spriteBatch.Draw(tile, new Vector2(0, 0), Color.White);
            }

            base.Draw(gameTime);
        }
    }
}
