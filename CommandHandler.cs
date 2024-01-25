using Discord;
using DotNetEnv;
using Discord.WebSocket;
using System.Security.Cryptography.X509Certificates;
using Discord.Net;
using Discord.Commands;
using System.Reflection;
using Newtonsoft.Json;
using Discord.Interactions;
using Microsoft.VisualBasic;
using Fuyuji;
using System;
using Discord.Rest;
using System.Threading.Tasks;

namespace Commands
{
    public class CommandHandler
    {
        private readonly DiscordSocketClient client;

        public CommandHandler(DiscordSocketClient client)
        {
            this.client = client;
            client.Ready += Client_Ready;
        }

        //Aqui é a criação de uma task que irá rodar os comandos quando o client(do bot) estiver em execução
        public async Task Client_Ready()
        {
            //nós pegamos a guild e colocamos em uma variável
            var guild = client.GetGuild(1091142595813580820);

            //criamos um criador de comandos slash, é tipo um criador embed só que para comandos slash
            var guildCommand = new SlashCommandBuilder();

            //criação do primeiro comando e damos um nome a ele ao qual será chamado
            guildCommand.WithName("primeiro-comando");

            //Criação da Descrição do comando.
            guildCommand.WithDescription("Este é o meu primeiro comando");

            //Agora vamos criar um comando global
            var globalCommand = new SlashCommandBuilder();
            
            //nome do comando global
            globalCommand.WithName("meu-primeiro-comando-global");

            //Descrição do comando global
            globalCommand.WithDescription("Este é o meu primeiro comando global");

            //comando para as informações do bot
            var meunome = new SlashCommandBuilder();
            meunome.WithName("meu-nome");
            meunome.WithDescription("Comando que mostra algumas informações do bot");
            
            //comando para uma criação da história do bot
            var sobremim = new SlashCommandBuilder();
            sobremim.WithName("sobre-mim");
            sobremim.WithDescription("Comando que mostra a história do bot");


            try
            {
                //agora que nós temos um construtor, nós podemos chamar o método CreateApplicationCommandAsync para fazer nossos comandos slash
                await guild.CreateApplicationCommandAsync(guildCommand.Build());

                // E fazemos a mesma coisa, só que com o global e sem o guild do canal.
                await client.CreateGlobalApplicationCommandAsync(globalCommand.Build());

                //criação do comando da história do bot
                await guild.CreateApplicationCommandAsync(sobremim.Build());

                //criação do comando das informações do bot
                await guild.CreateApplicationCommandAsync(meunome.Build());

                //usar o evento ready é uma implementação simples para o bem do exemplo.
                //para um bot de produção, é recomendado executar somente os comandos globais para cada comando.
            }
            catch (ApplicationCommandException exception)
            {
                //se nosso comando for inválido, nós jogamos ele no catch. Essa execeção contém o caminho para o erro assim como a mensagem do erro. você pode serializar o campo do erro na execeção para ter uma forma visual do erro.
                var json = JsonConvert.SerializeObject(exception.Errors, Formatting.Indented);

                //você pode mandar este erro para algum lugar ou simplesmente printá-lo no console.
                Console.WriteLine(json);
            }

            //hookando o evento no client
            client.SlashCommandExecuted += SlashCommandHandler;


            //Task para a responsividade do comando executado.
            async Task SlashCommandHandler(SocketSlashCommand command)
            { 
                //Criação da data de criação do bot.
                DateTime date = new(2024, 1, 4, 1, 6, 0);

                if (command.Data.Name == "primeiro-comando")
                {
                    await command.RespondAsync($"Você executou o meu primeiro comando de guild, ou o {command.Data.Name}");
                }
                if (command.Data.Name == "meu-primeiro-comando-global")
                {
                    await command.RespondAsync($"Você executou o meu primeiro comando global, ou o {command.Data.Name}");
                }
                if (command.Data.Name == "sobre-mim")
                {
                    await command.RespondAsync("Meu nome é fuyuji. Sou o fruto de um projeto em C# que o meu dono, José Antônio, resolveu fazer para consolidar os seus conhecimentos nesta linguagem de programação.");
                }
                if (command.Data.Name == "meu-nome")
                {
                    await command.RespondAsync($"Olá! Meu nome é fuyuji, e fui criado em {date}");
                }
            }
        }
    }
}


