apos2-asdb
==========

.NET program til at hente data fra APOS2 webservices til Microsoft SQL

apos2-asdb-builder udvilket til .NET 4.0
Testet med SQL server 2008

1.	Opret en database på en SQL server. Det medfølgende script til oprettelse af tabeller er baseret på en SQL
	database med navnet ASDB

2.	Opret en bruger med følgende rettigheder på databasen: db_datareader, db_datawriter, db_ddladmin, public
	Alternativt kan db_owner anvendes.
	
3.	Kør det medfølgende SQL script på databasen, for at oprette de nødvendige tabeller. Ret databasenavn
	hvis du ikke bruger navnet ASDB
	
4.	Kopier filerne (apos2-asdb*, RestSharp*) til den maskine hvor programmet skal afvikles. Filerne skal ligge 
	i samme folder. Programmet er udvilket til .NET Framework 4.0 som er en direkte afhængighed.
	
5.	Tilret apos2-asdb-builder.exe.config til det miljø programmet skal afvikles i.
	Specielt skal SQL connection string tilrettes. De øvrige kan sættes efter behov.
	
	debug til skærm får programmet til at skrive i konsolvinduet hvor langt det er, og det vil standse og
	afvente tastetryk inden det afslutter.
	
	Sæt generelt debug til "false" under almindelig drift.
	
6.	Kør nu programmet og kontroller at data kommer ind i tabellerne.

7.	Opret en "Scheduled task" til afvikling af programmet med jævne mellemrum. 1 gang i døgnet er fint, gerne om natten.


