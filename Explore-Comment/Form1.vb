Imports System.Net, System.Text.RegularExpressions, System.Threading, System.IO
Public Class Form1
#Region "Publics"
    Public Bool As Boolean : Public FullCookie As String
    Public SecureUri As String : Public NewToken As String
#End Region
#Region "Controls"
    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Microsoft.VisualBasic.CompilerServices.ProjectData.EndApp()
    End Sub
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CheckForIllegalCrossThreadCalls = False
    End Sub
    Private Sub AllInOneBTN_Click(sender As Object, e As EventArgs) Handles AllInOneBTN.Click
        If AllInOneBTN.Text = "Browse Comments" Then
            If CommentsBox.Items.Count = 0 Then
                Dim FileDio As OpenFileDialog = New OpenFileDialog With {
                .Filter = "Text Files|*.txt",
                .Title = "Load Comments",
                .Multiselect = False
            } : If FileDio.ShowDialog() = DialogResult.OK Then : Dim TmpList As New List(Of String)(File.ReadAllLines(FileDio.FileName)) : For Each TxtLine As String In TmpList : CommentsBox.Items.Add(TxtLine) : Next : AllInOneBTN.Text = "Login" : End If : End If
        ElseIf AllInOneBTN.Text = "Login" Then
            Dim T As Thread = New Thread(Sub()
                                             Dim LoginResp As String = Me.Login(User:=UserBox.Text, Pass:=PassBox.Text)
                                             If LoginResp.Contains("logged_in_user") Then
                                                 MsgBox($"Done Login > @{UserBox.Text} <",, "Sql-EXPLORE-C")
                                                 AllInOneBTN.Text = "Start"
                                             ElseIf LoginResp.Contains("bad_password") Then
                                                 MsgBox($"Bad Password > @{UserBox.Text} <",, "Sql-EXPLORE-C")
                                             ElseIf LoginResp.Contains("challenge") Then
                                                 SecureUri = Regex.Match(LoginResp, """challenge"": {""url"": ""(.*?)""").Groups(1).Value
                                                 MsgBox($"Secure > @{UserBox.Text} < Click Ok To Continue",, "Sql-Secure-Solve")
                                                 Dim PEResp As String = GetSecure()
                                                 Dim Phone As String = Regex.Match(PEResp, """label"":""Phone: (.*?)"",").Groups(1).Value : Dim Email As String = Regex.Match(PEResp, """label"":""Email: (.*?)"",").Groups(1).Value
                                                 Dim Choice As String = InputBox($"[1] Phone : {Phone}{vbCrLf}[2] Email : {Email}", "Enter Choice : ")
                                                 If SendCode(Choice).Contains("contact_point") Then MsgBox("Done Send Code",, "Sql-Secure-Solve") : 
                                                 Dim Code As String = InputBox("Enter Code : ", "Sql-Secure-Solve") : If SubmitCode(Code) Then If LoginToken() Then MsgBox("Secure Solved",, "Sql-Secure-Solve") : AllInOneBTN.Text = "Start" Else MsgBox("Error Login With Token",, "Sql-Secure-Solve") Else MsgBox("Invaild Code",, "Sql-Secure-Solve")
                                             Else
                                                 MsgBox(LoginResp)
                                             End If
                                         End Sub) With {
                                         .Priority = ThreadPriority.Highest,
                                         .IsBackground = False} : T.Start()
        ElseIf AllInOneBTN.Text = "Start" Then
            CommentsBox.SelectedIndex = 0 : Me.Bool = False : Dim T As Thread = New Thread(New ThreadStart(AddressOf Me.WorkComments)) With {.Priority = ThreadPriority.Lowest, .IsBackground = False} : T.Start() : AllInOneBTN.Text = "Stop"
        ElseIf AllInOneBTN.Text = "Stop" Then
            Me.Bool = True : AllInOneBTN.Text = "Start"
        End If
    End Sub
#End Region
#Region "Api"
    Private Function Login(User As String, Pass As String) As String
        Dim Resp As String = ""
        Try
            Dim uuid As String = Guid.NewGuid().ToString().ToLower()
            Dim Encoding As New Text.UTF8Encoding
            Dim _PostData As String = "{""phone_id"":""" + uuid + """,""username"":""" + User + """,""adid"":""" + uuid + """,""guid"":""" + uuid + """,""device_id"":""android-" + uuid.Split("-"c)(4) + """,""password"":""" + Pass + """,""login_attempt_count"":""1""}"
            Dim _HashedPostData As String = ""
            Dim _FinalPostData As String = "signed_body=" + Uri.EscapeDataString(_HashedPostData.ToLower() + "." + _PostData) + "&ig_sig_key_version=4"
            Dim Bytes As Byte() = Encoding.GetBytes(_FinalPostData)
            Dim AJ As HttpWebRequest = WebRequest.CreateHttp("https://i.instagram.com/api/v1/accounts/login/")
            With AJ
                .Method = "POST"
                .Headers.Add("X-IG-Capabilities: 3brTBw==")
                .Headers.Add("X-IG-App-ID: 567067343352427")
                .UserAgent = "Instagram 35.0.0.20.96 Android (25/7.1.2; 191dpi; 576x1024; samsung; SM-N975F; SM-N975F; intel; en_US; 95414346)"
                .Headers.Add("Accept-Language: en-US")
                .ContentType = "application/x-www-form-urlencoded; charset=UTF-8"
                .AutomaticDecompression = Net.DecompressionMethods.Deflate Or Net.DecompressionMethods.GZip
                .Host = "i.instagram.com"
                .Headers.Add("X-FB-HTTP-Engine: Liger")
                .KeepAlive = True
            End With
            Dim Stream As IO.Stream = AJ.GetRequestStream() : Stream.Write(Bytes, 0, Bytes.Length) : Stream.Dispose() : Stream.Close()
            Dim Reader As New IO.StreamReader(DirectCast(AJ.GetResponse(), Net.HttpWebResponse).GetResponseStream()) : Dim Text As String = Reader.ReadToEnd : Reader.Dispose() : Reader.Close()
#Region "Cookies"
            Dim CookieGrab As HttpWebResponse = DirectCast(AJ.GetResponse(), HttpWebResponse)
            Dim RegexSetting As String = CookieGrab.GetResponseHeader("Set-Cookie")
            Dim Csrftoken As String = Regex.Match(RegexSetting, "csrftoken=(.*?);").Groups(1).Value
            Dim Rur As String = Regex.Match(RegexSetting, "rur=(.*?);").Groups(1).Value
            Dim SessionId As String = Regex.Match(RegexSetting, "sessionid=(.*?);").Groups(1).Value
            Dim DsUserId As String = Regex.Match(RegexSetting, "ds_user_id=(.*?);").Groups(1).Value
            Dim DsUser As String = Regex.Match(RegexSetting, "ds_user=(.*?);").Groups(1).Value
            Me.FullCookie = $"is_starred_enabled=yes; sessionid={SessionId}; ds_user={DsUser}; ds_user_id={DsUserId}; csrftoken={Csrftoken}; igfl={DsUser}; rur={Rur};"
#End Region
            Resp = Text
        Catch ex As WebException
#Region "Cookies"
            Dim RegexSetting As String = ex.Response.Headers.Get("Set-Cookie")
            Dim Csrftoken As String = Regex.Match(RegexSetting, "csrftoken=(.*?);").Groups(1).Value
            Dim Mid As String = Regex.Match(RegexSetting, "mid=(.*?);").Groups(1).Value
            Dim Rur As String = Regex.Match(RegexSetting, "rur=(.*?);").Groups(1).Value
            Dim SessionId As String = Regex.Match(RegexSetting, "sessionid=(.*?);").Groups(1).Value
            Dim DsUserId As String = Regex.Match(RegexSetting, "ds_user_id=(.*?);").Groups(1).Value
            Dim DsUser As String = Regex.Match(RegexSetting, "ds_user=(.*?);").Groups(1).Value
            Me.FullCookie = $"is_starred_enabled=yes; sessionid={SessionId}; mid={Mid}; ds_user={DsUser}; ds_user_id={DsUserId}; csrftoken={Csrftoken}; igfl={DsUser}; rur={Rur};"
#End Region
            Resp = New IO.StreamReader(ex.Response.GetResponseStream()).ReadToEnd()
        End Try
        Return Resp
    End Function
    Private Function Comment(C As String, Id As String) As String
        Dim Resp As String = ""
        Try
            Dim Encoding As New Text.UTF8Encoding : Dim uuid As String = Guid.NewGuid().ToString().ToLower()
            Dim Bytes As Byte() = Encoding.GetBytes("signed_body=SIGNATURE.{""delivery_class"":""organic"",""idempotence_token"":""" + uuid + """,""_csrftoken"":""" + Regex.Match(FullCookie, "csrftoken=(.*?);").Groups(1).Value + """,""radio_type"":""wifi-none"",""_uid"":""" + Regex.Match(FullCookie, "ds_user_id=(.*?);").Groups(1).Value + """,""_uuid"":""" + uuid + """,""comment_text"":""" + CommentFix(C) + """,""is_carousel_bumped_post"":""false"",""container_module"":""comments_v2_photo_view_other"",""feed_position"":""0""}")
            Dim AJ As Net.HttpWebRequest = Net.WebRequest.CreateHttp("https://i.instagram.com/api/v1/media/" + Id + "/comment/")
            With AJ
                .Method = "POST"
                .Proxy = Nothing
                .Headers.Add("X-IG-Connection-Type: WIFI")
                .Headers.Add("X-IG-Capabilities: 3brTvw8=")
                .Headers.Add("X-IG-App-ID: 567067343352427")
                .UserAgent = "Instagram 155.0.0.37.107 Android (25/7.1.2; 191dpi; 1024x576; samsung; SM-N975F; SM-N975F; intel; en_US; 239490544)"
                .Headers.Add("Accept-Language: en-US")
                .Headers.Add("Cookie", FullCookie)
                .Headers.Add("X-MID", Regex.Match(FullCookie, "mid=(.*?);").Groups(1).Value)
                .ContentType = "application/x-www-form-urlencoded; charset=UTF-8"
                .AutomaticDecompression = Net.DecompressionMethods.Deflate Or Net.DecompressionMethods.GZip
                .Host = "i.instagram.com"
                .Headers.Add("X-FB-HTTP-Engine: Liger")
                .KeepAlive = True
            End With
            Dim Stream As IO.Stream = AJ.GetRequestStream() : Stream.Write(Bytes, 0, Bytes.Length) : Stream.Dispose() : Stream.Close()
            Dim Reader As New IO.StreamReader(DirectCast(AJ.GetResponse(), Net.HttpWebResponse).GetResponseStream()) : Dim Text As String = Reader.ReadToEnd : Reader.Dispose() : Reader.Close()
            Resp = Text
        Catch ex As WebException : Dim AJJ As String = New IO.StreamReader(ex.Response.GetResponseStream()).ReadToEnd()
            Resp = AJJ
        End Try
        Return Resp
    End Function
    Private Function RandomExplorePost() As String
        Dim Resp As String = ""
        Try
            Dim AJ As Net.HttpWebRequest = WebRequest.CreateHttp("https://i.instagram.com/api/v1/discover/topical_explore/?is_prefetch=false&omit_cover_media=true&module=explore_popular&reels_configuration=default&use_sectional_payload=true&timezone_offset=28800&lat=" + New Random().Next(10, 30).ToString() + "lng=" + New Random().Next(10, 150).ToString() + "&cluster_id=explore_all:0&session_id=" + Guid.NewGuid().ToString().ToLower() + "&include_fixed_destinations=true")
            With AJ
                .Method = "GET"
                .Headers.Add("X-IG-Connection-Type: WIFI")
                .Headers.Add("X-IG-Capabilities: 3brTvw8=")
                .Headers.Add("X-IG-App-ID: 567067343352427")
                .UserAgent = "Instagram 155.0.0.37.107 Android (25/7.1.2; 191dpi; 1024x576; samsung; SM-N975F; SM-N975F; intel; en_US; 239490544)"
                .Headers.Add("Accept-Language: en-US")
                .Headers.Add("Cookie", FullCookie)
                .Headers.Add("X-MID", Regex.Match(FullCookie, "mid=(.*?);").Groups(1).Value)
                .AutomaticDecompression = Net.DecompressionMethods.Deflate Or Net.DecompressionMethods.GZip
                .Host = "i.instagram.com"
                .Headers.Add("X-FB-HTTP-Engine: Liger")
                .KeepAlive = True
            End With
            Dim Response As Net.HttpWebResponse = DirectCast(AJ.GetResponse, Net.HttpWebResponse)
            Dim reader As New IO.StreamReader(Response.GetResponseStream) : Resp = reader.ReadToEnd : reader.Dispose() : reader.Close() : Response.Dispose() : Response.Close()
        Catch ex As Net.WebException : Resp = New IO.StreamReader(ex.Response.GetResponseStream()).ReadToEnd()
        End Try
        Return Resp
    End Function
#End Region
#Region "Else & Workers"
    Private Sub WorkComments()
        While Not Me.Bool
            Dim PostId As String = Regex.Match(RandomExplorePost(), """taken_at"": (.*?), ""pk"": (.*?), """).Groups(2).Value
            Dim CommentResp As String = Comment(CommentsBox.SelectedItem, PostId)
            If CommentResp.Contains("""status"": ""ok") Then
                Good.Text += 1
                Try : CommentsBox.SelectedIndex += 1 : Catch : CommentsBox.SelectedIndex = 0 : End Try
            ElseIf CommentResp.Contains("""status"": ""fail") Then
                TextBox1.Text += 1
                Try : CommentsBox.SelectedIndex += 1 : Catch : CommentsBox.SelectedIndex = 0 : End Try
            End If
            Thread.Sleep(New Random().Next(0, 6) * 1000)
        End While
    End Sub
#End Region
#Region "Secure Solve"
    Private Function GetSecure() As String
        Dim WebC As WebClient = New WebClient()
        Dim Resp As String = ""
        Dim MidT As String = Regex.Match(FullCookie, "mid=(.*?);").Groups(1).Value
        Dim CsrT As String = Regex.Match(FullCookie, "csrftoken=(.*?);").Groups(1).Value
        WebC.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-US,en;q=0.9,ar-EG;q=0.8,ar;q=0.7,bas-CM;q=0.6,bas;q=0.5")
        WebC.Headers.Add(HttpRequestHeader.Accept, "*/*")
        WebC.Headers.Add(HttpRequestHeader.ContentType, "application/x-www-form-urlencoded; charset=UTF-8")
        WebC.Headers.Add(HttpRequestHeader.UserAgent, "Instagram 155.0.0.37.107 Android (29/10; 480dpi; 1080x2043; Xiaomi/xiaomi; Mi A2 Lite; daisy_sprout; qcom; en_US; 239490544)")
        WebC.Headers.Add(HttpRequestHeader.Host, "i.instagram.com")
        WebC.Headers.Add($"Cookie: mid={MidT}; csrftoken={CsrT};")
        Try
            Resp = WebC.DownloadString(SecureUri)
        Catch ex As WebException
        End Try
        Return Resp
    End Function
    Private Function SendCode(Choice As String) As String
        Dim ChoiceIs As String = String.Empty
        If Choice = "2" Then ChoiceIs = "1"
        If Choice = "1" Then ChoiceIs = "0"
        Dim WebC As WebClient = New WebClient()
        Dim Resp As String = ""
        Dim MidT As String = Regex.Match(FullCookie, "mid=(.*?);").Groups(1).Value
        Dim CsrT As String = Regex.Match(FullCookie, "csrftoken=(.*?);").Groups(1).Value
        Dim _Data As String = "choice=" + ChoiceIs
        WebC.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-US,en;q=0.9,ar-EG;q=0.8,ar;q=0.7,bas-CM;q=0.6,bas;q=0.5")
        WebC.Headers.Add(HttpRequestHeader.Accept, "*/*")
        WebC.Headers.Add(HttpRequestHeader.ContentType, "application/x-www-form-urlencoded")
        WebC.Headers.Add(HttpRequestHeader.UserAgent, "Instagram 155.0.0.37.107 Android (29/10; 480dpi; 1080x2043; Xiaomi/xiaomi; Mi A2 Lite; daisy_sprout; qcom; en_US; 239490544)")
        WebC.Headers.Add(HttpRequestHeader.Host, "i.instagram.com")
        WebC.Headers.Add($"Cookie: mid={MidT}; csrftoken={CsrT};")
        WebC.Headers.Add("X-CSRFToken: " + CsrT)
        WebC.Headers.Add("X-Requested-With: XMLHttpRequest")
        Try
            Resp = WebC.UploadString(SecureUri, _Data)
        Catch ex As WebException
            MessageBox.Show("[+] Error ! - Send Code", "Sql-Secure-Solve")
        End Try
        Return Resp
    End Function
    Private Function SubmitCode(Code As String) As Boolean
        Dim WebC As WebClient = New WebClient()
        Dim IsSubmitted As Boolean = False
        Dim MidT As String = Regex.Match(FullCookie, "mid=(.*?);").Groups(1).Value
        Dim CsrT As String = Regex.Match(FullCookie, "csrftoken=(.*?);").Groups(1).Value
        Dim _Data As String = "security_code=" + Code
        WebC.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-US,en;q=0.9,ar-EG;q=0.8,ar;q=0.7,bas-CM;q=0.6,bas;q=0.5")
        WebC.Headers.Add(HttpRequestHeader.Accept, "*/*")
        WebC.Headers.Add(HttpRequestHeader.ContentType, "application/x-www-form-urlencoded; charset=UTF-8")
        WebC.Headers.Add(HttpRequestHeader.UserAgent, "Instagram 155.0.0.37.107 Android (29/10; 480dpi; 1080x2043; Xiaomi/xiaomi; Mi A2 Lite; daisy_sprout; qcom; en_US; 239490544)")
        WebC.Headers.Add(HttpRequestHeader.Host, "i.instagram.com")
        WebC.Headers.Add($"Cookie: mid={MidT}; csrftoken={CsrT};")
        WebC.Headers.Add("X-CSRFToken: " + CsrT)
        WebC.Headers.Add("X-Requested-With: XMLHttpRequest")
        Try
            Dim _Response As String = WebC.UploadString(SecureUri, _Data)
            Dim NewTkn As String = Regex.Match(WebC.ResponseHeaders.Get("Set-Cookie"), "csrftoken=(.*?);").Groups(1).Value
            If Not CsrT = NewTkn Then
                NewToken = Regex.Match(_Response, "csrf_token"":""(.*?)""").Groups(1).Value
                IsSubmitted = True
            End If
        Catch ex As WebException
            MessageBox.Show("[+] Error ! - Submit Code", "Sql-Secure-Solve")
        End Try
        Return IsSubmitted
    End Function
    Private Function LoginToken() As Boolean
        Dim Resp As Boolean = False
        Try
            Dim uuid As String = Guid.NewGuid().ToString().ToLower()
            Dim Encoding As New Text.UTF8Encoding
            Dim _PostData As String = "{""phone_id"":""" + uuid + """,""_csrftoken"":""" + NewToken + """,""username"":""" + UserBox.Text + """,""adid"":""" + uuid + """,""guid"":""" + uuid + """,""device_id"":""android-" + uuid.Split("-"c)(4) + """,""password"":""" + PassBox.Text + """,""login_attempt_count"":""1""}"
            Dim _HashedPostData As String = ""
            Dim _FinalPostData As String = "signed_body=" + Uri.EscapeDataString(_HashedPostData.ToLower() + "." + _PostData) + "&ig_sig_key_version=4"
            Dim Bytes As Byte() = Encoding.GetBytes(_FinalPostData)
            Dim AJ As HttpWebRequest = WebRequest.CreateHttp("https://i.instagram.com/api/v1/accounts/login/")
            With AJ
                .Method = "POST"
                .Headers.Add("X-IG-Capabilities: 3brTBw==")
                .Headers.Add("X-IG-App-ID: 567067343352427")
                .UserAgent = "Instagram 35.0.0.20.96 Android (25/7.1.2; 191dpi; 576x1024; samsung; SM-N975F; SM-N975F; intel; en_US; 95414346)"
                .Headers.Add("Accept-Language: en-US")
                .Headers.Add("Cookie", FullCookie)
                .ContentType = "application/x-www-form-urlencoded; charset=UTF-8"
                .AutomaticDecompression = Net.DecompressionMethods.Deflate Or Net.DecompressionMethods.GZip
                .Host = "i.instagram.com"
                .Headers.Add("X-FB-HTTP-Engine: Liger")
                .KeepAlive = True
            End With
            Dim Stream As IO.Stream = AJ.GetRequestStream() : Stream.Write(Bytes, 0, Bytes.Length) : Stream.Dispose() : Stream.Close()
            Dim Reader As New IO.StreamReader(DirectCast(AJ.GetResponse(), Net.HttpWebResponse).GetResponseStream()) : Dim Text As String = Reader.ReadToEnd : Reader.Dispose() : Reader.Close()
            If Text.Contains("logged_in_user") Then
                Resp = True
#Region "Cookies"
                Dim CookieGrab As HttpWebResponse = DirectCast(AJ.GetResponse(), HttpWebResponse)
                Dim RegexSetting As String = CookieGrab.GetResponseHeader("Set-Cookie")
                Dim Csrftoken As String = Regex.Match(RegexSetting, "csrftoken=(.*?);").Groups(1).Value
                Dim Rur As String = Regex.Match(RegexSetting, "rur=(.*?);").Groups(1).Value
                Dim SessionId As String = Regex.Match(RegexSetting, "sessionid=(.*?);").Groups(1).Value
                Dim DsUserId As String = Regex.Match(RegexSetting, "ds_user_id=(.*?);").Groups(1).Value
                Dim DsUser As String = Regex.Match(RegexSetting, "ds_user=(.*?);").Groups(1).Value
                Me.FullCookie = $"is_starred_enabled=yes; sessionid={SessionId}; ds_user={DsUser}; ds_user_id={DsUserId}; csrftoken={Csrftoken}; igfl={DsUser}; rur={Rur};"
#End Region
            End If
        Catch ex As WebException
            Resp = False
        End Try
        Return Resp
    End Function
#End Region
#Region "Algorithm Maker"
    Public Function CommentFix(Comment As String) As String
        Return Comment.Replace(vbCrLf, "\n")
    End Function
    Public Function HashGen(_PostString As String, _Hash As String) As String
        Dim Sha256 As System.Security.Cryptography.HMACSHA256 = New System.Security.Cryptography.HMACSHA256(System.Text.Encoding.UTF8.GetBytes(_Hash))
        Return BitConverter.ToString(Sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(_PostString))).Replace("-", "").ToLower()
    End Function
#End Region
End Class