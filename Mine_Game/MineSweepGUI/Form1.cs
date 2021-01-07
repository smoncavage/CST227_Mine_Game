/*
Stephan Moncavage
CST227
Milestone 5
02 Oct 2020
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

namespace MineSweepGUI {
    public partial class Form1:Form {
        //Form Start
        public Form1() {
            InitializeComponent();
        }
        //Determine Difficulty Level and send to Board.CS to build board
        private void Button1_Click(object sender, EventArgs e) {
            PlayerStats ply = new PlayerStats();
            PlayerStats.FiletoList();
            if(textBox1.Text!="") {
                int difficulty = 0;
                RadioButton[] radbtns = { radioButton1, radioButton3, radioButton5 };
                for(int i = 0; i < radbtns.Length; i++) {
                    if(radbtns[i].Checked) {
                        difficulty=i+1;
                    }
                }
                ply.Initials=textBox1.Text;
                ply.Score=0;
                PlayerStats.playerStats.Add(ply);
                
                Form2 frm2 = new Form2(difficulty);
                frm2.Show();
                Hide();
            }
            else {
                MessageBox.Show("Please enter initials before starting.");
            }
        }
    }
}
