using System;

namespace Banco
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Sistema Bancário Iniciado ===");

            // CONTA PF
            var pessoa = new PessoaFisica(
                nome: "Gabriel",
                sobrenome: "Souza",
                rg: "1234567",
                cpf: "11122233344",
                dataNasc: new DateTime(2000, 5, 20),
                renda: 3500,
                endereco: "Rua A",
                tel: "99999-9999",
                email: "gabriel@email.com"
            );

            // CONTA POUPANÇA
            var conta = new ContaPoupanca(pessoa, 12345, 1, 2.50);

            conta.Depositar(200);
            conta.Sacar(50);

            Console.WriteLine($"Titular: {pessoa.Nome} {pessoa.Sobrenome}");
            Console.WriteLine($"Idade: {pessoa.Idade}");
            Console.WriteLine($"Saldo atual: {conta.ConsultarSaldo()}");

            Console.WriteLine("\nPressione ENTER para sair...");
            Console.ReadLine();
        }
    }
}
