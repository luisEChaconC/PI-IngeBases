namespace API_Registro_Nacional.Helpers
{
    public class LegalIDValidator
    {
        public static bool IsLegalIDValid(string legalID)
        {
            if (legalID.Length != 10 || !legalID.All(char.IsDigit))
                return false;

            if (legalID.StartsWith("0") || legalID.StartsWith("8"))
                return false;

            if (legalID.EndsWith("00000"))
                return false;

            return true;
        }
    }
}
