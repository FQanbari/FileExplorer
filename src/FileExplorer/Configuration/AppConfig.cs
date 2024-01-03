using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExplorer.Configuration;

public class AppConfig
{
    public string DefaultSearchType { get; set; } = "TXT";
    public string PluginPath { get; set; }
    public int DefaultSearchThreshold { get; set; }
    public Dictionary<string, int> PluginSearchThresholds { get; set; }
}
