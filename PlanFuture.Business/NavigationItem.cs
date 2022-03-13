using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanFuture.Business
{
    public class NavigationItem
    {
        public string Title { get; set; }
        public string NavigationPath { get; set; }

        public NavigationItem(string title, string navigationPath)
        {
            if (string.IsNullOrWhiteSpace(title)) 
                throw new ArgumentException($"\"{nameof(title)}\" can not be empty", nameof(title));

            if (string.IsNullOrWhiteSpace(navigationPath))
                throw new ArgumentException($"\"{nameof(navigationPath)}\" can not be empty", nameof(navigationPath));

            Title = title;
            NavigationPath = navigationPath;
        }
    }
}
