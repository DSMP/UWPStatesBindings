using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;

namespace lab2Bindowanie
{
	class DataSource : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged = delegate { };

		string fName;
		string lName;
		private ApplicationDataCompositeValue composite;
		string lifeHistory;
		private ApplicationDataContainer localSettings;

		public string FName
		{
			get { return fName; }
			set
			{
				fName = value;
				StoreInLocalStorage();
				Summary = "";
			}
		}
		public string LName
		{
			get { return lName; }
			set
			{
				lName = value;
				StoreInLocalStorage();
				Summary = "";
			}
		}

		public string Summary
		{
			get { return "Witaj " + fName + " " + lName; }
			set
			{
				this.OnPropertyChanged();
			}
		}

		public void StoreInLocalStorage()
		{			
			composite["fname"] = fName;
			composite["lname"] = lName;
			composite["lifehistory"] = lifeHistory;
			localSettings.Values["exampleCompositeSetting"] = composite;
		}

		private static DataSource myInstance;
		public static ref DataSource GetInstance
		{
			get
			{
                if (myInstance == null)
                {
                    myInstance = new DataSource();
                }
                return ref myInstance;
			}
		}

		public string LifeHistory
		{
			get { return lifeHistory; }
			set
			{
				switch (value)
				{
					case "launched" : { lifeHistory += "1 "; break; }
					case "restored" : { lifeHistory += "2 "; break; }
					case "suspended": { lifeHistory += "3 "; break; }
					case "resumed": { lifeHistory += "4 "; break; }
					default:
						break;
                }
                this.OnPropertyChanged();
                StoreInLocalStorage();
			}
		}

		public ICommand ResetHistory
		{
			get
			{
				return new CommandHandler(() => this.resetHistory());
			}
		}

		private void resetHistory()
		{
			lifeHistory = "";
            this.OnPropertyChanged("LifeHistory");
        }

		public DataSource()
		{
            myInstance = this;
            localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
			composite = (Windows.Storage.ApplicationDataCompositeValue)localSettings.Values["exampleCompositeSetting"];
			if (composite == null)
			{
				composite = new Windows.Storage.ApplicationDataCompositeValue();
                fName = "Damian";
                lName = "Smug";
            }
			else
			{
				fName = (string)composite["fname"];
				lName = (string)composite["lname"];
                Summary = "";
				lifeHistory = (string)composite["lifehistory"];
			}
			
		}

		public void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			// Raise the PropertyChanged event, passing the name of the property whose value has changed.
			this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
