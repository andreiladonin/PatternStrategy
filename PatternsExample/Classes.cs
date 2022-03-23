using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PatternsExample
{
    // Strategy
    public class User
    {
        public string Name { get; set; }
        public string Login { get; set; }
        public string Password { get; set;}
        public string Salt { get; set; }
    }


    public class UserSerlization
    {
        private IMySerialization _mySerialization;

        private string _pathFile;
        private string _pathSaveFile;
        public UserSerlization(string pathFile, string pathSaveFile, IMySerialization mySerialization)
        {
            _pathFile = pathFile;
            _pathSaveFile = pathSaveFile;
            _mySerialization = mySerialization;
        }

        public async Task Serializable ()
        {
            var users = await GetUsers(_pathFile);
            _mySerialization.Serializable(_pathSaveFile, users);
        }

        async Task<List<User>> GetUsers(string pathFile)
        {

            var users = new List<User>();

            using (StreamReader reader = new StreamReader(pathFile))
            {
                string? line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        var newLines = line.Split(',');

                        string name = newLines[0].Split('-')[1].Trim();
                        string login = newLines[1].Split('-')[1].Trim();
                        string psw = newLines[2].Split('-')[1].Trim();
                        string salt = newLines[3].Split('-')[1].Trim();

                        var user = new User()
                        {
                            Name = name,
                            Login = login,
                            Password = psw,
                            Salt = salt
                        };

                        users.Add(user);
                    }

                }
            }

            return users;
        }
    }


    public interface IMySerialization
    {
        void Serializable(string pathSave, List<User> users);  
    }

    public class MyJsonSerialization : IMySerialization
    {
        public void Serializable(string pathSave, List<User> users)
        {
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                WriteIndented = true
            };
            var json = JsonSerializer.Serialize(users, options);

            using (var sw = new StreamWriter(pathSave, false, Encoding.UTF8))
            {
                sw.Write(json);
            }
        }
    }

    public class MyXmlSerialization : IMySerialization
    {
        public void Serializable(string pathSave, List<User> users)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<User>));
            using (var fs = new FileStream(pathSave, FileMode.OpenOrCreate))
            {
                xmlSerializer.Serialize(fs, users);
            }
        }
    }

    public class MyCSVSerialization : IMySerialization
    {
        public void Serializable(string pathSave, List<User> users)
        {
            throw new NotImplementedException();
        }
    }
}
