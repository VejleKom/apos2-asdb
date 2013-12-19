/*
 * Created by SharpDevelop.
 * User: Allan Jul Woer, Vejle Kommune
 * Date: 17-12-2013
 * Time: 16:33
 */

using System;
using System.IO;
using System.Configuration;
using RestSharp;
using System.Collections.Generic;
using System.Xml;
using apos2_asdb.Model;
using System.Data.SqlClient;

namespace apos2_asdb
{
	class Program
	{
		public static void Main(string[] args)
		{
			// Hent indstillinger fra app.config fil
			string sql_connstr = ConfigurationManager.ConnectionStrings["apos2-asdb-sql"].ConnectionString;
			string server_name = ConfigurationManager.AppSettings["apos2-server-name"];
			string server_port = ConfigurationManager.AppSettings["apos2-server-port"];
			string environment = ConfigurationManager.AppSettings["apos2-environment"];
			string debug_to_screen = ConfigurationManager.AppSettings["debug-to-screen"];
			string debug_to_file = ConfigurationManager.AppSettings["debug-to-file"];
			string debug_File = ConfigurationManager.AppSettings["debug-file-path"];
			
			// Set variable til initielle værdier
			DateTime dto_start = DateTime.Now;
			int int_facetter = 0;
			int int_functions = 0;
			int int_klassifikationer = 0;
			int int_units = 0;
			int int_persons = 0;
			int int_locations = 0;
			int int_jobtitles = 0;
			int int_engagements = 0;
			int int_order = 0;
			int querycount = 0;
			string str_server = "http://" + server_name + ":" + server_port;
			List<string> queries = new List<string>();
			List<string> units = new List<string>();
			
			// Prøv at oprette logfil på den valgte sti. Hvis det ikke lykkes, forsøges stien oprettet.
			try
			{
				myVariables.file = new System.IO.StreamWriter(debug_File);
				myVariables.file.Close();
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
					myVariables.file = new System.IO.StreamWriter(debug_File);
					myVariables.file.Close();
				}
				catch
				{
					Console.WriteLine("Prøvede at oprette mappe og fil, men fejlede. Giver op!!");
					Console.WriteLine("Kontroller sti og rettigheder");
					Console.ReadKey();
					return;
				}
			}

			if (debug_to_file == "true")
			{
					myVariables.file = new System.IO.StreamWriter(debug_File);
					myVariables.file.WriteLine("Starttid: " + dto_start.ToString());
					myVariables.file.Close();
			}
			if (debug_to_screen == "true")
				Console.WriteLine ("Starttid: " + dto_start.ToString());
			
			// Henter facetter
			var client = new RestClient("http://" + server_name + ":" + server_port);
		    var request = new RestRequest("/apos2-app-klassifikation" + environment + "/GetKlasseForFacet?uuid=41504f53-0202-0013-4158-41504f494e54", Method.GET);
			request.AddHeader("Accept","text/xml");
			var getKlasseForFacetResponse = client.Execute<getKlasseForFacetResponse>(request);
			foreach (var klasse in getKlasseForFacetResponse.Data)
			{
				queries.Add ("INSERT INTO ASDB.dbo.facetter ([uuid], [type], [objektid], [brugervendtnoegle], [title]) VALUES ('" + klasse.uuid + "','" + klasse.type + "','" + klasse.objektid + "','" + klasse.brugervendtnoegle + "','" + klasse.title + "')");
				int_facetter++;
				querycount++;
			}
			if (debug_to_screen == "true")
				Console.WriteLine ("Modtog " + int_facetter + " poster til tabellen facetter");
			

			// Henter funktioner for typen leder
			request = new RestRequest("/apos2-app-organisation" + environment + "/GetFunctionsForSubTree?type=leder&showUnits=true", Method.GET);
			request.AddHeader("Accept","text/xml");
			var functionsResponse = client.Execute<functionsResponse>(request);
			foreach (var funktion in functionsResponse.Data)
			{
				queries.Add ("INSERT INTO ASDB.dbo.functions ([functionUuid], [objectid], [name]) VALUES ('" + funktion.uuid + "','" + null + "','" + funktion.navn + "')");
				querycount++;
				foreach (var person in funktion.persons)
				{
					queries.Add ("INSERT INTO ASDB.dbo.functionpersons ([functionUuid], [personUuid]) VALUES ('" + funktion.uuid + "','" + person.uuid + "')");
					querycount++;
				}
				foreach (var unit in funktion.units)
				{
					queries.Add ("INSERT INTO ASDB.dbo.functionunits ([functionUuid], [unitUuid]) VALUES ('" + funktion.uuid + "','" + unit.uuid + "')");
					querycount++;
				}
				foreach (var task in funktion.tasks)
				{
					queries.Add ("INSERT INTO ASDB.dbo.functiontasks ([functionUuid], [taskUuid]) VALUES ('" + funktion.uuid + "','" + task.uuid + "')");
					querycount++;
				}
				int_functions++;
			}
			if (debug_to_screen == "true")
				Console.WriteLine ("Modtog " + int_functions + " poster til tabellen functions for typen leder");
			
			// Henter klassifikationer
			request = new RestRequest("/apos2-app-klassifikation" + environment + "/GetKlassifikationList", Method.GET);
			request.AddHeader("Accept","text/xml");
			var klassifikationListResponse = client.Execute<klassifikationListResponse>(request);
			foreach (var klassifikation in klassifikationListResponse.Data)
			{
				queries.Add ("INSERT INTO ASDB.dbo.klassifikation ([uuid], [objectid], [brugervendtnoegle], [kaldenavn]) VALUES ('" + klassifikation.uuid + "','" + klassifikation.objektid + "','" + klassifikation.brugervendtnoegle + "','" + klassifikation.kaldenavn + "')");
				int_klassifikationer++;
				querycount++;
			}
			if (debug_to_screen == "true")
				Console.WriteLine ("Modtog " + int_klassifikationer + " poster til tabellen klassifikation");
			
			// Henter organisationshieraki
			request = new RestRequest("/apos2-app-organisation" + environment + "/GetEntireHierarchy", Method.GET);
			request.AddHeader("Accept","text/xml");
			var hierakiResponse = client.Execute<hierakiResponse>(request);
			foreach (var node in hierakiResponse.Data)
			{
				queries.Add ("INSERT INTO ASDB.dbo.unit ([uuid], [type], [objectid], [overordnetid], [navn], [brugervendtNoegle]) VALUES ('" + node.uuid + "','" + node.type + "','" + node.objectid + "','" + node.overordnetid + "','" + node.navn + "','" + node.brugervendtNoegle + "')");
				units.Add(node.uuid);
				int_units++;
				querycount++;
			}
			if (debug_to_screen == "true")
				Console.WriteLine ("Modtog " + int_units + " poster til tabellen unit");

			// Henter poster der relaterer sig til hver enkelt organisationsenhed.
			int_units = 1;
			foreach (var enhed in units)
			{
				Console.Write ("\rBehandler:  " + int_units++ + " af " + units.Count);
				request = new RestRequest("/apos2-composite-services" + environment + "/GetEngagement?unitUuid=" + enhed, Method.GET);
				request.AddHeader("Accept","text/xml");
				var getEngagementResponse = client.Execute<getEngagementResponse>(request);
				// Henter engagementer for hver enhed
				foreach (var engagement in getEngagementResponse.Data)
				{
					queries.Add ("INSERT INTO ASDB.dbo.engagement ([uuid], [stillingUuid], [userKey], [personUuid], [unitUuid], [locationUuid], [name]) VALUES ('" + engagement.uuid + "','" + engagement.stillingsbetegnelse.uuid + "','" + engagement.brugervendtNoegle + "','" + engagement.person.uuid + "','" + engagement.enhed.uuid + "','" + null + "','" + engagement.navn.Replace("'", "''") + "')");
					querycount++;
					int_engagements++;
					int_order = 1;
					// Henter kontaktkanaler for hvert engagement
					foreach (var kontaktkanal in engagement.kontaktkanaler)
					{
						queries.Add ("INSERT INTO ASDB.dbo.contactchannel ([uuid], [ownerUuid], [typeUuid], [value], [order_r], [usages]) VALUES ('" + kontaktkanal.uuid + "','" + engagement.uuid + "','" + kontaktkanal.type.uuid + "','" + kontaktkanal.value.legend + "','" + int_order + "','" + kontaktkanal.type.legend + "')");
						int_order++;
						querycount++;
					}
				}
				
				// Henter tilknyttede personer for hver enhed
				request = new RestRequest("/apos2-app-organisation" + environment + "/GetAttachedPersonsForUnit?uuid=" + enhed, Method.GET);
				request.AddHeader("Accept","text/xml");
				var getAttachedPersonsForUnitResponse = client.Execute<getAttachedPersonsForUnitResponse>(request);
				foreach (var person in getAttachedPersonsForUnitResponse.Data)
				{
					queries.Add ("INSERT INTO ASDB.dbo.attachedpersons ([unituuid], [personUuid]) VALUES ('" + enhed + "','" + person.uuid + "')");
					querycount++;
				}
				
				// Henter lokationer for hver enhed
				request = new RestRequest("/apos2-app-organisation" + environment + "/GetLocations?uuid=" + enhed, Method.GET);
				request.AddHeader("Accept","text/xml");
				var lokationListResponse = client.Execute<lokationListResponse>(request);
				foreach (var lokation in lokationListResponse.Data)
				{
					queries.Add ("INSERT INTO ASDB.dbo.unitlocation ([unituuid], [locationUuid]) VALUES ('" + enhed + "','" + lokation.adresse + "')");
					querycount++;
				}

				// Henter funktioner for hver enhed
				request = new RestRequest("/apos2-app-organisation" + environment + "/GetFunctionsForUnit?uuid=" + enhed, Method.GET);
				request.AddHeader("Accept","text/xml");
				functionsResponse = client.Execute<functionsResponse>(request);
				foreach (var funktion in functionsResponse.Data)
				{
					if (funktion.persons.Count == 0)
					{
						queries.Add ("INSERT INTO ASDB.dbo.engagement ([uuid], [userKey], [unitUuid], [name]) VALUES ('" + funktion.uuid + "','" + funktion.bvn + "','" + enhed + "','" + funktion.navn.Replace("'", "''") + "')");
						int_engagements++;
						querycount++;
					}
				}
			}
			if (debug_to_screen == "true")
			{
				Console.Write("\r\nModtog " + int_engagements + " poster til tabellen engagement\r\n");
				Console.Write("\r\nFærdig med behandling af units\r\n");
			}
			
			// Henter personer
			request = new RestRequest("/apos2-app-part" + environment + "/GetPersonList", Method.GET);
			request.AddHeader("Accept","text/xml");
			var personListResponse = client.Execute<personListResponse>(request);
			foreach (var person in personListResponse.Data)
			{
				queries.Add ("INSERT INTO ASDB.dbo.person ([uuid], [objektid], [userKey], [personNumber], [givenName], [surName], [addresseringsnavn], [koen]) VALUES ('" + person.uuid + "','" + person.objektid + "','" + null + "','" + person.personnummer + "','" + person.fornavn + "','" + person.mellemnavn + "','" + person.adresseringsnavn.Replace("'", "''") + "','" + person.koen + "')");
				int_persons++;
				querycount++;
			}
			if (debug_to_screen == "true")
				Console.WriteLine ("Modtog " + int_persons + " poster til tabellen person");
			
			// Henter adresser
			request = new RestRequest("/apos2-app-part" + environment + "/GetAdresseList", Method.GET);
			request.AddHeader("Accept","text/xml");
			var adresseListResponse = client.Execute<adresseListResponse>(request);
			foreach (var adresse in adresseListResponse.Data)
			{
				queries.Add ("INSERT INTO ASDB.dbo.location ([uuid], [objektid], [postnummer], [postdistrikt], [bynavn], [vejnavn], [husnummer], [name], [coordinate-lat], [coordinate-long]) VALUES ('" + adresse.uuid + "','" + adresse.objektid + "','" + adresse.postnummer + "','" + adresse.postdistrikt + "','" + adresse.bynavn + "','" + adresse.vejnavn + "','" + adresse.husnummer + "','" + adresse.vejadresseringsnavn + "','" + adresse.nordlig + "','" + adresse.oestlig + "')");
				int_locations++;
				querycount++;
			}
			if (debug_to_screen == "true")
				Console.WriteLine ("Modtog " + int_locations + " poster til tabellen location");
			
			// Henter klasser (jobtitler)
			request = new RestRequest("/apos2-app-klassifikation" + environment + "/GetKlasseList", Method.GET);
			request.AddHeader("Accept","text/xml");
			var klasseListResponse = client.Execute<klasseListResponse>(request);
			foreach (var klasse in klasseListResponse.Data)
			{
				queries.Add ("INSERT INTO ASDB.dbo.jobtitles ([uuid], [objektid], [title], [brugervendtnoegle]) VALUES ('" + klasse.uuid + "','" + klasse.objektid + "','" + klasse.title + "','" + klasse.brugervendtnoegle + "')");
				int_jobtitles++;
				querycount++;
			}
			if (debug_to_screen == "true")
				Console.WriteLine ("Modtog " + int_jobtitles + " poster til tabellen jobtitles");
			
			if (debug_to_file == "true")
			{
				myVariables.file = new System.IO.StreamWriter(debug_File, true);
				foreach (string query in queries)
				{
					myVariables.file.WriteLine(query);
				}
				myVariables.file.Close();
			}
			

			// Opret forbindelse til SQL Server
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

			//Indsæt data i tabeller
			foreach (string query in queries)
			{
				sql_query(query,sql_conn);
			}
			
			// Luk forbindelse til SQL Server
			try
			{
				sql_conn.Close();
			}
			catch (Exception ex)
			{
				Console.Write(ex.Message);
			}
			
			DateTime dto_slut = DateTime.Now;

			if (debug_to_file == "true")
			{
				myVariables.file = new System.IO.StreamWriter(debug_File, true);
				myVariables.file.WriteLine("Sluttid : " + dto_slut.ToString());
				myVariables.file.Close();
			}
			if (debug_to_screen =="true")
			{
				Console.Write("\r\nSluttid : " + dto_slut.ToString());
				Console.Write("\r\nAPOS2 servername: " + server_name + "\r\n");
				Console.Write("APOS2 serverport: " + server_port + "\r\n");
				Console.Write("Skrev " + querycount + " poster til SQL\r\n");
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