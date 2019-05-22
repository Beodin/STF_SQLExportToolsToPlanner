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
                MakeFile_ShipData(db);
                MakeFile_TalentPoints(db); 
                MakeFile_JobList(db);
                MakeFile_StfEngineData(db);
                MakeFile_ShipComponents(db);
                MakeFile_SkillPerJobList(db);
                MakeFile_TalentList(db);
                MakeFile_ShipWeaponData(db);
                MakeFile_SmallCraftList(db);
            }
        }

        public static string AddQuotes(string str)
        {
            return string.Format("\"{0}\"", str);
        }

        private static void MakeFile_SmallCraftList(SqlNado.SQLiteDatabase db)
        {
            //output the header
            StreamWriter smallCraft = new StreamWriter(@"smallCraft.csv");
            smallCraft.WriteLine("Name:Description:Cost:Hull:Armor:Shields:Launch Fuel Cost:Pilot:Electronics:Gunnery:AP:Agile:Speed:Repair Cost:Chance to hit Ship:Chance to hit Craft:Dodge");

            foreach (var craft in db.Load<SmallCraft>("SELECT * FROM SmallCraft;"))
            {
                smallCraft.WriteLine("{0}:{1}:{2}:{3}:{4}:{5}:{6}:{7}:{8}:{9}:{10}:{11}:{12}:{13}:{14}:{15}:{16}",
                    craft.craftName, craft.description, craft.cost, craft.maxHull, craft.craftArmor, craft.craftDeflection,
                    craft.launchFuelCost, craft.skPilot, craft.skElectronics, craft.skGunnery, craft.baseActionPoints,
                    craft.craftAgile, craft.craftSpeed, craft.repairCost, craft.baseToHitShip, craft.baseToHitCraft,
                    craft.baseToDodgeHit);
            }
            smallCraft.Close();
        }

        private static void MakeFile_ShipData(SqlNado.SQLiteDatabase db)
        {
            //output the header
            StreamWriter shipData = new StreamWriter(@"STF_Ship_Data.csv");
            shipData.WriteLine("Ship:Max Mass:Price:Small Slots:Medium Slots:Large Slots:Hull Points:Base Armor:Base Deflection:Max Officers:Max Life Support:Max Craft:Base Fuel");

            foreach (var shipType in db.Load<ShipType>("SELECT * FROM ShipType;"))
            {
                shipData.WriteLine("{0}:{1}:{2}:{3}:{4}:{5}:{6}:{7}:{8}:{9}:{10}:{11}:{12}",
                shipType.shipTypeName, shipType.baseMass, shipType.shipCost, shipType.smallSlots, shipType.mediumSlots, shipType.largeSlots, shipType.hullPoints, shipType.baseArmor, shipType.baseDeflection, shipType.maxOfficer, shipType.maxLifeSupport, shipType.maxCraft, shipType.baseFuel);
            }
            shipData.Close();
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
            skillPerJobList.WriteLine("Rank:1-Name:1-Num:2-Name:2-Num:3-Name:3-Num:Job");

                //Should the form be restructured this will provide all jobs and their level for all skills

                //output the header
                /*StreamWriter skillPerJobList = new StreamWriter(@"skill_per_job_list.csv");
                skillPerJobList.WriteLine("Job Type:Rank:Pistols:Rifles:Blades:Evasion:Tactics:Stealth:Gunnery:Pilot:Ship Ops:Repair:Electronics:Navigation:Doctor:Command:Negotiate:Intimidate:Explore:Job");
                //iterate over filtered collection of rows, output text
                foreach (var skillPerJob in db.Load<Job>("SELECT jobType, jobLevel, skLightFirearms, skHeavyFirearms, skMelee, skEvasion, skTactics, " +
                    "skStealth, skGunnery, skPilot, skShipOps, skRepair, skElectronics, skNavigation, skDoctor, skCommand, skNegotiate, skIntimidate, skExplorer, jobName FROM Job ORDER BY jobType;"))
                {
                    skillPerJobList.WriteLine("{0}:{1}:{2}:{3}:{4}:{5}:{6}:{7}:{8}:{9}:{10}:{11}:{12}:{13}:{14}:{15}:{16}:{17}:{18}:{19}",
                        skillPerJob.jobType, skillPerJob.jobLevel, skillPerJob.skLightFirearms, skillPerJob.skHeavyFirearms, skillPerJob.skMelee, skillPerJob.skEvasion,
                        skillPerJob.skTactics, skillPerJob.skStealth, skillPerJob.skGunnery, skillPerJob.skPilot, skillPerJob.skShipOps, skillPerJob.skRepair, skillPerJob.skElectronics,
                        skillPerJob.skNavigation, skillPerJob.skDoctor, skillPerJob.skCommand, skillPerJob.skNegotiate, skillPerJob.skIntimidate, skillPerJob.skExplorer, skillPerJob.jobName);
                }*/

            //Given the structure of the form any new jobs will need to be entered manually. 
            //To do this the SELECT statement should simply be SELECT * FROM Job WHERE jobType = "the jobType number of the new job"
            //You will then need to specify the 3 skills the job contains. Should the job only have 2 skills the third will be "Null" with a given value of "0".
            //Not how I wanted to handle this but the rewriting of the form would have been a much larger task than deemed necessary at the time.
            //This foreach will iterate over the filtered collection of rows and output text.
            foreach (var crewDog in db.Load<Job>("SELECT * FROM Job WHERE jobType = 1;"))
            {
                skillPerJobList.WriteLine("{0}:Ship Ops:{1}:Gunnery:{2}:Repair:{3}:{4}",
                    crewDog.jobLevel, crewDog.skShipOps, crewDog.skGunnery, crewDog.skRepair, crewDog.jobName);
            }

            foreach (var mechanic in db.Load<Job>("SELECT * FROM Job WHERE jobType = 2;"))
            {
                skillPerJobList.WriteLine("{0}:Ship Ops:{1}:Repair:{2}:Electronics:{3}:{4}",
                    mechanic.jobLevel, mechanic.skShipOps, mechanic.skRepair, mechanic.skElectronics, mechanic.jobName);
            }

            foreach (var electronicsTech in db.Load<Job>("SELECT * FROM Job WHERE jobType = 3;"))
            {
                skillPerJobList.WriteLine("{0}:Ship Ops:{1}:Gunnery:{2}:Electronics:{3}:{4}",
                    electronicsTech.jobLevel, electronicsTech.skShipOps, electronicsTech.skGunnery, electronicsTech.skElectronics, electronicsTech.jobName);
            }

            foreach (var gunner in db.Load<Job>("SELECT * FROM Job WHERE jobType = 4;"))
            {
                skillPerJobList.WriteLine("{0}:Ship Ops:{1}:Gunnery:{2}:Null:{3}",
                    gunner.jobLevel, gunner.skShipOps, gunner.skGunnery, gunner.jobName);
            }

            foreach (var soldier in db.Load<Job>("SELECT * FROM Job WHERE jobType = 5;"))
            {
                skillPerJobList.WriteLine("{0}:Rifles:{1}:Evasion:{2}:Null:0:{3}",
                    soldier.jobLevel, soldier.skHeavyFirearms, soldier.skEvasion, soldier.jobName);
            }

            foreach (var pistoleer in db.Load<Job>("SELECT * FROM Job WHERE jobType = 6;"))
            {
                skillPerJobList.WriteLine("{0}:Pistols:{1}:Evasion:{2}:Null:0:{3}",
                    pistoleer.jobLevel, pistoleer.skLightFirearms, pistoleer.skEvasion, pistoleer.jobName);
            }

            foreach (var sniper in db.Load<Job>("SELECT * FROM Job WHERE jobType = 7;"))
            {
                skillPerJobList.WriteLine("{0}:Rifles:{1}:Evasion:{2}:Stealth:{3}:{4}",
                    sniper.jobLevel, sniper.skHeavyFirearms, sniper.skEvasion, sniper.skStealth, sniper.jobName);
            }

            foreach (var swordsman in db.Load<Job>("SELECT * FROM Job WHERE jobType = 8;"))
            {
                skillPerJobList.WriteLine("{0}:Blades:{1}:Evasion:{2}:Null:0:{3}",
                    swordsman.jobLevel, swordsman.skMelee, swordsman.skEvasion, swordsman.jobName);
            }

            foreach (var assassin in db.Load<Job>("SELECT * FROM Job WHERE jobType = 10;"))
            {
                skillPerJobList.WriteLine("{0}:Blades:{1}:Evasion:{2}:Stealth:{3}:{4}",
                    assassin.jobLevel, assassin.skMelee, assassin.skEvasion, assassin.skStealth, assassin.jobName);
            }

            foreach (var combatMedic in db.Load<Job>("SELECT * FROM Job WHERE jobType = 11;"))
            {
                skillPerJobList.WriteLine("{0}:Pistols:{1}:Doctor:{2}:Null:0:{3}",
                    combatMedic.jobLevel, combatMedic.skLightFirearms, combatMedic.skDoctor, combatMedic.jobName);
            }

            foreach (var doctor in db.Load<Job>("SELECT * FROM Job WHERE jobType = 12;"))
            {
                skillPerJobList.WriteLine("{0}:Tactics:{1}:Doctor:{2}:Command:{3}:{4}",
                    doctor.jobLevel, doctor.skTactics, doctor.skDoctor, doctor.skCommand, doctor.jobName);
            }

            foreach (var diplomat in db.Load<Job>("SELECT * FROM Job WHERE jobType = 13;"))
            {
                skillPerJobList.WriteLine("{0}:Negotiate:{1}:Intimidate:{2}:Null:0:{3}",
                    diplomat.jobLevel, diplomat.skNegotiate, diplomat.skIntimidate, diplomat.jobName);
            }

            foreach (var zealot in db.Load<Job>("SELECT * FROM Job WHERE jobType = 15;"))
            {
                skillPerJobList.WriteLine("{0}:Blades:{1}:Intimidate:{2}:Command:{3}:{4}",
                    zealot.jobLevel, zealot.skMelee, zealot.skIntimidate, zealot.skCommand, zealot.jobName);
            }

            foreach (var hyperwarpNavigator in db.Load<Job>("SELECT * FROM Job WHERE jobType = 16;"))
            {
                skillPerJobList.WriteLine("{0}:Tactics:{1}:Navigator:{2}:Null:0:{3}",
                    hyperwarpNavigator.jobLevel, hyperwarpNavigator.skTactics, hyperwarpNavigator.skNavigation, hyperwarpNavigator.jobName);
            }

            foreach (var wingTech in db.Load<Job>("SELECT * FROM Job WHERE jobType = 17;"))
            {
                skillPerJobList.WriteLine("{0}:Tactics:{1}:Ship Ops:{2}:Repair:{3}:{4}",
                    wingTech.jobLevel, wingTech.skTactics, wingTech.skShipOps, wingTech.skRepair, wingTech.jobName);
            }

            foreach (var exoScout in db.Load<Job>("SELECT * FROM Job WHERE jobType = 18;"))
            {
                skillPerJobList.WriteLine("{0}:Rifles:{1}:Explore:{2}:Null:0:{3}",
                    exoScout.jobLevel, exoScout.skHeavyFirearms, exoScout.skExplorer, exoScout.jobName);
            }

            foreach (var scavenger in db.Load<Job>("SELECT * FROM Job WHERE jobType = 19;"))
            {
                skillPerJobList.WriteLine("{0}:Repair:{1}:Doctor:{2}:Explore:{3}:{4}",
                    scavenger.jobLevel, scavenger.skRepair, scavenger.skDoctor, scavenger.skExplorer, scavenger.jobName);
            }

            foreach (var xenoHunter in db.Load<Job>("SELECT * FROM Job WHERE jobType = 20;"))
            {
                skillPerJobList.WriteLine("{0}:Rifles:{1}:Intimidate:{2}:Explore:{3}:{4}",
                    xenoHunter.jobLevel, xenoHunter.skHeavyFirearms, xenoHunter.skIntimidate, xenoHunter.skExplorer, xenoHunter.jobName);
            }

            foreach (var commander in db.Load<Job>("SELECT * FROM Job WHERE jobType = 23;"))
            {
                skillPerJobList.WriteLine("{0}:Tactics:{1}:Intimidate:{2}:Command:{3}:{4}",
                    commander.jobLevel, commander.skTactics, commander.skIntimidate, commander.skCommand, commander.jobName);
            }

            foreach (var pilot in db.Load<Job>("SELECT * FROM Job WHERE jobType = 24;"))
            {
                skillPerJobList.WriteLine("{0}:Tactics:{1}:Pilot:{2}:Navigator:{3}:{4}",
                    pilot.jobLevel, pilot.skTactics, pilot.skPilot, pilot.skNavigation, pilot.jobName);
            }

            foreach (var wingCommando in db.Load<Job>("SELECT * FROM Job WHERE jobType = 25;"))
            {
                skillPerJobList.WriteLine("{0}:Blades:{1}:Pilot:{2}:Evasion:{3}:{4}",
                   wingCommando.jobLevel, wingCommando.skMelee, wingCommando.skPilot, wingCommando.skEvasion, wingCommando.jobName);
            }

            foreach (var merchant in db.Load<Job>("SELECT * FROM Job WHERE jobType = 26;"))
            {
                skillPerJobList.WriteLine("{0}:Tactics:{1}:Command:{2}:Negotiate:{3}:{4}",
                    merchant.jobLevel, merchant.skTactics, merchant.skCommand, merchant.skNegotiate, merchant.jobName);
            }

            foreach (var smuggler in db.Load<Job>("SELECT * FROM Job WHERE jobType = 27;"))
            {
                skillPerJobList.WriteLine("{0}:Stealth:{1}:Negotiate:{2}:Intimidate:{3}:{4}",
                    smuggler.jobLevel, smuggler.skStealth, smuggler.skNegotiate, smuggler.skIntimidate, smuggler.jobName);
            }

            foreach (var pirate in db.Load<Job>("SELECT * FROM Job WHERE jobType = 28;"))
            {
                skillPerJobList.WriteLine("{0}:Gunnery:{1}:Pilot:{2}:Intimidate:{3}:{4}",
                    pirate.jobLevel, pirate.skGunnery, pirate.skPilot, pirate.skIntimidate, pirate.jobName);
            }

            foreach (var bountyHunter in db.Load<Job>("SELECT * FROM Job WHERE jobType = 29;"))
            {
                skillPerJobList.WriteLine("{0}:Rifles:{1}:Evasion:{2}:Intimidate:{3}:{4}",
                    bountyHunter.jobLevel, bountyHunter.skHeavyFirearms, bountyHunter.skEvasion, bountyHunter.skIntimidate, bountyHunter.jobName);
            }

            foreach (var militaryOfficer in db.Load<Job>("SELECT * FROM Job WHERE jobType = 30;"))
            {
                skillPerJobList.WriteLine("{0}:Tactics:{1}:Pistols:{2}:Command:{3}:{4}",
                    militaryOfficer.jobLevel, militaryOfficer.skTactics, militaryOfficer.skLightFirearms, militaryOfficer.skCommand, militaryOfficer.jobName);
            }

            foreach (var explorer in db.Load<Job>("SELECT * FROM Job WHERE jobType = 31;"))
            {
                skillPerJobList.WriteLine("{0}:Tactics:{1}:Electronics:{2}:Explore:{3}:{4}",
                    explorer.jobLevel, explorer.skTactics, explorer.skElectronics, explorer.skExplorer, explorer.jobName);
            }

            foreach (var spy in db.Load<Job>("SELECT * FROM Job WHERE jobType = 32;"))
            {
                skillPerJobList.WriteLine("{0}:Pistols:{1}:Stealth:{2}:Electronics:{3}:{4}",
                    spy.jobLevel, spy.skLightFirearms, spy.skStealth, spy.skElectronics, spy.jobName);
            }

            foreach (var engineer in db.Load<Job>("SELECT * FROM Job WHERE jobType = 33;"))
            {
                skillPerJobList.WriteLine("{0}:Repair:{1}:Electronics:{2}:Null:0:{3}",
                    engineer.jobLevel, engineer.skRepair, engineer.skElectronics, engineer.jobName);
            }

            foreach (var quartermaster in db.Load<Job>("SELECT * FROM Job WHERE jobType = 34;"))
            {
                skillPerJobList.WriteLine("{0}:Ship Ops:{1}:Command:{2}:Intimidate:{3}:{4}",
                    quartermaster.jobLevel, quartermaster.skShipOps, quartermaster.skCommand, quartermaster.skIntimidate, quartermaster.jobName);
            }

            foreach (var wingBomber in db.Load<Job>("SELECT * FROM Job WHERE jobType = 35;"))
            {
                skillPerJobList.WriteLine("{0}:Tactics:{1}:Pilot:{2}:Electronics:{3}:{4}",
                    wingBomber.jobLevel, wingBomber.skTactics, wingBomber.skPilot, wingBomber.skElectronics, wingBomber.jobName);
            }

            foreach (var wingLeader in db.Load<Job>("SELECT * FROM Job WHERE jobType = 36;"))
            {
                skillPerJobList.WriteLine("{0}:Gunnery:{1}:Pilot:{2}:Electronics:{3}:{4}",
                    wingLeader.jobLevel, wingLeader.skGunnery, wingLeader.skPilot, wingLeader.skElectronics, wingLeader.jobName);
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
                    return "Yes";
                default:
                    break;
            }

            return "No";
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

    public class ShipType
    {
        [SQLiteColumn(IsPrimaryKey = true)]
        public int _id { get; set; }
        public string shipTypeName { get; set; }
        public int baseMass { get; set; }
        public int shipCost { get; set; }
        public int smallSlots { get; set; }
        public int mediumSlots { get; set; }
        public int largeSlots { get; set; }
        public int hullPoints { get; set; }
        public int baseArmor { get; set; }
        public int baseDeflection { get; set; }
        public int maxOfficer { get; set; }
        public int maxLifeSupport { get; set; }
        public int maxCraft { get; set; }
        public int baseFuel { get; set; }
    }

    public class SmallCraft
    {
        [SQLiteColumn(IsPrimaryKey = true)]
        public int _id { get; set; }
        public string craftName { get; set; }
        public string description { get; set; }
        public int cost { get; set; }
        public int maxHull { get; set; }
        public int craftTypeId { get; set; }
        public int shipWeaponId { get; set; }
        public int craftArmor { get; set; }
        public int craftDeflection { get; set; }
        public int launchFuelCost { get; set; }
        public int skPilot { get; set; }
        public int skElectronics { get; set; }
        public int skGunnery { get; set; }
        public int baseActionPoints { get; set; }
        public int craftAgile { get; set; }
        public int craftSpeed { get; set; }
        public int repairCost { get; set; }
        public int maintCost { get; set; }
        public int baseToHitShip { get; set; }
        public int baseToHitCraft { get; set; }
        public int baseToDodgeHit { get; set; }
    }
}
