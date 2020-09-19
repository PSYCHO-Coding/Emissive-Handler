using Sandbox.ModAPI;
using System.Linq;
using System.Threading.Tasks;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Game.Entities;
using Sandbox.ModAPI.Interfaces;
using Sandbox.ModAPI.Interfaces.Terminal;
using SpaceEngineers.Game.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRageMath;
using VRage.Utils;

using PSYCHO.ApplyEmissives;

/*
    IMPORTANT!
    Note, this only works for terminal blocks. Simply said, if a block you'd like to add emissive handling to is available in the ships terminal, you can add to it.
*/

namespace PSYCHO.ApplyEmissivesSettings
{
    public class Settings
    {
        ApplyEmissivesHandler EmissiveDataInstance => ApplyEmissivesHandler.EmissiveDataInstance;

        EmissiveData emissiveData;

        // ALL DATA GOES IN HERE!
        public void ConstructData()
        {
            // DECLARE A NEW DATA SET
            emissiveData = new EmissiveData();

            // SET DATA
            emissiveData.FullyWorkingEmissiveColor = new Color(0, 255, 0);
            emissiveData.FullyWorkingEmissiveGlow = 1f;
            emissiveData.BustedEmissiveColor = new Color(255, 0, 0);
            emissiveData.BustedEmissiveGlow = 1f;

            // ADD DATA TO BLOCKS FOR BY MATERIAL NAME
            EmissiveDataInstance.AddData("SomeSubtypeID", "EmissiveMaterialName", emissiveData);
            EmissiveDataInstance.AddData("SomeSubtypeID", "OtherEmissiveMaterialName", emissiveData);
            EmissiveDataInstance.AddData("SomeOtherSubtypeID", "EmissiveMaterialName", emissiveData);

            // DECLARE A NEW DATA SET
            emissiveData = new EmissiveData();

            // SET DATA
            emissiveData.FullyWorkingEmissiveColor = new Color(0, 0, 255);
            emissiveData.FullyWorkingEmissiveGlow = 1f;
            emissiveData.BustedEmissiveColor = new Color(255, 0, 0);
            emissiveData.BustedEmissiveGlow = 1f;

            EmissiveDataInstance.AddData("SomeOtherOtherSubtypeID", "EmissiveMaterialName", emissiveData);
        }









        // =======================
        // PLEASE IGNORE THE BELOW
        // DEV LEFTOVERS
        // =======================

        /*
        Color FullyWorkingEmissiveColor = new Color(242, 110, 80); // Emissive color when the block is working and enabled.
        float FullyWorkingEmissiveGlow = 1f;                       // Emissive glow when the block is working and enabled.
        Color BustedEmissiveColor = new Color(255, 0, 0);          // Emissive color when the block is broken or disabled.
        float BustedEmissiveGlow = 1f;                             // Emissive glow when the block is broken or disabled.

        string[] MaterialName =
            { "Emissive", "Emissive2" }
        ;

        // UNCOMMENT THE BLOCK TYPES YOU WISH TO APPLY THIS SCRIPT ON
        private readonly HashSet<MyObjectBuilderType> BlockTypes = new HashSet<MyObjectBuilderType>()
        {
            //typeof(MyObjectBuilder_BatteryBlock),
            //typeof(MyObjectBuilder_ButtonPanel),
            //typeof(MyObjectBuilder_Beacon),
            //typeof(MyObjectBuilder_Cockpit),
            //typeof(MyObjectBuilder_AdvancedDoor),
            //typeof(MyObjectBuilder_AirtightHangarDoor),
            //typeof(MyObjectBuilder_AirtightSlideDoor),
            //typeof(MyObjectBuilder_AirVent),
            //typeof(MyObjectBuilder_Assembler),
            //typeof(MyObjectBuilder_CameraBlock),
            //typeof(MyObjectBuilder_Collector),
            //typeof(MyObjectBuilder_ContractBlock),
            //typeof(MyObjectBuilder_Conveyor),
            //typeof(MyObjectBuilder_ConveyorConnector),
            //typeof(MyObjectBuilder_ConveyorSorter),
            //typeof(MyObjectBuilder_ConveyorTurretBase),
            //typeof(MyObjectBuilder_CryoChamber),
            //typeof(MyObjectBuilder_Decoy),
            //typeof(MyObjectBuilder_Door),
            //typeof(MyObjectBuilder_Drill),
            //typeof(MyObjectBuilder_GasTank),
            //typeof(MyObjectBuilder_GravityGenerator),
            //typeof(MyObjectBuilder_Gyro),
            //typeof(MyObjectBuilder_HydrogenEngine),
            //typeof(MyObjectBuilder_InteriorLight),
            //typeof(MyObjectBuilder_InteriorTurret),
            //typeof(MyObjectBuilder_Jukebox),
            //typeof(MyObjectBuilder_JumpDrive),
            //typeof(MyObjectBuilder_Kitchen),
            //typeof(MyObjectBuilder_LaserAntenna),
            //typeof(MyObjectBuilder_LargeGatlingTurret),
            //typeof(MyObjectBuilder_LargeMissileTurret),
            //typeof(MyObjectBuilder_LCDPanelsBlock),
            //typeof(MyObjectBuilder_LightingBlock),
            //typeof(MyObjectBuilder_MedicalRoom),
            //typeof(MyObjectBuilder_MergeBlock),
            //typeof(MyObjectBuilder_MotorAdvancedRotor),
            //typeof(MyObjectBuilder_MotorAdvancedStator),
            //typeof(MyObjectBuilder_MotorRotor),
            //typeof(MyObjectBuilder_MotorStator),
            //typeof(MyObjectBuilder_MotorSuspension),
            //typeof(MyObjectBuilder_MyProgrammableBlock),
            //typeof(MyObjectBuilder_OreDetector),
            //typeof(MyObjectBuilder_OxygenFarm),
            //typeof(MyObjectBuilder_OxygenGenerator),
            //typeof(MyObjectBuilder_OxygenTank),
            //typeof(MyObjectBuilder_Parachute),
            //typeof(MyObjectBuilder_Passage),
            //typeof(MyObjectBuilder_PistonBase),
            //typeof(MyObjectBuilder_PistonTop),
            //typeof(MyObjectBuilder_Planter),
            //typeof(MyObjectBuilder_Projector),
            //typeof(MyObjectBuilder_RadioAntenna),
            //typeof(MyObjectBuilder_Reactor),
            //typeof(MyObjectBuilder_Refinery),
            //typeof(MyObjectBuilder_ReflectorLight),
            //typeof(MyObjectBuilder_RemoteControl),
            //typeof(MyObjectBuilder_SafeZone),
            //typeof(MyObjectBuilder_SensorBlock),
            //typeof(MyObjectBuilder_ShipConnector),
            //typeof(MyObjectBuilder_ShipGrinder),
            //typeof(MyObjectBuilder_ShipWelder),
            //typeof(MyObjectBuilder_SignalLight),
            //typeof(MyObjectBuilder_SmallMissileLauncher),
            //typeof(MyObjectBuilder_SmallMissileLauncherReload),
            //typeof(MyObjectBuilder_SolarPanel),
            //typeof(MyObjectBuilder_SoundBlock),
            //typeof(MyObjectBuilder_SpaceBall),
            //typeof(MyObjectBuilder_StoreBlock),
            //typeof(MyObjectBuilder_SurvivalKit),
            //typeof(MyObjectBuilder_TextPanel),
            //typeof(MyObjectBuilder_Thrust),
            //typeof(MyObjectBuilder_TimerBlock),
            //typeof(MyObjectBuilder_UpgradeModule),
            //typeof(MyObjectBuilder_VendingMachine),
            //typeof(MyObjectBuilder_VirtualMass),
            //typeof(MyObjectBuilder_Warhead),
            //typeof(MyObjectBuilder_Welder),
            //typeof(MyObjectBuilder_WindTurbine),
        };
        */
    }
}
