using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
        
        
        int paddleWidth = 16;
        int paddleHeight = 110;
        int ballSize = 14;

        KeyboardState currentKeyboardState;

        float paddleSpeed = 420f;
        Vector2 ballVelocity = new Vector2(300, 300);

        int leftScore = 0;
        int rightScore = 0;

        SpriteFont font;
        string outputText;

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
            rightPaddle.Y = MathHelper.Clamp(rightPaddle.Y, 0, screenHeight -  rightPaddle.Height);

            ball.X += (int)(ballVelocity.X * deltaTime);
            ball.Y += (int)(ballVelocity.Y * deltaTime);

            if(ball.Top <= 0)
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
            }
            else if (ball.Intersects(rightPaddle) && ballVelocity.X > 0f)
            {
                ball.X = rightPaddle.Left - ball.Width;
                ballVelocity.X *= -1f;
            }

            if(ball.Right < 0)
            {
                ResetBall();
                rightScore += 1;
            }
            else if(ball.Left > screenWidth)
            {
                ResetBall();
                leftScore += 1;
            }

            outputText = $"P1: {leftScore} --- P2: {rightScore}";

                base.Update(gameTime);
        }

        private void ResetBall()
        {
            ball.X = (screenWidth / 2) - (ball.Width / 2);
            ball.Y = (screenHeight / 2) - (ball.Height / 2);

            ballVelocity.X = -ballVelocity.X;


        }

        // comment
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            spriteBatch.Draw(whiteTexture, leftPaddle, Color.White);
            spriteBatch.Draw(whiteTexture, rightPaddle, Color.White);
            spriteBatch.Draw(whiteTexture, ball, Color.White);


            float fontsize = font.MeasureString(outputText).X;
            spriteBatch.DrawString(font, outputText, new Vector2((screenWidth/2) - (fontsize/2), 0), Color.White);


            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
