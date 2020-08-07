using OnSideCountdown.Enums;
using OnSideCountdown.Interfaces;
using OnSideCountdown.Services;
using System;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;

namespace OnSideCountdown
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        IWordSearch wordSearchService;
        int round = 1;
        int letterPos = 1;
        int vowelCount = 0;
        int score1 = 0;
        int score2 = 0;
        int constCount = 0;
        string letters = string.Empty;
        public MainPage()
        {
            InitializeComponent();
            this.entryWord.IsEnabled = false;
            wordSearchService = new WordSearchService();
        }
        private void btnSubmit_Clicked(object sender, EventArgs e)
        {
            switch (this.btnSubmit.Text.Trim())
            {
                case "Press Here to Start Game":
                case "Next Round":
                    this.ResetRound();
                    break;

                case "Submit your word":
                    if (this.entryWord.Text != null)
                        this.CheckWord(this.entryWord.Text.Trim().ToUpper());
                    break;
            }
        }
        private void ResetRound()
        {
            this.letters = string.Empty;
            this.letterPos = 1;
            this.vowelCount = 0;
            this.constCount = 0;
            this.lblLongestWord.Text = "";
            this.ResetLetters();
            this.lblErrorMessage.Text = string.Empty;
            this.btnSubmit.Text = "Select your letters";
            this.btnSubmit.IsEnabled = false;
            this.btnConsonant.IsEnabled = true;
            this.btnVowel.IsEnabled = true;
            this.entryWord.IsEnabled = false;
            this.entryWord.Text = "Select your letters";
        }
        private void CheckWord(string word)
        {
            try
            {
                this.lblErrorMessage.Text = "Checking word....";

                if (word == string.Empty)
                {
                    this.lblErrorMessage.Text = "You need to enter a word!";
                    return;
                }

                if (word.Any(char.IsDigit))
                {
                    this.lblErrorMessage.Text = "You word cannot contain numbers !";
                    return;
                }

                var checkLetters = letters;
                // Check word only has possible letters
                for (int x = 0; x < word.Length; x++)
                {
                    if (!checkLetters.Contains(word[x]))
                    {
                        this.lblErrorMessage.Text = "Your word is contains invalid letters !";
                        return;
                    }
                    else
                    {
                        int index = checkLetters.IndexOf(word[x]);
                        String startOfString = checkLetters.Substring(0, index);
                        String endOfString = checkLetters.Substring(index + 1);
                        checkLetters = startOfString + endOfString;
                    }
                }

                var wordList = wordSearchService.CheckWord(word);
                if (wordList.Count == 0)
                {
                    this.lblErrorMessage.Text = "Your word is invalid!";
                    return;
                }

                var wordFound = false;
                foreach (var wordfound in wordList)
                {
                    if (wordfound.word.ToLower() == word.ToLower())
                    {
                        wordFound = true;
                        break;
                    }
                }

                var maxLen = 0;
                wordList = wordSearchService.CheckWord(letters);
                foreach (var wordfound in wordList)
                {
                    if (wordfound.length > maxLen)
                    {
                        lblLongestWord.Text = "Mossie found this word : " + wordfound.word;
                        maxLen = wordfound.length;
                    }
                }

                score2 = score2 + maxLen;
                this.lblErrorMessage.Text = wordFound ? "Your word is Valid!" : "Your word is invalid!";
                if (wordFound) score1 = score1 + word.Length;

                var sb = new System.Text.StringBuilder();
                sb.Append("After round <round> of 4 your score is <score1>.  Mossies score is <score2>");
                sb.Replace("<round>", round.ToString());
                sb.Replace("<score1>", score1.ToString());
                sb.Replace("<score2>", score2.ToString());

                this.lblScore.Text = sb.ToString();

                round++;
                if (round <= 4)
                {
                    this.btnSubmit.Text = "Next Round";
                }
                else
                {
                    this.lblErrorMessage.Text = "Game Over !";
                    this.score1 = 0;
                    this.score2 = 0;
                    this.btnSubmit.Text = "Press Here to Start Game";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void btnConsonant_Clicked(object sender, EventArgs e)
        {
            try
            {
                var letter = wordSearchService.SuggestLetter(LetterType.Consonant);
                this.SetLetter(letter);
                this.constCount++;
                this.btnConsonant.IsEnabled = (constCount < 6);
                this.CheckAllLettersSelected(letter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void btnVowel_Clicked(object sender, EventArgs e)
        {
            try
            {
                var letter = wordSearchService.SuggestLetter(LetterType.Vowel);
                this.SetLetter(letter);
                this.vowelCount++;
                this.btnVowel.IsEnabled = (vowelCount < 3);
                this.CheckAllLettersSelected(letter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void CheckAllLettersSelected(string letter)
        {
            letterPos++;

            letters = letters + letter;

            if (this.letterPos > 9)
            {
                this.btnSubmit.Text = "Submit your word";
                this.btnSubmit.IsEnabled = true;
                this.entryWord.IsEnabled = true;
                this.entryWord.Text = string.Empty;
            }
        }
        private void SetLetter(string letter)
        {
            try
            {
                switch (letterPos)
                {
                    case 1: this.lblLeter1.Text = letter; break;
                    case 2: this.lblLeter2.Text = letter; break;
                    case 3: this.lblLeter3.Text = letter; break;
                    case 4: this.lblLeter4.Text = letter; break;
                    case 5: this.lblLeter5.Text = letter; break;
                    case 6: this.lblLeter6.Text = letter; break;
                    case 7: this.lblLeter7.Text = letter; break;
                    case 8: this.lblLeter8.Text = letter; break;
                    case 9: this.lblLeter9.Text = letter; break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void ResetLetters()
        {
            try
            {
                this.lblLeter1.Text = "?";
                this.lblLeter2.Text = "?";
                this.lblLeter3.Text = "?";
                this.lblLeter4.Text = "?";
                this.lblLeter5.Text = "?";
                this.lblLeter6.Text = "?";
                this.lblLeter7.Text = "?";
                this.lblLeter8.Text = "?";
                this.lblLeter9.Text = "?";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
