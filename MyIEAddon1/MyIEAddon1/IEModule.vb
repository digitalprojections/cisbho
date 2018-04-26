Imports System
Imports System.Runtime.InteropServices
Imports System.ComponentModel
Imports System.Windows.Forms
Imports IE = Interop.SHDocVw
Imports AddinExpress.IE

'Add-in Express for Internet Explorer Module
<ComVisible(True), GuidAttribute("EC929847-53DA-41A2-B974-36CABB2750A3")> _
Public Class IEModule
    Inherits AddinExpress.IE.ADXIEModule
 
#Region " Component Designer generated code. "
    'Required by designer
    Private components As System.ComponentModel.IContainer
 
    'Required by designer - do not modify
    'the following method
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        '
        'IEModule
        '
        Me.HandleShortcuts = True
        Me.LoadInMainProcess = False
        Me.ModuleName = "MyIEAddon1"
    End Sub
 
#End Region
 
#Region " Add-in Express automatic code "
 
    'Required by Add-in Express - do not modify
    'the methods within this region
 
    Public Overrides Function GetContainer() As System.ComponentModel.IContainer
        If components Is Nothing Then
            components = New System.ComponentModel.Container
        End If
        GetContainer = components
    End Function
 
    <ComRegisterFunctionAttribute()> _
    Public Shared Sub RegisterIEModule(ByVal t As Type)
        AddinExpress.IE.ADXIEModule.RegisterIEModuleInternal(t)
    End Sub
 
    <ComUnregisterFunctionAttribute()> _
    Public Shared Sub UnregisterIEModule(ByVal t As Type)
        AddinExpress.IE.ADXIEModule.UnregisterIEModuleInternal(t)
    End Sub
 
    <ComVisible(True)> _
    Public Class IECustomContextMenuCommands
        Inherits AddinExpress.IE.ADXIEModule.ADXIEContextMenuCommandDispatcher
 
    End Class
 
    <ComVisible(True)> _
    Public Class IECustomCommands
        Inherits AddinExpress.IE.ADXIEModule.ADXIECommandDispatcher
 
    End Class
 
#End Region
 
    Public Sub New()
        MyBase.New()
 
        'This call is required by the Component Designer
        InitializeComponent()
        'Please write any initialization code in the OnConnect event handler
    End Sub
 
    Public Sub New(ByVal container As IContainer)
        MyBase.New(container)
 
        container.Add(Me)
        'This call is required by the Component Designer
        InitializeComponent()
        'Please write any initialization code in the OnConnect event handler
    End Sub
 
    Public ReadOnly Property IEApp() As IE.WebBrowser
        Get
            Return CType(Me.IEObj, IE.WebBrowser)
        End Get
    End Property
 
    Public ReadOnly Property HTMLDocument() As mshtml.HTMLDocument
        Get
            Return CType(Me.HTMLDocumentObj, mshtml.HTMLDocument)
        End Get
    End Property

 
End Class

