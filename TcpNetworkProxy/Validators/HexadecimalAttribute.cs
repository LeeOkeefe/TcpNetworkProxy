using System.ComponentModel.DataAnnotations;

namespace TcpNetworkProxy.Validators;

public sealed class HexadecimalAttribute : ValidationAttribute
{
    private static readonly char[] HexCharacters = { 'A', 'B', 'C', 'D', 'E', 'F' };
    private static readonly char[] HexNumbers = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

    public override bool IsValid(object value)
    {
        var hexadecimalString = value as string;

        if (string.IsNullOrWhiteSpace(hexadecimalString))
        {
            return false;
        }
        
        hexadecimalString = hexadecimalString.Replace(" ", string.Empty);
        hexadecimalString = hexadecimalString.Replace("-", string.Empty);

        if (hexadecimalString.Length % 2 != 0)
        {
            return false;
        }

        for (var i = 0; i < hexadecimalString.Length; i+=2)
        {
            var hex = hexadecimalString.Substring(i, 2);
            if (!IsHexCharacter(hex[0]) || !IsHexCharacter(hex[1]))
            {
                return false;
            }
        }
        return true;
    }

    private static bool IsHexCharacter(char c) => HexCharacters.Contains(char.ToUpper(c)) || HexNumbers.Contains(char.ToUpper(c));
}