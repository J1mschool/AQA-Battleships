//Skeleton Program for the AQA AS1 Summer 2016 examination
//Version Number 1.0

using System;
using System.IO;

class Program
{
  public struct ShipType
  {
    public string Name;
    public int Size;
  }
    const string Folder = "C:\\ProgramData\\CS Text Files";
    const string TrainingGame = "C:\\ProgramData\\CS Text Files\\Training.txt";


    //Init

        // Create Ship Types
        private static void SetUpShips(ref ShipType[] Ships)
        {
            Ships[0].Name = "Aircraft Carrier";
            Ships[0].Size = 5;
            Ships[1].Name = "Battleship";
            Ships[1].Size = 4;
            Ships[2].Name = "Submarine";
            Ships[2].Size = 3;
            Ships[3].Name = "Destroyer";
            Ships[3].Size = 3;
            Ships[4].Name = "Patrol Boat";
            Ships[4].Size = 2;
        }
    
         #region GameMode Selection

        //display the menu
        private static void DisplayMenu()
        {
            Console.WriteLine("MAIN MENU");
            Console.WriteLine("");
            Console.WriteLine("1. Start new game");
            Console.WriteLine("2. Load training game");
            Console.WriteLine("3. Test Map Generation");

        Console.WriteLine("9. Quit");
            Console.WriteLine();
        }

        //gets main menu selection
        private static int GetMainMenuChoice()
        {
            int Choice = 0;
            Console.Write("Please enter your choice: ");
            Choice = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine();
            return Choice;
        }

    #endregion

         #region  Random Map Generation For GameMode 1
        // sets every board position to "-"
        private static void SetUpBoard(ref char[,] Board)
            {
                for (int Row = 0; Row < 10; Row++)
                {
                    for (int Column = 0; Column < 10; Column++)
                    {
                        Board[Row, Column] = '-';
                    }


                }
            }

        // Responsible for placing random ships
        private static void PlaceRandomShips(ref char[,] Board, ShipType[] Ships)
        {
            Random RandomNumber = new Random();
            bool Valid;
            char Orientation = ' ';
            int Row = 0;
            int Column = 0;
            int HorV = 0;
            foreach (var Ship in Ships)
            {
                Valid = false;

                // While the position is invalid  
                while (Valid == false)
                {

                    // generate new random location and orientation 

                    Row = RandomNumber.Next(0, 10);
                    Column = RandomNumber.Next(0, 10);
                    HorV = RandomNumber.Next(0, 2);
                    if (HorV == 0)
                    {
                        Orientation = 'v';
                    }
                    else
                    {
                        Orientation = 'h';
                    }

                    //Checks if placement is valid
                    Valid = ValidateBoatPosition(Board, Ship, Row, Column, Orientation);
                }
                Console.WriteLine("Computer placing the " + Ship.Name);

                //Places ships if placement is valid
                PlaceShip(ref Board, Ship, Row, Column, Orientation);
            }
        }

        //Places ships in board
        private static void PlaceShip(ref char[,] Board, ShipType Ship, int Row, int Column, char Orientation)
        {
            if (Orientation == 'v')
            {
                for (int Scan = 0; Scan < Ship.Size; Scan++)
                {
                    Board[Row + Scan, Column] = Ship.Name[0];
                }
            }
            else if (Orientation == 'h')
            {
                for (int Scan = 0; Scan < Ship.Size; Scan++)
                {
                    Board[Row, Column + Scan] = Ship.Name[0];
                }
            }
        }

        // checks if boats fit in their random position
        private static bool ValidateBoatPosition(char[,] Board, ShipType Ship, int Row, int Column, char Orientation)
        {
            if (Orientation == 'v' && Row + Ship.Size > 10)
            {
                return false;
            }
            else if (Orientation == 'h' && Column + Ship.Size > 10)
            {
                return false;
            }
            else
            {
                if (Orientation == 'v')
                {
                    // checks if board can vertically fit on board
                    for (int Scan = 0; Scan < Ship.Size; Scan++)
                    {
                        // if space is not empty [-] then it try new position
                        if (Board[Row + Scan, Column] != '-')
                        {
                            return false;
                        }
                    }
                }
                else if (Orientation == 'h')
                {
                // checks if board can horizontally fit on board
                for (int Scan = 0; Scan < Ship.Size; Scan++)
                    {
                    // if space is not empty [-] then it try new position
                    if (Board[Row, Column + Scan] != '-')
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        #endregion


        // Assigns training file to game board (ONLY FOR GAMEMODE 2)
        private static void LoadGame(string TrainingGame, ref char[,] Board)
        {
            string Line = "";
            StreamReader BoardFile = new StreamReader(TrainingGame);
            for (int Row = 0; Row < 10; Row++)
            {
                Line = BoardFile.ReadLine();
                for (int Column = 0; Column < 10; Column++)
                {
                    Board[Row, Column] = Line[Column];
                }
            }
            BoardFile.Close();
        }



    //Update

        // Asks For input and coloum
        private static void GetRowColumn(ref int Row, ref int Column)
        {
            Console.WriteLine();
            Console.Write("Please enter column: ");
            Column = Convert.ToInt32(Console.ReadLine());
            Console.Write("Please enter row: ");
            Row = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine();
        }

        // Check if shot is a hit or miss :P
        private static void MakePlayerMove(ref char[,] Board, ref ShipType[] Ships)
        {
            int Row = 0;
            int Column = 0;
            GetRowColumn(ref Row, ref Column);  // Get coord input

            //if already hit square
            if (Board[Row, Column] == 'm' || Board[Row, Column] == 'h')
            {
                Console.WriteLine("Sorry, you have already shot at the square (" + Column + "," + Row + "). Please try again.");
            }
            else if (Board[Row, Column] == '-') // if hit a blank space
            {
                Console.WriteLine("Sorry, (" + Column + "," + Row + ") is a miss.");
                Board[Row, Column] = 'm';
            }
            //if map is not an empty space
            else
            {
                Console.WriteLine("Hit at (" + Column + "," + Row + ").");
                Board[Row, Column] = 'h';
            }
        }

    //Asks user if they would like to save their progress
        private static void SaveGame(char[,] Board )
        { 
            bool saveFile = false;

            Console.WriteLine("Would you like to save your progress?");
            string answer = Console.ReadLine();

        if (answer == "yes")
        {
            Console.WriteLine("Which File would you like to save your progress to? ");
            string fname = Console.ReadLine();
            string fulldirectory = Path.Combine(Folder, fname);

            File.Delete(fulldirectory);

            // iterate throguh board and write every item to the file
            foreach (char c in Board)
            {
                
                File.AppendAllText(fulldirectory, Convert.ToString(c) );
            }

            Console.WriteLine("File Saved");
          
        }
          
      
    
        }

        // checks if every  square with a ship in it has been hit has been 
        private static bool CheckWin(char[,] Board)
        {

            //returns false if any of the slots on the board have a ship that isnt hit in them
            for (int Row = 0; Row < 10; Row++)
            {
                for (int Column = 0; Column < 10; Column++)
                {

                    if (Board[Row, Column] == 'A' || Board[Row, Column] == 'B' || Board[Row, Column] == 'S' || Board[Row, Column] == 'D' || Board[Row, Column] == 'P')
                    {
                        return false;
                    }
                }
            }
            return true;
        }



    //Draw

        // Method to display the board
        private static void PrintBoard(char[,] Board)
        {
            Console.WriteLine();
            Console.WriteLine("The board looks like this: ");
            Console.WriteLine();
            Console.Write(" ");
            for (int Column = 0; Column < 10; Column++)
            {
                Console.Write(" " + Column + "  ");
            }
            Console.WriteLine();
            for (int Row = 0; Row < 10; Row++)
            {
                Console.Write(Row + " ");
                for (int Column = 0; Column < 10; Column++)
                {
                    if (Board[Row, Column] == '-')
                    {
                        Console.Write(" ");
                    }
                    else if (Board[Row, Column] == 'A' || Board[Row, Column] == 'B' || Board[Row, Column] == 'S' || Board[Row, Column] == 'D' || Board[Row, Column] == 'P')
                    {

                    Console.Write(" ");

                    }
                else
                    {
                        Console.Write(Board[Row, Column]);
                    }
                    if (Column != 9)
                    {
                        Console.Write(" | ");
                    }
                }
                Console.WriteLine();
            }
        }

        //Method to displat Full Board
        private static void TestMapGen(char[,] Board)
        {

            Console.WriteLine();
            Console.WriteLine("The board looks like this: ");
            Console.WriteLine();
            Console.Write(" ");
        
            for (int Column = 0; Column < 10; Column++)
            {
                Console.Write(" " + Column + "  ");
            }

            Console.WriteLine();

            for (int Row = 0; Row < 10; Row++)
            {
                Console.Write(Row + " ");
                for (int Column = 0; Column < 10; Column++)
                {
                    if (Board[Row, Column] == '-')
                    {
                        Console.Write(" ");
                    }

                    else if (Board[Row, Column] == 'A' || Board[Row, Column] == 'B' || Board[Row, Column] == 'S' || Board[Row, Column] == 'D' || Board[Row, Column] == 'P')
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("S");          //Debug line - Shows where the ships are
                        Console.ResetColor();
                    }

                    else
                    {
                        Console.Write(Board[Row, Column]);
                    }

                    if (Column != 9)
                    {
                        Console.Write(" | ");
                    }
                }

                Console.WriteLine();

            }
        }

    // GameLoop - edited to allow for saving
         private static void PlayGame(ref char[,] Board, ref ShipType[] Ships)
    {
        bool GameWon = false;
        while (GameWon == false)
        {
            int turnCount = 0;
            PrintBoard(Board);
            
            MakePlayerMove(ref Board, ref Ships);
            turnCount++;
            Console.WriteLine("You have had " + turnCount + " turns ");
            
            if (turnCount % 3 == 1 && turnCount != 0 )
            {
                SaveGame(Board);
            }

            GameWon = CheckWin(Board);
            if (GameWon == true)
            {
                Console.WriteLine("All ships sunk!");
                Console.WriteLine();
            }
        }
    }



    //Entry Point
        static void Main(string[] args)
        {
         ShipType[] Ships = new ShipType[5];
        char[,] Board = new char[10, 10];

        int MenuOption = 0 ;
         while (MenuOption != 9)
         {
           
           SetUpBoard(ref Board);
           SetUpShips(ref Ships);
           DisplayMenu();
           MenuOption = GetMainMenuChoice();


           if (MenuOption == 1)
           {
             PlaceRandomShips(ref Board, Ships);
             PlayGame(ref Board, ref Ships);
           }
           if (MenuOption == 2)
           {
             LoadGame(TrainingGame, ref Board);
             PlayGame(ref Board, ref Ships);
           }
          
           //GameMode to test print function
            if (MenuOption == 3)
            {
                PlaceRandomShips(ref Board, Ships);
                TestMapGen(Board);
                
            }


        }
        


  }

}