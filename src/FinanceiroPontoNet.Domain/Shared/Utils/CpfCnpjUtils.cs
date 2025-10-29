using System.Text.RegularExpressions;

namespace FinanceiroPontoNet.Domain.Shared.Utils
{
    public class CpfCpnjUtils
    {
        public static bool Validate(string document)
        {
            return ValidateCpf(document) && ValidateCnpj(document);
        }

        public static string RemoveFormat(string document)
        {
            return Regex.Replace(document, @"\D", "");
        }

        public static string FormatCpf(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
            {
                return string.Empty;
            }

            string numbersOnly = Regex.Replace(cpf, @"\D", "");
            if (numbersOnly.Length != 11)
            {
                return numbersOnly;
            }
            return Convert.ToInt64(numbersOnly).ToString(@"000\.000\.000\-00");
        }

        public static bool ValidateCpf(string cpf)
        {
            if (string.IsNullOrEmpty(cpf))
                return false;

            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");

            var invalidCpfs = new List<string>()
            {
                "00000000000",
                "11111111111",
                "22222222222",
                "33333333333",
                "44444444444",
                "55555555555",
                "66666666666",
                "77777777777",
                "88888888888",
                "99999999999",
            };

            if (invalidCpfs.Contains(cpf))
                return false;

            if (!Regex.IsMatch(cpf, @"^\d+$"))
                return false;

            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            int soma;
            int resto;
            if (cpf.Length != 11)
                return false;
            tempCpf = cpf.Substring(0, 9);
            soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCpf = tempCpf + digito;
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            return cpf.EndsWith(digito);
        }

        public static string FormatCnpj(string cnpj)
        {
            if (string.IsNullOrWhiteSpace(cnpj))
            {
                return string.Empty;
            }

            string numbersOnly = Regex.Replace(cnpj, @"\D", "");
            if (numbersOnly.Length != 11)
            {
                return numbersOnly;
            }
            return Convert.ToInt64(numbersOnly).ToString(@"00\.000\.000\/0000\-00");
        }

        public static bool ValidateCnpj(string cnpj)
        {
            cnpj = Regex.Replace(cnpj, "[^0-9]", "");

            if (cnpj.Length != 14 || cnpj.Distinct().Count() == 1)
                return false;

            var multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            var multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCnpj = cnpj.Substring(0, 12);
            int soma = 0;

            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];

            int resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;

            string digito = resto.ToString();
            tempCnpj += digito;
            soma = 0;

            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;

            digito += resto.ToString();

            return cnpj.EndsWith(digito);
        }
    }
}
