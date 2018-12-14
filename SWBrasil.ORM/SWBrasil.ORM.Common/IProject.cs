using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SWBrasil.ORM.Common
{
    public interface IProject
    {
        void Load(string projectDefinitionFile);
        void Load(ProjectModel model);

        List<ProjectConsoleMessages> Mensagens { get; }
        ProjectModel ProjectDefinition { get; }

        void Save(string projectDefinitionPath);
        void Build(string outputPath);

        string CommandID { get; }
    }

    public class ProjectConsoleMessages
    {
        public DateTime data { get; set; }
        public string mensagem { get; set; }
        public bool erro { get; set; }
    }

    public class ProjectModel
    {
        public ProjectModel()
        {
            this.Tables = new List<TableModel>();
            this.Procedures = new List<ProcModel>();
        }

        public string name { get; set; }
        public string nameSpace { get; set; }

        public string connectionString { get; set; }
        public string connectionStringID { get; set; }

        public string outputFolder { get; set; }
        public string projectTemplate { get; set; }

        public List<TableModel> Tables { get; set; }
        public List<ProcModel> Procedures { get; set; }

        public string tableFilter { get; set; }

        public void Save(string fileName)
        {
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(this);
            File.WriteAllText(fileName, json);
        }

        public void Load(string fileName)
        {
            string json = File.ReadAllText(fileName);
            var tmp = Newtonsoft.Json.JsonConvert.DeserializeObject<ProjectModel>(json);

            this.name = tmp.name;
            this.nameSpace = tmp.nameSpace;
            this.connectionString = tmp.connectionString;
            this.connectionStringID = tmp.connectionStringID;
            this.Tables = tmp.Tables;
            this.Procedures = tmp.Procedures;
            this.outputFolder = tmp.outputFolder;
            this.projectTemplate = tmp.projectTemplate;
            this.tableFilter = tmp.tableFilter;
        }
    }

    public class DataBaseObject
    {
        public string name { get; set; }
        public string type { get; set; }
    }
}
