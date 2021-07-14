using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;


public class IndiaCurrencyConverter
{
    private Hashtable htPunctuation;
    private List<DictionaryEntry> listStaticSuffix;
    private List<DictionaryEntry> listStaticPrefix;
    private List<DictionaryEntry> listHelpNotation;
    private System.Drawing.Color color;

    public IndiaCurrencyConverter()
    {
        htPunctuation = new Hashtable();
        listStaticPrefix = new List<DictionaryEntry>();
        listStaticSuffix = new List<DictionaryEntry>();
        listHelpNotation = new List<DictionaryEntry>();
        LoadStaticPrefix();
        LoadStaticSuffix();
        LoadHelpofNotation();
    }
    public string ConvertToWord(string Value, System.Drawing.Color color,string mode)
    {
        this.color = color;
        String ConvertedString = String.Empty;
        if (!(Value.ToString().Length > 40))
        {
            if (IsNumeric(Value.ToString()))
            {
                try
                {
                    string strValue = Reverse(Value);
                    switch (strValue.Length)
                    {
                        case 1:
                            if (int.Parse(strValue.ToString()) > 0)
                                ConvertedString = GetWordConversion(Value);
                            else
                                ConvertedString = "Zero ";
                            break;
                        case 2:
                            ConvertedString = GetWordConversion(Value);
                            ; break;
                        default:
                            InsertToPunctuationTable(strValue);
                            ReverseHashTable();
                            ConvertedString = ReturnHashtableValue();
                            break;
                    }
                }
                catch (Exception Ex)
                {
                    ConvertedString = "Unexpected Error Occured <br/>";
                    ConvertedString += Ex.Message;
                }
            }
            else
            {
                ConvertedString = "Please Enter Numbers Only, Decimal Values Are not supported";
            }
        }
        else
        {
            ConvertedString = "Please Enter Value in Less Then or Equal to 40 Digit";
        }
        return ConvertedString.Substring(0, 1).ToUpper() + ConvertedString.Substring(1).ToLower();
    }
    internal bool IsNumeric(string ValueInNumeric)
    {
        bool IsFine = true;
        foreach (char ch in ValueInNumeric)
        {
            if (!(ch >= '0' && ch <= '9'))
            {
                IsFine = false;
            }
        }
        return IsFine;
    }
    private string ReturnHashtableValue()
    {
        string strFinalString = String.Empty;
        for (int i = htPunctuation.Count; i > 0; i--)
        {
            if (GetWordConversion((htPunctuation[i]).ToString()) != "")
                strFinalString = strFinalString + GetWordConversion((htPunctuation[i]).ToString()) + StaticPrefixFind((i).ToString());
        }
        return strFinalString;
    }
    private void LoadStaticSuffix()
    {
        listStaticSuffix.Add(new DictionaryEntry(1, "one "));
        listStaticSuffix.Add(new DictionaryEntry(2, "two "));
        listStaticSuffix.Add(new DictionaryEntry(3, "three "));
        listStaticSuffix.Add(new DictionaryEntry(4, "four "));
        listStaticSuffix.Add(new DictionaryEntry(5, "five "));
        listStaticSuffix.Add(new DictionaryEntry(6, "six "));
        listStaticSuffix.Add(new DictionaryEntry(7, "seven "));
        listStaticSuffix.Add(new DictionaryEntry(8, "eight "));
        listStaticSuffix.Add(new DictionaryEntry(9, "nine "));
        listStaticSuffix.Add(new DictionaryEntry(10, "ten "));
        listStaticSuffix.Add(new DictionaryEntry(11, "eleven "));
        listStaticSuffix.Add(new DictionaryEntry(12, "twelve "));
        listStaticSuffix.Add(new DictionaryEntry(13, "thirteen "));
        listStaticSuffix.Add(new DictionaryEntry(14, "fourteen "));
        listStaticSuffix.Add(new DictionaryEntry(15, "fifteen "));
        listStaticSuffix.Add(new DictionaryEntry(16, "sixteen "));
        listStaticSuffix.Add(new DictionaryEntry(17, "seventeen "));
        listStaticSuffix.Add(new DictionaryEntry(18, "eighteen "));
        listStaticSuffix.Add(new DictionaryEntry(19, "nineteen "));
        listStaticSuffix.Add(new DictionaryEntry(20, "twenty "));
        listStaticSuffix.Add(new DictionaryEntry(30, "thirty "));
        listStaticSuffix.Add(new DictionaryEntry(40, "fourty "));
        listStaticSuffix.Add(new DictionaryEntry(50, "fifty "));
        listStaticSuffix.Add(new DictionaryEntry(60, "sixty "));
        listStaticSuffix.Add(new DictionaryEntry(70, "seventy "));
        listStaticSuffix.Add(new DictionaryEntry(80, "eighty "));
        listStaticSuffix.Add(new DictionaryEntry(90, "ninty "));
    }
    private void LoadStaticPrefix()
    {
        listStaticPrefix.Add(new DictionaryEntry(2, "thousand "));
        listStaticPrefix.Add(new DictionaryEntry(3, "lakh "));
        listStaticPrefix.Add(new DictionaryEntry(4, "crore "));
        listStaticPrefix.Add(new DictionaryEntry(5, "arab "));
        listStaticPrefix.Add(new DictionaryEntry(6, "kharab "));
        listStaticPrefix.Add(new DictionaryEntry(7, "neel "));
        listStaticPrefix.Add(new DictionaryEntry(8, "padma "));
        listStaticPrefix.Add(new DictionaryEntry(9, "shankh "));
        listStaticPrefix.Add(new DictionaryEntry(10, "maha-shankh "));
        listStaticPrefix.Add(new DictionaryEntry(11, "ank "));
        listStaticPrefix.Add(new DictionaryEntry(12, "jald "));
        listStaticPrefix.Add(new DictionaryEntry(13, "madh "));
        listStaticPrefix.Add(new DictionaryEntry(14, "paraardha "));
        listStaticPrefix.Add(new DictionaryEntry(15, "ant "));
        listStaticPrefix.Add(new DictionaryEntry(16, "maha-ant "));
        listStaticPrefix.Add(new DictionaryEntry(17, "shisht "));
        listStaticPrefix.Add(new DictionaryEntry(18, "singhar "));
        listStaticPrefix.Add(new DictionaryEntry(19, "maha-singhar "));
        listStaticPrefix.Add(new DictionaryEntry(20, "adant-singhar "));
    }
    private void LoadHelpofNotation()
    {
        listHelpNotation.Add(new DictionaryEntry(2, "=1,000 (3 Trailing Zeros)"));
        listHelpNotation.Add(new DictionaryEntry(3, "=1,00,000 (5 Trailing Zeros)"));
        listHelpNotation.Add(new DictionaryEntry(4, "=1,00,00,000 (7 Trailing Zeros)"));
        listHelpNotation.Add(new DictionaryEntry(5, "=1,00,00,00,000 (9 Trailing Zeros)"));
        listHelpNotation.Add(new DictionaryEntry(6, "=1,00,00,00,00,000 (11 Trailing Zeros)"));
        listHelpNotation.Add(new DictionaryEntry(7, "=1,00,00,00,00,00,000 (13 Trailing Zeros)"));
        listHelpNotation.Add(new DictionaryEntry(8, "=1,00,00,00,00,00,00,000 (15 Trailing Zeros)"));
        listHelpNotation.Add(new DictionaryEntry(9, "=1,00,00,00,00,00,00,00,000 (17 Trailing Zeros)"));
        listHelpNotation.Add(new DictionaryEntry(10, "=1,00,00,00,00,00,00,00,00,000 (19 Trailing Zeros)"));
        listHelpNotation.Add(new DictionaryEntry(11, "=1,00,00,00,00,00,00,00,00,00,000 (21 Trailing Zeros)"));
        listHelpNotation.Add(new DictionaryEntry(12, "=1,00,00,00,00,00,00,00,00,00,00,000 (23 Trailing Zeros)"));
        listHelpNotation.Add(new DictionaryEntry(13, "=1,00,00,00,00,00,00,00,00,00,00,00,000 (25 Trailing Zeros)"));
        listHelpNotation.Add(new DictionaryEntry(14, "=1,00,00,00,00,00,00,00,00,00,00,00,00,000 (27 Trailing Zeros)"));
        listHelpNotation.Add(new DictionaryEntry(15, "=1,00,00,00,00,00,00,00,00,00,00,00,00,00,000 (29 Trailing Zeros)"));
        listHelpNotation.Add(new DictionaryEntry(16, "=1,00,00,00,00,00,00,00,00,00,00,00,00,00,00,000 (31 Trailing Zeros)"));
        listHelpNotation.Add(new DictionaryEntry(17, "=1,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,000 (33 Trailing Zeros)"));
        listHelpNotation.Add(new DictionaryEntry(18, "=1,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,000 (35 Trailing Zeros)"));
        listHelpNotation.Add(new DictionaryEntry(19, "=1,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,000 (37 Trailing Zeros)"));
        listHelpNotation.Add(new DictionaryEntry(20, "=1,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,00,000 (39 Trailing Zeros)"));
    }
    private void ReverseHashTable()
    {
        Hashtable htTemp = new Hashtable();
        foreach (DictionaryEntry item in htPunctuation)
        {
            htTemp.Add(item.Key, Reverse(item.Value.ToString()));
        }
        htPunctuation.Clear();
        htPunctuation = htTemp;
    }
    private void InsertToPunctuationTable(string strValue)
    {
        htPunctuation.Add(1, strValue.Substring(0, 3).ToString());
        int j = 2;
        for (int i = 3; i < strValue.Length; i = i + 2)
        {
            if (strValue.Substring(i).Length > 0)
            {
                if (strValue.Substring(i).Length >= 2)
                    htPunctuation.Add(j, strValue.Substring(i, 2).ToString());
                else
                    htPunctuation.Add(j, strValue.Substring(i, 1).ToString());
            }
            else
                break;
            j++;

        }
    }
    private string Reverse(string strValue)
    {
        string Reversed = String.Empty;
        foreach (char Ch in strValue)
        {
            Reversed = Ch + Reversed;
        }
        return Reversed;
    }
    private string GetWordConversion(string inputNumber)
    {
        string ToReturnWord = String.Empty;
        if (inputNumber.Length <= 3 && inputNumber.Length > 0)
        {
            if (inputNumber.Length == 3)
            {
                if (int.Parse(inputNumber.Substring(0, 1)) > 0)
                    ToReturnWord = ToReturnWord + StaticSuffixFind(inputNumber.Substring(0, 1)) + "hundread ";

                string TempString = StaticSuffixFind(inputNumber.Substring(1, 2));

                if (TempString == "")
                {
                    ToReturnWord = ToReturnWord + StaticSuffixFind(inputNumber.Substring(1, 1) + "0");
                    ToReturnWord = ToReturnWord + StaticSuffixFind(inputNumber.Substring(2, 1));
                }
                ToReturnWord = ToReturnWord + TempString;
            }
            if (inputNumber.Length == 2)
            {
                string TempString = StaticSuffixFind(inputNumber.Substring(0, 2));
                if (TempString == "")
                {
                    ToReturnWord = ToReturnWord + StaticSuffixFind(inputNumber.Substring(0, 1) + "0");
                    ToReturnWord = ToReturnWord + StaticSuffixFind(inputNumber.Substring(1, 1));
                }
                ToReturnWord = ToReturnWord + TempString;
            }
            if (inputNumber.Length == 1)
            {
                ToReturnWord = ToReturnWord + StaticSuffixFind(inputNumber.Substring(0, 1));
            }

        }
        return ToReturnWord;
    }
    internal string StaticSuffixFind(string NumberKey)
    {
        string ValueFromNumber = String.Empty;
        foreach (DictionaryEntry Pair in listStaticSuffix)
        {
            if (Pair.Key.ToString().Trim() == NumberKey.Trim())
            {
                ValueFromNumber = Pair.Value.ToString();
            }
        }
        return ValueFromNumber;
    }
    private string StaticPrefixFind(string NumberKey)
    {
        string ValueFromNumber = String.Empty;
        foreach (DictionaryEntry Pair in listStaticPrefix)
        {
            if (Pair.Key.ToString().Trim() == NumberKey.Trim())
            {
                ValueFromNumber = Pair.Value.ToString();
                /*
                if (this.color == null)
                    ValueFromNumber = Pair.Value.ToString();
                else
                    ValueFromNumber = "<span title='" + StaticHelpNotationFind(Pair.Key.ToString()) + "' style='color:" + this.color.ToKnownColor().ToString() + "'>" + Pair.Value.ToString() + "</span>";
                 */
            }
        }
        return ValueFromNumber;
    }
    private string StaticHelpNotationFind(string NumberKey)
    {
        string HelpText = String.Empty;
        foreach (DictionaryEntry Pair in listHelpNotation)
        {
            if (Pair.Key.ToString().Trim() == NumberKey.Trim())
            {
                HelpText = Pair.Value.ToString();
            }
        }
        return HelpText;
    }
}
