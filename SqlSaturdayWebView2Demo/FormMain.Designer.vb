<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FormMain
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim PictureBoxLogo As PictureBox
        WebView2Main = New Microsoft.Web.WebView2.WinForms.WebView2()
        Panel1 = New Panel()
        PictureBoxLogo = New PictureBox()
        CType(PictureBoxLogo, ComponentModel.ISupportInitialize).BeginInit()
        CType(WebView2Main, ComponentModel.ISupportInitialize).BeginInit()
        Panel1.SuspendLayout()
        SuspendLayout()
        ' 
        ' PictureBoxLogo
        ' 
        PictureBoxLogo.Dock = DockStyle.Top
        PictureBoxLogo.Image = My.Resources.Resources.sql_saturday
        PictureBoxLogo.Location = New Point(0, 10)
        PictureBoxLogo.Margin = New Padding(0)
        PictureBoxLogo.Name = "PictureBoxLogo"
        PictureBoxLogo.Size = New Size(800, 150)
        PictureBoxLogo.SizeMode = PictureBoxSizeMode.Zoom
        PictureBoxLogo.TabIndex = 0
        PictureBoxLogo.TabStop = False
        ' 
        ' WebView2Main
        ' 
        WebView2Main.AllowExternalDrop = True
        WebView2Main.CreationProperties = Nothing
        WebView2Main.DefaultBackgroundColor = Color.White
        WebView2Main.Dock = DockStyle.Fill
        WebView2Main.Location = New Point(0, 10)
        WebView2Main.Margin = New Padding(0)
        WebView2Main.Name = "WebView2Main"
        WebView2Main.Size = New Size(800, 280)
        WebView2Main.TabIndex = 2
        WebView2Main.ZoomFactor = 1R
        ' 
        ' Panel1
        ' 
        Panel1.Controls.Add(WebView2Main)
        Panel1.Dock = DockStyle.Fill
        Panel1.Location = New Point(0, 160)
        Panel1.Margin = New Padding(0)
        Panel1.Name = "Panel1"
        Panel1.Padding = New Padding(0, 10, 0, 0)
        Panel1.Size = New Size(800, 290)
        Panel1.TabIndex = 3
        ' 
        ' FormMain
        ' 
        AutoScaleDimensions = New SizeF(10F, 25F)
        AutoScaleMode = AutoScaleMode.Font
        BackColor = Color.FromArgb(CByte(248), CByte(249), CByte(250))
        ClientSize = New Size(800, 450)
        Controls.Add(Panel1)
        Controls.Add(PictureBoxLogo)
        Name = "FormMain"
        Padding = New Padding(0, 10, 0, 0)
        Text = "SQL Saturday - WebView2 Demonstration"
        WindowState = FormWindowState.Maximized
        CType(PictureBoxLogo, ComponentModel.ISupportInitialize).EndInit()
        CType(WebView2Main, ComponentModel.ISupportInitialize).EndInit()
        Panel1.ResumeLayout(False)
        ResumeLayout(False)
    End Sub

    Friend WithEvents PictureBoxLogo As PictureBox
    Friend WithEvents WebView2Main As Microsoft.Web.WebView2.WinForms.WebView2
    Friend WithEvents Panel1 As Panel

End Class
