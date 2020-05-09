using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EFnestonesto
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public Osoba o { get; set; } = new Osoba();
		private string _pretraga;
		public string Pretraga 
		{ 
			get => _pretraga; 
			set
			{
				_pretraga = value;
				Baza db = new Baza();
				dg.ItemsSource = db.Osobas.Where(o => o.Ime.Contains(value) || o.Prezime.Contains(value)).ToList();
			}
		}
		
		public MainWindow()
		{
			InitializeComponent();
			DataContext = this;
			BindingGroup = new BindingGroup();

			Baza db = new Baza();
			dg.ItemsSource = db.Osobas.ToList();
		}

		private void Unos(object sender, RoutedEventArgs e)
		{
			Baza db = new Baza();
			BindingGroup.CommitEdit();
			db.Osobas.Add(o);
			o = new Osoba();
			db.SaveChanges();
			dg.ItemsSource = db.Osobas.ToList();
		}
	}

	public class Baza : DbContext
	{
		public Baza() : base("Data Source=DESKTOP-75VO5EN\\TESTSERVER;Initial Catalog=EFbaza;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False") { }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Osoba>().HasKey(o => o.ID);
		}

		public DbSet<Osoba> Osobas { get; set; }
	}

	public class Osoba
	{
		public int ID { get; set; }
		public string Ime { get; set; }
		public string Prezime { get; set; }

		public Osoba(string i, string p)
		{
			Ime = i;
			Prezime = p;
		}

		public Osoba() { }
	}
}
