using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BingoBoardGenerator
{
    class BingoBoardGenerator
    {
        /**
         * Number of boards
         * Number of numbers per board
         * Ask if the boards should contain blank spaces
         * Where to get the words/numbers from (.CSV file)
         */

        HashSet<int>[] boards;
        int numberOfBoards;
        int numberPerBoard;
        Random rnd;

        public string[] bingoSquareContent;

        public BingoBoardGenerator(int numberOfBoards, int numberPerBoard, string inputFilePath)
        {
            this.numberOfBoards = numberOfBoards;
            this.numberPerBoard = numberPerBoard;
            boards = new HashSet<int>[numberOfBoards];

            ReadInputFile(inputFilePath, out bingoSquareContent);
        }

        public void GenerateGame()
        {
            rnd = new Random();
            boards[0] = GenerateBoard();
            PrintBoardNumber(0, boards[0]);
            //Run through each board to generate a board
            for (int i = 1; i < boards.Length; i++)
            {
                bool noMatchBoardFound = true;
                
                //Keep attempting to make a board as long as we don't have a valid one
                while (noMatchBoardFound)
                {
                    HashSet<int> board = GenerateBoard();

                    //Check through the already generated boards if it exists
                    for (int j = 0; j < i; j++)
                    {
                        if (BoardEquals(board, boards[j]))
                        {
                            //A matching board was found, break out of the for J for loop because we don't need to continue
                            break;
                        }
                        noMatchBoardFound = false;
                    }

                    //If no matching board was found, store the generated board, else reset the while loop and try again
                    if (!noMatchBoardFound)
                    {
                        boards[i] = board;
                        PrintBoardNumber(i, board);
                    }
                }
            }
            HashSet<string>[] finalBoards;
            ConvertBoard(boards, out finalBoards);
            WriteOutputFile(finalBoards);
            Console.ReadKey();
        }

        HashSet<int> GenerateBoard()
        {
            HashSet<int> board = new HashSet<int>();
            while (board.Count < numberPerBoard)
            {
                board.Add(rnd.Next(bingoSquareContent.Length));
            }
            return board;
        }

        bool BoardEquals(HashSet<int> board1, HashSet<int> board2)
        {
            foreach (int x in board1)
            {
                if (!board2.Contains(x))
                {
                    return false;
                }
            }

            Console.WriteLine("\nFUCK");
            return true;
        }

        void PrintBoardNumber(int boardNumber, HashSet<int> board)
        {
            Console.Write("\n " + boardNumber + ": " + board.First());
            for (int i = 1; i < board.Count; i++)
            {
                Console.Write(", {0}", board.Skip(i).First());
            }
        }

        void ConvertBoard(HashSet<int>[] input, out HashSet<string>[] output)
        {
            output = new HashSet<string>[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                output[i] = new HashSet<string>();
                foreach (int x in input[i])
                {
                    output[i].Add(bingoSquareContent[x]);
                }
            }
        }

        void ReadInputFile(string inputFileName, out string[] output)
        {
            output = new string[0];
            string codeBase = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            string executionPath = Path.GetDirectoryName(path);
            try
            {
                output = File.ReadAllLines(executionPath + "\\" + inputFileName);
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File not found at the following path: {0}\\{1}", executionPath, inputFileName);
                Console.ReadLine();
            }
            //use the length of the array to know how many different "numbers" you have (regular bingo is 1-90)
        }

        void WriteOutputFile(HashSet<string>[] boards)
        {
            string[] output = new string[boards.Length];
            for (int i = 0; i < boards.Length; i++)
            {
                string outputLine = "";
                foreach (string x in boards[i])
                {
                    outputLine += x + ", ";
                }

                outputLine = outputLine.Remove(outputLine.Length - 2,2);
                
                output[i] = outputLine;
            }

            try
            {
                string codeBase = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                string executionPath = Path.GetDirectoryName(path);
                File.WriteAllLines(executionPath + "\\" + "output.csv",output);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
