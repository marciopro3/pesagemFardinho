using System;
using System.Collections.Generic;
using System.IO;

namespace Grupo_JCN___Pesagem_Offline_de_Fardinhos
{
    public static class ConfigManager
    {
        private static readonly string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.txt");

        public static Dictionary<string, string> LerConfiguracoes()
        {
            var config = new Dictionary<string, string>();

            if (!File.Exists(configPath))
                return config;

            var linhas = File.ReadAllLines(configPath);
            foreach (var linha in linhas)
            {
                if (string.IsNullOrWhiteSpace(linha) || !linha.Contains("=")) continue;

                var partes = linha.Split(new[] { '=' }, 2);
                config[partes[0]] = partes[1];
            }

            return config;
        }

        public static void SalvarConfiguracoes(Dictionary<string, string> config)
        {
            using (StreamWriter sw = new StreamWriter(configPath, false))
            {
                foreach (var par in config)
                {
                    sw.WriteLine($"{par.Key}={par.Value}");
                }
            }
        }
    }
}
