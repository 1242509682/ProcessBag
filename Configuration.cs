using Newtonsoft.Json;

//命名空间，想其他项目调用这个项目必须统一命名空间
namespace ProcessBag
{

    public class Config
    {
        public List<Bag> 礼包 = new List<Bag>();

        public static Action<Config> ConfigR;

        public static Config Read(string Path)
        {
            if (!File.Exists(Path))
            {
                return new Config();
            }
            using FileStream stream = new FileStream(Path, FileMode.Open, FileAccess.Read, FileShare.Read);
            return Read(stream);
        }

        public static Config Read(Stream stream)
        {
            using StreamReader streamReader = new StreamReader(stream);
            Config config = JsonConvert.DeserializeObject<Config>(streamReader.ReadToEnd());
            if (ConfigR != null)
            {
                ConfigR(config);
            }
            return config;
        }

        public void Write(string Path)
        {
            using FileStream stream = new FileStream(Path, FileMode.Create, FileAccess.Write, FileShare.Write);
            Write(stream);
        }

        public void Write(Stream stream)
        {
            string value = JsonConvert.SerializeObject(this, (Formatting)1);
            using StreamWriter streamWriter = new StreamWriter(stream);
            streamWriter.Write(value);
        }
    }
}