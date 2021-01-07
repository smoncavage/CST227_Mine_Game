/*
Stephan Moncavage
CST227
Milestone 7
17 Oct 2020
Minesweeper Game Project
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace MineSweepGUI {
    public partial class Form2:Form {
        //Create Initial Properties
        readonly Image flg = Properties.Resources.flag;
        readonly Image bmb = Properties.Resources.bomb;
        readonly Image rex = Properties.Resources.redx;
        private PlayerStats plystat = new PlayerStats();
        public static Stopwatch watch = new Stopwatch();
        static Board myBoard;
        public static int Difficulty = 1;
        public Button[,] btnGrid = new Button[Difficulty*10, Difficulty*10];

        //Form start
        public Form2(int diff) {
            InitializeComponent();
            Difficulty=diff;
            PopulateGrid();
            watch.Start();
            plystat.Timer=watch.Elapsed;
        }

        //Fill the Panel with Buttons And Create Game Board to Match
        public void PopulateGrid() {
            int buttonSize = 32;
            panel1.Width=buttonSize*(Difficulty*10);
            panel1.Height=panel1.Width;
            Button[,] butnGrid = new Button[Difficulty*10, Difficulty*10];
            btnGrid=butnGrid;
            myBoard=new Board(Difficulty*10,Difficulty);
            myBoard.InitializeGrid();
            myBoard.SetupLiveNeighbors();
            for(int rw = 0; rw<Difficulty*10; rw++) {
                for(int cl = 0; cl<Difficulty*10; cl++) {
                    btnGrid[rw, cl]=new Button {
                        //make them square
                        Width=buttonSize,
                        Height=buttonSize
                    };
                    btnGrid[rw, cl].MouseUp+=Grid_Button_Click; //Same click event for each button
                    panel1.Controls.Add(btnGrid[rw, cl]);
                    btnGrid[rw, cl].Location=new Point(buttonSize*rw, buttonSize*cl);

                }
            }
        }

        //Show numbers if neighbor cells have bombs, and Check to see if the Game has been Won.
        public void UpdateButtonLabels() {
            int count = 0;
            int mines = 0;
            bool gmchk = false;
            for(int cl = 0; cl<Difficulty*10; cl++) {
                for(int rw = 0; rw<Difficulty*10; rw++) {
                    if(myBoard.grid[rw, cl].visited==true) {
                        count++;
                        if(myBoard.grid[rw, cl].liveNeighbors!=0&&btnGrid[rw,cl].Image!=flg&&!myBoard.grid[rw,cl].live) {
                            btnGrid[rw, cl].Text=myBoard.grid[rw, cl].liveNeighbors.ToString();
                        }
                        btnGrid[rw, cl].BackColor=Color.AliceBlue;
                        btnGrid[rw, cl].Enabled=false;
                        plystat.Score+=10*Difficulty;
                    }
                    if(myBoard.grid[rw, cl].live) {
                        mines++;
                    }
                }
            }
            if(count==((Difficulty*10)*(Difficulty*10))) {
                gmchk=true;
            }
            if(count==(((Difficulty*10)*(Difficulty*10))-mines)) {
                for(int cl = 0; cl<Difficulty*10; cl++) {
                    for(int rw = 0; rw<Difficulty*10; rw++) {
                        if(myBoard.grid[rw,cl].live){
                            myBoard.grid[rw, cl].visited=true;
                            btnGrid[rw, cl].Image=flg;
                            gmchk=true;
                        }
                    }
                }  
            }
            if(gmchk) {
                GameFinished(true, true);
            }
        }

        //Determine if user right-clicked button cell and show flag and disable button if so.
        public void Grid_Button_Click(object sender, MouseEventArgs e) {
            bool gmchk=false;
            for(int rw = 0; rw<Difficulty*10; rw++) {
                for(int cl = 0; cl<Difficulty*10; cl++) {
                    if((sender as Button).Equals(btnGrid[rw,cl])){
                        if(e.Button==MouseButtons.Right) {
                            if(btnGrid[rw, cl].Image==null) {
                                btnGrid[rw, cl].Image=flg;
                                btnGrid[rw, cl].Enabled=true;
                                myBoard.grid[rw, cl].visited=true;
                                plystat.Score+=100*Difficulty; //Add score if cell is bomb
                                if(!myBoard.grid[rw, cl].live) {
                                    plystat.Score-=150*Difficulty; //Penalty for flagging non-bomb cell
                                    btnGrid[rw, cl].Image=rex;
                                }
                            }
                            else {
                                btnGrid[rw, cl].Enabled=true;
                                btnGrid[rw, cl].Image.Dispose();
                                myBoard.grid[rw, cl].visited=false;
                                plystat.Score-=100*Difficulty;
                            }
                        }
                        else {
                            if(myBoard.grid[rw, cl].live) {
                                btnGrid[rw, cl].Image=bmb;
                                gmchk=true;
                            }
                            else {
                                if(myBoard.grid[rw, cl].liveNeighbors==0&&!myBoard.grid[rw,cl].live) {
                                    myBoard.CheckSurround(rw,cl);
                                }
                                myBoard.grid[rw, cl].visited=true;   
                            }
                        }
                    }
                }
            }
            if(gmchk){
                GameFinished(true, false);
            }
            UpdateButtonLabels();

            //Set Background of clicked button to a different color
            (sender as Button).BackColor=Color.AliceBlue;
            
        }

        //Schow All Cells with Bombs/Flags/Or Numbers
        private void ShowAll() {
            for(int cl = 0; cl<Difficulty*10; cl++) {
                for(int rw = 0; rw<Difficulty*10; rw++) {
                    if(myBoard.grid[rw, cl].live&&btnGrid[rw,cl].Image!=flg) {
                        btnGrid[rw, cl].Image=bmb;
                    }
                    if(myBoard.grid[rw, cl].liveNeighbors>0&&!myBoard.grid[rw, cl].live) {
                        btnGrid[rw, cl].Text=myBoard.grid[rw,cl].liveNeighbors.ToString();
                    }
                }
            }
        }

        //Ensure we close the program and now just "hide" the current form
        private void Form2_FormClosed(object sender, FormClosedEventArgs e) {
            Dispose(true);
            Close();
            Application.Exit();
        }

        //When Game is finished show appropriate message and display high score list page.
        private void GameFinished(bool complete, bool win) {
            watch.Stop();
            plystat.Timer=watch.Elapsed;
            int cnt = PlayerStats.playerStats.Count-1;
            plystat.Initials=PlayerStats.playerStats.ElementAt(cnt).Initials;
            PlayerStats.playerStats.ElementAt(cnt).Score=plystat.Score;
            PlayerStats.playerStats.ElementAt(cnt).Time=Math.Round(plystat.Timer.TotalSeconds,2);
            PlayerStats.ScoreOutput();
            ShowAll();
            if(complete&&win) {
                if(plystat.Timer.TotalSeconds<=30) {
                    plystat.Score+=750*Difficulty;
                }
                else if(plystat.Timer.TotalSeconds>=30&&plystat.Timer.TotalSeconds<=90) {
                    plystat.Score+=500*Difficulty;
                }
                else {
                    plystat.Score+=250*Difficulty;
                }
                MessageBox.Show(string.Format("{0} WON! Length of play was: {1}. Your score was: {2}!", plystat.Initials, plystat.Timer.TotalSeconds, plystat.Score.ToString()));
                
            }
            else if(complete&&!win) {;
                MessageBox.Show(string.Format("{0} hit a Mine! Length of play was: {1}. Your score was: {2}!", plystat.Initials, plystat.Timer.TotalSeconds, plystat.Score.ToString())); 
            }
            Form3 form = new Form3();
            form.Show();
            Hide();
        }
    }
}
