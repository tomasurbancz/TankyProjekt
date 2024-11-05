using System;
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
    private Rectangle _rectangle;
    
    private Vector2 _mousePosition;
    
    private Rectangle _upPartRectangle;
    private float _upPartRotation;
    private Texture2D _upPartTexture;
    private Vector2 _offSet;
    
    private Texture2D _bulletTexture;
    private Vector2 _bulletPosition;
    private Rectangle _bulletRectangle;
    private float _bulletRotation;
    private float _bulletSpeed;

    public Game1()
    {
        _bulletRectangle = new Rectangle(0, 0, 50, 25);
        _upPartRectangle = new Rectangle(0, 0, 100, 25);
        _rectangle = new Rectangle(0, 0, 150, 75);
        _speed = 5;
        _bulletSpeed = 10;
        _bulletRotation = 180;
        _offSet = new Vector2(_rectangle.Width / 2f - 10, _rectangle.Height / 2f);
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

        _upPartTexture = new Texture2D(GraphicsDevice, 1, 1);
        _upPartTexture.SetData([Color.Orange]);
        
        _bulletTexture = new Texture2D(GraphicsDevice, 1, 1);
        _bulletTexture.SetData([Color.Black]);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here

        UpdateMousePosition();
        RotateByMouse();
        UpdateInput();
        UpdateBullet();
        
        base.Update(gameTime);
    }

    private void UpdateMousePosition()
    {
        MouseState mouseState = Mouse.GetState();
        _mousePosition = new Vector2(mouseState.X, mouseState.Y);
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

        if (keyboardState.IsKeyDown(Keys.Space))
        {
            Shoot();
            _upPartRotation += 0.05f;
        }
        
        _position.X = MathHelper.Clamp(_position.X, 0, GraphicsDevice.Viewport.Width - _rectangle.Width);
        _position.Y = MathHelper.Clamp(_position.Y, 0, GraphicsDevice.Viewport.Height - _rectangle.Height);
    }

    private void RotateByMouse()
    {
        Vector2 tankCenter = _position + _offSet;
        Vector2 direction = _mousePosition - tankCenter;
        _upPartRotation = (float)Math.Atan2(direction.Y, direction.X);
    }

    private bool isAwayFromScreen(Vector2 position)
    {
        return true;
        return (position.X <= 0 || position.X >= GraphicsDevice.Viewport.Width || position.Y <= 0 ||
                position.Y >= GraphicsDevice.Viewport.Height);
    }
    
    private void Shoot()
    {
        if (isAwayFromScreen(_bulletPosition))
        {
            _bulletPosition = (_position + _offSet) + (_upPartRectangle.Width / 2f) * new Vector2((float)Math.Cos(_upPartRotation), (float)Math.Sin(_upPartRotation));
            _bulletRotation = _upPartRotation;
        }
    }

    private void UpdateBullet()
    {
        _bulletPosition += new Vector2((float) Math.Cos(_bulletRotation) * _bulletSpeed, (float) Math.Sin(_bulletRotation) * _bulletSpeed);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here

        _spriteBatch.Begin();
        
        _spriteBatch.Draw(_texture, _position, _rectangle, Color.White);
        
        _spriteBatch.Draw(_bulletTexture, _bulletPosition, _bulletRectangle, Color.White, _bulletRotation, new Vector2(_bulletRectangle.Width/2f, _bulletRectangle.Height/2f), Vector2.One, SpriteEffects.None, 0);

        _spriteBatch.Draw(_upPartTexture,
            _position + _offSet,
            _upPartRectangle, Color.White, _upPartRotation, new Vector2(-10, _upPartRectangle.Height / 2f), Vector2.One, SpriteEffects.None, 0);
        
        _spriteBatch.End();
        
        base.Draw(gameTime);
    }
}