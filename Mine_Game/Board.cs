/*
Stephan Moncavage
CST227
Milestone 1
04 Sep 2020
Minesweeper Game Project
*/
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace Mine_Game {
    class Board {
        public int difficulty;
        public int size;
        //public int length;
        //public int width;
        public int mine;
        public int[] rwRound = { -1, -1, -1, 0, 1, 1, 1, 0 };
        public int[] clRound = { -1, 0, 1, -1, 1, 0, -1, 1 };
        public bool inPlay;
        //Could not get the program to work(Print) by setting the Array to "CELL" values so I changed it to a Character array instead for easier printing.
        public Cell[,] grid = null;
        //public Cell cell = new Cell();

        //Set/Get Values for Board params
        public int Size {
            get => size;
            set => size=value;
        }

        public int Difficulty {
            get => difficulty;
            set => difficulty=value;
        }
        /*
        public int Length {
            get => length;
            set => length=value;
        }

        public int Width {
            get => width;
            set => width=value;
        }
        */
        public int Mine {
            get => mine;
            set => mine=value;
        }

        public bool InPlay {
            get => inPlay;
            set => inPlay=false;
        }

        //Empty Constructor
        public Board() : this(10, 1) { //Set Default Size to 10x10

        }

        //Constructor using Difficulty to set board params
        public Board(int isize = 10, int idifficulty = 1) {
            size=isize;
            difficulty=idifficulty;
            SetupLiveNeighbors();
            //PrintBoard();
        }
        public Cell[,] InitializeGrid() {
            this.grid=new Cell[this.Size, this.Size];
            for(int iOuter = 0; iOuter<this.Size; iOuter++) {
                for(int jInner = 0; jInner<this.Size; jInner++) {
                    this.grid[iOuter, jInner]=new Cell();
                }
            }
            return this.grid;
        }
        //Set bomb cells
        public Cell[,] SetupLiveNeighbors() {
            Random rand = new Random(this.Size+1);
            int totalsize;
            grid=InitializeGrid(); //*This is Necessary* otherwise will recieve Null Object Exception
            totalsize=this.Size*this.Size;
            mine=totalsize/(10-this.difficulty);
            for(int iRand = mine; iRand>0; iRand--) {
                int randwth = rand.Next(this.Size+1);
                int randlth = rand.Next(this.Size+1);
                //Validate random #'s
                if((randwth>=0)&&(randwth<Size)&&(randlth>=0)&&(randlth<Size)&&!grid[randlth, randwth].live) {
                    grid[randlth, randwth].live=true;
                }
                else {
                    //print only for debugging
                    //this.grid[randwth, randlth].live=false;
                }
                //Write's are for Development ONLY
                /* 
                Console.Out.Write(
                "RANDWIDTH: ");
                int width = randwth+1;
                int length = randlth+1;
                Console.Out.Write(width.ToString());
                Console.Out.WriteLine();
                Console.Out.Write("RANDLENGTH: ");
                Console.Out.Write(length.ToString());
                Console.Out.WriteLine();
                */
                CalculateLiveNeighbors();
            }
            return this.grid;
        }

        //set neighboring bomb cells to value

        public Array CalculateLiveNeighbors() {
            for(int iOuter = 0; iOuter<Size; iOuter++) {
                for(int jInner = 0; jInner<Size; jInner++) {
                    try {
                        this.grid[iOuter, jInner].liveNeighbors=
                            LiveNeighbor(iOuter-1, jInner-1)+ //Upper Left Cell
                            LiveNeighbor(iOuter-1, jInner)+   //Left Cell
                            LiveNeighbor(iOuter-1, jInner+1)+ //Lower Left
                            LiveNeighbor(iOuter, jInner-1)+   //LowerCell
                            LiveNeighbor(iOuter+1, jInner+1)+ //Lower Right Cell
                            LiveNeighbor(iOuter+1, jInner)+   //Right Cell
                            LiveNeighbor(iOuter+1, jInner-1)+ //Upper Right Cell
                            LiveNeighbor(iOuter, jInner+1);   //Upper Cell
                    }
                    catch {
                        //Display Error
                        Console.Out.WriteLine("Unexpected Result during Neighbor Sets. ");
                    }
                }
            }

            return grid;
        }
        //Count Nieghboring Cells for nearby bombs
        private int LiveNeighbor(int iRow, int iCol) {
            int count = 0;
            // Validate data
            if((iRow<0)||(iRow>this.Size)||(iCol<0)&&(iCol>this.Size)) {
                count=0;
            }

            try {
                if(this.grid[iRow, iCol].live==true)
                    count++; // Incriment count
            }
            catch {
                // Display error or such
                //Console.Out.WriteLine("Error on \"NON Live\" Cell. ");
            }

            return count;
        }

        //print the board
        public void PrintBoard() {
            int row = 0;
            for(int cnt = 0; cnt<this.Size-1; cnt++) {
                Console.Write(" {0} ", cnt);
            }
            Console.Write(Environment.NewLine+Environment.NewLine);
            for(int i = 0; i<grid.GetLength(0); i++) {
                for(int j = 0; j<grid.GetLength(1); j++) {
                    if(inPlay==false) {
                        if(grid[i, j].live!=true) {
                            Console.Out.Write(string.Format("-{0}-", grid[i, j].liveNeighbors));
                        }
                        else {
                            Console.Out.Write("-*-");
                        }
                    }
                    else {
                        Console.Out.Write("-?-");
                    }
                }
                Console.Write("  {0}  ", (row++));
                Console.Write(Environment.NewLine+Environment.NewLine);
            }
            //*/
        }

        public void PrintCell(Cell cell) {
            //Display Code for Single cell

            if(cell.live.Equals(true)) {
                Console.Write("-*-");
            }
            else {
                Console.Write("-{0}-", cell.liveNeighbors);
            }
        }
        //recursive algorithm to iterate through open cells next to each other 
        public void FloodFill(int rw, int cl, int count) {
            RuntimeHelpers.EnsureSufficientExecutionStack();
            //int count = 1;
            try {
                //int count = 1;
                if(grid[rw, cl].liveNeighbors==0) {
                    CheckSurround(rw, cl);
                    //count++;
                    //Begin recusive function to move away from original cell and check other surrounding cells
                    if(IsSquareSafe(rw-count, cl-count)&&grid[rw-count, cl-count].liveNeighbors==0) {
                        FloodFill(rw-count, cl-count, 1);
                    }
                    else if(IsSquareSafe(rw, cl-count)&&grid[rw, cl-count].liveNeighbors==0) {
                        FloodFill(rw, cl-count, 1);
                    }
                    else if(IsSquareSafe(rw+count, cl-count)&&grid[rw+count, cl-count].liveNeighbors==0) {
                        FloodFill(rw+count, cl-count, 1);
                    }
                    else if(IsSquareSafe(rw+count, cl)&&grid[rw+count, cl].liveNeighbors==0) {
                        FloodFill(rw+count, cl, 1);
                    }
                    else if(IsSquareSafe(rw+count, cl+count)&&grid[rw+count, cl+count].liveNeighbors==0) {
                        FloodFill(rw+count, cl+count, 1);
                    }
                    else if(IsSquareSafe(rw, cl+count)&&grid[rw, cl+count].liveNeighbors==0) {
                        FloodFill(rw, cl+count, 1);
                    }
                    else if(IsSquareSafe(rw-count, cl+count)&&grid[rw-count, cl+count].liveNeighbors==0) {
                        FloodFill(rw-count, cl+count, 1);
                    }
                    else if(IsSquareSafe(rw-count, cl)&&grid[rw-count, cl].liveNeighbors==0) {
                        FloodFill(rw-count, cl, 1);
                    }
                    else {
                        //count++;
                        FloodFill(rw, cl, count++);
                    }
                    if(count>size*size) {
                        count=1;
                    }

                }
            }
            catch(InsufficientExecutionStackException) {
                Console.WriteLine("Ran out of stack Memory 1.");
            }
            //return false;
        }
        void FloodFill(int rw, int cl, int nxtrw, int nxtcl) {
            RuntimeHelpers.EnsureSufficientExecutionStack();
            int count = 1;
            try {
                //Begin recusive function to move away from original cell and check other surrounding cells
                if(grid[nxtrw, nxtcl].live==false) {
                    FloodFill(nxtrw, nxtcl, count);
                }
                /*
                else if(IsSquareSafe(nxtrw, cl)&&grid[nxtrw, cl-count].visited==false) {
                    FloodFill(nxtrw, cl);
                }
                else if(IsSquareSafe(rw-count, cl+count)&&grid[rw, cl-count].visited==false) {
                    FloodFill(rw-count, cl+count);
                }
                else if(IsSquareSafe(rw, cl+count)&&grid[rw, cl-count].visited==false) {
                    FloodFill(rw, cl+count);
                }
                else if(IsSquareSafe(rw+count, cl+count)&&grid[rw, cl-count].visited==false) {
                    FloodFill(rw+count, cl+count);
                }
                else if(IsSquareSafe(rw+count, cl)) {
                    FloodFill(rw+count, cl);
                }
                else if(IsSquareSafe(rw+count, cl-count)&&grid[rw, cl-count].visited==false) {
                    FloodFill(rw+count, cl-count);
                }
                else if(IsSquareSafe(rw, cl-count)&&grid[rw, cl-count].visited==false) {
                    FloodFill(rw, cl-count);
                }
                */
                else {
                    FloodFill(rw, cl, count++);
                }
                //if(count>size) {
                //     count=0;
                //}
                //else {
                //    count++;
                // }
            }
            catch(InsufficientExecutionStackException) {
                Console.WriteLine("Ran out of stack Memory 2.");
            }

        }
        void CheckSurround(int rw, int cl) {
            //int count = 1;
            try {
                if(IsSquareSafe(rw-1, cl-1)) {
                    if(grid[rw-1, cl-1].live==false&&grid[rw-1, cl-1].visited==false) {
                        grid[rw-1, cl-1].visited=true;
                    }
                }
                if(IsSquareSafe(rw-1, cl)) {
                    if(grid[rw-1, cl].live==false&&grid[rw-1, cl].visited==false) {
                        grid[rw-1, cl].visited=true;
                    }
                }
                if(IsSquareSafe(rw-1, cl+1)) {
                    if(grid[rw-1, cl+1].live==false&&grid[rw-1, cl+1].visited==false) {
                        grid[rw-1, cl+1].visited=true;
                    }
                }
                if(IsSquareSafe(rw, cl+1)) {
                    if(grid[rw, cl+1].live==false&&grid[rw, cl+1].visited==false) {
                        grid[rw, cl+1].visited=true;
                    }
                }
                if(IsSquareSafe(rw+1, cl+1)) {
                    if(grid[rw+1, cl+1].live==false&&grid[rw+1, cl+1].visited==false) {
                        grid[rw+1, cl+1].visited=true;
                    }
                }
                if(IsSquareSafe(rw+1, cl)) {
                    if(grid[rw+1, cl].live==false&&grid[rw+1, cl].visited==false) {
                        grid[rw+1, cl].visited=true;
                    }
                }
                if(IsSquareSafe(rw+1, cl-1)) {
                    if(grid[rw+1, cl-1].live==false&&grid[rw+1, cl-1].visited==false) {
                        grid[rw+1, cl-1].visited=true;
                    }
                }
                if(IsSquareSafe(rw, cl-1)) {
                    if(grid[rw, cl-1].live==false&&grid[rw, cl-1].visited==false) {
                        grid[rw, cl-1].visited=true;
                    }
                }
            }
            catch(InsufficientExecutionStackException) {
                Console.WriteLine("Ran out of stack Memory 3.");
            }
        }

        //Check if Cell is within the Grid
        bool IsSquareSafe(int x, int y) {
            if(x>-1&&x<Size&&y>-1&&y<Size)
                return true;
            return false;
        }
    }
}
