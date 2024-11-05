using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct3D9;

namespace TankyProjekt;

public class EnemyTank
{
    public Texture2D _texture { get; private set; }
    public Vector2 _position;
    public float _speed { get; private set; }
    public Rectangle _rectangle { get; private set; }
    
    public Rectangle _upPartRectangle { get; private set; }
    public float _upPartRotation { get; private set; }
    public Texture2D _upPartTexture { get; private set; }
    public Vector2 _offSet { get; private set; }
    
    public Texture2D _bulletTexture { get; private set; }
    public Vector2 _bulletPosition { get; private set; }
    public Rectangle _bulletRectangle { get; private set; }
    public float _bulletRotation { get; private set; }
    public float _bulletSpeed { get; private set; }

    private Game1 _game1;

    private Timer _timer;
    
    private bool _pausedMovement;

    private float _wantedRotation;
    public float _rotation { get; private set; }
    
    public EnemyTank(Game1 game1)
    {
        
        _position = new Vector2(600, 300);
        _game1 = game1;
        _bulletRectangle = new Rectangle(0, 0, 25, 12);
        _upPartRectangle = new Rectangle(0, 0, 75, 12);
        _rectangle = new Rectangle(0, 0, 75, 37); 
        _bulletPosition = new Vector2(-1000, -1000);
        _speed = 5;
        _bulletSpeed = 10;
        _bulletRotation = 180;
        Wait();
        _offSet = new Vector2(0, _rectangle.Height / 2f);
    }

    public void LoadTexture()
    {
        _texture = new Texture2D(_game1.GraphicsDevice, 1, 1);
        _texture.SetData([Color.White]);

        _upPartTexture = new Texture2D(_game1.GraphicsDevice, 1, 1);
        _upPartTexture.SetData([Color.Orange]);
        
        _bulletTexture = new Texture2D(_game1.GraphicsDevice, 1, 1);
        _bulletTexture.SetData([Color.Black]);
    }

    public void Update(GameTime gameTime)
    {
        _timer.Update(gameTime);
        Move();
        RotateUp();
        Shoot();
        UpdateBullet();
    }

    private void Wait()
    {
        _pausedMovement = true;
        _timer = new Timer(1, RandomRotate);
    }

    private void RandomRotate()
    {
        Console.WriteLine("Random rotate");
        _pausedMovement = false;
        _wantedRotation = new Random().Next(0, 628)/100f;
        _timer = new Timer(3, Wait);
    }

    private void Move()
    {
        if (!_pausedMovement)
        {
            _position += new Vector2((float)Math.Cos(_rotation), (float)Math.Sin(_rotation));

            _position.X = MathHelper.Clamp(_position.X, 0, _game1.GraphicsDevice.Viewport.Width - _rectangle.Width);
            _position.Y = MathHelper.Clamp(_position.Y, 0, _game1.GraphicsDevice.Viewport.Height - _rectangle.Height);
        }
        else
        {
            if (Math.Abs(_rotation - _wantedRotation) > 0.1)
            {
                if (_rotation > _wantedRotation)
                {
                    _rotation -= Math.Min(0.05f, Math.Abs(_rotation - _wantedRotation));
                    _upPartRotation -= Math.Min(0.05f, Math.Abs(_rotation - _wantedRotation));
                }
                else
                {
                    _rotation -= Math.Min(0.05f, Math.Abs(_rotation - _wantedRotation));
                    _upPartRotation -= Math.Min(0.05f, Math.Abs(_rotation - _wantedRotation));
                }
            }
        }
    }
    
    public Vector2 GetOffsetWithRotation()
    {
        float offsetX = (float)Math.Cos(_rotation) * _offSet.X; 
        float offsetY = (float)Math.Sin(_rotation) * _offSet.Y;

        return new Vector2(offsetX, offsetY);
    }
    
    private void RotateUp()
    {
        if(!_pausedMovement) {
            Vector2 start = _position + _offSet;
            Vector2 direction = _game1._position - start;
            _upPartRotation = (float)Math.Atan2(direction.Y, direction.X);
        }
    }
    
    private void Shoot()
    {
        if (_game1.isAwayFromScreen(_bulletPosition) && !_pausedMovement)
        {
            _bulletPosition = (_position + _offSet) + (_upPartRectangle.Width / 2f) * new Vector2((float)Math.Cos(_upPartRotation), (float)Math.Sin(_upPartRotation));
            _bulletRotation = _upPartRotation;
        }
    }
    
    private void UpdateBullet()
    {
        _bulletPosition += new Vector2((float) Math.Cos(_bulletRotation) * _bulletSpeed, (float) Math.Sin(_bulletRotation) * _bulletSpeed);
        if (new Rectangle((int)_bulletPosition.X, (int)_bulletPosition.Y, _rectangle.Width, _rectangle.Height)
            .Intersects(new Rectangle((int)_game1._position.X, (int)_game1._position.Y, _game1._rectangle.Width,
                _game1._rectangle.Height)))
        {
            Console.WriteLine("Prohra");
            Environment.Exit(0);
        }
    }
}