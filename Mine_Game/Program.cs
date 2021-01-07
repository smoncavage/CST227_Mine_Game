/*
Stephan Moncavage
CST227
Milestone 1
04 Sep 2020
Minesweeper Game Project
*/
using System;

namespace Mine_Game {
    class Program {
       static Board myBoard = new Board();
        static bool isWon = false;
        static void Main(string[] args) {
            int diff=0;
            Console.Write("Thank you for playing my mine game. Please select Difficulty (1-5). ");
            try{
                
                diff = int.Parse(Console.ReadLine());
                Console.WriteLine("Setting Up the Board...Please Wait...");
                if(diff > 5) {
                    Console.WriteLine("Please enter a valid number between 1 and 5.");
                }    
            }
            catch {
                Console.WriteLine("Please enter a valid number between 1 and 5.");
            }

            myBoard=new Board(diff*10, diff) {
                inPlay=true
            };
            myBoard.PrintBoard();
            while(myBoard.inPlay==true){
                try{
                    Console.WriteLine("Please Select a Row: ");
                    int curntRW = int.Parse(Console.ReadLine());
                    if(curntRW<0||curntRW>myBoard.Size) {
                        Console.WriteLine("Please select a number within the grid. ");
                    }
                    Console.WriteLine("Please Select a Column: ");
                    int curntCol = int.Parse(Console.ReadLine());
                    if(curntCol<0||curntCol>myBoard.Size) {
                        Console.WriteLine("Please select a number within the grid. ");
                    }
                    myBoard.FloodFill(curntRW, curntCol, 1);
                    try {

                        PrintInGame(curntRW, curntCol);
                    }
                    catch {
                        Console.WriteLine("Current Cell selection is invalid. Please try again.");
                    }
                }
                catch {
                    Console.WriteLine("An invalid character was entered, please try again. ");
                }
                
                
            CountVisits();
            }
        }

        public static void PrintInGame(int rw, int cl) {
            int row = 0;
            
            for(int cnt = 0; cnt<=myBoard.Size-1; cnt++) {
                Console.Write(" {0} ", cnt);
            }
            Console.Write(Environment.NewLine+Environment.NewLine);
            //myBoard.FloodFill(rw, cl);
            for(int i = 0; i<myBoard.grid.GetLength(0); i++) {
                for(int j = 0; j<myBoard.grid.GetLength(1); j++) {
                    if(myBoard.inPlay==false) {
                        if(myBoard.grid[i, j].live!=true) {
                            if(myBoard.grid[i, j].liveNeighbors==0) {
                                Console.Out.Write("-~-");
                            }
                            else {
                                Console.Out.Write(string.Format("-{0}-", myBoard.grid[i, j].liveNeighbors));
                            }
                        }
                        else {
                            Console.Out.Write("-*-");
                        }
                    }
                    else {
                        if(myBoard.grid[rw, cl].live) {
                            myBoard.inPlay=false;
                            Console.WriteLine("You have hit a MINE!");
                            //PrintInGame();
                        }
                        else if(isWon){
                            myBoard.inPlay=false;
                            Console.WriteLine("Congratulations! You Have WON!");
                        }
                        else if(myBoard.grid[rw,cl]==myBoard.grid[i,j] && myBoard.grid[rw,cl].liveNeighbors>0 || myBoard.grid[i,j].visited==true) {
                            if(myBoard.grid[i, j].visited==true && myBoard.grid[i,j].liveNeighbors==0) {
                                Console.Out.Write("-~-");
                            }
                            else{
                                myBoard.grid[rw, cl].visited=true;
                                Console.Out.Write(string.Format("-{0}-", myBoard.grid[i, j].liveNeighbors));
                            }
                        }
                        else if(myBoard.grid[rw, cl]==myBoard.grid[i, j]&&myBoard.grid[rw, cl].liveNeighbors==0||myBoard.grid[i, j].visited==true) {
                            myBoard.grid[rw, cl].visited=true;
                            Console.Out.Write("-~-");
                        }
                        else{
                        Console.Out.Write("-?-");
                        }
                    }
                }
                Console.Write("  {0}  ", (row++));
                Console.Write(Environment.NewLine+Environment.NewLine);
            }
        }
        //Count visits to a cell
        public static bool CountVisits() {
            int count = 0;
            for(int i = 0; i<myBoard.grid.GetLength(0); i++) {
                for(int j = 0; j<myBoard.grid.GetLength(1); j++) {
                    if(myBoard.grid[i, j].visited) {
                        count++;
                    }
                    if(((myBoard.grid.GetLength(0)*myBoard.grid.GetLength(1))-myBoard.mine)==count) {
                        isWon=true;
                    }
                }
            }  
        return isWon;
        }

    }
}
