using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.IO;
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
		public Adresa a { get; set; } = new Adresa();
		
		public MainWindow()
		{
			InitializeComponent();
			DataContext = this;
			BindingGroup = new BindingGroup();

			Baza db = new Baza();
			dgOsobe.ItemsSource = db.Osobas.ToList();

		}

		private void Unos(object sender, RoutedEventArgs e)
		{
			Baza db = new Baza();
			BindingGroup.CommitEdit();
			try
			{
				db.Osobas.Add(o);
				o = new Osoba();
				db.SaveChanges();
				dgOsobe.ItemsSource = db.Osobas.ToList();
			} catch (Exception ee)
			{
				MessageBox.Show("Proverite podatke!");
				MessageBox.Show(ee.Message);
			}
			
		}

		private void UnosAdr(object sender, RoutedEventArgs e)
		{
			if (dgOsobe.SelectedItem == null)
			{
				MessageBox.Show("Izaberite osobu!");
				return;
			}
			Baza db = new Baza();
			BindingGroup.CommitEdit();
			db.Adresas.Add(a);
			a.Osobe.Add(db.Osobas.Where(oDB => oDB.ID == o.ID).First());
			db.SaveChanges();
			o = new Osoba();
			a = new Adresa();
		}

		private void PromenaOsobe(object sender, SelectionChangedEventArgs e)
		{
			o = ((sender as DataGrid).SelectedItem as Osoba);
			Baza db = new Baza();
			dgAdrese.ItemsSource = db.Adresas.ToList();
		}
	}

	public class Baza : DbContext
	{
		public Baza() : base("Data Source=DESKTOP-75VO5EN\\TESTSERVER;Initial Catalog=EFbaza;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False") { }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Osoba>().HasKey(o => o.ID)
										.Property(o => o.Ime).IsRequired();
			
			modelBuilder.Entity<Osoba>().Property(o => o.Prezime).IsRequired();
			modelBuilder.Entity<Osoba>().HasMany(o => o.Adrese)
										.WithMany(adr=>adr.Osobe);
			modelBuilder.Entity<Adresa>().HasKey(a => a.ID)
										 .Property(a =>a.Grad).IsRequired();
			modelBuilder.Entity<Adresa>().Property(a => a.Broj).IsRequired();
			modelBuilder.Entity<Adresa>().Property(a => a.Ulica).IsRequired();
			modelBuilder.Entity<Adresa>().Property(a => a.Postanski).IsRequired();
	}

		public DbSet<Osoba> Osobas { get; set; }
		public DbSet<Adresa> Adresas { get; set; }
	}


	public class Osoba : INotifyPropertyChanged
	{
		public int ID { get; set; }
		private string ime;
		public string Ime 
		{ 
			get => ime;
			set
			{
				ime = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Ime"));
			} 
		}
		private string prezime;
		public string Prezime
		{
			get => prezime;
			set
			{
				prezime = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Prezime"));
			}
		}
	public List<Adresa> Adrese { get; set; } = new List<Adresa>();

		public Osoba(string i, string p)
		{
			Ime = i;
			Prezime = p;
		}

		public Osoba() { }

		public event PropertyChangedEventHandler PropertyChanged;
	}

	public class Adresa : INotifyPropertyChanged
	{
		public int ID { get; set; }
		private string grad;
		public string Grad
		{
			get => grad;
			set
			{
				grad = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Grad"));
			}
		}
		private string postanski;
		public string Postanski
		{
			get => postanski;
			set
			{
				postanski = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Postanski"));
			}
		}
		private string ulica;
		public string Ulica
		{
			get => ulica;
			set
			{
				ulica = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Ulica"));
			}
		}
		private string broj;
		public string Broj
		{
			get => broj;
			set
			{
				broj = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Broj"));
			}
		}
		public List<Osoba> Osobe { get; set; } = new List<Osoba>();

		public Adresa(string g, string p, string u, string b)
		{
			Grad = g;
			Postanski = p;
			Ulica = u;
			Broj = b;
		}
		public Adresa() { }

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
