using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace BluePrismTechnicalTestAaronLewis
{
    public class WordDictionary
    {
        private string _startWord;
        private string _endWord;
        private List<string> _dictionaryFile;
        //Folder path for the word dictionaries
        private string _dictionaryFolderPath = "WordDictionaries";
        //Folder path for output file
        private string _outputFolderPath = "ResultFile";
        //Change to make the word challenge work off a different amount of letters.
        private const int LengthOfWord = 4;


        public void Test()
        {
            TextEditor textEditor = new TextEditor();

            //Introduce the console app
            Introduction();

            //First we need to get the list of dictionaries that we should be using from the default directory
            var textFileLocation = textEditor.GetWordDictionaries(_dictionaryFolderPath);

            //Pass them into here to decide which dictionary file we should use.
            string wordDictionaryLocation = DictionaryFileSelector(textFileLocation);

            // Turn the dictionary file into a list of strings
            _dictionaryFile = textEditor.ReadTextFile(wordDictionaryLocation);

            //Tidy the dictionary up removing all words that don't match the length and converting all words in the dictionary to lower case
            SanitiseDictionary();

            //Set the start word
            SetStartWord();

            //Set the end word
            SetEndWord();

            //Find the quickest path between the words
            WordChallenge wordChallenge = new WordChallenge();
            var words = wordChallenge.FindQuickestPath(_startWord, _endWord, _dictionaryFile);

            //Formats the list object to the text file
            FormatListToTextFile(textEditor, words);
        }

        private void Introduction()
        {
            WriteLine("Blue Prism Technical Challenge by Aaron Lewis");
            WriteLine("------------------------------------------------");
            WriteLine();
            WriteLine();
        }

        private string DictionaryFileSelector(List<string> dictionaryFiles)
        {
            //If no files are found then exit the program as it can't work without a word dictionary.
            if (dictionaryFiles.Count == 0)
            {
                WriteLine($"No valid text files could be found. Please paste them into the {_dictionaryFolderPath} folder.");
                WriteLine("Program will now exit");
                ReadLine();
                Environment.Exit(0);
            }
            //If theres only one word dictionary then auto select it.
            else if (dictionaryFiles.Count == 1)
            {
                WriteLine($"Only one text file was found in the {_dictionaryFolderPath} folder.");
                WriteLine("Using " + dictionaryFiles.FirstOrDefault());
                return dictionaryFiles.First();
            }
            //If theres multiple word dictionarys then allow the user to select one
            else
            {
                WriteLine($"Multiple text files were found in the {_dictionaryFolderPath} folder.");
                WriteLine("Please select one of the following:");
                Dictionary<int, string> wordDictionary = new Dictionary<int, string>();
                for (int i = 1; i <= dictionaryFiles.Count; i++)
                {
                    wordDictionary.Add(i, dictionaryFiles[i - 1]);
                    WriteLine(i + ".\t" + dictionaryFiles[i - 1]);
                }

                int dataDictionaryId;
                while (true)
                {
                    int selectedId;
                    bool isInt = Int32.TryParse(ReadLine(), out selectedId);
                    if (isInt && wordDictionary.ContainsKey(selectedId))
                    {
                        dataDictionaryId = Convert.ToInt32(selectedId);
                        break;
                    }
                    WriteLine("Not a valid number, please select a valid number from the list above:");
                }
                return wordDictionary[dataDictionaryId];
            }
            return String.Empty;
        }

        private void SetStartWord()
        {
            WriteLine();
            //Ask the user for a valid word, when one is selected then escape
            while (true)
            {
                WriteLine("Type the start word and then press Enter");
                _startWord = ValidateWord(ReadLine());
                if (_startWord.Length == LengthOfWord)
                    break;
            }
        }

        private void SetEndWord()
        {
            WriteLine();
            //Ask the user for a valid word, when one is selected then escape
            while (true)
            {
                WriteLine("Type the end word and then press Enter");
                _endWord = ValidateWord(ReadLine());
                if (_startWord == _endWord)
                {
                    WriteLine("The end word cannot match with the start word");
                    continue;
                }

                if (_endWord.Length == LengthOfWord)
                    break;
            }
        }

        private string SetFileName(TextEditor textEditor)
        {
            //Asks the user for a unique filename
            while (true)
            {
                WriteLine("Choose a name for your text file, if left blank then a name will be generated for your file:");
                var fileName = ReadLine();
                if (String.IsNullOrWhiteSpace(fileName))
                {
                    var dateTime = DateTime.Now;
                    fileName = $"{_startWord}-{_endWord}-{dateTime.Day}{dateTime.Month}{dateTime.Year}{dateTime.Hour}{dateTime.Minute}{dateTime.Second}.txt";
                }
                else if (!fileName.Contains(".txt"))
                    fileName = $"{fileName}.txt";

                if (!textEditor.CheckIfFileExists(_outputFolderPath, fileName))
                {
                    WriteLine("That file already exists. Please choose another file name.");
                    continue;
                }

                return fileName;
            }
        }

        private void SanitiseDictionary()
        {
            //Converts to lower case and removes words that don't match the length as they can't be selected
            List<string> newArray = new List<string> { };
            foreach (var word in _dictionaryFile)
                if (word.Length == LengthOfWord)
                    newArray.Add(word.ToLower());

            _dictionaryFile = newArray;
        }

        private void FormatListToTextFile(TextEditor textEditor, List<string> words)
        {
            WriteLine();
            WriteLine();
            //Convert the list of words into the text file if any were found.
            if (words.Any())
            {
                var fileName = SetFileName(textEditor);
                textEditor.WriteToTextFile(words, _outputFolderPath, fileName);
                WriteLine($"File has been created: {fileName}");
            }
            else
                WriteLine("Sorry, a path couldn't be found between the two words.");

            WriteLine("Press Enter to exit.");
            ReadLine();
        }

        private string ValidateWord(string word)
        {
            //Convert word to lower and check if its the right length and if it appears in the word dictionary.
            word = word.Trim().ToLower();
            if (word.Length == LengthOfWord && _dictionaryFile.Contains(word))
                return word;
            if (word.Length != LengthOfWord)
                WriteLine($"Word needs to be {LengthOfWord} letters long");
            else
                WriteLine("Word not present in data dictionary");


            return String.Empty;
        }
    }
}
