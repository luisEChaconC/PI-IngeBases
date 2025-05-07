namespace API_Registro_Nacional.Helpers
{
    public class LegalIDValidator
    {
        public static bool IsLegalIDValid(string legalID)
        {
            return legalID.Length == 10 && legalID.All(char.IsDigit) && 
                   !legalID.StartsWith("0") && !legalID.StartsWith("8") && 
                   !legalID.EndsWith("00000");
        }
    }
}
