using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TankyProjekt;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Texture2D _texture;
    private Vector2 _position;
    private float _speed;

    public Game1()
    {
        _speed = 1;
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
        
        _texture = new Texture2D(GraphicsDevice, 1, 1);
        _texture.SetData([Color.White]);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here

        UpdateInput();
        
        base.Update(gameTime);
    }
    
    private void UpdateInput()
    {
        KeyboardState keyboardState = Keyboard.GetState();
        if (keyboardState.IsKeyDown(Keys.W))
        {
            _position.Y -= _speed;
        }

        if (keyboardState.IsKeyDown(Keys.A))
        {
            _position.X -= _speed;
        }

        if (keyboardState.IsKeyDown(Keys.S))
        {
            _position.Y += _speed;
        }

        if (keyboardState.IsKeyDown(Keys.D))
        {
            _position.X += _speed;
        }
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here

        _spriteBatch.Begin();

        var rectangle = new Rectangle(0, 0, 50, 25);
        
        _spriteBatch.Draw(_texture, _position, rectangle, Color.White);

        _spriteBatch.End();
        
        base.Draw(gameTime);
    }
}