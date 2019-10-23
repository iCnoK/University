using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paint.Utility
{
    public class ProgramSettings
    {
        private string DefaultPathToDatabase { get; } = $"{Environment.CurrentDirectory}\\PaintCourceWork";
        private string PathToConfigFile
        {
            get
            {
                return $"{CustomPathToDatabase}\\Config.bin";
            }
        }
        public string CustomPathToDatabase { get; } = "";

        

        public ProgramSettings() 
        {
            //if (File.Exists())
            CustomPathToDatabase = DefaultPathToDatabase;
            CheckOrCreateDirectory();
            CheckOrCreateFile(PathToConfigFile);

            //if (!CheckOrCreateDirectory() && !CheckOrCreateFile(PathToConfigFile))
            //{

            //}
        }
        public ProgramSettings(string newPathToDatabase)
        {
            CustomPathToDatabase = newPathToDatabase;
        }

        /// <summary>
        /// true - exists, false - created
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private bool CheckOrCreateFile(string path)
        {
            if (!File.Exists(PathToConfigFile))
            {
                File.Create(PathToConfigFile);
                return false;
            }
            return true;
        }

        /// <summary>
        /// true - exists, false - created
        /// </summary>
        /// <returns></returns>
        private bool CheckOrCreateDirectory()
        {
            if (!Directory.Exists(CustomPathToDatabase))
            {
                Directory.CreateDirectory(CustomPathToDatabase);
                return false;
            }
            return true;
        }
        

        public static ProgramSettings GetProgramSettings(string fileName)
        {
            return null;
        }
    }
}
