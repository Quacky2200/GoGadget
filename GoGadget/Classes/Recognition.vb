'Imports System.Speech.Recognition
'Public Class Recognitioneng
'    Inherits SpeechRecognitionEngine
'    Private Property _IsEnabled As Boolean = False
'    Public ReadOnly Property IsEnabled As Boolean
'        Get
'            Return _IsEnabled
'        End Get
'    End Property
'    Public Property Name As String
'    Private WithEvents RecognizerEngine As New SpeechRecognitionEngine
'    Private ActionMenu As New List(Of Action)
'    Private WordList As New Dictionary(Of String, Integer)
'    Sub New(Name As String, SpeechToAction As Dictionary(Of String(), Action))
'        Me.Name = Name
'        For Each Item In SpeechToAction
'            ActionMenu.Add(Item.Value)
'            Item.Key.ToList.ForEach(Sub(x As String)
'                                        WordList.Add(Name & x, ActionMenu.Count - 1)
'                                    End Sub)
'        Next
'        Dim grammerbuilder As New GrammarBuilder(New Choices(WordList.Keys.ToArray)) 'JarvisTest.Words.toStringArray)) 'Words.Keys.ToArray))
'        Dim g As New Grammar(grammerbuilder)
'        Me.LoadGrammar(g)
'        Me.SetInputToDefaultAudioDevice()
'    End Sub
'    Private Sub RecognizerEngine_RecognisedItem(sender As Object, e As SpeechRecognizedEventArgs) Handles Me.SpeechRecognized
'        ActionMenu.Item(WordList.Item(e.Result.Text)).Invoke()
'    End Sub
'    Public Sub [Start]()
'        _IsEnabled = True
'        Me.RecognizeAsync(System.Speech.Recognition.RecognizeMode.Multiple)
'    End Sub
'    Public Sub [Stop]()
'        _IsEnabled = False
'        Me.RecognizeAsyncStop()
'    End Sub
'End Class
'Public Class test2
'    Inherits Dictionary(Of String(), Action)
'    Sub New()
'        Me.Add({"hello"}, Sub()
'                              MsgBox("Hello World")
'                          End Sub)
'    End Sub
'End Class