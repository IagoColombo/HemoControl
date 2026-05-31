using HemoControl.Core.Data;
using HemoControl.Core.Models;
using HemoControl.Core.Services;

Environment.SetEnvironmentVariable(
    "ConnectionStrings__Default",
    "Server=localhost;Port=3306;Database=hemocontrol;User=root;Password=;");

var context = new PacienteService(new HemoControlDbContext());

Console.WriteLine("Digite o nome do paciente:");
var nome = Console.ReadLine();
Console.WriteLine("Digite o CPF:");
var cpf = Console.ReadLine();
Console.WriteLine("Digite o tipo de hemofilia (Hemofilia A / Hemofilia B / Von Willebrand):");
var tipo = Console.ReadLine();
Console.WriteLine("Digite a dose prescrita em UI:");
var dose = decimal.Parse(Console.ReadLine());
Console.WriteLine("Digite o número de aplicações por semana:");
var freq = int.Parse(Console.ReadLine());

var paciente = new Paciente
{
    Nome = nome,
    Cpf = cpf,
    TipoHemofilia = tipo,
    DosePrescrita = dose,
    AplicacoesPorSemana = freq
};

var sucesso = context.Criar(paciente, out var erros);

if (sucesso)
    Console.WriteLine("Paciente criado com sucesso! Id: #{0}", paciente.Id);
else
    Console.WriteLine("Erro na criação do paciente.");
