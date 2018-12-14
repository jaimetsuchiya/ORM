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
    public class MarketPlaceTemplate01 : BaseProject, IProject
    {
        public MarketPlaceTemplate01(): base() { }

        public string CommandID
        {
            get { return "MarketPlaceTemplate01"; }
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

            string _modelsFolder = Path.Combine(_dataFolder + "\\Models");
            CheckDirectory(_modelsFolder, "Data Model");
            ApplyTemplate(new NapierModel(), _modelsFolder, "Model");

            string _xmlFolder = Path.Combine(_dataFolder + "\\Models");
            CheckDirectory(_xmlFolder, "Data XML");
            ApplyTemplate(new NapierXML(), _xmlFolder, "XML");

            #endregion Data Project

        }
    }
}
