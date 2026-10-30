using System.Reflection;
using DbUp;

const string connectionStringEnvironmentVariableName = "PT_MIGRATOR_CONNECTION_STRING";

var connectionString = Environment.GetEnvironmentVariable(connectionStringEnvironmentVariableName);

if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException(
        $"Переменная окружения \"{connectionStringEnvironmentVariableName}\" не задана.");
}

var upgrader = DeployChanges.To
    .PostgresqlDatabase(connectionString)
    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
    .WithTransactionPerScript()
    .LogToConsole()
    .Build();

var result = upgrader.PerformUpgrade();

if (!result.Successful)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(result.Error);
    Console.ResetColor();
#if DEBUG
    Console.ReadLine();
#endif
    return -1;
}

Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("Миграции применены!");
Console.ResetColor();
return 0;