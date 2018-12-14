using SWBrasil.ORM.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWBrasil.ORM.CommandTemplate
{
    public class BaseProject
    {
        protected ProjectModel _projectModel = null;
        protected List<ProjectConsoleMessages> _mensagens = new List<ProjectConsoleMessages>();

        public BaseProject()
        {
            _projectModel = new ProjectModel();
            _mensagens = new List<ProjectConsoleMessages>();
        }

        public List<ProjectConsoleMessages> Mensagens
        {
            get { return _mensagens; }
        }

        public ProjectModel ProjectDefinition
        {
            get { return _projectModel; }
        }

        protected void ApplyTemplate(ITableTransformation command, string baseFolder, string label, List<TableModel> tables = null)
        {
            command.NameSpace = _projectModel.nameSpace;
            command.ConnectionStringID = _projectModel.connectionStringID;

            if (tables == null)
                tables = _projectModel.Tables;
            foreach (var tabela in tables)
            {
                string dtoContent = command.ApplyTemplate(tabela, _projectModel.Tables);
                if (string.IsNullOrEmpty(dtoContent) == false)
                {
                    string fileName = Path.Combine(baseFolder + "\\" + command.FileName + command.Extension);
                    File.WriteAllText(fileName, dtoContent);
                    _mensagens.Add(new ProjectConsoleMessages() { data = DateTime.Now, mensagem = string.Format("{0} [{1}] Criado!", label, fileName), erro = false });
                }
                else
                    _mensagens.Add(new ProjectConsoleMessages() { data = DateTime.Now, mensagem = string.Format("{0} [{1}] Ignorado!", label, tabela.Name), erro = false });
            }
        }

        protected void CheckDirectory(string fullPath, string label)
        {
            if (Directory.Exists(fullPath) == false)
            {
                Directory.CreateDirectory(fullPath);
                _mensagens.Add(new ProjectConsoleMessages() { data = DateTime.Now, mensagem = string.Format("Diretório [{0}] Criada - {1}!", fullPath, label), erro = false });
            }
        }


        #region Save

        public void Save(string projectDefinitionPath)
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(_projectModel);
            File.WriteAllText(projectDefinitionPath, json);
        }

        #endregion Save

        #region Load

        public void Load(string projectDefinitionFile)
        {
            string json = File.ReadAllText(projectDefinitionFile);
            var model = Newtonsoft.Json.JsonConvert.DeserializeObject<ProjectModel>(json);
            load(model);
        }

        public void Load(ProjectModel model)
        {
            load(model);
        }

        protected void load(ProjectModel model)
        {
            _mensagens = new List<ProjectConsoleMessages>();
            if (model == null)
            {
                _mensagens.Add(new ProjectConsoleMessages() { data = DateTime.Now, mensagem = "Projeto inválido ou não definido!", erro = true });
                throw new ApplicationException("Projeto inválido ou não definido!");
            }

            //Verifica se existem tabelas selecionadas
            if (model.Tables != null)
            {
                foreach (var table in model.Tables)
                {
                    if (table.Type == enumTableType.Basic)
                    {
                        //Verifica se todas as tabelas base possuem DTOS associados
                        if (table.IgnoreDTO && string.IsNullOrEmpty(table.Alias))
                        {
                            _mensagens.Add(new ProjectConsoleMessages() { data = DateTime.Now, mensagem = "Tabela [" + table.Name + "] não possui o respectivo DTO parametrizado!", erro = true });
                            throw new ApplicationException("Tabela [" + table.Name + "] não possui o respectivo DTO parametrizado!");
                        }

                    }
                }
            }

            _projectModel = model;
        }

        #endregion Load
    }
}
