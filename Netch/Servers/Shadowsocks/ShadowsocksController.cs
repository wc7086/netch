using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Netch.Controllers;
using Netch.Interfaces;
using Netch.Models;

namespace Netch.Servers
{
    public class ShadowsocksController : Guard, IServerController
    {
        public ShadowsocksController() : base("Shadowsocks.exe")
        {
        }

        protected override IEnumerable<string> StartedKeywords => new[] { "listening at" };

        protected override IEnumerable<string> FailedKeywords => new[] { "Invalid config path", "usage", "plugin service exit unexpectedly" };

        public override string Name => "Shadowsocks";

        public ushort? Socks5LocalPort { get; set; }

        public string? LocalAddress { get; set; }

        public async Task<Socks5Server> StartAsync(Server s)
        {
            var server = (ShadowsocksServer)s;

            var arguments = new object?[]
            {
                "-s", await server.AutoResolveHostnameAsync(),
                "-p", server.Port,
                "-b", this.LocalAddress(),
                "-l", this.Socks5LocalPort(),
                "-m", server.EncryptMethod,
                "-k", server.Password,
                "-u", SpecialArgument.Flag,
                "--plugin", server.Plugin,
                "--plugin-opts", server.PluginOption
            };

            await StartGuardAsync(Arguments.Format(arguments));
            return new Socks5LocalServer(IPAddress.Loopback.ToString(), this.Socks5LocalPort(), server.Hostname);
        }
    }
}