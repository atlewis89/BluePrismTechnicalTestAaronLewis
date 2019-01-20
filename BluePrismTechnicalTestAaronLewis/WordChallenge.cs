using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BluePrismTechnicalTestAaronLewis
{
    public class WordChallenge
    {
        public List<string> FindQuickestPath(String start, String end, List<string> dict)
        {
            var preVisitedStr = new List<WordList> { };
            preVisitedStr.Add(new WordList() { PreviousWords = new List<string>(), CurrentWord = start });
            while (preVisitedStr.Count != 0)
            {
                var nextVisitedWords = new List<WordList>();

                foreach (var visited in preVisitedStr)
                {
                    if (IsOnlyOneCharacterDifferent(end, visited.CurrentWord))
                    {
                        visited.PreviousWords.Add(visited.CurrentWord);
                        visited.PreviousWords.Add(end);
                        return visited.PreviousWords;
                    }
                    var listOfParents = new List<string>();
                    listOfParents.AddRange(visited.PreviousWords);
                    listOfParents.Add(visited.CurrentWord);
                    for (var i = dict.Count() - 1; i >= 0; i--)
                    {
                        if (!IsOnlyOneCharacterDifferent(visited.CurrentWord, dict[i]))
                        {
                            continue;
                        }

                        nextVisitedWords.Add(new WordList() { PreviousWords = listOfParents, CurrentWord = dict[i] });
                        dict.RemoveAt(i);
                    }
                }

                preVisitedStr = nextVisitedWords;
            }

            return new List<string>();
        }

        private bool IsOnlyOneCharacterDifferent(string word1, string word2)
        {
            //Goes through each character and checks if they all match except for one instance.
            return word1.Where((t, i) => !t.Equals(word2[i])).Count() == 1;
        }
    }
}
