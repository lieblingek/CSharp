// Decompiled with JetBrains decompiler
// Type: OffsetServer
// Assembly: GameInterface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5FC7B6-B5A2-415E-96F4-14B1CB5CCBFB
// Assembly location: C:\Users\ezsovin\Documents\The Lord of the Rings Online\pwnagebot.old2\GameInterface.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;

[DebuggerStepThrough]
[WebServiceBinding(Name = "OffsetServerSoap", Namespace = "http://offsetserver.pwnagebot.com/")]
[DesignerCategory("code")]
[GeneratedCode("wsdl", "2.0.50727.42")]
internal class OffsetServer : SoapHttpClientProtocol
{
  private SendOrPostCallback CommandOperationCompleted;

  public event CommandCompletedEventHandler CommandCompleted;

  public OffsetServer()
  {
    this.Url = "http://offsetserver.pwnagebot.com/MyServices/OffsetServer.asmx";
  }

  [SoapDocumentMethod("http://offsetserver.pwnagebot.com/Command", ParameterStyle = SoapParameterStyle.Wrapped, RequestNamespace = "http://offsetserver.pwnagebot.com/", ResponseNamespace = "http://offsetserver.pwnagebot.com/", Use = SoapBindingUse.Literal)]
  public string Command(string cmd)
  {
    return (string) this.Invoke("Command", new object[1]{ (object) cmd })[0];
  }

  public IAsyncResult BeginCommand(string cmd, AsyncCallback callback, object asyncState)
  {
    return this.BeginInvoke("Command", new object[1]{ (object) cmd }, callback, asyncState);
  }

  public string EndCommand(IAsyncResult asyncResult)
  {
    return (string) this.EndInvoke(asyncResult)[0];
  }

  public void CommandAsync(string cmd)
  {
    this.CommandAsync(cmd, (object) null);
  }

  public void CommandAsync(string cmd, object userState)
  {
    if (this.CommandOperationCompleted == null)
      this.CommandOperationCompleted = new SendOrPostCallback(this.OnCommandOperationCompleted);
    this.InvokeAsync("Command", new object[1]{ (object) cmd }, this.CommandOperationCompleted, userState);
  }

  private void OnCommandOperationCompleted(object arg)
  {
    if (this.CommandCompleted == null)
      return;
    InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
    this.CommandCompleted((object) this, new CommandCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
  }

  public new void CancelAsync(object userState)
  {
    base.CancelAsync(userState);
  }
}
