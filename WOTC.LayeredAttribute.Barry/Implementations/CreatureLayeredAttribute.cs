﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using WOTC.LayeredAttribute.Barry.Helpers;

namespace WOTC.LayeredAttribute.Barry
{
    public class CreatureLayeredAttributes : ILayeredAttributes
    {
        private const int DEFAULT_ATTRIBUTE_VALUE = 0;
      
        //Base attributes collection to keep a separation of concern from the applied effects
        private Dictionary<AttributeKey, int> AttributesDictionary { get; set; } = new Dictionary<AttributeKey, int>();
        //List of  layered effects stored via hashmap of they keys
        private Dictionary<AttributeKey, List<LayeredEffectDefinition>> LayeredAttributesDictionary { get; set; } = new Dictionary<AttributeKey, List<LayeredEffectDefinition>>();


        private List<LayeredEffectDefinition> GetlayeredAttributes(AttributeKey key)
        {
            List<LayeredEffectDefinition> retval = null;
            LayeredAttributesDictionary.TryGetValue(key, out retval);
            if (retval != null) retval =  retval.OrderBy(x => x.Layer).ThenBy(y=>y.TimeStamp).ToList();
            return retval;
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
            var layers = GetlayeredAttributes(key);
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
            var retVal = DEFAULT_ATTRIBUTE_VALUE;
            AttributesDictionary.TryGetValue(key, out retVal);
            //Return default value if no attributes set 
            return retVal;
        }



        public void ClearLayeredEffects()
        {
            //Simple and clear way of removing effects as the base and layers are separated
            LayeredAttributesDictionary.Clear();
        }

        public void AddLayeredEffect(LayeredEffectDefinition effect)
        {
            if (!LayeredAttributesDictionary.ContainsKey(effect.Attribute)) LayeredAttributesDictionary[effect.Attribute] = new List<LayeredEffectDefinition>();
            if (!effect.TimeStamp.HasValue) effect.TimeStamp = DateTime.Now;
            LayeredAttributesDictionary[effect.Attribute].Add(effect);
        }

    }
}
