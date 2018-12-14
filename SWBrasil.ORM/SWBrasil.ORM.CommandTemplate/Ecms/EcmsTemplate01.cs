using SWBrasil.ORM.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SWBrasil.ORM.CommandTemplate
{
    public class EcmsTemplate01 : BaseProject, IProject
    {
        public EcmsTemplate01(): base() { }

        public string CommandID
        {
            get { return "EcmsTemplate01"; }
        }

        public void Build(string outputPath)
        {
            DirectoryInfo di = new DirectoryInfo(outputPath);
            
            //Create Solution Folder
            string _solutionFolder = Path.Combine(outputPath + "\\" + _projectModel.name);
            CheckDirectory(_solutionFolder, "Solution Folder");

            //Create Assemblies Folder
            string _assembliesFolder = Path.Combine(_solutionFolder + "\\" + "Assemblies");
            CheckDirectory(_assembliesFolder, "Assemblies Folder");

            //Create Data Project Folder
            #region Core Project

            string _coreFolder = Path.Combine(_assembliesFolder + "\\Core");
            CheckDirectory(_coreFolder, "Core Folder");

            string _modelsFolder = Path.Combine(_coreFolder + "\\WebStore.Sites.Core");
            CheckDirectory(_modelsFolder, "Models Folder");
            ApplyTemplate(new EcmsNapierModel(), _modelsFolder, "Model");

            #endregion Core Project

            #region Data Project
            string _dataFolder = Path.Combine(_assembliesFolder + "\\Data");
            CheckDirectory(_dataFolder, "Data Folder");

            string _baseModelFolder = Path.Combine(_dataFolder + "\\WebStore.Sites.Data");
            CheckDirectory(_baseModelFolder, "Data Folder");
            ApplyTemplate(new EcmsNapierBaseModel(), _baseModelFolder, "Base Model");

            string _xmlFolder = Path.Combine(_baseModelFolder + "\\FileXML");
            CheckDirectory(_xmlFolder, "XML Folder");
            ApplyTemplate(new NapierXML(), _xmlFolder, "XML");

            #endregion Data Project

            #region Facade Project

            string _facadeFolder = Path.Combine(_assembliesFolder + "\\Facade");
            CheckDirectory(_facadeFolder, "Facade Folder");

            string _facadeProjectFolder = Path.Combine(_facadeFolder + "\\ECMS.Sites.Facade");
            CheckDirectory(_facadeProjectFolder, "Facade Project Folder");
            ApplyTemplate(new EcmsFacade(), _facadeProjectFolder, "Facade");

            #endregion Facade Project

            #region Ecms Project

            var mainControllers = _projectModel.Tables.Where(t => t.MainDTO).ToList();

            string _clientFolder = Path.Combine(_solutionFolder + "\\" + "Client");
            CheckDirectory(_clientFolder, "Client");

            string _ecmsFolder = Path.Combine(_clientFolder + "\\" + "WebStore.Ecms");
            CheckDirectory(_ecmsFolder, "Ecms");

            //Controllers
            string _controllerFolder = Path.Combine(_ecmsFolder + "\\" + "Controllers");
            CheckDirectory(_controllerFolder, "Controller");
            ApplyTemplate(new EcmsMVCController(), _controllerFolder, "Controller");

            //ViewModel 
            _modelsFolder = Path.Combine(_ecmsFolder + "\\" + "Models");
            CheckDirectory(_modelsFolder, "Models");
            ApplyTemplate(new EcmsViewModel(), _modelsFolder, "Model");

            //Views
            string _viewsFolder = Path.Combine(_ecmsFolder + "\\" + "Views");
            CheckDirectory(_viewsFolder, "Views");

            for( var i=0; i < mainControllers.Count; i++)
            {
                string _viewFolder = Path.Combine(_viewsFolder + "\\" + mainControllers[i].Alias.Replace("DTO", ""));
                CheckDirectory(_viewFolder, "View");

                var lstTmp = new List<TableModel>();
                lstTmp.Add(mainControllers[i]);

                ApplyTemplate(new EcmsListPage(), _viewFolder, "List View", lstTmp);
            }

            #endregion Ecms Project
        }
    }
}
