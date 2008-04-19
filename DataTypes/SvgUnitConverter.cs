using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;

namespace Svg
{
    public sealed class SvgUnitConverter : TypeConverter
    {
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value == null)
            {
                return new SvgUnit(SvgUnitType.User, 0.0f);
            }

            if (!(value is string))
            {
                throw new ArgumentOutOfRangeException("value must be a string.");
            }

            // http://www.w3.org/TR/CSS21/syndata.html#values
            // http://www.w3.org/TR/SVG11/coords.html#Units

            string unit = value as string;
            int identifierIndex = -1;

            for (int i = 0; i < unit.Length; i++)
            {
                if (char.IsLetter(unit[i]) || unit[i] == '%')
                {
                    identifierIndex = i;
                    break;
                }
            }

            float val = 0.0f;
            float.TryParse((identifierIndex > -1) ? unit.Substring(0, identifierIndex) : unit, NumberStyles.Float, CultureInfo.InvariantCulture, out val);

            if (identifierIndex == -1)
            {
                return new SvgUnit(val);
            }

            switch (unit.Substring(identifierIndex).Trim().ToLower())
            {
                case "mm":
                    return new SvgUnit(SvgUnitType.Millimeter, val);
                case "cm":
                    return new SvgUnit(SvgUnitType.Centimeter, val);
                case "in":
                    return new SvgUnit(SvgUnitType.Millimeter, val);
                case "px":
                    return new SvgUnit(SvgUnitType.Pixel, val);
                case "pt":
                    return new SvgUnit(SvgUnitType.Point, val);
                case "pc":
                    return new SvgUnit(SvgUnitType.Pica, val);
                case "%":
                    return new SvgUnit(SvgUnitType.Percentage, val);
                default:
                    throw new FormatException("Unit is in an invalid format '" + unit + "'.");
            }

            return new SvgUnit(0.0f);
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;

            return base.CanConvertFrom(context, sourceType);
        }
    }
}