using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Rendering;

namespace AvaloniaApplication2
{
    public partial class MainWindow : Window
    {
        List<string> appNames = new List<string>();
        List<string> appUrls = new List<string>();
        List<string> fileNames = new List<string>();
        Window1 window1 = new Window1();

        public MainWindow()
        {
            try
            {
                for (int i = 0; i < ConfigurationManager.AppSettings.Keys.Count;)
                {
                    appNames.Add(ConfigurationManager.AppSettings[i]);
                    i++;
                    appUrls.Add(ConfigurationManager.AppSettings[i]);
                    i++;
                    fileNames.Add(ConfigurationManager.AppSettings[i]);
                    i++;
                }
            }
            catch (Exception ex)
            { 
                Console.WriteLine(ex);
            }
            InitializeComponent();
        }
        public async Task downloadfile()
        {
            await Task.Run(async ()=>
            {
                for (int i = 0; i < appNames.Count; i++)
                {
                    string url;
                    url = appUrls[i];
                    string path = Environment.CurrentDirectory.ToString()+@"\files\";
                    Console.WriteLine($"Name:{fileNames[i]} URL:{url} Path:{path}");
                    using(WebClient wc = new WebClient()){
                        Uri uri = new Uri(url);
                        wc.DownloadFile(uri, fileNames[i]);
                    }
                    Console.WriteLine(@$"move file to: {path+fileNames[i]}");
                    if (Directory.Exists(path)){
                        Console.WriteLine("Directory exists");
                    }
                    else
                    {
                        Directory.CreateDirectory(path);
                    }
                    File.Move(fileNames[i], path+fileNames[i], true);
                    File.Delete(fileNames[i]);
                }
            });
        }
        public async void Button_Click(object sender, RoutedEventArgs e)
        {
            window1.Show();
            await downloadfile();
            window1.Close();
        }
    }
}