/*
 * Created by SharpDevelop.
 * User: aljwo
 * Date: 09-12-2013
 * Time: 13:04
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Xml.XPath;
using System.Xml;

namespace apos2_asdb_restsharp.Models
{
	/// <summary>
	/// Klasser, der bruges af RestSharp til at opløse XML
	/// </summary>

	public class ktype
	{
		public string uuid { get; set; }
		public string legend { get; set; }
	}
	
	public class kvalue
	{
		public string legend { get; set; }
	}
	
	public class suuid
	{
		public string uuid { get; set; }
	}
	
	public class puuid
	{
		public string uuid { get; set; }
	}
	
	public class euuid
	{
		public string uuid { get; set; }
	}
	
	public class kontaktkanal
	{
		public string uuid { get; set; }
		public kvalue value { get; set; }
		public ktype type { get; set; }
	}
	
	public class engagement
	{
		public string uuid { get; set; }
		public string navn { get; set; }
		public string brugervendtNoegle { get; set; }
		public suuid stillingsbetegnelse { get; set; }
		public puuid person { get; set; }
		public euuid enhed { get; set; }
		public List<kontaktkanal> kontaktkanaler { get; set; }
	}
	
	public class node
	{
		public string uuid { get; set; }
		public string type { get; set; }
		public string objectid { get; set; }
		public string overordnetid { get; set;}
		public string navn { get; set; }
		public string brugervendtNoegle { get; set; }
	}
	
	public class task
	{
		public string uuid { get; set; }
	}
	
	public class unit
	{
		public string uuid { get; set; }
	}
	
	public class person
	{
		public string uuid { get; set; }
		public string objektid { get; set; }
		public string personnummer { get; set; }
		public string fornavn { get; set; }
		public string mellemnavn { get; set; }
		public string efternavn { get; set; }
		public string adresseringsnavn { get; set; }
		public string koen { get; set; }
	}
	
	public class function
	{
		public string uuid { get; set; }
		public string navn { get; set; }
		public string bvn { get; set; }
		public string objektid { get; set; }
		public List<person> persons{ get; set; }
		public List<unit> units{ get; set; }
		public List<task> tasks{ get; set; }
	}

	public class klasse
	{
		public string uuid { get; set; }
		public string type { get; set; }
		public string objektid { get; set; }
		public string brugervendtnoegle { get; set; }
		public string title { get; set; }
		public string beskrivelse { get; set; }
	}

	public class klassifikation
	{
		public string uuid { get; set; }
		public string objektid { get; set; }
		public string brugervendtnoegle { get; set; }
		public string kaldenavn { get; set; }
	}

	public class location
	{
		public string uuid { get; set; }
		public string navn { get; set; }
		public string pnummer { get; set; }
		public string adresse { get; set; }
	}
	
	public class adresse
	{
		public string uuid { get; set; }
		public string objektid { get; set; }
		public string postnummer { get; set; }
		public string postdistrikt { get; set; }
		public string bynavn { get; set; }
		public string vejnavn { get; set; }
		public string husnummer { get; set; }
		public string vejadresseringsnavn { get; set; }
		public string nordlig { get; set; }
		public string oestlig { get; set; }
	}
	
	public class myVariables
	{
		public static XmlNamespaceManager manager { get; set; }
		public static XPathNavigator navigator { get; set; }
		public static SqlDataReader sql_dr { get; set; }
	}
	
}
