using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using LCSDiff.Properties;

namespace LCSDiff
{
    public class Lcs
    {
        public static void LcsAlgorithm(string string1, string string2, RichTextBox rtb)
        {
            var plus = new List<int>();
            var minus = new List<int>();
            int i = 0;
            int j;
            int temp;
            int plusbound = 0;
            int minusbound = 0;

            int position = 0;

            int[,] arr = LongestCommonSubsequence(string1, string2);


            //for (int m = 0; m <= arr.GetUpperBound(0); m++)
            //{
            //    for (int n = 0; n <= arr.GetUpperBound(1); n++)
            //    {
            //        Console.Write(arr[m, n] + " ");
            //    }
            //    Console.WriteLine();
            //}

            string a = GetDiff(arr, string1, string2, string1.Length, string2.Length);

            while (position >= 0)
            {
                minus.Add(a.IndexOf("€", position, StringComparison.Ordinal));
                if (minus[i] >= 0)
                {
                    position = minus[i] + 1;
                }
                else
                {
                    break;
                }
                minusbound = i + 1;
                i++;
            }
            position = 0;
            i = 0;
            while (position >= 0)
            {
                plus.Add(a.IndexOf("§", position, StringComparison.Ordinal));
                if (plus[i] >= 0)
                {
                    position = plus[i] + 1;
                }
                else
                {
                    break;
                }
                plusbound = i + 1;
                i++;
            }

            var merge = new int[minusbound + plusbound];
            if (minusbound + plusbound == 0)
            {
                MessageBox.Show(Resources.NOUPDATE);
                return;
            }

            for (i = 0; i < minusbound; i++)
            {
                merge[i] = minus[i];
            }
            for (i = 0; i < plusbound; i++)
            {
                merge[i + minusbound] = plus[i];
            }

            for (i = 0; i < merge.Length - 1; i++)
            {
                for (j = i + 1; j < merge.Length; j++)
                {
                    if (merge[i] > merge[j])
                    {
                        temp = merge[j];
                        merge[j] = merge[i];
                        merge[i] = temp;
                    }
                }
            }

            position = 0;
            while (position >= 0)
            {
                position = a.IndexOf("€", position, StringComparison.Ordinal);
                if (position >= 0)
                {
                    a = a.Substring(0, position) + a.Substring(position + 1);
                    position = position + 1;
                }
                else
                {
                    break;
                }
            }

            position = 0;
            while (position >= 0)
            {
                position = a.IndexOf("§", position, StringComparison.Ordinal);
                if (position >= 0)
                {
                    a = a.Substring(0, position) + a.Substring(position + 1);
                    position = position + 1;
                }
                else
                {
                    break;
                }
            }
            rtb.Clear();
            rtb.Text = a;
            for (i = 0; i < minus.Count; i++)
            {
                if (minus[i] == -1)
                    break;
                for (j = 0; j < merge.Length; j++)
                {
                    if (minus[i] == merge[j])
                    {
                        rtb.SelectionStart = minus[i] - j;
                        rtb.SelectionLength = 1;
                        rtb.SelectionColor = Color.FromArgb(255, 0, 0);
                        var oldFont = rtb.SelectionFont;
                        var newFont = new Font(oldFont, oldFont.Style ^ FontStyle.Strikeout);
                        rtb.SelectionFont = newFont;
                        break;
                    }
                }
            }

            for (i = 0; i < plus.Count; i++)
            {
                if (plus[i] == -1)
                    break;
                for (j = 0; j < merge.Length; j++)
                {
                    if (plus[i] == merge[j])
                    {
                        rtb.SelectionStart = plus[i] - j;
                        rtb.SelectionLength = 1;
                        rtb.SelectionColor = Color.FromArgb(0, 255, 0);
                        var oldFont = rtb.SelectionFont;
                        var newFont = new Font(oldFont, oldFont.Style ^ FontStyle.Underline);
                        rtb.SelectionFont = newFont;
                        break;
                    }
                }
            }
        }

        public static int[,] LongestCommonSubsequence(string str1, string str2)
        {
            if (string.IsNullOrEmpty(str1) || string.IsNullOrEmpty(str2))
            {
                return null;
            }
            //define the array, note rows of zeros get added to front automatically
            int str1Len = str1.Length;
            int str2Len = str2.Length;
            var num = new int[str1Len + 1, str2Len + 1];
            for (int i = 1; i <= str1Len; i++)
            {
                for (int j = 1; j <= str2Len; j++)
                {
                    if (str1[i - 1] == str2[j - 1])
                    {
                        num[i, j] = num[i - 1, j - 1] + 1;
                    }
                    else
                    {
                        num[i, j] = Max(num[i - 1, j], num[i, j - 1]);
                    }
                }
            }
            return num;
        }

        public static int Max(int a, int b)
        {
            return a >= b ? a : b;
        }

        public static string GetDiff(int[,] c, string stringOld, string stringNew, int i, int j)
        {
            string functionReturnValue = null;
            if (i > 0)
            {
                //both are greater than zero
                if (j > 0)
                {
                    //can only do the following comparison when i and j are greater than or equal zero
                    if (stringOld[i - 1] == stringNew[j - 1])
                    {
                        functionReturnValue = GetDiff(c, stringOld, stringNew, i - 1, j - 1) + stringOld[i - 1];
                    }
                    else
                    {
                        if (i == 0)
                        {
                            functionReturnValue = GetDiff(c, stringOld, stringNew, i, j - 1) + "§" + stringNew[j - 1];
                        }
                        else if (c[i, j - 1] >= c[i - 1, j])
                        {
                            functionReturnValue = GetDiff(c, stringOld, stringNew, i, j - 1) + "§" + stringNew[j - 1];
                        }
                        else if (j == 0)
                        {
                            functionReturnValue = GetDiff(c, stringOld, stringNew, i - 1, j) + "€" + stringOld[i - 1];
                        }
                        else if (c[i, j - 1] < c[i - 1, j])
                        {
                            functionReturnValue = GetDiff(c, stringOld, stringNew, i - 1, j) + "€" + stringOld[i - 1];

                        }
                    }
                    //i is greater than zero
                }
                else
                {
                    if (j == 0)
                    {
                        functionReturnValue = GetDiff(c, stringOld, stringNew, i - 1, j) + "€" + stringOld[i - 1];
                    }
                    else if (c[i, j - 1] < c[i - 1, j])
                    {
                        functionReturnValue = GetDiff(c, stringOld, stringNew, i - 1, j) + "€" + stringOld[i - 1];
                    }
                }
            }
            else
            {
                //j is  greater than zero
                if (j > 0)
                {
                    if (i == 0)
                    {
                        functionReturnValue = GetDiff(c, stringOld, stringNew, i, j - 1) + "§" + stringNew[j - 1];
                    }
                    else if (c[i, j - 1] >= c[i - 1, j])
                    {
                        functionReturnValue = GetDiff(c, stringOld, stringNew, i, j - 1) + "§" + stringNew[j - 1];
                    }
                    //none are greater than or equal zero
                }
            }
            return functionReturnValue;
        }
    }
}
