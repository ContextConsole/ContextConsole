Imports System
Imports System.Reflection
Imports System.Runtime.InteropServices

<Assembly: AssemblyCompany("ICM")> 
<Assembly: AssemblyProduct("ICM Classes")> 
<Assembly: AssemblyCopyright("Copyright (c) Ignacio Calvo")> 
<Assembly: AssemblyTrademark("")> 

<Assembly: ComVisible(False)> 
<Assembly: System.Resources.NeutralResourcesLanguage("en-US")> 

<Assembly: AssemblyVersion("1.0.0.0")> 
<Assembly: AssemblyInformationalVersion("1.0.0-alpha")>   ' a.k.a. "Product version"

#If DEBUG Then
<Assembly: AssemblyConfiguration("Debug")> 
#Else
<Assembly: AssemblyConfiguration("Release")> 
#End If