using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Tests
{
    public class Validation
    {
        // metoden kollar om innehållet i en textarea är tomt
        static public bool noText(TextBox tb)
        {
            if (tb.Text.Length == 0)
            {
                MessageBox.Show("Samtliga fält måste vara ifyllda");
                return true;
            }
            else
            {
                return false;
            }
        }

        static public bool ContainsLetter(TextBox tb)
        {
            string myString = tb.Text;
            if (!Regex.IsMatch(myString, @"[a-zA-ZåäöÅÄÖ]"))
            {
                MessageBox.Show("Måste inkludera bokstäver", "meddelande");
                return true;
            }
            else
            {
                return false;
            }
          

        }
        static public bool restrictMinLength(TextBox tb)
        {
            if (tb.Text.Length < 2)
            {
                MessageBox.Show("Måste vara minst 2 tecken inmatade i ett fält (ska vara bokstäver eller nummer) ");
    
                return true;
            }
            else
            {
                return false;
            }
        }

        static public bool minLengthLong(TextBox tb)
        {
            if (tb.Text.Length < 10)
            {
                MessageBox.Show("Måste vara mer än 10 tecken inmatade i ett fält ");
                return true;
            }
            else
            {
                return false;
            }

        }
        // Vilket denna metod kollar upp
        static public bool restrictLength(TextBox tb)
           {
            if (tb.Text.Length > 40)
            {
                MessageBox.Show("Får ej vara mer än 40 tecken i namnfältet ", "Meddelande");
                return true;
            }
            else
            {
                return false;
            }
        }

        static public bool restrictLongerLength(TextBox tb)
        {
            if (tb.Text.Length > 150)
            {
                MessageBox.Show("Får ej vara mer än 150 tecken i URL-fältet", "Meddelande");
                return true;
            }
            else
            {
                return false;
            }

        }


        public static bool CorrectStartString (TextBox tb)
        {
            if (!(tb.Text.StartsWith("http://") || tb.Text.StartsWith("www.") || tb.Text.StartsWith("https://")
                || tb.Text.StartsWith("feeds.")))
            {
                MessageBox.Show("Måste ange en korrekt URL", "Meddelande");
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}








