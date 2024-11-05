using System;
using System.Numerics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace TankyProjekt;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    
    private Texture2D _texture;
    public Vector2 _position;
    private float _speed;
    public Rectangle _rectangle;
    private float _rotation;
    
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

    private GameTime _gameTime = new GameTime();
    
    private EnemyTank _enemyTank;
    
    public Game1()
    {
        _bulletRectangle = new Rectangle(0, 0, 25, 12);
        _upPartRectangle = new Rectangle(0, 0, 75, 12);
        _rectangle = new Rectangle(0, 0, 75, 37);
        _speed = 5;
        _bulletSpeed = 10;
        _bulletRotation = 180;
        _bulletPosition = new Vector2(-1000, -1000);
        _offSet = new Vector2(0, _rectangle.Height / 2f);
        _graphics = new GraphicsDeviceManager(this);
        _enemyTank = new EnemyTank(this);
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
        
        _enemyTank.LoadTexture();
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
        
        _enemyTank.Update(_gameTime);
        
        base.Update(gameTime);
    }

    private Vector2 GetOffsetWithRotation()
    {
        float offsetX = (float)Math.Cos(_rotation) * _offSet.X; 
        float offsetY = (float)Math.Sin(_rotation) * _offSet.Y;

        return new Vector2(offsetX, offsetY);
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
            _position += new Vector2((float) Math.Cos(_rotation) * _speed, (float) Math.Sin(_rotation) * _speed);
        }

        if (keyboardState.IsKeyDown(Keys.A))
        {
            _rotation -= 0.05f;
        }

        if (keyboardState.IsKeyDown(Keys.S))
        {
            _position -= new Vector2((float) Math.Cos(_rotation) * _speed, (float) Math.Sin(_rotation) * _speed);
        }

        if (keyboardState.IsKeyDown(Keys.D))
        {
            _rotation += 0.05f;
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
        Vector2 start = _position + GetOffsetWithRotation();
        Vector2 direction = _mousePosition - start;
        _upPartRotation = (float)Math.Atan2(direction.Y, direction.X);
    }

    public bool isAwayFromScreen(Vector2 position)
    {
        return (position.X <= 0 || position.X >= GraphicsDevice.Viewport.Width || position.Y <= 0 ||
                position.Y >= GraphicsDevice.Viewport.Height);
    }
    
    private void Shoot()
    {
        if (isAwayFromScreen(_bulletPosition))
        {
            _bulletPosition = (_position + GetOffsetWithRotation()) + (_upPartRectangle.Width / 2f) * new Vector2((float)Math.Cos(_upPartRotation), (float)Math.Sin(_upPartRotation));
            _bulletRotation = _upPartRotation;
        }
    }

    private void UpdateBullet()
    {
        _bulletPosition += new Vector2((float) Math.Cos(_bulletRotation) * _bulletSpeed, (float) Math.Sin(_bulletRotation) * _bulletSpeed);
        
        if (new Rectangle((int)_bulletPosition.X, (int)_bulletPosition.Y, _rectangle.Width, _rectangle.Height)
            .Intersects(new Rectangle((int)_enemyTank._position.X, (int)_enemyTank._position.Y, _enemyTank._rectangle.Width,
                _enemyTank._rectangle.Height)))
        {
            Console.WriteLine("Výhra");
            Environment.Exit(0);
        }
    }

    protected override void Draw(GameTime gameTime)
    {
        _gameTime = gameTime;
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here

        _spriteBatch.Begin();
        
        _spriteBatch.Draw(_texture, _position, _rectangle, Color.White, _rotation,
            new Vector2(_rectangle.Width / 2f, _rectangle.Height / 2f), Vector2.One, SpriteEffects.None, 0);
        
        _spriteBatch.Draw(_bulletTexture, _bulletPosition, _bulletRectangle, Color.White, _bulletRotation, new Vector2(_bulletRectangle.Width/2f, _bulletRectangle.Height/2f), Vector2.One, SpriteEffects.None, 0);

        _spriteBatch.Draw(_upPartTexture,
            _position + GetOffsetWithRotation(),
            _upPartRectangle, Color.White, _upPartRotation, new Vector2(-10, _upPartRectangle.Height / 2f), Vector2.One, SpriteEffects.None, 0);
        
        _spriteBatch.Draw(_enemyTank._texture, _enemyTank._position, _enemyTank._rectangle, Color.White, _enemyTank._rotation,
            new Vector2(_enemyTank._rectangle.Width / 2f, _enemyTank._rectangle.Height / 2f), Vector2.One, SpriteEffects.None, 0);
        
        _spriteBatch.Draw(_enemyTank._bulletTexture, _enemyTank._bulletPosition, _enemyTank._bulletRectangle, 
            Color.White, _enemyTank._bulletRotation, new Vector2(_enemyTank._bulletRectangle.Width/2f, _enemyTank._bulletRectangle.Height/2f), 
            Vector2.One, SpriteEffects.None, 0);

        _spriteBatch.Draw(_enemyTank._upPartTexture,
            _enemyTank._position + _enemyTank.GetOffsetWithRotation(), _enemyTank._upPartRectangle, Color.White, _enemyTank._upPartRotation, 
            new Vector2(-10, _enemyTank._upPartRectangle.Height / 2f), Vector2.One, SpriteEffects.None, 0);
        
        _spriteBatch.End();
        
        base.Draw(gameTime);
    }
}