using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonaTest
{
    public class Config
    {
        string folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PersonalTest");
        
        public Config()
        {
            if(!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            if(File.Exists(Path.Combine(folder,"test.txt")))
            {
                Questions._alllist=JsonConvert.DeserializeObject<List<Question>>(File.ReadAllText(Path.Combine(folder, "test.txt")));
              
            }
        }
        public void Save()
        {
            File.WriteAllText(Path.Combine(folder, "test.txt"), JsonConvert.SerializeObject(Questions._alllist));
            Questions._alllist = JsonConvert.DeserializeObject<List<Question>>(File.ReadAllText(Path.Combine(folder, "test.txt")));
        }
    }
}
