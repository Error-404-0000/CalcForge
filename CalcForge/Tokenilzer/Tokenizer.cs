namespace CalcForge.Tokenilzer;
using System.Text;

public static class Tokenizer {
    public static string[] Tokenize(string Input)
    {
        Input = Input.Replace("\r\n","");
        List<string> stringTokens = new List<string>();
        bool isCurrentlyHoldingaNumber = false;
        StringBuilder currentNumberGroup = new();
        StringBuilder currentFuncName = new();
        bool isCurrentlyHoldingaFunctionName = false;
        for (int i = 0; i < Input.Length; i++)
        {
            if (IsNumber(Input[i]))
            {
                if (isCurrentlyHoldingaNumber)
                {

                    currentNumberGroup.Append(Input[i]);
                }
                else
                {
                    isCurrentlyHoldingaNumber = true;

                    currentNumberGroup.Append(Input[i].ToString());
                }
                continue;
            }
            else if (isCurrentlyHoldingaNumber && Input[i] == '.')
            {
                if (i + 1 < Input.Length && IsNumber(Input[i + 1]))
                {
                    currentNumberGroup.Append(Input[i]);
                    continue;
                }
            }
            else if (isCurrentlyHoldingaNumber)
            {
                isCurrentlyHoldingaNumber = false;

                stringTokens.Add(currentNumberGroup.ToString());
                currentNumberGroup.Clear();

            }
            if (isChar(Input[i]))
            {
                if (isCurrentlyHoldingaFunctionName)
                {
                    currentFuncName.Append(Input[i]);
                }
                else
                {
                    isCurrentlyHoldingaFunctionName = true;

                    currentFuncName.Append(Input[i].ToString());
                }
                continue;
            }
            else if (isCurrentlyHoldingaFunctionName)
            {
                isCurrentlyHoldingaFunctionName = false;
                stringTokens.Add(currentFuncName.ToString());
                currentFuncName.Clear();
                if (Input[i] is not ' ')
                {
                    stringTokens.Add(Input[i].ToString());
                }
            }
            else if (Input[i] is not ' ')
            {

                stringTokens.Add(Input[i].ToString());
            }

        }
        if (isCurrentlyHoldingaNumber)
            stringTokens.Add(currentNumberGroup.ToString());
        else if (isCurrentlyHoldingaFunctionName)
            stringTokens.Add(currentFuncName.ToString());

        return stringTokens.ToArray();
    }
    public static bool IsFunctionName(string str)
    {
        for (int i = 0; i < str.Length; i++)
        {
            if (!isChar(str[i]))
                return false;
        }
        return true;
    }
    public static bool isChar(char ch)
    {
       
        return ch is >= 'a' and <= 'z' || ch is >= 'A' and <= 'Z';
    }
    public static bool IsNumber(string numstr)
    {
        bool hitdot = false;
        for (int i = 0; i < numstr.Length; i++)
        {
            if (numstr[i] >= '0' && numstr[i] <= '9')
                continue;
            if (hitdot is false && numstr[i] == '.')
            {
                if (i + 1 < numstr.Length && IsNumber(numstr[i + 1]))
                {
                    hitdot = true;
                    continue;
                }
            }
            return false;
        }
        return true;
    }
    private static bool IsNumber(char num)
    {
       
        return num >= '0' && num <= '9';
    }
}