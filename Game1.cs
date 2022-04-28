using System;
using System.IO;
using System.Text;
using System.Threading;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;


namespace FEI_MAN_Lotz
{
    public class Game1 : Game
    {

        /// <summary>
        ///  Deklarace proměnných 
        /// </summary>
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;


        Random rd = new Random();      

        Texture2D FEI_MAN;
        Texture2D FEI_MAN_DONE;
        Texture2D Background;
        Texture2D Wall;
        Texture2D Credit;
        Texture2D Ghost;
        Texture2D Ghost_2;
        Texture2D Ghost_3;

        Texture2D FEI_BUILD;
        Texture2D Boss;

        const int Grid = 40;
        const int Height = 20;
        const int Widht = 19;

        Rectangle FEI_MAN_Position = new Rectangle(8*Grid, Grid, Grid, Grid);
        Rectangle FEI_MAN_Done_Position = new Rectangle((int)(Widht*Grid/2f)-5*Grid, (int)(Height * Grid / 2f) - 5 * Grid, 10*Grid, 10*Grid);
        Rectangle Wall_Position = new Rectangle(0, 0, Grid, Grid);
        Rectangle Credit_Position = new Rectangle(0, 0, Grid/2, Grid/2);
        Rectangle Ghost_Position = new Rectangle(9*Grid,10*Grid, Grid, Grid);
        Rectangle Ghost_2_Position = new Rectangle(9*Grid, 11*Grid, Grid, Grid);
        Rectangle Ghost_3_Position = new Rectangle(9 * Grid, 10 * Grid, Grid, Grid);
        Rectangle Boss_Position = new Rectangle(9 * Grid, 10 * Grid, Grid, Grid);


        Rectangle BackGround = new Rectangle(0, 0, (Widht+1) * Grid, (Height+1) * Grid);

        Vector2 Load_Pos = new Vector2(300, 300);

        SpriteFont Font;

        SoundEffect Coin;

        MouseState Yenkee;

        AntiFreeze Ghost_Freeze_1 = new AntiFreeze { ActualX = 0, ActualY = 0, LastX = 0, LastY = 0 };
        AntiFreeze Ghost_Freeze_2 = new AntiFreeze { ActualX = 0, ActualY = 0, LastX = 0, LastY = 0 };
        AntiFreeze Ghost_Freeze_3 = new AntiFreeze { ActualX = 0, ActualY = 0, LastX = 0, LastY = 0 };
        AntiFreeze Boss_Freeze = new AntiFreeze { ActualX = 0, ActualY = 0, LastX = 0, LastY = 0 };

        int UlozX = 0;
        int UlozY = 0;
        int Direction = 4;

        int Score = 0;
        int Max_Lvl_Score;

        int Screen = 0;
        int k = 0;

        string path = @"C:\Users\bohum\source\repos\FEI_MAN_Lotz\Content\stats.txt";

        int Ghost_Direction = 3;
        int Ghost_2_Direction = 1;
        int Ghost_3_Direction = 2;
        int Boss_Direction = 0;

        int PlayerSpeed = (int)(Grid/8f);

        int GhostSpeed = (int)(Grid/16f);
        int Ghost2Speed = (int)(Grid/10);
        int Ghost3Speed = (int)(Grid / 10);
        int BossSpeed = (int)(Grid / 5);


        int[,] Lvl1 = new int[Widht, Height] {
                                             {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                                             {1,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,1},
                                             {1,0,1,1,0,1,1,1,0,1,1,0,1,1,1,0,1,1,0,1},
                                             {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
                                             {1,0,1,1,0,1,0,1,1,1,1,1,1,0,1,0,1,1,0,1},
                                             {1,0,0,0,0,1,0,0,0,1,1,0,0,0,1,0,0,0,0,1},
                                             {1,1,1,1,0,1,1,1,0,1,1,0,1,1,1,0,1,1,1,1},
                                             {1,1,1,1,0,1,0,0,0,0,0,0,0,0,1,0,1,1,1,1},
                                             {0,0,0,0,0,0,0,1,1,0,0,1,1,0,0,0,0,0,0,0},
                                             {1,1,1,1,0,1,0,1,0,0,0,0,1,0,1,0,1,1,1,1},
                                             {1,1,1,1,0,1,0,1,0,1,1,0,1,0,1,0,1,1,1,1},
                                             {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
                                             {1,0,1,1,0,1,1,1,0,1,1,0,1,1,1,0,1,1,0,1},
                                             {1,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,1,0,2,1},
                                             {1,1,0,1,0,1,0,1,1,1,1,1,1,0,1,0,1,0,1,1},
                                             {1,0,0,0,0,1,0,0,0,1,1,0,0,0,1,0,0,0,0,1},
                                             {1,3,1,1,1,1,1,1,0,1,1,0,1,1,1,1,1,1,0,1},
                                             {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
                                             {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}};

        int[,] Lvl2 = new int[Widht, Height] {
                                             {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                                             {1,0,0,0,0,1,0,0,0,1,1,0,0,0,1,0,0,0,0,1},
                                             {1,1,1,1,0,1,1,1,0,1,1,0,1,1,1,0,1,1,1,1},
                                             {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
                                             {1,0,1,1,0,1,0,1,1,1,1,1,1,0,1,0,1,1,0,1},
                                             {1,0,0,1,0,1,0,0,0,1,1,0,0,0,1,0,1,0,0,1},
                                             {1,1,1,1,0,1,1,1,0,1,1,0,1,1,1,0,1,1,1,1},
                                             {1,1,1,1,0,1,0,0,0,0,0,0,0,0,1,0,1,1,1,1},
                                             {0,0,0,0,0,0,0,1,1,0,0,1,1,0,0,0,0,0,0,0},
                                             {1,1,1,1,0,1,0,1,0,0,0,0,1,0,1,0,1,1,1,1},
                                             {1,1,1,1,0,1,0,1,0,1,1,0,1,0,1,0,1,1,1,1},
                                             {1,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,1},
                                             {1,0,1,1,0,1,1,1,0,1,1,0,1,1,1,0,1,1,0,1},
                                             {1,0,0,1,0,0,0,1,0,0,0,0,1,0,0,0,1,0,0,1},
                                             {1,1,0,1,0,1,0,1,1,1,1,1,1,0,1,0,1,0,1,1},
                                             {1,0,0,1,0,1,0,0,0,1,1,0,0,0,1,0,1,0,0,1},
                                             {1,1,1,1,1,1,1,1,0,1,1,0,1,1,1,1,1,1,1,1},
                                             {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
                                             {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}};

        int[,] Lvl3 = new int[Widht, Height] {
                                             {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                                             {1,0,0,1,0,0,0,0,0,1,1,0,0,0,0,0,1,0,0,1},
                                             {1,0,1,1,0,1,1,1,0,1,1,0,1,1,1,0,1,1,0,1},
                                             {1,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,1},
                                             {1,0,1,1,0,1,0,1,1,1,1,1,1,0,1,0,1,1,0,1},
                                             {1,0,1,0,0,1,0,0,0,1,1,0,0,0,1,0,0,1,0,1},
                                             {1,0,0,1,0,1,1,1,0,1,1,0,1,1,1,0,1,0,0,1},
                                             {1,1,0,1,1,1,0,0,0,0,0,0,0,0,1,1,1,0,1,1},
                                             {0,0,0,0,0,0,0,1,1,0,0,1,1,0,0,0,0,0,0,0},
                                             {1,1,1,1,0,1,0,1,0,0,0,0,1,0,1,0,1,1,1,1},
                                             {1,0,1,1,0,1,0,1,0,1,1,0,1,0,1,0,1,1,0,1},
                                             {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
                                             {1,0,1,1,0,1,1,1,0,1,1,0,1,1,1,0,1,1,0,1},
                                             {1,0,0,1,0,0,0,0,0,1,1,0,0,0,0,0,1,0,0,1},
                                             {1,1,0,1,0,1,0,1,1,1,1,1,1,0,1,0,1,0,1,1},
                                             {1,0,0,1,0,1,0,0,0,1,1,0,0,0,1,0,1,0,0,1},
                                             {1,0,1,1,1,1,1,1,0,1,1,0,1,1,1,1,1,1,0,1},
                                             {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
                                             {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}};

        int[,] CoinM = new int[Widht, Height] {
                                             {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                             {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                             {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                             {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                             {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                             {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                             {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                             {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                             {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                             {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                             {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                             {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                             {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                             {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                             {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                             {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                             {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                             {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                                             {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}};

        int[,] Mapa = new int[Widht, Height];
       
        /// <summary>
        ///  Třída založena pro korektnímu pohybu duchů
        /// </summary>
        public class AntiFreeze
        {
            public int LastX { get; set; }
            public int LastY { get; set; }
            public int ActualX { get; set; }
            public int ActualY { get; set; }
        }

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {

            base.Initialize();

            _graphics.PreferredBackBufferWidth = Height*Grid;
            _graphics.PreferredBackBufferHeight = Widht*Grid;
            _graphics.ApplyChanges();

        }

        /// <summary>
        ///  Načtení Content souborů
        /// </summary>

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Background = Content.Load<Texture2D>("Background");

            FEI_MAN = Content.Load<Texture2D>("FEI_MAN");
            FEI_MAN_DONE = Content.Load<Texture2D>("FEI_MAN_DONE");
            Ghost_3 = Content.Load<Texture2D>("Ghost_3");
            Ghost_2 = Content.Load<Texture2D>("Ghost_2");
            Ghost = Content.Load<Texture2D>("Ghost");
            Boss = Content.Load<Texture2D>("Boss");

            Wall = Content.Load<Texture2D>("Wall");
            FEI_BUILD = Content.Load<Texture2D>("FEI_BUILDING");
            Credit = Content.Load<Texture2D>("FEI_COIN");

            Coin = Content.Load<SoundEffect>("Coin");
   
            Font = Content.Load<SpriteFont>("Font");

          
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            if ((((Math.Abs(FEI_MAN_Position.X - Ghost_Position.X) < Grid / 2) && (FEI_MAN_Position.Y == Ghost_Position.Y)) || ((FEI_MAN_Position.X == Ghost_Position.X) && Math.Abs(FEI_MAN_Position.Y - Ghost_Position.Y) < Grid / 2))
            || (((Math.Abs(FEI_MAN_Position.X - Ghost_2_Position.X) < Grid / 2) && (FEI_MAN_Position.Y == Ghost_2_Position.Y)) || ((FEI_MAN_Position.X == Ghost_2_Position.X) && Math.Abs(FEI_MAN_Position.Y - Ghost_2_Position.Y) < Grid / 2))
            || (((Math.Abs(FEI_MAN_Position.X - Ghost_3_Position.X) < Grid / 2) && (FEI_MAN_Position.Y == Ghost_3_Position.Y)) || ((FEI_MAN_Position.X == Ghost_3_Position.X) && Math.Abs(FEI_MAN_Position.Y - Ghost_3_Position.Y) < Grid / 2))
            || (((Math.Abs(FEI_MAN_Position.X - Boss_Position.X) < Grid / 2) && (FEI_MAN_Position.Y == Boss_Position.Y)) || ((FEI_MAN_Position.X == Boss_Position.X) && Math.Abs(FEI_MAN_Position.Y - Boss_Position.Y) < Grid / 2)))
            {
                Screen = 0;
                FEI_MAN_Position.X = 8 * Grid;
                FEI_MAN_Position.Y = Grid;
                Ghost_Position.X = 9 * Grid;
                Ghost_Position.Y = 10 * Grid;
                Ghost_2_Position.X = 9 * Grid;
                Ghost_2_Position.Y = 11 * Grid;
                Ghost_3_Position.X = 9 * Grid;
                Ghost_3_Position.Y = 11 * Grid;
                Boss_Position.X = 9 * Grid;
                Boss_Position.Y = 11 * Grid;
                Direction = 4;
            }


            Yenkee = Mouse.GetState();
            if ((Screen == 0) && ((Yenkee.Position.X >= 500) && (Yenkee.Position.Y >= 250)) && (Yenkee.Position.X <= 565) && (Yenkee.Position.Y <= 275) && (Yenkee.LeftButton == ButtonState.Pressed)) { Screen++; };
            if ((Screen == 0) && (Yenkee.Position.X >= 350) && (Yenkee.Position.Y >= 325) && (Yenkee.Position.X <= 415) && (Yenkee.Position.Y <= 350) && (Yenkee.LeftButton == ButtonState.Pressed)) Screen = 10;
            if ((Screen == 0) && (Yenkee.Position.X >= 200) && (Yenkee.Position.Y >= 400) && (Yenkee.Position.X <= 300) && (Yenkee.Position.Y <= 450) && (Yenkee.LeftButton == ButtonState.Pressed)) Exit();
            if ((Screen == 1)||(Screen==2)||(Screen == 3))
            {
                Pohyb();
                Ghost_Pohyb();
                Ghost_2_Pohyb();
                if (Screen >= 2) { Ghost_3_Pohyb(); GhostSpeed = (int)(Grid / 10); }
                if (Screen == 3) Boss_Pohyb();
                
            }
            
            base.Update(gameTime);
        }

        /// <summary>
        ///  Vykreslovani jednotlivych screenů
        /// </summary>
      
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();
            
            if (Screen == 10)
            {
                
                string Load = File.ReadAllText(path);

                
                _spriteBatch.Draw(FEI_BUILD, BackGround, Color.White);
                _spriteBatch.DrawString(Font,Load, Load_Pos, Color.Black);
                _spriteBatch.DrawString(Font, "Back", new Vector2(300, 530), Color.Black);

                if ((Yenkee.Position.X >= 300) && (Yenkee.Position.Y >= 300) && (Yenkee.Position.X <= 370) && (Yenkee.Position.Y < 335) && (Yenkee.LeftButton == ButtonState.Pressed) && (k >= 1)) Screen = 1;
                if ((Yenkee.Position.X >= 300) && (Yenkee.Position.Y >= 335) && (Yenkee.Position.X <= 370) && (Yenkee.Position.Y < 370) && (Yenkee.LeftButton == ButtonState.Pressed) && (k >= 2 )) Screen = 2;
                if ((Yenkee.Position.X >= 300) && (Yenkee.Position.Y >= 370) && (Yenkee.Position.X <= 370) && (Yenkee.Position.Y < 405) && (Yenkee.LeftButton == ButtonState.Pressed) && (k >= 3 )) Screen = 3;

                if ((Yenkee.Position.X >= 300) && (Yenkee.Position.Y >= 530) && (Yenkee.Position.X <= 370) && (Yenkee.Position.Y < 560) && (Yenkee.LeftButton == ButtonState.Pressed)) Screen = 0;


            }

            if (Screen == 0)
            {
                string Load = File.ReadAllText(path);
                int a = Load.Length;
                if (a > 10) k = 3;
                else if (a > 5) k = 2;
                else if (a > 1) k = 1;

                int C = 0;

                Mapa = Lvl1;
                Max_Lvl_Score = 40;
                Score = 0;
                for (int i = 4; i < Widht; i += 2)    // Radky
                {
                    for (int j = 1; j < Height-1; j++)    //Sloupce
                    {
                        if ((Mapa[i, j] == 0)&&(C<Max_Lvl_Score))
                        {
                            CoinM[i, j] = 2;
                            C++;
                         
                        }
                    }
                }

                _spriteBatch.Draw(Wall, Wall_Position, Color.White);
                            
                _spriteBatch.Draw(FEI_BUILD, BackGround, Color.White);
                _spriteBatch.DrawString(Font, "Start", new Vector2(500,250) ,Color.Black);
                _spriteBatch.DrawString(Font, "Level", new Vector2(350, 325), Color.Black);
                _spriteBatch.DrawString(Font, "End", new Vector2(200, 400), Color.Black);
             
            }
            if (Screen == 1)
            {
                
                GhostSpeed = (int)(Grid / 16f);
                _spriteBatch.Draw(Background, Vector2.Zero, Color.White);
                _spriteBatch.Draw(FEI_MAN, FEI_MAN_Position, Color.White);

                _spriteBatch.Draw(Ghost, Ghost_Position, Color.White);
                _spriteBatch.Draw(Ghost_2, Ghost_2_Position, Color.White);

            for (int i = 0; i < Widht; i++)    // Radky
            {
                for (int j = 0; j < Height; j++)    //Sloupce
                {
                    if (Mapa[i, j] == 1)
                    {
                        Wall_Position.X = (j * Grid);
                        Wall_Position.Y = (i * Grid);


                        _spriteBatch.Draw(Wall, Wall_Position, Color.White);
                    }
                    if (CoinM[i, j] == 2)
                    {
                        Credit_Position.X = (j * Grid) + 12;
                        Credit_Position.Y = (i * Grid) + 12;

                        _spriteBatch.Draw(Credit, Credit_Position, Color.White);
                    }
                   
                }

            }

            }
            if (Screen == 2)
            {
                Mapa = Lvl2;
                
                _spriteBatch.Draw(Background, Vector2.Zero, Color.White);
                _spriteBatch.Draw(FEI_MAN, FEI_MAN_Position, Color.White);

                _spriteBatch.Draw(Ghost, Ghost_Position, Color.White);
                _spriteBatch.Draw(Ghost_2, Ghost_2_Position, Color.White);
                _spriteBatch.Draw(Ghost_3, Ghost_3_Position, Color.White);

                for (int i = 0; i < Widht; i++)    // Radky
                {
                    for (int j = 0; j < Height; j++)    //Sloupce
                    {
                        if (Mapa[i, j] == 1)
                        {
                            Wall_Position.X = (j * Grid);
                            Wall_Position.Y = (i * Grid);


                            _spriteBatch.Draw(Wall, Wall_Position, Color.White);
                        }
                        if (CoinM[i, j] == 2)
                        {
                            Credit_Position.X = (j * Grid) + 12;
                            Credit_Position.Y = (i * Grid) + 12;


                            _spriteBatch.Draw(Credit, Credit_Position, Color.White);
                        }
                       
                        
                    }

                }
            }
            if (Screen == 3)
            {
                Mapa = Lvl3;
                           
                _spriteBatch.Draw(Background, Vector2.Zero, Color.White);
                _spriteBatch.Draw(FEI_MAN, FEI_MAN_Position, Color.White);

                _spriteBatch.Draw(Ghost, Ghost_Position, Color.White);
                _spriteBatch.Draw(Ghost_2, Ghost_2_Position, Color.White);
                _spriteBatch.Draw(Ghost_3, Ghost_3_Position, Color.White);
                _spriteBatch.Draw(Boss, Boss_Position, Color.White);

                for (int i = 0; i < Widht; i++)    // Radky
                {
                    for (int j = 0; j < Height; j++)    //Sloupce
                    {
                        if (Mapa[i, j] == 1)
                        {
                            Wall_Position.X = (j * Grid);
                            Wall_Position.Y = (i * Grid);


                            _spriteBatch.Draw(Wall, Wall_Position, Color.White);
                        }
                        if (CoinM[i, j] == 2)
                        {
                            Credit_Position.X = (j * Grid) + 12;
                            Credit_Position.Y = (i * Grid) + 12;


                            _spriteBatch.Draw(Credit, Credit_Position, Color.White);
                        }
                     
                    }

                }
            }
            if (Screen == 4)
            {
                _spriteBatch.Draw(Background, Vector2.Zero, Color.White);
                _spriteBatch.DrawString(Font, "Winner", new Vector2((Grid * Widht / 2f)-Grid/2, (Grid * Height / 2f) - 300), Color.Black);
                _spriteBatch.Draw(FEI_MAN_DONE, FEI_MAN_Done_Position, Color.White);
                Thread.Sleep(2000);
                Screen = 10;
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        void Pohyb()
        {
            
            if ((FEI_MAN_Position.X % Grid == 0) && (FEI_MAN_Position.Y % Grid == 0))
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Right)) Direction = 0;
                if (Keyboard.GetState().IsKeyDown(Keys.Down)) Direction = 1;
                if (Keyboard.GetState().IsKeyDown(Keys.Left)) Direction = 2;
                if (Keyboard.GetState().IsKeyDown(Keys.Up)) Direction = 3;
            }

            switch (Direction)
            {
                case 0:     // Pravo

                    if ((FEI_MAN_Position.X > ((Widht - 1) * Grid)+20) && (FEI_MAN_Position.Y == 8 * Grid)) { FEI_MAN_Position.X = 40; break; }

                    UlozX = FEI_MAN_Position.X / Grid;
                    UlozY = FEI_MAN_Position.Y / Grid;
                    if (Mapa[UlozY, UlozX + 1] == 1)
                    {
                        FEI_MAN_Position.X -= PlayerSpeed;
                    }
                    FEI_MAN_Position.X += PlayerSpeed;
                    if ((FEI_MAN_Position.X > 945) && (FEI_MAN_Position.Y == 400))
                        FEI_MAN_Position.X = 10;
                    break;
                case 1:     // Dolu

                    UlozX = FEI_MAN_Position.X / Grid;
                    UlozY = FEI_MAN_Position.Y / Grid;
                    if (Mapa[UlozY + 1, UlozX] == 1)
                    {
                        FEI_MAN_Position.Y -= PlayerSpeed;
                    }
                    FEI_MAN_Position.Y += PlayerSpeed;
                    break;
                case 2:     // Levo

                    if ((FEI_MAN_Position.X < 20) && (FEI_MAN_Position.Y == 8 * Grid)) { FEI_MAN_Position.X = ((Widht-1)*Grid)+20; break; }
                    FEI_MAN_Position.X -= PlayerSpeed;
                    UlozX = FEI_MAN_Position.X / Grid;
                    UlozY = FEI_MAN_Position.Y / Grid;
                    if (Mapa[UlozY, UlozX] == 1)
                    {
                        FEI_MAN_Position.X += PlayerSpeed;
                    }
                    if ((FEI_MAN_Position.X < Grid) && (FEI_MAN_Position.Y == 400))
                        FEI_MAN_Position.X = 940;
                    break;
                case 3:     // Nahoru

                    FEI_MAN_Position.Y -= PlayerSpeed;
                    UlozX = FEI_MAN_Position.X / Grid;
                    UlozY = FEI_MAN_Position.Y / Grid;
                    if (Mapa[UlozY, UlozX] == 1)
                    {
                        FEI_MAN_Position.Y += PlayerSpeed;
                    }
                    break;
                default: break;
            }
            if (CoinM[UlozY, UlozX] == 2)
            {
                CoinM[UlozY, UlozX] = 0;
               
                Score++;
                Coin.Play();
            }
            if (Score == Max_Lvl_Score) {
                int A = 0;
                switch (Screen)
                {
                    case 1: A = 1; break;
                    case 2: A = 2; break;
                    case 3: A = 3; break; 
                }
                Screen++; 
                Score = 0;
                if (k<=A) k++;

                switch(k)
                {
                    case 1: File.WriteAllText(path, "Lvl 1"); break;
                    case 2: File.WriteAllText(path, "Lvl 1\nLvl 2"); break;
                    case 3: File.WriteAllText(path, "Lvl 1\nLvl 2\nLvl 3"); break;
                }
                switch (Screen)
                {
                    case 1: Mapa = Lvl1; Max_Lvl_Score = 30; break;
                    case 2: Mapa = Lvl2; Max_Lvl_Score = 40; break;
                    case 3: Mapa = Lvl3; Max_Lvl_Score = 40; break;
                    default: break;
                }
                    
               
                int C = 0;

                for (int i = 2; i < Widht; i += 2)    // Radky
                {
                    for (int j = 2; j < Height - 2; j++)    //Sloupce
                    {
                        if ((Mapa[i, j] == 0) && (C < Max_Lvl_Score))
                        {
                            CoinM[i, j] = 2;
                            C++;

                        }
                    }
                }

                Thread.Sleep(2000);

                FEI_MAN_Position.X = 8 * Grid;
                FEI_MAN_Position.Y = Grid;
                Ghost_Position.X = 9 * Grid;
                Ghost_Position.Y = 10 * Grid;
                Ghost_2_Position.X = 9 * Grid;
                Ghost_2_Position.Y = 11 * Grid;
                Ghost_3_Position.X = 9 * Grid;
                Ghost_3_Position.Y = 11*Grid;
                Boss_Position.X = 9 * Grid;
                Boss_Position.Y = 11 * Grid;
                Direction = 4;
            }

        }

        /// <summary>
        ///  Pohyb duchů je někdy předurčen, někdy náhodný, záleží na směru a typu křižovatky. Nesledují vás jako v klasické hře. 
        /// </summary>
       
        void Ghost_Pohyb()
        {
            if ((Ghost_Freeze_1.ActualX == Ghost_Freeze_1.LastX)&&(Ghost_Freeze_1.ActualY==Ghost_Freeze_1.LastY))
            {
                int Rand = rd.Next(0, 4);
                switch (Rand)
                {
                    case 0: Ghost_Direction = 0; break;
                    case 1: Ghost_Direction = 1; break;
                    case 2: Ghost_Direction = 2; break;
                    case 3: Ghost_Direction = 3; break;
                    default: Ghost_Direction = 0; break;
                }
            }

            Ghost_Freeze_1.ActualX = Ghost_Position.X;
            Ghost_Freeze_1.ActualY = Ghost_Position.Y;

            if ((Ghost_Position.X % Grid == 0) && (Ghost_Position.Y % Grid == 0))
            {
                switch (Ghost_Direction)
                {
                    case 0: // Right
                        if ((Mapa[(int)(Ghost_Position.Y / Grid) - 1, (int)(Ghost_Position.X / Grid)] == 0) && (Mapa[(int)(Ghost_Position.Y / Grid) + 1, (int)(Ghost_Position.X / Grid)] == 0) && (Mapa[(int)(Ghost_Position.Y / Grid), (int)(Ghost_Position.X / Grid) + 1] == 0))
                        {
                            int Rand = rd.Next(0, 3);
                            switch (Rand)
                            {
                                case 0: Ghost_Direction = 1; break;
                                case 1: Ghost_Direction = 3; break;
                                case 2: Ghost_Direction = 0; break;
                                default: Ghost_Direction = 1; break;
                            }
                        }
                        else if ((Mapa[(int)(Ghost_Position.Y / Grid) - 1, (int)(Ghost_Position.X / Grid)] == 0) && (Mapa[(int)(Ghost_Position.Y / Grid) + 1, (int)(Ghost_Position.X / Grid)] == 0))
                        {
                            int Rand = rd.Next(0, 2);
                            switch (Rand)
                            {
                                case 0: Ghost_Direction = 1; break;
                                case 1: Ghost_Direction = 3; break;
                                default: Ghost_Direction = 1; break;
                            }
                        }
                        else if ((Mapa[(int)(Ghost_Position.Y / Grid) - 1, (int)(Ghost_Position.X / Grid)] == 0) || (Mapa[(int)(Ghost_Position.Y / Grid) + 1, (int)(Ghost_Position.X / Grid)] == 0))
                        {
                            if (Mapa[(int)(Ghost_Position.Y / Grid) - 1, (int)(Ghost_Position.X / Grid)] == 0) Ghost_Direction = 3;
                            else Ghost_Direction = 1;
                        }
                        else if (Mapa[(int)(Ghost_Position.Y / Grid), (int)(Ghost_Position.X / Grid) + 1] == 0) Ghost_Direction = 0;
                        else Ghost_Direction = 2;
                        break;

                    case 1: // Down
                        if ((Mapa[(int)(Ghost_Position.Y / Grid), (int)(Ghost_Position.X / Grid) + 1] == 0) && (Mapa[(int)(Ghost_Position.Y / Grid), (int)(Ghost_Position.X / Grid) - 1] == 0) && (Mapa[(int)(Ghost_Position.Y / Grid) + 1, (int)(Ghost_Position.X / Grid)] == 0))
                        {
                            int Rand = rd.Next(0, 4);
                            switch (Rand)
                            {
                                case 0: Ghost_Direction = 0; break;
                                case 1: Ghost_Direction = 1; break;
                                case 2: Ghost_Direction = 2; break;
                                default: Ghost_Direction = 1; break;
                            }
                        }
                        else if ((Mapa[(int)(Ghost_Position.Y / Grid), (int)(Ghost_Position.X / Grid) + 1] == 0) && (Mapa[(int)(Ghost_Position.Y / Grid), (int)(Ghost_Position.X / Grid) - 1] == 0))
                        {
                            int Rand = rd.Next(0, 2);
                            switch (Rand)
                            {
                                case 0: Ghost_Direction = 0; break;
                                case 1: Ghost_Direction = 2; break;
                                default: Ghost_Direction = 0; break;
                            }
                        }
                        else if ((Mapa[(int)(Ghost_Position.Y / Grid), (int)(Ghost_Position.X / Grid) + 1] == 0) || (Mapa[(int)(Ghost_Position.Y / Grid), (int)(Ghost_Position.X / Grid) - 1] == 0))
                        {
                            if (Mapa[(int)(Ghost_Position.Y / Grid), (int)(Ghost_Position.X / Grid) + 1] == 0) Ghost_Direction = 0;
                            else Ghost_Direction = 2;
                        }
                        else if (Mapa[(int)(Ghost_Position.Y / Grid) + 1, (int)(Ghost_Position.X / Grid)] == 0) Ghost_Direction = 1;
                        else Ghost_Direction = 3;
                        break;

                    case 2: //Left
                        if ((Mapa[(int)(Ghost_Position.Y / Grid) - 1, (int)(Ghost_Position.X / Grid)] == 0) && (Mapa[(int)(Ghost_Position.Y / Grid) + 1, (int)(Ghost_Position.X / Grid)] == 0) && (Mapa[(int)(Ghost_Position.Y / Grid), (int)(Ghost_Position.X / Grid) + 1] == 0))
                        {
                            int Rand = rd.Next(0, 3);
                            switch (Rand)
                            {
                                case 0: Ghost_Direction = 1; break;
                                case 1: Ghost_Direction = 3; break;
                                case 2: Ghost_Direction = 2; break;
                                default: Ghost_Direction = 1; break;
                            }
                        }
                        else if ((Mapa[(int)(Ghost_Position.Y / Grid) - 1, (int)(Ghost_Position.X / Grid)] == 0) && (Mapa[(int)(Ghost_Position.Y / Grid) + 1, (int)(Ghost_Position.X / Grid)] == 0))
                        {
                            int Rand = rd.Next(0, 2);
                            switch (Rand)
                            {
                                case 0: Ghost_Direction = 1; break;
                                case 1: Ghost_Direction = 3; break;
                                default: Ghost_Direction = 1; break;
                            }
                        }
                        else if ((Mapa[(int)(Ghost_Position.Y / Grid) - 1, (int)(Ghost_Position.X / Grid)] == 0) || (Mapa[(int)(Ghost_Position.Y / Grid) + 1, (int)(Ghost_Position.X / Grid)] == 0))
                        {
                            if (Mapa[(int)(Ghost_Position.Y / Grid) - 1, (int)(Ghost_Position.X / Grid)] == 0) Ghost_Direction = 3;
                            else Ghost_Direction = 1;
                        }
                        else if (Mapa[(int)(Ghost_Position.Y / Grid), (int)(Ghost_Position.X / Grid) + 1] == 0) Ghost_Direction = 2;
                        else Ghost_Direction = 0;
                        break;

                    case 3: //Up
                        if ((Mapa[(int)(Ghost_Position.Y / Grid), (int)(Ghost_Position.X / Grid) + 1] == 0) && (Mapa[(int)(Ghost_Position.Y / Grid), (int)(Ghost_Position.X / Grid) - 1] == 0) && (Mapa[(int)(Ghost_Position.Y / Grid) + 1, (int)(Ghost_Position.X / Grid)] == 0))
                        {
                            int Rand = rd.Next(0, 4);
                            switch (Rand)
                            {
                                case 0: Ghost_Direction = 0; break;
                                case 1: Ghost_Direction = 3; break;
                                case 2: Ghost_Direction = 2; break;
                                default: Ghost_Direction = 1; break;
                            }
                        }
                        else if ((Mapa[(int)(Ghost_Position.Y / Grid), (int)(Ghost_Position.X / Grid) + 1] == 0) && (Mapa[(int)(Ghost_Position.Y / Grid), (int)(Ghost_Position.X / Grid) - 1] == 0))
                        {
                            int Rand = rd.Next(0, 2);
                            switch (Rand)
                            {
                                case 0: Ghost_Direction = 0; break;
                                case 1: Ghost_Direction = 2; break;
                                default: Ghost_Direction = 0; break;
                            }
                        }
                        else if ((Mapa[(int)(Ghost_Position.Y / Grid), (int)(Ghost_Position.X / Grid) + 1] == 0) || (Mapa[(int)(Ghost_Position.Y / Grid), (int)(Ghost_Position.X / Grid) - 1] == 0))
                        {
                            if (Mapa[(int)(Ghost_Position.Y / Grid), (int)(Ghost_Position.X / Grid) + 1] == 0) Ghost_Direction = 0;
                            else Ghost_Direction = 2;
                        }
                        else if (Mapa[(int)(Ghost_Position.Y / Grid) - 1, (int)(Ghost_Position.X / Grid)] == 0) Ghost_Direction = 3;
                        else Ghost_Direction = 1;
                        break;

                }
            }

            switch (Ghost_Direction)
            {
                case 0:     //Right

                    if ((Ghost_Position.X > ((Widht - 1) * Grid) + 20) && (Ghost_Position.Y == 8 * Grid)) { Ghost_Position.X = 40; break; }
                    if (Mapa[(int)(Ghost_Position.Y / Grid), (int)(Ghost_Position.X / Grid) + 1] == 1)
                    {
                        Ghost_Position.X -= GhostSpeed;

                    }
                    Ghost_Position.X += GhostSpeed;
                    if ((Ghost_Position.X > 945) && (Ghost_Position.Y == 400)) Ghost_Position.X = 10;
                    break;

                case 1:     //Down

                    if (Mapa[(int)(Ghost_Position.Y / Grid) + 1, (int)(Ghost_Position.X / Grid)] == 1)
                    {
                        Ghost_Position.Y -= GhostSpeed;


                    }
                    Ghost_Position.Y += GhostSpeed;
                    break;
                case 2:     //Left
                    if ((Ghost_Position.X < 20) && (Ghost_Position.Y == 8 * Grid)) { Ghost_Position.X = ((Widht - 1) * Grid) + 20; break; }
                    Ghost_Position.X -= GhostSpeed;
                    if (Mapa[(int)(Ghost_Position.Y / Grid), (int)(Ghost_Position.X / Grid)] == 1)
                    {

                        Ghost_Position.X += GhostSpeed;

                    }
                    if ((Ghost_Position.X < 5) && (Ghost_Position.Y == 400))
                        Ghost_Position.X = 940;
                    break;

                case 3:      //Up
                    Ghost_Position.Y -= GhostSpeed;

                    if (Mapa[(int)(Ghost_Position.Y / Grid), (int)(Ghost_Position.X / Grid)] == 1)
                    {
                        Ghost_Position.Y += GhostSpeed;



                    }
                    break;
            }

            Ghost_Freeze_1.LastX = Ghost_Position.X;
            Ghost_Freeze_1.LastY = Ghost_Position.Y;


        }

        void Ghost_2_Pohyb()
        {
            if ((Ghost_Freeze_2.ActualX == Ghost_Freeze_2.LastX) && (Ghost_Freeze_2.ActualY == Ghost_Freeze_2.LastY))
            {
                int Rand = rd.Next(0, 4);
                switch (Rand)
                {
                    case 0: Ghost_2_Direction = 0; break;
                    case 1: Ghost_2_Direction = 1; break;
                    case 2: Ghost_2_Direction = 2; break;
                    case 3: Ghost_2_Direction = 3; break;
                    default: Ghost_2_Direction = 1; break;
                }
            }

            Ghost_Freeze_2.ActualX = Ghost_2_Position.X;
            Ghost_Freeze_2.ActualY = Ghost_2_Position.Y;

            if ((Ghost_2_Position.X % Grid == 0) && (Ghost_2_Position.Y % Grid == 0))
            {
                switch (Ghost_2_Direction)
                {
                    case 0: // Right
                        if ((Mapa[(int)(Ghost_2_Position.Y / Grid) - 1, (int)(Ghost_2_Position.X / Grid)] == 0) && (Mapa[(int)(Ghost_2_Position.Y / Grid) + 1, (int)(Ghost_2_Position.X / Grid)] == 0) && (Mapa[(int)(Ghost_2_Position.Y / Grid), (int)(Ghost_2_Position.X / Grid) + 1] == 0))
                        {
                            int Rand = rd.Next(0, 3);
                            switch (Rand)
                            {
                                case 0: Ghost_2_Direction = 1; break;
                                case 1: Ghost_2_Direction = 3; break;
                                case 2: Ghost_2_Direction = 0; break;
                                default: Ghost_2_Direction = 1; break;
                            }
                        }
                        else if ((Mapa[(int)(Ghost_2_Position.Y / Grid) - 1, (int)(Ghost_2_Position.X / Grid)] == 0) && (Mapa[(int)(Ghost_2_Position.Y / Grid) + 1, (int)(Ghost_2_Position.X / Grid)] == 0))
                        {
                            int Rand = rd.Next(0, 2);
                            switch (Rand)
                            {
                                case 0: Ghost_2_Direction = 1; break;
                                case 1: Ghost_2_Direction = 3; break;
                                default: Ghost_2_Direction = 1; break;
                            }
                        }
                        else if ((Mapa[(int)(Ghost_2_Position.Y / Grid) - 1, (int)(Ghost_2_Position.X / Grid)] == 0) || (Mapa[(int)(Ghost_2_Position.Y / Grid) + 1, (int)(Ghost_2_Position.X / Grid)] == 0))
                        {
                            if (Mapa[(int)(Ghost_2_Position.Y / Grid) - 1, (int)(Ghost_2_Position.X / Grid)] == 0) Ghost_2_Direction = 3;
                            else Ghost_2_Direction = 1;
                        }
                        else if (Mapa[(int)(Ghost_2_Position.Y / Grid), (int)(Ghost_2_Position.X / Grid) + 1] == 0) Ghost_2_Direction = 0;
                        else Ghost_2_Direction = 2;
                        break;

                    case 1: // Down
                        if ((Mapa[(int)(Ghost_2_Position.Y / Grid), (int)(Ghost_2_Position.X / Grid) + 1] == 0) && (Mapa[(int)(Ghost_2_Position.Y / Grid), (int)(Ghost_2_Position.X / Grid) - 1] == 0) && (Mapa[(int)(Ghost_2_Position.Y / Grid) + 1, (int)(Ghost_2_Position.X / Grid)] == 0))
                        {
                            int Rand = rd.Next(0, 4);
                            switch (Rand)
                            {
                                case 0: Ghost_2_Direction = 0; break;
                                case 1: Ghost_2_Direction = 1; break;
                                case 2: Ghost_2_Direction = 2; break;
                                default: Ghost_2_Direction = 1; break;
                            }
                        }
                        else if ((Mapa[(int)(Ghost_2_Position.Y / Grid), (int)(Ghost_2_Position.X / Grid) + 1] == 0) && (Mapa[(int)(Ghost_2_Position.Y / Grid), (int)(Ghost_2_Position.X / Grid) - 1] == 0))
                        {
                            int Rand = rd.Next(0, 2);
                            switch (Rand)
                            {
                                case 0: Ghost_2_Direction = 0; break;
                                case 1: Ghost_2_Direction = 2; break;
                                default: Ghost_2_Direction = 0; break;
                            }
                        }
                        else if ((Mapa[(int)(Ghost_2_Position.Y / Grid), (int)(Ghost_2_Position.X / Grid) + 1] == 0) || (Mapa[(int)(Ghost_2_Position.Y / Grid), (int)(Ghost_2_Position.X / Grid) - 1] == 0))
                        {
                            if (Mapa[(int)(Ghost_2_Position.Y / Grid), (int)(Ghost_2_Position.X / Grid) + 1] == 0) Ghost_2_Direction = 0;
                            else Ghost_2_Direction = 2;
                        }
                        else if (Mapa[(int)(Ghost_2_Position.Y / Grid) + 1, (int)(Ghost_2_Position.X / Grid)] == 0) Ghost_2_Direction = 1;
                        else Ghost_2_Direction = 3;
                        break;

                    case 2: //Left
                        if ((Mapa[(int)(Ghost_2_Position.Y / Grid) - 1, (int)(Ghost_2_Position.X / Grid)] == 0) && (Mapa[(int)(Ghost_2_Position.Y / Grid) + 1, (int)(Ghost_2_Position.X / Grid)] == 0) && (Mapa[(int)(Ghost_2_Position.Y / Grid), (int)(Ghost_2_Position.X / Grid) + 1] == 0))
                        {
                            int Rand = rd.Next(0, 3);
                            switch (Rand)
                            {
                                case 0: Ghost_2_Direction = 1; break;
                                case 1: Ghost_2_Direction = 3; break;
                                case 2: Ghost_2_Direction = 2; break;
                                default: Ghost_2_Direction = 1; break;
                            }
                        }
                        else if ((Mapa[(int)(Ghost_2_Position.Y / Grid) - 1, (int)(Ghost_2_Position.X / Grid)] == 0) && (Mapa[(int)(Ghost_2_Position.Y / Grid) + 1, (int)(Ghost_2_Position.X / Grid)] == 0))
                        {
                            int Rand = rd.Next(0, 2);
                            switch (Rand)
                            {
                                case 0: Ghost_2_Direction = 1; break;
                                case 1: Ghost_2_Direction = 3; break;
                                default: Ghost_2_Direction = 1; break;
                            }
                        }
                        else if ((Mapa[(int)(Ghost_2_Position.Y / Grid) - 1, (int)(Ghost_2_Position.X / Grid)] == 0) || (Mapa[(int)(Ghost_2_Position.Y / Grid) + 1, (int)(Ghost_2_Position.X / Grid)] == 0))
                        {
                            if (Mapa[(int)(Ghost_2_Position.Y / Grid) - 1, (int)(Ghost_2_Position.X / Grid)] == 0) Ghost_2_Direction = 3;
                            else Ghost_2_Direction = 1;
                        }
                        else if (Mapa[(int)(Ghost_2_Position.Y / Grid), (int)(Ghost_2_Position.X / Grid) + 1] == 0) Ghost_2_Direction = 2;
                        else Ghost_2_Direction = 0;
                        break;

                    case 3: //Up
                        if ((Mapa[(int)(Ghost_2_Position.Y / Grid), (int)(Ghost_2_Position.X / Grid) + 1] == 0) && (Mapa[(int)(Ghost_2_Position.Y / Grid), (int)(Ghost_2_Position.X / Grid) - 1] == 0) && (Mapa[(int)(Ghost_2_Position.Y / Grid) + 1, (int)(Ghost_2_Position.X / Grid)] == 0))
                        {
                            int Rand = rd.Next(0, 4);
                            switch (Rand)
                            {
                                case 0: Ghost_2_Direction = 0; break;
                                case 1: Ghost_2_Direction = 3; break;
                                case 2: Ghost_2_Direction = 2; break;
                                default: Ghost_2_Direction = 1; break;
                            }
                        }
                        else if ((Mapa[(int)(Ghost_2_Position.Y / Grid), (int)(Ghost_2_Position.X / Grid) + 1] == 0) && (Mapa[(int)(Ghost_2_Position.Y / Grid), (int)(Ghost_2_Position.X / Grid) - 1] == 0))
                        {
                            int Rand = rd.Next(0, 2);
                            switch (Rand)
                            {
                                case 0: Ghost_2_Direction = 0; break;
                                case 1: Ghost_2_Direction = 2; break;
                                default: Ghost_2_Direction = 0; break;
                            }
                        }
                        else if ((Mapa[(int)(Ghost_2_Position.Y / Grid), (int)(Ghost_2_Position.X / Grid) + 1] == 0) || (Mapa[(int)(Ghost_2_Position.Y / Grid), (int)(Ghost_2_Position.X / Grid) - 1] == 0))
                        {
                            if (Mapa[(int)(Ghost_2_Position.Y / Grid), (int)(Ghost_2_Position.X / Grid) + 1] == 0) Ghost_2_Direction = 0;
                            else Ghost_2_Direction = 2;
                        }
                        else if (Mapa[(int)(Ghost_2_Position.Y / Grid) - 1, (int)(Ghost_2_Position.X / Grid)] == 0) Ghost_2_Direction = 3;
                        else Ghost_2_Direction = 1;
                        break;

                }
            }

            switch (Ghost_2_Direction)
            {
                case 0:     //Right
                    if ((Ghost_2_Position.X > ((Widht - 1) * Grid) + 20) && (Ghost_2_Position.Y == 8 * Grid)) { Ghost_2_Position.X = 40; break; }

                    if (Mapa[(int)(Ghost_2_Position.Y / Grid), (int)(Ghost_2_Position.X / Grid) + 1] == 1)
                    {
                        Ghost_2_Position.X -= Ghost2Speed;

                    }
                    Ghost_2_Position.X += Ghost2Speed;
                    if ((Ghost_2_Position.X > 945) && (Ghost_2_Position.Y == 400)) Ghost_2_Position.X = 10;
                    break;

                case 1:     //Down

                    if (Mapa[(int)(Ghost_2_Position.Y / Grid) + 1, (int)(Ghost_2_Position.X / Grid)] == 1)
                    {
                        Ghost_2_Position.Y -= Ghost2Speed;


                    }
                    Ghost_2_Position.Y += Ghost2Speed;
                    break;
                case 2:     //Left
                    if ((Ghost_2_Position.X < 20) && (Ghost_2_Position.Y == 8 * Grid)) { Ghost_2_Position.X = ((Widht - 1) * Grid) + 20; break; }
                    Ghost_2_Position.X -= Ghost2Speed;
                    if (Mapa[(int)(Ghost_2_Position.Y / Grid), (int)(Ghost_2_Position.X / Grid)] == 1)
                    {

                        Ghost_2_Position.X += Ghost2Speed;

                    }
                    if ((Ghost_2_Position.X < 5) && (Ghost_2_Position.Y == 400))
                        Ghost_2_Position.X = 940;
                    break;

                case 3:      //Up
                    Ghost_2_Position.Y -= Ghost2Speed;

                    if (Mapa[(int)(Ghost_2_Position.Y / Grid), (int)(Ghost_2_Position.X / Grid)] == 1)
                    {
                        Ghost_2_Position.Y += Ghost2Speed;



                    }
                    break;
            }


            Ghost_Freeze_2.LastX = Ghost_2_Position.X;
            Ghost_Freeze_2.LastY = Ghost_2_Position.Y;

        }

        void Ghost_3_Pohyb()
        {

            if ((Ghost_Freeze_3.ActualX == Ghost_Freeze_3.LastX) && (Ghost_Freeze_3.ActualY == Ghost_Freeze_3.LastY))
            {
                int Rand = rd.Next(0, 4);
                switch (Rand)
                {
                    case 0: Ghost_3_Direction = 0; break;
                    case 1: Ghost_3_Direction = 1; break;
                    case 2: Ghost_3_Direction = 2; break;
                    case 3: Ghost_3_Direction = 3; break;
                    default: Ghost_3_Direction = 1; break;
                }
            }

            Ghost_Freeze_3.ActualX = Ghost_3_Position.X;
            Ghost_Freeze_3.ActualY = Ghost_3_Position.Y;

            if ((Ghost_3_Position.X % Grid == 0) && (Ghost_3_Position.Y % Grid == 0))
            {
                switch (Ghost_3_Direction)
                {
                    case 0: // Right
                        if ((Mapa[(int)(Ghost_3_Position.Y / Grid) - 1, (int)(Ghost_3_Position.X / Grid)] == 0) && (Mapa[(int)(Ghost_3_Position.Y / Grid) + 1, (int)(Ghost_3_Position.X / Grid)] == 0) && (Mapa[(int)(Ghost_3_Position.Y / Grid), (int)(Ghost_3_Position.X / Grid) + 1] == 0))
                        {
                            int Rand = rd.Next(0, 3);
                            switch (Rand)
                            {
                                case 0: Ghost_3_Direction = 1; break;
                                case 1: Ghost_3_Direction = 3; break;
                                case 2: Ghost_3_Direction = 0; break;
                                default: Ghost_3_Direction = 1; break;
                            }
                        }
                        else if ((Mapa[(int)(Ghost_3_Position.Y / Grid) - 1, (int)(Ghost_3_Position.X / Grid)] == 0) && (Mapa[(int)(Ghost_3_Position.Y / Grid) + 1, (int)(Ghost_3_Position.X / Grid)] == 0))
                        {
                            int Rand = rd.Next(0, 2);
                            switch (Rand)
                            {
                                case 0: Ghost_3_Direction = 1; break;
                                case 1: Ghost_3_Direction = 3; break;
                                default: Ghost_3_Direction = 1; break;
                            }
                        }
                        else if ((Mapa[(int)(Ghost_3_Position.Y / Grid) - 1, (int)(Ghost_3_Position.X / Grid)] == 0) || (Mapa[(int)(Ghost_3_Position.Y / Grid) + 1, (int)(Ghost_3_Position.X / Grid)] == 0))
                        {
                            if (Mapa[(int)(Ghost_3_Position.Y / Grid) - 1, (int)(Ghost_3_Position.X / Grid)] == 0) Ghost_3_Direction = 3;
                            else Ghost_3_Direction = 1;
                        }
                        else if (Mapa[(int)(Ghost_3_Position.Y / Grid), (int)(Ghost_3_Position.X / Grid) + 1] == 0) Ghost_3_Direction = 0;
                        else Ghost_3_Direction = 2;
                        break;

                    case 1: // Down
                        if ((Mapa[(int)(Ghost_3_Position.Y / Grid), (int)(Ghost_3_Position.X / Grid) + 1] == 0) && (Mapa[(int)(Ghost_3_Position.Y / Grid), (int)(Ghost_3_Position.X / Grid) - 1] == 0) && (Mapa[(int)(Ghost_3_Position.Y / Grid) + 1, (int)(Ghost_3_Position.X / Grid)] == 0))
                        {
                            int Rand = rd.Next(0, 4);
                            switch (Rand)
                            {
                                case 0: Ghost_3_Direction = 0; break;
                                case 1: Ghost_3_Direction = 1; break;
                                case 2: Ghost_3_Direction = 2; break;
                                default: Ghost_3_Direction = 1; break;
                            }
                        }
                        else if ((Mapa[(int)(Ghost_3_Position.Y / Grid), (int)(Ghost_3_Position.X / Grid) + 1] == 0) && (Mapa[(int)(Ghost_3_Position.Y / Grid), (int)(Ghost_3_Position.X / Grid) - 1] == 0))
                        {
                            int Rand = rd.Next(0, 2);
                            switch (Rand)
                            {
                                case 0: Ghost_3_Direction = 0; break;
                                case 1: Ghost_3_Direction = 2; break;
                                default: Ghost_3_Direction = 0; break;
                            }
                        }
                        else if ((Mapa[(int)(Ghost_3_Position.Y / Grid), (int)(Ghost_3_Position.X / Grid) + 1] == 0) || (Mapa[(int)(Ghost_3_Position.Y / Grid), (int)(Ghost_3_Position.X / Grid) - 1] == 0))
                        {
                            if (Mapa[(int)(Ghost_3_Position.Y / Grid), (int)(Ghost_3_Position.X / Grid) + 1] == 0) Ghost_3_Direction = 0;
                            else Ghost_3_Direction = 2;
                        }
                        else if (Mapa[(int)(Ghost_3_Position.Y / Grid) + 1, (int)(Ghost_3_Position.X / Grid)] == 0) Ghost_3_Direction = 1;
                        else Ghost_3_Direction = 3;
                        break;

                    case 2: //Left
                        if ((Mapa[(int)(Ghost_3_Position.Y / Grid) - 1, (int)(Ghost_3_Position.X / Grid)] == 0) && (Mapa[(int)(Ghost_3_Position.Y / Grid) + 1, (int)(Ghost_3_Position.X / Grid)] == 0) && (Mapa[(int)(Ghost_3_Position.Y / Grid), (int)(Ghost_3_Position.X / Grid) + 1] == 0))
                        {
                            int Rand = rd.Next(0, 3);
                            switch (Rand)
                            {
                                case 0: Ghost_3_Direction = 1; break;
                                case 1: Ghost_3_Direction = 3; break;
                                case 2: Ghost_3_Direction = 2; break;
                                default: Ghost_3_Direction = 1; break;
                            }
                        }
                        else if ((Mapa[(int)(Ghost_3_Position.Y / Grid) - 1, (int)(Ghost_3_Position.X / Grid)] == 0) && (Mapa[(int)(Ghost_3_Position.Y / Grid) + 1, (int)(Ghost_3_Position.X / Grid)] == 0))
                        {
                            int Rand = rd.Next(0, 2);
                            switch (Rand)
                            {
                                case 0: Ghost_3_Direction = 1; break;
                                case 1: Ghost_3_Direction = 3; break;
                                default: Ghost_3_Direction = 1; break;
                            }
                        }
                        else if ((Mapa[(int)(Ghost_3_Position.Y / Grid) - 1, (int)(Ghost_3_Position.X / Grid)] == 0) || (Mapa[(int)(Ghost_3_Position.Y / Grid) + 1, (int)(Ghost_3_Position.X / Grid)] == 0))
                        {
                            if (Mapa[(int)(Ghost_3_Position.Y / Grid) - 1, (int)(Ghost_3_Position.X / Grid)] == 0) Ghost_3_Direction = 3;
                            else Ghost_3_Direction = 1;
                        }
                        else if (Mapa[(int)(Ghost_3_Position.Y / Grid), (int)(Ghost_3_Position.X / Grid) + 1] == 0) Ghost_3_Direction = 2;
                        else Ghost_3_Direction = 0;
                        break;

                    case 3: //Up
                        if ((Mapa[(int)(Ghost_3_Position.Y / Grid), (int)(Ghost_3_Position.X / Grid) + 1] == 0) && (Mapa[(int)(Ghost_3_Position.Y / Grid), (int)(Ghost_3_Position.X / Grid) - 1] == 0) && (Mapa[(int)(Ghost_3_Position.Y / Grid) + 1, (int)(Ghost_3_Position.X / Grid)] == 0))
                        {
                            int Rand = rd.Next(0, 4);
                            switch (Rand)
                            {
                                case 0: Ghost_3_Direction = 0; break;
                                case 1: Ghost_3_Direction = 3; break;
                                case 2: Ghost_3_Direction = 2; break;
                                default: Ghost_3_Direction = 1; break;
                            }
                        }
                        else if ((Mapa[(int)(Ghost_3_Position.Y / Grid), (int)(Ghost_3_Position.X / Grid) + 1] == 0) && (Mapa[(int)(Ghost_3_Position.Y / Grid), (int)(Ghost_3_Position.X / Grid) - 1] == 0))
                        {
                            int Rand = rd.Next(0, 2);
                            switch (Rand)
                            {
                                case 0: Ghost_3_Direction = 0; break;
                                case 1: Ghost_3_Direction = 2; break;
                                default: Ghost_3_Direction = 0; break;
                            }
                        }
                        else if ((Mapa[(int)(Ghost_3_Position.Y / Grid), (int)(Ghost_3_Position.X / Grid) + 1] == 0) || (Mapa[(int)(Ghost_3_Position.Y / Grid), (int)(Ghost_3_Position.X / Grid) - 1] == 0))
                        {
                            if (Mapa[(int)(Ghost_3_Position.Y / Grid), (int)(Ghost_3_Position.X / Grid) + 1] == 0) Ghost_3_Direction = 0;
                            else Ghost_3_Direction = 2;
                        }
                        else if (Mapa[(int)(Ghost_3_Position.Y / Grid) - 1, (int)(Ghost_3_Position.X / Grid)] == 0) Ghost_3_Direction = 3;
                        else Ghost_3_Direction = 1;
                        break;

                }
            }

            switch (Ghost_3_Direction)
            {
                case 0:     //Right
                    if ((Ghost_3_Position.X > ((Widht - 1) * Grid) + 20) && (Ghost_3_Position.Y == 8 * Grid)) { Ghost_3_Position.X = 40; break; }

                    if (Mapa[(int)(Ghost_3_Position.Y / Grid), (int)(Ghost_3_Position.X / Grid) + 1] == 1)
                    {
                        Ghost_3_Position.X -= Ghost3Speed;

                    }
                    Ghost_3_Position.X += Ghost2Speed;
                    if ((Ghost_3_Position.X > 945) && (Ghost_3_Position.Y == 400)) Ghost_3_Position.X = 10;
                    break;

                case 1:     //Down

                    if (Mapa[(int)(Ghost_3_Position.Y / Grid) + 1, (int)(Ghost_3_Position.X / Grid)] == 1)
                    {
                        Ghost_3_Position.Y -= Ghost3Speed;


                    }
                    Ghost_3_Position.Y += Ghost3Speed;
                    break;
                case 2:     //Left
                    if ((Ghost_3_Position.X < 20) && (Ghost_3_Position.Y == 8 * Grid)) { Ghost_3_Position.X = ((Widht - 1) * Grid) + 20; break; }
                    Ghost_3_Position.X -= Ghost3Speed;
                    if (Mapa[(int)(Ghost_3_Position.Y / Grid), (int)(Ghost_3_Position.X / Grid)] == 1)
                    {

                        Ghost_3_Position.X += Ghost3Speed;

                    }
                    if ((Ghost_3_Position.X < 5) && (Ghost_3_Position.Y == 400))
                        Ghost_3_Position.X = 940;
                    break;

                case 3:      //Up
                    Ghost_3_Position.Y -= Ghost3Speed;

                    if (Mapa[(int)(Ghost_3_Position.Y / Grid), (int)(Ghost_3_Position.X / Grid)] == 1)
                    {
                        Ghost_3_Position.Y += Ghost3Speed;



                    }
                    break;
            }


            Ghost_Freeze_3.LastX = Ghost_3_Position.X;
            Ghost_Freeze_3.LastY = Ghost_3_Position.Y;

        }

        void Boss_Pohyb()
        {

            if ((Boss_Freeze.ActualX == Boss_Freeze.LastX) && (Boss_Freeze.ActualY == Boss_Freeze.LastY))
            {
                int Rand = rd.Next(0, 4);
                switch (Rand)
                {
                    case 0: Boss_Direction = 0; break;
                    case 1: Boss_Direction = 1; break;
                    case 2: Boss_Direction = 2; break;
                    case 3: Boss_Direction = 3; break;
                    default: Boss_Direction = 1; break;
                }
            }

            Boss_Freeze.ActualX = Boss_Position.X;
            Boss_Freeze.ActualY = Boss_Position.Y;

            if ((Boss_Position.X % Grid == 0) && (Boss_Position.Y % Grid == 0))
            {
                switch (Boss_Direction)
                {
                    case 0: // Right
                        if ((Mapa[(int)(Boss_Position.Y / Grid) - 1, (int)(Boss_Position.X / Grid)] == 0) && (Mapa[(int)(Boss_Position.Y / Grid) + 1, (int)(Boss_Position.X / Grid)] == 0) && (Mapa[(int)(Boss_Position.Y / Grid), (int)(Boss_Position.X / Grid) + 1] == 0))
                        {
                            int Rand = rd.Next(0, 3);
                            switch (Rand)
                            {
                                case 0: Boss_Direction = 1; break;
                                case 1: Boss_Direction = 3; break;
                                case 2: Boss_Direction = 0; break;
                                default: Boss_Direction = 1; break;
                            }
                        }
                        else if ((Mapa[(int)(Boss_Position.Y / Grid) - 1, (int)(Boss_Position.X / Grid)] == 0) && (Mapa[(int)(Boss_Position.Y / Grid) + 1, (int)(Boss_Position.X / Grid)] == 0))
                        {
                            int Rand = rd.Next(0, 2);
                            switch (Rand)
                            {
                                case 0: Boss_Direction = 1; break;
                                case 1: Boss_Direction = 3; break;
                                default: Boss_Direction = 1; break;
                            }
                        }
                        else if ((Mapa[(int)(Boss_Position.Y / Grid) - 1, (int)(Boss_Position.X / Grid)] == 0) || (Mapa[(int)(Boss_Position.Y / Grid) + 1, (int)(Boss_Position.X / Grid)] == 0))
                        {
                            if (Mapa[(int)(Boss_Position.Y / Grid) - 1, (int)(Boss_Position.X / Grid)] == 0) Boss_Direction = 3;
                            else Boss_Direction = 1;
                        }
                        else if (Mapa[(int)(Boss_Position.Y / Grid), (int)(Boss_Position.X / Grid) + 1] == 0) Boss_Direction = 0;
                        else Boss_Direction = 2;
                        break;

                    case 1: // Down
                        if ((Mapa[(int)(Boss_Position.Y / Grid), (int)(Boss_Position.X / Grid) + 1] == 0) && (Mapa[(int)(Boss_Position.Y / Grid), (int)(Boss_Position.X / Grid) - 1] == 0) && (Mapa[(int)(Boss_Position.Y / Grid) + 1, (int)(Boss_Position.X / Grid)] == 0))
                        {
                            int Rand = rd.Next(0, 4);
                            switch (Rand)
                            {
                                case 0: Boss_Direction = 0; break;
                                case 1: Boss_Direction = 1; break;
                                case 2: Boss_Direction = 2; break;
                                default: Boss_Direction = 1; break;
                            }
                        }
                        else if ((Mapa[(int)(Boss_Position.Y / Grid), (int)(Boss_Position.X / Grid) + 1] == 0) && (Mapa[(int)(Boss_Position.Y / Grid), (int)(Boss_Position.X / Grid) - 1] == 0))
                        {
                            int Rand = rd.Next(0, 2);
                            switch (Rand)
                            {
                                case 0: Boss_Direction = 0; break;
                                case 1: Boss_Direction = 2; break;
                                default: Boss_Direction = 0; break;
                            }
                        }
                        else if ((Mapa[(int)(Boss_Position.Y / Grid), (int)(Boss_Position.X / Grid) + 1] == 0) || (Mapa[(int)(Boss_Position.Y / Grid), (int)(Boss_Position.X / Grid) - 1] == 0))
                        {
                            if (Mapa[(int)(Boss_Position.Y / Grid), (int)(Boss_Position.X / Grid) + 1] == 0) Boss_Direction = 0;
                            else Boss_Direction = 2;
                        }
                        else if (Mapa[(int)(Boss_Position.Y / Grid) + 1, (int)(Boss_Position.X / Grid)] == 0) Boss_Direction = 1;
                        else Boss_Direction = 3;
                        break;

                    case 2: //Left
                        if ((Mapa[(int)(Boss_Position.Y / Grid) - 1, (int)(Boss_Position.X / Grid)] == 0) && (Mapa[(int)(Boss_Position.Y / Grid) + 1, (int)(Boss_Position.X / Grid)] == 0) && (Mapa[(int)(Boss_Position.Y / Grid), (int)(Boss_Position.X / Grid) + 1] == 0))
                        {
                            int Rand = rd.Next(0, 3);
                            switch (Rand)
                            {
                                case 0: Boss_Direction = 1; break;
                                case 1: Boss_Direction = 3; break;
                                case 2: Boss_Direction = 2; break;
                                default: Boss_Direction = 1; break;
                            }
                        }
                        else if ((Mapa[(int)(Boss_Position.Y / Grid) - 1, (int)(Boss_Position.X / Grid)] == 0) && (Mapa[(int)(Boss_Position.Y / Grid) + 1, (int)(Boss_Position.X / Grid)] == 0))
                        {
                            int Rand = rd.Next(0, 2);
                            switch (Rand)
                            {
                                case 0: Boss_Direction = 1; break;
                                case 1: Boss_Direction = 3; break;
                                default: Boss_Direction = 1; break;
                            }
                        }
                        else if ((Mapa[(int)(Boss_Position.Y / Grid) - 1, (int)(Boss_Position.X / Grid)] == 0) || (Mapa[(int)(Boss_Position.Y / Grid) + 1, (int)(Boss_Position.X / Grid)] == 0))
                        {
                            if (Mapa[(int)(Boss_Position.Y / Grid) - 1, (int)(Boss_Position.X / Grid)] == 0) Boss_Direction = 3;
                            else Boss_Direction = 1;
                        }
                        else if (Mapa[(int)(Boss_Position.Y / Grid), (int)(Boss_Position.X / Grid) + 1] == 0) Boss_Direction = 2;
                        else Boss_Direction = 0;
                        break;

                    case 3: //Up
                        if ((Mapa[(int)(Boss_Position.Y / Grid), (int)(Boss_Position.X / Grid) + 1] == 0) && (Mapa[(int)(Boss_Position.Y / Grid), (int)(Boss_Position.X / Grid) - 1] == 0) && (Mapa[(int)(Boss_Position.Y / Grid) + 1, (int)(Boss_Position.X / Grid)] == 0))
                        {
                            int Rand = rd.Next(0, 4);
                            switch (Rand)
                            {
                                case 0: Boss_Direction = 0; break;
                                case 1: Boss_Direction = 3; break;
                                case 2: Boss_Direction = 2; break;
                                default: Boss_Direction = 1; break;
                            }
                        }
                        else if ((Mapa[(int)(Boss_Position.Y / Grid), (int)(Boss_Position.X / Grid) + 1] == 0) && (Mapa[(int)(Boss_Position.Y / Grid), (int)(Boss_Position.X / Grid) - 1] == 0))
                        {
                            int Rand = rd.Next(0, 2);
                            switch (Rand)
                            {
                                case 0: Boss_Direction = 0; break;
                                case 1: Boss_Direction = 2; break;
                                default: Boss_Direction = 0; break;
                            }
                        }
                        else if ((Mapa[(int)(Boss_Position.Y / Grid), (int)(Boss_Position.X / Grid) + 1] == 0) || (Mapa[(int)(Boss_Position.Y / Grid), (int)(Boss_Position.X / Grid) - 1] == 0))
                        {
                            if (Mapa[(int)(Boss_Position.Y / Grid), (int)(Boss_Position.X / Grid) + 1] == 0) Boss_Direction = 0;
                            else Boss_Direction = 2;
                        }
                        else if (Mapa[(int)(Boss_Position.Y / Grid) - 1, (int)(Boss_Position.X / Grid)] == 0) Boss_Direction = 3;
                        else Boss_Direction = 1;
                        break;

                }
            }

            switch (Boss_Direction)
            {
                case 0:     //Right

                    if ((Boss_Position.X > ((Widht - 1) * Grid) + 20) && (Boss_Position.Y == 8 * Grid)) {Boss_Position.X = 40; break; }
                    if (Mapa[(int)(Boss_Position.Y / Grid), (int)(Boss_Position.X / Grid) + 1] == 1)
                    {
                        Boss_Position.X -= BossSpeed;

                    }
                    Boss_Position.X += BossSpeed;
                    if ((Boss_Position.X > 945) && (Boss_Position.Y == 400)) Boss_Position.X = 10;
                    break;

                case 1:     //Down

                    if (Mapa[(int)(Boss_Position.Y / Grid) + 1, (int)(Boss_Position.X / Grid)] == 1)
                    {
                        Boss_Position.Y -= BossSpeed;


                    }
                    Boss_Position.Y += BossSpeed;
                    break;
                case 2:     //Left
                    if ((Boss_Position.X < 20) && (Boss_Position.Y == 8 * Grid)) { Boss_Position.X = ((Widht - 1) * Grid) + 20; break; }
                    Boss_Position.X -= BossSpeed;
                    if (Mapa[(int)(Boss_Position.Y / Grid), (int)(Boss_Position.X / Grid)] == 1)
                    {

                        Boss_Position.X += BossSpeed;

                    }
                    if ((Boss_Position.X < 5) && (Boss_Position.Y == 400))
                        Boss_Position.X = 940;
                    break;

                case 3:      //Up
                    Boss_Position.Y -= BossSpeed;

                    if (Mapa[(int)(Boss_Position.Y / Grid), (int)(Boss_Position.X / Grid)] == 1)
                    {
                        Boss_Position.Y += BossSpeed;



                    }
                    break;
            }


            Boss_Freeze.LastX = Boss_Position.X;
            Boss_Freeze.LastY = Boss_Position.Y;

        }
    }
}