using Sandbox.ModAPI;
using System.Collections.Generic;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRageMath;
using VRage.Utils;
using VRage.Game;
using VRage.Game.Entity;

// =======================
// PLEASE DO NOT EDIT THIS
// =======================

namespace PSYCHO.ApplyEmissives
{
    public class EmissiveData
    {
        public string EmissiveMaterialName { get; set; }
        public Color FullyWorkingEmissiveColor { get; set; }
        public float FullyWorkingEmissiveGlow { get; set; }
        public Color BustedEmissiveColor { get; set; }
        public float BustedEmissiveGlow { get; set; }
    }

    [MySessionComponentDescriptor(MyUpdateOrder.NoUpdate)]

    class ApplyEmissivesHandler : MySessionComponentBase
    {
        public static ApplyEmissivesHandler EmissiveDataInstance;

        public Dictionary<string, List<EmissiveData>> EmissiveUserData = new Dictionary<string, List<EmissiveData>>();

        public override void LoadData()
        {
            EmissiveDataInstance = this;
            ApplyEmissivesSettings.Settings settings = new ApplyEmissivesSettings.Settings();
            settings.ConstructData();
        }

        protected override void UnloadData()
        {
            EmissiveDataInstance = null;
        }

        public void AddData(string subtypeId, string matName, EmissiveData data)
        {
            data.EmissiveMaterialName = matName;

            if (!EmissiveUserData.ContainsKey(subtypeId))
                EmissiveUserData[subtypeId] = new List<EmissiveData>();
            EmissiveUserData[subtypeId].Add(data);
        }
    }

    /*
    [MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation)]

    public class ApplySessionEmissivesLogic : MySessionComponentBase
    {
        ushort Tick = 0;

        public override void LoadData()
        {

        }

        public override void BeforeStart()
        {
            try
            {

            }
            catch (Exception e)
            {

            }
        }

        protected override void UnloadData()
        {

        }

        public override void UpdateAfterSimulation()
        {
            Tick++;

            if (Tick % 3 == 0)
            {
                if (Tick == ushort.MaxValue)
                    Tick = 0;
            }

            RunLogic();
        }

        void RunLogic()
        {

        }
    }
    */

    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_TerminalBlock), false)]

    public class ApplyEmissivesLogic : MyGameLogicComponent
    {
        //private int Tick;
        IMyTerminalBlock Block;
        Vector3 BlockColor = Vector3.Zero;
        MyStringHash BlockTexture;
        Color EmissiveColor = new Color(0, 0, 0);

        List<EmissiveData> EmissiveDataSet = new List<EmissiveData>();

        ApplyEmissivesHandler EmissiveDataInstance => ApplyEmissivesHandler.EmissiveDataInstance;
        PSYCHO.ApplyEmissivesSettings.Settings SettingsData = new ApplyEmissivesSettings.Settings();

        List<MyEntitySubpart> SubpartList = new List<MyEntitySubpart>();
        bool HasSubpart = false;

        public override void Init(MyObjectBuilder_EntityBase objectBuilder)
        {
            if (MyAPIGateway.Utilities.IsDedicated)
                return;

            Block = Entity as IMyTerminalBlock;

            /*
            if (!MyUserData.ThrusterSubtypeIDs.Contains(block.BlockDefinition.SubtypeId))
            {
                Block = null;
                return;
            }
            */

            NeedsUpdate = MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
        }

        public override void UpdateOnceBeforeFrame()
        {
            if (Block == null)
                return;


            var subtypeId = Block.BlockDefinition.SubtypeId;

            if (EmissiveDataInstance.EmissiveUserData.ContainsKey(subtypeId))
            {
                EmissiveDataSet = EmissiveDataInstance.EmissiveUserData[subtypeId];
            }
            else
            {
                return;
            }

            // Not really used but ready if needed.
            Dictionary<string, IMyModelDummy> modelDummy = new Dictionary<string, IMyModelDummy>();
            MyEntitySubpart foundSubpart;

            Block.Model.GetDummies(modelDummy);
            foreach (var subpart in modelDummy.Keys)
            {
                if (Block.TryGetSubpart(subpart, out foundSubpart))
                {
                    SubpartList.Add(foundSubpart);
                }
            }

            if (SubpartList.Count > 0)
            {
                HasSubpart = true;
            }

            // Hook to events.
            Block.PropertiesChanged += PropertiesChanged;
            Block.IsWorkingChanged += IsWorkingChanged;

            NeedsUpdate = MyEntityUpdateEnum.EACH_100TH_FRAME;
        }

        private void PropertiesChanged(IMyTerminalBlock obj)
        {
            BlockColor = Block.SlimBlock.ColorMaskHSV;
            BlockTexture = Block.SlimBlock.SkinSubtypeId;

            CheckAndSetEmissives(true);
        }

        private void IsWorkingChanged(IMyCubeBlock obj)
        {
            BlockColor = Block.SlimBlock.ColorMaskHSV;
            BlockTexture = Block.SlimBlock.SkinSubtypeId;

            CheckAndSetEmissives(true);
        }

        // Cleanup.
        public override void Close()
        {
            Block.PropertiesChanged -= PropertiesChanged;
            Block.IsWorkingChanged -= IsWorkingChanged;

            Block = null;
        }

        // Mostly needed due to not having a 'block recolored' event.
        public override void UpdateAfterSimulation100()
        {
            if (Block == null || Block.MarkedForClose || Block.Closed)
            {
                NeedsUpdate = MyEntityUpdateEnum.NONE;
                return;
            }

            // Only if we want to use it within some frame set not available natively.
            // In this instance, it's cosmetic in nature so we don't care if the effects apply at a delay so we're handling it in a update 100 (each 100th frame).
            /*
            Tick++;

            if (Tick % 3 == 0)
            {
                // Do conditionals here...
            }
            */

            bool updateEmissives = false;

            if (BlockColor != Block.SlimBlock.ColorMaskHSV)
            {
                BlockColor = Block.SlimBlock.ColorMaskHSV;
                updateEmissives = true;
            }

            if (BlockTexture != Block.SlimBlock.SkinSubtypeId)
            {
                BlockTexture = Block.SlimBlock.SkinSubtypeId;
                updateEmissives = true;
            }

            if (updateEmissives)
            {
                CheckAndSetEmissives(true);
            }
        }

        // Conditions and colors go here.
        void CheckAndSetEmissives(bool _force = false)
        {
            foreach (var emissive in EmissiveDataSet)
            {
                if (Block.IsFunctional)
                {
                    if (Block.IsWorking)
                    {
                        if (EmissiveColor != emissive.FullyWorkingEmissiveColor || _force)
                        {
                            EmissiveColor = emissive.FullyWorkingEmissiveColor;
                            Block.SetEmissiveParts(emissive.EmissiveMaterialName, EmissiveColor, emissive.FullyWorkingEmissiveGlow);

                            if (HasSubpart)
                            {
                                Block.SetEmissivePartsForSubparts(emissive.EmissiveMaterialName, EmissiveColor, emissive.FullyWorkingEmissiveGlow);
                                // PLACEHOLDER
                                //ApplyEmissiveToSubparts(SubpartList, emissive.EmissiveMaterialName, EmissiveColor, emissive.FullyWorkingEmissiveGlow);
                            }
                        }
                    }
                    else
                    {
                        if (EmissiveColor != emissive.BustedEmissiveColor || _force)
                        {
                            EmissiveColor = emissive.BustedEmissiveColor;
                            Block.SetEmissiveParts(emissive.EmissiveMaterialName, EmissiveColor, emissive.BustedEmissiveGlow);

                            if (HasSubpart)
                            {
                                Block.SetEmissivePartsForSubparts(emissive.EmissiveMaterialName, EmissiveColor, emissive.BustedEmissiveGlow);
                                // PLACEHOLDER
                                //ApplyEmissiveToSubparts(SubpartList, emissive.EmissiveMaterialName, EmissiveColor, emissive.BustedEmissiveGlow);
                            }
                        }
                    }
                }
                else
                {
                    if (EmissiveColor != emissive.BustedEmissiveColor || _force)
                    {
                        EmissiveColor = emissive.BustedEmissiveColor;
                        Block.SetEmissiveParts(emissive.EmissiveMaterialName, EmissiveColor, emissive.BustedEmissiveGlow);

                        if (HasSubpart)
                        {
                            Block.SetEmissivePartsForSubparts(emissive.EmissiveMaterialName, EmissiveColor, emissive.BustedEmissiveGlow);
                            // PLACEHOLDER
                            //ApplyEmissiveToSubparts(SubpartList, emissive.EmissiveMaterialName, EmissiveColor, emissive.BustedEmissiveGlow);
                        }
                    }
                }
            }
        }

        void ApplyEmissiveToSubparts(List<MyEntitySubpart> list, string materialName, Color emissiveColor, float emissiveGlow)
        {
            foreach (var subpart in list)
            {
                subpart.SetEmissiveParts(materialName, emissiveColor, emissiveGlow);
            }
        }
    }
}
