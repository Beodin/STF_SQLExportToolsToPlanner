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
                MakeFile_TalentPoints(db); 
                MakeFile_JobList(db);
                MakeFile_StfEngineData(db);
                MakeFile_ShipComponents(db);
                MakeFile_SkillPerJobList(db);
                MakeFile_TalentList(db);
                MakeFile_ShipWeaponData(db);
            }
        }

        public static string AddQuotes(string str)
        {
            return string.Format("\"{0}\"", str);
        }

        private static void MakeFile_TalentPoints(SqlNado.SQLiteDatabase db)
        {
            //output the header
            StreamWriter talentPoints = new StreamWriter(@"talent_points.csv");
            talentPoints.WriteLine("Rank:Talents");

            foreach (var tPoint in db.Load<CharLevel>("SELECT * FROM CharacterLevel;"))
            {
                talentPoints.WriteLine("{0}:{1}", tPoint.level, addTalentPoint(tPoint.level, tPoint.levelType));
            }

            talentPoints.Close();
        }

        public static int addTalentPoint(int level, int levelType)
        {
            
            if (levelType == 1)
            {
                switch (level)
                {
                    case 1:
                    case 3:
                    case 6:
                    case 8:
                    case 12:
                    case 14:
                    case 17:
                    case 21:
                    case 25:
                    case 28:
                    case 30:
                    case 31:
                    case 32:
                    case 35:
                    case 37:
                    case 40:
                        return 1;
                    default:
                        break;
                }

                return 0;
            }
            else if (levelType ==2) {
                switch (level)
                {
                    case 1:
                    case 3:
                    case 6:
                    case 8:
                    case 12:
                    case 14:
                    case 17:
                    case 21:
                    case 25:
                    case 28:
                    case 30:
                    case 32:
                    case 34:
                    case 37:
                    case 40:
                        return 1;
                    default:
                        break;
                }

                return 0;
            }
            else if (levelType == 3)
            {
                switch (level)
                {
                    case 1:
                    case 6:
                    case 11:
                    case 15:
                    case 17:
                    case 21:
                    case 25:
                    case 32:
                        return 1;
                    default:
                        break;
                }

                return 0;
            }
            return 0;
        }

        private static void MakeFile_ShipWeaponData(SqlNado.SQLiteDatabase db)
        {
            //output the header
            StreamWriter shipWeaponList = new StreamWriter(@"stf_weapon_data.csv");
            shipWeaponList.WriteLine("Name:Type:Damage:Radiation:Void:Range:AP:Accuracy:Critical Chance:Cripple Chance:Level");

            //iterate over filtered collection of rows, output text
            foreach(var shipWeap in db.Load<ShipWeapon>("SELECT * FROM ShipWeapon WHERE weaponName NOT LIKE 'X%' AND weaponName NOT LIKE 'NON%'; "))
            {
                shipWeaponList.WriteLine("{0}:{1}:{2}:{3}:{4}:{5}:{6}:{7}:{8}:{9}:{10}",
                    shipWeap.weaponName, shipWeap.weaponType, shipWeap.damage, shipWeap.radDamage, 
                    shipWeap.voidDamage, shipWeap.range, shipWeap.ap, shipWeap.accuracy, 
                    shipWeap.critChance, shipWeap.effectChance, shipWeap.level);
            }
            shipWeaponList.Close();
        }

        private static void MakeFile_TalentList(SqlNado.SQLiteDatabase db)
        {
            //output the header
            StreamWriter talentList = new StreamWriter(@"stf_talent_job.csv");
            talentList.WriteLine("Name:Rank:Description:Cooldown:Job:Type");

            //iterate over filtered collection of rows, output text
            foreach(var talent in db.Load<Talent>("SELECT * FROM Talent;"))
            {
                talentList.WriteLine("{0}:{1}:{2}:{3}:{4}:{5}",
                    talent.talentName, talent.jobLevel, talent.talentName2, talent.cooldown, 
                    talent.jobType, talent.actionType);
            }
            talentList.Close();
        }

        private static void MakeFile_StfEngineData(SqlNado.SQLiteDatabase db)
        {
            //output the header
            StreamWriter shipEngine = new StreamWriter(@"stf_engine_data.csv");
            shipEngine.WriteLine("Name:Mass:Speed:Agility:Fuel Cost:Combat Cost:Safety:Range Cost");

            //iterate over filtered collection of rows, output text
            //uses NOT LIKE 'X%' to limit to player components
            foreach (var engine in db.Load<ShipEngine>("SELECT * FROM ShipEngine WHERE name NOT LIKE 'X%';"))
            {
                shipEngine.WriteLine("{0}:{1}:{2}:{3}:{4}:{5}:{6}:{7}", 
                    engine.name, engine.designMass, engine.shipSpeed, engine.shipAgile, 
                    engine.mapFuelCost, engine.combatFuelCost, engine.safetyRating, engine.moveCost);
            }
            shipEngine.Close();
        }

        private static void MakeFile_ShipComponents(SqlNado.SQLiteDatabase db)
        {
            //output the header
            StreamWriter shipComp = new StreamWriter(@"ship components.csv");
            shipComp.WriteLine("Name:Type:Size:Mass:Pilot:Ship Ops:Gunnery:Electronics:Navigation:Explorer:Cargo:Max Crew:Max Officer:Jump Cost:Armor:Fuel Bonus:Guest:Prison:Max Crafts:Medical");

            //iterate over filtered collection of rows, output text
            //uses WHERE componentType > -1 to limit to player components
            foreach (var shipComponent in db.Load<ShipComponent>("SELECT componentName, componentType, componentSize, mass, skPilot, skShipOps, skGunnery, skElectronics, skNavigation, skExplorer, holdsCargo, " +
                "holdsCrew, holdsOfficer, jumpCost, armorBonus, fuelBonus, holdsGuest, holdsPrisoner, holdsCraft, medicalRating FROM ShipComponent WHERE componentType > -1 ORDER BY componentType;"))
            {
                shipComp.WriteLine("{0}:{1}:{2}:{3}:{4}:{5}:{6}:{7}:{8}:{9}:{10}:{11}:{12}:{13}:{14}:{15}:{16}:{17}:{18}:{19}", 
                    AddQuotes(shipComponent.componentName), shipComponent.componentType, shipComponent.componentSize, shipComponent.mass, 
                    shipComponent.skPilot, shipComponent.skShipOps, shipComponent.skGunnery, shipComponent.skElectronics, shipComponent.skNavigation, 
                    shipComponent.skExplorer, shipComponent.holdsCargo, shipComponent.holdsCrew, shipComponent.holdsOfficer, shipComponent.jumpCost, 
                    shipComponent.armorBonus, shipComponent.fuelBonus, shipComponent.holdsGuest, shipComponent.holdsPrisoner, shipComponent.holdsCraft, shipComponent.medicalRating);
            }
            shipComp.Close();
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

        private static void MakeFile_JobList(SqlNado.SQLiteDatabase db)
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

    public class ShipComponent
    {
        [SQLiteColumn(IsPrimaryKey = true)]
        public int _id { get; set; }
        public string componentName { get; set; }
        public int componentType { get; set; }
        public int componentSize { get; set; }
        public int mass { get; set; }
        public int skPilot { get; set; }
        public int skShipOps { get; set; }
        public int skGunnery { get; set; }
        public int skElectronics { get; set; }
        public int skNavigation { get; set; }
        public int skExplorer { get; set; }
        public int holdsCargo { get; set; }
        public int holdsCrew { get; set; }
        public int holdsOfficer { get; set; }
        public int jumpCost { get; set; }
        public int armorBonus { get; set; }
        public int fuelBonus { get; set; }
        public int holdsGuest { get; set; }
        public int holdsPrisoner { get; set; }
        public int holdsCraft { get; set; }
        public int medicalRating { get; set; }

    }

    public class ShipEngine
    {
        [SQLiteColumn(IsPrimaryKey = true)]
        public int _id { get; set; }
        public string name { get; set; }
        public int designMass { get; set; }
        public int shipSpeed { get; set; }
        public int shipAgile { get; set; }
        public int mapFuelCost { get; set; }
        public int combatFuelCost { get; set; }
        public int safetyRating { get; set; }
        public int moveCost { get; set; }
    }
    
    public class Talent
    {
        [SQLiteColumn(IsPrimaryKey = true)]
        public int _id { get; set; }
        public string talentName { get; set; }
        public int jobLevel { get; set; }
        public int jobType { get; set; }
        public string talentName2 { get; set; }
        public int cooldown { get; set; }
        public string actionType { get; set; }
    }

    public class ShipWeapon
    {
        [SQLiteColumn(IsPrimaryKey = true)]
        public int _id { get; set; }
        public string weaponName { get; set; }
        public int weaponType { get; set; }
        public int damage { get; set; }
        public int radDamage { get; set; }
        public int voidDamage { get; set; }
        public int range { get; set; }
        public int ap { get; set; }
        public int accuracy { get; set; }
        public int critChance { get; set; }
        public int effectChance { get; set; }
        public int level { get; set; }
    }

    public class CharLevel
    {
        [SQLiteColumn(IsPrimaryKey = true)]
        public int _id { get; set; }
        public int level { get; set; }
        public int levelType { get; set; }
    }
}
