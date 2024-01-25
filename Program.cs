using Discord;
using DotNetEnv;
using Discord.WebSocket;
using System.Security.Cryptography.X509Certificates;
using Discord.Net;
using Discord.Commands;
using System.Windows.Input;
using System.Threading.Tasks;
using Commands;

namespace Fuyuji
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program().RunbotAsync().GetAwaiter().GetResult();
        }

        async Task RunbotAsync()
        {
            //Carrega o arquivo .env e armazena ele dentro de uma variável chamada discordToken
            Env.Load();
            var discordToken = Environment.GetEnvironmentVariable("DiscordToken");

            //Inicia a inicialização do bot
            DiscordSocketClient client = await InitializeBotAsync(discordToken);
            

            //Desligamento do bot manual
            Console.WriteLine("Digite qualquer tecla para encerrar o bot.");
            Console.ReadKey();

            //Aguarda a função de desligar o bot caso seja pressionada uma tecla
            await DisconnectBotAsync(client);
        }

        //Função para a inicialização, login e funcionamento do bot.
        private async Task<DiscordSocketClient> InitializeBotAsync(string discordToken)
        {
            var client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Verbose
            });

            //Loga o bot com o token fornecido
            await client.LoginAsync(TokenType.Bot, discordToken);

            //Mostra os status do login do bot no console
            Console.WriteLine($"ESTADO DE LOGIN: {client.LoginState}");

            //Inicia o Bot
            await client.StartAsync();


            //Evento que é executado quando o bot está pronto, nele informo o ID do bot e o canal, e digo para ele mandar uma mensagem confirmando o funcionamento.
            client.Ready += async () =>
            { 
                var guild = client.GetGuild(1091142595813580820);
                var channel = guild.GetTextChannel(1091142596509843499);

                Console.WriteLine("Bot Funcionando!");
                await channel.SendMessageAsync("Estou Funcionando!");
            };

            //informações complementares do log que aparecem à esquerda que são a data e a hora do log.
            client.Log += (log) =>
            {
                Console.WriteLine($"{DateTime.Now} => {log.Message}");
                return Task.CompletedTask;
            };

            var CommandHandler = new CommandHandler(client);

            return client;
        } //Função para fazer o desligamento manual do bot
        private async Task DisconnectBotAsync(DiscordSocketClient client)
        {
            Console.WriteLine("Desconectando o bot...");
            await client.LogoutAsync();
            await client.StopAsync();
        }
    }
}
