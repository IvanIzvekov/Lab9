using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.IO;

namespace lab9.Models
{
    public class Node
    {
        public ObservableCollection<Node> Content { get; set; }
        public Node Parent { get; }
        public string Fullpath { get; }
        public string Name { get; }
        public int ImageCounter { get; set; }
        public Node(string _path, Node? parent)
        {
            Parent = parent;
            Fullpath = _path;
            Name = Path.GetFileName(_path);
            ImageCounter = 0;
            if (!File.Exists(_path))
            {
                Content = new ObservableCollection<Node>();
                try
                {
                    string[] images = Directory.GetFiles(_path, "*.jpg");
                    foreach (string image in images)
                    {
                        Content.Add(new Node(image, this));
                        ImageCounter++;
                    }
                    images = Directory.GetFiles(_path, "*.png");
                    foreach (string image in images)
                    {
                        Content.Add(new Node(image, this));
                        ImageCounter++;
                    }
                }
                catch (UnauthorizedAccessException)
                {

                }
            }
        }
    }
}