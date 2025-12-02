using System;
using System.Collections.Generic;
namespace Banco
{
    public static class Auxiliar
    {
        public static int CalcularIdade(DateTime data)
        {
            var hoje = DateTime.Today;
            int idade = hoje.Year - data.Year;
            if (data.Date > hoje.AddYears(-idade)) idade--;
            return idade;
        }
        public static string FaixaEtaria(int idade)
        {
            if (idade <= 11) return "criança";
            else if (idade <= 21) return "jovem";
            else if (idade <= 59) return "adulto";
            else return "idoso";
        }
    }
    public abstract class Pessoa
    {
        public static int NumeroDePessoas { get; set; } = 0;
        public int Id { get; set; }
        public string Endereco { get; set; }
        public string Tel { get; set; }
        public string Email { get; set; }
    }
    public class PessoaFisica : Pessoa
    {
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string Rg { get; set; }
        public string Cpf { get; set; }
        public DateTime DataNasc { get; set; }
        public int Idade { get { return Auxiliar.CalcularIdade(DataNasc); } }
        public string FaixaEtaria { get { return Auxiliar.FaixaEtaria(Idade); } }
        public double Renda { get; set; }
        public PessoaFisica(string nome, string sobrenome, string rg, string cpf, DateTime dataNasc, double renda,
        string endereco, string tel, string email)
        {
            Id = ++NumeroDePessoas;
            Nome = nome;
            Sobrenome = sobrenome;
            Rg = rg;
            Cpf = cpf;
            DataNasc = dataNasc;
            Renda = renda;
            Endereco = endereco;
            Tel = tel;
            Email = email;
        }
    }
    public class PessoaJuridica : Pessoa
    {
        public List<PessoaFisica> Socios { get; set; } = new List<PessoaFisica>();
        public int Cnpj { get; set; }
        public string RazaoSocial { get; set; }
        public string NomeFantasia { get; set; }
        public int InscrEstadual { get; set; }
        public DateTime DataAbertura { get; set; }
        public int Idade { get { return Auxiliar.CalcularIdade(DataAbertura); } }
        public double Faturamento { get; set; }
        public PessoaJuridica(List<PessoaFisica> socios, int cnpj, string razaoSocial, string nomeFantasia,
        int inscrEstadual, DateTime dataAbertura, double faturamento,
        string endereco, string tel, string email)
        {
            Id = ++NumeroDePessoas;
            Socios = socios;
            Cnpj = cnpj;
            RazaoSocial = razaoSocial;
            NomeFantasia = nomeFantasia;
            InscrEstadual = inscrEstadual;
            DataAbertura = dataAbertura;
            Faturamento = faturamento;
            Endereco = endereco;
            Tel = tel;
            Email = email;
        }
    }
    public abstract class Conta
    {
        public Pessoa Titular { get; set; }
        public long Numero { get; set; }
        public int Agencia { get; set; }
        public double Saldo { get; protected set; }
        public double TaxaSaque { get; set; }
        public Conta(Pessoa titular, long numero, int agencia, double taxaSaque)
        {
            Titular = titular;
            Numero = numero;
            Agencia = agencia;
            Saldo = 0.0;
            TaxaSaque = taxaSaque;
        }
        public abstract bool Sacar(double valor);
        public double ConsultarSaldo() => Saldo;
        public virtual bool Transferir(Conta conta, double valor)
        {
            if (Sacar(valor))
            {
                conta.Depositar(valor);
                return true;
            }
            return false;
        }
        public abstract void Depositar(double valor);
    }
    public interface IDepositavel
    {
        void Depositar(double valor);
    }
    public class ContaPoupanca : Conta, IDepositavel
    {
        public ContaPoupanca(Pessoa titular, long numero, int agencia, double taxaSaque)
        : base(titular, numero, agencia, taxaSaque) { }
        public override bool Sacar(double valor)
        {
            if (Saldo - valor - TaxaSaque >= 0)
            {
                Saldo -= (valor + TaxaSaque);
                return true;
            }
            return false;
        }
        public override void Depositar(double valor)
        {
            Saldo += valor;
        }
    }
    public class ContaSalario : Conta
    {
        public ContaSalario(Pessoa titular, long numero, int agencia)
        : base(titular, numero, agencia, 0) { }
        public override bool Sacar(double valor)
        {
            if (Saldo - valor >= 0)
            {
                Saldo -= valor;
                return true;
            }
            return false;
        }
        public override bool Transferir(Conta conta, double valor)
        {
            if (Titular == conta.Titular && Sacar(valor))
            {
                conta.Depositar(valor);
                return true;
            }
            return false;
        }
        public override void Depositar(double valor) { Saldo += valor; }
    }
    public class ContaCorrente : Conta, IDepositavel
    {
        public string Tipo { get; private set; }
        public double Limite { get; private set; }
        public double TaxaDoLimite { get; private set; }
        public ContaCorrente(Pessoa titular, long numero, int agencia, string tipo, double renda)
        : base(titular, numero, agencia, 0)
        {
            Tipo = tipo.ToUpper();
            if (Tipo == "ESPECIAL")
            {
                if (renda <= 5000)
                    throw new ArgumentException("Faturamento/renda insuficiente para conta especial.");
                Limite = renda * 2.5;
                TaxaDoLimite = 0.02;
            }
            else
            {
                Limite = renda * 1.5;
                TaxaDoLimite = 0.05;
            }
        }
        public override bool Sacar(double valor)
        {
            if (Saldo - valor >= -Limite)
            {
                Saldo -= valor;
                return true;
            }
            return false;
        }
        public override void Depositar(double valor)
        {
            Saldo += valor;
        }
        public void Pagar(string codigoBarras)
        {
        }
        public void Emprestimo(double valor)
        {
            if (Saldo + valor <= Limite)
                Saldo += valor;
        }
    }