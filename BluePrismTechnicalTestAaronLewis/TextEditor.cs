using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BluePrismTechnicalTestAaronLewis
{
    public class TextEditor
    {
        public List<string> GetWordDictionaries(string folderPath)
        {
            //Get all the text files in the folder
            return Directory.GetFiles($@"{folderPath}", "*.txt").ToList();
        }

        public List<string> ReadTextFile(string file)
        {
            //Converts text file to list of strings.
            return File.ReadLines(file).ToList();
        }

        public bool CheckIfFileExists(string folderPath, string fileName)
        {
            //Simple check to see if the file already exists
            if (File.Exists($@"{folderPath}\{fileName}"))
                return false;

            return true;
        }

        public void WriteToTextFile(List<string> words, string folderPath, string fileName)
        {
            //Output the list of strings to the text file
            using (TextWriter textWriter = new StreamWriter($@"{folderPath}\{fileName}"))
                foreach (String word in words)
                    textWriter.WriteLine(word);
        }
    }
}
