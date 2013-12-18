/*
 * Created by SharpDevelop.
 * User: aw
 * Date: 17-12-2013
 * Time: 16:35
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO;
using System.Net;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace apos2_asdb.Model
{
	// <summary>
	// Description of Apos2_Restsharp_Klasser.
	// </summary>
	// 
	
	//Variable
	// 	
	
	public class myVariables
	{
		public static StreamWriter file { get; set; }
		public static SqlDataReader sql_dr { get; set; }
	}
	
	public class getKlasseForFacetResponse : List<klasse> {}
	public class klasseListResponse : List<klasse> {}
	public class klasse
	{
		public string uuid { get; set; }
		public string type { get; set; }
		public string objektid { get; set; }
		public string brugervendtnoegle { get; set; }
		public string title { get; set; }
		public string beskrivelse { get; set; }
	}
	
	public class functionsResponse : List<function> {}
	public class function
	{
		public string uuid { get; set; }
		public string navn { get; set; }
		public string bvn { get; set; }
		public List<person> persons { get; set; }
		public List<unit> units { get; set; }
		public List<task> tasks { get; set; }
	}
	
	public class personListResponse : List<person> {}
	public class person
	{
		public string uuid { get; set; }
		public string objektid { get; set; }
		public string personnummer { get; set; }
		public string fornavn { get; set; }
		public string mellemnavn { get; set; }
		public string efternavn { get; set; }
		public string adresseringsnavn { get; set; }
		public string kaldenavn { get; set; }
		public string koen { get; set; }		
	}
	
	public class unit
	{
		public string uuid { get; set; }
	}
	
	public class task
	{
		public string uuid { get; set; }
	}
	
	public class klassifikationListResponse : List<klassifikation> {}
	public class klassifikation
	{
		public string uuid { get; set; }
		public string objektid { get; set; }
		public string brugervendtnoegle { get; set; }
		public string kaldenavn { get; set; }
	}
	
	public class hierakiResponse : List<node> {}
	public class node
	{
		public string uuid { get; set; }
		public string type { get; set; }
		public string objectid { get; set; }
		public string overordnetid { get; set; }
		public string navn { get; set; }
		public string brugervendtNoegle { get; set; }
	}
	
	public class getEngagementResponse : List<engagement> {}
	public class engagement
	{
		public string uuid { get; set; }
		public string navn { get; set; }
		public string brugervendtNoegle { get; set; }
		public stillingsbetegnelse stillingsbetegnelse { get; set; }
		public tilknytningsType tilknytningsType { get; set; }
		public person person { get; set; }
		public enhed enhed { get; set; }
		public List<kontaktkanal> kontaktkanaler { get; set; }
	}
	
	public class stillingsbetegnelse
	{
		public string uuid { get; set; }
	}
	
	public class tilknytningsType
	{
		public string uuid { get; set; }
	}
	
	public class enhed
	{
		public string uuid { get; set; }
	}

	public class kontaktkanal
	{
		public string uuid { get; set; }
		public value value { get; set; }
		public type type { get; set; }
	}
	
	public class value
	{
		public string legend { get; set; }
	}
	
	public class type
	{
		public string uuid { get; set; }
		public string legend { get; set; }
	}
	
	public class getAttachedPersonsForUnitResponse : List<person> {}
	
	public class lokationListResponse : List<location> {}
	public class location
	{
		public string uuid { get; set; }
		public string navn { get; set; }
		public string pnummer { get; set; }
		public string adresse { get; set; }
	}
	
	public class adresseListResponse : List<adresse> {}
	public class adresse
	{
		public string uuid { get; set; }
		public string objektid { get; set; }
		public string bvn { get; set; }
		public string postnummer { get; set; }
		public string postdistrikt { get; set; }
		public string bynavn { get; set; }
		public string vejnavn { get; set; }
		public string husnummer { get; set; }
		public string vejadresseringsnavn { get; set; }
		public string doerbetegnelse { get; set; }
		public string etage { get; set; }
		public string kommunenavn { get; set; }
		public string landekode { get; set; }
		public string lokalitet { get; set; }
		public string vejkode { get; set; }
		public string postboks { get; set; }
		public string kommunekode { get; set; }
		public string hoejde { get; set; }
		public string nordlig { get; set; }
		public string oestlig { get; set; }
		public string bydel { get; set; }
		public string politidistrikt { get; set; }
		public string skoledistrikt { get; set; }
		public string socialdistrikt { get; set; }
		public string specielvejkode { get; set; }
		public string valgkreds { get; set; }
	}
	
}
