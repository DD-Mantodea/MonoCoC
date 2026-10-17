using MonoCoC.Extensions;
using System;

namespace MonoCoC.Utils
{
    public static class MeasurementsUtils
    {
        public static string FootInchOrMetre(float inches, int precision = 2)
        {
            if (GameSettings.Metric)
                return MathF.Round(inches * 2.54f / MathF.Pow(10, precision), precision) + " meter";

            return (int)MathF.Floor(inches / 12) + " foot " + (int)MathF.Round(inches % 12) + " inch";
        }

        public static string BriefHeight(float inches)
        {
            if (GameSettings.Metric)
                return MathF.Round(inches * 2.54f) + "Measurements.CentimetreShort".Localized();

            var feet = (int)(inches / 12);

            var rest = (int)(inches % 12);

            var text = feet > 0 ? feet + "Measurements.FootSymbol".Localized() : "";

            if (rest > 0)
                text += rest + "Measurements.InchSymbol".Localized();

            return text;
        }

        public static string NumInchesOrCentimetres(float inches)
        {
            if (inches < 1)
                return InchesOrCentimetres(inches);

            if (GameSettings.Metric)
            {
                var centimetres = MathF.Round(inches * 2.54f);

                return NumberUtils.Num2Text((int)centimetres) + " " + (centimetres <= 1 ? "Measurements.Centimetre" : "Measurements.Centimetres").Localized();
            }

            var value = (int)MathF.Round(inches);

            if (value % 12 == 0)
                return value == 12 ? "Measurements.AFoot".Localized() : NumberUtils.Num2Text(value / 12) + " " + "Measurements.Feet".Localized();

            return NumberUtils.Num2Text(value) + " " + (value == 1 ? "Measurements.Inch" : "Measurements.Inches").Localized();
        }

        public static string ShortSuffix(float inches, int precision = 1)
        {
            var value = GameSettings.Metric ? inches * 2.54f : inches;

            var rounded = MathF.Round(value, precision);

            var suffix = "-" + (GameSettings.Metric ? "Measurements.CentimetreShort".Localized() : "Measurements.Inch".Localized());

            return rounded.ToString(System.Globalization.CultureInfo.InvariantCulture) + suffix;
        }

        public static string InchOrCentimetre(float inches)
        {
            var value = MathF.Round(GameSettings.Metric ? inches * 2.54f : inches);

            return value + " " + (GameSettings.Metric ? "Measurements.Centimetre" : "Measurements.Inch").Localized();
        }

        public static string InchesOrCentimetres(float inches, int precision = 1)
        {
            var value = GameSettings.Metric ? inches * 2.54f : inches;

            var rounded = MathF.Round(value, precision);

            var singular = MathF.Abs(rounded - 1f) < 0.0001f;

            var key = GameSettings.Metric
                ? (singular ? "Measurements.Centimetre" : "Measurements.Centimetres")
                : (singular ? "Measurements.Inch" : "Measurements.Inches");

            return $"{rounded} {key.Localized()}";
        }
    }
}
