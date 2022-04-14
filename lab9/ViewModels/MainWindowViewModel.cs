using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using lab9.Models;
using System.IO;
using ReactiveUI;
using System.Reactive;

namespace lab9.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<Node> Items { get; }
        public string RootFolder { get; set; }
        Node selectedNode;
        string imagePath;
        public Node SelectedNode
        {
            get => selectedNode;
            set
            {
                this.RaiseAndSetIfChanged(ref selectedNode, value);
                ImagePath = value.Fullpath;
            }
        }
        public string ImagePath
        {
            get => imagePath;
            set => this.RaiseAndSetIfChanged(ref imagePath, value);
        }
        public ReactiveCommand<Unit, Unit> Prev { get; }
        public ReactiveCommand<Unit, Unit> Next { get; }
        public MainWindowViewModel()
        {
            RootFolder = @"C:\Users\Иван Извеков\Desktop";//Отредактировать
            Items = new ObservableCollection<Node>();
            Node rootNode = new Node(RootFolder, null);
            rootNode.Content = GetSubFolders(rootNode);
            Items.Add(rootNode);
            SelectedNode = rootNode;
            var clickEnabled = this.WhenAnyValue(x => x.SelectedNode, x => (x.Parent != null && x.Parent.ImageCounter > 1));
            Prev = ReactiveCommand.Create(() => ChangeImage(-1), clickEnabled);
            Next = ReactiveCommand.Create(() => ChangeImage(1), clickEnabled);
        }
        public void ChangeImage(int offset)
        {
            var folder = SelectedNode.Parent;
            if (folder != null)
            {
                foreach (Node content in folder.Content)
                {
                    if (content.Fullpath == ImagePath)
                    {
                        int index = folder.Content.IndexOf(content);
                        index += offset;
                        if (index < folder.Content.Count - folder.ImageCounter) index = folder.Content.Count - 1;
                        else if (index >= folder.Content.Count) index = folder.Content.Count - folder.ImageCounter;
                        SelectedNode = folder.Content[index];
                        break;
                    }
                }
            }
        }
        public ObservableCollection<Node> GetSubFolders(Node dir)
        {
            ObservableCollection<Node> subFolders = new ObservableCollection<Node>();
            string[] subdirs = Directory.GetDirectories(dir.Fullpath, "*", SearchOption.TopDirectoryOnly);
            foreach (string subdir in subdirs)
            {
                try
                {
                    Node thisnode = new Node(subdir, dir);
                    if (Directory.GetDirectories(subdir, "*", SearchOption.TopDirectoryOnly).Length > 0)
                    {
                        ObservableCollection<Node> subfolders = GetSubFolders(thisnode);
                        foreach (Node subfolder in subfolders)
                        {
                            thisnode.Content.Insert(0, subfolder);
                        }
                    }
                    subFolders.Add(thisnode);
                }
                catch (UnauthorizedAccessException)
                {
                    //continue;
                }
            }
            return subFolders;
        }
    }
}