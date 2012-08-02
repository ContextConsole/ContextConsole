Imports System.Linq
Imports System.Runtime.CompilerServices

Public Module TypeExtensions
		<Extension> _
		Function GetAttributes(Of T As Attribute)(ByVal type As Type, ByVal inherit As Boolean) As T()
				Return DirectCast(type.GetCustomAttributes(GetType(T), inherit), T())
		End Function
		<Extension> _
		Function HasAttribute(Of T As Attribute)(ByVal type As Type, ByVal inherit As Boolean) As Boolean
				Return DirectCast(type.GetCustomAttributes(GetType(T), inherit), T()).Count > 0
		End Function
		<Extension> _
		Function GetAttribute(Of T As Attribute)(ByVal type As Type, ByVal inherit As Boolean) As T
				Return DirectCast(type.GetCustomAttributes(GetType(T), inherit), T()).SingleOrDefault
		End Function
End Module
