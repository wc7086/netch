﻿using Netch.Forms;

namespace Netch.Servers
{
    public class Socks5Form : ServerForm
    {
        public Socks5Form(Socks5Server? server = default)
        {
            server ??= new Socks5Server();
            Server = server;
            CreateTextBox("Username", "Username", s => true, s => server.Username = s, server.Username);
            CreateTextBox("Password", "Password", s => true, s => server.Password = s, server.Password);
        }

        protected override string TypeName { get; } = "Socks5";
    }
}