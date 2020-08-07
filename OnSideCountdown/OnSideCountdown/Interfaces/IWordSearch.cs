using OnSideCountdown.Enums;
using OnSideCountdown.Models;
using System.Collections.Generic;

namespace OnSideCountdown.Interfaces
{
    public interface IWordSearch
    {
        List<WordResponse> CheckWord(string word);
        string SuggestLetter(LetterType type);
    }
}
