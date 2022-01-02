using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ce103_hw5_snake_dll
{
    public enum Direction { Up, Down, Right, Left }

    public class SnakeGameFunctions
    {
        // Const max length of the snake
        public const int maxLengthOfSnake = 100;

        // Starting direction of the snake
        private Direction currentDirection = Direction.Right;

        /**
        *	  @name Main Menu (mainMenu)
        *
        *	  @brief Main Menu
        *
        *	  Print main menu, and create selection
        **/
        public void mainMenu()
        {
            int x = 51;
            int y = 14;

            // Display loading screen
            loadingScreen();
            // Print main menu
            printMainMenu();

            // Hide cursor
            Console.CursorVisible = false;

            // Give key some value for go into the while loop with declaration
            ConsoleKey key = ConsoleKey.NoName;

            // Print selection arrow
            Console.SetCursorPosition(x - 1, y);
            Console.Write((char)9658);
            Console.SetCursorPosition(x - 1, y);

            // While loop for main menu selection
            while (key != ConsoleKey.Enter)
            {
                key = Console.ReadKey(false).Key;
                switch (key)
                {
                    case ConsoleKey.UpArrow: 
                    {
                            // Console set before and after write functions because if you press other characters then messing
                            Console.SetCursorPosition(x - 1, y);
                            Console.Write((char)32);
                            Console.SetCursorPosition(x - 1, y);
                            y--;
                            if (y == 13) { y = 17; }
                            Console.SetCursorPosition(x - 1, y);
                            Console.Write((char)9658);
                            Console.SetCursorPosition(x - 1, y);
                            break;
                    }
                    case ConsoleKey.DownArrow: 
                    {
                            Console.SetCursorPosition(x - 1, y);
                            Console.Write((char)32);
                            Console.SetCursorPosition(x - 1, y);
                            y++;
                            if (y == 18) { y = 14; }
                            Console.SetCursorPosition(x - 1, y);
                            Console.Write((char)9658);
                            Console.SetCursorPosition(x - 1, y);
                            break; 
                    }
                    case ConsoleKey.Enter:
                    {
                            if (y == 14) { selectGameType(); }
                            else if (y == 15) { getTopScores(); }
                            else if (y == 16) { printControls(); }
                            else { Environment.Exit(0); }
                            break;
                    }
                    default: { Console.SetCursorPosition(x - 1, y); Console.Write((char)9658); Console.SetCursorPosition(x - 1, y); break; }
                }
            }
        }

        /**
        *	  @name Start Game (startGame)
        *
        *	  @brief Start Game
        *
        *	  Load functions and start the game
        **/
        public void startGame()
        {
            // Initializations and Declarations
            int[,] snakeBodyCoord = new int[2, maxLengthOfSnake];
            int[] baitCoord = new int[2];
            int lengthOfSnake = 4;
            int speed = 0;
            int score = 0;
            snakeBodyCoord[0, 0] = 10;
            snakeBodyCoord[1, 0] = 10;
            baitCoord[0] = 50;
            baitCoord[1] = 10;
            speed = getGameSpeed(speed);
            loadEnvironment(lengthOfSnake);
            generateBait(snakeBodyCoord, baitCoord, lengthOfSnake);

            // While collision not true process game functions
            while (!collision(snakeBodyCoord, lengthOfSnake))
            {
                moveSnakeBody(lengthOfSnake, snakeBodyCoord);
                if (eatBait(snakeBodyCoord, baitCoord))
                {
                    generateBait(snakeBodyCoord, baitCoord, lengthOfSnake);
                    lengthOfSnake++;
                    score = getScore(speed, score);

                    Console.SetCursorPosition(74, 11);
                    Console.Write("Score: " + score);

                    Console.SetCursorPosition(53, 24);
                    Console.Write("Length Of Snake: " + lengthOfSnake);
                }
                if (lengthOfSnake >= maxLengthOfSnake) break;
                Thread.Sleep(speed);
            }

            // Save score to topscores.txt file
            File.AppendAllText("topscores.txt", score.ToString() + Environment.NewLine);

            Console.SetCursorPosition(30, 18);
            Console.Write("SCORE IS SAVED");

            // If length of snake greater than max length of snake call gamewin function else gameover
            if (lengthOfSnake >= maxLengthOfSnake)
            {
                printGameWin();
            }
            else printGameOver();
            Console.ReadLine();
        }

        /**
        *	  @name Start Game 2 (startGame2)
        *
        *	  @brief Start game without effect of walls
        *
        *	  Load functions and remove logic that colliding with walls and start the game
        **/
        public void startGame2()
        {
            // Initializations and Declarations
            int[,] snakeBodyCoord = new int[2, maxLengthOfSnake];
            int[] baitCoord = new int[2];
            int lengthOfSnake = 4;
            int speed = 0;
            int score = 0;
            snakeBodyCoord[0, 0] = 10;
            snakeBodyCoord[1, 0] = 10;
            baitCoord[0] = 50;
            baitCoord[1] = 10;
            speed = getGameSpeed(speed);
            loadEnvironment(lengthOfSnake);
            generateBait(snakeBodyCoord, baitCoord, lengthOfSnake);

            // While snake is not collided withself, process game functions
            while (!isCollidedWithselfOrBait(snakeBodyCoord[0, 0], snakeBodyCoord[1, 0], snakeBodyCoord, lengthOfSnake, 1))
            {
                moveSnakeBody(lengthOfSnake, snakeBodyCoord);

                // Making portal walls
                if (snakeBodyCoord[0, 0] == 2) { Console.SetCursorPosition(snakeBodyCoord[0, 0], snakeBodyCoord[1, 0]); Console.Write("|"); snakeBodyCoord[0, 0] = 70; }
                if (snakeBodyCoord[0, 0] == 71) { Console.SetCursorPosition(snakeBodyCoord[0, 0], snakeBodyCoord[1, 0]); Console.Write("|"); snakeBodyCoord[0, 0] = 3; }
                if (snakeBodyCoord[1, 0] == 2) { Console.SetCursorPosition(snakeBodyCoord[0, 0], snakeBodyCoord[1, 0]); Console.Write("▬"); snakeBodyCoord[1, 0] = 20; }
                if (snakeBodyCoord[1, 0] == 21) { Console.SetCursorPosition(snakeBodyCoord[0, 0], snakeBodyCoord[1, 0]); Console.Write("▬"); snakeBodyCoord[1, 0] = 3; }

                if (eatBait(snakeBodyCoord, baitCoord))
                {
                    generateBait(snakeBodyCoord, baitCoord, lengthOfSnake);
                    lengthOfSnake++;
                    score = getScore(speed, score);

                    Console.SetCursorPosition(74, 11);
                    Console.Write("Score: " + score);

                    Console.SetCursorPosition(53, 24);
                    Console.Write("Length Of Snake: " + lengthOfSnake);
                }
                if (lengthOfSnake >= maxLengthOfSnake) break;
                Thread.Sleep(speed);
            }

            // Save score to topscores.txt file
            File.AppendAllText("topscores.txt", score.ToString() + Environment.NewLine);

            Console.SetCursorPosition(30, 18);
            Console.Write("SCORE IS SAVED");

            // If length of snake greater than max length of snake call gamewin function else gameover
            if (lengthOfSnake >= maxLengthOfSnake)
            {
                printGameWin();
            }
            else printGameOver();
            Console.ReadLine();
        }

        /**
        *	  @name Move Snake Body (moveSnakeBody)
        *
        *	  @brief Move snake body according to the direction
        *
        *	  Move snake body according to the direction. Erase tail and add to head.
        *
        *	  @param  [in] lengthOfSnake [\b int] Length of the Snake
        *
        *	  @param  [in] snakeBodyCoord [\b int[,]] Snake's body coordinates
        **/
        public void moveSnakeBody(int lengthOfSnake, int[,] snakeBodyCoord)
        {
            Console.SetCursorPosition(snakeBodyCoord[0, lengthOfSnake - 1], snakeBodyCoord[1, lengthOfSnake - 1]);
            Console.Write(" ");
            Console.SetCursorPosition(0, 0);

            // Changes the head of the snake to a body part
            Console.SetCursorPosition(snakeBodyCoord[0, 0], snakeBodyCoord[1, 0]);
            Console.Write("O");
            Console.SetCursorPosition(0, 0);

            // Write tempDirection for backward keys
            Direction tempDirection = currentDirection;
            changeDirection(lengthOfSnake, snakeBodyCoord, tempDirection);

            // Write head part depending on direction
            Console.SetCursorPosition(snakeBodyCoord[0, 0], snakeBodyCoord[1, 0]);
            if (currentDirection == Direction.Up)
                Console.Write("▲");
            if (currentDirection == Direction.Down)
                Console.Write("▼");
            if (currentDirection == Direction.Right)
                Console.Write("►");
            if (currentDirection == Direction.Left)
                Console.Write("◄");
            Console.SetCursorPosition(0, 0);
        }

        /**
        *	  @name Change Direction (changeDirection)
        *
        *	  @brief Change snake direction
        *
        *	  Change snake direction and move array
        *
        *	  @param  [in] lengthOfSnake [\b int] Length of the Snake
        *	  
        *	  @param  [in] snakeBodyCoord [\b int[,]] Holding snake coordinates
        *	  
        *	  @param  [in] tempDirection [\b enum] Holding previous direction
        **/
        public void changeDirection(int lengthOfSnake, int[,] snakeBodyCoord, Direction tempDirection) 
        {
            // Create snake array
            for (int i = lengthOfSnake - 1; i != 0; i--)
            {
                snakeBodyCoord[0, i] = snakeBodyCoord[0, i - 1];
                snakeBodyCoord[1, i] = snakeBodyCoord[1, i - 1];
            }

            // Change direction if proper
            if (Console.KeyAvailable)
            {
                ConsoleKey key = Console.ReadKey(false).Key;
                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        if (tempDirection == Direction.Down) break;
                        currentDirection = Direction.Up; break;
                    case ConsoleKey.DownArrow:
                        if (tempDirection == Direction.Up) break;
                        currentDirection = Direction.Down; break;
                    case ConsoleKey.RightArrow:
                        if (tempDirection == Direction.Left) break;
                        currentDirection = Direction.Right; break;
                    case ConsoleKey.LeftArrow:
                        if (tempDirection == Direction.Right) break;
                        currentDirection = Direction.Left; break;
                    case ConsoleKey.P:
                        pauseMenu(tempDirection); break;
                    default: break;
                }
            }

            // Move snake by one depending on direction
            switch (currentDirection)
            {
                case Direction.Up:
                    snakeBodyCoord[1, 0]--;
                    break;
                case Direction.Down:
                    snakeBodyCoord[1, 0]++;
                    break;
                case Direction.Right:
                    snakeBodyCoord[0, 0]++;
                    break;
                case Direction.Left:
                    snakeBodyCoord[0, 0]--;
                    break;
                default: break;
            }
        }

        /**
        *	  @name Collision (collision)
        *
        *	  @brief Check collision
        *
        *	  Check if there is a collision
        *
        *	  @param  [in] snakeBodyCoord [\b int[,]] Snake's body coordinates
        *	  
        *	  @param  [in] lengthOfSnake [\b int] Length of the Snake
        **/
        public bool collision(int[,] snakeBodyCoord, int lengthOfSnake)
        {
            // If there is collision with wall(if side) or withself(else side) return true
            if ((snakeBodyCoord[0, 0] == 2) || (snakeBodyCoord[1, 0] == 2) || (snakeBodyCoord[0, 0] == 71) || (snakeBodyCoord[1, 0] == 21))
                return true;
            else
                if (isCollidedWithselfOrBait(snakeBodyCoord[0, 0], snakeBodyCoord[1, 0], snakeBodyCoord, lengthOfSnake, 1))
                return true;

            return false;
        }

        /**
        *	  @name Is Collided Withself or Bait (isCollidedWithselfOrBait)
        *
        *	  @brief Is Collided Withself or Bait
        *
        *	  Check if there is a collision with snake itself or with snake's body and generated food
        *	  
        *	  @param  [in] x [\b int] Given x coordinate
        *	  
        *	  @param  [in] y [\b int] Given y coordinate
        *
        *	  @param  [in] snakeBodyCoord [\b int[,]] Snake's body coordinates
        *	  
        *	  @param  [in] lengthOfSnake [\b int] Length of the Snake
        *	  
        *	  @param  [in] detectFrom [\b int] Start from this to scan
        **/
        public bool isCollidedWithselfOrBait(int x, int y, int[,] snakeBodyCoord, int lengthOfSnake, int detectFrom)
        {
            // Check if collided withself or with generated bait to current snake body
            for (int i = detectFrom; i < lengthOfSnake; i++)
	        {
		        if (x == snakeBodyCoord[0, i] && y == snakeBodyCoord[1, i])
			        return true;
	        }
	        return false;
        }

        /**
        *	  @name Generate Bait (generateBait)
        *
        *	  @brief Generate the bait
        *
        *	  Generate bait which will be eaten by snake
        *
        *	  @param  [in] snakeBodyCoord [\b int[,]] Snake's body coordinates
        *
        *	  @param  [in] baitCoord [\b int[]] Bait's coordinates
        *	  
        *	  @param  [in] lengthOfSnake [\b int] Length of the Snake
        **/
        public void generateBait(int[,] snakeBodyCoord, int[] baitCoord, int lengthOfSnake)
        {
            Random randomNumber = new Random();

            // Generate bait and check if there is collision with sneak current body
            do
            {
                // for X coordinate
                baitCoord[0] = (randomNumber.Next() % 67) + 3;
                // for Y coordinate
                baitCoord[1] = (randomNumber.Next() % 17) + 3;
            } while (isCollidedWithselfOrBait(baitCoord[0], baitCoord[1], snakeBodyCoord, lengthOfSnake, 0));

            Console.SetCursorPosition(baitCoord[0], baitCoord[1]);
            Console.Write((char)5);
        }

        /**
        *	  @name Eat Bait (eatBait)
        *
        *	  @brief Eat Bait
        *
        *	  If bait is eaten by snake return true else false
        *
        *	  @param  [in] snakeBodyCoord [\b int[,]] Snake's body coordinates
        *	  
        *	  @param  [in] baitCoord [\b int[]] Bait's coordinates
        *
        *	  @retval [\b bool] Is bait eaten by snake(true or false)
        **/
        public bool eatBait(int[,] snakeBodyCoord, int[] baitCoord)
        {
            // If sneak head matching with bait, then return true
            if (snakeBodyCoord[0, 0] == baitCoord[0] && snakeBodyCoord[1, 0] == baitCoord[1])
            {
                baitCoord[0] = 0;
                baitCoord[1] = 0;
                return true;
            }
            return false;
        }

        /**
        *	  @name Select Game Type (selectGameType)
        *
        *	  @brief Select Game Type
        *
        *	  Select the type of the game(with walls or transparent walls)
        **/
        public void selectGameType()
        {
            int x = 34;
            int y = 10;

            // Make printings
            Console.Clear();
            Console.SetCursorPosition(35, 10);
            Console.Write("Start Game with Impassable Walls");
            Console.SetCursorPosition(35, 11);
            Console.Write("Start Game with Passable Walls");

            Console.CursorVisible = false;
            ConsoleKey key = ConsoleKey.NoName;

            // Print selection arrow
            Console.SetCursorPosition(x - 1, y);
            Console.Write((char)9658);
            Console.SetCursorPosition(x - 1, y);

            // While loop for menu selection
            while (key != ConsoleKey.Enter)
            {
                key = Console.ReadKey(false).Key;
                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        {
                            Console.SetCursorPosition(x - 1, y);
                            Console.Write((char)32);
                            Console.SetCursorPosition(x - 1, y);
                            y--;
                            if (y == 9) { y = 11; }
                            Console.SetCursorPosition(x - 1, y);
                            Console.Write((char)9658);
                            Console.SetCursorPosition(x - 1, y);
                            break;
                        }
                    case ConsoleKey.DownArrow:
                        {
                            Console.SetCursorPosition(x - 1, y);
                            Console.Write((char)32);
                            Console.SetCursorPosition(x - 1, y);
                            y++;
                            if (y == 12) { y = 10; }
                            Console.SetCursorPosition(x - 1, y);
                            Console.Write((char)9658);
                            Console.SetCursorPosition(x - 1, y);
                            break;
                        }
                    case ConsoleKey.Enter:
                        {
                            if (y == 10) { startGame(); }
                            else if (y == 11) { startGame2(); }
                            break;
                        }
                    default: { Console.SetCursorPosition(x - 1, y); Console.Write((char)9658); Console.SetCursorPosition(x - 1, y); break; }
                }
            }
        }

        /**
        *	  @name Get Score (getScore)
        *
        *	  @brief Get Score
        *
        *	  Get score of the game
        *
        *	  @param  [in] speed [\b int] Program will be wait for "speed" miliseconds
        *	  
        *	  @param  [in] score [\b int] Score of the game
        **/
        public int getScore(int speed, int score)
        {
            // Get score according to speed(miliseconds to wait)
            if (speed <= 25)
            {
                int result = (score + 16);
                return result;
            }
            else if (speed <= 50)
            {
                int result = (score + 8);
                return result;
            }
            else if (speed <= 100)
            {
                int result = (score + 4);
                return result;
            }
            else return (score + 4);
        }

        /**
        *	  @name Get Game Speed (getGameSpeed)
        *
        *	  @brief Get speed
        *
        *	  Get Speed of the Game
        *
        *	  @param  [in] speed [\b int]  Given speed
        *
        *	  @retval [\b int] Selected speed
        **/
        public int getGameSpeed(int speed)
        {
            int x = 34;
            int y = 10;

            // Make printings
            Console.Clear();
            Console.SetCursorPosition(35, 10);
            Console.Write("Super Speed");
            Console.SetCursorPosition(35, 11);
            Console.Write("Full Speed");
            Console.SetCursorPosition(35, 12);
            Console.Write("Normal Speed");
            Console.SetCursorPosition(35, 13);
            Console.Write("Enter Speed");

            Console.CursorVisible = false;
            ConsoleKey key = ConsoleKey.NoName;

            // Print selection arrow
            Console.SetCursorPosition(x - 1, y);
            Console.Write((char)9658);
            Console.SetCursorPosition(x - 1, y);

            // While loop for menu selection
            while (key != ConsoleKey.Enter)
            {
                key = Console.ReadKey(false).Key;
                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        {
                            Console.SetCursorPosition(x - 1, y);
                            Console.Write((char)32);
                            Console.SetCursorPosition(x - 1, y);
                            y--;
                            if (y == 9) { y = 13; }
                            Console.SetCursorPosition(x - 1, y);
                            Console.Write((char)9658);
                            Console.SetCursorPosition(x - 1, y);
                            break;
                        }
                    case ConsoleKey.DownArrow:
                        {
                            Console.SetCursorPosition(x - 1, y);
                            Console.Write((char)32);
                            Console.SetCursorPosition(x - 1, y);
                            y++;
                            if (y == 14) { y = 10; }
                            Console.SetCursorPosition(x - 1, y);
                            Console.Write((char)9658);
                            Console.SetCursorPosition(x - 1, y);
                            break;
                        }
                    case ConsoleKey.Enter:
                        {
                            if (y == 10) { return 25; }
                            else if (y == 11) { return 50; }
                            else if (y == 12) { return 100; }
                            else
                            {
                                Console.Clear();
                                int value;

                                // While loop for wrong enters
                                while (true)
                                {
                                    Console.Write("Enter Speed(Low numbers are faster) : ");
                                    try { value = int.Parse(Console.ReadLine()); break; }
                                    catch (Exception e) { Console.WriteLine(e.Message); Console.WriteLine("Please write some numbers..."); }
                                }

                                return value;
                            }
                        }
                    default: { Console.SetCursorPosition(x - 1, y); Console.Write((char)9658); Console.SetCursorPosition(x - 1, y); break; }
                }
            }
            return 0;
        }

        /**
        *	  @name Get Top Scores (getTopScores)
        *
        *	  @brief Get top scores
        *
        *	  First generate file if there aren't any file and pad with zero.
        *	  If there are any lines then read all lines. Convert it and send
        *	  to the int array. Then sort scores with bubble sort algorithm
        **/
        public void getTopScores()
        {
            // Clear console
            Console.Clear();

            // If file does not exist then create and append zero character to first line.
            if (!File.Exists("topscores.txt")) 
            { 
                File.AppendAllText("topscores.txt", "0" + Environment.NewLine);
            }

            // Read all lines from file. Convert strings to int. Send to new int array
            string[] strarr = File.ReadAllLines("topscores.txt");
            int[] intarr = new int[strarr.Length];
            for (int i = 0; i < strarr.Length; i++)
            {
                intarr[i] = Int16.Parse(strarr[i]);
            }

            // Creates message for not playing games for high scores section
            if (intarr.Length == 1 && intarr[0] == 0) { Console.Write("***You should gain scores to see high scores***"); }

            // Bubble Sort for sorting values
            for (int i = 0; i < intarr.Length - 1; i++)
            {
                for (int j = 0; j < intarr.Length - 1 - i; j++)
                {
                    if (intarr[j] < intarr[j + 1])
                    {
                        int temp = intarr[j + 1];
                        intarr[j + 1] = intarr[j];
                        intarr[j] = temp;
                    }
                }
            }

            // Print console messages
            int x = 30;
            int y = 5;
            Console.SetCursorPosition(x, y);
            Console.Write("Top 10 Scores");
            x += 2;
            for (int i = 0; i < intarr.Length; i++)
            {
                if (i >= 10) break;
                Console.SetCursorPosition(x, ++y);
                Console.Write(i + 1);
                Console.Write(" ");
                Console.Write(intarr[i]);
            }
            y += 2;
            Console.SetCursorPosition(x, y);
            Console.Write("Enter to continue...");
            y += 2;
            Console.SetCursorPosition(x, y);
            Console.Write("***Write \"delete\" to delete file and all scores***");
            x += 15;
            y += 2;
            Console.SetCursorPosition(x, y);
            Console.Write("Enter: ");

            string written = Console.ReadLine();

            // Delete file and scores if user writes delete
            if (written == "delete")
            {
                File.Delete("topscores.txt");
            }
        }

        /**
        *	  @name Load Environment (loadEnvironment)
        *
        *	  @brief Print game map
        *
        *	  Printing game map
        *
        *	  @param  [in] lengthOfSnake [\b int]  Length of the Snake
        **/
        public void loadEnvironment(int lengthOfSnake)
        {
            Console.Clear();

            Console.SetCursorPosition(74, 11);
            Console.Write("Score: " + 0);

            Console.SetCursorPosition(43, 23);
            Console.Write("Goal Snake Length to Win: " + maxLengthOfSnake);

            Console.SetCursorPosition(53, 24);
            Console.Write("Length Of Snake: " + lengthOfSnake);

            for (int i = 0; i < 70; i++)
            {
                Console.SetCursorPosition(2 + i, 2);
                Console.Write((char)22);
            }

            for (int i = 0; i < 20; i++)
            {
                Console.SetCursorPosition(2, 2 + i);
                Console.Write('|');
            }

            for (int i = 0; i < 70; i++)
            {
                Console.SetCursorPosition(2 + i, 21);
                Console.Write((char)22);
            }

            for (int i = 0; i < 20; i++)
            {
                Console.SetCursorPosition(71, 2 + i);
                Console.Write('|');
            }

            // Wall's corners
            Console.SetCursorPosition(2, 2);
            Console.Write('+');
            Console.SetCursorPosition(2, 21);
            Console.Write('+');
            Console.SetCursorPosition(71, 2);
            Console.Write('+');
            Console.SetCursorPosition(71, 21);
            Console.Write('+');
        }

        /**
        *	  @name Loading Screen (loadingScreen)
        *
        *	  @brief Loading bar
        *
        *	  Print loading effect on entry
        **/
        public void loadingScreen()
        {
            Console.Clear();

            // Set characters to variable
            char a = '-', b = (char)15;

            Console.SetCursorPosition(45, 9);
            Console.Write("Loading...");

            Console.SetCursorPosition(45, 10);

            // Print initial loading bar 
            for (int i = 0; i < 26; i++)
                Console.Write(a);

            // Set the cursor again starting point of loading bar 
            Console.SetCursorPosition(45, 10);

            // Print loading bar progress 
            for (int i = 0; i < 26; i++)
            {
                Console.Write(b);
                Thread.Sleep(15);
            }

            Console.Clear();
        }

        /**
        *	  @name Pause Menu (pauseMenu)
        *
        *	  @brief Print pause menu
        *
        *	  Print pause menu
        **/
        public void pauseMenu(Direction tempDirection)
        {
            Console.SetCursorPosition(32, 11);
            Console.Write("***Paused***");
            switch (Console.ReadKey(false).Key)
            {
                case ConsoleKey.UpArrow:
                    if (tempDirection == Direction.Down) break;
                    currentDirection = Direction.Up; break;
                case ConsoleKey.DownArrow:
                    if (tempDirection == Direction.Up) break;
                    currentDirection = Direction.Down; break;
                case ConsoleKey.RightArrow:
                    if (tempDirection == Direction.Left) break;
                    currentDirection = Direction.Right; break;
                case ConsoleKey.LeftArrow:
                    if (tempDirection == Direction.Right) break;
                    currentDirection = Direction.Left; break;
                default: break;
            }
            Console.SetCursorPosition(32, 11);
            Console.Write("              ");
        }

        /**
        *	  @name Print Controls (printControls)
        *
        *	  @brief Print Control Menu
        *
        *	  Print control menu instructions for user.
        **/
        public void printControls()
        {
            int x = 10, y = 5;
            Console.Clear();
            Console.SetCursorPosition(x, y++);
            Console.Write("Controls\n");
            Console.SetCursorPosition(x++, y++);
            Console.Write("Use the following arrow keys to direct the snake to the food: ");
            Console.SetCursorPosition(x, y++);
            Console.Write("Right Arrow ►");
            Console.SetCursorPosition(x, y++);
            Console.Write("Left Arrow ◄");
            Console.SetCursorPosition(x, y++);
            Console.Write("Top Arrow ▲");
            Console.SetCursorPosition(x, y++);
            Console.Write("Bottom Arrow ▼");
            Console.SetCursorPosition(x, y++);
            Console.SetCursorPosition(x, y++);
            Console.Write("P pauses the game.");
            Console.SetCursorPosition(x, y++);
            Console.SetCursorPosition(x, y++);
            Console.Write("Press any key to continue...");
            Console.ReadKey();
        }

        /**
        *	  @name Print Main Menu (printMainMenu)
        *
        *	  @brief Main menu text
        *
        *	  Print main menu text
        **/
        public void printMainMenu()
        {
            Console.Clear();
            Console.SetCursorPosition(0, 5);
            Console.WriteLine(" ________  ________   ________  ___  __    _______           ________  ________  _____ ______   _______      ");
            Console.WriteLine("|\\   ____\\|\\   ___  \\|\\   __  \\|\\  \\|\\  \\ |\\  ___ \\         |\\   ____\\|\\   __  \\|\\   _ \\  _   \\|\\  ___ \\     ");
            Console.WriteLine("\\ \\  \\___|\\ \\  \\\\ \\  \\ \\  \\|\\  \\ \\  \\/  /|\\ \\   __/|        \\ \\  \\___|\\ \\  \\|\\  \\ \\  \\\\\\__\\ \\  \\ \\   __/|    ");
            Console.WriteLine(" \\ \\_____  \\ \\  \\\\ \\  \\ \\   __  \\ \\   ___  \\ \\  \\_|/__       \\ \\  \\  __\\ \\   __  \\ \\  \\\\|__| \\  \\ \\  \\_|/__  ");
            Console.WriteLine("  \\|____|\\  \\ \\  \\\\ \\  \\ \\  \\ \\  \\ \\  \\\\ \\  \\ \\  \\_|\\ \\       \\ \\  \\|\\  \\ \\  \\ \\  \\ \\  \\    \\ \\  \\ \\  \\_|\\ \\ ");
            Console.WriteLine("    ____\\_\\  \\ \\__\\\\ \\__\\ \\__\\ \\__\\ \\__\\\\ \\__\\ \\_______\\       \\ \\_______\\ \\__\\ \\__\\ \\__\\    \\ \\__\\ \\_______\\");
            Console.WriteLine("   |\\_________\\|__| \\|__|\\|__|\\|__|\\|__| \\|__|\\|_______|        \\|_______|\\|__|\\|__|\\|__|     \\|__|\\|_______|");
            Console.WriteLine("   \\|_________|                                                                                              ");
            Console.SetCursorPosition(52, 14);
            Console.Write("Start Game");
            Console.SetCursorPosition(52, 15);
            Console.Write("High Scores");
            Console.SetCursorPosition(52, 16);
            Console.Write("Controls");
            Console.SetCursorPosition(52, 17);
            Console.Write("Exit");
            Console.SetCursorPosition(51, 14);
        }

        /**
        *	  @name Game Win Text (printGameWin)
        *
        *	  @brief Prints some text
        *
        *	  Print game win text
        **/
        public void printGameWin()
        {
            Console.SetCursorPosition(19, 7);
            Console.Write("__   __            _    _ _       ");
            Console.SetCursorPosition(19, 8);
            Console.Write("\\ \\ / /           | |  | (_)      ");
            Console.SetCursorPosition(19, 9);
            Console.Write(" \\ V /___  _   _  | |  | |_ _ __  ");
            Console.SetCursorPosition(19, 10);
            Console.Write("  \\ // _ \\| | | | | |/\\| | | '_ \\ ");
            Console.SetCursorPosition(19, 11);
            Console.Write("  | | (_) | |_| | \\  /\\  / | | | |");
            Console.SetCursorPosition(19, 12);
            Console.Write("  \\_/\\___/ \\__,_|  \\/  \\/|_|_| |_|");
            Console.SetCursorPosition(19, 13);
            Console.Write("      Press enter to continue...");
        }

        /**
        *	  @name Game Over Text (printGameOver)
        *
        *	  @brief Prints some text
        *
        *	  Print game over text
        **/
        public void printGameOver()
        {
            Console.SetCursorPosition(0, 7);
            Console.WriteLine(" _______  _______  __   __  _______    _______  __   __  _______  ______   ");
            Console.WriteLine("|       ||   _   ||  |_|  ||       |  |       ||  | |  ||       ||    _ |  ");
            Console.WriteLine("|    ___||  |_|  ||       ||    ___|  |   _   ||  |_|  ||    ___||   | ||  ");
            Console.WriteLine("|   | __ |       ||       ||   |___   |  | |  ||       ||   |___ |   |_||_ ");
            Console.WriteLine("|   ||  ||       ||       ||    ___|  |  |_|  ||       ||    ___||    __  |");
            Console.WriteLine("|   |_| ||   _   || ||_|| ||   |___   |       | |     | |   |___ |   |  | |");
            Console.WriteLine("|_______||__| |__||_|   |_||_______|  |_______|  |___|  |_______||___|  |_|");
            Console.WriteLine();
            Console.WriteLine("                         Press enter to continue...");
        }
    }
}
    