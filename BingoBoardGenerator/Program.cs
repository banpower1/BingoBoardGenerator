using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

/***
* Ask user for input
* -Number of boards
* -Number of numbers per board
* -Ask if the boards should contain blank spaces
* -Where to get the words/numbers from (.CSV file)
* 
* Generate boards
* -
***/

namespace BingoBoardGenerator
{
    class Program
    {
        enum printMessage
        {
            WelcomeScreen, 
            MainMenu,
            GoodBye
        }


        static void Main(string[] args)
        {
            bool mainMenu = true;
            while (mainMenu)
            {


                bool waitingForInput = true;
                Regex rgxDigit = new Regex(@"D[0-9]");
                Regex rgxNumPad = new Regex(@"NumPad[0-9]");

                while (waitingForInput)
                {
                    Console.Clear();
                    Print(printMessage.WelcomeScreen);
                    Print(printMessage.MainMenu);
                    ConsoleKeyInfo input = Console.ReadKey(true);
                    string inputString = input.Key.ToString();
                    

                    if (rgxDigit.IsMatch(inputString) || rgxNumPad.IsMatch(inputString))
                    {
                        waitingForInput = false;
                        string inputNumber = Regex.Replace(input.Key.ToString(), @"[^0-9]", "");

                        switch (inputNumber)
                        {
                            case "0":
                                mainMenu = false;
                                Print(printMessage.GoodBye);
                                break;
                            case "1":

                                //int numberOfBoards, int numberPerBoard, string inputFilePath, int boardWidth, int boardHeight
                                Console.WriteLine("Please enter the following information:");
                                int numberOfBoards;
                                int numberPerBoard;
                                string inputFilePath;
                                do
                                {
                                    Console.Write("Number of unique boards to generate: ");
                                } while (!int.TryParse(Console.ReadLine(), out numberOfBoards));
                                do
                                {
                                    Console.Write("Amount of \"Numbers\" per board: ");
                                } while (!int.TryParse(Console.ReadLine(), out numberPerBoard));

                                Console.Write("Filename: ");
                                inputFilePath = Console.ReadLine();

                                BingoBoardGenerator generator = new BingoBoardGenerator(numberOfBoards, numberPerBoard, inputFilePath);

                                generator.GenerateGame();
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }

        static void Print(printMessage option)
        {
            switch (option)
            {
                case printMessage.WelcomeScreen:
                    Console.WriteLine("Copyright \u00a9 Bertram André Nicolas - https://github.com/banpower1");
                    Console.WriteLine("");
                    Console.WriteLine("This is the Bingo Board Generator.");
                    break;
                case printMessage.MainMenu:
                    Console.WriteLine("1. - Generate plates");
                    Console.WriteLine("0. - Exit");
                    break;
                case printMessage.GoodBye:
                    Console.WriteLine("\nThis was a triumph!\nI'm making a note here:\nHuge success!");
                    System.Threading.Thread.Sleep(3000);
                    break;
                default:
                    break;
            }
            
        }
    }
}
