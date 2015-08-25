using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace UserLoader
{
    class Program
    {
        static void Main(string[] args)
        {
            //WriteXmlToFile();
            IEnumerable<UserFileViewModel> users = ReadXmlFromFile();
            foreach (var user in users) 
            {
                Console.WriteLine(user.ToString());
            }
        }

        /// <summary>
        /// Reads the XML from file.
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<UserFileViewModel> ReadXmlFromFile()
        {
            // Now we can read the serialized book ...
            XmlSerializer reader = new XmlSerializer(typeof(List<UserFileViewModel>));
            System.IO.StreamReader file = new System.IO.StreamReader(
                @"c:\temp\Book11.xml");
            var users = (List<UserFileViewModel>)reader.Deserialize(file);
            file.Close();

            return users;
        }

        private static void WriteXmlToFile()
        {
            var users = new List<UserFileViewModel>
            {
                new UserFileViewModel
                {
                    UserName = "Pesho",
                    Email = "pesho@test.com",
                    FirstName = "Peso",
                    MiddleName = "Goshev",
                    LastName = "Peshev",
                    Occupation = "Golem gazar",
                    Roles = new List<RoleFileViewModel>
                    {
                        new RoleFileViewModel { Name = "User" },
                        new RoleFileViewModel { Name = "Administrator" }
                    }
                },
                new UserFileViewModel
                {
                    UserName = "Gosho",
                    Email = "gosho@test.com",
                    FirstName = "Gosho",
                    MiddleName = "Gruev",
                    LastName = "Toshev",
                    Occupation = "Nai-Golem gazar",
                    Roles = new List<RoleFileViewModel>
                    {
                        new RoleFileViewModel { Name = "PowerUser" },
                        new RoleFileViewModel { Name = "Boss" }
                    }
                }
            };

            XmlSerializer xsSubmit = new XmlSerializer(typeof(List<UserFileViewModel>));
            var settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = ("\t");
            settings.OmitXmlDeclaration = true;

            using (StreamWriter sww = new StreamWriter(@"c:\temp\test.xml"))
            using (XmlWriter writer = XmlWriter.Create(sww))
            {
                xsSubmit.Serialize(writer, users);
                var xml = sww.ToString(); // Your XML
                Console.WriteLine(xml);
            }
        }
    }
}