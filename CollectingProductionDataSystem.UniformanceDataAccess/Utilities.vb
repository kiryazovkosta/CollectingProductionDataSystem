Imports System.Security.Principal

Public Class Utilities
    Private Const Logon32ProviderDefault As Integer = 0
    Private Const Logon32LogonInteractive As Integer = 2
    'Private Const Logon32LogonNetwork As Integer = 3
    'Private Const Logon32LogonBatch As Integer = 4
    'Private Const Logon32LogonService As Integer = 5
    'Private Const Logon32LogonUnlock As Integer = 7
    'Private Const Logon32LogonNetworkCleartext As Integer = 8
    'Private Const Logon32LogonNewCredentials As Integer = 9

    Private Shared impersonationContext As WindowsImpersonationContext

    Declare Function LogonUserA Lib "advapi32.dll" ( _
                            ByVal lpszUsername As String, _
                            ByVal lpszDomain As String, _
                            ByVal lpszPassword As String, _
                            ByVal dwLogonType As Integer, _
                            ByVal dwLogonProvider As Integer, _
                            ByRef phToken As IntPtr) As Integer

    Declare Auto Function DuplicateToken Lib "advapi32.dll" ( _
                            ByVal existingTokenHandle As IntPtr, _
                            ByVal impersonationLevel As Integer, _
                            ByRef duplicateTokenHandle As IntPtr) As Integer
    Declare Auto Function RevertToSelf Lib "advapi32.dll" () As Long
    Declare Auto Function CloseHandle Lib "kernel32.dll" (ByVal handle As IntPtr) As Long
    '
    Public Shared Function ImpersonateValidUser(ByVal strUserName As String, _
           ByVal strDomain As String, ByVal strPassword As String) As Boolean
        Dim token As IntPtr = IntPtr.Zero
        Dim tokenDuplicate As IntPtr = IntPtr.Zero
        Dim tempWindowsIdentity As WindowsIdentity

        ImpersonateValidUser = False

        If RevertToSelf() <> 0 Then
            If LogonUserA(strUserName, strDomain, _
               strPassword, _
               Logon32LogonInteractive, _
               Logon32ProviderDefault, token) <> 0 Then
                If DuplicateToken(token, 2, tokenDuplicate) <> 0 Then
                    tempWindowsIdentity = New WindowsIdentity(tokenDuplicate)
                    impersonationContext = tempWindowsIdentity.Impersonate()

                    If Not (impersonationContext Is Nothing) Then
                        ImpersonateValidUser = True
                    End If
                End If
            End If
        End If

        If Not tokenDuplicate.Equals(IntPtr.Zero) Then
            CloseHandle(tokenDuplicate)
        End If

        If Not token.Equals(IntPtr.Zero) Then
            CloseHandle(token)
        End If

    End Function

    Public Shared Sub UndoImpersonation()

        impersonationContext.Undo()

    End Sub

    Public Overloads Shared Function GetNullString(ByVal obj As Object) As Object
        If obj Is Nothing OrElse IsDBNull(obj) Then
            Return String.Empty
        Else
            Return obj
        End If
    End Function

    Public Overloads Shared Function GetNullString(ByVal obj As Object, ByVal nullReplacement As Object) As Object
        If obj Is Nothing OrElse IsDBNull(obj) OrElse obj.ToString.Length = 0 Then
            Return nullReplacement
        Else
            Return obj
        End If
    End Function
End Class
