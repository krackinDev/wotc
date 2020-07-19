using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using WOTC.LayeredAttribute.Barry.Helpers;

namespace WOTC.LayeredAttribute.Barry
{
    public class LayeredAttributes : ILayeredAttributes
    {
        private const int DEFAULT_ATTRIBUTE_VALUE = 0;

        //Base attributes collection to keep a separation of concern from the applied effects
        private Dictionary<AttributeKey, int> AttributesDictionary { get; set; } = new Dictionary<AttributeKey, int>();

        #region FASTER WRITE CODE ATTRIBUTES
        //List of  layered effects stored via hashmap of they keys If write speed is more important
        //private Dictionary<AttributeKey, List<LayeredEffectDefinition>> LayeredAttributesDictionary { get; set; } = new Dictionary<AttributeKey, List<LayeredEffectDefinition>>();
        #endregion
        #region FASTER READ CODE
        //List of  layered effects stored via hashmap of they keys If Read speed is more important
        private Dictionary<AttributeKey, SortedList<int, List<LayeredEffectDefinition>>> SortedLayeredAttributesDictionary { get; set; } = new Dictionary<AttributeKey, SortedList<int, List<LayeredEffectDefinition>>>();
        #endregion
        #region WRITE FASTER CODE
        //private List<LayeredEffectDefinition> GetlayeredAttributes(AttributeKey key)
        //{
        //    List<LayeredEffectDefinition> retval = null;
        //    LayeredAttributesDictionary.TryGetValue(key, out retval);
        //    if (retval != null) retval = retval.OrderBy(x => x.Layer).ToList();
        //    return retval;
        //}
        #endregion
        #region READ FASTER CODE
        private SortedList<int, List<LayeredEffectDefinition>> GetSortedLayeredAttributes(AttributeKey key)
        {
            SortedList<int, List<LayeredEffectDefinition>> retval = null;
            SortedLayeredAttributesDictionary.TryGetValue(key, out retval);

            return retval;
        }
        #endregion
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

            #region WRITE FASTER CODE
            //var layers = GetlayeredAttributes(key);
            //if (layers != null) 
            //{
            //    foreach (var effect in layers)
            //    {
            //        returnValue = EffectHelper.ApplyEffect(effect.Operation, returnValue, effect.Modification);
            //    }
            //}
            #endregion

            #region READ FASTER CODE
            var sortedlayers = GetSortedLayeredAttributes(key);
            if (sortedlayers != null)
            {
                foreach (KeyValuePair<int, List<LayeredEffectDefinition>> pair in sortedlayers)
                {
                    foreach (var effect in pair.Value)
                    {
                        returnValue = EffectHelper.ApplyEffect(effect.Operation, returnValue, effect.Modification);
                    }
                }
            }
            #endregion


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
            SortedLayeredAttributesDictionary.Clear();
        }

        public void AddLayeredEffect(LayeredEffectDefinition effect)
        {

            //TOTAL run time 340ms
            //if (!LayeredAttributesDictionary.ContainsKey(effect.Attribute)) LayeredAttributesDictionary[effect.Attribute] = new List<LayeredEffectDefinition>();
            //LayeredAttributesDictionary[effect.Attribute].Add(effect);


            //TOTAL run time 206ms
            if (!SortedLayeredAttributesDictionary.ContainsKey(effect.Attribute)) SortedLayeredAttributesDictionary[effect.Attribute] = new SortedList<int, List<LayeredEffectDefinition>>();
            if (!SortedLayeredAttributesDictionary[effect.Attribute].ContainsKey(effect.Layer))
            {
                SortedLayeredAttributesDictionary[effect.Attribute][effect.Layer] = new List<LayeredEffectDefinition>();
            }
            SortedLayeredAttributesDictionary[effect.Attribute][effect.Layer].Add(effect);


        }

    }
}
