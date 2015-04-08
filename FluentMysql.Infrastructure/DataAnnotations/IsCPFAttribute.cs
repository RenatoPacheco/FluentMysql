using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FluentMysql.Infrastructure.DataAnnotations
{
    public class IsCPFAttribute : BaseValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (!base.IsValid(value))
            {
                Boolean retorno;
                string pattern;
                Regex regex;

                retorno = false;

                // Checando se o valor não é nulo ou tem o mínimo de 11 dígitos
                if (Input != null)
                {
                    pattern = @"^([0-9]{11})$";
                    regex = new Regex(pattern);
                    retorno = regex.Match(Input).Success;
                    if (retorno == false)
                    {
                        pattern = @"^([0-9]{9}-[0-9]{2})$";
                        regex = new Regex(pattern);
                        retorno = regex.Match(Input).Success;
                        if (retorno == false)
                        {
                            pattern = @"^([0-9]{3}\.[0-9]{3}\.[0-9]{3}-[0-9]{2})$";
                            regex = new Regex(pattern);
                            retorno = regex.Match(Input).Success;
                        }
                    }
                }
                // Checando se tem apenas números
                pattern = @"^([0-9]{11})$";
                regex = new Regex(pattern);
                retorno = regex.Match(Input).Success;
                //Checando se não são 11 dígitos iguais
                if (retorno == true)
                {
                    Input = Input.Replace("-", "");
                    Input = Input.Replace(".", "");
                    pattern = @"^([1]{11}|[2]{11}|[3]{11}|[4]{11}|[5]{11}|[6]{11}|[7]{11}|[8]{11}|[9]{11}|[0]{11})$";
                    regex = new Regex(pattern);
                    retorno = !regex.Match(Input).Success;
                }

                if (retorno == true)
                {
                    /**/
                    // Para validar calculamos usando os 9 primeiro dígito
                    string cpf = Input.Substring(0, 9);
                    int soma;
                    int resto = 0;
                    int quociente = 0;
                    int primeiroDigito = 0;
                    int segundoDigito = 0;
                    int multiplicador;
                    // Calculando o primeiro dígito
                    multiplicador = 10;
                    soma = 0;
                    for (int indice = 0; indice < cpf.Length; indice++)
                    {
                        soma += (int.Parse(cpf[indice].ToString()) * multiplicador);
                        multiplicador--;
                    }
                    resto = soma % 11;
                    quociente = (soma - resto) / 11;
                    if (resto < 2)
                    {
                        primeiroDigito = 0;
                    }
                    else
                    {
                        primeiroDigito = 11 - resto;
                    }
                    // Calculando o segundo dígito
                    // para calcular adicionamos o digito ao cpf
                    cpf = cpf + primeiroDigito.ToString();
                    multiplicador = 11;
                    soma = 0;
                    for (int indice = 0; indice < cpf.Length; indice++)
                    {
                        soma += (int.Parse(cpf[indice].ToString()) * multiplicador);
                        multiplicador--;
                    }
                    resto = soma % 11;
                    quociente = (soma - resto) / 11;
                    if (resto < 2)
                    {
                        segundoDigito = 0;
                    }
                    else
                    {
                        segundoDigito = 11 - resto;
                    }
                    // Para finalizar adicionamos o digito ao cpf
                    cpf = cpf + segundoDigito.ToString();
                    // Agora que obtivemos um cpf completo comparamos o resultado com o informado
                    if (Input != cpf)
                    {
                        retorno = false;
                    }
                }
            }
            return true;
        }
    }
}
