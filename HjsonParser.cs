using Hjson;
using System;
using System.Text.RegularExpressions;

namespace MonoCoC
{
    public static class HjsonParser
    {
        public static JsonValue Parse(string text)
        {
            try
            {
                return HjsonValue.Parse(text);
            }
            catch (Exception exception)
            {
                var message = Regex.Replace(
                    exception.Message,
                    @"(?<prefix>got ')(?<code>\d+)(?=')",
                    match => match.Groups["prefix"].Value +
                             (char)int.Parse(match.Groups["code"].Value));

                message = Regex.Replace(
                    message,
                    @"(?<prefix>Bad \\u char )(?<code>\d+)",
                    match => match.Groups["prefix"].Value +
                             (char)int.Parse(match.Groups["code"].Value));

                throw new ArgumentException(message, exception);
            }
        }
    }
}
