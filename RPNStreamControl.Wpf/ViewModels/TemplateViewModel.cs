using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPNStreamControl.Wpf.ViewModels
{
    public class TemplateViewModel : ObservableObject
    {
        public TemplateViewModel()
        {
                
        }

		private string _title;

		public string Title
		{
			get => _title;
			set => SetProperty(ref _title, value);
		}

		private string _anchor;

		public string Anchor
		{
			get => _anchor;
			set => SetProperty(ref _anchor, value);
		}

		private string _hotlines;

		public string Hotlines	
		{
			get => _hotlines;
			set => SetProperty(ref _hotlines, value);
		}
	}
}
