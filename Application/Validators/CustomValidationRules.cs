using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators
{
    public static class CustomValidationRules
    {
        public static bool BeAValidCpf(string? cpf)
        {
            //Lógica simplificada apenas para validar o formato básico do CPF.
            if (string.IsNullOrEmpty(cpf)) return true;
            cpf = cpf.Trim().Replace(".", "").Replace("-", "");
            if (cpf.Length != 11) return false;
            if (new string(cpf[0], 11) == cpf) return false;
            return true;
        }
    }
}
