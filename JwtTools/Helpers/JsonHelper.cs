using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Text;

// the following is from the DevToys project JsonHelpers
// https://github.com/veler/DevToys/

namespace JwtTools.Helpers
{
    internal static class JsonHelper
    {
        internal static bool IsValid(string? input)
        {
            input = input?.Trim();

            if (input == null)
            {
                return true;
            }

            if (long.TryParse(input, out _))
            {
                return false;
            }

            try
            {
                var jtoken = JToken.Parse(input);
                return jtoken is not null;
            }
            catch (JsonReaderException)
            {
                // Exception in parsing json. It likely mean the text isn't a JSON.
                return false;
            }
            catch (Exception) //some other exception
            {
                return false;
            }
        }
        internal static string Format(string? input, Indentation indentationMode)
        {
            if (input == null || !IsValid(input))
            {
                return string.Empty;
            }

            try
            {
                var jtoken = JToken.Parse(input);
                if (jtoken is not null)
                {
                    var stringBuilder = new StringBuilder();
                    using (var stringWriter = new StringWriter(stringBuilder))
                    using (var jsonTextWriter = new JsonTextWriter(stringWriter))
                    {
                        switch (indentationMode)
                        {
                            case Indentation.TwoSpaces:
                                jsonTextWriter.Formatting = Formatting.Indented;
                                jsonTextWriter.IndentChar = ' ';
                                jsonTextWriter.Indentation = 2;
                                break;
                            case Indentation.FourSpaces:
                                jsonTextWriter.Formatting = Formatting.Indented;
                                jsonTextWriter.IndentChar = ' ';
                                jsonTextWriter.Indentation = 4;
                                break;
                            case Indentation.OneTab:
                                jsonTextWriter.Formatting = Formatting.Indented;
                                jsonTextWriter.IndentChar = '\t';
                                jsonTextWriter.Indentation = 1;
                                break;
                            case Indentation.Minified:
                                jsonTextWriter.Formatting = Formatting.None;
                                break;
                            default:
                                throw new NotSupportedException();
                        }
                        jtoken.WriteTo(jsonTextWriter);
                    }

                    return stringBuilder.ToString();
                }

                return string.Empty;
            }
            catch (JsonReaderException ex)
            {
                return ex.Message;
            }
            catch (Exception ex) //some other exception
            {
                return ex.Message;
            }
        }
    }
}
