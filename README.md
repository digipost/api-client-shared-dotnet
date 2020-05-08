# api-client-shared-dotnet

## Hvordan utvikle på dette prosjektet?
Bruk Rider, som er C#-varianten av Intellij IDEA.

Fjern `AssemblyOriginatorKeyFile` og `SignAssembly` for å deaktivere strong-named assemblies under utvikling. 

Har du tilgang til signingkey (digipost-utviklere) kan du evt dekryptere `signingkey.snk.enc` først. 
Man kan verifisere at DLL-en er strong-named ved å benytte `sn -v <path-to-dll>`.
 

## Hvordan deploye?
Releasing er gjort via tagging med [Semver](http://semver.org) versjons schema. For en beta-release, bruk `-beta` som versjon suffix i taggen.
