using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace YamlTools
{

    public sealed class YamlConvert
    {

        public static T DeserializeObject<T>(string value)
        {
            var deserializer = new DeserializerBuilder().WithNamingConvention(new UnderscoredNamingConvention()).Build();
            var reader = new StringReader(value);
            var data = deserializer.Deserialize<T>(reader);
            reader.Close();

            return (T)data;
        }

    }

}
