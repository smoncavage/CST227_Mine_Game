/*
Stephan Moncavage
CST227
Milestone 7
17 Oct 2020
Minesweeper Game Project
*/
using MineSweepGUI.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Windows.Forms;

namespace MineSweepGUI {
    class PlayerStats {
        public string Initials{ get; set; }
        public double Score{ get; set; }

        public double Time{ get; set; }

        public System.TimeSpan Timer { get; set; }

        public static List<PlayerStats> playerStats = new List<PlayerStats>();

        public PlayerStats() {
        }

        public PlayerStats(string _initials, double _score, double _time) {
            Initials=_initials;
            Score=_score;
            Time=_time;
        }

        //Display Bound Data to show Score List on Form3
        public string Display {
            get {
                return string.Format("Player: {0} Score: {1} Time: {2}", Initials, Score, Time).ToUpper();
            }
        }

        //Create list of High Scores from File
        public static List<PlayerStats> FiletoList() {
            //Check to see if we have a list started
            string filer = @"Resources\PlayStats.txt";
            List<string> lines = File.ReadAllLines(filer).ToList();
            try {
                foreach(string line in lines) {
                    string[] entry = line.Split(',');
                    PlayerStats plyr = new PlayerStats {
                        Initials=entry[0],
                        Score=double.Parse(entry[1]),
                        Time=double.Parse(entry[2])
                    };
                    playerStats.Add(plyr);
                }
            }
            catch(Exception e) {
                //MessageBox.Show("Please check input file for proper formatting.");
                MessageBox.Show(e.Message);
            }
            if(playerStats.Count>=7) {
                playerStats.RemoveAt(6);
            }
            playerStats=RunQuery().Distinct().ToList();
            return playerStats;
        }

        //Outfile List to External File
        public static void ScoreOutput() {
            string outPath = @"Resources\PlayStats.txt";
            List<string> outputLines = new List<string>();
            playerStats=RunQuery().Distinct().ToList();
            try{
                if(playerStats.Count>=5) {
                    playerStats.RemoveAt(5);
                }
            }
            catch(Exception e) {
                MessageBox.Show(e.Message);
            }
            for(int playercount = 0; playercount < playerStats.Count; playercount++) {
                outputLines.Add($"{playerStats[playercount].Initials}, {playerStats[playercount].Score}, {playerStats[playercount].Time}");
                outputLines.Distinct().ToList();
            }
            try {
                File.WriteAllLines(outPath, outputLines);
            }
            catch(Exception e) {
                MessageBox.Show(e.Message);
            }
        }
        //Use LINQ to re-order the list by score in decending order
        static IEnumerable<PlayerStats> RunQuery() {
            var scoreQuery = from PlayerStats players in playerStats
                        orderby players.Score descending
                        select players;
            return scoreQuery;
        }
    }
}
