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
    public class AngularNapier01 : BaseProject, IProject
    {
        public AngularNapier01() : base() { }

        public string CommandID
        {
            get { return "AngularNapier01"; }
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
            #region Data Project

            string _dataFolder = Path.Combine(_assembliesFolder + "\\" + _projectModel.nameSpace + ".Data");
            CheckDirectory(_dataFolder, "Data Project");

            //Apply LightAbstractModel Template
            string _modelsFolder = Path.Combine(_dataFolder + "\\Models");
            string _baseFolder = Path.Combine(_modelsFolder + "\\Abstract");
            CheckDirectory(_baseFolder, "Data Base Model");
            ApplyTemplate(new NapierModel(), _baseFolder, "Base Model");

            //Apply LightModel Template
            CheckDirectory(_modelsFolder, "Data Model");
            ApplyTemplate(new LightModel(), _baseFolder, "Model");
            LightModel lightORM = new LightModel();

            #endregion Data Project

            //Create Common Project Folder
            #region Common Project

            string _commonFolder = Path.Combine(_assembliesFolder + "\\" + _projectModel.nameSpace + ".Common");
            CheckDirectory(_commonFolder, "Common Folder");

            //Apply Template DTOs
            string _dtosFolder = Path.Combine(_commonFolder + "\\" + "DTOs");
            CheckDirectory(_dtosFolder, "DTOs Folder");
            ApplyTemplate(new DTOLightModel(), _dtosFolder, "DTO");


            string _businessFolder = Path.Combine(_commonFolder + "\\" + "Business");
            CheckDirectory(_businessFolder, "Interfaces Folder");
            ApplyTemplate(new BusinessInterfaces(), _businessFolder, "DTO");

            #endregion Common Project

            //Create Core Project Folder
            #region Core

            string _coreFolder = Path.Combine(_assembliesFolder + "\\" + _projectModel.nameSpace + ".Core");
            CheckDirectory(_coreFolder, "Core Folder");

            _businessFolder = Path.Combine(_coreFolder + "\\Business");
            CheckDirectory(_businessFolder, "Implementation Folder");
            ApplyTemplate(new ImplementationLightClass(), _businessFolder, "Implementation");

            #endregion Core

            //Create Client Folder
            #region Client

            string _clientFolder = Path.Combine(_solutionFolder + "\\" + "Client");
            CheckDirectory(_clientFolder, "Client Folder");
            
            //Create Web Project Folder
            string _webFolder = Path.Combine(_clientFolder + "\\" + _projectModel.nameSpace + ".Web");
            CheckDirectory(_webFolder, "Web Project Folder");

            string _viewsFolder = Path.Combine(_webFolder + "\\Views");
            CheckDirectory(_viewsFolder, "Views Folder");

            string _controllersFolder = Path.Combine(_webFolder + "\\Controllers");
            CheckDirectory(_controllersFolder, "Controllers Folder");
            ApplyTemplate(new AngularSearchWidgetController(), _controllersFolder, "Controller");

            string _widgetsFolder = Path.Combine(_viewsFolder + "\\Widgets");
            CheckDirectory(_widgetsFolder, "Widgets Folder");
            ApplyTemplate(new AngularSearchWidget(), _widgetsFolder, "Widget");

            #endregion Client
        }
    }
}
