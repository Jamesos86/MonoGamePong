using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct2D1.Effects;
using System;

namespace MonoGamePong
{
    
    
    
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        Texture2D whiteTexture;

        int screenWidth = 960;
        int screenHeight = 540;

        Rectangle leftPaddle;
        Rectangle rightPaddle;
        Rectangle ball;

        Rectangle midline;
        
        int paddleWidth = 16;
        int paddleHeight = 110;
        int ballSize = 14;

        int midlineWidth = 1;

        float delayTime = 2;
        float currentCooldown = 0;
        bool isOnCooldown = true;


        KeyboardState currentKeyboardState;

        float paddleSpeed = 420f;
        Vector2 ballVelocity = new Vector2(450, 450);

        int leftScore = 0;
        int rightScore = 0;

        SpriteFont font;
        string outputText;


        float cpuDeadZone = 10;
        


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.PreferredBackBufferHeight = screenHeight;

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            leftPaddle = new Rectangle(40, (screenHeight / 2) - (paddleHeight), paddleWidth, paddleHeight);
            rightPaddle = new Rectangle(screenWidth - (40 + paddleWidth), (screenHeight / 2) - (paddleHeight), paddleWidth, paddleHeight);
            ball = new Rectangle((screenWidth/2)-(ballSize/2), (screenHeight/2)-(ballSize/2), ballSize, ballSize);

            midline = new Rectangle((screenWidth / 2), (0), (midlineWidth),(screenHeight));

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            whiteTexture = new Texture2D(GraphicsDevice, 1, 1);
            Color[] pixelColor = { Color.White };
            whiteTexture.SetData(pixelColor);

            font = Content.Load<SpriteFont>("font");
         
        }

        protected override void Update(GameTime gameTime)
        {
            currentKeyboardState = Keyboard.GetState();

            if (currentKeyboardState.IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (isOnCooldown)
            {
                currentCooldown += deltaTime;
                if (currentCooldown >= delayTime)
                {
                    currentCooldown = 0;
                    isOnCooldown = false;
                }
            }
            else
            {


                if (currentKeyboardState.IsKeyDown(Keys.W))
                {
                    leftPaddle.Y -= (int)(paddleSpeed * deltaTime);
                }
                if (currentKeyboardState.IsKeyDown(Keys.S))
                {
                    leftPaddle.Y += (int)(paddleSpeed * deltaTime);
                }

                if (currentKeyboardState.IsKeyDown(Keys.Up))
                {
                    rightPaddle.Y -= (int)(paddleSpeed * deltaTime);
                }
                if (currentKeyboardState.IsKeyDown(Keys.Down))
                {
                    rightPaddle.Y += (int)(paddleSpeed * deltaTime);
                }

                leftPaddle.Y = MathHelper.Clamp(leftPaddle.Y, 0, screenHeight - leftPaddle.Height);
                rightPaddle.Y = MathHelper.Clamp(rightPaddle.Y, 0, screenHeight - rightPaddle.Height);

                ball.X += (int)(ballVelocity.X * deltaTime);
                ball.Y += (int)(ballVelocity.Y * deltaTime);

                if (ball.Top <= 0)
                {
                    ball.Y = 0;
                    ballVelocity.Y *= -1f;
                }
                else if (ball.Bottom >= screenHeight)
                {
                    ball.Y = screenHeight - ball.Height;
                    ballVelocity.Y *= -1f;
                }

                if (ball.Intersects(leftPaddle) && ballVelocity.X < 0f)
                {
                    ball.X = leftPaddle.Right;
                    ballVelocity.X *= -1f;
                    ballVelocity.Y *= 1f;
                    if (ballVelocity.X < 1000)
                    {
                        ballVelocity.X *= 1.1f;
                    }
                    if (ballVelocity.Y < 1000)
                    {
                        ballVelocity.Y *= 1.1f;
                    }
                    if (leftPaddle.Height > 10)
                    {
                        leftPaddle.Height -= 10;
                    }


                }
                else if (ball.Intersects(rightPaddle) && ballVelocity.X > 0f)
                {
                    ball.X = rightPaddle.Left - ball.Width;
                    ballVelocity.X *= -1f;
                    ballVelocity.Y *= 1f;
                    if (ballVelocity.X < 1000)
                    {
                        ballVelocity.X *= 1.1f;
                    }
                    if (ballVelocity.Y < 1000)
                    {
                        ballVelocity.Y *= 1.1f;
                    }
                    if (rightPaddle.Height > 10)
                    {
                        rightPaddle.Height -= 10;
                    }

                }

                if (ball.Right < 0)
                {
                    ResetBall();
                    rightScore++;

                }
                else if (ball.Left > screenWidth)
                {
                    ResetBall();
                    leftScore++;

                }
            }
                outputText = $"P1: {leftScore} --- P2: {rightScore}";

                //CPU PLAYER - Right Paddle - UNBEATABLE
                //rightPaddle.Y = ball.Y;

                //CPU PLAYER - Right Paddle - FAIR
                if (ball.X >= screenWidth / 2)
                {
                    float paddleCenterY = rightPaddle.Y + rightPaddle.Height / 2;
                    float ballCenterY = ball.Y + ball.Height / 2;

                    float diff = ballCenterY - paddleCenterY;
                    if (Math.Abs(diff) > cpuDeadZone)
                    {
                        if (diff > 0) { rightPaddle.Y += (int)(paddleSpeed * deltaTime); }
                        else { rightPaddle.Y -= (int)(paddleSpeed * deltaTime); }
                    }
                }



            

                base.Update(gameTime);
        }

        private void ResetBall()
        {
            Initialize();
            isOnCooldown = true;
            
            ball.X = (screenWidth / 2) - (ball.Width / 2);
            ball.Y = (screenHeight / 2) - (ball.Height / 2);

            

            leftPaddle.Height = paddleHeight;
            rightPaddle.Height = paddleHeight;


            ballVelocity.X = -ballVelocity.X;

            ballVelocity.X = 450;
            ballVelocity.Y = 450;

        }

        // comment
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            spriteBatch.Draw(whiteTexture, leftPaddle, Color.White);
            spriteBatch.Draw(whiteTexture, rightPaddle, Color.White);
            spriteBatch.Draw(whiteTexture, ball, Color.White);

            spriteBatch.Draw(whiteTexture, midline, Color.White);

            float fontsize = font.MeasureString(outputText).X;
            spriteBatch.DrawString(font, outputText, new Vector2((screenWidth/2) - (fontsize/2), 0), Color.White);


            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
