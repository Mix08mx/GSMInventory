Imports System.IO.Ports

Public Class Form1

    Dim StrBufferEntrada As String
    Dim StrBufferSalida As String

    Dim cadena As String
    Dim returnStr As String = ""
    Dim receivedData As String = ""
    Dim receivedData2 As String = ""
    Dim commandCount As Integer = 0
    Dim rcvdata As String = ""
    Dim readBuffer As String

    Dim numeroDatos As Integer = 0
    Dim valor As Double = 0

    Dim flag As Boolean = False
    Dim flag2 As Boolean = False

    Dim prod1 As Integer = 0
    Dim prod2 As Integer = 0
    Dim prod3 As Integer = 0
    Dim prod4 As Integer = 0
    Dim prod5 As Integer = 0

    Dim suma As Integer = 0


    Private Delegate Sub DelegadoAcceso(ByVal AdicionarTexto As String)

    Private Sub AccesoFormPrincipal(ByVal TextoForm As String)



        Do
            StrBufferEntrada = TextoForm
            'StrBufferEntrada = SpPuerto.ReadByte()
            If StrBufferEntrada Is "W" Then
                Exit Do
                returnStr = ""
            Else
                returnStr &= StrBufferEntrada
            End If
        Loop

        'RichTextBox1.Text = returnStr


    End Sub

    Private Sub PuertaAccesoInterrupcion(ByVal BufferIn As String)

        Dim TextoInterrupcion() As Object = {BufferIn}
        Dim DelegadoInterrupcion As DelegadoAcceso
        DelegadoInterrupcion = New DelegadoAcceso(AddressOf AccesoFormPrincipal)
        MyBase.Invoke(DelegadoInterrupcion, TextoInterrupcion)

    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        StrBufferEntrada = ""
        StrBufferSalida = ""
        BtnConectar.Enabled = False
        'RichTextBox1.Enabled = False
        'GroupBox1.Enabled = False
        'clear_BTN.Enabled = False

        TextBox2.Text = "CocaC 355 mL"
        TextBox3.Text = "Bonafont 1.5L"
        TextBox4.Text = "CocaC 600 mL"
        TextBox5.Text = "Sprite"
        TextBox6.Text = "Bonafont 1L"

        TStripEstado.Text = "Lector: Desconectado"
        TStripEstado2.Text = "Módulo GSM: Desconectado"


    End Sub

    Private Sub BtnBuscarPuertos_Click(sender As Object, e As EventArgs) Handles BtnBuscarPuertos.Click

        CboPuertos.Items.Clear()
        CboPuertos2.Items.Clear()

        For Each PuertoDisponible As String In My.Computer.Ports.SerialPortNames
            CboPuertos.Items.Add(PuertoDisponible)
            CboPuertos2.Items.Add(PuertoDisponible)
        Next

        If CboPuertos.Items.Count > 0 Then
            CboPuertos.Text = CboPuertos.Items(0)
            CboPuertos2.Text = CboPuertos2.Items(0)
            TStripEstado.Text = "Seleccionar el puerto"
            'MessageBox.Show("Seleccionar el puerto")
            BtnConectar.Enabled = True

        Else
            ' MessageBox.Show("Ningun puerto encontrado")
            TStripEstado.Text = "Ningun puerto encontrado"
            BtnConectar.Enabled = False
            CboPuertos.Items.Clear()
        End If
    End Sub

    Private Sub BtnConectar_Click(sender As Object, e As EventArgs) Handles BtnConectar.Click

        If BtnConectar.Text = "Conectar" Then
            Try

                With SpPuerto
                    .BaudRate = 9600
                    .DataBits = 8
                    .Parity = IO.Ports.Parity.None
                    .StopBits = IO.Ports.StopBits.One
                    .PortName = CboPuertos.Text
                    .ReadTimeout = 10000
                    .Open()
                End With

                BtnConectar.Text = "DESCONECTAR"
                'RichTextBox1.Enabled = True
                TStripEstado.Text = "Lector: Conectado"
                Timer1.Enabled = True
                GroupBox1.Enabled = True
                'clear_BTN.Enabled = True

            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Critical)
            End Try

        ElseIf BtnConectar.Text = "DESCONECTAR" Then
            BtnConectar.Text = "Conectar"
            'RichTextBox1.Enabled = False
            TStripEstado.Text = "Lector: Desconectado"
            SpPuerto.Close()
            Timer1.Enabled = False
        End If

    End Sub


    Private Sub BtnConectar2_Click(sender As Object, e As EventArgs) Handles BtnConectar2.Click


        If BtnConectar2.Text = "Conectar" Then
            Try

                With SpPuerto2
                    .BaudRate = 9600
                    .DataBits = 8
                    .Parity = IO.Ports.Parity.None
                    .StopBits = IO.Ports.StopBits.One
                    .PortName = CboPuertos2.Text
                    .ReadTimeout = 10000
                    .Encoding = System.Text.Encoding.Default 'GetEncoding(1252)
                    .NewLine = vbCr
                    .ReceivedBytesThreshold = 1
                    .Open()

                End With

                BtnConectar2.Text = "DESCONECTAR"
                TStripEstado2.Text = "Módulo GSM: Conectado"


                'GroupBox1.Enabled = True
                'clear_BTN.Enabled = True

                SpPuerto2.WriteLine("AT+CMGF=1")
                System.Threading.Thread.Sleep(50)
                SpPuerto2.WriteLine("AT+CNMI=1,2,0,0,0")
                System.Threading.Thread.Sleep(50)



            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Critical)
            End Try

        ElseIf BtnConectar2.Text = "DESCONECTAR" Then
            BtnConectar2.Text = "Conectar"
            TStripEstado2.Text = "Módulo GSM: Desconectado"
            'RichTextBox1.Enabled = False
            'TStripEstado.Text = "Desconectado"
            SpPuerto2.Close()
            'Timer1.Enabled = False
        End If


    End Sub

    '''//////////////BASE

    Private Sub BtnReloj_MouseUp(sender As Object, e As MouseEventArgs)
        SpPuerto.DiscardOutBuffer()
        StrBufferSalida = "X"
        SpPuerto.Write(StrBufferSalida)
    End Sub



    Private Sub LblDatosRecibidos_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub TxtDatosEnviados_TextChanged(sender As Object, e As EventArgs)

    End Sub


    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Timer1.Enabled = False


        'SpPuerto.Write(StrBufferSalida)

        ' get any new data and add the the global variable receivedData

        receivedData = ReceiveSerialData()
        'receivedData2 = ReceiveSerialData2()
        'If receivedData contains a "<" and a ">" then we have data
        If ((receivedData.Contains("1") And receivedData.Contains("8"))) Then
            parseData()
            'RichTextBox1.AppendText("Datos= " & receivedData & vbCr)
            'RichTextBox1.SelectionStart = RichTextBox1.TextLength
            'RichTextBox1.ScrollToCaret()
            'RichTextBox1.Focus()

        End If


        ' restart the timer
        Timer1.Enabled = True



    End Sub


    Function ReceiveSerialData() As String
        Dim Incoming As String
        Try
            Incoming = SpPuerto.ReadExisting()
            If Incoming Is Nothing Then
                Return "nothing" '& vbCrLf
            Else
                Return Incoming
            End If
        Catch ex As TimeoutException
            Return "Error: Serial Port read timed out."
        End Try

    End Function

    Function ReceiveSerialData2() As String

        Dim returnStr As String = ""

        Try
            Do
                Dim Incoming2 As String = SpPuerto2.ReadLine()
                If Incoming2 Is Nothing Then
                    Exit Do
                Else
                    returnStr &= Incoming2 & vbCrLf
                End If
            Loop
        Catch ex As TimeoutException
            returnStr = "Error: Serial Port read timed out."
        End Try

        Return returnStr

    End Function

    Function parseData()

        ' uses the global variable receivedData
        Dim pos1 As Integer
        Dim pos2 As Integer
        Dim length As Integer
        Dim newCommand As String
        Dim done As Boolean = False

        While (Not done)

            pos1 = receivedData.IndexOf("1") + 1
            pos2 = receivedData.IndexOf("8") + 1

            'occasionally we may not get complete data and the end marker will be in front of the start marker
            ' for exampe "55><"
            ' if pos2 < pos1 then remove the first part of the string from receivedData
            If (pos2 < pos1) Then
                receivedData = Microsoft.VisualBasic.Mid(receivedData, pos2 + 1)
                pos1 = receivedData.IndexOf("1") + 1
                pos2 = receivedData.IndexOf("8") + 1
            End If

            If (pos1 = 0 Or pos2 = 0) Then
                ' we do not have both start and end markers and we are done
                done = True

            Else
                ' we have both start and end markers

                length = pos2 - pos1 + 1
                If (length > 0) Then
                    'remove the start and end markers from the command
                    newCommand = Mid(receivedData, pos1 + 1, length - 2)

                    ' show the command in the text box
                    TextBox1.Text = " " & receivedData & vbCr

                    'remove the command from receivedData
                    receivedData = Mid(receivedData, pos2 + 1)


                    ' Producto 1
                    If (newCommand.Substring(0, 1) = "2") Then
                        prod1 = prod1 + 1
                        Label6.Text = Convert.ToString(prod1)

                        SpPuerto2.WriteLine("AT+CMGF=1")
                        System.Threading.Thread.Sleep(50)
                        SpPuerto2.WriteLine("AT+CMGS=" & Chr(34) & TextBox7.Text & Chr(34))
                        System.Threading.Thread.Sleep(50)
                        SpPuerto2.WriteLine("Producto 1 agregado, " & TextBox2.Text & ", Total: " & prod1)
                        System.Threading.Thread.Sleep(50)
                        SpPuerto2.WriteLine(Chr(26))

                        suma = suma + 1

                    End If '(newCommand.Substring(0, 1) = "2")

                    ' Producto 2
                    If (newCommand.Substring(0, 1) = "0") Then
                        prod2 = prod2 + 1
                        Label7.Text = Convert.ToString(prod2)

                        SpPuerto2.WriteLine("AT+CMGF=1")
                        System.Threading.Thread.Sleep(50)
                        SpPuerto2.WriteLine("AT+CMGS=" & Chr(34) & TextBox7.Text & Chr(34))
                        System.Threading.Thread.Sleep(50)
                        SpPuerto2.WriteLine("Producto 2 agregado, " & TextBox3.Text & ", Total: " & prod2)
                        System.Threading.Thread.Sleep(50)
                        SpPuerto2.WriteLine(Chr(26))

                        suma = suma + 1

                    End If '(newCommand.Substring(0, 1) = "0")

                    ' Producto 3
                    If (newCommand.Substring(0, 1) = "1") Then
                        prod3 = prod3 + 1
                        Label8.Text = Convert.ToString(prod3)

                        SpPuerto2.WriteLine("AT+CMGF=1")
                        System.Threading.Thread.Sleep(50)
                        SpPuerto2.WriteLine("AT+CMGS=" & Chr(34) & TextBox7.Text & Chr(34))
                        System.Threading.Thread.Sleep(50)
                        SpPuerto2.WriteLine("Producto 3 agregado, " & TextBox4.Text & ", Total: " & prod3)
                        System.Threading.Thread.Sleep(50)
                        SpPuerto2.WriteLine(Chr(26))

                        suma = suma + 1

                    End If '(newCommand.Substring(0, 1) = "1")

                    ' Producto 4
                    If (newCommand.Substring(0, 1) = "9") Then
                        prod4 = prod4 + 1
                        Label9.Text = Convert.ToString(prod4)

                        SpPuerto2.WriteLine("AT+CMGF=1")
                        System.Threading.Thread.Sleep(50)
                        SpPuerto2.WriteLine("AT+CMGS=" & Chr(34) & TextBox7.Text & Chr(34))
                        System.Threading.Thread.Sleep(50)
                        SpPuerto2.WriteLine("Producto 4 agregado, " & TextBox5.Text & ", Total: " & prod4)
                        System.Threading.Thread.Sleep(50)
                        SpPuerto2.WriteLine(Chr(26))

                        suma = suma + 1

                    End If '(newCommand.Substring(0, 1) = "9")

                    ' Producto 5
                    If (newCommand.Substring(0, 1) = "6") Then
                        prod5 = prod5 + 1
                        Label11.Text = Convert.ToString(prod5)

                        SpPuerto2.WriteLine("AT+CMGF=1")
                        System.Threading.Thread.Sleep(50)
                        SpPuerto2.WriteLine("AT+CMGS=" & Chr(34) & TextBox7.Text & Chr(34))
                        System.Threading.Thread.Sleep(50)
                        SpPuerto2.WriteLine("Producto 5 agregado, " & TextBox6.Text & ", Total: " & prod5)
                        System.Threading.Thread.Sleep(50)
                        SpPuerto2.WriteLine(Chr(26))

                        suma = suma + 1

                    End If '(newCommand.Substring(0, 1) = "6")

                    'commandCount = commandCount + 1

                    'commandCountVal_lbl.Text = commandCount

                    Label15.Text = suma


                End If ' (length > 0) 

            End If '(pos1 = 0 Or pos2 = 0)



        End While

    End Function





    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click   '' ENVIAR TODO

        enviarINV()

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click 'BORRAR TODO

        prod1 = prod2 = prod3 = prod4 = prod5 = suma = 0
        Label6.Text = "0"
        Label7.Text = "0"
        Label8.Text = "0"
        Label9.Text = "0"
        Label11.Text = "0"
        Label15.Text = "0"

        SpPuerto2.WriteLine("AT+CMGF=1")
        System.Threading.Thread.Sleep(50)
        SpPuerto2.WriteLine("AT+CMGS=" & Chr(34) & TextBox7.Text & Chr(34))
        System.Threading.Thread.Sleep(50)
        SpPuerto2.WriteLine("Inventario borrado.")
        System.Threading.Thread.Sleep(50)
        SpPuerto2.WriteLine(Chr(26))

        SpPuerto2.WriteLine("AT+CMGDA=""DEL ALL""")
        System.Threading.Thread.Sleep(50)

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click

        SpPuerto2.WriteLine(TextBox9.Text)

    End Sub


    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        RichTextBox1.Clear()
    End Sub


    Private Sub SpPuerto2_DataReceived(ByVal sender As System.Object, _
                                     ByVal e As System.IO.Ports.SerialDataReceivedEventArgs) _
                                     Handles SpPuerto2.DataReceived
        If SpPuerto2.IsOpen Then
            Try
                readBuffer = SpPuerto2.ReadLine()
                'data to UI thread 
                Me.Invoke(New EventHandler(AddressOf DoUpdate))
            Catch ex As Exception
                ' MsgBox("read " & ex.Message)
            End Try
        End If
    End Sub

    ''' <summary> 
    ''' update received string in UI 
    ''' </summary> 
    Public Sub DoUpdate(ByVal sender As Object, ByVal e As System.EventArgs)
        'TextBox8.Text = readBuffer

        RichTextBox1.AppendText(readBuffer & vbCr)
        RichTextBox1.SelectionStart = RichTextBox1.TextLength
        RichTextBox1.ScrollToCaret()
        RichTextBox1.Focus()

        If readBuffer.Contains("INVENT") Then
            enviarINV()
        ElseIf readBuffer.Contains("BORRART") Then
            prod1 = 0
            prod2 = 0
            prod3 = 0
            prod4 = 0
            prod5 = 0
            suma = 0
        ElseIf readBuffer.Contains("BORRAR1") Then
            prod1 = 0
        ElseIf readBuffer.Contains("BORRAR2") Then
            prod2 = 0
        ElseIf readBuffer.Contains("BORRAR3") Then
            prod3 = 0
        ElseIf readBuffer.Contains("BORRAR4") Then
            prod4 = 0
        ElseIf readBuffer.Contains("BORRAR5") Then
            prod5 = 0
        End If

        refreshLabels()


    End Sub


    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing

        If SpPuerto.IsOpen Then
            SpPuerto.DiscardInBuffer()
            SpPuerto.Close()
        End If

        If SpPuerto2.IsOpen Then
            SpPuerto2.DiscardInBuffer()
            SpPuerto2.Close()

        End If

    End Sub

    Function enviarINV()

        suma = prod1 + prod2 + prod3 + prod4 + prod5

        SpPuerto2.WriteLine("AT+CMGF=1")
        System.Threading.Thread.Sleep(50)
        SpPuerto2.WriteLine("AT+CMGS=" & Chr(34) & TextBox7.Text & Chr(34))
        System.Threading.Thread.Sleep(50)
        SpPuerto2.WriteLine("Producto 1 , " & TextBox2.Text & ": " & prod1)
        System.Threading.Thread.Sleep(50)
        SpPuerto2.WriteLine("Producto 2 , " & TextBox3.Text & ": " & prod2)
        System.Threading.Thread.Sleep(50)
        SpPuerto2.WriteLine("Producto 3 , " & TextBox4.Text & ": " & prod3)
        System.Threading.Thread.Sleep(50)
        SpPuerto2.WriteLine("Producto 4 , " & TextBox5.Text & ": " & prod4)
        System.Threading.Thread.Sleep(50)
        SpPuerto2.WriteLine("Producto 5 , " & TextBox6.Text & ": " & prod5)
        System.Threading.Thread.Sleep(50)
        SpPuerto2.Write("Total productos : " & suma & Chr(26))


    End Function

    Function refreshLabels()

        Label6.Text = prod1
        Label7.Text = prod2
        Label8.Text = prod3
        Label9.Text = prod4
        Label11.Text = prod5
        Label15.Text = suma


    End Function

End Class
