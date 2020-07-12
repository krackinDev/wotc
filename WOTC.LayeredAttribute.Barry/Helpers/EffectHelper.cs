using System;
using System.Collections.Generic;
using System.Text;

namespace WOTC.LayeredAttribute.Barry.Helpers
{
    public static class EffectHelper
    {
        public static int ApplyEffect(EffectOperation operation, int value, int modifier)
        {
            int result = 0;
            switch (operation)
            {
                case EffectOperation.Add:
                    result = value + modifier;
                    break;
                case EffectOperation.BitwiseAnd:
                    result = value & modifier;
                    break;
                case EffectOperation.BitwiseOr:
                    result = value | modifier;
                    break;
                case EffectOperation.BitwiseXor:
                    result = value ^ modifier;
                    break;
                case EffectOperation.Invalid:
                    result = (int)EffectOperation.Invalid;
                    break;
                case EffectOperation.Multiply:
                    result = value * modifier;
                    break;
                case EffectOperation.Set:
                    result = modifier;
                    break;
                case EffectOperation.Subtract:
                    result = value - modifier;
                    break;
            }
            return result;
        }

    }
}
