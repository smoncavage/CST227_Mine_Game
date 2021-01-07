/*
Stephan Moncavage
CST227
Milestone 6
11 Oct 2020
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
    public partial class Form3:Form {
        public Form3() {
            InitializeComponent();
            SetBindings();
        }

        private void Button1_Click(object sender, EventArgs e) {
            PlayerStats.playerStats.Clear();
            Form1 form1 = new Form1();
            form1.Show();
            Hide();
        }

        private void Button2_Click(object sender, EventArgs e) {
            Dispose(true);
            Close();
            Application.Exit();
        }

        private void Form3_FormClosed(object sender, FormClosedEventArgs e) {
            Dispose(true);
            Close();
            Application.Exit();
        }
    }
}
