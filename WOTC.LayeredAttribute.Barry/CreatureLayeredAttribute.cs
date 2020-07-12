using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WOTC.LayeredAttribute.Barry.Helpers;

namespace WOTC.LayeredAttribute.Barry
{
    public class CreatureLayeredAttributes : ILayeredAttributes
    {
        private const int _defaultAttributevalue = 0;
        public CreatureLayeredAttributes()
        {
            AttributesDictionary = new Dictionary<AttributeKey, int>();
            LayeredAttributesDictionary = new Dictionary<AttributeKey, List<LayeredEffectDefinition>>();
        }

        public Dictionary<AttributeKey, int> AttributesDictionary { get; set; }
        public Dictionary<AttributeKey, List<LayeredEffectDefinition>> LayeredAttributesDictionary { get; set; }


        private List<LayeredEffectDefinition> GetOrderedLayeredListForAttribute(AttributeKey key)
        {
            if (LayeredAttributesDictionary.ContainsKey(key))
            {
                return LayeredAttributesDictionary[key].OrderBy(x => x.Layer).OrderBy(y => y.TimeStamp).ToList();
            }
            else return null;
        }



        public void SetBaseAttribute(AttributeKey key, int value)
        {
            if (AttributesDictionary.ContainsKey(key))
            {
                AttributesDictionary[key] = value;
            }
            else
            {
                AttributesDictionary.Add(key, value);
            }

        }

        public int GetCurrentAttribute(AttributeKey key)
        {
            var returnValue = GetBaseAttributeValue(key);
            var layers = GetOrderedLayeredListForAttribute(key);
            if (layers != null)
            {
                foreach (var effect in layers)
                {
                    returnValue = EffectHelper.ApplyEffect(effect.Operation, returnValue, effect.Modification);
                }
            }

            return returnValue;
        }

        private int GetBaseAttributeValue(AttributeKey key)
        {
            if (AttributesDictionary.ContainsKey(key)) return AttributesDictionary[key];
            //Return default value if no attributes set 
            return _defaultAttributevalue;
        }



        public void ClearLayeredEffects()
        {
            LayeredAttributesDictionary.Clear();
        }

        public void AddLayeredEffect(LayeredEffectDefinition effect)
        {

            if (!LayeredAttributesDictionary.ContainsKey(effect.Attribute)) LayeredAttributesDictionary[effect.Attribute] = new List<LayeredEffectDefinition>();
            if (effect.TimeStamp == DateTime.MinValue) effect.TimeStamp = DateTime.Now;
            LayeredAttributesDictionary[effect.Attribute].Add(effect);



        }

    }
}
