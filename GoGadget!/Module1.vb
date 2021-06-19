Imports IWshRuntimeLibrary
Imports System.IO

Module Module1
    Sub GetIcon(ByRef Reference As String)
        If Reference.EndsWith(".lnk") Or Reference.EndsWith(".exe") Then
            Dim name As String = Mid(Reference, Reference.LastIndexOf("\") + 2, Reference.Length)
            Try
                name = name.Replace(".exe", "")
                name = name.Replace(".lnk", "")
            Catch ex As Exception
            End Try
            Dim w As New WshShell 'Used to create the shortcut
            Dim lnk As WshShortcut = w.CreateShortcut(Reference) 'Create a shortcut to get the origional file path to the icon and exe
            'MsgBox(Process.GetProcessesByName(Mid(lnk.TargetPath, lnk.TargetPath.ToString.LastIndexOf("\") + 2).Replace(".exe", "")).Length)
            Dim Icon1 As String
            If lnk.IconLocation.Contains(",") = True Then 'Icons are normally found with a comma so we get rid of it
                If lnk.TargetPath = "" Then
                    Icon1 = Reference
                Else
                    Icon1 = lnk.TargetPath
                    Try
                        Icon1 = Icon1.Replace("%ProgramFiles%", My.Computer.FileSystem.SpecialDirectories.ProgramFiles)
                        Icon1 = Icon1.Replace("%systemroot%", "C:\Windows\")
                    Catch ex As Exception
                    End Try
                End If
            Else
                Icon1 = lnk.IconLocation
                Try
                    Icon1 = Icon1.Replace("%ProgramFiles%", My.Computer.FileSystem.SpecialDirectories.ProgramFiles)
                    Icon1 = Icon1.Replace("%systemroot%", "C:\Windows")
                Catch ex As Exception
                End Try
                'Icon1 = If(lnk.IconLocation.Contains("%ProgramFiles%") = True, My.Computer.FileSystem.SpecialDirectories.ProgramFiles & lnk.IconLocation.Replace("%ProgramFiles%", ""), lnk.IconLocation)
            End If
            If frmDock.Placer.Location.Y <> frmDock.Database(1, 1, 1).Count * 47 + 3 Then
                For i = 0 To frmDock.Database(1, 1, 1).Count - 1
                    If i * 47 + 3 = frmDock.Placer.Top Then
                        frmDock.AddApp(name, Icon1, If(lnk.TargetPath = "", Reference, lnk.TargetPath), i)
                        i = frmDock.Database(1, 1, 1).Count
                    End If
                Next
            Else
                frmDock.AddApp(name, Icon1, Reference)
                'frmDock.AddApp(name, Icon1, If(lnk.TargetPath = "", Reference, lnk.TargetPath))
            End If
            'if the exe path is unclear then use the origional file.
            'If the icon location uses ,0 with the execution, use the whole path instead
        End If
    End Sub

End Module
