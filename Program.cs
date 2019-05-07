using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlNado;
using SqlNado.Utilities;
using System.IO;


namespace STF_SQLExportToolsToPlanner {
    class Program {

        static void Main(string[] args) {

            string path = "tools.db";

            using (var db = new SQLiteDatabase(path)) {

                MakeFIle_JobList(db);
                //MakeFIle_StfEngineData(db);
                //MakeFile_ShipComponents(db);

            }
        }

        private static void MakeFIle_JobList(SqlNado.SQLiteDatabase db) {
            // output the header
            StreamWriter jobList = new StreamWriter(@"jobList.csv");
            jobList.WriteLine("Job:Starter");

            // iterate over filtered collection of rows, output text
            foreach (var job in db.Load<Job>("SELECT * FROM JOB WHERE jobLevel = 1;")) {
                jobList.WriteLine("{0}:{1}", job.jobName, getIsStarter(job.jobType));
            }
            jobList.Close();
        }

        // map database keys to string YES/NO for tool
        public static string getIsStarter(int jobType) {
            switch (jobType) {
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                case 10:
                case 11:
                case 12:
                case 13:
                case 50:
                    return "YES";
                default:
                    break;
            }

            return "NO";
        }
    }

    // schema definitions

    // CREATE TABLE Job (_id INTEGER PRIMARY KEY AUTOINCREMENT, jobName TEXT, jobType INTEGER, jobLevel INTEGER, skLightFirearms INTEGER, skHeavyFirearms INTEGER, skMelee INTEGER, 
    //          skEvasion INTEGER, skTactics INTEGER, skStealth INTEGER, skGunnery INTEGER, skPilot INTEGER, skShipOps INTEGER, skRepair INTEGER, skElectronics INTEGER, skNavigation INTEGER, 
    //          skDoctor INTEGER, skCommand INTEGER, skNegotiate INTEGER, skIntimidate INTEGER, skExplorer INTEGER, enabled INTEGER, sortBy INTEGER);
    public class Job {
        [SQLiteColumn(IsPrimaryKey = true)]
        public int _id { get; set; }
        public string jobName { get; set; }
        public int jobType { get; set; }
    }

}
