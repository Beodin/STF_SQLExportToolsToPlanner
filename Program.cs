using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlNado;
using SqlNado.Utilities;
using System.IO;


namespace STF_SQLExportToolsToPlanner
{
    class Program
    {

        static void Main(string[] args)
        {

            string path = "tools.db";

            using (var db = new SQLiteDatabase(path))
            {

                MakeFIle_JobList(db);
                //MakeFIle_StfEngineData(db);
                //MakeFile_ShipComponents(db);
                MakeFile_SkillPerJobList(db);

            }
        }

        private static void MakeFile_SkillPerJobList(SqlNado.SQLiteDatabase db)
        {
            //output the header
            StreamWriter skillPerJobList = new StreamWriter(@"skill_per_job_list.csv");
            skillPerJobList.WriteLine("Job Type:Rank:Pistols:Rifles:Blades:Evasion:Tactics:Stealth:Gunnery:Pilot:Ship Ops:Repair:Electronics:Navigation:Doctor:Command:Negotiate:Intimidate:Explore:Job");

            //iterate over filtered collection of rows, output text
            foreach (var skillPerJob in db.Load<Job>("SELECT jobType, jobLevel, skLightFirearms, skHeavyFirearms, skMelee, skEvasion, skTactics, " +
                "skStealth, skGunnery, skPilot, skShipOps, skRepair, skElectronics, skNavigation, skDoctor, skCommand, skNegotiate, skIntimidate, skExplorer, jobName FROM Job ORDER BY jobType;"))
            {
                skillPerJobList.WriteLine("{0}:{1}:{2}:{3}:{4}:{5}:{6}:{7}:{8}:{9}:{10}:{11}:{12}:{13}:{14}:{15}:{16}:{17}:{18}:{19}",
                    skillPerJob.jobType, skillPerJob.jobLevel, skillPerJob.skLightFirearms, skillPerJob.skHeavyFirearms, skillPerJob.skMelee, skillPerJob.skEvasion,
                    skillPerJob.skTactics, skillPerJob.skStealth, skillPerJob.skGunnery, skillPerJob.skPilot, skillPerJob.skShipOps, skillPerJob.skRepair, skillPerJob.skElectronics,
                    skillPerJob.skNavigation, skillPerJob.skDoctor, skillPerJob.skCommand, skillPerJob.skNegotiate, skillPerJob.skIntimidate, skillPerJob.skExplorer, skillPerJob.jobName);
            }
            skillPerJobList.Close();
        }

        private static void MakeFIle_JobList(SqlNado.SQLiteDatabase db)
        {
            // output the header
            StreamWriter jobList = new StreamWriter(@"job_list.csv");
            jobList.WriteLine("Job:Starter");

            // iterate over filtered collection of rows, output text
            foreach (var job in db.Load<Job>("SELECT * FROM JOB WHERE jobLevel = 1 ORDER BY jobType;"))
            {
                jobList.WriteLine("{0}:{1}", job.jobName, getIsStarter(job.jobType));
            }
            jobList.Close();
        }

        // map database keys to string YES/NO for tool
        public static string getIsStarter(int jobType)
        {
            switch (jobType)
            {
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
    public class Job
    {
        [SQLiteColumn(IsPrimaryKey = true)]
        public int _id { get; set; }
        public string jobName { get; set; }
        public int jobType { get; set; }
        public int jobLevel { get; set; }
        public int skLightFirearms { get; set; }
        public int skHeavyFirearms { get; set; }
        public int skMelee { get; set; }
        public int skEvasion { get; set; }
        public int skTactics { get; set; }
        public int skStealth { get; set; }
        public int skGunnery { get; set; }
        public int skPilot { get; set; }
        public int skShipOps { get; set; }
        public int skRepair { get; set; }
        public int skElectronics { get; set; }
        public int skNavigation { get; set; }
        public int skDoctor { get; set; }
        public int skCommand { get; set; }
        public int skNegotiate { get; set; }
        public int skIntimidate { get; set; }
        public int skExplorer { get; set; }
    }

}
