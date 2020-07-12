using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WOTC.LayeredAttribute.Barry;

namespace WOTC.LayeredAttribute.Tests
{
    [TestClass]
    public class AttributesTest
    {
        public CreatureLayeredAttributes Creature { get; set; }
        //Crystalline Giant example creature
        const int BASEPOWERTOUGHNESS = 3;
        //Shock Instant
        const int SHOCKDAMAGE = 2;
        //Steel Overseer Tap
        const int PLUSONECOUNTERMODIFIER = 1;
        //Duress instant
        const int DURESSMINUSCOUNTERMODIFIER = 2;

        //FieryEmancipation Effect
        const int MANACOSTMULTIPLIER = 3;


        [TestInitialize]
        public void Initialize()
        {
            Creature = new CreatureLayeredAttributes();
            SetPowerAttribute();
            SetToughnessAttribute();

        }

        [TestMethod]
        public void CheckDefaultvalues()
        {
            var newCreature = new CreatureLayeredAttributes();
            Assert.IsTrue(newCreature.GetCurrentAttribute(AttributeKey.Color) == 0);
            Assert.IsTrue(newCreature.GetCurrentAttribute(AttributeKey.Controller) == 0);
            Assert.IsTrue(newCreature.GetCurrentAttribute(AttributeKey.ConvertedManaCost) == 0);
            Assert.IsTrue(newCreature.GetCurrentAttribute(AttributeKey.Invalid) == 0);
            Assert.IsTrue(newCreature.GetCurrentAttribute(AttributeKey.Loyalty) == 0);
            Assert.IsTrue(newCreature.GetCurrentAttribute(AttributeKey.Power) == 0);
            Assert.IsTrue(newCreature.GetCurrentAttribute(AttributeKey.Subtypes) == 0);
            Assert.IsTrue(newCreature.GetCurrentAttribute(AttributeKey.Supertypes) == 0);
            Assert.IsTrue(newCreature.GetCurrentAttribute(AttributeKey.Toughness) == 0);
            Assert.IsTrue(newCreature.GetCurrentAttribute(AttributeKey.Types) == 0);
        }

        [TestMethod]
        public void SetPowerAttribute()
        {
            Creature.SetBaseAttribute(AttributeKey.Power, BASEPOWERTOUGHNESS);
            Assert.IsTrue(Creature.GetCurrentAttribute(AttributeKey.Power) == 3);
        }
        [TestMethod]
        public void SetToughnessAttribute()
        {
            Creature.SetBaseAttribute(AttributeKey.Toughness, BASEPOWERTOUGHNESS);
            Assert.IsTrue(Creature.GetCurrentAttribute(AttributeKey.Toughness) == 3);
        }
        [TestMethod]
        public void ShockOnlyCreature()
        {
            Creature.AddLayeredEffect(Shock(1));
            Assert.IsTrue(Creature.GetCurrentAttribute(AttributeKey.Toughness) == BASEPOWERTOUGHNESS - SHOCKDAMAGE);
        }

        [TestMethod]
        public void Shock_PlusOne_Creature()
        {
            Creature.AddLayeredEffect(Shock(1));
            Creature.AddLayeredEffect(PlusOnePower(1));
            Creature.AddLayeredEffect(PlusOneToughness(1));


            Assert.IsTrue(Creature.GetCurrentAttribute(AttributeKey.Toughness) == BASEPOWERTOUGHNESS - SHOCKDAMAGE + PLUSONECOUNTERMODIFIER);
            Assert.IsTrue(Creature.GetCurrentAttribute(AttributeKey.Power) == BASEPOWERTOUGHNESS + PLUSONECOUNTERMODIFIER);
        }

        [TestMethod]
        public void Shock_PlusOne_Duress_Creature()
        {
            Creature.AddLayeredEffect(Shock(1));
            Creature.AddLayeredEffect(PlusOneToughness(1));
            Creature.AddLayeredEffect(PlusOnePower(1));
            Creature.AddLayeredEffect(DuressPower(1));
            Creature.AddLayeredEffect(DuressToughness(1));


            Assert.IsTrue(Creature.GetCurrentAttribute(AttributeKey.Toughness) == 0);
            Assert.IsTrue(Creature.GetCurrentAttribute(AttributeKey.Power) == 2);
        }

        [TestMethod]
        public void Shocklayer2_plusone_layer2_plusone_layer2_duress_layer1_shock_layer1_Creature()
        {

            Creature.AddLayeredEffect(Shock(2));
            Creature.AddLayeredEffect(PlusOneToughness(1));
            Creature.AddLayeredEffect(PlusOnePower(1));
            Creature.AddLayeredEffect(PlusOneToughness(2));
            Creature.AddLayeredEffect(PlusOnePower(2));
            Creature.AddLayeredEffect(DuressPower(1));
            Creature.AddLayeredEffect(DuressToughness(1));
            Creature.AddLayeredEffect(Shock(1));




            Assert.IsTrue(Creature.GetCurrentAttribute(AttributeKey.Toughness) == BASEPOWERTOUGHNESS - SHOCKDAMAGE + PLUSONECOUNTERMODIFIER - DURESSMINUSCOUNTERMODIFIER - SHOCKDAMAGE + PLUSONECOUNTERMODIFIER);
            Assert.IsTrue(Creature.GetCurrentAttribute(AttributeKey.Power) == BASEPOWERTOUGHNESS + PLUSONECOUNTERMODIFIER + PLUSONECOUNTERMODIFIER - DURESSMINUSCOUNTERMODIFIER);
        }

        [TestMethod]
        public void MultiEffectReset_Creature()
        {

            Creature.AddLayeredEffect(Shock(2));
            Creature.AddLayeredEffect(PlusOneToughness(1));
            Creature.AddLayeredEffect(PlusOnePower(1));
            Creature.AddLayeredEffect(PlusOneToughness(2));
            Creature.AddLayeredEffect(PlusOnePower(2));
            Creature.AddLayeredEffect(DuressPower(1));
            Creature.AddLayeredEffect(DuressToughness(1));
            Creature.AddLayeredEffect(Shock(1));
            Assert.IsTrue(Creature.GetCurrentAttribute(AttributeKey.Toughness) == BASEPOWERTOUGHNESS - SHOCKDAMAGE + PLUSONECOUNTERMODIFIER - DURESSMINUSCOUNTERMODIFIER - SHOCKDAMAGE + PLUSONECOUNTERMODIFIER);
            Assert.IsTrue(Creature.GetCurrentAttribute(AttributeKey.Power) == BASEPOWERTOUGHNESS + PLUSONECOUNTERMODIFIER + PLUSONECOUNTERMODIFIER - DURESSMINUSCOUNTERMODIFIER);
            Creature.ClearLayeredEffects();

            Assert.IsTrue(Creature.GetCurrentAttribute(AttributeKey.Toughness) == BASEPOWERTOUGHNESS);
            Assert.IsTrue(Creature.GetCurrentAttribute(AttributeKey.Power) == BASEPOWERTOUGHNESS);
        }

        [TestMethod]
        public void MultiEffectSetLarge_lateLayer_Creature()
        {
            int largePowerToughness = 15;
            Creature.AddLayeredEffect(Shock(2));
            Creature.AddLayeredEffect(PlusOneToughness(1));
            Creature.AddLayeredEffect(PlusOnePower(1));
            Creature.AddLayeredEffect(PlusOneToughness(2));
            Creature.AddLayeredEffect(PlusOnePower(2));
            Creature.AddLayeredEffect(DuressPower(1));
            Creature.AddLayeredEffect(DuressToughness(1));
            Creature.AddLayeredEffect(Shock(1));
            Assert.IsTrue(Creature.GetCurrentAttribute(AttributeKey.Toughness) == BASEPOWERTOUGHNESS - SHOCKDAMAGE + PLUSONECOUNTERMODIFIER - DURESSMINUSCOUNTERMODIFIER - SHOCKDAMAGE + PLUSONECOUNTERMODIFIER);
            Assert.IsTrue(Creature.GetCurrentAttribute(AttributeKey.Power) == BASEPOWERTOUGHNESS + PLUSONECOUNTERMODIFIER + PLUSONECOUNTERMODIFIER - DURESSMINUSCOUNTERMODIFIER);
            Creature.ClearLayeredEffects();

            Assert.IsTrue(Creature.GetCurrentAttribute(AttributeKey.Toughness) == BASEPOWERTOUGHNESS);
            Assert.IsTrue(Creature.GetCurrentAttribute(AttributeKey.Power) == BASEPOWERTOUGHNESS);

            Creature.AddLayeredEffect(new LayeredEffectDefinition() { Attribute = AttributeKey.Power, Layer = 10, Modification = largePowerToughness, Operation = EffectOperation.Set });
            Creature.AddLayeredEffect(new LayeredEffectDefinition() { Attribute = AttributeKey.Toughness, Layer = 10, Modification = largePowerToughness, Operation = EffectOperation.Set });

            Assert.IsTrue(Creature.GetCurrentAttribute(AttributeKey.Toughness) == largePowerToughness);
            Assert.IsTrue(Creature.GetCurrentAttribute(AttributeKey.Power) == largePowerToughness);
        }


        [TestMethod]
        public void MultiEffectSetLarge_lateLayer_Creature_shock_shock()
        {
            int largePowerToughness = 15;
            Creature.AddLayeredEffect(Shock(2));
            Creature.AddLayeredEffect(PlusOneToughness(1));
            Creature.AddLayeredEffect(PlusOnePower(1));
            Creature.AddLayeredEffect(PlusOneToughness(2));
            Creature.AddLayeredEffect(PlusOnePower(2));
            Creature.AddLayeredEffect(DuressPower(1));
            Creature.AddLayeredEffect(DuressToughness(1));
            Creature.AddLayeredEffect(Shock(1));
            Assert.IsTrue(Creature.GetCurrentAttribute(AttributeKey.Toughness) == BASEPOWERTOUGHNESS - SHOCKDAMAGE + PLUSONECOUNTERMODIFIER - DURESSMINUSCOUNTERMODIFIER - SHOCKDAMAGE + PLUSONECOUNTERMODIFIER);
            Assert.IsTrue(Creature.GetCurrentAttribute(AttributeKey.Power) == BASEPOWERTOUGHNESS + PLUSONECOUNTERMODIFIER + PLUSONECOUNTERMODIFIER - DURESSMINUSCOUNTERMODIFIER);
            Creature.ClearLayeredEffects();

            Assert.IsTrue(Creature.GetCurrentAttribute(AttributeKey.Toughness) == BASEPOWERTOUGHNESS);
            Assert.IsTrue(Creature.GetCurrentAttribute(AttributeKey.Power) == BASEPOWERTOUGHNESS);

            Creature.AddLayeredEffect(new LayeredEffectDefinition() { Attribute = AttributeKey.Power, Layer = 10, Modification = largePowerToughness, Operation = EffectOperation.Set });
            Creature.AddLayeredEffect(new LayeredEffectDefinition() { Attribute = AttributeKey.Toughness, Layer = 10, Modification = largePowerToughness, Operation = EffectOperation.Set });

            Assert.IsTrue(Creature.GetCurrentAttribute(AttributeKey.Toughness) == largePowerToughness);
            Assert.IsTrue(Creature.GetCurrentAttribute(AttributeKey.Power) == largePowerToughness);

            Creature.AddLayeredEffect(Shock(11));
            Creature.AddLayeredEffect(Shock(11));
            Assert.IsTrue(Creature.GetCurrentAttribute(AttributeKey.Toughness) == largePowerToughness - (2 * SHOCKDAMAGE));
        }

        [TestMethod]
        public void MultiEffect_then_SetBaseToughness_Creature_shock_shock()
        {

            Creature.AddLayeredEffect(Shock(2));
            Creature.AddLayeredEffect(PlusOneToughness(1));
            Creature.AddLayeredEffect(PlusOnePower(1));
            Creature.AddLayeredEffect(PlusOneToughness(2));
            Creature.AddLayeredEffect(PlusOnePower(2));
            Creature.AddLayeredEffect(DuressPower(1));
            Creature.AddLayeredEffect(DuressToughness(1));
            Creature.AddLayeredEffect(Shock(1));
            Assert.IsTrue(Creature.GetCurrentAttribute(AttributeKey.Toughness) == BASEPOWERTOUGHNESS - SHOCKDAMAGE + PLUSONECOUNTERMODIFIER - DURESSMINUSCOUNTERMODIFIER - SHOCKDAMAGE + PLUSONECOUNTERMODIFIER);
            Assert.IsTrue(Creature.GetCurrentAttribute(AttributeKey.Power) == BASEPOWERTOUGHNESS + PLUSONECOUNTERMODIFIER + PLUSONECOUNTERMODIFIER - DURESSMINUSCOUNTERMODIFIER);
            Creature.ClearLayeredEffects();

            Assert.IsTrue(Creature.GetCurrentAttribute(AttributeKey.Toughness) == BASEPOWERTOUGHNESS);
            Assert.IsTrue(Creature.GetCurrentAttribute(AttributeKey.Power) == BASEPOWERTOUGHNESS);

            Creature.SetBaseAttribute(AttributeKey.Power, 1);
            Creature.SetBaseAttribute(AttributeKey.Toughness, 1);



            Creature.AddLayeredEffect(Shock(11));
            Creature.AddLayeredEffect(Shock(11));
            Assert.IsTrue(Creature.GetCurrentAttribute(AttributeKey.Toughness) == -3);
        }



        [TestMethod]
        public void IncreasaManaCostWithTriple_multiple()
        {
            Creature.SetBaseAttribute(AttributeKey.ConvertedManaCost, 3);
            Creature.AddLayeredEffect(ManacostMultiplier(1));

            Assert.IsTrue(Creature.GetCurrentAttribute(AttributeKey.ConvertedManaCost) == MANACOSTMULTIPLIER * 3);
        }


        [TestMethod]
        public void IncreasaManaCostWithTripleTriple()
        {
            Creature.SetBaseAttribute(AttributeKey.ConvertedManaCost, 3);
            Creature.AddLayeredEffect(ManacostMultiplier(1));
            Creature.AddLayeredEffect(ManacostMultiplier(2));
            Assert.IsTrue(Creature.GetCurrentAttribute(AttributeKey.ConvertedManaCost) == MANACOSTMULTIPLIER * MANACOSTMULTIPLIER * 3);
        }


        #region HelperEffectsForCommonSpells
        private static LayeredEffectDefinition PlusOnePower(int layer)
        {
            return new LayeredEffectDefinition() { Attribute = AttributeKey.Power, Layer = layer, Modification = PLUSONECOUNTERMODIFIER, Operation = EffectOperation.Add };
        }

        private static LayeredEffectDefinition ManacostMultiplier(int layer)
        {
            return new LayeredEffectDefinition() { Attribute = AttributeKey.ConvertedManaCost, Layer = layer, Modification = MANACOSTMULTIPLIER, Operation = EffectOperation.Multiply };
        }




        private static LayeredEffectDefinition PlusOneToughness(int layer)
        {
            return new LayeredEffectDefinition() { Attribute = AttributeKey.Toughness, Layer = layer, Modification = PLUSONECOUNTERMODIFIER, Operation = EffectOperation.Add };
        }

        private static LayeredEffectDefinition DuressPower(int layer)
        {
            return new LayeredEffectDefinition() { Attribute = AttributeKey.Power, Layer = layer, Modification = DURESSMINUSCOUNTERMODIFIER, Operation = EffectOperation.Subtract };
        }
        private static LayeredEffectDefinition DuressToughness(int layer)
        {
            return new LayeredEffectDefinition() { Attribute = AttributeKey.Toughness, Layer = layer, Modification = DURESSMINUSCOUNTERMODIFIER, Operation = EffectOperation.Subtract };
        }
        private static LayeredEffectDefinition Shock(int layer)
        {
            return new LayeredEffectDefinition() { Attribute = AttributeKey.Toughness, Layer = layer, Modification = SHOCKDAMAGE, Operation = EffectOperation.Subtract };
        }
        #endregion
    }

}
