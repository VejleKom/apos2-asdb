/*
 * Created by SharpDevelop.
 * User: aljwo
 * Date: 09-12-2013
 * Time: 08:51
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Xml;
using System.Xml.XPath;
using RestSharp;

using apos2_asdb_restsharp.Models;

namespace apos2_asdb_restsharp
{
	class Program
	{
		public static void Main(string[] args)
		{
			StreamWriter file;
			
			// Hent app.config indstillinger
			string sql_connstr = ConfigurationManager.ConnectionStrings["apos2-asdb-sql"].ConnectionString;
			string server_name = ConfigurationManager.AppSettings["apos2-server-name"];
			string server_port = ConfigurationManager.AppSettings["apos2-server-port"];
			string environment = ConfigurationManager.AppSettings["apos2-environment"];
			string debug_to_screen = ConfigurationManager.AppSettings["debug-to-screen"];
			string debug_to_file = ConfigurationManager.AppSettings["debug-to-file"];
			string debug_File = ConfigurationManager.AppSettings["debug-file-path"];

			// Set variable til startværdier
			int int_facetter = 0;
			int int_functions = 0;
			int int_klassifikationer = 0;
			int int_units = 0;
			int int_persons = 0;
			int int_locations = 0;
			int int_jobtitles = 0;
			int int_engagements = 0;
			int querycount = 0;
			int temp_count = 0;
			string str_server = "http://" + server_name + ":" + server_port;
			List<string> queries = new List<string>();
			List<string> units = new List<string>();
			
			// Se om det er muligt at oprette debug fil på valgt placering.
			try
			{
				file = new System.IO.StreamWriter(debug_File);
				file.WriteLine ( "Tidspunkt for start: " + DateTime.Now );
				file.Close();
			}
			catch (Exception ex)
			{
				Console.WriteLine("Der opstod en fejl under oprettelse af logfil: ");
				Console.WriteLine("kontroller mapper er oprettet til stien og at der er skriverettigheder til filen");
				Console.WriteLine("Placeringen er angivet i configurationsfilen til: " + debug_File);
				Console.WriteLine("Systemets fejlbesked: " + ex.Message);
				Console.WriteLine("Prøver at oprette mappe");
				string debug_dir = Path.GetDirectoryName(debug_File);
				DirectoryInfo dir_info = new DirectoryInfo(debug_dir);
				if (!dir_info.Exists)
					dir_info.Create();
				try
				{
					file = new System.IO.StreamWriter(debug_File);
					file.Close();
				}
				catch
				{
					Console.WriteLine("Prøvede at oprette mappe og fil, men fejlede. Giver op!!");
					Console.WriteLine("Kontroller sti og rettigheder");
					Console.ReadKey();
					return;
				}
			}
			
			if (debug_to_screen == "true")
				Console.WriteLine ( "Tidspunkt for start: " + DateTime.Now );
			
			// Hent facetter.
			var client = new RestClient( "http://" + server_name + ":" + server_port );
			var request = new RestRequest( "apos2-app-klassifikation" + environment + "/GetKlasseForFacet?uuid=41504f53-0202-0013-4158-41504f494e54", Method.GET );
			request.AddHeader ( "Accept", "text/xml" );
			var facetter = client.Execute<List<klasse>>(request);
			foreach ( var facet in facetter.Data )
			{
				queries.Add ("INSERT INTO ASDB.dbo.facetter ([uuid], [type], [objektid], [brugervendtnoegle], [title]) VALUES ('" + facet.uuid + "','" + facet.type + "','" + facet.objektid + "','" + facet.brugervendtnoegle + "','" + facet.title + "')");
				int_facetter++;
				querycount++;
			}
			if (debug_to_screen == "true")
				Console.WriteLine ("Modtog " + int_facetter + " poster til tabellen facetter");
			
			// Hent funktioner for typen "leder"
			client = new RestClient ( "http://" + server_name + ":" + server_port );
			request = new RestRequest( "apos2-app-organisation" + environment + "/GetFunctionsForSubTree?type=leder&showUnits=true", Method.GET );
			request.AddHeader ( "Accept", "text/xml" );
			var functions = client.Execute<List<function>>(request);
			foreach ( var function in functions.Data )
			{
				foreach ( var person in function.persons )
				{
					queries.Add ("INSERT INTO ASDB.dbo.functionpersons ([functionUuid], [personUuid]) VALUES ('" + function.uuid + "','" + person.uuid + "')");
					querycount++;
				}
				foreach ( var unit in function.units )
				{
					queries.Add ("INSERT INTO ASDB.dbo.functionunits ([functionUuid], [unitUuid]) VALUES ('" + function.uuid + "','" + unit.uuid + "')");
					querycount++;
				}
				foreach ( var task in function.tasks )
				{
					queries.Add ("INSERT INTO ASDB.dbo.functiontasks ([functionUuid], [taskUuid]) VALUES ('" + function.uuid + "','" + task.uuid + "')");
					querycount++;
				}
				queries.Add ("INSERT INTO ASDB.dbo.functions ([functionUuid], [objectid], [name]) VALUES ('" + function.uuid + "','" + function.objektid + "','" + function.navn + "')");
				querycount++;
				int_functions++;
			}
			if (debug_to_screen == "true")
				Console.WriteLine ("Modtog " + int_functions + " poster til tabellen functions for ledere");
			
			// Hent klassifikationer
			client = new RestClient ( "http://" + server_name + ":" + server_port );
			request = new RestRequest( "apos2-app-klassifikation" + environment + "/GetKlassifikationList", Method.GET );
			request.AddHeader ( "Accept", "text/xml" );
			var klassifikationer = client.Execute<List<klassifikation>>(request);
			foreach ( var klassifikation in klassifikationer.Data )
			{
				queries.Add ("INSERT INTO ASDB.dbo.klassifikation ([uuid], [objectid], [brugervendtnoegle], [kaldenavn]) VALUES ('" + klassifikation.uuid + "','" + klassifikation.objektid + "','" + klassifikation.brugervendtnoegle + "','" + klassifikation.kaldenavn + "')");
				querycount++;
				int_klassifikationer++;
			}
			if (debug_to_screen == "true")
				Console.WriteLine ("Modtog " + int_klassifikationer + " poster til tabellen klassifikation");
			
			// Hent hele organisationshierakiet
			client = new RestClient ( "http://" + server_name + ":" + server_port );
			request = new RestRequest( "apos2-app-organisation" + environment + "/GetEntireHierarchy", Method.GET );
			request.AddHeader ( "Accept", "text/xml" );
			var noder = client.Execute<List<node>>(request);
			foreach ( var node in noder.Data )
			{
				queries.Add ("INSERT INTO ASDB.dbo.unit ([uuid], [type], [objectid], [overordnetid], [navn], [brugervendtNoegle]) VALUES ('" + node.uuid + "','" + node.type + "','" + node.objectid + "','" + node.overordnetid + "','" + node.navn + "','" + node.brugervendtNoegle + "')");
				units.Add(node.uuid);
				int_units++;
				querycount++;
			}
			if (debug_to_screen == "true")
				Console.WriteLine ("Modtog " + int_units + " poster til tabellen unit");
			
			//Hent relevante data for hver organisationsenhed
			for ( temp_count = 0; temp_count < int_units; temp_count++ )
			{
				if (debug_to_screen == "true")
				{
					Console.CursorLeft = 0;
					Console.Write("Behandler: " + (temp_count + 1) + " af " + (int_units));
				}
				client = new RestClient ( "http://" + server_name + ":" + server_port );
				request = new RestRequest( "apos2-composite-services" + environment + "/GetEngagement?unitUuid=" + units[temp_count], Method.GET );
				request.AddHeader ( "Accept", "text/xml" );
				var engagementer = client.Execute<List<engagement>>(request);
				foreach ( var engagement in engagementer.Data )
				{
					string str_navn = engagement.navn.Replace("'", "''");
					queries.Add ("INSERT INTO ASDB.dbo.engagement ([uuid], [stillingUuid], [userKey], [personUuid], [unitUuid], [locationUuid], [name]) VALUES ('" + engagement.uuid + "','" + engagement.stillingsbetegnelse.uuid + "','" + engagement.brugervendtNoegle + "','" + engagement.person.uuid + "','" + engagement.enhed.uuid + "','" + null + "','" + str_navn + "')");
					querycount++;
					int_engagements++;
					int int_order = 1;
					foreach (var kontaktkanal in engagement.kontaktkanaler)
					{
						queries.Add ("INSERT INTO ASDB.dbo.contactchannel ([uuid], [ownerUuid], [typeUuid], [value], [order_r], [usages]) VALUES ('" + kontaktkanal.uuid + "','" + engagement.uuid + "','" + kontaktkanal.type.uuid + "','" + kontaktkanal.value.legend + "','" + int_order + "','" + kontaktkanal.type.legend + "')");
						querycount++;
						int_order++;
					}
				}
				client = new RestClient ( "http://" + server_name + ":" + server_port );
				request = new RestRequest( "apos2-app-organisation" + environment + "/GetAttachedPersonsForUnit?uuid=" + units[temp_count], Method.GET );
				request.AddHeader ( "Accept", "text/xml" );
				var persons = client.Execute<List<person>>(request);
				foreach ( var person in persons.Data )
				{
					queries.Add ("INSERT INTO ASDB.dbo.attachedpersons ([unituuid], [personUuid]) VALUES ('" + units[temp_count] + "','" + person.uuid + "')");
					querycount++;
				}
				client = new RestClient ( "http://" + server_name + ":" + server_port );
				request = new RestRequest( "apos2-app-organisation" + environment + "/GetLocations?uuid=" + units[temp_count], Method.GET );
				request.AddHeader ( "Accept", "text/xml" );
				var locations = client.Execute<List<location>>(request);
				foreach ( var location in locations.Data )
				{
					queries.Add ("INSERT INTO ASDB.dbo.unitlocation ([unituuid], [locationUuid]) VALUES ('" + units[temp_count] + "','" + location.uuid + "')");
					querycount++;
				}
				client = new RestClient ( "http://" + server_name + ":" + server_port );
				request = new RestRequest( "apos2-app-organisation" + environment + "/GetFunctionsForUnit?uuid=" + units[temp_count], Method.GET );
				request.AddHeader ( "Accept", "text/xml" );
				functions = client.Execute<List<function>>(request);
				foreach ( var function in functions.Data )
				{
					if (function.persons.Count == 0)
					{
						string str_navn = function.navn.Replace("'", "''");
						queries.Add ("INSERT INTO ASDB.dbo.engagement ([uuid], [userKey], [unitUuid], [name]) VALUES ('" + function.uuid + "','" + function.bvn + "','" + units[temp_count] + "','" + str_navn + "')");
						querycount++;
						int_engagements++;
					}
				}
			}
			if (debug_to_screen == "true")
			{
				Console.Write("\r\nModtog " + int_engagements + " poster til tabellen engagement\r\n");
				Console.Write("\r\nFærdig med behandling af units\r\n");
			}

			// Henter personer
			client = new RestClient ( "http://" + server_name + ":" + server_port );
			request = new RestRequest( "apos2-app-part" + environment + "/GetPersonList", Method.GET );
			request.AddHeader ( "Accept", "text/xml" );
			var personer = client.Execute<List<person>>(request);
			foreach (var person in personer.Data) 
			{
				string str_adresseringsnavn = person.adresseringsnavn.Replace("'", "''");
				queries.Add ("INSERT INTO ASDB.dbo.person ([uuid], [objektid], [userKey], [personNumber], [givenName], [surName], [addresseringsnavn], [koen]) VALUES ('" + person.uuid + "','" + person.objektid + "','" + null + "','" + person.personnummer + "','" + person.fornavn + "','" + person.mellemnavn + "','" + str_adresseringsnavn + "','" + person.koen + "')");
				querycount++;
				int_persons++;
			}
			if (debug_to_screen == "true")
				Console.WriteLine ("Modtog " + int_persons + " poster til tabellen person");

			// Henter adresser
			client = new RestClient ( "http://" + server_name + ":" + server_port );
			request = new RestRequest( "apos2-app-part" + environment + "/GetAdresseList", Method.GET );
			request.AddHeader ( "Accept", "text/xml" );
			var adresser = client.Execute<List<adresse>>(request);
			foreach ( var adresse in adresser.Data )
			{
				queries.Add ("INSERT INTO ASDB.dbo.location ([uuid], [objektid], [postnummer], [postdistrikt], [bynavn], [vejnavn], [husnummer], [name], [coordinate-lat], [coordinate-long]) VALUES ('" + adresse.uuid + "','" + adresse.objektid + "','" + adresse.postnummer + "','" + adresse.postdistrikt + "','" + adresse.bynavn + "','" + adresse.vejnavn + "','" + adresse.husnummer + "','" + adresse.vejadresseringsnavn + "','" + adresse.nordlig + "','" + adresse.oestlig + "')");
				querycount++;
				int_locations++;
			}
			if (debug_to_screen == "true")
				Console.WriteLine ("Modtog " + int_locations + " poster til tabellen location");
			
			// Henter klasser (jobtitler)
			client = new RestClient ( "http://" + server_name + ":" + server_port );
			request = new RestRequest( "apos2-app-klassifikation" + environment + "/GetKlasseList", Method.GET );
			request.AddHeader ( "Accept", "text/xml" );
			var klasser = client.Execute<List<klasse>>(request);
			foreach ( var klasse in klasser.Data )
			{
				//Console.WriteLine ( klasse.uuid + ", " + klasse.brugervendtnoegle);
				queries.Add ("INSERT INTO ASDB.dbo.jobtitles ([uuid], [objektid], [title], [brugervendtnoegle]) VALUES ('" + klasse.uuid + "','" + klasse.objektid + "','" + klasse.title + "','" + klasse.brugervendtnoegle + "')");
				querycount++;
				int_jobtitles++;
			}
			if (debug_to_screen == "true")
				Console.WriteLine ("Modtog " + int_jobtitles + " poster til tabellen jobtitles");

			if (debug_to_file == "true")
			{
				file = new System.IO.StreamWriter(debug_File, true);
				foreach (string query in queries)
				{
					file.WriteLine(query);
				}
			}
			

			// Åbn forbindelse til SQL Server
			SqlConnection sql_conn = new SqlConnection(sql_connstr);
			try
			{
				sql_conn.Open();
			}
			catch (Exception ex)
			{
				Console.Write(ex.Message);
			}
			
			// Tøm tabeller for data
			sql_query("TRUNCATE TABLE ASDB.dbo.attachedpersons",sql_conn);
			sql_query("TRUNCATE TABLE ASDB.dbo.contactchannel",sql_conn);
			sql_query("TRUNCATE TABLE ASDB.dbo.engagement",sql_conn);
			sql_query("TRUNCATE TABLE ASDB.dbo.facetter",sql_conn);
			sql_query("TRUNCATE TABLE ASDB.dbo.functionpersons",sql_conn);
			sql_query("TRUNCATE TABLE ASDB.dbo.functions",sql_conn);
			sql_query("TRUNCATE TABLE ASDB.dbo.functiontasks",sql_conn);
			sql_query("TRUNCATE TABLE ASDB.dbo.functionunits",sql_conn);
			sql_query("TRUNCATE TABLE ASDB.dbo.jobtitles",sql_conn);
			sql_query("TRUNCATE TABLE ASDB.dbo.klassifikation",sql_conn);
			sql_query("TRUNCATE TABLE ASDB.dbo.location",sql_conn);
			sql_query("TRUNCATE TABLE ASDB.dbo.person",sql_conn);
			sql_query("TRUNCATE TABLE ASDB.dbo.unit",sql_conn);
			sql_query("TRUNCATE TABLE ASDB.dbo.unitlocation",sql_conn);

			// Indsæt data i tabeller
			foreach (string query in queries)
			{
				sql_query(query,sql_conn);
			}
			
			// Luk forbindelse til SQL server
			try
			{
				sql_conn.Close();
			}
			catch (Exception ex)
			{
				Console.Write(ex.Message);
			}
			
			if (debug_to_file == "true")
			{
				file.WriteLine ( "Tidspunkt for stop: " + DateTime.Now );
				file.Close();
			}

			// Console.Write("SQL Connection String: " + sql_connstr + "\r\n");
			if (debug_to_screen =="true")
			{
				Console.WriteLine ( "Tidspunkt for stop: " + DateTime.Now );
				Console.Write("APOS2 servername: " + server_name + "\r\n");
				Console.Write("APOS2 serverport: " + server_port + "\r\n");
				Console.Write("Skrev " + querycount + " poster til SQL\r\n\r\n");
				Console.Write("Press any key to continue . . . ");
				Console.ReadKey(true);
			}
		}

		public static void sql_query(string query, SqlConnection conn)
		{
			SqlCommand sql_cmd = new SqlCommand(query,conn);

			try
			{
				myVariables.sql_dr = sql_cmd.ExecuteReader();
			}
			catch (Exception ex)
			{
				string debug_File = ConfigurationManager.AppSettings["debug-file-path"];
				StreamWriter file = new System.IO.StreamWriter(debug_File, true);
				Console.WriteLine("Error message: " + ex.Message + " SQL query: " + query);
				file.WriteLine("Error message: " + ex.Message + " SQL query: " + query);
				file.Close();
			}
			myVariables.sql_dr.Close();
		}
	}
}