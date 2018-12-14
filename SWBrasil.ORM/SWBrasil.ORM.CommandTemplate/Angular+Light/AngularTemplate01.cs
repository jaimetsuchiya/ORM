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
    public class AngularTemplate01 : BaseProject, IProject
    {
        public AngularTemplate01(): base() { }

        public string CommandID
        {
            get { return "AngularTemplate01"; }
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
            //string _baseFolder = Path.Combine(_modelsFolder + "\\Abstract");
            //CheckDirectory(_baseFolder, "Data Base Model");
            //ApplyTemplate(new LightAbstractModel(), _baseFolder, "Base Model");

            //Apply LightModel Template
            CheckDirectory(_modelsFolder, "Data Model");
            ApplyTemplate(new LightModel(), _modelsFolder, "Model");

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


            string _testsFolder = Path.Combine(_assembliesFolder + "\\" + _projectModel.nameSpace + ".Tests");
            CheckDirectory(_testsFolder, "Tests Folder");

            _businessFolder = Path.Combine(_testsFolder + "\\Core");
            CheckDirectory(_businessFolder, "Core Tests Folder");
            ApplyTemplate(new AngularTestCases(), _businessFolder, "Tests");

            #endregion Core

            //Create Client Folder
            #region Client

            string _clientFolder = Path.Combine(_solutionFolder + "\\" + "Client");
            CheckDirectory(_clientFolder, "Client Folder");
            
            //Create Web Project Folder
            string _webFolder = Path.Combine(_clientFolder + "\\" + _projectModel.nameSpace + ".Web");
            CheckDirectory(_webFolder, "Web Project Folder");
            ApplyTemplate(new SimpleInjector(), _webFolder, "SimpleInjector Register Method");

            string _scriptFolder = Path.Combine(_webFolder + "\\Scripts");
            CheckDirectory(_scriptFolder, "Scripts Folder");
            string _controllersFolder = Path.Combine(_scriptFolder+ "\\Controllers");
            CheckDirectory(_controllersFolder, "Controllers Folder");
            ApplyTemplate(new AngularJSController(), _controllersFolder, "Angular Controller");

            string _viewsFolder = Path.Combine(_webFolder + "\\Views");
            CheckDirectory(_viewsFolder, "Views Folder");

            _controllersFolder = Path.Combine(_webFolder + "\\Controllers");
            CheckDirectory(_controllersFolder, "Controllers Folder");
            ApplyTemplate(new AngularSearchWidgetController(), _controllersFolder, "Widget Controller");

            var controllers = _projectModel.Tables.Where(t => t.MainDTO).ToList();
            ApplyTemplate(new AngularMVCController(), _controllersFolder, "Controller", controllers);

            string _widgetsFolder = Path.Combine(_viewsFolder + "\\Widgets");
            CheckDirectory(_widgetsFolder, "Widgets Folder");
            ApplyTemplate(new AngularSearchWidget(), _widgetsFolder, "Widget");

            for( var i=0; i < controllers.Count; i++)
            {
                string _controllerViewFolder = Path.Combine(_viewsFolder + "\\" + controllers[i].Alias.Replace("DTO", ""));
                CheckDirectory(_controllerViewFolder, "Controller View Folder");

                var lstTmp = new List<TableModel>();
                lstTmp.Add(controllers[i]);

                ApplyTemplate(new AngularListPage(), _controllerViewFolder, "List View", lstTmp);
                ApplyTemplate(new AngularDetailPage(), _controllerViewFolder, "Detail View", lstTmp);
            }

            #endregion Client
        }
    }
}
