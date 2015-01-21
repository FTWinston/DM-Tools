using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace DMTools.Services
{
    public class DiceService
    {
        private static Random randomGenerator = new Random();

        /// <summary>
        /// Pass a die roll string in any standard d20-type format, including 
        /// parenthetical rolls, and it will output a string breaking down each 
        /// roll as well as summing the total.
        /// </summary>
        /// <param name="diceString">This is a standard d20 die roll string, parenthesis 
        /// are allowed. Example Input: (2d8+9)+(3d6+1)-10
        /// <returns>A string breaking down and summing the roll. Example Output: 
        /// (2d8+9)+(3d6+1)-10 = 7+4+9+5+1+2+1-10 = 20+9-10 = 19</returns>
        public static string Roll(string diceString)
        {
            string tempString = "";
            int intermediateTotal = 0, total = 0;
            bool collate = false, positive = true;
            List<int> sums = new List<int>(), items = new List<int>(), dice = new List<int>();
            const string validChars = "1234567890d";

            foreach (char c in diceString)
                switch (c)
                {
                    case '+':
                        {
                            if (tempString.Length < 1)
                            {
                                positive = true;
                                break;
                            }
                            dice = ResolveItem(tempString);
                            for (int j = 0; j < dice.Count; j++)
                            {
                                int val = (positive ? 1 : -1) * dice[j];

                                items.Add(val);
                                intermediateTotal += val;
                            }
                            if (!collate)
                            {
                                sums.Add(intermediateTotal);
                                intermediateTotal = 0;
                            }
                            positive = true;
                            tempString = "";
                            break;
                        }
                    case '-':
                        {
                            if (tempString.Length < 1)
                            {
                                positive = false;
                                break;
                            }
                            dice = ResolveItem(tempString);
                            for (int j = 0; j < dice.Count; j++)
                            {
                                int val = (positive ? 1 : -1) * dice[j];

                                items.Add(val);
                                intermediateTotal += val;
                            }
                            if (!collate)
                            {
                                sums.Add(intermediateTotal);
                                intermediateTotal = 0;
                            }
                            positive = false;
                            tempString = "";
                            break;
                        }
                    case '(': collate = true; break;
                    case ')': collate = false; break;
                    default:
                        {
                            if (validChars.Contains(c.ToString()))
                                tempString += c;
                            break;
                        }
                }

            // And once more for the remaining text
            if (tempString.Length > 0)
            {
                dice = ResolveItem(tempString);
                for (int j = 0; j < dice.Count; j++)
                {
                    int val = (positive ? 1 : -1) * dice[j];

                    items.Add(val);
                    intermediateTotal += val;
                }
                sums.Add(intermediateTotal);
                intermediateTotal = 0;
            }

            // Print it all.
            StringBuilder output = new StringBuilder(diceString + " = ");
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i] > 0 && i > 0)
                    output.Append("+" + items[i].ToString());
                else
                    output.Append(items[i].ToString());
            }
            if (sums.Count > 1 && items.Count > sums.Count)
            { // Don't print just one, or items again.
                output.Append(" = ");
                for (int i = 0; i < sums.Count; i++)
                {
                    if (sums[i] > 0 && i > 0)
                        output.Append("+" + sums[i].ToString());
                    else
                        output.Append(sums[i].ToString());
                }
            }
            for (int i = 0; i < sums.Count; i++)
                total += sums[i];
            output.Append(" = ");
            output.Append(total);

            return output.ToString();
        }

        /// <summary>
        /// Resolves a *basic* die roll string
        /// </summary>
        /// <param name="s">A simple die roll string, such as 3d6.</param>
        /// <returns>Returns a List containing the various die rolls.</returns>
        private static List<int> ResolveItem(string s)
        {
            List<int> dice = new List<int>();
            int split = s.IndexOf('d');
            if (split != -1)
            {
                int num = Convert.ToInt32(s.Substring(0, split));
                int sides = Convert.ToInt32(s.Substring(split + 1));

                for (int i = 0; i < num; i++)
                    dice.Add(randomGenerator.Next(sides) + 1);
            }
            else
                dice.Add(Convert.ToInt32(s));

            return dice;
        }
    }
}